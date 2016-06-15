﻿using System;
using System.Collections.Generic;
using System.Linq;

using NuClear.Replication.Core;
using NuClear.Replication.Core.DataObjects;
using NuClear.Storage.API.Readings;
using NuClear.Storage.API.Specifications;
using NuClear.ValidationRules.Replication.Commands;
using NuClear.ValidationRules.Replication.Specifications;
using NuClear.ValidationRules.Storage.Model.Facts;

namespace NuClear.ValidationRules.Replication.Accessors
{
    public sealed class PricePositionNotActiveAccessor : IStorageBasedDataObjectAccessor<PricePositionNotActive>, IDataChangesHandler<PricePositionNotActive>
    {
        private readonly IQuery _query;

        public PricePositionNotActiveAccessor(IQuery query)
        {
            _query = query;
        }

        public IQueryable<PricePositionNotActive> GetSource() => Specs.Map.Erm.ToFacts.PricePositionNotActive.Map(_query);

        public FindSpecification<PricePositionNotActive> GetFindSpecification(IReadOnlyCollection<ICommand> commands)
        {
            var ids = commands.Cast<SyncDataObjectCommand>().Select(c => c.DataObjectId).ToArray();
            return new FindSpecification<PricePositionNotActive>(x => ids.Contains(x.Id));
        }

        public IReadOnlyCollection<IEvent> HandleCreates(IReadOnlyCollection<PricePositionNotActive> dataObjects) => Array.Empty<IEvent>();

        public IReadOnlyCollection<IEvent> HandleUpdates(IReadOnlyCollection<PricePositionNotActive> dataObjects) => Array.Empty<IEvent>();

        public IReadOnlyCollection<IEvent> HandleDeletes(IReadOnlyCollection<PricePositionNotActive> dataObjects) => Array.Empty<IEvent>();

        public IReadOnlyCollection<IEvent> HandleRelates(IReadOnlyCollection<PricePositionNotActive> dataObjects) => Array.Empty<IEvent>();
    }
}