﻿using System.Collections.Generic;
using System.Linq;

using NuClear.Replication.Core;
using NuClear.Storage.API.Readings;
using NuClear.Storage.API.Specifications;
using NuClear.ValidationRules.Replication.Specifications;
using NuClear.ValidationRules.Storage;
using NuClear.ValidationRules.Storage.Model.Facts;

namespace NuClear.ValidationRules.Replication.Accessors.EntityNames
{
    public sealed class FirmAddressNameAccessor : IStorageBasedEntityNameAccessor<FirmAddress>
    {
        private readonly IQuery _query;

        public FirmAddressNameAccessor(IQuery query)
        {
            _query = query;
        }

        public IQueryable<EntityName> GetSource() => _query
            .For(Specs.Find.Erm.FirmAddress)
            .Select(x => new EntityName
            {
                Id = x.Id,
                EntityType = EntityTypeIds.FirmAddress,
                Name = x.Address
            });

        public FindSpecification<EntityName> GetFindSpecification(IReadOnlyCollection<ICommand> commands)
            => commands.GetEntityNameFindSpecification(typeof(FirmAddress), EntityTypeIds.FirmAddress);
    }
}