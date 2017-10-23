﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

using NuClear.Messaging.API.Processing;
using NuClear.Messaging.API.Processing.Actors.Handlers;
using NuClear.Messaging.API.Processing.Stages;
using NuClear.OperationsLogging.API;
using NuClear.Replication.Core;
using NuClear.Replication.Core.Commands;
using NuClear.Replication.OperationsProcessing;
using NuClear.Telemetry.Probing;
using NuClear.Tracing.API;
using NuClear.ValidationRules.Replication;
using NuClear.ValidationRules.Replication.Commands;
using NuClear.ValidationRules.Replication.Events;

namespace NuClear.ValidationRules.OperationsProcessing.FactsFlow
{
    public sealed class FactsFlowHandler : IMessageProcessingHandler
    {
        private static readonly EventEqualityComparer EqualityComparer = new EventEqualityComparer();

        private readonly IDataObjectsActorFactory _dataObjectsActorFactory;
        private readonly SyncEntityNameActor _syncEntityNameActor;
        private readonly IEventLogger _eventLogger;
        private readonly ITracer _tracer;
        private readonly FactsFlowTelemetryPublisher _telemetryPublisher;
        private readonly TransactionOptions _transactionOptions;

        public FactsFlowHandler(
            IDataObjectsActorFactory dataObjectsActorFactory,
            SyncEntityNameActor syncEntityNameActor,
            IEventLogger eventLogger,
            FactsFlowTelemetryPublisher telemetryPublisher,
            ITracer tracer)
        {
            _dataObjectsActorFactory = dataObjectsActorFactory;
            _syncEntityNameActor = syncEntityNameActor;
            _eventLogger = eventLogger;
            _telemetryPublisher = telemetryPublisher;
            _tracer = tracer;
            _transactionOptions = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.Zero };
        }

        public IEnumerable<StageResult> Handle(IReadOnlyDictionary<Guid, List<IAggregatableMessage>> processingResultsMap)
        {
            try
            {
                using (Probe.Create("ETL1 Transforming"))
                using (var transaction = new TransactionScope(TransactionScopeOption.Required, _transactionOptions))
                {
                    var commands = processingResultsMap.SelectMany(x => x.Value).Cast<AggregatableMessage<ICommand>>().SelectMany(x => x.Commands).ToList();

                    var syncEvents = Handle(commands.OfType<ISyncDataObjectCommand>().ToList())
                                     .Select(x => new FlowEvent(FactsFlow.Instance, x)).ToList();
                    var stateEvents = Handle(commands.OfType<IncrementErmStateCommand>().ToList())
                                      .Select(x => new FlowEvent(FactsFlow.Instance, x));

                    using (new TransactionScope(TransactionScopeOption.Suppress))
                        _eventLogger.Log<IEvent>(syncEvents);

                    transaction.Complete();

                    using (new TransactionScope(TransactionScopeOption.Suppress))
                        _eventLogger.Log<IEvent>(syncEvents.Concat(stateEvents).ToList());
                }

                return processingResultsMap.Keys.Select(bucketId => MessageProcessingStage.Handling.ResultFor(bucketId).AsSucceeded());
            }
            catch (Exception ex)
            {
                _tracer.Error(ex, "Error when import facts for ERM");
                return processingResultsMap.Keys.Select(bucketId => MessageProcessingStage.Handling.ResultFor(bucketId).AsFailed().WithExceptions(ex));
            }
        }

        private IEnumerable<IEvent> Handle(IReadOnlyCollection<IncrementErmStateCommand> commands)
        {
            if (!commands.Any())
            {
                return Array.Empty<IEvent>();
            }

            var eldestEventTime = commands.SelectMany(x => x.States).Min(x => x.UtcDateTime);
            var delta = DateTime.UtcNow - eldestEventTime;
            _telemetryPublisher.Delay((int)delta.TotalMilliseconds);

            return new IEvent[]
            {
                new ErmStateIncrementedEvent(commands.SelectMany(x => x.States)),
                new DelayLoggedEvent(DateTime.UtcNow)
            };
        }

        private IEnumerable<IEvent> Handle(IReadOnlyCollection<ISyncDataObjectCommand> commands)
        {
            if (!commands.Any())
            {
                return Array.Empty<IEvent>();
            }

            var actors = _dataObjectsActorFactory.Create(new HashSet<Type>(commands.Select(x => x.DataObjectType)));
            var events = new HashSet<IEvent>(EqualityComparer);

            foreach (var actor in actors)
            {
                var actorType = actor.GetType().GetFriendlyName();
                using (Probe.Create($"ETL1 {actorType}"))
                {
                    events.UnionWith(actor.ExecuteCommands(commands));
                }
            }

            _syncEntityNameActor.ExecuteCommands(commands);

            return events;
        }

        private sealed class EventEqualityComparer : IEqualityComparer<IEvent>
        {
            public bool Equals(IEvent x, IEvent y)
            {
                switch (x)
                {
                    case DataObjectCreatedEvent dataObjectCreatedEventX:
                        return y is DataObjectCreatedEvent dataObjectCreatedEventY && DataObjectCreatedEvent.Comparer.Equals(dataObjectCreatedEventX, dataObjectCreatedEventY);
                    case DataObjectDeletedEvent dataObjectDeletedEventX:
                        return y is DataObjectDeletedEvent dataObjectDeletedEventY && DataObjectDeletedEvent.Comparer.Equals(dataObjectDeletedEventX, dataObjectDeletedEventY);
                    case DataObjectUpdatedEvent dataObjectUpdatedEventX:
                        return y is DataObjectUpdatedEvent dataObjectUpdatedEventY && DataObjectUpdatedEvent.Comparer.Equals(dataObjectUpdatedEventX, dataObjectUpdatedEventY);
                    case RelatedDataObjectOutdatedEvent<long> relatedDataObjectOutdatedEventLongX:
                        return y is RelatedDataObjectOutdatedEvent<long> relatedDataObjectOutdatedEventLongY && RelatedDataObjectOutdatedEvent<long>.Comparer.Equals(relatedDataObjectOutdatedEventLongX, relatedDataObjectOutdatedEventLongY);
                    case RelatedDataObjectOutdatedEvent<PeriodKey> relatedDataObjectOutdatedEventPeriodKeyX:
                        return y is RelatedDataObjectOutdatedEvent<PeriodKey> relatedDataObjectOutdatedEventPeriodKeyY && RelatedDataObjectOutdatedEvent<PeriodKey>.Comparer.Equals(relatedDataObjectOutdatedEventPeriodKeyX, relatedDataObjectOutdatedEventPeriodKeyY);
                    default:
                        throw new ArgumentOutOfRangeException(nameof(x));
                }
            }

            public int GetHashCode(IEvent obj)
            {
                switch (obj)
                {
                    case DataObjectCreatedEvent dataObjectCreatedEvent:
                        return DataObjectCreatedEvent.Comparer.GetHashCode(dataObjectCreatedEvent);
                    case DataObjectDeletedEvent dataObjectDeletedEvent:
                        return DataObjectDeletedEvent.Comparer.GetHashCode(dataObjectDeletedEvent);
                    case DataObjectUpdatedEvent dataObjectUpdatedEvent:
                        return DataObjectUpdatedEvent.Comparer.GetHashCode(dataObjectUpdatedEvent);
                    case RelatedDataObjectOutdatedEvent<long> relatedDataObjectOutdatedEventLong:
                        return RelatedDataObjectOutdatedEvent<long>.Comparer.GetHashCode(relatedDataObjectOutdatedEventLong);
                    case RelatedDataObjectOutdatedEvent<PeriodKey> relatedDataObjectOutdatedEventPeriodKey:
                        return RelatedDataObjectOutdatedEvent<PeriodKey>.Comparer.GetHashCode(relatedDataObjectOutdatedEventPeriodKey);
                    default:
                        throw new ArgumentOutOfRangeException(nameof(obj));
                }
            }
        }
    }
}
