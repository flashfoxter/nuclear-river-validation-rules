﻿using System;

using NuClear.Replication.Core;

namespace NuClear.ValidationRules.Replication.Events
{
    public class FactsBatchProcessedEvent : IEvent
    {
        public FactsBatchProcessedEvent(DateTime eventTime)
        {
            EventTime = eventTime;
        }

        public DateTime EventTime { get; }
    }
}