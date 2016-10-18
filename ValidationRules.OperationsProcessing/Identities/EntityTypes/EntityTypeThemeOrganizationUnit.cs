﻿using NuClear.Model.Common.Entities;
using NuClear.ValidationRules.Replication;

namespace NuClear.ValidationRules.OperationsProcessing.Identities.EntityTypes
{
    public sealed class EntityTypeThemeOrganizationUnit : EntityTypeBase<EntityTypeThemeOrganizationUnit>
    {
        public override int Id { get; } = EntityTypeIds.ThemeOrganizationUnit;
        public override string Description { get; } = nameof(EntityTypeIds.ThemeOrganizationUnit);
    }
}