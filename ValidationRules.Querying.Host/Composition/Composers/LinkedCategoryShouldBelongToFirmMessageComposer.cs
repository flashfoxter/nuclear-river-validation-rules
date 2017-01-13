﻿using NuClear.ValidationRules.Querying.Host.Properties;
using NuClear.ValidationRules.Storage.Model.Messages;

namespace NuClear.ValidationRules.Querying.Host.Composition.Composers
{
    public sealed class LinkedCategoryShouldBelongToFirmMessageComposer : IMessageComposer
    {
        public MessageTypeCode MessageType => MessageTypeCode.LinkedCategoryShouldBelongToFirm;

        public MessageComposerResult Compose(Version.ValidationResult validationResult)
        {
            var orderReference = validationResult.ReadOrderReference();
            var orderPositionReference = validationResult.ReadOrderPositionReference();
            var categoryReference = validationResult.ReadCategoryReference();

            return new MessageComposerResult(
                orderReference,
                Resources.OrderPositionCategoryNotBelongsToFirm,
                orderPositionReference,
                categoryReference);
        }
    }
}