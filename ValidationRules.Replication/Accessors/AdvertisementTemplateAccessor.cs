﻿using System;
using System.Collections.Generic;
using System.Linq;

using NuClear.Replication.Core;
using NuClear.Replication.Core.DataObjects;
using NuClear.Replication.Core.Specs;
using NuClear.Storage.API.Readings;
using NuClear.Storage.API.Specifications;
using NuClear.ValidationRules.Replication.Commands;
using NuClear.ValidationRules.Storage.Model.Facts;

using Erm = NuClear.ValidationRules.Storage.Model.Erm;

namespace NuClear.ValidationRules.Replication.Accessors
{
    public sealed class AdvertisementTemplateAccessor : IStorageBasedDataObjectAccessor<AdvertisementTemplate>, IDataChangesHandler<AdvertisementTemplate>
    {
        private readonly IQuery _query;

        public AdvertisementTemplateAccessor(IQuery query)
        {
            _query = query;
        }

        public IQueryable<AdvertisementTemplate> GetSource()
            => Array.Empty<AdvertisementTemplate>().AsQueryable();

        public FindSpecification<AdvertisementTemplate> GetFindSpecification(IReadOnlyCollection<ICommand> commands)
        {
            var ids = commands.Cast<SyncDataObjectCommand>().Select(c => c.DataObjectId).ToList();
            return SpecificationFactory<AdvertisementTemplate>.Contains(x => x.Id, ids);
        }

        public IReadOnlyCollection<IEvent> HandleCreates(IReadOnlyCollection<AdvertisementTemplate> dataObjects) => Array.Empty<IEvent>();

        public IReadOnlyCollection<IEvent> HandleUpdates(IReadOnlyCollection<AdvertisementTemplate> dataObjects) => Array.Empty<IEvent>();

        public IReadOnlyCollection<IEvent> HandleDeletes(IReadOnlyCollection<AdvertisementTemplate> dataObjects) => Array.Empty<IEvent>();

        public IReadOnlyCollection<IEvent> HandleRelates(IReadOnlyCollection<AdvertisementTemplate> dataObjects)
        {
            var dataObjectIds = dataObjects.Select(x => x.Id).ToList();

            var advertisementIds = from advertisement in _query.For<Advertisement>().Where(x => dataObjectIds.Contains(x.AdvertisementTemplateId))
                                   select advertisement.Id;

            // позиция номенклатуры и шаблон РМ является константой для заказа, агрегат Order не пересчитываем
            return new EventCollectionHelper<AdvertisementTemplate> { { typeof(Advertisement), advertisementIds } };
        }
    }
}