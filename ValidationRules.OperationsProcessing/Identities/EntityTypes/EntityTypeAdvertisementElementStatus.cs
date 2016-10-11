﻿using NuClear.Model.Common.Entities;
using NuClear.ValidationRules.Replication;

namespace NuClear.ValidationRules.OperationsProcessing.Identities.EntityTypes
{
    public sealed class EntityTypeAdvertisementElementStatus : EntityTypeBase<EntityTypeAdvertisementElementStatus>
    {
        public override int Id { get; } = EntityTypeIds.AdvertisementElementStatus;
        public override string Description { get; } = nameof(EntityTypeIds.AdvertisementElementStatus);
    }
}