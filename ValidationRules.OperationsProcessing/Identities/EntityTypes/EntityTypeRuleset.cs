﻿using NuClear.Model.Common.Entities;
using NuClear.ValidationRules.Replication;

namespace NuClear.ValidationRules.OperationsProcessing.Identities.EntityTypes
{
    public sealed class EntityTypeRuleset : EntityTypeBase<EntityTypeRuleset>
    {
        public override int Id { get; } = EntityTypeIds.Ruleset;
        public override string Description { get; } = nameof(EntityTypeIds.Ruleset);
    }
}