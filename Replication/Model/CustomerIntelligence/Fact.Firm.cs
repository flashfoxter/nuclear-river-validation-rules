﻿using System;

namespace NuClear.AdvancedSearch.Replication.Model.CustomerIntelligence
{
    public static partial class Fact
    {
        public sealed class Firm : IIdentifiable
        {
            public long Id { get; set; }

            public string Name { get; set; }

            public DateTimeOffset CreatedOn { get; set; }

            public DateTimeOffset? LastDisqualifiedOn { get; set; }

            public DateTimeOffset? LastDistributedOn { get; set; }

            public bool HasPhone { get; set; }

            public bool HasWebsite { get; set; }

            public int AddressCount { get; set; }

            public long? ClientId { get; set; }

            public long OrganizationUnitId { get; set; }

            public long TerritoryId { get; set; }
        }
    }
}