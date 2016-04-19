﻿using System;

using NuClear.River.Common.Metadata;

namespace NuClear.CustomerIntelligence.Domain.Events
{
    public class DataObjectDeletedEvent : IEvent
    {
        public DataObjectDeletedEvent(Type dataObjectType, long dataObjectId)
        {
            DataObjectType = dataObjectType;
            DataObjectId = dataObjectId;
        }

        public Type DataObjectType { get; }
        public long DataObjectId { get; }

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

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((DataObjectDeletedEvent)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((DataObjectType?.GetHashCode() ?? 0) * 397) ^ DataObjectId.GetHashCode();
            }
        }

        private bool Equals(DataObjectDeletedEvent other)
        {
            return DataObjectType == other.DataObjectType && DataObjectId == other.DataObjectId;
        }
    }
}