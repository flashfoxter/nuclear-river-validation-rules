﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Xml.Linq;

using NuClear.Replication.Core;
using NuClear.Replication.Core.Actors;
using NuClear.Replication.Core.DataObjects;
using NuClear.Storage.API.Readings;
using NuClear.ValidationRules.Storage.Model.Aggregates;

using Version = NuClear.ValidationRules.Storage.Model.Messages.Version;

namespace NuClear.ValidationRules.Replication.Actors
{
    public sealed class AdvertisementAmountActor : IActor
    {
        private const int MessageTypeId = 1;

        private readonly IQuery _query;
        private readonly IBulkRepository<Version.ValidationResult> _repository;

        public AdvertisementAmountActor(IQuery query, IBulkRepository<Version.ValidationResult> repository)
        {
            _query = query;
            _repository = repository;
        }

        public IReadOnlyCollection<IEvent> ExecuteCommands(IReadOnlyCollection<ICommand> commands)
        {
            var currentVersion = _query.For<Version>().OrderByDescending(x => x.Id).First().Id;

            IReadOnlyCollection<Version.ValidationResult> sourceObjects;
            using (var scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                // Запрос к данным источника посылаем вне транзакции, большой беды от этого быть не должно.
                sourceObjects = GetValidationResults(_query, currentVersion).ToArray();

                // todo: удалить, добавлено с целью отладки
                sourceObjects = sourceObjects.Where(x => x.PeriodStart >= DateTime.Parse("2016-06-01")).ToArray();

                scope.Complete();
            }

            // Данные в целевых таблицах меняем в одной большой транзакции (сейчас она управляется из хендлера)
            var targetObjects = _query.For<Version.ValidationResult>().Where(x => x.MessageType == MessageTypeId && x.VersionId == 0).ToArray();
            _repository.Delete(targetObjects);
            _repository.Create(sourceObjects);

            return Array.Empty<IEvent>();
        }

        private static IQueryable<Version.ValidationResult> GetValidationResults(IQuery query, long version)
        {
            // возможно, неправильно составл модель: ограничения привязаны к CategoryCode, а не к position
            // у CategoryCode есть имя, которое даже не хранится в erm, а вычисляется как FirstOf<Position>
            // это приводит к group by и агрегации (в erm делается то же самое, но это не оправдание)

            var restrictionGrid = from position in query.For<Position>().Where(x => x.IsControlledByAmount)
                                  join restriction in query.For<AdvertisementAmountRestriction>().Where(x => x.Min > 0 && x.Max < int.MaxValue) on position.Id equals restriction.PositionId
                                  join price in query.For<Price>() on restriction.PriceId equals price.Id
                                  join pp in query.For<PricePeriod>() on price.Id equals pp.PriceId
                                  join period in query.For<Period>() on new { pp.Start, pp.OrganizationUnitId } equals new { period.Start, period.OrganizationUnitId }
                                  group new { period.Start, period.End, period.OrganizationUnitId, position.CategoryCode, restriction.Min, restriction.Max, position.Name }
                                      by new { period.Start, period.End, period.OrganizationUnitId, position.CategoryCode } into groups
                                  select new { groups.Key, Min = groups.Min(x => x.Min), Max = groups.Max(x => x.Max) };

            var saleGrid = from position in query.For<Position>().Where(x => x.IsControlledByAmount)
                           join orderPosition in query.For<OrderPosition>() on position.Id equals orderPosition.ItemPositionId
                           join order in query.For<Order>() on orderPosition.OrderId equals order.Id
                           join op in query.For<OrderPeriod>() on order.Id equals op.OrderId
                           join period in query.For<Period>() on new { op.Start, op.OrganizationUnitId } equals new { period.Start, period.OrganizationUnitId }
                           group new { period.Start, period.End, period.OrganizationUnitId, position.CategoryCode }
                               by new { period.Start, period.End, period.OrganizationUnitId, position.CategoryCode } into groups
                           select new { groups.Key, Count = groups.Count() };

            var ruleViolations = from restriction in restrictionGrid
                                 from sale in saleGrid.Where(x => x.Key == restriction.Key).DefaultIfEmpty()
                                 where sale == null || restriction.Min > sale.Count || sale.Count > restriction.Max
                                 select new { restriction.Key, restriction.Min, restriction.Max, sale.Count };

            var orderCategories = from position in query.For<Position>().Where(x => x.IsControlledByAmount)
                                  join orderPosition in query.For<OrderPosition>() on position.Id equals orderPosition.ItemPositionId
                                  join order in query.For<Order>() on orderPosition.OrderId equals order.Id
                                  join op in query.For<OrderPeriod>() on order.Id equals op.OrderId
                                  join period in query.For<Period>() on new { op.Start, op.OrganizationUnitId } equals new { period.Start, period.OrganizationUnitId }
                                  select new { Key = new { period.Start, period.End, period.OrganizationUnitId, position.CategoryCode }, OrderId = order.Id };

            var ruleResults = from voilation in ruleViolations
                              from order in orderCategories.Where(x => x.Key == voilation.Key).DefaultIfEmpty()
                              let position = query.For<Position>().OrderBy(x => x.Name.Length).FirstOrDefault(x => x.CategoryCode == voilation.Key.CategoryCode)
                              select new Version.ValidationResult
                                  {
                                      MessageType = MessageTypeId,
                                      MessageParams =
                                          new XElement("empty",
                                                       new XAttribute("min", voilation.Min),
                                                       new XAttribute("max", voilation.Max),
                                                       new XAttribute("count", voilation.Count),
                                                       new XAttribute("name", position.Name)),
                                      OrderId = order.OrderId,
                                      PeriodStart = voilation.Key.Start,
                                      PeriodEnd = voilation.Key.End,
                                      OrganizationUnitId = voilation.Key.OrganizationUnitId,
                                      Result = 1,
                                      VersionId = version
                              };

            return ruleResults;
        }
    }
}