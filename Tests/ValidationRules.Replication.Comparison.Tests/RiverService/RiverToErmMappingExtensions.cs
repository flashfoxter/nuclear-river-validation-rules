﻿using System.Collections.Generic;
using System.Linq;

using NuClear.ValidationRules.Storage.Model.Messages;

namespace ValidationRules.Replication.Comparison.Tests.RiverService
{
    public static class RiverToErmMappingExtensions
    {
        private static readonly IReadOnlyDictionary<MessageTypeCode, int> RiverToErmRuleCodeMapping
            = new Dictionary<MessageTypeCode, int>
                    {
                            { MessageTypeCode.AdvertisementAmountShouldMeetMaximumRestrictions, 26 },
                            { MessageTypeCode.OrderMustHaveActualPrice, 15 },
                            { MessageTypeCode.OrderPositionCorrespontToInactivePosition, 15 },
                            { MessageTypeCode.OrderPositionMayCorrespontToActualPrice, 15 },
                            { MessageTypeCode.OrderPositionMustCorrespontToActualPrice, 15 },
                            { MessageTypeCode.AdvertisementAmountShouldMeetMinimumRestrictions, 26 },
                            { MessageTypeCode.AdvertisementAmountShouldMeetMinimumRestrictionsMass, 26 },
                            { MessageTypeCode.FirmPositionMustNotHaveDeniedPositions, 6 },
                            { MessageTypeCode.FirmAssociatedPositionMustHavePrincipal, 6 },
                            { MessageTypeCode.FirmAssociatedPositionMustHavePrincipalWithMatchedBindingObject, 6 },
                            { MessageTypeCode.FirmAssociatedPositionMustHavePrincipalWithDifferentBindingObject, 6 },
                            { MessageTypeCode.FirmAssociatedPositionShouldNotStayAlone, 6 },
                            { MessageTypeCode.AdvertisementCountPerThemeShouldBeLimited, 44 },
                            { MessageTypeCode.AdvertisementCountPerCategoryShouldBeLimited, 31 },
                            { MessageTypeCode.AccountShouldExist, 3 },
                            { MessageTypeCode.AccountBalanceShouldBePositive, 20 },
                            { MessageTypeCode.OrderBeginDistrubutionShouldBeFirstDayOfMonth, 9 },
                            { MessageTypeCode.OrderEndDistrubutionShouldBeLastSecondOfMonth, 9 },
                            { MessageTypeCode.LegalPersonProfileBargainShouldNotBeExpired, 25 },
                            { MessageTypeCode.LegalPersonProfileWarrantyShouldNotBeExpired, 24 },
                            { MessageTypeCode.OrderShouldNotBeSignedBeforeBargain, 1 },
                            { MessageTypeCode.LegalPersonShouldHaveAtLeastOneProfile, 23 },
                            { MessageTypeCode.OrderShouldHaveAtLeastOnePosition, 14 },
                            { MessageTypeCode.OrderScanShouldPresent, 16 },
                            { MessageTypeCode.BargainScanShouldPresent, 16 },
                            { MessageTypeCode.OrderRequiredFieldsShouldBeSpecified, 18 },
                            { MessageTypeCode.LinkedFirmAddressShouldBeValid, 12 },
                            { MessageTypeCode.LinkedCategoryFirmAddressShouldBeValid, 12 },
                            { MessageTypeCode.LinkedCategoryShouldBelongToFirm, 12 },
                            { MessageTypeCode.LinkedCategoryAsterixMayBelongToFirm, 12 },
                            { MessageTypeCode.LinkedCategoryShouldBeActive, 12 },
                            { MessageTypeCode.LinkedFirmShouldBeValid, 11 },
                            { MessageTypeCode.BillsSumShouldMatchOrder, 7 },
                            { MessageTypeCode.BillsShouldBeCreated, 7 },
                            { MessageTypeCode.FirmAndOrderShouldBelongTheSameOrganizationUnit, 10 },
                            { MessageTypeCode.FirmShouldHaveLimitedCategoryCount, 32 },
                            { MessageTypeCode.OrderPositionAdvertisementMustBeCreated, 22 },
                            { MessageTypeCode.OrderPositionAdvertisementMustHaveAdvertisement, 22 },
                            { MessageTypeCode.OrderPositionAdvertisementMustHaveOptionalAdvertisement, 0 },
                            { MessageTypeCode.OrderMustUseCategoriesOnlyAvailableInProject, 8 },
                            { MessageTypeCode.OrderMustNotIncludeReleasedPeriod, 17 },
                            { MessageTypeCode.OrderPositionCostPerClickMustNotBeLessMinimum, 48 },
                            { MessageTypeCode.FirmAddressMustBeLocatedOnTheMap, 34 },
                            { MessageTypeCode.ThemeCategoryMustBeActiveAndNotDeleted, 43 },
                            { MessageTypeCode.ThemePeriodMustContainOrderPeriod, 42 },
                            { MessageTypeCode.DefaultThemeMustHaveOnlySelfAds, 41 },
                            { MessageTypeCode.DefaultThemeMustBeExactlyOne, 40 },
                            { MessageTypeCode.OrderPositionCostPerClickMustBeSpecified, 46 },
                            { MessageTypeCode.OrderMustHaveActiveDeal, 51 },
                            { MessageTypeCode.OrderMustHaveActiveLegalEntities, 52 },
                            { MessageTypeCode.AdvantageousPurchasesBannerMustBeSoldInTheSameCategory, 38 },
                            { MessageTypeCode.OrderPositionSalesModelMustMatchCategorySalesModel, 0 },
                            { MessageTypeCode.ProjectMustContainCostPerClickMinimumRestriction, 0 },
                            { MessageTypeCode.AdvertisementMustBelongToFirm, 0 },
                            { MessageTypeCode.AdvertisementMustPassReview, 0 },
                            { MessageTypeCode.AdvertisementShouldNotHaveComments, 0 },
                            { MessageTypeCode.OptionalAdvertisementMustPassReview, 0 },
                            { MessageTypeCode.FirmAddressMustNotHaveMultiplePremiumPartnerAdvertisement, 0 },
                            { MessageTypeCode.FirmAddressShouldNotHaveMultiplePartnerAdvertisement, 0 },
                            { MessageTypeCode.PartnerAdvertisementShouldNotBeSoldToAdvertiser, 0 },
                            { MessageTypeCode.PartnerAdvertisementMustNotCauseProblemsToTheAdvertiser, 0 },
                            { MessageTypeCode.AmsMessagesShouldBeNew, 0 },
                    }.ToDictionary(x => x.Key, x => x.Value);

        public static int ToErmRuleCode(this int riverMessageTypeCode)
            => RiverToErmRuleCodeMapping[(MessageTypeCode)riverMessageTypeCode];

        public static int CoerceFromErmRuleCode(this int ermMessageTypeCode)
        {
            const int RiverRuleCodesOffsetUsedByErmService = 1000;
            return ermMessageTypeCode - RiverRuleCodesOffsetUsedByErmService;
        }
    }
}