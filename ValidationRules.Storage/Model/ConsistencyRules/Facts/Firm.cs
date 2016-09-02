﻿namespace NuClear.ValidationRules.Storage.Model.ConsistencyRules.Facts
{
    public sealed class Firm
    {
        public long Id { get; set; }
        public bool IsClosedForAscertainment { get; set; }
        public bool IsHidden { get; set; } // todo: вернуть IsActive
        public bool IsDeleted { get; set; }
        public string Name { get; set; }
    }
}