﻿using System.Linq;

using NuClear.Storage.API.Readings;
using NuClear.ValidationRules.Replication.Specifications;
using NuClear.ValidationRules.Storage.Identitites.EntityTypes;
using NuClear.ValidationRules.Storage.Model.FirmRules.Aggregates;
using NuClear.ValidationRules.Storage.Model.Messages;

using Version = NuClear.ValidationRules.Storage.Model.Messages.Version;

namespace NuClear.ValidationRules.Replication.FirmRules.Validation
{
    /// <summary>
    /// Для заказов, размещающих позиции партнёрской рекламы в карточках фирм-рекламодателей, должно выводиться предупреждение	.
    /// "Адрес {0} принадлежит фирме-рекламодателю {1} с заказом {2}"
    /// 
    /// * Не выводить это сообщение в заказе, который размещает ЗМК в карточке своей-же фирмы.
    /// </summary>
    public sealed class PartnerAdvertisementShouldNotBeSoldToAdvertiser : ValidationResultAccessorBase
    {
        public PartnerAdvertisementShouldNotBeSoldToAdvertiser(IQuery query) : base(query, MessageTypeCode.PartnerAdvertisementShouldNotBeSoldToAdvertiser)
        {
        }

        protected override IQueryable<Version.ValidationResult> GetValidationResults(IQuery query)
        {
            var messages =
                from order in query.For<Order>()
                from partnerPosition in query.For<Order.PartnerPosition>().Where(x => x.DestinationFirmId == order.FirmId)
                from partnerOrder in query.For<Order>().Where(x => x.Id == partnerPosition.OrderId).Where(x => Scope.CanSee(x.Scope, order.Scope)).Where(x => order.Begin < x.End && x.Begin < order.End)
                where partnerOrder.FirmId != partnerPosition.DestinationFirmId // о позициях в карточках своей фирмы не предупреждаем
                select new Version.ValidationResult
                {
                    MessageParams =
                            new MessageParams(
                                              new Reference<EntityTypeOrder>(partnerOrder.Id), // Заказ, размещающий ссылку
                                              new Reference<EntityTypeOrder>(order.Id), // Заказ фирмы-рекламодателя (хоста)
                                              new Reference<EntityTypeFirm>(order.FirmId), // Фирма-рекламодатель (хост)
                                              new Reference<EntityTypeFirmAddress>(partnerPosition.DestinationFirmAddressId))
                                .ToXDocument(),

                    PeriodStart = partnerOrder.Begin,
                    PeriodEnd = partnerOrder.End,
                    OrderId = partnerOrder.Id,
                };

            return messages;
        }
    }
}
