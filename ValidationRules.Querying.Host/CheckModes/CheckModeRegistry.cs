﻿using System;
using System.Collections.Generic;
using System.Linq;

using NuClear.ValidationRules.Storage.Model.Messages;

namespace NuClear.ValidationRules.Querying.Host.CheckModes
{
    // Если хочешь включить single-режим у проверки, то проверь загружены ли в ErmDataLoader все нужные данные
    internal static class CheckModeRegistry
    {
        public static readonly IReadOnlyCollection<Tuple<MessageTypeCode, IReadOnlyDictionary<CheckMode, RuleSeverityLevel>>> Map =
            new[]
                {
                    Rule(MessageTypeCode.AccountBalanceShouldBePositive,
                         manualReportWithAccount: RuleSeverityLevel.Error,
                         release: RuleSeverityLevel.Error),

                    Rule(MessageTypeCode.AccountShouldExist,
                         prerelease: RuleSeverityLevel.Error,
                         release: RuleSeverityLevel.Error),

                    Rule(MessageTypeCode.OrderPositionAdvertisementMustBeCreated,
                         single: RuleSeverityLevel.Error,
                         manualReport: RuleSeverityLevel.Error,
                         prerelease: RuleSeverityLevel.Error,
                         release: RuleSeverityLevel.Error),

                    Rule(MessageTypeCode.OrderPositionAdvertisementMustHaveAdvertisement,
                         single: RuleSeverityLevel.Warning,
                         manualReport: RuleSeverityLevel.Warning,
                         prerelease: RuleSeverityLevel.Warning,
                         release: RuleSeverityLevel.Error),

                    Rule(MessageTypeCode.OrderPositionAdvertisementMustHaveOptionalAdvertisement,
                         single: RuleSeverityLevel.Info,
                         manualReport: RuleSeverityLevel.Warning,
                         prerelease: RuleSeverityLevel.Warning,
                         release: RuleSeverityLevel.Warning),

                    Rule(MessageTypeCode.AdvantageousPurchasesBannerMustBeSoldInTheSameCategory,
                         single: RuleSeverityLevel.Error,
                         manualReport: RuleSeverityLevel.Error,
                         prerelease: RuleSeverityLevel.Error,
                         release: RuleSeverityLevel.Error),

                    Rule(MessageTypeCode.BargainScanShouldPresent,
                         single: RuleSeverityLevel.Warning),

                    Rule(MessageTypeCode.BillsShouldBeCreated,
                         single: RuleSeverityLevel.Error),

                    Rule(MessageTypeCode.BillsSumShouldMatchOrder,
                         single: RuleSeverityLevel.Error),

                    Rule(MessageTypeCode.LegalPersonProfileBargainShouldNotBeExpired,
                         single: RuleSeverityLevel.Info),

                    Rule(MessageTypeCode.LegalPersonProfileWarrantyShouldNotBeExpired,
                         single: RuleSeverityLevel.Info),

                    Rule(MessageTypeCode.LegalPersonShouldHaveAtLeastOneProfile,
                         single: RuleSeverityLevel.Error),

                    Rule(MessageTypeCode.LinkedCategoryAsterixMayBelongToFirm,
                         single: RuleSeverityLevel.Info,
                         prerelease: RuleSeverityLevel.Info,
                         release: RuleSeverityLevel.Info),

                    Rule(MessageTypeCode.LinkedCategoryFirmAddressShouldBeValid,
                         single: RuleSeverityLevel.Warning,
                         prerelease: RuleSeverityLevel.Error,
                         release: RuleSeverityLevel.Error),

                    Rule(MessageTypeCode.LinkedCategoryShouldBeActive,
                         single: RuleSeverityLevel.Error,
                         prerelease: RuleSeverityLevel.Error,
                         release: RuleSeverityLevel.Error),

                    Rule(MessageTypeCode.LinkedCategoryShouldBelongToFirm,
                         single: RuleSeverityLevel.Warning,
                         prerelease: RuleSeverityLevel.Error,
                         release: RuleSeverityLevel.Error),

                    Rule(MessageTypeCode.LinkedFirmAddressShouldBeValid,
                         single: RuleSeverityLevel.Error,
                         prerelease: RuleSeverityLevel.Error,
                         release: RuleSeverityLevel.Error),

                    Rule(MessageTypeCode.OrderBeginDistrubutionShouldBeFirstDayOfMonth,
                         single: RuleSeverityLevel.Error,
                         prerelease: RuleSeverityLevel.Error,
                         release: RuleSeverityLevel.Error),

                    Rule(MessageTypeCode.OrderEndDistrubutionShouldBeLastSecondOfMonth,
                         single: RuleSeverityLevel.Error,
                         prerelease: RuleSeverityLevel.Error,
                         release: RuleSeverityLevel.Error),

                    Rule(MessageTypeCode.OrderMustHaveActiveDeal,
                         single: RuleSeverityLevel.Error,
                         manualReport: RuleSeverityLevel.Error,
                         prerelease: RuleSeverityLevel.Warning,
                         release: RuleSeverityLevel.Warning),

                    Rule(MessageTypeCode.OrderMustHaveActiveLegalEntities,
                         single: RuleSeverityLevel.Error),

                    Rule(MessageTypeCode.OrderRequiredFieldsShouldBeSpecified,
                         single: RuleSeverityLevel.Error,
                         prerelease: RuleSeverityLevel.Error,
                         release: RuleSeverityLevel.Error),

                    Rule(MessageTypeCode.OrderScanShouldPresent,
                         single: RuleSeverityLevel.Warning),

                    Rule(MessageTypeCode.OrderShouldHaveAtLeastOnePosition,
                         single: RuleSeverityLevel.Error,
                         prerelease: RuleSeverityLevel.Error,
                         release: RuleSeverityLevel.Error),

                    Rule(MessageTypeCode.OrderShouldNotBeSignedBeforeBargain,
                         single: RuleSeverityLevel.Error),

                    Rule(MessageTypeCode.FirmAndOrderShouldBelongTheSameOrganizationUnit,
                         single: RuleSeverityLevel.Error,
                         manualReport: RuleSeverityLevel.Error,
                         prerelease: RuleSeverityLevel.Error,
                         release: RuleSeverityLevel.Error),

                    Rule(MessageTypeCode.FirmShouldHaveLimitedCategoryCount,
                         single: RuleSeverityLevel.Warning,
                         manualReport: RuleSeverityLevel.Warning,
                         prerelease: RuleSeverityLevel.Warning),

                    Rule(MessageTypeCode.LinkedFirmShouldBeValid,
                         single: RuleSeverityLevel.Error,
                         prerelease: RuleSeverityLevel.Error,
                         release: RuleSeverityLevel.Error),

                    Rule(MessageTypeCode.AdvertisementCountPerCategoryShouldBeLimited,
                         single: RuleSeverityLevel.Warning,
                         singleForApprove: RuleSeverityLevel.Error,
                         manualReport: RuleSeverityLevel.Error,
                         prerelease: RuleSeverityLevel.Error,
                         release: RuleSeverityLevel.Error),

                    Rule(MessageTypeCode.AdvertisementCountPerThemeShouldBeLimited,
                         single: RuleSeverityLevel.Error,
                         manualReport: RuleSeverityLevel.Error,
                         prerelease: RuleSeverityLevel.Error,
                         release: RuleSeverityLevel.Error),

                    Rule(MessageTypeCode.FirmAssociatedPositionMustHavePrincipal,
                         single: RuleSeverityLevel.Error,
                         singleForApprove: RuleSeverityLevel.Error,
                         manualReport: RuleSeverityLevel.Error,
                         prerelease: RuleSeverityLevel.Error,
                         release: RuleSeverityLevel.Error),

                    Rule(MessageTypeCode.FirmAssociatedPositionMustHavePrincipalWithDifferentBindingObject,
                         single: RuleSeverityLevel.Error,
                         singleForApprove: RuleSeverityLevel.Error,
                         manualReport: RuleSeverityLevel.Error,
                         prerelease: RuleSeverityLevel.Error,
                         release: RuleSeverityLevel.Error),

                    Rule(MessageTypeCode.FirmAssociatedPositionMustHavePrincipalWithMatchedBindingObject,
                         single: RuleSeverityLevel.Error,
                         singleForApprove: RuleSeverityLevel.Error,
                         manualReport: RuleSeverityLevel.Error,
                         prerelease: RuleSeverityLevel.Error,
                         release: RuleSeverityLevel.Error),

                    Rule(MessageTypeCode.FirmAssociatedPositionShouldNotStayAlone,
                         singleForCancel: RuleSeverityLevel.Warning),

                    Rule(MessageTypeCode.FirmPositionMustNotHaveDeniedPositions,
                         single: RuleSeverityLevel.Error,
                         singleForApprove: RuleSeverityLevel.Error,
                         manualReport: RuleSeverityLevel.Error,
                         prerelease: RuleSeverityLevel.Error,
                         release: RuleSeverityLevel.Error),

                    Rule(MessageTypeCode.AdvertisementAmountShouldMeetMaximumRestrictions,
                         single: RuleSeverityLevel.Error,
                         prerelease: RuleSeverityLevel.Error,
                         release: RuleSeverityLevel.Error),

                    Rule(MessageTypeCode.AdvertisementAmountShouldMeetMinimumRestrictions,
                         single: RuleSeverityLevel.Warning),

                    Rule(MessageTypeCode.AdvertisementAmountShouldMeetMinimumRestrictionsMass,
                         prerelease: RuleSeverityLevel.Warning,
                         release: RuleSeverityLevel.Warning),

                    Rule(MessageTypeCode.OrderPositionCorrespontToInactivePosition,
                         single: RuleSeverityLevel.Error),

                    Rule(MessageTypeCode.OrderPositionMayCorrespontToActualPrice,
                         single: RuleSeverityLevel.Warning),

                    Rule(MessageTypeCode.OrderPositionMustCorrespontToActualPrice,
                         single: RuleSeverityLevel.Error),

                    Rule(MessageTypeCode.OrderMustHaveActualPrice,
                         single: RuleSeverityLevel.Error),

                    Rule(MessageTypeCode.FirmAddressMustBeLocatedOnTheMap,
                         single: RuleSeverityLevel.Error,
                         manualReport: RuleSeverityLevel.Error,
                         prerelease: RuleSeverityLevel.Error,
                         release: RuleSeverityLevel.Error),

                    Rule(MessageTypeCode.OrderMustNotIncludeReleasedPeriod,
                         single: RuleSeverityLevel.Warning),

                    Rule(MessageTypeCode.OrderMustUseCategoriesOnlyAvailableInProject,
                         single: RuleSeverityLevel.Error,
                         prerelease: RuleSeverityLevel.Error,
                         release: RuleSeverityLevel.Error),

                    Rule(MessageTypeCode.OrderPositionCostPerClickMustBeSpecified,
                         single: RuleSeverityLevel.Error,
                         manualReport: RuleSeverityLevel.Error,
                         prerelease: RuleSeverityLevel.Error,
                         release: RuleSeverityLevel.Error),

                    Rule(MessageTypeCode.OrderPositionCostPerClickMustNotBeLessMinimum,
                         single: RuleSeverityLevel.Error,
                         manualReport: RuleSeverityLevel.Error,
                         prerelease: RuleSeverityLevel.Error,
                         release: RuleSeverityLevel.Error),

                    Rule(MessageTypeCode.OrderPositionSalesModelMustMatchCategorySalesModel,
                         single: RuleSeverityLevel.Error,
                         manualReport: RuleSeverityLevel.Error,
                         prerelease: RuleSeverityLevel.Error,
                         release: RuleSeverityLevel.Error),

                    Rule(MessageTypeCode.ProjectMustContainCostPerClickMinimumRestriction,
                         single: RuleSeverityLevel.Error,
                         manualReport: RuleSeverityLevel.Error,
                         prerelease: RuleSeverityLevel.Error,
                         release: RuleSeverityLevel.Error),

                    Rule(MessageTypeCode.DefaultThemeMustBeExactlyOne,
                         manualReport: RuleSeverityLevel.Error,
                         prerelease: RuleSeverityLevel.Error,
                         release: RuleSeverityLevel.Error),

                    Rule(MessageTypeCode.DefaultThemeMustHaveOnlySelfAds,
                         single: RuleSeverityLevel.Error,
                         manualReport: RuleSeverityLevel.Error,
                         prerelease: RuleSeverityLevel.Error,
                         release: RuleSeverityLevel.Error),

                    Rule(MessageTypeCode.ThemeCategoryMustBeActiveAndNotDeleted,
                         manualReport: RuleSeverityLevel.Error,
                         prerelease: RuleSeverityLevel.Error,
                         release: RuleSeverityLevel.Error),

                    Rule(MessageTypeCode.ThemePeriodMustContainOrderPeriod,
                         single: RuleSeverityLevel.Error,
                         manualReport: RuleSeverityLevel.Error,
                         prerelease: RuleSeverityLevel.Error,
                         release: RuleSeverityLevel.Error),

                    Rule(MessageTypeCode.FirmAddressMustNotHaveMultiplePremiumPartnerAdvertisement,
                         single: RuleSeverityLevel.Error,
                         singleForApprove: RuleSeverityLevel.Error,
                         manualReport: RuleSeverityLevel.Error,
                         prerelease: RuleSeverityLevel.Error,
                         release: RuleSeverityLevel.Error),

                    Rule(MessageTypeCode.PartnerAdvertisementMustNotCauseProblemsToTheAdvertiser,
                         single: RuleSeverityLevel.Warning,
                         singleForApprove: RuleSeverityLevel.Warning,
                         manualReport: RuleSeverityLevel.Error,
                         prerelease: RuleSeverityLevel.Error,
                         release: RuleSeverityLevel.Error),

                    Rule(MessageTypeCode.FirmAddressShouldNotHaveMultiplePartnerAdvertisement,
                         manualReport: RuleSeverityLevel.Info,
                         prerelease: RuleSeverityLevel.Info,
                         release: RuleSeverityLevel.Info),

                    Rule(MessageTypeCode.PartnerAdvertisementShouldNotBeSoldToAdvertiser,
                         single: RuleSeverityLevel.Warning,
                         singleForApprove: RuleSeverityLevel.Warning,
                         manualReport: RuleSeverityLevel.Warning,
                         prerelease: RuleSeverityLevel.Warning,
                         release: RuleSeverityLevel.Warning),

                    Rule(MessageTypeCode.AdvertisementMustBelongToFirm,
                         manualReport: RuleSeverityLevel.Error,
                         prerelease: RuleSeverityLevel.Error,
                         release: RuleSeverityLevel.Error),

                    Rule(MessageTypeCode.AdvertisementMustPassReview,
                         manualReport: RuleSeverityLevel.Error,
                         prerelease: RuleSeverityLevel.Error,
                         release: RuleSeverityLevel.Error),

                    Rule(MessageTypeCode.AdvertisementShouldNotHaveComments,
                         manualReport: RuleSeverityLevel.Info,
                         prerelease: RuleSeverityLevel.Info,
                         release: RuleSeverityLevel.Info),

                    Rule(MessageTypeCode.OptionalAdvertisementMustPassReview,
                         manualReport: RuleSeverityLevel.Warning,
                         prerelease: RuleSeverityLevel.Warning,
                         release: RuleSeverityLevel.Warning),

                    Rule(MessageTypeCode.AmsMessagesShouldBeNew,
                        manualReport: RuleSeverityLevel.Error,
                        prerelease: RuleSeverityLevel.Error,
                        release: RuleSeverityLevel.Error),

                    Rule(MessageTypeCode.PoiAmountForEntranceShouldMeetMaximumRestrictions,
                         single: RuleSeverityLevel.Warning,
                         singleForApprove: RuleSeverityLevel.Warning,
                         manualReport: RuleSeverityLevel.Warning,
                         prerelease: RuleSeverityLevel.Warning,
                         release: RuleSeverityLevel.Warning),

                    Rule(MessageTypeCode.AtLeastOneLinkedPartnerFirmAddressShouldBeValid,
                         single: RuleSeverityLevel.Error,
                         singleForApprove: RuleSeverityLevel.Error,
                         manualReport: RuleSeverityLevel.Error,
                         prerelease: RuleSeverityLevel.Error,
                         release: RuleSeverityLevel.Error)
                };

        private static Tuple<MessageTypeCode, IReadOnlyDictionary<CheckMode, RuleSeverityLevel>> Rule(
            MessageTypeCode rule,
            RuleSeverityLevel? single = null,
            RuleSeverityLevel? singleForCancel = null,
            RuleSeverityLevel? singleForApprove = null,
            RuleSeverityLevel? manualReport = null,
            RuleSeverityLevel? manualReportWithAccount = null,
            RuleSeverityLevel? prerelease = null,
            RuleSeverityLevel? release = null)
        {
            if (manualReport.HasValue && !manualReportWithAccount.HasValue)
            {
                manualReportWithAccount = manualReport;
            }

            var values = new Dictionary<CheckMode, RuleSeverityLevel?>
                {
                    { CheckMode.Single, single },
                    { CheckMode.SingleForCancel, singleForCancel },
                    { CheckMode.SingleForApprove, singleForApprove },
                    { CheckMode.Manual, manualReport },
                    { CheckMode.ManualWithAccount, manualReportWithAccount },
                    { CheckMode.Prerelease, prerelease },
                    { CheckMode.Release, release },
                };

            return new Tuple<MessageTypeCode, IReadOnlyDictionary<CheckMode, RuleSeverityLevel>>(rule, values.Where(x => x.Value.HasValue).ToDictionary(x => x.Key, x => x.Value.Value));
        }
    }
}