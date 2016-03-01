﻿using System;
using System.Linq;

using NuClear.Storage.API.Readings;
using NuClear.Storage.API.Specifications;

namespace NuClear.ValidationRules.Domain.Specifications
{
    using Facts = Model.Facts;
    using Aggregates = Model.Aggregates;

    public static partial class Specs
    {
        public static partial class Map
        {
            public static partial class Facts
            {
                // ReSharper disable once InconsistentNaming
                public static class ToAggregates
                {
                    public static readonly MapSpecification<IQuery, IQueryable<Aggregates::Price>> Prices
                        = new MapSpecification<IQuery, IQueryable<Aggregates::Price>>(
                            q => q.For<Facts::Price>().Select(x => new Aggregates::Price
                                {
                                    Id = x.Id
                                }));

                    public static readonly MapSpecification<IQuery, IQueryable<Aggregates::PriceDeniedPosition>> PriceDeniedPositions
                        = new MapSpecification<IQuery, IQueryable<Aggregates::PriceDeniedPosition>>(
                            q => q.For<Facts::DeniedPosition>().Select(x => new Aggregates::PriceDeniedPosition
                            {
                                PriceId = x.PriceId,
                                DeniedPositionId = x.PositionDeniedId,
                                PrincipalPositionId = x.PositionId,
                                ObjectBindingType = x.ObjectBindingType,
                            }));

                    public static readonly MapSpecification<IQuery, IQueryable<Aggregates::PriceAssociatedPosition>> PriceAssociatedPositions
                        = new MapSpecification<IQuery, IQueryable<Aggregates::PriceAssociatedPosition>>(
                            q =>
                                {
                                    var aggs = from associatedPosition in q.For<Facts::AssociatedPosition>()
                                                join associatedPositionGroup in q.For<Facts::AssociatedPositionsGroup>() on associatedPosition.AssociatedPositionsGroupId equals associatedPositionGroup.Id
                                                join pricePosition in q.For<Facts::PricePosition>() on associatedPositionGroup.PricePositionId equals pricePosition.Id
                                                join price in q.For<Facts::Price>() on pricePosition.PriceId equals price.Id
                                                select new Aggregates::PriceAssociatedPosition
                                                {
                                                    PriceId = price.Id,
                                                    AssociatedPositionId = pricePosition.PositionId,
                                                    PrincipalPositionId = associatedPosition.PositionId,
                                                    ObjectBindingType = associatedPosition.ObjectBindingType,
                                                    GroupId = associatedPositionGroup.Id
                                                };
                                    return aggs;
                                });

                    public static readonly MapSpecification<IQuery, IQueryable<Aggregates::AdvertisementAmountRestriction>> AdvertisementAmountRestrictions
                        = new MapSpecification<IQuery, IQueryable<Aggregates::AdvertisementAmountRestriction>>(
                            q => q.For<Facts::PricePosition>().Select(x => new Aggregates::AdvertisementAmountRestriction
                                {
                                    PriceId = x.PriceId,
                                    PositionId = x.PositionId,
                                    Max = x.MaxAdvertisementAmount,
                                    Min = x.MinAdvertisementAmount
                                }));

                    public static readonly MapSpecification<IQuery, IQueryable<Aggregates::Ruleset>> Rulesets
                        = new MapSpecification<IQuery, IQueryable<Aggregates::Ruleset>>(
                            q =>
                            {
                                var associated = q.For<Facts::GlobalAssociatedPosition>().Select(x => x.RulesetId);
                                var denied = q.For<Facts::GlobalDeniedPosition>().Select(x => x.RulesetId);

                                return associated.Union(denied).Select(x => new Aggregates::Ruleset
                                {
                                    Id = x
                                });
                            });

                    public static readonly MapSpecification<IQuery, IQueryable<Aggregates::RulesetDeniedPosition>> RulesetDeniedPositions
                        = new MapSpecification<IQuery, IQueryable<Aggregates::RulesetDeniedPosition>>(
                            q => q.For<Facts::GlobalDeniedPosition>().Select(x => new Aggregates::RulesetDeniedPosition
                            {
                                RulesetId = x.RulesetId,
                                DeniedPositionId = x.DeniedPositionId,
                                PrincipalPositionId = x.PrincipalPositionId,
                                ObjectBindingType = x.ObjectBindingType,
                            }));

                    public static readonly MapSpecification<IQuery, IQueryable<Aggregates::RulesetAssociatedPosition>> RulesetAssociatedPositions
                        = new MapSpecification<IQuery, IQueryable<Aggregates::RulesetAssociatedPosition>>(
                            q => q.For<Facts::GlobalAssociatedPosition>().Select(x => new Aggregates::RulesetAssociatedPosition
                            {
                                RulesetId = x.RulesetId,
                                AssociatedPositionId = x.AssociatedPositionId,
                                PrincipalPositionId = x.PrincipalPositionId,
                                ObjectBindingType = x.ObjectBindingType,
                            }));

                    public static readonly MapSpecification<IQuery, IQueryable<Aggregates::Order>> Orders
                        = new MapSpecification<IQuery, IQueryable<Aggregates::Order>>(
                            q => q.For<Facts::Order>().Select(x => new Aggregates::Order
                                {
                                    Id = x.Id,
                                    FirmId = x.FirmId,
                                }));

                    public static readonly MapSpecification<IQuery, IQueryable<Aggregates::OrderPosition>> OrderPositions
                        = new MapSpecification<IQuery, IQueryable<Aggregates::OrderPosition>>(
                            q =>
                                {
                                    var opas = from opa in q.For<Facts::OrderPositionAdvertisement>()
                                               join orderPosition in q.For<Facts::OrderPosition>() on opa.OrderPositionId equals orderPosition.Id
                                               select new Aggregates::OrderPosition
                                                {
                                                    OrderId = orderPosition.OrderId,
                                                    ItemPositionId = opa.PositionId,
                                                    CompareMode = (from position in q.For<Facts::Position>()
                                                                   where position.Id == opa.PositionId
                                                                   select position.CompareMode
                                                                   ).FirstOrDefault(),
                                                    Category3Id = opa.CategoryId,
                                                    FirmAddressId = opa.FirmAddressId,

                                                    PackagePositionId = (from pricePosition in q.For<Facts::PricePosition>()
                                                                        where pricePosition.Id == orderPosition.PricePositionId
                                                                        select pricePosition.PositionId
                                                                        ).FirstOrDefault(),
                                                    Category1Id = (from c3 in q.For<Facts::Category>()
                                                                    where c3.Id == opa.CategoryId
                                                                    join c2 in q.For<Facts::Category>() on c3.ParentId equals c2.Id
                                                                    join c1 in q.For<Facts::Category>() on c2.ParentId equals c1.Id
                                                                    select c1.Id
                                                                    ).FirstOrDefault()
                                                };

                                    var pkgs = from orderPosition in q.For<Facts::OrderPosition>()
                                               join pricePosition in q.For<Facts::PricePosition>() on orderPosition.PricePositionId equals pricePosition.Id
                                               join position in q.For<Facts::Position>() on pricePosition.PositionId equals position.Id
                                               where position.IsComposite
                                               select new Aggregates::OrderPosition
                                                {
                                                    OrderId = orderPosition.OrderId,
                                                    ItemPositionId = pricePosition.PositionId,
                                                    CompareMode = position.CompareMode,
                                                    Category3Id = null,
                                                    FirmAddressId = null,

                                                    PackagePositionId = pricePosition.PositionId,
                                                    Category1Id = null
                                                };

                                    return opas.Union(pkgs);
                                });

                    public static readonly MapSpecification<IQuery, IQueryable<Aggregates::OrderPrice>> OrderPrices
                        = new MapSpecification<IQuery, IQueryable<Aggregates::OrderPrice>>(
                            q =>
                                {
                                    var orderPrices = from order in q.For<Facts::Order>()
                                                      join orderPosition in q.For<Facts::OrderPosition>() on order.Id equals orderPosition.OrderId
                                                      join pricePosition in q.For<Facts::PricePosition>() on orderPosition.PricePositionId equals pricePosition.Id
                                                      select new Aggregates::OrderPrice
                                                          {
                                                              OrderId = order.Id,
                                                              PriceId = pricePosition.PriceId
                                                          };

                                    return orderPrices.Distinct();
                                });

                    public static readonly MapSpecification<IQuery, IQueryable<Aggregates::Position>> Positions
                        = new MapSpecification<IQuery, IQueryable<Aggregates::Position>>(
                            q => q.For<Facts::Position>().Select(x => new Aggregates::Position
                                {
                                    Id = x.Id,
                                    PositionCategoryId = x.PositionCategoryId
                                }));

                    public static readonly MapSpecification<IQuery, IQueryable<Aggregates::Period>> Periods
                        = new MapSpecification<IQuery, IQueryable<Aggregates::Period>>(
                            q =>
                                {
                                    var dates = q.For<Facts::Order>()
                                                 .Select(x => new { Date = x.BeginDistributionDate, OrganizationUnitId = x.DestOrganizationUnitId })
                                                 .Union(q.For<Facts::Order>().Select(x => new { Date = x.EndDistributionDateFact, OrganizationUnitId = x.DestOrganizationUnitId }))
                                                 .Union(q.For<Facts::Price>().Select(x => new { Date = x.BeginDate, OrganizationUnitId = x.OrganizationUnitId }))
                                                 .Select(x => new { x.Date, x.OrganizationUnitId })
                                                 .OrderBy(x => x.Date)
                                                 .Distinct();

                                    return dates.Select(x => new { start = x, end = dates.FirstOrDefault(y => y.Date > x.Date && y.OrganizationUnitId == x.OrganizationUnitId) })
                                                .Select(x => new Aggregates::Period
                                                    {
                                                        Start = x.start.Date,
                                                        End = x.end != null ? x.end.Date : DateTime.MaxValue,
                                                        OrganizationUnitId = x.start.OrganizationUnitId
                                                    });
                                });
                }
            }
        }
    }
}