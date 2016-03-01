﻿using LinqToDB.DataProvider.SqlServer;
using LinqToDB.Mapping;

using NuClear.ValidationRules.Domain.Model.Erm;

namespace NuClear.ValidationRules.Storage
{
    public static partial class Schema
    {
        private const string BillingSchema = "Billing";
        private const string BusinessDirectorySchema = "BusinessDirectory";

        public static MappingSchema Erm
        {
            get
            {
                var schema = new MappingSchema(new SqlServerMappingSchema());
                var config = schema.GetFluentMappingBuilder();

                config.Entity<AssociatedPositionsGroup>().HasSchemaName(BillingSchema).HasTableName("AssociatedPositionsGroups").HasPrimaryKey(x => x.Id);
                config.Entity<AssociatedPosition>().HasSchemaName(BillingSchema).HasTableName("AssociatedPositions").HasPrimaryKey(x => x.Id);
                config.Entity<DeniedPosition>().HasSchemaName(BillingSchema).HasTableName("DeniedPositions").HasPrimaryKey(x => x.Id);
                config.Entity<GlobalAssociatedPosition>().HasSchemaName(BillingSchema).HasTableName("GlobalAssociatedPositions").HasPrimaryKey(x => x.Id);
                config.Entity<GlobalDeniedPosition>().HasSchemaName(BillingSchema).HasTableName("GlobalDeniedPositions").HasPrimaryKey(x => x.Id);
                config.Entity<Order>().HasSchemaName(BillingSchema).HasTableName("Orders").HasPrimaryKey(x => x.Id);
                config.Entity<OrderPosition>().HasSchemaName(BillingSchema).HasTableName("OrderPositions").HasPrimaryKey(x => x.Id);
                config.Entity<OrderPositionAdvertisement>().HasSchemaName(BillingSchema).HasTableName("OrderPositionAdvertisement").HasPrimaryKey(x => x.Id);
                config.Entity<OrganizationUnit>().HasSchemaName(BillingSchema).HasTableName("OrganizationUnits").HasPrimaryKey(x => x.Id);
                config.Entity<Price>().HasSchemaName(BillingSchema).HasTableName("Prices").HasPrimaryKey(x => x.Id);
                config.Entity<PricePosition>().HasSchemaName(BillingSchema).HasTableName("PricePositions").HasPrimaryKey(x => x.Id);
                config.Entity<Project>().HasSchemaName(BillingSchema).HasTableName("Projects").HasPrimaryKey(x => x.Id);
                config.Entity<Position>().HasSchemaName(BillingSchema).HasTableName("Positions").HasPrimaryKey(x => x.Id);
                config.Entity<Category>().HasSchemaName(BusinessDirectorySchema).HasTableName("Categories").HasPrimaryKey(x => x.Id);

                return schema;
            }
        }
    }
}
