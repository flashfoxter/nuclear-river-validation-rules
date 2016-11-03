﻿using System;

namespace NuClear.ValidationRules.Storage.Model.AdvertisementRules.Aggregates
{
    public sealed class Order
    {
        public long Id { get; set; }
        public string Number { get; set; }

        public DateTime BeginDistributionDate { get; set; }
        public DateTime EndDistributionDatePlan { get; set; }
        public DateTime EndDistributionDateFact { get; set; }
        public long ProjectId { get; set; }
        public long FirmId { get; set; }
        public bool RequireWhiteListAdvertisement { get; set; }
        public bool ProvideWhiteListAdvertisement { get; set; }

        public sealed class MissingAdvertisementReference
        {
            public long OrderId { get; set; }

            public long OrderPositionId { get; set; }
            public long CompositePositionId { get; set; }

            public long PositionId { get; set; }
        }

        public sealed class MissingOrderPositionAdvertisement
        {
            public long OrderId { get; set; }

            public long OrderPositionId { get; set; }
            public long CompositePositionId { get; set; }

            public long PositionId { get; set; }
        }

        public sealed class AdvertisementDeleted
        {
            public long OrderId { get; set; }

            public long OrderPositionId { get; set; }
            public long PositionId { get; set; }

            public long AdvertisementId { get; set; }
            public string AdvertisementName { get; set; }
        }

        public sealed class AdvertisementMustBelongToFirm
        {
            public long OrderId { get; set; }

            public long OrderPositionId { get; set; }
            public long PositionId { get; set; }

            public long AdvertisementId { get; set; }

            public long FirmId { get; set; }
        }

        public sealed class AdvertisementIsDummy
        {
            public long OrderId { get; set; }

            public long OrderPositionId { get; set; }

            public long PositionId { get; set; }
        }

        // todo: подумать об аггрегате "купон". ну или в фирму попробовать затолкать.
        public sealed class CouponDistributionPeriod
        {
            public long OrderId { get; set; }
            public long OrderPositionId { get; set; }
            public long PositionId { get; set; }
            public long AdvertisementId { get; set; }
            public DateTime Begin { get; set; }
            public DateTime End { get; set; }
            public long Scope { get; set; }
        }

        public sealed class OrderPositionAdvertisement
        {
            public long OrderId { get; set; }
            public long OrderPositionId { get; set; }
            public long PositionId { get; set; }
            public long AdvertisementId { get; set; }
        }
    }
}
