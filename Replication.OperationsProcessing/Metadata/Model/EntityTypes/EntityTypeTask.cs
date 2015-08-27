﻿using NuClear.Model.Common.Entities;

namespace NuClear.Replication.OperationsProcessing.Metadata.Model.EntityTypes
{
    public sealed class EntityTypeTask : EntityTypeBase<EntityTypeTask>
    {
        public override int Id
        {
            get { return EntityTypeIds.Task; }
        }

        public override string Description
        {
            get { return "Task"; }
        }
    }
}