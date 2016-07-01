﻿using System;
using System.Collections.Generic;
using System.Linq;

using NuClear.Replication.Core;
using NuClear.Replication.Core.DataObjects;
using NuClear.Storage.API.Readings;
using NuClear.Storage.API.Specifications;
using NuClear.ValidationRules.Replication.Commands;
using NuClear.ValidationRules.Replication.Events;
using NuClear.ValidationRules.Replication.Specifications;
using NuClear.ValidationRules.Storage.Model.Facts;

namespace NuClear.ValidationRules.Replication.PriceRules.Facts
{
    public sealed class PricePositionAccessor : IStorageBasedDataObjectAccessor<PricePosition>, IDataChangesHandler<PricePosition>
    {
        private readonly IQuery _query;

        public PricePositionAccessor(IQuery query)
        {
            _query = query;
        }

        public IQueryable<PricePosition> GetSource() => Specs.Map.Erm.ToFacts.PricePosition.Map(_query);

        public FindSpecification<PricePosition> GetFindSpecification(IReadOnlyCollection<ICommand> commands)
        {
            var ids = commands.Cast<SyncDataObjectCommand>().Select(c => c.DataObjectId).ToArray();
            return new FindSpecification<PricePosition>(x => ids.Contains(x.Id));
        }

        public IReadOnlyCollection<IEvent> HandleCreates(IReadOnlyCollection<PricePosition> dataObjects) => Array.Empty<IEvent>();

        public IReadOnlyCollection<IEvent> HandleUpdates(IReadOnlyCollection<PricePosition> dataObjects) => Array.Empty<IEvent>();

        public IReadOnlyCollection<IEvent> HandleDeletes(IReadOnlyCollection<PricePosition> dataObjects) => Array.Empty<IEvent>();

        public IReadOnlyCollection<IEvent> HandleRelates(IReadOnlyCollection<PricePosition> dataObjects)
        {
            var ids = dataObjects.Select(x => x.Id).ToArray();
            var specification = new FindSpecification<PricePosition>(x => ids.Contains(x.Id));

            var priceIds = (from pricePosition in _query.For(specification)
                            select pricePosition.PriceId).Distinct()
                            .Distinct()
                            .ToArray();

            return priceIds.Select(x => new RelatedDataObjectOutdatedEvent<long>(typeof(Price), x))
                          .ToArray();
        }
    }
}