﻿using System;
using System.Linq;
using System.Linq.Expressions;

using NuClear.Storage.API.Readings;
using NuClear.Storage.API.Specifications;
using NuClear.ValidationRules.Storage.Model.Enums;

namespace NuClear.ValidationRules.Replication.Specifications
{
    using Erm = Storage.Model.Erm;
    using Facts = Storage.Model.Facts;

    public static partial class Specs
    {
        public static partial class Map
        {
            public static class Erm
            {
                public static class ToFacts
                {
                    private static readonly TimeSpan OneSecond = TimeSpan.FromSeconds(1);

                    public static readonly MapSpecification<IQuery, IQueryable<Facts::AssociatedPosition>> AssociatedPosition =
                        new MapSpecification<IQuery, IQueryable<Facts::AssociatedPosition>>(
                            q => q.For(Find.Erm.AssociatedPositions())
                                  .Select(Transform.AssociatedPosition));

                    public static readonly MapSpecification<IQuery, IQueryable<Facts::AssociatedPositionsGroup>> AssociatedPositionsGroup =
                        new MapSpecification<IQuery, IQueryable<Facts::AssociatedPositionsGroup>>(
                            q => q.For(Find.Erm.AssociatedPositionsGroups())
                                  .Select(Transform.AssociatedPositionsGroup));

                    public static readonly MapSpecification<IQuery, IQueryable<Facts::DeniedPosition>> DeniedPosition =
                        new MapSpecification<IQuery, IQueryable<Facts::DeniedPosition>>(
                            q => q.For(Find.Erm.DeniedPositions())
                                  .Select(Transform.DeniedPosition));

                    public static readonly MapSpecification<IQuery, IQueryable<Facts::RulesetRule>> RulesetRule =
                        new MapSpecification<IQuery, IQueryable<Facts::RulesetRule>>(
                            q => from ruleset in q.For(Find.Erm.Rulesets())
                                 join rulesetRule in q.For<Erm::RulesetRule>() on ruleset.Id equals rulesetRule.RulesetId
                                 select new Facts::RulesetRule
                                     {
                                         Id = rulesetRule.RulesetId,
                                         RuleType = rulesetRule.RuleType,
                                         DependentPositionId = rulesetRule.DependentPositionId,
                                         PrincipalPositionId = rulesetRule.PrincipalPositionId,
                                         Priority = ruleset.Priority,
                                         ObjectBindingType = rulesetRule.ObjectBindingType
                                     });

                    public static readonly MapSpecification<IQuery, IQueryable<Facts::Category>> Category =
                        new MapSpecification<IQuery, IQueryable<Facts::Category>>(
                            q => q.For(Find.Erm.Categories())
                                  .Select(Transform.Category));

                    public static readonly MapSpecification<IQuery, IQueryable<Facts::Order>> Order =
                        new MapSpecification<IQuery, IQueryable<Facts::Order>>(
                            q => q.For(Find.Erm.Orders())
                                  .Select(Transform.Order));

                    public static readonly MapSpecification<IQuery, IQueryable<Facts::OrderPosition>> OrderPosition =
                        new MapSpecification<IQuery, IQueryable<Facts::OrderPosition>>(
                            q => q.For(Find.Erm.OrderPositions())
                                  .Select(Transform.OrderPosition));

                    public static readonly MapSpecification<IQuery, IQueryable<Facts::OrderPositionAdvertisement>> OrderPositionAdvertisement =
                        new MapSpecification<IQuery, IQueryable<Facts::OrderPositionAdvertisement>>(
                            q => q.For(Find.Erm.OrderPositionAdvertisements())
                                  .Select(Transform.OrderPositionAdvertisement));

                    public static readonly MapSpecification<IQuery, IQueryable<Facts::OrganizationUnit>> OrganizationUnit =
                        new MapSpecification<IQuery, IQueryable<Facts::OrganizationUnit>>(
                            q => q.For(Find.Erm.OrganizationUnits())
                                  .Select(Transform.OrganizationUnit));

                    public static readonly MapSpecification<IQuery, IQueryable<Facts::Position>> Position =
                        new MapSpecification<IQuery, IQueryable<Facts::Position>>(
                            q => q.For(Find.Erm.Positions())
                                  .Select(Transform.Position));

                    public static readonly MapSpecification<IQuery, IQueryable<Facts::Price>> Price =
                        new MapSpecification<IQuery, IQueryable<Facts::Price>>(
                            q => q.For(Find.Erm.Prices())
                                  .Select(Transform.Price));

                    public static readonly MapSpecification<IQuery, IQueryable<Facts::PricePosition>> PricePosition =
                        new MapSpecification<IQuery, IQueryable<Facts::PricePosition>>(
                            q => q.For(Find.Erm.PricePositions())
                                  .Select(Transform.PricePosition));

                    public static readonly MapSpecification<IQuery, IQueryable<Facts::Project>> Project =
                        new MapSpecification<IQuery, IQueryable<Facts::Project>>(
                            q => q.For(Find.Erm.Projects())
                                  .Select(Transform.Project));

                    private static class Transform
                    {
                        public static readonly Expression<Func<Erm::AssociatedPosition, Facts::AssociatedPosition>> AssociatedPosition =
                            x => new Facts::AssociatedPosition
                                {
                                    Id = x.Id,
                                    AssociatedPositionsGroupId = x.AssociatedPositionsGroupId,
                                    PositionId = x.PositionId,
                                    ObjectBindingType = x.ObjectBindingType,
                                };

                        public static readonly Expression<Func<Erm::AssociatedPositionsGroup, Facts::AssociatedPositionsGroup>> AssociatedPositionsGroup =
                            x => new Facts::AssociatedPositionsGroup
                                {
                                    Id = x.Id,
                                    PricePositionId = x.PricePositionId,
                                };

                        public static readonly Expression<Func<Erm::DeniedPosition, Facts::DeniedPosition>> DeniedPosition =
                            x => new Facts::DeniedPosition
                                {
                                    Id = x.Id,
                                    PriceId = x.PriceId,
                                    PositionDeniedId = x.PositionDeniedId,
                                    PositionId = x.PositionId,
                                    ObjectBindingType = x.ObjectBindingType,
                                };

                        public static readonly Expression<Func<Erm::Category, Facts::Category>> Category =
                            x => new Facts::Category
                            {
                                Id = x.Id,
                                ParentId = x.ParentId
                            };

                        public static readonly Expression<Func<Erm::Order, Facts::Order>> Order =
                            x => new Facts::Order
                                {
                                    Id = x.Id,
                                    FirmId = x.FirmId,
                                    OwnerId = x.OwnerCode,
                                    DestOrganizationUnitId = x.DestOrganizationUnitId,
                                    SourceOrganizationUnitId = x.SourceOrganizationUnitId,
                                    WorkflowStepId = x.WorkflowStepId,
                                    BeginDistributionDate = x.BeginDistributionDate,
                                    EndDistributionDateFact = x.EndDistributionDateFact + OneSecond,
                                    BeginReleaseNumber = x.BeginReleaseNumber,
                                    EndReleaseNumberPlan = x.EndReleaseNumberPlan,
                                    EndReleaseNumberFact = x.EndReleaseNumberFact,
                                    Number = x.Number,
                                };

                        public static readonly Expression<Func<Erm::OrderPosition, Facts::OrderPosition>> OrderPosition =
                            x => new Facts::OrderPosition
                                {
                                    Id = x.Id,
                                    OrderId = x.OrderId,
                                    PricePositionId = x.PricePositionId,
                                };

                        public static readonly Expression<Func<Erm::OrderPositionAdvertisement, Facts::OrderPositionAdvertisement>> OrderPositionAdvertisement =
                            x => new Facts::OrderPositionAdvertisement
                                {
                                    Id = x.Id,
                                    CategoryId = x.CategoryId,
                                    FirmAddressId = x.FirmAddressId,
                                    PositionId = x.PositionId,
                                    OrderPositionId = x.OrderPositionId,
                                };

                        public static readonly Expression<Func<Erm::OrganizationUnit, Facts::OrganizationUnit>> OrganizationUnit =
                            x => new Facts::OrganizationUnit
                                {
                                    Id = x.Id,
                                };

                        public static readonly Expression<Func<Erm::Position, Facts::Position>> Position =
                            x => new Facts::Position
                                {
                                    Id = x.Id,
                                    Name = x.Name,
                                    IsComposite = x.IsComposite,
                                    CompareMode =
                                          x.BindingObjectTypeEnum == (int)PositionBindingObjectType.CategorySingle ||
                                          x.BindingObjectTypeEnum == (int)PositionBindingObjectType.CategoryMultiple ||
                                          x.BindingObjectTypeEnum == (int)PositionBindingObjectType.CategoryMultipleAsterix
                                          ? (int)CompareMode.Category :

                                          x.BindingObjectTypeEnum == (int)PositionBindingObjectType.AddressSingle ||
                                          x.BindingObjectTypeEnum == (int)PositionBindingObjectType.AddressMultiple
                                          ? (int)CompareMode.Address :

                                          x.BindingObjectTypeEnum == (int)PositionBindingObjectType.AddressCategorySingle ||
                                          x.BindingObjectTypeEnum == (int)PositionBindingObjectType.AddressCategoryMultiple
                                          ? (int)CompareMode.AddressCategory :

                                          x.BindingObjectTypeEnum == (int)PositionBindingObjectType.AddressFirstLevelCategorySingle ||
                                          x.BindingObjectTypeEnum == (int)PositionBindingObjectType.AddressFirstLevelCategoryMultiple
                                          ? (int)CompareMode.AddressFirstLevelCategory :

                                          (int)CompareMode.None
                                         ,
                                    IsControlledByAmount = x.IsControlledByAmount,
                                    CategoryCode = x.CategoryCode
                                };

                        public static readonly Expression<Func<Erm::Price, Facts::Price>> Price =
                            x => new Facts::Price
                                {
                                    Id = x.Id,
                                    BeginDate = x.BeginDate,
                                    OrganizationUnitId = x.OrganizationUnitId,
                                };

                        public static readonly Expression<Func<Erm::PricePosition, Facts::PricePosition>> PricePosition =
                            x => new Facts::PricePosition
                                {
                                    Id = x.Id,
                                    MaxAdvertisementAmount = x.MaxAdvertisementAmount,
                                    MinAdvertisementAmount = x.MinAdvertisementAmount,
                                    PositionId = x.PositionId,
                                    PriceId = x.PriceId,
                                };

                        public static readonly Expression<Func<Erm::Project, Facts::Project>> Project =
                            x => new Facts::Project
                                {
                                    Id = x.Id,
                                    OrganizationUnitId = x.OrganizationUnitId.Value
                                };

                        private enum PositionBindingObjectType
                        {
                            CategorySingle = 33,
                            CategoryMultiple = 34,
                            CategoryMultipleAsterix = 1,

                            AddressSingle = 6,
                            AddressMultiple = 35,

                            AddressCategorySingle = 7,
                            AddressCategoryMultiple = 8,

                            AddressFirstLevelCategorySingle = 36,
                            AddressFirstLevelCategoryMultiple = 37,
                        }
                    }
                }
            }
        }
    }
}