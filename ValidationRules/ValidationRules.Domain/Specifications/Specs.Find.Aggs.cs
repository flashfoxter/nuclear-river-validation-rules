﻿using System.Collections.Generic;
using System.Linq;

using NuClear.Storage.API.Specifications;

namespace NuClear.ValidationRules.Domain.Specifications
{
    using Aggregates = Model.Aggregates;

    public static partial class Specs
    {
        public static class Find
        {
            public static class Aggs
            {
                public static FindSpecification<Aggregates::PriceDeniedPosition> PriceDeniedPositions(IReadOnlyCollection<long> aggregateIds)
                {
                    return new FindSpecification<Aggregates::PriceDeniedPosition>(x => aggregateIds.Cast<long?>().Contains(x.PriceId));
                }
                public static FindSpecification<Aggregates::PriceAssociatedPosition> PriceAssociatedPositions(IReadOnlyCollection<long> aggregateIds)
                {
                    return new FindSpecification<Aggregates::PriceAssociatedPosition>(x => aggregateIds.Cast<long?>().Contains(x.PriceId));
                }
                public static FindSpecification<Aggregates::AdvertisementAmountRestriction> AdvertisementAmountRestrictions(IReadOnlyCollection<long> aggregateIds)
                {
                    return new FindSpecification<Aggregates::AdvertisementAmountRestriction>(x => aggregateIds.Contains(x.PriceId));
                }

                public static FindSpecification<Aggregates::RulesetDeniedPosition> RulesetDeniedPositions(IReadOnlyCollection<long> aggregateIds)
                {
                    return new FindSpecification<Aggregates::RulesetDeniedPosition>(x => aggregateIds.Cast<long?>().Contains(x.RulesetId));
                }
                public static FindSpecification<Aggregates::RulesetAssociatedPosition> RulesetAssociatedPositions(IReadOnlyCollection<long> aggregateIds)
                {
                    return new FindSpecification<Aggregates::RulesetAssociatedPosition>(x => aggregateIds.Cast<long?>().Contains(x.RulesetId));
                }

                public static FindSpecification<Aggregates::OrderPosition> OrderPositions(IReadOnlyCollection<long> aggregateIds)
                {
                    return new FindSpecification<Aggregates::OrderPosition>(x => aggregateIds.Contains(x.OrderId));
                }

                public static FindSpecification<Aggregates::OrderPrice> OrderPrices(IReadOnlyCollection<long> aggregateIds)
                {
                    return new FindSpecification<Aggregates::OrderPrice>(x => aggregateIds.Contains(x.OrderId));
                }
            }
        }
    }
}