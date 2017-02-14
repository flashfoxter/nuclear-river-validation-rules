﻿using System.Collections.Generic;
using System.Linq;

using NuClear.ValidationRules.Querying.Host.DataAccess;
using NuClear.ValidationRules.Querying.Host.Model;
using NuClear.ValidationRules.Querying.Host.Properties;
using NuClear.ValidationRules.Storage.Identitites.EntityTypes;
using NuClear.ValidationRules.Storage.Model.Messages;

namespace NuClear.ValidationRules.Querying.Host.Composition.Composers
{
    public sealed class FirmShouldHaveLimitedCategoryCountMessageComposer : IMessageComposer, IDistinctor
    {
        public MessageTypeCode MessageType => MessageTypeCode.FirmShouldHaveLimitedCategoryCount;

        public MessageComposerResult Compose(NamedReference[] references, IReadOnlyDictionary<string, string> extra)
        {
            var orderReference = references.Get<EntityTypeOrder>();
            var firmReference = references.Get<EntityTypeFirm>();
            var categoryCount = extra.ReadCategoryCount();

            return new MessageComposerResult(
                orderReference,
                string.Format(Resources.TooManyCategorieForFirm, categoryCount.Actual, categoryCount.Allowed),
                firmReference);
        }

        // Может быть несколько разных сообщений
        public IEnumerable<Message> Distinct(IEnumerable<Message> messages)
            => messages.GroupBy(result => result.OrderId)
                      .Select(group => group.OrderByDescending(result => result.Extra.ReadCategoryCount().Actual).First());
    }
}