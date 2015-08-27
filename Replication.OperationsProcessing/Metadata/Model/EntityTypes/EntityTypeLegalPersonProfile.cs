﻿using System;

using NuClear.AdvancedSearch.Replication.API.Model;
using NuClear.Model.Common.Entities;

namespace NuClear.Replication.OperationsProcessing.Metadata.Model.EntityTypes
{
    [Obsolete("Сущность не имеет отношения к домену поиска, удалить после рефакторинга на стороне ERM")]
    public sealed class EntityTypeLegalPersonProfile : EntityTypeBase<EntityTypeLegalPersonProfile>
    {
        public override int Id
        {
            get { return EntityTypeIds.LegalPersonProfile; }
        }

        public override string Description
        {
            get { return "LegalPersonProfile"; }
        }
    }
}