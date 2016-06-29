﻿using System;

using NuClear.Messaging.API.Flows;
using NuClear.Messaging.Transports.ServiceBus.API;
using NuClear.Replication.OperationsProcessing.Transports.ServiceBus.Factories;
using NuClear.River.Hosting.Common.Identities.Connections;
using NuClear.Settings;
using NuClear.Settings.API;
using NuClear.Storage.API.ConnectionStrings;
using NuClear.ValidationRules.OperationsProcessing.Identities.Flows;

namespace NuClear.ValidationRules.Replication.Host.Factories
{
    public sealed class ServiceBusSettingsFactory : IServiceBusSettingsFactory
    {
        private readonly string _serviceBusConnectionString;

        private readonly StringSetting _ermOperationsFlowTopic = ConfigFileSetting.String.Optional("ErmEventsFlowTopic", "topic.performedoperations");
        private readonly StringSetting _commonEventsFlowTopic = ConfigFileSetting.String.Optional("CommonEventsFlowTopic", "topic.river.validationrules.price.common");
        private readonly StringSetting _mesageEventsFlowTopic = ConfigFileSetting.String.Optional("MessageEventsFlowTopic", "topic.river.validationrules.price.messages");

        public ServiceBusSettingsFactory(IConnectionStringSettings connectionStringSettings)
        {
            _serviceBusConnectionString = connectionStringSettings.GetConnectionString(ServiceBusConnectionStringIdentity.Instance);
        }

        public IServiceBusMessageReceiverSettings CreateReceiverSettings(IMessageFlow messageFlow)
        {
            if (messageFlow.Id == ImportFactsFromErmFlow.Instance.Id)
                return new Settings
                {
                    ConnectionString = _serviceBusConnectionString,
                    TransportEntityPath = _ermOperationsFlowTopic.Value,
                };

            if (messageFlow.Id == CommonEventsFlow.Instance.Id)
                return new Settings
                {
                    ConnectionString = _serviceBusConnectionString,
                    TransportEntityPath = _commonEventsFlowTopic.Value,
                };

            if (messageFlow.Id == MessagesFlow.Instance.Id)
                return new Settings
                {
                    ConnectionString = _serviceBusConnectionString,
                    TransportEntityPath = _mesageEventsFlowTopic.Value,
                };

            throw new ArgumentException($"Flow '{messageFlow.Description}' settings for MS ServiceBus are undefined");
        }

        public IServiceBusMessageSenderSettings CreateSenderSettings(IMessageFlow messageFlow)
        {
            if (messageFlow.Id == CommonEventsFlow.Instance.Id)
                return new Settings
                {
                    ConnectionString = _serviceBusConnectionString,
                    TransportEntityPath = _commonEventsFlowTopic.Value,
                };

            if (messageFlow.Id == MessagesFlow.Instance.Id)
                return new Settings
                {
                    ConnectionString = _serviceBusConnectionString,
                    TransportEntityPath = _mesageEventsFlowTopic.Value,
                };

            throw new ArgumentException($"Flow '{messageFlow.Description}' settings for MS ServiceBus are undefined");
        }

        private class Settings : IServiceBusMessageReceiverSettings, IServiceBusMessageSenderSettings
        {
            public string TransportEntityPath { get; set; }
            public string ConnectionString { get; set; }
            public int ConnectionsCount { get; } = 1;
            public bool UseTransactions { get; } = true;
        }
    }
}