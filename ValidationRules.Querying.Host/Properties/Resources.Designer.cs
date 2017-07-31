﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NuClear.ValidationRules.Querying.Host.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("NuClear.ValidationRules.Querying.Host.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} &quot;{{0}}&quot; является запрещённой для: {1} &quot;{{1}}&quot;.
        /// </summary>
        internal static string ADPCheckModeSpecificOrder_MessageTemplate {
            get {
                return ResourceManager.GetString("ADPCheckModeSpecificOrder_MessageTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} &quot;{{0}}&quot; данного Заказа является основной для следующих позиций: {1} &quot;{{1}}&quot;.
        /// </summary>
        internal static string ADPValidation_Template {
            get {
                return ResourceManager.GetString("ADPValidation_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Позиция {0} должна присутствовать в сборке в количестве от {1} до {2}. Может быть выпущено количество позиций в месяц {3:MMMM} - {4}.
        /// </summary>
        internal static string AdvertisementAmountShortErrorMessage {
            get {
                return ResourceManager.GetString("AdvertisementAmountShortErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Для фирмы {0} в белый список выбран рекламный материал {1}.
        /// </summary>
        internal static string AdvertisementChoosenForWhitelist {
            get {
                return ResourceManager.GetString("AdvertisementChoosenForWhitelist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Для фирмы {0} не указан рекламный материал в белый список.
        /// </summary>
        internal static string AdvertisementForWhitelistDoesNotSpecified {
            get {
                return ResourceManager.GetString("AdvertisementForWhitelistDoesNotSpecified", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Позиция {0} оформлена на пустой адрес {1}.
        /// </summary>
        internal static string AdvertisementIsLinkedWithEmptyAddressError {
            get {
                return ResourceManager.GetString("AdvertisementIsLinkedWithEmptyAddressError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Период размещения рекламного материала {0}, выбранного в позиции {1} должен захватывать 5 дней от текущего месяца размещения.
        /// </summary>
        internal static string AdvertisementPeriodEndsBeforeReleasePeriodBegins {
            get {
                return ResourceManager.GetString("AdvertisementPeriodEndsBeforeReleasePeriodBegins", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Период не может быть менее пяти дней..
        /// </summary>
        internal static string AdvertisementPeriodError {
            get {
                return ResourceManager.GetString("AdvertisementPeriodError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to В позиции {0} выбран рекламный материал {1}, не принадлежащий фирме {2}.
        /// </summary>
        internal static string AdvertisementSpecifiedForPositionDoesNotBelongToFirm {
            get {
                return ResourceManager.GetString("AdvertisementSpecifiedForPositionDoesNotBelongToFirm", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} &quot;{{0}}&quot; является сопутствующей, основная позиция не найдена..
        /// </summary>
        internal static string AssociatedPositionWithoutPrincipalTemplate {
            get {
                return ResourceManager.GetString("AssociatedPositionWithoutPrincipalTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Договор не может иметь дату подписания позднее даты подписания заказа.
        /// </summary>
        internal static string BargainSignedLaterThanOrder {
            get {
                return ResourceManager.GetString("BargainSignedLaterThanOrder", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Юр. лицо организации.
        /// </summary>
        internal static string BranchOffice {
            get {
                return ResourceManager.GetString("BranchOffice", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Юр. лицо исполнителя.
        /// </summary>
        internal static string BranchOfficeOrganizationUnit {
            get {
                return ResourceManager.GetString("BranchOfficeOrganizationUnit", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Позиция &quot;{0}&quot; не может быть продана в рубрику &quot;{1}&quot; проекта &quot;{2}&quot;.
        /// </summary>
        internal static string CategoryIsRestrictedForSpecifiedSalesModelError {
            get {
                return ResourceManager.GetString("CategoryIsRestrictedForSpecifiedSalesModelError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} &quot;{{0}}&quot; содержит объекты привязки, конфликтующие с объектами привязки следующей основной позиции: {1} &quot;{{1}}&quot;.
        /// </summary>
        internal static string ConflictingPrincipalPositionTemplate {
            get {
                return ResourceManager.GetString("ConflictingPrincipalPositionTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Купон на скидку {{0}} прикреплён к нескольким позициям: {0}.
        /// </summary>
        internal static string CouponIsBoundToMultiplePositionTemplate {
            get {
                return ResourceManager.GetString("CouponIsBoundToMultiplePositionTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Для позиции {0} в рубрику {1} отсутствует CPC.
        /// </summary>
        internal static string CpcIsMissing {
            get {
                return ResourceManager.GetString("CpcIsMissing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Для позиции {0} в рубрику {1} указан CPC меньше минимального.
        /// </summary>
        internal static string CpcIsTooSmall {
            get {
                return ResourceManager.GetString("CpcIsTooSmall", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Для рубрики {0} в проекте {1} не указан минимальный CPC.
        /// </summary>
        internal static string CpcRestrictionIsMissing {
            get {
                return ResourceManager.GetString("CpcRestrictionIsMissing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Валюта.
        /// </summary>
        internal static string Currency {
            get {
                return ResourceManager.GetString("Currency", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Не найден действующий для заказа прайс-лист.
        /// </summary>
        internal static string CurrentPriceNotFound {
            get {
                return ResourceManager.GetString("CurrentPriceNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Установленная по умолчанию тематика {0} должна содержать только саморекламу.
        /// </summary>
        internal static string DeafaultThemeMustContainOnlySelfAds {
            get {
                return ResourceManager.GetString("DeafaultThemeMustContainOnlySelfAds", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Для подразделения {0} не указана тематика по умолчанию.
        /// </summary>
        internal static string DefaultThemeIsNotSpecified {
            get {
                return ResourceManager.GetString("DefaultThemeIsNotSpecified", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Для фирмы {{0}} заказана рекламная ссылка {0} позиция {{1}} в заказе {{2}} , дублирующая контакт фирмы.
        /// </summary>
        internal static string FirmContactContainsSponsoredLinkError {
            get {
                return ResourceManager.GetString("FirmContactContainsSponsoredLinkError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Фирма {0} удалена.
        /// </summary>
        internal static string FirmIsDeleted {
            get {
                return ResourceManager.GetString("FirmIsDeleted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Фирма {0} скрыта навсегда.
        /// </summary>
        internal static string FirmIsPermanentlyClosed {
            get {
                return ResourceManager.GetString("FirmIsPermanentlyClosed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to В Позиции прайс-листа {0} содержится более одной группы сопутствующих позиций, что не поддерживается системой.
        /// </summary>
        internal static string InPricePositionOf_Price_ContaiedMoreThanOneAssociatedPositions {
            get {
                return ResourceManager.GetString("InPricePositionOf_Price_ContaiedMoreThanOneAssociatedPositions", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Юр. лицо заказчика.
        /// </summary>
        internal static string LegalPerson {
            get {
                return ResourceManager.GetString("LegalPerson", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Профиль юр. лица заказчика.
        /// </summary>
        internal static string LegalPersonProfile {
            get {
                return ResourceManager.GetString("LegalPersonProfile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} &quot;{{0}}&quot; содержит объекты привязки, отсутствующие в основных позициях..
        /// </summary>
        internal static string LinkedObjectsMissedInPrincipals {
            get {
                return ResourceManager.GetString("LinkedObjectsMissedInPrincipals", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Для подразделения {0} установлено более одной тематики по умолчанию.
        /// </summary>
        internal static string MoreThanOneDefaultTheme {
            get {
                return ResourceManager.GetString("MoreThanOneDefaultTheme", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to У юр. лица клиента отсутствует профиль.
        /// </summary>
        internal static string MustMakeLegalPersonProfile {
            get {
                return ResourceManager.GetString("MustMakeLegalPersonProfile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Сумма по счетам не совпадает с планируемой суммой заказа.
        /// </summary>
        internal static string OrderApproval_BillsSumDoesntMatchWhenProcessingOrderOnApproval {
            get {
                return ResourceManager.GetString("OrderApproval_BillsSumDoesntMatchWhenProcessingOrderOnApproval", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to В позиции {0} необходимо указать рекламные материалы для подпозиции {1}.
        /// </summary>
        internal static string OrderCheckCompositePositionMustHaveAdvertisements {
            get {
                return ResourceManager.GetString("OrderCheckCompositePositionMustHaveAdvertisements", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to В позиции {0} необходимо указать хотя бы один объект привязки для подпозиции {1}.
        /// </summary>
        internal static string OrderCheckCompositePositionMustHaveLinkingObject {
            get {
                return ResourceManager.GetString("OrderCheckCompositePositionMustHaveLinkingObject", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Заказ оформлен на период, по которому уже сформирована сборка. Необходимо указать другие даты размещения заказа.
        /// </summary>
        internal static string OrderCheckHasReleases {
            get {
                return ResourceManager.GetString("OrderCheckHasReleases", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Указана некорректная дата начала размещения.
        /// </summary>
        internal static string OrderCheckIncorrectBeginDistributionDate {
            get {
                return ResourceManager.GetString("OrderCheckIncorrectBeginDistributionDate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Указана некорректная дата окончания размещения.
        /// </summary>
        internal static string OrderCheckIncorrectEndDistributionDate {
            get {
                return ResourceManager.GetString("OrderCheckIncorrectEndDistributionDate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Отсутствует сканированная копия договора.
        /// </summary>
        internal static string OrderCheckOrderHasNoBargainScans {
            get {
                return ResourceManager.GetString("OrderCheckOrderHasNoBargainScans", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Заказ не содержит ни одной позиции.
        /// </summary>
        internal static string OrderCheckOrderHasNoPositions {
            get {
                return ResourceManager.GetString("OrderCheckOrderHasNoPositions", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Отсутствует сканированная копия Бланка заказа.
        /// </summary>
        internal static string OrderCheckOrderHasNoScans {
            get {
                return ResourceManager.GetString("OrderCheckOrderHasNoScans", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Необходимо заполнить все обязательные для заполнения поля: {0}.
        /// </summary>
        internal static string OrderCheckOrderHasUnspecifiedFields {
            get {
                return ResourceManager.GetString("OrderCheckOrderHasUnspecifiedFields", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Позиция {0} соответствует скрытой позиции прайс листа. Необходимо указать активную позицию из текущего действующего прайс-листа..
        /// </summary>
        internal static string OrderCheckOrderPositionCorrespontToInactivePosition {
            get {
                return ResourceManager.GetString("OrderCheckOrderPositionCorrespontToInactivePosition", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Позиция {0} не соответствует актуальному прайс-листу. Необходимо указать позицию из текущего действующего прайс-листа..
        /// </summary>
        internal static string OrderCheckOrderPositionDoesntCorrespontToActualPrice {
            get {
                return ResourceManager.GetString("OrderCheckOrderPositionDoesntCorrespontToActualPrice", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to В позиции {0} необходимо указать рекламные материалы.
        /// </summary>
        internal static string OrderCheckPositionMustHaveAdvertisements {
            get {
                return ResourceManager.GetString("OrderCheckPositionMustHaveAdvertisements", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Позиция {0} содержит заглушку рекламного материала.
        /// </summary>
        internal static string OrderContainsDummyAdvertisementError {
            get {
                return ResourceManager.GetString("OrderContainsDummyAdvertisementError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Для заказа указана неактивная работа.
        /// </summary>
        internal static string OrderDealIsInactive {
            get {
                return ResourceManager.GetString("OrderDealIsInactive", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to  в заказе {{2}}.
        /// </summary>
        internal static string OrderDescriptionTemplate {
            get {
                return ResourceManager.GetString("OrderDescriptionTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Фирма {0} скрыта до выяснения..
        /// </summary>
        internal static string OrderFirmHiddenForAscertainmentTemplate {
            get {
                return ResourceManager.GetString("OrderFirmHiddenForAscertainmentTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to В позиции {0} адрес фирмы {1} скрыт навсегда.
        /// </summary>
        internal static string OrderPositionAddressDeleted {
            get {
                return ResourceManager.GetString("OrderPositionAddressDeleted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to В позиции {0} адрес фирмы {1} скрыт до выяснения.
        /// </summary>
        internal static string OrderPositionAddressHidden {
            get {
                return ResourceManager.GetString("OrderPositionAddressHidden", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to В позиции {0} найден неактивный адрес {1}.
        /// </summary>
        internal static string OrderPositionAddressNotActive {
            get {
                return ResourceManager.GetString("OrderPositionAddressNotActive", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to В позиции {0} найден адрес {1}, не принадлежащий фирме заказа.
        /// </summary>
        internal static string OrderPositionAddressNotBelongToFirm {
            get {
                return ResourceManager.GetString("OrderPositionAddressNotBelongToFirm", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to В позиции {0} найдена неактивная рубрика {1}.
        /// </summary>
        internal static string OrderPositionCategoryNotActive {
            get {
                return ResourceManager.GetString("OrderPositionCategoryNotActive", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to В позиции {0} найдена рубрика {1}, не принадлежащая адресу {2}.
        /// </summary>
        internal static string OrderPositionCategoryNotBelongsToAddress {
            get {
                return ResourceManager.GetString("OrderPositionCategoryNotBelongsToAddress", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to В позиции {0} найдена рубрика {1}, не принадлежащая фирме заказа.
        /// </summary>
        internal static string OrderPositionCategoryNotBelongsToFirm {
            get {
                return ResourceManager.GetString("OrderPositionCategoryNotBelongsToFirm", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Заказ ссылается на неактивные объекты: {0}.
        /// </summary>
        internal static string OrderReferencesInactiveEntities {
            get {
                return ResourceManager.GetString("OrderReferencesInactiveEntities", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to В рекламном материале &quot;{0}&quot;, который подлежит выверке, элемент &quot;{1}&quot; находится в статусе &apos;Черновик&apos;.
        /// </summary>
        internal static string OrdersCheckAdvertisementElementIsDraft {
            get {
                return ResourceManager.GetString("OrdersCheckAdvertisementElementIsDraft", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to В рекламном материале &quot;{0}&quot;, который подлежит выверке, элемент &quot;{1}&quot; содержит ошибки выверки.
        /// </summary>
        internal static string OrdersCheckAdvertisementElementWasInvalidated {
            get {
                return ResourceManager.GetString("OrdersCheckAdvertisementElementWasInvalidated", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Отделение организации назначения заказа не соответствует отделению организации выбранной фирмы.
        /// </summary>
        internal static string OrdersCheckDestOrganizationUnitDoesntMatchFirmsOne {
            get {
                return ResourceManager.GetString("OrdersCheckDestOrganizationUnitDoesntMatchFirmsOne", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Для заказа необходимо сформировать счета.
        /// </summary>
        internal static string OrdersCheckNeedToCreateBills {
            get {
                return ResourceManager.GetString("OrdersCheckNeedToCreateBills", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Заказ не имеет привязки к лицевому счёту.
        /// </summary>
        internal static string OrdersCheckOrderHasNoAccount {
            get {
                return ResourceManager.GetString("OrdersCheckOrderHasNoAccount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Для оформления заказа недостаточно средств. Необходимо: {0}. Имеется: {1}..
        /// </summary>
        internal static string OrdersCheckOrderInsufficientFunds {
            get {
                return ResourceManager.GetString("OrdersCheckOrderInsufficientFunds", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Рубрика {0} используется в позиции {1}, но не привязана к отделению организации города назначения заказа.
        /// </summary>
        internal static string OrdersCheckOrderPositionContainsCategoriesFromWrongOrganizationUnit {
            get {
                return ResourceManager.GetString("OrdersCheckOrderPositionContainsCategoriesFromWrongOrganizationUnit", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to В рекламном материале {0} не заполнен обязательный элемент {1}.
        /// </summary>
        internal static string OrdersCheckPositionMustHaveAdvertisementElements {
            get {
                return ResourceManager.GetString("OrdersCheckPositionMustHaveAdvertisementElements", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to В позиции прайса {0} необходимо указать минимальное количество рекламы в выпуск.
        /// </summary>
        internal static string PricePositionHasNoMinAdvertisementAmount {
            get {
                return ResourceManager.GetString("PricePositionHasNoMinAdvertisementAmount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to У юр. лица клиента, в профиле {0} указан договор с датой окончания действия раньше даты подписания заказа.
        /// </summary>
        internal static string ProfileBargainEndDateIsLessThanSignDate {
            get {
                return ResourceManager.GetString("ProfileBargainEndDateIsLessThanSignDate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to У юр. лица клиента, в профиле {0} указана доверенность с датой окончания действия раньше даты подписания заказа.
        /// </summary>
        internal static string ProfileWarrantyEndDateIsLessThanSignDate {
            get {
                return ResourceManager.GetString("ProfileWarrantyEndDateIsLessThanSignDate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to В позиции {0} выбран удалённый рекламный материал {1}.
        /// </summary>
        internal static string RemovedAdvertisemendSpecifiedForPosition {
            get {
                return ResourceManager.GetString("RemovedAdvertisemendSpecifiedForPosition", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Подпозиция &quot;{0}&quot; позиции.
        /// </summary>
        internal static string RichChildPositionTypeTemplate {
            get {
                return ResourceManager.GetString("RichChildPositionTypeTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Позиция.
        /// </summary>
        internal static string RichDefaultPositionTypeTemplate {
            get {
                return ResourceManager.GetString("RichDefaultPositionTypeTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Позиция &quot;Самореклама только для ПК&quot; продана одновременно с рекламой в другую платформу.
        /// </summary>
        internal static string SelfAdvertisementOrderValidationRuleMessage {
            get {
                return ResourceManager.GetString("SelfAdvertisementOrderValidationRuleMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Заказ {0} не может иметь продаж в тематику {1}, поскольку тематика действует не весь период размещения заказа.
        /// </summary>
        internal static string ThemePeriodDoesNotOverlapOrderPeriod {
            get {
                return ResourceManager.GetString("ThemePeriodDoesNotOverlapOrderPeriod", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Слишком много продаж в тематику {{0}}. Продано {0} позиций вместо {1} возможных.
        /// </summary>
        internal static string ThemeSalesExceedsLimit {
            get {
                return ResourceManager.GetString("ThemeSalesExceedsLimit", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Тематика {0} использует удаленную рубрику {1}.
        /// </summary>
        internal static string ThemeUsesInactiveCategory {
            get {
                return ResourceManager.GetString("ThemeUsesInactiveCategory", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to У фирмы {0}, с рубрикой &quot;Выгодные покупки с 2ГИС&quot;, отсутствуют продажи по позициям &quot;Самореклама только для ПК&quot; или &quot;Выгодные покупки с 2ГИС&quot;..
        /// </summary>
        internal static string ThereIsNoAdvertisementForAdvantageousPurchasesCategory {
            get {
                return ResourceManager.GetString("ThereIsNoAdvertisementForAdvantageousPurchasesCategory", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Для заказа не указана работа.
        /// </summary>
        internal static string ThereIsNoSpecifiedDealForOrder {
            get {
                return ResourceManager.GetString("ThereIsNoSpecifiedDealForOrder", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to В рубрику {{0}} заказано слишком много объявлений: Заказано {0}, допустимо не более {1}.
        /// </summary>
        internal static string TooManyAdvertisementForCategory {
            get {
                return ResourceManager.GetString("TooManyAdvertisementForCategory", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Для фирмы {{0}} задано слишком большое число рубрик - {0}. Максимально допустимое - {1}..
        /// </summary>
        internal static string TooManyCategorieForFirm {
            get {
                return ResourceManager.GetString("TooManyCategorieForFirm", resourceCulture);
            }
        }
    }
}
