﻿using System;

using NuClear.Messaging.API.Flows.Metadata;
using NuClear.Messaging.DI.Factories.Unity.Transformers.Resolvers;
using NuClear.OperationsProcessing.Transports.ServiceBus.Primary;
using NuClear.Replication.OperationsProcessing.Transports.ServiceBus;
using NuClear.ValidationRules.OperationsProcessing.Identities.Flows;

namespace NuClear.ValidationRules.Replication.Host.Factories.Messaging.Transformer
{
    //todo: по аналогии с receiver вынести в метаданные
    public sealed class PrimaryMessageTransformerResolveStrategy : IMessageTransformerResolveStrategy
    {
        public bool TryGetAppropriateTransformer(MessageFlowMetadata messageFlowMetadata, out Type resolvedFlowReceiverType)
        {
            var messageFlow = messageFlowMetadata.MessageFlow;

            if (messageFlow.Equals(ImportFactsFromErmFlow.Instance))
            {
                resolvedFlowReceiverType = typeof(BinaryEntireBrokeredMessage2TrackedUseCaseTransformer);
                return true;
            }

            if (messageFlow.Equals(CommonEventsFlow.Instance))
            {
                resolvedFlowReceiverType = typeof(BrokeredMessageWithXmlPayload2EventMessageTransformer);
                return true;
            }

            if (messageFlow.Equals(MessagesFlow.Instance))
            {
                resolvedFlowReceiverType = typeof(BrokeredMessageWithXmlPayload2EventMessageTransformer);
                return true;
            }

            resolvedFlowReceiverType = null;
            return false;
        }
    }
}
