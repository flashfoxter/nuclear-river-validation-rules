﻿using System.Collections.Generic;

using NuClear.ValidationRules.Querying.Host.Model;
using NuClear.ValidationRules.Querying.Host.Properties;
using NuClear.ValidationRules.Storage.Identitites.EntityTypes;
using NuClear.ValidationRules.Storage.Model.Messages;

namespace NuClear.ValidationRules.Querying.Host.Composition.Composers
{
    public sealed class AdvertisementCountPerThemeShouldBeLimitedMessageComposer : IMessageComposer
    {
        public MessageTypeCode MessageType => MessageTypeCode.AdvertisementCountPerThemeShouldBeLimited;

        public MessageComposerResult Compose(NamedReference[] references, IReadOnlyDictionary<string, string> extra)
        {
            var orderReference = references.Get<EntityTypeOrder>();
            var themeReference = references.Get<EntityTypeTheme>();
            var dto = extra.ReadOversalesMessage();

            return new MessageComposerResult(
                orderReference,
                string.Format(Resources.ThemeSalesExceedsLimit, dto.Count, dto.Max),
                themeReference);
        }
    }
}