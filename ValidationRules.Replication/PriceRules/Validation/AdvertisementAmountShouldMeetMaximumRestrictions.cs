﻿using System.Collections.Generic;
using System.Linq;

using NuClear.Storage.API.Readings;
using NuClear.ValidationRules.Replication.Specifications;
using NuClear.ValidationRules.Storage.Identitites.EntityTypes;
using NuClear.ValidationRules.Storage.Model.Messages;
using NuClear.ValidationRules.Storage.Model.PriceRules.Aggregates;

namespace NuClear.ValidationRules.Replication.PriceRules.Validation
{
    /// <summary>
    /// Для проекта, в котором продано недостаточно рекламы в Position.Category должна выводиться ошибка.
    /// "Позиция {0} должна присутствовать в сборке в количестве от {1} до {2}. Фактическое количество позиций в месяц {6} - {3} (оформлено - {4}, содержит ошибки - {5})"
    /// В силу того, что мы (пока) не знаем, число заказов с ошибками, сократим сообщение (erm тоже так делает иногда)
    /// "Позиция {0} должна присутствовать в сборке в количестве от {1} до {2}. Фактическое количество позиций в месяц {3} - {4}"
    /// 
    /// Source: AdvertisementAmountOrderValidationRule/AdvertisementAmountErrorMessage
    /// </summary>
    public sealed class AdvertisementAmountShouldMeetMaximumRestrictions : ValidationResultAccessorBase
    {
        // todo: Выяснить востребованность этой проверки.
        // В режиме единичной реакция пользователя на неё не предусмотрена, в массовом - работает MinimumAdvertisementAmountProject.
        public AdvertisementAmountShouldMeetMaximumRestrictions(IQuery query) : base(query, MessageTypeCode.AdvertisementAmountShouldMeetMaximumRestrictions)
        {
        }

        protected override IQueryable<Version.ValidationResult> GetValidationResults(IQuery query)
        {
            var restrictionGrid =
                from period in query.For<Period>()
                from pp in query.For<Price.PricePeriod>().Where(x => x.Begin <= period.Start && period.End <= x.End)
                from restriction in query.For<Price.AdvertisementAmountRestriction>().Where(x => x.PriceId == pp.PriceId)
                select new { period.Start, period.End, pp.ProjectId, restriction.CategoryCode, restriction.Min, restriction.Max, restriction.CategoryName };

            var saleGrid =
                from orderPeriod in query.For<Order.OrderPeriod>()
                from position in query.For<Order.AmountControlledPosition>().Where(x => orderPeriod.OrderId == x.OrderId)
                select new { orderPeriod.Begin, orderPeriod.End, orderPeriod.Scope, position.OrderId, position.ProjectId, position.CategoryCode };

            var violations =
                from restriction in restrictionGrid
                from sale in saleGrid.Where(x => x.Begin <= restriction.Start && restriction.End <= x.End && x.ProjectId == restriction.ProjectId && x.CategoryCode == restriction.CategoryCode)
                let count = saleGrid.Count(x => x.CategoryCode == restriction.CategoryCode &&
                                                x.ProjectId == restriction.ProjectId &&
                                                x.Begin <= restriction.Start && restriction.End <= x.End &&
                                                Scope.CanSee(sale.Scope, x.Scope))
                select new { sale.OrderId, restriction.Start, restriction.End, restriction.Min, restriction.Max, restriction.CategoryName, Count = count };

            var messages =
                from violation in violations
                where violation.Count > violation.Max
                select new Version.ValidationResult
                    {
                        MessageParams =
                            new MessageParams(
                                    new Dictionary<string, object>
                                        {
                                            { "min", violation.Min },
                                            { "max", violation.Max },
                                            { "count", violation.Count },
                                            { "name", violation.CategoryName },
                                            { "begin", violation.Start },
                                            { "end", violation.End },
                                        },
                                    new Reference<EntityTypeOrder>(violation.OrderId))
                                .ToXDocument(),

                        PeriodStart = violation.Start,
                        PeriodEnd = violation.End,
                        OrderId = violation.OrderId,
                    };

            return messages;
        }
    }
}
