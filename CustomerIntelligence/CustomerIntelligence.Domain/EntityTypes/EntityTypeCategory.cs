﻿using NuClear.Model.Common.Entities;
using NuClear.River.Common.Metadata;

namespace NuClear.CustomerIntelligence.Domain.EntityTypes
{
    public sealed class EntityTypeCategory : EntityTypeBase<EntityTypeCategory>
    {
        public override int Id
        {
            get { return EntityTypeIds.Category; }
        }

        public override string Description
        {
            get { return "Category"; }
        }
    }
}