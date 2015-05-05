﻿using NuClear.AdvancedSearch.Replication.Model;

namespace NuClear.AdvancedSearch.Replication.CustomerIntelligence.Model
{
    public sealed class FirmBalance : IObject
    {
        public long AccountId { get; set; }

        public long FirmId { get; set; }

        public decimal Balance { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            return obj is FirmBalance && Equals((FirmBalance)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = AccountId.GetHashCode();
                hashCode = (hashCode * 397) ^ FirmId.GetHashCode();
                hashCode = (hashCode * 397) ^ Balance.GetHashCode();
                return hashCode;
            }
        }

        private bool Equals(FirmBalance other)
        {
            return AccountId == other.AccountId && FirmId == other.FirmId && Balance == other.Balance;
        }
    }
}