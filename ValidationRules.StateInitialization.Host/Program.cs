﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Confluent.Kafka;

using NuClear.Assembling.TypeProcessing;
using NuClear.Messaging.API.Flows;
using NuClear.Replication.Core;
using NuClear.River.Hosting.Common.Settings;
using NuClear.StateInitialization.Core.Actors;
using NuClear.Storage.API.ConnectionStrings;
using NuClear.Tracing.API;
using NuClear.ValidationRules.OperationsProcessing.AmsFactsFlow;
using NuClear.ValidationRules.OperationsProcessing.RulesetFactsFlow;
using NuClear.ValidationRules.StateInitialization.Host.Assembling;
using NuClear.ValidationRules.StateInitialization.Host.Kafka;
using NuClear.ValidationRules.StateInitialization.Host.Kafka.Ams;
using NuClear.ValidationRules.StateInitialization.Host.Kafka.Rulesets;
using NuClear.ValidationRules.Storage.Connections;

using ValidationRules.Hosting.Common;
using ValidationRules.Hosting.Common.Settings.Connections;
using ValidationRules.Hosting.Common.Settings.Kafka;

namespace NuClear.ValidationRules.StateInitialization.Host
{
    public sealed class Program
    {
        public static void Main(string[] args)
        {
            StateInitializationRoot.Instance.PerformTypesMassProcessing(Array.Empty<IMassProcessor>(), true, typeof(object));

            var commands = new List<ICommand>();

            if (args.Contains("-facts"))
            {
                commands.Add(BulkReplicationCommands.ErmToFacts);
                // Надо подумать о лишней обёртке
                commands.Add(new KafkaReplicationCommand(AmsFactsFlow.Instance, BulkReplicationCommands.AmsToFacts));
                commands.Add(new KafkaReplicationCommand(RulesetFactsFlow.Instance, BulkReplicationCommands.RulesetsToFacts));
                commands.Add(SchemaInitializationCommands.WebApp);
                commands.Add(SchemaInitializationCommands.Facts);
            }

            if (args.Contains("-aggregates"))
            {
                commands.Add(BulkReplicationCommands.FactsToAggregates);
                commands.Add(SchemaInitializationCommands.WebApp);
                commands.Add(SchemaInitializationCommands.Aggregates);
            }

            if (args.Contains("-messages"))
            {
                commands.Add(BulkReplicationCommands.AggregatesToMessages);
                commands.Add(SchemaInitializationCommands.WebApp);
                commands.Add(SchemaInitializationCommands.Messages);
            }

            var connectionStrings = ConnectionStrings.For(ErmConnectionStringIdentity.Instance,
                                                          AmsConnectionStringIdentity.Instance,
                                                          FactsConnectionStringIdentity.Instance,
                                                          AggregatesConnectionStringIdentity.Instance,
                                                          MessagesConnectionStringIdentity.Instance,
                                                          RulesetConnectionStringIdentity.Instance);
            var connectionStringSettings = new ConnectionStringSettingsAspect(connectionStrings);
            var environmentSettings = new EnvironmentSettingsAspect();

            var kafkaSettingsFactory =
                new KafkaSettingsFactory(new Dictionary<IMessageFlow, string>
                                             {
                                                 [AmsFactsFlow.Instance] =
                                                     connectionStringSettings.GetConnectionString(AmsConnectionStringIdentity.Instance),
                                                 [RulesetFactsFlow.Instance] =
                                                     connectionStringSettings.GetConnectionString(RulesetConnectionStringIdentity.Instance)
                                             },
                                         environmentSettings,
                                         Offset.Beginning);

            var kafkaMessageFlowReceiverFactory = new KafkaMessageFlowReceiverFactory(new NullTracer(), kafkaSettingsFactory);

            var dataObjectTypesProviderFactory = new DataObjectTypesProviderFactory();
            var bulkReplicationActor = new BulkReplicationActor(dataObjectTypesProviderFactory, connectionStringSettings);
            var kafkaReplicationActor = new KafkaReplicationActor(connectionStringSettings,
                                                                  dataObjectTypesProviderFactory,
                                                                  kafkaMessageFlowReceiverFactory,
                                                                  new IBulkCommandFactory<Message>[]
                                                                      {
                                                                          new AmsFactsBulkCommandFactory(),
                                                                          new RulesetFactsBulkCommandFactory(environmentSettings)
                                                                      });

            var schemaInitializationActor = new SchemaInitializationActor(connectionStringSettings);

            var sw = Stopwatch.StartNew();
            schemaInitializationActor.ExecuteCommands(commands);
            bulkReplicationActor.ExecuteCommands(commands.Where(x => x == BulkReplicationCommands.ErmToFacts).ToList());
            kafkaReplicationActor.ExecuteCommands(commands);
            bulkReplicationActor.ExecuteCommands(commands.Where(x => x != BulkReplicationCommands.ErmToFacts).ToList());

            Console.WriteLine($"Total time: {sw.ElapsedMilliseconds}ms");
        }
    }
}
