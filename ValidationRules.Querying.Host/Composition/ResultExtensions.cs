﻿using System;
using System.Collections.Generic;
using System.Globalization;

using NuClear.ValidationRules.Storage.Model.ConsistencyRules.Aggregates;
using NuClear.ValidationRules.Storage.Model.FirmRules.Aggregates;

using Order = NuClear.ValidationRules.Storage.Model.AdvertisementRules.Aggregates.Order;

namespace NuClear.ValidationRules.Querying.Host.Composition
{
    public static class ResultExtensions
    {
        public static AccountBalanceMessageDto ReadAccountBalanceMessage(this IReadOnlyDictionary<string, string> message)
        {
            return new AccountBalanceMessageDto
                {
                    Available = decimal.Parse(message["available"], CultureInfo.InvariantCulture),
                    Planned = decimal.Parse(message["planned"], CultureInfo.InvariantCulture),
                };
        }

        public static AdvertisementCountDto ReadAdvertisementCountMessage(this IReadOnlyDictionary<string, string> message)
        {
            return new AdvertisementCountDto
            {
                Min = int.Parse(message["min"], CultureInfo.InvariantCulture),
                Max = int.Parse(message["max"], CultureInfo.InvariantCulture),
                Count = int.Parse(message["count"], CultureInfo.InvariantCulture),
                Name = message["name"],
                Begin = DateTime.Parse(message["begin"], CultureInfo.InvariantCulture),
                End = DateTime.Parse(message["end"], CultureInfo.InvariantCulture),
            };
        }

        public static OversalesDto ReadOversalesMessage(this IReadOnlyDictionary<string, string> message)
        {
            return new OversalesDto
                {
                    Max = int.Parse(message["max"], CultureInfo.InvariantCulture),
                    Count = int.Parse(message["count"], CultureInfo.InvariantCulture),
                };
        }

        public static InvalidFirmAddressState ReadFirmAddressState(this IReadOnlyDictionary<string, string> message)
        {
            return (InvalidFirmAddressState)int.Parse(message["invalidFirmAddressState"], CultureInfo.InvariantCulture);
        }

        public static CategoryCountDto ReadCategoryCount(this IReadOnlyDictionary<string, string> message)
        {
            return new CategoryCountDto
            {
                Actual = int.Parse(message["count"], CultureInfo.InvariantCulture),
                Allowed = int.Parse(message["allowed"], CultureInfo.InvariantCulture),
            };
        }

        public static InvalidFirmState ReadFirmState(this IReadOnlyDictionary<string, string> message)
        {
            return (InvalidFirmState)int.Parse(message["invalidFirmState"], CultureInfo.InvariantCulture);
        }

        public static OrderRequiredFieldsDto ReadOrderRequiredFieldsMessage(this IReadOnlyDictionary<string, string> message)
        {
            return new OrderRequiredFieldsDto
            {
                LegalPerson = bool.Parse(message["legalPerson"]),
                LegalPersonProfile = bool.Parse(message["legalPersonProfile"]),
                BranchOfficeOrganizationUnit = bool.Parse(message["branchOfficeOrganizationUnit"]),
                Currency = bool.Parse(message["currency"]),
            };
        }

        public static OrderInactiveFieldsDto ReadOrderInactiveFieldsMessage(this IReadOnlyDictionary<string, string> message)
        {
            return new OrderInactiveFieldsDto
            {
                LegalPerson = bool.Parse(message["legalPerson"]),
                LegalPersonProfile = bool.Parse(message["legalPersonProfile"]),
                BranchOfficeOrganizationUnit = bool.Parse(message["branchOfficeOrganizationUnit"]),
                BranchOffice = bool.Parse(message["branchOffice"]),
            };
        }

        public static int ReadProjectThemeCount(this IReadOnlyDictionary<string, string> message)
        {
            return int.Parse(message["themeCount"], CultureInfo.InvariantCulture);
        }

        public static DealState ReadDealState(this IReadOnlyDictionary<string, string> message)
        {
            return (DealState)int.Parse(message["state"]);
        }

        public static DateTime ReadBeginDate(this IReadOnlyDictionary<string, string> message)
        {
            return DateTime.Parse(message["begin"]);
        }

        public static Order.AdvertisementReviewState ReadAdvertisementReviewState(this IReadOnlyDictionary<string, string> message)
        {
            return (Order.AdvertisementReviewState)int.Parse(message["reviewState"], CultureInfo.InvariantCulture);
        }

        public sealed class CategoryCountDto
        {
            public int Allowed { get; set; }
            public int Actual { get; set; }
        }

        public sealed class AccountBalanceMessageDto
        {
            public decimal Available { get; set; }
            public decimal Planned { get; set; }
        }

        public sealed class AdvertisementCountDto
        {
            public int Min { get; set; }
            public int Max { get; set; }
            public int Count { get; set; }
            public string Name { get; set; }
            public DateTime Begin { get; set; }
            public DateTime End { get; set; }
        }

        public sealed class OversalesDto
        {
            public int Max { get; set; }
            public int Count { get; set; }
        }

        public sealed class OrderInactiveFieldsDto
        {
            public bool LegalPerson { get; set; }
            public bool LegalPersonProfile { get; set; }
            public bool BranchOfficeOrganizationUnit { get; set; }
            public bool BranchOffice { get; set; }
        }

        public sealed class OrderRequiredFieldsDto
        {
            public bool LegalPerson { get; set; }
            public bool LegalPersonProfile { get; set; }
            public bool BranchOfficeOrganizationUnit { get; set; }
            public bool Currency { get; set; }
        }
    }
}