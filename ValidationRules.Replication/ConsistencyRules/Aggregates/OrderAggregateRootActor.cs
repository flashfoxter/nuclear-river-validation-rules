﻿using System.Collections.Generic;
using System.Linq;

using NuClear.Replication.Core;
using NuClear.Replication.Core.DataObjects;
using NuClear.Replication.Core.Equality;
using NuClear.Storage.API.Readings;
using NuClear.Storage.API.Specifications;
using NuClear.ValidationRules.Replication.Commands;
using NuClear.ValidationRules.Storage.Model.ConsistencyRules.Aggregates;
using NuClear.ValidationRules.Storage.Model.Messages;

using Facts = NuClear.ValidationRules.Storage.Model.ConsistencyRules.Facts;

namespace NuClear.ValidationRules.Replication.ConsistencyRules.Aggregates
{
    public sealed class OrderAggregateRootActor : AggregateRootActor<Order>
    {
        public OrderAggregateRootActor(
            IQuery query,
            IEqualityComparerFactory equalityComparerFactory,
            IBulkRepository<Order> bulkRepository,
            IBulkRepository<Order.InvalidFirm> orderInvalidFirmRepository,
            IBulkRepository<Order.InvalidFirmAddress> orderInvalidFirmAddressRepository,
            IBulkRepository<Order.BargainSignedLaterThanOrder> orderBargainSignedLaterThanOrderRepository,
            IBulkRepository<Order.HasNoAnyLegalPersonProfile> orderHasNoAnyLegalPersonProfileRepository,
            IBulkRepository<Order.HasNoAnyPosition> orderHasNoAnyPositionRepository,
            IBulkRepository<Order.InactiveReference> inactiveReferenceRepository,
            IBulkRepository<Order.InvalidBeginDistributionDate> orderInvalidBeginDistributionDateRepository,
            IBulkRepository<Order.InvalidEndDistributionDate> orderInvalidEndDistributionDateRepository,
            IBulkRepository<Order.InvalidBillsPeriod> orderInvalidBillsPeriodRepository,
            IBulkRepository<Order.InvalidBillsTotal> orderInvalidBillsTotalRepository,
            IBulkRepository<Order.LegalPersonProfileBargainExpired> orderLegalPersonProfileBargainEndDateIsEarlierThanOrderSignupDateRepository,
            IBulkRepository<Order.LegalPersonProfileWarrantyExpired> orderLegalPersonProfileWarrantyEndDateIsEarlierThanOrderSignupDateRepository,
            IBulkRepository<Order.MissingBargainScan> orderMissingBargainScanRepository,
            IBulkRepository<Order.MissingBills> orderMissingBillsRepository,
            IBulkRepository<Order.MissingRequiredField> orderMissingRequiredFieldRepository,
            IBulkRepository<Order.MissingOrderScan> orderMissingOrderScanRepository)
            : base(query, equalityComparerFactory)
        {
            HasRootEntity(new OrderAccessor(query), bulkRepository,
                HasValueObject(new InvalidFirmAccessor(query), orderInvalidFirmRepository),
                HasValueObject(new InvalidFirmAddressAccessor(query), orderInvalidFirmAddressRepository),
                HasValueObject(new OrderBargainSignedLaterThanOrderAccessor(query), orderBargainSignedLaterThanOrderRepository),
                HasValueObject(new OrderHasNoAnyLegalPersonProfileAccessor(query), orderHasNoAnyLegalPersonProfileRepository),
                HasValueObject(new OrderHasNoAnyPositionAccessor(query), orderHasNoAnyPositionRepository),
                HasValueObject(new InactiveReferenceAccessor(query), inactiveReferenceRepository),
                HasValueObject(new OrderInvalidBeginDistributionDateAccessor(query), orderInvalidBeginDistributionDateRepository),
                HasValueObject(new OrderInvalidEndDistributionDateAccessor(query), orderInvalidEndDistributionDateRepository),
                HasValueObject(new OrderInvalidBillsPeriodAccessor(query), orderInvalidBillsPeriodRepository),
                HasValueObject(new OrderInvalidBillsTotalAccessor(query), orderInvalidBillsTotalRepository),
                HasValueObject(new OrderLegalPersonProfileBargainEndDateIsEarlierThanOrderSignupDateAccessor(query), orderLegalPersonProfileBargainEndDateIsEarlierThanOrderSignupDateRepository),
                HasValueObject(new OrderLegalPersonProfileWarrantyEndDateIsEarlierThanOrderSignupDateAccessor(query), orderLegalPersonProfileWarrantyEndDateIsEarlierThanOrderSignupDateRepository),
                HasValueObject(new OrderMissingBargainScanAccessor(query), orderMissingBargainScanRepository),
                HasValueObject(new OrderMissingBillsAccessor(query), orderMissingBillsRepository),
                HasValueObject(new MissingRequiredFieldAccessor(query), orderMissingRequiredFieldRepository),
                HasValueObject(new OrderMissingOrderScanAccessor(query), orderMissingOrderScanRepository));
        }

        public sealed class OrderAccessor : AggregateDataChangesHandler<Order>, IStorageBasedDataObjectAccessor<Order>
        {
            private readonly IQuery _query;

            public OrderAccessor(IQuery query)
            {
                Invalidate(MessageTypeCode.BargainScanShouldPresent);
                Invalidate(MessageTypeCode.BillsPeriodShouldMatchOrder);
                Invalidate(MessageTypeCode.BillsShouldBeCreated);
                Invalidate(MessageTypeCode.BillsSumShouldMatchOrder);
                Invalidate(MessageTypeCode.LegalPersonProfileBargainShouldNotBeExpired);
                Invalidate(MessageTypeCode.LegalPersonProfileWarrantyShouldNotBeExpired);
                Invalidate(MessageTypeCode.LegalPersonShouldHaveAtLeastOneProfile);
                Invalidate(MessageTypeCode.LinkedCategoryAsterixMayBelongToFirm);
                Invalidate(MessageTypeCode.LinkedCategoryFirmAddressShouldBeValid);
                Invalidate(MessageTypeCode.LinkedCategoryShouldBeActive);
                Invalidate(MessageTypeCode.LinkedCategoryShouldBelongToFirm);
                Invalidate(MessageTypeCode.LinkedFirmAddressShouldBeValid);
                Invalidate(MessageTypeCode.LinkedFirmShouldBeValid);
                Invalidate(MessageTypeCode.OrderBeginDistrubutionShouldBeFirstDayOfMonth);
                Invalidate(MessageTypeCode.OrderEndDistrubutionShouldBeLastSecondOfMonth);
                Invalidate(MessageTypeCode.OrderMustHaveActiveDeal);
                Invalidate(MessageTypeCode.OrderMustHaveActiveLegalEntities);
                Invalidate(MessageTypeCode.OrderRequiredFieldsShouldBeSpecified);
                Invalidate(MessageTypeCode.OrderScanShouldPresent);
                Invalidate(MessageTypeCode.OrderShouldHaveAtLeastOnePosition);
                Invalidate(MessageTypeCode.OrderShouldNotBeSignedBeforeBargain);

                _query = query;
            }

            public IQueryable<Order> GetSource()
                => from order in _query.For<Facts::Order>()
                   from project in _query.For<Facts::Project>().Where(x => x.OrganizationUnitId == order.DestOrganizationUnitId)
                   select new Order
                       {
                           Id = order.Id,
                           ProjectId = project.Id,
                           BeginDistribution = order.BeginDistribution,
                           EndDistributionFact = order.EndDistributionFact,
                           EndDistributionPlan = order.EndDistributionPlan,
                           Number = order.Number,
                   };

            public FindSpecification<Order> GetFindSpecification(IReadOnlyCollection<ICommand> commands)
            {
                var aggregateIds = commands.OfType<CreateDataObjectCommand>().Select(c => c.DataObjectId)
                                           .Concat(commands.OfType<SyncDataObjectCommand>().Select(c => c.DataObjectId))
                                           .Concat(commands.OfType<DeleteDataObjectCommand>().Select(c => c.DataObjectId))
                                           .Distinct()
                                           .ToArray();
                return new FindSpecification<Order>(x => aggregateIds.Contains(x.Id));
            }
        }

        public sealed class InvalidFirmAccessor : AggregateDataChangesHandler<Order.InvalidFirm>, IStorageBasedDataObjectAccessor<Order.InvalidFirm>
        {
            private readonly IQuery _query;

            public InvalidFirmAccessor(IQuery query)
            {
                Invalidate(MessageTypeCode.LinkedFirmShouldBeValid);

                _query = query;
            }

            public IQueryable<Order.InvalidFirm> GetSource()
                => from order in _query.For<Facts::Order>()
                   from firm in _query.For<Facts::Firm>().Where(x => x.Id == order.FirmId)
                   let state = firm.IsDeleted ? InvalidFirmState.Deleted
                                   : !firm.IsActive ? InvalidFirmState.ClosedForever
                                   : firm.IsClosedForAscertainment ? InvalidFirmState.ClosedForAscertainment 
                                   : InvalidFirmState.NotSet
                   where state != InvalidFirmState.NotSet // todo: интересно было бы глянуть на сгенерированный sql
                   select new Order.InvalidFirm
                       {
                           FirmId = firm.Id,
                           FirmName = firm.Name,
                           OrderId = order.Id,
                           State = state,
                       };

            public FindSpecification<Order.InvalidFirm> GetFindSpecification(IReadOnlyCollection<ICommand> commands)
            {
                var aggregateIds = commands.OfType<CreateDataObjectCommand>().Select(c => c.DataObjectId)
                                           .Concat(commands.OfType<SyncDataObjectCommand>().Select(c => c.DataObjectId))
                                           .Concat(commands.OfType<DeleteDataObjectCommand>().Select(c => c.DataObjectId))
                                           .Distinct()
                                           .ToArray();
                return new FindSpecification<Order.InvalidFirm>(x => aggregateIds.Contains(x.OrderId));
            }
        }

        public sealed class InvalidFirmAddressAccessor : AggregateDataChangesHandler<Order.InvalidFirmAddress>, IStorageBasedDataObjectAccessor<Order.InvalidFirmAddress>
        {
            private readonly IQuery _query;

            public InvalidFirmAddressAccessor(IQuery query)
            {
                Invalidate(MessageTypeCode.LinkedFirmAddressShouldBeValid);

                _query = query;
            }

            public IQueryable<Order.InvalidFirmAddress> GetSource()
                => from order in _query.For<Facts::Order>()
                   from orderPosition in _query.For<Facts::OrderPosition>().Where(x => x.OrderId == order.Id)
                   from opa in _query.For<Facts::OrderPositionAdvertisement>().Where(x => x.OrderPositionId == orderPosition.Id)
                   from position in _query.For<Facts::Position>().Where(x => x.Id == opa.PositionId)
                   from address in _query.For<Facts::FirmAddress>().Where(x => x.Id == opa.FirmAddressId)
                   let state = address.FirmId != order.FirmId ? InvalidFirmAddressState.NotBelongToFirm
                                : address.IsDeleted ? InvalidFirmAddressState.Deleted
                                : !address.IsActive ? InvalidFirmAddressState.NotActive
                                : address.IsClosedForAscertainment ? InvalidFirmAddressState.ClosedForAscertainment
                                : InvalidFirmAddressState.NotSet
                   where state != InvalidFirmAddressState.NotSet // todo: интересно было бы глянуть на сгенерированный sql
                   select new Order.InvalidFirmAddress
                       {
                           OrderId = order.Id,
                           FirmAddressId = address.Id,
                           FirmAddressName = address.Name,
                           OrderPositionId = orderPosition.Id,
                           OrderPositionName = position.Name,
                           State = state,
                       };

            public FindSpecification<Order.InvalidFirmAddress> GetFindSpecification(IReadOnlyCollection<ICommand> commands)
            {
                var aggregateIds = commands.OfType<CreateDataObjectCommand>().Select(c => c.DataObjectId)
                                           .Concat(commands.OfType<SyncDataObjectCommand>().Select(c => c.DataObjectId))
                                           .Concat(commands.OfType<DeleteDataObjectCommand>().Select(c => c.DataObjectId))
                                           .Distinct()
                                           .ToArray();
                return new FindSpecification<Order.InvalidFirmAddress>(x => aggregateIds.Contains(x.OrderId));
            }
        }

        public sealed class InvalidCategoryFirmAddressAccessor : AggregateDataChangesHandler<Order.InvalidCategoryFirmAddress>, IStorageBasedDataObjectAccessor<Order.InvalidCategoryFirmAddress>
        {
            private readonly IQuery _query;

            public InvalidCategoryFirmAddressAccessor(IQuery query)
            {
                Invalidate(MessageTypeCode.LinkedCategoryFirmAddressShouldBeValid);

                _query = query;
            }

            public IQueryable<Order.InvalidCategoryFirmAddress> GetSource()
                => from order in _query.For<Facts::Order>()
                   from orderPosition in _query.For<Facts::OrderPosition>().Where(x => x.OrderId == order.Id)
                   from opa in _query.For<Facts::OrderPositionAdvertisement>().Where(x => x.OrderPositionId == orderPosition.Id && x.CategoryId.HasValue && x.FirmAddressId.HasValue)
                   from position in _query.For<Facts::Position>().Where(x => x.Id == opa.PositionId)
                   from address in _query.For<Facts::FirmAddress>().Where(x => x.Id == opa.FirmAddressId && x.IsActive && !x.IsClosedForAscertainment && !x.IsDeleted)
                   from category in _query.For<Facts::Category>().Where(x => x.Id == opa.CategoryId)
                   from cfa in _query.For<Facts::CategoryFirmAddress>().Where(x => x.FirmAddressId == opa.FirmAddressId && x.CategoryId == opa.CategoryId).DefaultIfEmpty()
                   where cfa == null
                   select new Order.InvalidCategoryFirmAddress
                   {
                       OrderId = order.Id,
                       FirmAddressId = address.Id,
                       FirmAddressName = address.Name,
                       CategoryId = category.Id,
                       CategoryName = category.Name,
                       OrderPositionId = orderPosition.Id,
                       OrderPositionName = position.Name,
                       State = InvalidCategoryFirmAddressState.CategoryNotBelongsToAddress,
                   };

            public FindSpecification<Order.InvalidCategoryFirmAddress> GetFindSpecification(IReadOnlyCollection<ICommand> commands)
            {
                var aggregateIds = commands.OfType<CreateDataObjectCommand>().Select(c => c.DataObjectId)
                                           .Concat(commands.OfType<SyncDataObjectCommand>().Select(c => c.DataObjectId))
                                           .Concat(commands.OfType<DeleteDataObjectCommand>().Select(c => c.DataObjectId))
                                           .Distinct()
                                           .ToArray();
                return new FindSpecification<Order.InvalidCategoryFirmAddress>(x => aggregateIds.Contains(x.OrderId));
            }
        }

        public sealed class InvalidCategoryAccessor : AggregateDataChangesHandler<Order.InvalidCategory>, IStorageBasedDataObjectAccessor<Order.InvalidCategory>
        {
            private const int BindingObjectTypeCategoryMultipleAsterix = 1;

            private readonly IQuery _query;

            public InvalidCategoryAccessor(IQuery query)
            {
                Invalidate(MessageTypeCode.LinkedCategoryAsterixMayBelongToFirm);
                Invalidate(MessageTypeCode.LinkedCategoryShouldBeActive);
                Invalidate(MessageTypeCode.LinkedCategoryShouldBelongToFirm);

                _query = query;
            }

            public IQueryable<Order.InvalidCategory> GetSource()
                => from order in _query.For<Facts::Order>()
                   from orderPosition in _query.For<Facts::OrderPosition>().Where(x => x.OrderId == order.Id)
                   from opa in _query.For<Facts::OrderPositionAdvertisement>().Where(x => x.OrderPositionId == orderPosition.Id && x.CategoryId.HasValue && x.FirmAddressId.HasValue)
                   from category in _query.For<Facts::Category>().Where(x => x.Id == opa.CategoryId)
                   from position in _query.For<Facts::Position>().Where(x => x.Id == opa.PositionId)
                   let categoryBelongToFirmAddress = _query.For<Facts::FirmAddress>()
                                                           .SelectMany(fa => _query.For<Facts::CategoryFirmAddress>().Where(cfa => cfa.FirmAddressId == fa.Id))
                                                           .Any(x => x.CategoryId == opa.CategoryId)
                   let state = !category.IsActiveNotDeleted ? InvalidCategoryState.Inactive
                                   : !categoryBelongToFirmAddress ? InvalidCategoryState.NotBelongToFirm
                                   : InvalidCategoryState.NotSet
                   where state != InvalidCategoryState.NotSet
                   select new Order.InvalidCategory
                       {
                           OrderId = order.Id,
                           CategoryId = category.Id,
                           CategoryName = category.Name,
                           OrderPositionId = orderPosition.Id,
                           OrderPositionName = position.Name,
                           MayNotBelongToFirm = position.BindingObjectType == BindingObjectTypeCategoryMultipleAsterix,
                           State = state,
                       };

            public FindSpecification<Order.InvalidCategory> GetFindSpecification(IReadOnlyCollection<ICommand> commands)
            {
                var aggregateIds = commands.OfType<CreateDataObjectCommand>().Select(c => c.DataObjectId)
                                           .Concat(commands.OfType<SyncDataObjectCommand>().Select(c => c.DataObjectId))
                                           .Concat(commands.OfType<DeleteDataObjectCommand>().Select(c => c.DataObjectId))
                                           .Distinct()
                                           .ToArray();
                return new FindSpecification<Order.InvalidCategory>(x => aggregateIds.Contains(x.OrderId));
            }
        }

        public sealed class OrderBargainSignedLaterThanOrderAccessor : AggregateDataChangesHandler<Order.BargainSignedLaterThanOrder>, IStorageBasedDataObjectAccessor<Order.BargainSignedLaterThanOrder>
        {
            private readonly IQuery _query;

            public OrderBargainSignedLaterThanOrderAccessor(IQuery query)
            {
                Invalidate(MessageTypeCode.OrderShouldNotBeSignedBeforeBargain);

                _query = query;
            }

            public IQueryable<Order.BargainSignedLaterThanOrder> GetSource()
                => from order in _query.For<Facts::Order>()
                   from bargain in _query.For<Facts::Bargain>().Where(x => x.Id == order.BargainId)
                   where bargain.SignupDate > order.SignupDate
                   select new Order.BargainSignedLaterThanOrder
                   {
                       OrderId = order.Id,
                   };

            public FindSpecification<Order.BargainSignedLaterThanOrder> GetFindSpecification(IReadOnlyCollection<ICommand> commands)
            {
                var aggregateIds = commands.OfType<CreateDataObjectCommand>().Select(c => c.DataObjectId)
                                           .Concat(commands.OfType<SyncDataObjectCommand>().Select(c => c.DataObjectId))
                                           .Concat(commands.OfType<DeleteDataObjectCommand>().Select(c => c.DataObjectId))
                                           .Distinct()
                                           .ToArray();
                return new FindSpecification<Order.BargainSignedLaterThanOrder>(x => aggregateIds.Contains(x.OrderId));
            }
        }

        public sealed class OrderHasNoAnyLegalPersonProfileAccessor : AggregateDataChangesHandler<Order.HasNoAnyLegalPersonProfile>, IStorageBasedDataObjectAccessor<Order.HasNoAnyLegalPersonProfile>
        {
            private readonly IQuery _query;

            public OrderHasNoAnyLegalPersonProfileAccessor(IQuery query)
            {
                Invalidate(MessageTypeCode.LegalPersonShouldHaveAtLeastOneProfile);

                _query = query;
            }

            public IQueryable<Order.HasNoAnyLegalPersonProfile> GetSource()
                => from order in _query.For<Facts::Order>()
                   from profile in _query.For<Facts::LegalPersonProfile>().Where(x => x.LegalPersonId == order.LegalPersonId).DefaultIfEmpty()
                   where profile == null
                   select new Order.HasNoAnyLegalPersonProfile
                   {
                       OrderId = order.Id,
                   };

            public FindSpecification<Order.HasNoAnyLegalPersonProfile> GetFindSpecification(IReadOnlyCollection<ICommand> commands)
            {
                var aggregateIds = commands.OfType<CreateDataObjectCommand>().Select(c => c.DataObjectId)
                                           .Concat(commands.OfType<SyncDataObjectCommand>().Select(c => c.DataObjectId))
                                           .Concat(commands.OfType<DeleteDataObjectCommand>().Select(c => c.DataObjectId))
                                           .Distinct()
                                           .ToArray();
                return new FindSpecification<Order.HasNoAnyLegalPersonProfile>(x => aggregateIds.Contains(x.OrderId));
            }
        }

        public sealed class OrderHasNoAnyPositionAccessor : AggregateDataChangesHandler<Order.HasNoAnyPosition>, IStorageBasedDataObjectAccessor<Order.HasNoAnyPosition>
        {
            private readonly IQuery _query;

            public OrderHasNoAnyPositionAccessor(IQuery query)
            {
                Invalidate(MessageTypeCode.OrderShouldHaveAtLeastOnePosition);

                _query = query;
            }

            public IQueryable<Order.HasNoAnyPosition> GetSource()
                => from order in _query.For<Facts::Order>()
                   from orderPosition in _query.For<Facts::OrderPosition>().Where(x => x.OrderId == order.Id).DefaultIfEmpty()
                   where orderPosition == null
                   select new Order.HasNoAnyPosition
                   {
                       OrderId = order.Id,
                   };

            public FindSpecification<Order.HasNoAnyPosition> GetFindSpecification(IReadOnlyCollection<ICommand> commands)
            {
                var aggregateIds = commands.OfType<CreateDataObjectCommand>().Select(c => c.DataObjectId)
                                           .Concat(commands.OfType<SyncDataObjectCommand>().Select(c => c.DataObjectId))
                                           .Concat(commands.OfType<DeleteDataObjectCommand>().Select(c => c.DataObjectId))
                                           .Distinct()
                                           .ToArray();
                return new FindSpecification<Order.HasNoAnyPosition>(x => aggregateIds.Contains(x.OrderId));
            }
        }

        public sealed class InactiveReferenceAccessor : AggregateDataChangesHandler<Order.InactiveReference>, IStorageBasedDataObjectAccessor<Order.InactiveReference>
        {
            private readonly IQuery _query;

            public InactiveReferenceAccessor(IQuery query)
            {
                Invalidate(MessageTypeCode.OrderMustHaveActiveDeal);
                Invalidate(MessageTypeCode.OrderMustHaveActiveLegalEntities);

                _query = query;
            }

            // todo: сравнить запросы left join и exists
            public IQueryable<Order.InactiveReference> GetSource()
                => from order in _query.For<Facts::Order>()
                   from boou in _query.For<Facts::BranchOfficeOrganizationUnit>().Where(x => x.Id == order.BranchOfficeOrganizationUnitId.Value).DefaultIfEmpty()
                   from bo in _query.For<Facts::BranchOffice>().Where(x => x.Id == boou.BranchOfficeId).DefaultIfEmpty()
                   from legalPerson in _query.For<Facts::LegalPerson>().Where(x => x.Id == order.LegalPersonId.Value).DefaultIfEmpty()
                   from legalPersonProfile in _query.For<Facts::LegalPersonProfile>().Where(x => x.Id == order.LegalPersonProfileId.Value).DefaultIfEmpty()
                   from deal in _query.For<Facts::Deal>().Where(x => x.Id == order.DealId.Value).DefaultIfEmpty()
                   where boou == null || bo == null || legalPerson == null || legalPersonProfile == null || deal == null
                   select new Order.InactiveReference
                       {
                           OrderId = order.Id,
                           BranchOfficeOrganizationUnit = order.BranchOfficeOrganizationUnitId.HasValue && boou == null,
                           BranchOffice = boou != null && bo == null,
                           LegalPerson = order.LegalPersonId.HasValue && legalPerson == null,
                           LegalPersonProfile = order.LegalPersonProfileId.HasValue && legalPersonProfile == null,
                           Deal = order.DealId.HasValue && deal == null,
                       };

            public FindSpecification<Order.InactiveReference> GetFindSpecification(IReadOnlyCollection<ICommand> commands)
            {
                var aggregateIds = commands.OfType<CreateDataObjectCommand>().Select(c => c.DataObjectId)
                                           .Concat(commands.OfType<SyncDataObjectCommand>().Select(c => c.DataObjectId))
                                           .Concat(commands.OfType<DeleteDataObjectCommand>().Select(c => c.DataObjectId))
                                           .Distinct()
                                           .ToArray();
                return new FindSpecification<Order.InactiveReference>(x => aggregateIds.Contains(x.OrderId));
            }
        }

        public sealed class OrderInvalidBeginDistributionDateAccessor : AggregateDataChangesHandler<Order.InvalidBeginDistributionDate>, IStorageBasedDataObjectAccessor<Order.InvalidBeginDistributionDate>
        {
            private readonly IQuery _query;

            public OrderInvalidBeginDistributionDateAccessor(IQuery query)
            {
                Invalidate(MessageTypeCode.OrderBeginDistrubutionShouldBeFirstDayOfMonth);

                _query = query;
            }

            public IQueryable<Order.InvalidBeginDistributionDate> GetSource()
                => from order in _query.For<Facts::Order>()
                   where order.BeginDistribution.Day != 1
                   select new Order.InvalidBeginDistributionDate
                   {
                       OrderId = order.Id,
                   };

            public FindSpecification<Order.InvalidBeginDistributionDate> GetFindSpecification(IReadOnlyCollection<ICommand> commands)
            {
                var aggregateIds = commands.OfType<CreateDataObjectCommand>().Select(c => c.DataObjectId)
                                           .Concat(commands.OfType<SyncDataObjectCommand>().Select(c => c.DataObjectId))
                                           .Concat(commands.OfType<DeleteDataObjectCommand>().Select(c => c.DataObjectId))
                                           .Distinct()
                                           .ToArray();
                return new FindSpecification<Order.InvalidBeginDistributionDate>(x => aggregateIds.Contains(x.OrderId));
            }
        }

        public sealed class OrderInvalidEndDistributionDateAccessor : AggregateDataChangesHandler<Order.InvalidEndDistributionDate>, IStorageBasedDataObjectAccessor<Order.InvalidEndDistributionDate>
        {
            private readonly IQuery _query;

            public OrderInvalidEndDistributionDateAccessor(IQuery query)
            {
                Invalidate(MessageTypeCode.OrderEndDistrubutionShouldBeLastSecondOfMonth);

                _query = query;
            }

            public IQueryable<Order.InvalidEndDistributionDate> GetSource()
                => from order in _query.For<Facts::Order>()
                   where order.EndDistributionPlan != order.BeginDistribution.AddMonths(order.ReleaseCountPlan)
                   select new Order.InvalidEndDistributionDate
                   {
                       OrderId = order.Id,
                   };

            public FindSpecification<Order.InvalidEndDistributionDate> GetFindSpecification(IReadOnlyCollection<ICommand> commands)
            {
                var aggregateIds = commands.OfType<CreateDataObjectCommand>().Select(c => c.DataObjectId)
                                           .Concat(commands.OfType<SyncDataObjectCommand>().Select(c => c.DataObjectId))
                                           .Concat(commands.OfType<DeleteDataObjectCommand>().Select(c => c.DataObjectId))
                                           .Distinct()
                                           .ToArray();
                return new FindSpecification<Order.InvalidEndDistributionDate>(x => aggregateIds.Contains(x.OrderId));
            }
        }

        public sealed class OrderInvalidBillsPeriodAccessor : AggregateDataChangesHandler<Order.InvalidBillsPeriod>, IStorageBasedDataObjectAccessor<Order.InvalidBillsPeriod>
        {
            private readonly IQuery _query;

            public OrderInvalidBillsPeriodAccessor(IQuery query)
            {
                Invalidate(MessageTypeCode.BillsPeriodShouldMatchOrder);

                _query = query;
            }

            public IQueryable<Order.InvalidBillsPeriod> GetSource()
                => from order in _query.For<Facts::Order>()
                   let minimumDate = _query.For<Facts::Bill>().Where(x => x.OrderId == order.Id).Min(x => x.Begin)
                   let maximumDate = _query.For<Facts::Bill>().Where(x => x.OrderId == order.Id).Max(x => x.End)
                   where order.BeginDistribution != minimumDate || order.EndDistributionPlan != maximumDate
                   select new Order.InvalidBillsPeriod
                       {
                           OrderId = order.Id,
                       };

            public FindSpecification<Order.InvalidBillsPeriod> GetFindSpecification(IReadOnlyCollection<ICommand> commands)
            {
                var aggregateIds = commands.OfType<CreateDataObjectCommand>().Select(c => c.DataObjectId)
                                           .Concat(commands.OfType<SyncDataObjectCommand>().Select(c => c.DataObjectId))
                                           .Concat(commands.OfType<DeleteDataObjectCommand>().Select(c => c.DataObjectId))
                                           .Distinct()
                                           .ToArray();
                return new FindSpecification<Order.InvalidBillsPeriod>(x => aggregateIds.Contains(x.OrderId));
            }
        }

        public sealed class OrderInvalidBillsTotalAccessor : AggregateDataChangesHandler<Order.InvalidBillsTotal>, IStorageBasedDataObjectAccessor<Order.InvalidBillsTotal>
        {
            private const int WorkflowStepOnRegistration = 1;

            private readonly IQuery _query;

            public OrderInvalidBillsTotalAccessor(IQuery query)
            {
                Invalidate(MessageTypeCode.BillsSumShouldMatchOrder);

                _query = query;
            }

            public IQueryable<Order.InvalidBillsTotal> GetSource()
                => from order in _query.For<Facts::Order>().Where(x => x.WorkflowStep == WorkflowStepOnRegistration)
                   let billTotal = _query.For<Facts::Bill>().Where(x => x.OrderId == order.Id).Sum(x => x.PayablePlan)
                   let orderTotal = (from op in _query.For<Facts::OrderPosition>().Where(x => x.OrderId == order.Id)
                                     from rw in _query.For<Facts::ReleaseWithdrawal>().Where(x => x.OrderPositionId == op.Id)
                                     select rw.Amount).Sum()
                   where billTotal != orderTotal
                   select new Order.InvalidBillsTotal
                   {
                       OrderId = order.Id,
                   };

            public FindSpecification<Order.InvalidBillsTotal> GetFindSpecification(IReadOnlyCollection<ICommand> commands)
            {
                var aggregateIds = commands.OfType<CreateDataObjectCommand>().Select(c => c.DataObjectId)
                                           .Concat(commands.OfType<SyncDataObjectCommand>().Select(c => c.DataObjectId))
                                           .Concat(commands.OfType<DeleteDataObjectCommand>().Select(c => c.DataObjectId))
                                           .Distinct()
                                           .ToArray();
                return new FindSpecification<Order.InvalidBillsTotal>(x => aggregateIds.Contains(x.OrderId));
            }
        }

        public sealed class OrderLegalPersonProfileBargainEndDateIsEarlierThanOrderSignupDateAccessor : AggregateDataChangesHandler<Order.LegalPersonProfileBargainExpired>, IStorageBasedDataObjectAccessor<Order.LegalPersonProfileBargainExpired>
        {
            private readonly IQuery _query;

            public OrderLegalPersonProfileBargainEndDateIsEarlierThanOrderSignupDateAccessor(IQuery query)
            {
                Invalidate(MessageTypeCode.LegalPersonProfileBargainShouldNotBeExpired);

                _query = query;
            }

            public IQueryable<Order.LegalPersonProfileBargainExpired> GetSource()
                => from order in _query.For<Facts::Order>()
                   from profile in _query.For<Facts::LegalPersonProfile>().Where(x => x.LegalPersonId == order.LegalPersonId)
                   where profile.BargainEndDate.HasValue && profile.BargainEndDate.Value < order.SignupDate
                   select new Order.LegalPersonProfileBargainExpired
                   {
                       OrderId = order.Id,
                       LegalPersonProfileId = profile.Id,
                       LegalPersonProfileName = profile.Name,
                   };

            public FindSpecification<Order.LegalPersonProfileBargainExpired> GetFindSpecification(IReadOnlyCollection<ICommand> commands)
            {
                var aggregateIds = commands.OfType<CreateDataObjectCommand>().Select(c => c.DataObjectId)
                                           .Concat(commands.OfType<SyncDataObjectCommand>().Select(c => c.DataObjectId))
                                           .Concat(commands.OfType<DeleteDataObjectCommand>().Select(c => c.DataObjectId))
                                           .Distinct()
                                           .ToArray();
                return new FindSpecification<Order.LegalPersonProfileBargainExpired>(x => aggregateIds.Contains(x.OrderId));
            }
        }

        public sealed class OrderLegalPersonProfileWarrantyEndDateIsEarlierThanOrderSignupDateAccessor : AggregateDataChangesHandler<Order.LegalPersonProfileWarrantyExpired>, IStorageBasedDataObjectAccessor<Order.LegalPersonProfileWarrantyExpired>
        {
            private readonly IQuery _query;

            public OrderLegalPersonProfileWarrantyEndDateIsEarlierThanOrderSignupDateAccessor(IQuery query)
            {
                Invalidate(MessageTypeCode.LegalPersonProfileWarrantyShouldNotBeExpired);

                _query = query;
            }

            public IQueryable<Order.LegalPersonProfileWarrantyExpired> GetSource()
                => from order in _query.For<Facts::Order>()
                   from profile in _query.For<Facts::LegalPersonProfile>().Where(x => x.LegalPersonId == order.LegalPersonId)
                   where profile.WarrantyEndDate.HasValue && profile.WarrantyEndDate.Value < order.SignupDate
                   select new Order.LegalPersonProfileWarrantyExpired
                   {
                       OrderId = order.Id,
                       LegalPersonProfileId = profile.Id,
                       LegalPersonProfileName = profile.Name,
                   };

            public FindSpecification<Order.LegalPersonProfileWarrantyExpired> GetFindSpecification(IReadOnlyCollection<ICommand> commands)
            {
                var aggregateIds = commands.OfType<CreateDataObjectCommand>().Select(c => c.DataObjectId)
                                           .Concat(commands.OfType<SyncDataObjectCommand>().Select(c => c.DataObjectId))
                                           .Concat(commands.OfType<DeleteDataObjectCommand>().Select(c => c.DataObjectId))
                                           .Distinct()
                                           .ToArray();
                return new FindSpecification<Order.LegalPersonProfileWarrantyExpired>(x => aggregateIds.Contains(x.OrderId));
            }
        }

        public sealed class OrderMissingBargainScanAccessor : AggregateDataChangesHandler<Order.MissingBargainScan>, IStorageBasedDataObjectAccessor<Order.MissingBargainScan>
        {
            private readonly IQuery _query;

            public OrderMissingBargainScanAccessor(IQuery query)
            {
                Invalidate(MessageTypeCode.BargainScanShouldPresent);

                _query = query;
            }

            public IQueryable<Order.MissingBargainScan> GetSource()
                => from order in _query.For<Facts::Order>()
                   from scan in _query.For<Facts::BargainScanFile>().Where(x => x.BargainId == order.BargainId).DefaultIfEmpty()
                   where order.BargainId.HasValue && scan == null
                   select new Order.MissingBargainScan
                   {
                       OrderId = order.Id,
                   };

            public FindSpecification<Order.MissingBargainScan> GetFindSpecification(IReadOnlyCollection<ICommand> commands)
            {
                var aggregateIds = commands.OfType<CreateDataObjectCommand>().Select(c => c.DataObjectId)
                                           .Concat(commands.OfType<SyncDataObjectCommand>().Select(c => c.DataObjectId))
                                           .Concat(commands.OfType<DeleteDataObjectCommand>().Select(c => c.DataObjectId))
                                           .Distinct()
                                           .ToArray();
                return new FindSpecification<Order.MissingBargainScan>(x => aggregateIds.Contains(x.OrderId));
            }
        }

        public sealed class OrderMissingBillsAccessor : AggregateDataChangesHandler<Order.MissingBills>, IStorageBasedDataObjectAccessor<Order.MissingBills>
        {
            private const int WorkflowStepOnRegistration = 1;

            private readonly IQuery _query;

            public OrderMissingBillsAccessor(IQuery query)
            {
                Invalidate(MessageTypeCode.BillsShouldBeCreated);

                _query = query;
            }

            public IQueryable<Order.MissingBills> GetSource()
                => from order in _query.For<Facts::Order>().Where(x => x.WorkflowStep == WorkflowStepOnRegistration)
                   let billCount = _query.For<Facts::Bill>().Count(x => x.OrderId == order.Id)
                   let orderTotal = (from op in _query.For<Facts::OrderPosition>().Where(x => x.OrderId == order.Id)
                                     from rw in _query.For<Facts::ReleaseWithdrawal>().Where(x => x.OrderPositionId == op.Id)
                                     select rw.Amount).Sum()
                   where orderTotal > 0 && !order.IsFreeOfCharge && billCount == 0
                   select new Order.MissingBills
                   {
                       OrderId = order.Id,
                   };

            public FindSpecification<Order.MissingBills> GetFindSpecification(IReadOnlyCollection<ICommand> commands)
            {
                var aggregateIds = commands.OfType<CreateDataObjectCommand>().Select(c => c.DataObjectId)
                                           .Concat(commands.OfType<SyncDataObjectCommand>().Select(c => c.DataObjectId))
                                           .Concat(commands.OfType<DeleteDataObjectCommand>().Select(c => c.DataObjectId))
                                           .Distinct()
                                           .ToArray();
                return new FindSpecification<Order.MissingBills>(x => aggregateIds.Contains(x.OrderId));
            }
        }

        public sealed class MissingRequiredFieldAccessor : AggregateDataChangesHandler<Order.MissingRequiredField>, IStorageBasedDataObjectAccessor<Order.MissingRequiredField>
        {
            private readonly IQuery _query;

            public MissingRequiredFieldAccessor(IQuery query)
            {
                Invalidate(MessageTypeCode.OrderMustHaveActiveDeal);
                Invalidate(MessageTypeCode.OrderRequiredFieldsShouldBeSpecified);

                _query = query;
            }

            public IQueryable<Order.MissingRequiredField> GetSource()
                => from order in _query.For<Facts::Order>()
                   where !(order.BranchOfficeOrganizationUnitId.HasValue && order.CurrencyId.HasValue && order.InspectorId.HasValue && order.LegalPersonId.HasValue && order.LegalPersonProfileId.HasValue && order.DealId.HasValue)
                   select new Order.MissingRequiredField
                       {
                           OrderId = order.Id,
                           BranchOfficeOrganizationUnit = !order.BranchOfficeOrganizationUnitId.HasValue,
                           Currency = !order.CurrencyId.HasValue,
                           Inspector = !order.InspectorId.HasValue,
                           LegalPerson = !order.LegalPersonId.HasValue,
                           LegalPersonProfile = !order.LegalPersonProfileId.HasValue,
                           ReleaseCountPlan = order.ReleaseCountPlan == 0,
                           Deal = !order.DealId.HasValue,
                       };

            public FindSpecification<Order.MissingRequiredField> GetFindSpecification(IReadOnlyCollection<ICommand> commands)
            {
                var aggregateIds = commands.OfType<CreateDataObjectCommand>().Select(c => c.DataObjectId)
                                           .Concat(commands.OfType<SyncDataObjectCommand>().Select(c => c.DataObjectId))
                                           .Concat(commands.OfType<DeleteDataObjectCommand>().Select(c => c.DataObjectId))
                                           .Distinct()
                                           .ToArray();
                return new FindSpecification<Order.MissingRequiredField>(x => aggregateIds.Contains(x.OrderId));
            }
        }

        public sealed class OrderMissingOrderScanAccessor : AggregateDataChangesHandler<Order.MissingOrderScan>, IStorageBasedDataObjectAccessor<Order.MissingOrderScan>
        {
            private readonly IQuery _query;

            public OrderMissingOrderScanAccessor(IQuery query)
            {
                Invalidate(MessageTypeCode.OrderScanShouldPresent);

                _query = query;
            }

            public IQueryable<Order.MissingOrderScan> GetSource()
                => from order in _query.For<Facts::Order>()
                   from scan in _query.For<Facts::OrderScanFile>().Where(x => x.OrderId == order.Id).DefaultIfEmpty()
                   where scan == null
                   select new Order.MissingOrderScan
                   {
                       OrderId = order.Id,
                   };

            public FindSpecification<Order.MissingOrderScan> GetFindSpecification(IReadOnlyCollection<ICommand> commands)
            {
                var aggregateIds = commands.OfType<CreateDataObjectCommand>().Select(c => c.DataObjectId)
                                           .Concat(commands.OfType<SyncDataObjectCommand>().Select(c => c.DataObjectId))
                                           .Concat(commands.OfType<DeleteDataObjectCommand>().Select(c => c.DataObjectId))
                                           .Distinct()
                                           .ToArray();
                return new FindSpecification<Order.MissingOrderScan>(x => aggregateIds.Contains(x.OrderId));
            }
        }
    }
}