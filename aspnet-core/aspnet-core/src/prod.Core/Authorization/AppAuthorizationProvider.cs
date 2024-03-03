using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.MultiTenancy;
using Castle.Core.Internal;
using prod.Master.Cmm;
using System.Text.RegularExpressions;

namespace prod.Authorization
{
    /// <summary>
    /// Application's authorization provider.
    /// Defines permissions for the application.
    /// See <see cref="AppPermissions"/> for all permission names.
    /// </summary>
    public class AppAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

        public AppAuthorizationProvider(bool isMultiTenancyEnabled)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
        }

        public AppAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }

        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)
            #region
            var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));
            pages.CreateChildPermission(AppPermissions.Pages_DemoUiComponents, L("DemoUiComponents"));

            var administration = pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));

            var roles = administration.CreateChildPermission(AppPermissions.Pages_Administration_Roles, L("Roles"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Create, L("CreatingNewRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Edit, L("EditingRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Delete, L("DeletingRole"));

            var users = administration.CreateChildPermission(AppPermissions.Pages_Administration_Users, L("Users"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Create, L("CreatingNewUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Edit, L("EditingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Delete, L("DeletingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_ChangePermissions, L("ChangingPermissions"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Impersonation, L("LoginForUsers"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Unlock, L("Unlock"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_ChangeProfilePicture, L("UpdateUsersProfilePicture"));

            var languages = administration.CreateChildPermission(AppPermissions.Pages_Administration_Languages, L("Languages"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Create, L("CreatingNewLanguage"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Edit, L("EditingLanguage"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Delete, L("DeletingLanguages"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_ChangeTexts, L("ChangingTexts"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_ChangeDefaultLanguage, L("ChangeDefaultLanguage"));
            
            administration.CreateChildPermission(AppPermissions.Pages_Administration_AuditLogs, L("AuditLogs"));

            var organizationUnits = administration.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits, L("OrganizationUnits"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageOrganizationTree, L("ManagingOrganizationTree"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageMembers, L("ManagingMembers"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageRoles, L("ManagingRoles"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_UiCustomization, L("VisualSettings"));

            var webhooks = administration.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription, L("Webhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Create, L("CreatingWebhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Edit, L("EditingWebhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_ChangeActivity, L("ChangingWebhookActivity"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Detail, L("DetailingSubscription"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_Webhook_ListSendAttempts, L("ListingSendAttempts"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_Webhook_ResendWebhook, L("ResendingWebhook"));

            var dynamicProperties = administration.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties, L("DynamicProperties"));
            dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties_Create, L("CreatingDynamicProperties"));
            dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties_Edit, L("EditingDynamicProperties"));
            dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties_Delete, L("DeletingDynamicProperties"));

            var dynamicPropertyValues = dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue, L("DynamicPropertyValue"));
            dynamicPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue_Create, L("CreatingDynamicPropertyValue"));
            dynamicPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue_Edit, L("EditingDynamicPropertyValue"));
            dynamicPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue_Delete, L("DeletingDynamicPropertyValue"));

            var dynamicEntityProperties = dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties, L("DynamicEntityProperties"));
            dynamicEntityProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties_Create, L("CreatingDynamicEntityProperties"));
            dynamicEntityProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties_Edit, L("EditingDynamicEntityProperties"));
            dynamicEntityProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties_Delete, L("DeletingDynamicEntityProperties"));

            var dynamicEntityPropertyValues = dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue, L("EntityDynamicPropertyValue"));
            dynamicEntityPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue_Create, L("CreatingDynamicEntityPropertyValue"));
            dynamicEntityPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue_Edit, L("EditingDynamicEntityPropertyValue"));
            dynamicEntityPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue_Delete, L("DeletingDynamicEntityPropertyValue"));

            var massNotification = administration.CreateChildPermission(AppPermissions.Pages_Administration_MassNotification, L("MassNotifications"));
            massNotification.CreateChildPermission(AppPermissions.Pages_Administration_MassNotification_Create, L("MassNotificationCreate"));
            
            //TENANT-SPECIFIC PERMISSIONS

            pages.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Tenant);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_SubscriptionManagement, L("Subscription"), multiTenancySides: MultiTenancySides.Tenant);

            //HOST-SPECIFIC PERMISSIONS

            var editions = pages.CreateChildPermission(AppPermissions.Pages_Editions, L("Editions"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Create, L("CreatingNewEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Edit, L("EditingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Delete, L("DeletingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_MoveTenantsToAnotherEdition, L("MoveTenantsToAnotherEdition"), multiTenancySides: MultiTenancySides.Host);

            var tenants = pages.CreateChildPermission(AppPermissions.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Create, L("CreatingNewTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Edit, L("EditingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_ChangeFeatures, L("ChangingFeatures"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Delete, L("DeletingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Impersonation, L("LoginForTenants"), multiTenancySides: MultiTenancySides.Host);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Host);
            
            var maintenance = administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Maintenance, L("Maintenance"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            maintenance.CreateChildPermission(AppPermissions.Pages_Administration_NewVersion_Create, L("SendNewVersionNotification"));
            
            administration.CreateChildPermission(AppPermissions.Pages_Administration_HangfireDashboard, L("HangfireDashboard"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Host);
            #endregion

            //NEW
            #region WORKING PATTERN
            // workingPattern 
            var workingPattern = pages.CreateChildPermission(AppPermissions.Pages_WorkingPattern, L("WorkingPattern"), multiTenancySides: MultiTenancySides.Tenant);

            //Master - Working Pattern - Calendar
            var mstWptCalendar = workingPattern.CreateChildPermission(AppPermissions.Pages_WorkingPattern_Calendar_View, L("MasterWorkingPatternCalendarView"), multiTenancySides: MultiTenancySides.Tenant);
            mstWptCalendar.CreateChildPermission(AppPermissions.Pages_WorkingPattern_Calendar_Edit, L("MasterWorkingPatternCalendarEdit"), multiTenancySides: MultiTenancySides.Tenant);
           
            //Master - Working Pattern - DailyWorkingTime
            var mstWptDailyWorkingTime = workingPattern.CreateChildPermission(AppPermissions.Pages_WorkingPattern_DailyWorkingTime_View, L("MasterWorkingPatternDailyWorkingTimeView"), multiTenancySides: MultiTenancySides.Tenant);
            mstWptDailyWorkingTime.CreateChildPermission(AppPermissions.Pages_WorkingPattern_DailyWorkingTime_Edit, L("MasterWorkingPatternDailyWorkingTimeEdit"), multiTenancySides: MultiTenancySides.Tenant);
           
            //Master - Working Pattern - PatternD
            var mstWptPatternD = workingPattern.CreateChildPermission(AppPermissions.Pages_WorkingPattern_PatternD_View, L("MasterWorkingPatternPatternDView"), multiTenancySides: MultiTenancySides.Tenant);
            mstWptPatternD.CreateChildPermission(AppPermissions.Pages_WorkingPattern_PatternD_Edit, L("MasterWorkingPatternPatternDEdit"), multiTenancySides: MultiTenancySides.Tenant);
          
            //Master - Working Pattern - PatternH
            var mstWptPatternH = workingPattern.CreateChildPermission(AppPermissions.Pages_WorkingPattern_PatternH_View, L("MasterWorkingPatternPatternHView"), multiTenancySides: MultiTenancySides.Tenant);
            mstWptPatternH.CreateChildPermission(AppPermissions.Pages_WorkingPattern_PatternH_Edit, L("MasterWorkingPatternPatternHEdit"), multiTenancySides: MultiTenancySides.Tenant);
           
            //Master - Working Pattern - SeasonMonth
            var mstWptSeasonMonth = workingPattern.CreateChildPermission(AppPermissions.Pages_WorkingPattern_SeasonMonth_View, L("MasterWorkingPatternSeasonMonthView"), multiTenancySides: MultiTenancySides.Tenant);
            mstWptSeasonMonth.CreateChildPermission(AppPermissions.Pages_WorkingPattern_SeasonMonth_Edit, L("MasterWorkingPatternSeasonMonthEdit"), multiTenancySides: MultiTenancySides.Tenant);
           
            //Master - Working Pattern - Week
            var mstWptWeek = workingPattern.CreateChildPermission(AppPermissions.Pages_WorkingPattern_Week_View, L("MasterWorkingPatternWeekView"), multiTenancySides: MultiTenancySides.Tenant);
            mstWptWeek.CreateChildPermission(AppPermissions.Pages_WorkingPattern_Week_Edit, L("MasterWorkingPatternWeekEdit"), multiTenancySides: MultiTenancySides.Tenant);
           
            //Master - Working Pattern - WorkingTime
            var mstWptWorkingTime = workingPattern.CreateChildPermission(AppPermissions.Pages_WorkingPattern_WorkingTime_View, L("MasterWorkingPatternWorkingTimeView"), multiTenancySides: MultiTenancySides.Tenant);
            mstWptWorkingTime.CreateChildPermission(AppPermissions.Pages_WorkingPattern_WorkingTime_Edit, L("MasterWorkingPatternWorkingTimeEdit"), multiTenancySides: MultiTenancySides.Tenant);
           
            //Master - Working Pattern - WorkingType
            var mstWptWorkingType = workingPattern.CreateChildPermission(AppPermissions.Pages_WorkingPattern_WorkingType_View, L("MasterWorkingPatternWorkingTypeView"), multiTenancySides: MultiTenancySides.Tenant);
            mstWptWorkingType.CreateChildPermission(AppPermissions.Pages_WorkingPattern_WorkingType_Edit, L("MasterWorkingPatternWorkingTypeEdit"), multiTenancySides: MultiTenancySides.Tenant);
           

            #endregion

            #region MASTER
            // master
            var master = pages.CreateChildPermission(AppPermissions.Pages_Master, L("Master"), multiTenancySides: MultiTenancySides.Tenant);

            // Master_Common
            var masterCommon = master.CreateChildPermission(AppPermissions.Pages_Master_Common, L("MasterCommon"), multiTenancySides: MultiTenancySides.Tenant);

            //Master_Common_ExchangeRate
            var perMstCmmExchangeRate = masterCommon.CreateChildPermission(AppPermissions.Pages_Master_Common_ExchangeRate_View, L("MasterCommonExchangeRateView"), multiTenancySides: MultiTenancySides.Tenant);
          //  perMstCmmExchangeRate.CreateChildPermission(AppPermissions.Pages_Master_Common_ExchangeRate_Check, L("Pages_Master_Common_ExchangeRate_Check"), multiTenancySides: MultiTenancySides.Tenant);
            perMstCmmExchangeRate.CreateChildPermission(AppPermissions.Pages_Master_Common_ExchangeRate_Approve, L("Pages_Master_Common_ExchangeRate_Approve"), multiTenancySides: MultiTenancySides.Tenant);
            perMstCmmExchangeRate.CreateChildPermission(AppPermissions.Pages_Master_Common_ExchangeRate_History, L("Pages_Master_Common_ExchangeRate_History"), multiTenancySides: MultiTenancySides.Tenant);

            //Master - Common - Model
            var perMstCmmModel = masterCommon.CreateChildPermission(AppPermissions.Pages_Master_Common_Model_View, L("Pages_Master_Common_Model_View"), multiTenancySides: MultiTenancySides.Tenant);
            perMstCmmModel.CreateChildPermission(AppPermissions.Pages_Master_Common_Model_Edit, L("Pages_Master_Common_Model_Edit"), multiTenancySides: MultiTenancySides.Tenant);
         

            //Master_Common_Shop
            var perMstCmmShop = masterCommon.CreateChildPermission(AppPermissions.Pages_Master_Common_Shop_View, L("Pages_Master_Common_Shop_View"), multiTenancySides: MultiTenancySides.Tenant);
            perMstCmmShop.CreateChildPermission(AppPermissions.Pages_Master_Common_Shop_Edit, L("Pages_Master_Common_Shop_Edit"), multiTenancySides: MultiTenancySides.Tenant);

            //Master_Common_DevanningCaseType
            var perMstCmmDevanningCaseType = masterCommon.CreateChildPermission(AppPermissions.Pages_Master_Common_DevanningCaseType_View, L("Pages_Master_Common_DevanningCaseType_View"), multiTenancySides: MultiTenancySides.Tenant);
            perMstCmmDevanningCaseType.CreateChildPermission(AppPermissions.Pages_Master_Common_DevanningCaseType_Edit, L("Pages_Master_Common_DevanningCaseType_Edit"), multiTenancySides: MultiTenancySides.Tenant);
            perMstCmmDevanningCaseType.CreateChildPermission(AppPermissions.Pages_Master_Common_DevanningCaseType_History, L("Pages_Master_Common_DevanningCaseType_History"), multiTenancySides: MultiTenancySides.Tenant);

            //Master_Common_ShopType
            var perMstCmmShopType = masterCommon.CreateChildPermission(AppPermissions.Pages_Master_Common_ShopType_View, L("Pages_Master_Common_ShopType_View"), multiTenancySides: MultiTenancySides.Tenant);
            perMstCmmShopType.CreateChildPermission(AppPermissions.Pages_Master_Common_ShopType_Edit, L("Pages_Master_Common_ShopType_Edit"), multiTenancySides: MultiTenancySides.Tenant);
         
            //Master_Common_Color
            var perMstCmmColor = masterCommon.CreateChildPermission(AppPermissions.Pages_Master_Common_Color_View, L("Pages_Master_Common_Color_View"), multiTenancySides: MultiTenancySides.Tenant);
            perMstCmmColor.CreateChildPermission(AppPermissions.Pages_Master_Common_Color_Edit, L("Pages_Master_Common_Color_Edit"), multiTenancySides: MultiTenancySides.Tenant);
            perMstCmmColor.CreateChildPermission(AppPermissions.Pages_Master_Common_Color_History, L("Pages_Master_Common_Color_History"), multiTenancySides: MultiTenancySides.Tenant);

            //MstCmmLotCodeGrade
            var perMstCmmLotCodeGrade = masterCommon.CreateChildPermission(AppPermissions.Pages_Master_Cmm_LotCodeGrade_History, L("Pages_InvtSetup_CheckingRule_LotCodeGrade_History"), multiTenancySides: MultiTenancySides.Tenant);

            //Master_Common_Color
            var perMstCmmGradeColor = masterCommon.CreateChildPermission(AppPermissions.Pages_Master_Common_MstCmmGradeColor_History, L("Pages_Master_Common_MstCmmGradeColor_History"), multiTenancySides: MultiTenancySides.Tenant);

            //Master_Common_VehicleColorType
            var perMstCmmVehicleColorType = masterCommon.CreateChildPermission(AppPermissions.Pages_Master_Common_VehicleColorType_View, L("Pages_Master_Common_VehicleColorType_View"), multiTenancySides: MultiTenancySides.Tenant);
         
            //Master_Common_VehicleColorName
            var perMstCmmVehicleColorName = masterCommon.CreateChildPermission(AppPermissions.Pages_Master_Common_VehicleName_View, L("Pages_Master_Common_VehicleName_View"), multiTenancySides: MultiTenancySides.Tenant);
         

            //Master - Common - TaktTime
            var perMstCmmTaktTime = masterCommon.CreateChildPermission(AppPermissions.Pages_Master_Common_TaktTime_View, L("Pages_Master_Common_TaktTime_View"), multiTenancySides: MultiTenancySides.Tenant);
            perMstCmmTaktTime.CreateChildPermission(AppPermissions.Pages_Master_Common_TaktTime_Edit, L("Pages_Master_Common_TaktTime_Edit"), multiTenancySides: MultiTenancySides.Tenant);
            

            // Master - Common - Lookup
            var mstCmmLookup = masterCommon.CreateChildPermission(AppPermissions.Pages_Master_Common_Lookup_View, L("Pages_Master_Common_Lookup_View"), multiTenancySides: MultiTenancySides.Tenant);
            mstCmmLookup.CreateChildPermission(AppPermissions.Pages_Master_Common_Lookup_Edit, L("Pages_Master_Common_Lookup_Edit"), multiTenancySides: MultiTenancySides.Tenant);
            mstCmmLookup.CreateChildPermission(AppPermissions.Pages_Master_Common_Lookup_History, L("Pages_Master_Common_Lookup_History"), multiTenancySides: MultiTenancySides.Tenant);

            //Master - Common - BusinessParter
            var perMstCmmBusinessParter = masterCommon.CreateChildPermission(AppPermissions.Pages_Master_Cmm_BusinessParter_View, L("Pages_Master_Cmm_BusinessParter_View"), multiTenancySides: MultiTenancySides.Tenant);
            perMstCmmBusinessParter.CreateChildPermission(AppPermissions.Pages_Master_Cmm_BusinessParter_CreateEdit, L("Pages_Master_Cmm_BusinessParter_CreateEdit"), multiTenancySides: MultiTenancySides.Tenant);
            perMstCmmBusinessParter.CreateChildPermission(AppPermissions.Pages_Master_Cmm_BusinessParter_Delete, L("Pages_Master_Cmm_BusinessParter_Delete"), multiTenancySides: MultiTenancySides.Tenant);


            //Master Cmm Vehice CBU
            var perMstCmmVehicleCBU = masterCommon.CreateChildPermission(AppPermissions.Pages_Master_Cmm_VehicleCBU_View, L("Pages_Master_Cmm_VehicleCBU_View"), multiTenancySides: MultiTenancySides.Tenant);
            perMstCmmVehicleCBU.CreateChildPermission(AppPermissions.Pages_Master_Cmm_VehicleCBU_Validate, L("Pages_Master_Cmm_VehicleCBU_Validate"), multiTenancySides: MultiTenancySides.Tenant);
            perMstCmmVehicleCBU.CreateChildPermission(AppPermissions.Pages_Master_Cmm_VehicleCBU_History, L("Pages_Master_Cmm_VehicleCBU_History"), multiTenancySides: MultiTenancySides.Tenant);
            perMstCmmVehicleCBU.CreateChildPermission(AppPermissions.Pages_Master_Cmm_VehicleCBU_CreateMat, L("Pages_Master_Cmm_VehicleCBU_CreateMat"), multiTenancySides: MultiTenancySides.Tenant);

            //Master Cmm Engine Master
            var perMstCmmEngineMaster = masterCommon.CreateChildPermission(AppPermissions.Pages_Master_Cmm_EngineMaster_View, L("Pages_Master_Cmm_EngineMaster_View"), multiTenancySides: MultiTenancySides.Tenant);
            perMstCmmEngineMaster.CreateChildPermission(AppPermissions.Pages_Master_Cmm_EngineMaster_CreateEdit, L("Pages_Master_Cmm_EngineMaster_CreateEdit"), multiTenancySides: MultiTenancySides.Tenant);
            perMstCmmEngineMaster.CreateChildPermission(AppPermissions.Pages_Master_Cmm_EngineMaster_Delete, L("Pages_Master_Cmm_EngineMaster_Delete"), multiTenancySides: MultiTenancySides.Tenant);
            perMstCmmEngineMaster.CreateChildPermission(AppPermissions.Pages_Master_Cmm_EngineMaster_History, L("Pages_Master_Cmm_EngineMaster_History"), multiTenancySides: MultiTenancySides.Tenant);



            // Master_Ckd
            var masterCkd = master.CreateChildPermission(AppPermissions.Pages_Master_Ckd, L("MasterCkd"), multiTenancySides: MultiTenancySides.Tenant);

            //Master - Ckd - CustomsPort
            var perMstInvCustomsPort = masterCkd.CreateChildPermission(AppPermissions.Pages_Master_Ckd_CustomsPort_View, L("Pages_Master_Ckd_CustomsPort_View"), multiTenancySides: MultiTenancySides.Tenant);
           

            //Master - Ckd - ShippingCompany
            var perMstInvShippingCompany = masterCkd.CreateChildPermission(AppPermissions.Pages_Master_Ckd_ShippingCompany_View, L("Pages_Master_Ckd_ShippingCompany_View"), multiTenancySides: MultiTenancySides.Tenant);
         

            // Master -Ckd-ContainerStatus
            var perMstInvContainerStatus = masterCkd.CreateChildPermission(AppPermissions.Pages_Master_Ckd_ContainerStatus_View, L("Pages_Master_Ckd_ContainerStatus_View"), multiTenancySides: MultiTenancySides.Tenant);
          
            // Master -Ckd-ContainerDeliveryType
            var perMstInvContainerDeliveryType = masterCkd.CreateChildPermission(AppPermissions.Pages_Master_Ckd_ContainerDeliveryType_View, L("Pages_Master_Ckd_ContainerDeliveryType_View"), multiTenancySides: MultiTenancySides.Tenant);
       

            // Master -Ckd-Forwarder
            var perMstInvForwarder = masterCkd.CreateChildPermission(AppPermissions.Pages_Master_Ckd_Forwarder_View, L("Pages_Master_Ckd_Forwarder_View"), multiTenancySides: MultiTenancySides.Tenant);
         
            //Master-Ckd-InvoiceStatus
            var perMstInvInvoiceStatus = masterCkd.CreateChildPermission(AppPermissions.Pages_Master_Ckd_InvoiceStatus_View, L("Pages_Master_Ckd_InvoiceStatus_View"), multiTenancySides: MultiTenancySides.Tenant);
         
            //Master - Ckd - CustomsStatus
            var perMstInvCustomsStatus = masterCkd.CreateChildPermission(AppPermissions.Pages_Master_Ckd_CustomsStatus_View, L("Pages_Master_Ckd_CustomsStatus_View"), multiTenancySides: MultiTenancySides.Tenant);

            // Master - Ckd - SupplierList
            var perMstInvSupplierList = masterCkd.CreateChildPermission(AppPermissions.Pages_Master_Inventory_SupplierList_View, L("Pages_Master_Inventory_SupplierList_View"), multiTenancySides: MultiTenancySides.Tenant);

 
            var perMstInvCkdRentalWarehouse = masterCkd.CreateChildPermission(AppPermissions.Pages_Master_Inventory_CkdRentalWarehouse, L("Pages_Master_Inventory_CkdRentalWarehouse"), multiTenancySides: MultiTenancySides.Tenant);
            masterCkd.CreateChildPermission(AppPermissions.Pages_Master_Inventory_CkdRentalWarehouse_CreateEdit, L("Pages_Master_Inventory_CkdRentalWarehouse_CreateEdit"), multiTenancySides: MultiTenancySides.Tenant);
            masterCkd.CreateChildPermission(AppPermissions.Pages_Master_Inventory_CkdRentalWarehouse_Delete, L("Pages_Master_Inventory_CkdRentalWarehouse_Delete"), multiTenancySides: MultiTenancySides.Tenant);



            //PIOPartType
            var perMstInvPIOPartType = masterCkd.CreateChildPermission(AppPermissions.Pages_Master_Inventory_PIOPartType_View, L("Pages_Master_Inventory_PIOPartType_View"), multiTenancySides: MultiTenancySides.Tenant);

			
			//PIOPartType
			//var perMstInvPIOPartType = masterCkd.CreateChildPermission(AppPermissions.Pages_Master_Inventory_PIOPartType_View, L("Pages_Master_Inventory_PIOPartType_View"), multiTenancySides: MultiTenancySides.Tenant);

            perMstInvPIOPartType.CreateChildPermission(AppPermissions.Pages_Master_Inventory_PIOPartType_Edit, L("MasterInventoryPIOPartTypeEdit"), multiTenancySides: MultiTenancySides.Tenant);

            // ImpSupplier
            var peMstPioImpSupplier = masterCkd.CreateChildPermission(AppPermissions.Pages_Master_MstPioImpSupplier_View, L("Pages_Master_MstPioImpSupplier_View"), multiTenancySides: MultiTenancySides.Tenant);


            //PIOPartType
            //var perMstInvPIOPartType = masterCkd.CreateChildPermission(AppPermissions.Pages_Master_Inventory_PIOPartType_View, L("Pages_Master_Inventory_PIOPartType_View"), multiTenancySides: MultiTenancySides.Tenant);

            peMstPioImpSupplier.CreateChildPermission(AppPermissions.Pages_Master_MstPioImpSupplier_Edit, L("Pages_Master_MstPioImpSupplier_Edit"), multiTenancySides: MultiTenancySides.Tenant);
          

            // ImpSupplier


            // PIOEmail
            var perMstInvPIOEmail = masterCkd.CreateChildPermission(AppPermissions.Pages_Master_Inventory_PIOEmail_View, L("Pages_Master_Inventory_PIOEmail_View"), multiTenancySides: MultiTenancySides.Tenant);
            perMstInvPIOEmail.CreateChildPermission(AppPermissions.Pages_Master_Inventory_PIOEmail_Edit, L("MasterInventoryPIOEmailEdit"), multiTenancySides: MultiTenancySides.Tenant);
        
            //Master - Ckd - CustomsLeadtime
            var perMstCkdCustomsLeadtime = masterCkd.CreateChildPermission(AppPermissions.Pages_Master_CKD_CustomsLeadtime_View, L("Pages_Master_CKD_CustomsLeadtime_View"), multiTenancySides: MultiTenancySides.Tenant);
            perMstCkdCustomsLeadtime.CreateChildPermission(AppPermissions.Pages_Master_CKD_CustomsLeadtime_Edit, L("MasterCKDCustomsLeadtimeEdit"), multiTenancySides: MultiTenancySides.Tenant);

            //CustomsLeadTimeMaster
            var perMstCkdCustomsLeadTimeMaster = masterCkd.CreateChildPermission(AppPermissions.Pages_Master_CKD_CustomsLeadTimeMaster_View, L("Pages_Master_CKD_CustomsLeadtimeMaster_View"), multiTenancySides: MultiTenancySides.Tenant);
            perMstCkdCustomsLeadTimeMaster.CreateChildPermission(AppPermissions.Pages_Master_CKD_CustomsLeadTimeMaster_Edit, L("Pages_Master_CKD_CustomsLeadtimeMaster_Edit"), multiTenancySides: MultiTenancySides.Tenant);
            masterCkd.CreateChildPermission(AppPermissions.Pages_Master_CKD_CustomsLeadTimeMaster_Import, L("Pages_Master_CKD_CustomsLeadtimeMaster_Import"), multiTenancySides: MultiTenancySides.Tenant);
            //DemDetFees
            var perMstInvDemDetFees = masterCkd.CreateChildPermission(AppPermissions.Pages_Master_CKD_DemDetFees_View, L("Pages_Master_CKD_DemDetFees_View"), multiTenancySides: MultiTenancySides.Tenant);
            perMstInvDemDetFees.CreateChildPermission(AppPermissions.Pages_Master_CKD_DemDetFees_Edit, L("Pages_Master_CKD_DemDetFees_Edit"), multiTenancySides: MultiTenancySides.Tenant);
            masterCkd.CreateChildPermission(AppPermissions.Pages_Master_CKD_DemDetFees_Import, L("Pages_Master_CKD_DemDetFees_Import"), multiTenancySides: MultiTenancySides.Tenant);
			//Master Dem Det Days
			var perMstInvDemDetDays = masterCkd.CreateChildPermission(AppPermissions.Pages_Master_CKD_DemDetDays_View, L("Pages_Master_CKD_DemDetDays_View"), multiTenancySides: MultiTenancySides.Tenant);
			perMstInvDemDetDays.CreateChildPermission(AppPermissions.Pages_Master_CKD_DemDetDays_Edit, L("Pages_Master_CKD_DemDetDays_Edit"), multiTenancySides: MultiTenancySides.Tenant);
			perMstInvDemDetDays.CreateChildPermission(AppPermissions.Pages_Master_CKD_DemDetDays_Import, L("Pages_Master_CKD_DemDetDays_Import"), multiTenancySides: MultiTenancySides.Tenant);


			// Master_Gps 
			var masterGps = master.CreateChildPermission(AppPermissions.Pages_Master_Gps, L("MasterGps"), multiTenancySides: MultiTenancySides.Tenant);

            //Master_Gps_GpsSupplierInfo
            var perMstInvGpsSupplierInfo = masterGps.CreateChildPermission(AppPermissions.Pages_Master_Gps_GpsSupplierInfo_View, L("Pages_Master_Gps_GpsSupplierInfo_View"), multiTenancySides: MultiTenancySides.Tenant);
            perMstInvGpsSupplierInfo.CreateChildPermission(AppPermissions.Pages_Master_Gps_GpsSupplierInfo_Edit, L("MasterGpsGpsSupplierInfoEdit"), multiTenancySides: MultiTenancySides.Tenant);
        

            //Master_Gps_GpsCalendar
            var perMstInvGpsCalendar = masterGps.CreateChildPermission(AppPermissions.Pages_Master_Gps_GpsCalendar_View, L("Pages_Master_Gps_GpsCalendar_View"), multiTenancySides: MultiTenancySides.Tenant);
            perMstInvGpsCalendar.CreateChildPermission(AppPermissions.Pages_Master_Gps_GpsCalendar_Edit, L("MasterGpsGpsCalendarEdit"), multiTenancySides: MultiTenancySides.Tenant);
         

            //Master_Gps_GpsTmvPic
            var perMstInvGpsTmvPic = masterGps.CreateChildPermission(AppPermissions.Pages_Master_Gps_GpsTmvPic_View, L("Pages_Master_Gps_GpsTmvPic_View"), multiTenancySides: MultiTenancySides.Tenant);
            perMstInvGpsTmvPic.CreateChildPermission(AppPermissions.Pages_Master_Gps_GpsTmvPic_Edit, L("MasterGpsGpsTmvPicEdit"), multiTenancySides: MultiTenancySides.Tenant);
           

            //Master_Gps_GpsTruckSupplier
            var perMstInvGpsTruckSupplier = masterGps.CreateChildPermission(AppPermissions.Pages_Master_Gps_GpsTruckSupplier_View, L("Pages_Master_Gps_GpsTruckSupplier_View"), multiTenancySides: MultiTenancySides.Tenant);
            perMstInvGpsTruckSupplier.CreateChildPermission(AppPermissions.Pages_Master_Gps_GpsTruckSupplier_Edit, L("MasterGpsGpsTruckSupplierEdit"), multiTenancySides: MultiTenancySides.Tenant);
         

            //Master_Gps_GpsSupplierPic
            var perMstInvGpsSupplierPic = masterGps.CreateChildPermission(AppPermissions.Pages_Master_Gps_GpsSupplierPic_View, L("Pages_Master_Gps_GpsSupplierPic_View"), multiTenancySides: MultiTenancySides.Tenant);
            perMstInvGpsSupplierPic.CreateChildPermission(AppPermissions.Pages_Master_Gps_GpsSupplierPic_Edit, L("MasterGpsGpsSupplierPicEdit"), multiTenancySides: MultiTenancySides.Tenant);
          

            //Master_Gps_GpsScreenSetting
            var perMstInvGpsScreenSetting = masterGps.CreateChildPermission(AppPermissions.Pages_Master_Gps_GpsScreenSetting_View, L("Pages_Master_Gps_GpsScreenSetting_View"), multiTenancySides: MultiTenancySides.Tenant);
            perMstInvGpsScreenSetting.CreateChildPermission(AppPermissions.Pages_Master_Gps_GpsScreenSetting_Edit, L("MasterGpsGpsScreenSettingEdit"), multiTenancySides: MultiTenancySides.Tenant);
       
             //Master_Gps_GpsCategory
            var perMstInvGpsCategory = masterGps.CreateChildPermission(AppPermissions.Pages_Master_Gps_GpsCategory_View, L("Pages_Master_Gps_GpsCategory_View"), multiTenancySides: MultiTenancySides.Tenant);


            //Master_Gps_Master_MaterialCategory
            var perGpsMstMaterialCategory = masterGps.CreateChildPermission(AppPermissions.Pages_Master_Gps_MaterialCategory_View, L("Pages_Master_Gps_MaterialCategory_View"), multiTenancySides: MultiTenancySides.Tenant);
            perGpsMstMaterialCategory.CreateChildPermission(AppPermissions.Pages_Master_Gps_MaterialCategory_CreateEdit, L("Pages_Master_Gps_MaterialCategory_CreateEdit"), multiTenancySides: MultiTenancySides.Tenant);
            masterGps.CreateChildPermission(AppPermissions.Pages_Master_Gps_MaterialCategory_Import, L("Pages_Master_Gps_MaterialCategory_Import"), multiTenancySides: MultiTenancySides.Tenant);

			//Master_Gps_Master_MaterialCategory
			var perGpsMstMaterialCategoryMapping = masterGps.CreateChildPermission(AppPermissions.Pages_Master_Gps_MaterialCategoryMapping_View, L("Pages_Master_Gps_MaterialCategoryMapping_View"), multiTenancySides: MultiTenancySides.Tenant);
			perGpsMstMaterialCategoryMapping.CreateChildPermission(AppPermissions.Pages_Master_Gps_MaterialCategoryMapping_CreateEdit, L("Pages_Master_Gps_MaterialCategoryMapping_CreateEdit"), multiTenancySides: MultiTenancySides.Tenant);
			masterGps.CreateChildPermission(AppPermissions.Pages_Master_Gps_MaterialCategoryMapping_Import, L("Pages_Master_Gps_MaterialCategoryMapping_Import"), multiTenancySides: MultiTenancySides.Tenant);
            perGpsMstMaterialCategoryMapping.CreateChildPermission(AppPermissions.Pages_Master_Gps_MaterialCategoryMapping_History, L("Pages_Master_Gps_MaterialCategoryMapping_History"), multiTenancySides: MultiTenancySides.Tenant);


            //Master_Gps_GpsMaterialRegisterByShop
            var perMstGpsMaterialRegisterByShop = masterGps.CreateChildPermission(AppPermissions.Pages_Master_Gps_GpsMaterialRegisterByShop_View, L("Pages_Master_Gps_GpsMaterialRegisterByShop_View"), multiTenancySides: MultiTenancySides.Tenant);
            perMstGpsMaterialRegisterByShop.CreateChildPermission(AppPermissions.Pages_Master_Gps_GpsMaterialRegisterByShop_Edit, L("Pages_Master_Gps_GpsMaterialRegisterByShop_Edit"), multiTenancySides: MultiTenancySides.Tenant);

            //Master_Gps_Uom
			var perMstGpsUom = masterGps.CreateChildPermission(AppPermissions.Pages_Master_Gps_Uom_View, L("Pages_Master_Gps_Uom_View"), multiTenancySides: MultiTenancySides.Tenant);
            perMstGpsUom.CreateChildPermission(AppPermissions.Pages_Master_Gps_Uom_Edit, L("Pages_Master_Gps_Uom_Edit"), multiTenancySides: MultiTenancySides.Tenant);

			//Master Gps WbsCCMapping 
			var perGpsWbsCCMapping = masterGps.CreateChildPermission(AppPermissions.Pages_Master_Gps_WbsCCMapping_View, L("Pages_Master_Gps_WbsCCMapping_View"), multiTenancySides: MultiTenancySides.Tenant);
			perGpsWbsCCMapping.CreateChildPermission(AppPermissions.Pages_Master_Gps_WbsCCMapping_CreateEdit, L("Pages_Master_Gps_WbsCCMapping_CreateEdit"), multiTenancySides: MultiTenancySides.Tenant);
			masterGps.CreateChildPermission(AppPermissions.Pages_Master_Gps_WbsCCMapping_Import, L("Pages_Master_Gps_WbsCCMapping_Import"), multiTenancySides: MultiTenancySides.Tenant);
			perGpsWbsCCMapping.CreateChildPermission(AppPermissions.Pages_Master_Gps_WbsCCMapping_History, L("Pages_Master_Gps_WbsCCMapping_History"), multiTenancySides: MultiTenancySides.Tenant);

			//Mst_gps_CostCenter
			var perGpsMstCostCenter = masterGps.CreateChildPermission(AppPermissions.Pages_Master_Gps_CostCenter_View, L("Pages_Master_Gps_CostCenter_View"), multiTenancySides: MultiTenancySides.Tenant);
            perGpsMstCostCenter.CreateChildPermission(AppPermissions.Pages_Master_Gps_CostCenter_CreateEdit, L("Pages_Master_Gps_CostCenter_CreateEdit"), multiTenancySides: MultiTenancySides.Tenant);
            masterGps.CreateChildPermission(AppPermissions.Pages_Master_Gps_CostCenter_Import, L("Pages_Master_Gps_CostCenter_Import"), multiTenancySides: MultiTenancySides.Tenant);

            // Master_Cps
            var masterCps = master.CreateChildPermission(AppPermissions.Pages_Master_Cps, L("MasterCps"), multiTenancySides: MultiTenancySides.Tenant);

            //Master_Cps_CpsSuppliers
            var perMstInvCpsSuppliers = masterCps.CreateChildPermission(AppPermissions.Pages_Master_Cps_CpsSuppliers_View, L("Pages_Master_Cps_CpsSuppliers_View"), multiTenancySides: MultiTenancySides.Tenant);
          

            //Master_Cps_CpsInventoryGroup
            var perMstInvCpsInventoryGroup = masterCps.CreateChildPermission(AppPermissions.Pages_Master_Cps_CpsInventoryGroup_View, L("Pages_Master_Cps_CpsInventoryGroup_View"), multiTenancySides: MultiTenancySides.Tenant);

            //Master_Cps_InventoryItems
            var perMstInvCpsInventoryItems = masterCps.CreateChildPermission(AppPermissions.Pages_Master_Inventory_CpsInventoryItems_View, L("Pages_Master_Inventory_CpsInventoryItems_View"), multiTenancySides: MultiTenancySides.Tenant);

            // Master_Hr
            var masterHr = master.CreateChildPermission(AppPermissions.Pages_Master_Hr, L("MasterHr"), multiTenancySides: MultiTenancySides.Tenant);

            //Master_Hr_HrOrgStructure
            var perMstInvHrOrgStructure = masterHr.CreateChildPermission(AppPermissions.Pages_Master_Hr_HrOrgStructure_View, L("Pages_Master_Hr_HrOrgStructure_View"), multiTenancySides: MultiTenancySides.Tenant);

            //Master_Hr_HrEmployee
            var perMstInvHrEmpolyeeData = masterHr.CreateChildPermission(AppPermissions.Pages_Master_Hr_HrEmployee_View, L("Pages_Master_Hr_HrEmployee_View"), multiTenancySides: MultiTenancySides.Tenant);

            //Master_Hr_HrglcodeCombination
            var perMstHrglcodeCombination = masterHr.CreateChildPermission(AppPermissions.Pages_Master_Hr_HrGlcodeCombination_View, L("Pages_Master_Hr_HrGlcodeCombination_View"), multiTenancySides: MultiTenancySides.Tenant);

            //Master_Hr_HrPosition
            var perMasterInvHrHrPosition = masterHr.CreateChildPermission(AppPermissions.Pages_Master_Hr_HrPosition_View, L("Pages_Master_Hr_HrPosition_View"), multiTenancySides: MultiTenancySides.Tenant);

            //Master_Hr_HrTitles
            var perMasterInvHrHrTitles = masterHr.CreateChildPermission(AppPermissions.Pages_Master_Hr_HrTitles_View, L("Pages_Master_Hr_HrTitles_View"), multiTenancySides: MultiTenancySides.Tenant);


			#endregion

			#region INVT. SETUP
			// INVT. SETUP
			var invtSetup = pages.CreateChildPermission(AppPermissions.Pages_InvtSetup, L("InvtSetup"), multiTenancySides: MultiTenancySides.Tenant);
           
            //InvtSetup_CommonSetup
            var invtSetupCommonSetup = invtSetup.CreateChildPermission(AppPermissions.Pages_InvtSetupCommonSetup, L("InvtSetupCommonSetup"), multiTenancySides: MultiTenancySides.Tenant);

            //InvtSetup_MaterialGroup
            var perMstCmmMaterialGroup = invtSetupCommonSetup.CreateChildPermission(AppPermissions.Pages_InvtSetup_MaterialGroup_View, L("Pages_InvtSetup_CommonSetup_MaterialGroup_View"), multiTenancySides: MultiTenancySides.Tenant);

            //InvtSetup_Plant
            var perMstCmmPlant = invtSetupCommonSetup.CreateChildPermission(AppPermissions.Pages_InvtSetup_Plant_View, L("Pages_InvtSetup_CommonSetup_Plant_View"), multiTenancySides: MultiTenancySides.Tenant);

            // InvtSetup_StorageLocation
            var perMstCmmStorageLocation = invtSetupCommonSetup.CreateChildPermission(AppPermissions.Pages_InvtSetup_StorageLocation_View, L("Pages_InvtSetup_CommonSetup_StorageLocation_View"), multiTenancySides: MultiTenancySides.Tenant);

            // InvtSetup_StorageLocationCategory
            var perMstCmmStorageLocationCategory = invtSetupCommonSetup.CreateChildPermission(AppPermissions.Pages_InvtSetup_StorageLocationCategory_View, L("Pages_InvtSetup_CommonSetup_StorageLocationCategory_View"), multiTenancySides: MultiTenancySides.Tenant);

            //InvtSetup_Brand
            var perMstCmmBrand = invtSetupCommonSetup.CreateChildPermission(AppPermissions.Pages_InvtSetup_Brand_View, L("Pages_InvtSetup_CommonSetup_Brand_View"), multiTenancySides: MultiTenancySides.Tenant);

            //InvtSetup_FuelType
            var perMstCmmFuelType = invtSetupCommonSetup.CreateChildPermission(AppPermissions.Pages_InvtSetup_FuelType_View, L("Pages_InvtSetup_CommonSetup_FuelType_View"), multiTenancySides: MultiTenancySides.Tenant);

            //InvtSetup_ProductType
            var perMstCmmProductType = invtSetupCommonSetup.CreateChildPermission(AppPermissions.Pages_InvtSetup_ProductType_View, L("Pages_InvtSetup_CommonSetup_ProductType_View"), multiTenancySides: MultiTenancySides.Tenant);

            //InvtSetup_TransmissionType
            var perMstCmmTransmissionType = invtSetupCommonSetup.CreateChildPermission(AppPermissions.Pages_InvtSetup_TransmissionType_View, L("Pages_InvtSetup_CommonSetup_TransmissionType"), multiTenancySides: MultiTenancySides.Tenant);
           
            //InvtSetup_Uom
            var perMstCmmUom = invtSetupCommonSetup.CreateChildPermission(AppPermissions.Pages_InvtSetup_Uom_View, L("Pages_InvtSetup_CommonSetup_Uom_View"), multiTenancySides: MultiTenancySides.Tenant);
          //  perMstCmmUom.CreateChildPermission(AppPermissions.Pages_InvtSetup_Uom_Edit, L("InvtSetupUomEdit"), multiTenancySides: MultiTenancySides.Tenant);

            //InvtSetup_ValuationClass
            var perMstCmmValuationClass = invtSetupCommonSetup.CreateChildPermission(AppPermissions.Pages_InvtSetup_ValuationClass_View, L("Pages_InvtSetup_CommonSetup_ValuationClass_View"), multiTenancySides: MultiTenancySides.Tenant);

            //InvtSetup_ValuationType
            var perMstCmmValuationType = invtSetupCommonSetup.CreateChildPermission(AppPermissions.Pages_InvtSetup_ValuationType_View, L("Pages_InvtSetup_CommonSetup_ValuationType_View"), multiTenancySides: MultiTenancySides.Tenant);

            //InvtSetup_MaterialType
            var perMstCmmMaterialType = invtSetupCommonSetup.CreateChildPermission(AppPermissions.Pages_InvtSetup_MaterialType_View, L("Pages_InvtSetup_CommonSetup_MaterialType_View"), multiTenancySides: MultiTenancySides.Tenant);
         
            //InvtSetup_Brand
            var perInvGenBOMData = invtSetupCommonSetup.CreateChildPermission(AppPermissions.Pages_InvtSetup_MstInvGenBOMData_View, L("Pages_InvtSetup_CommonSetup_MstInvGenBOMData_View"), multiTenancySides: MultiTenancySides.Tenant);


            //InvtSetup_CarSeries
            var perMstCmmCarSeries = invtSetupCommonSetup.CreateChildPermission(AppPermissions.Pages_InvtSetup_CarSeries_View, L("Pages_InvtSetup_CommonSetup_CarSeries_View"), multiTenancySides: MultiTenancySides.Tenant);

            //InvtSetup_Carfamily
            var perMstCmmCarfamily = invtSetupCommonSetup.CreateChildPermission(AppPermissions.Pages_InvtSetup_Carfamily_View, L("Pages_InvtSetup_CommonSetup_Carfamily_View"), multiTenancySides: MultiTenancySides.Tenant);

            //InvtSetup_DriveTrain
            var perMstCmmDriveTrain = invtSetupCommonSetup.CreateChildPermission(AppPermissions.Pages_InvtSetup_DriveTrain_View, L("Pages_InvtSetup_CommonSetup_DriveTrain_View"), multiTenancySides: MultiTenancySides.Tenant);

            //Master - Common - ProductGroup
            var perMstCmmProductGroup = invtSetupCommonSetup.CreateChildPermission(AppPermissions.Pages_Master_Cmm_ProductGroup_View, L("Pages_InvtSetup_CommonSetup_ProductGroup_View"), multiTenancySides: MultiTenancySides.Tenant);
            //perMstCmmProductGroup.CreateChildPermission(AppPermissions.Pages_Master_Cmm_ProductGroup_CreateEdit, L("Pages_Master_Cmm_ProductGroup_CreateEdit"), multiTenancySides: MultiTenancySides.Tenant);
            //perMstCmmProductGroup.CreateChildPermission(AppPermissions.Pages_Master_Cmm_ProductGroup_Delete, L("Pages_Master_Cmm_ProductGroup_Delete"), multiTenancySides: MultiTenancySides.Tenant);

            //Master Cmm Engine Model
            var perMstCmmEngineModel = invtSetupCommonSetup.CreateChildPermission(AppPermissions.Pages_Master_Cmm_EngineModel_View, L("Pages_InvtSetup_CommonSetup_EngineModel_View"), multiTenancySides: MultiTenancySides.Tenant);
            //perMstCmmEngineModel.CreateChildPermission(AppPermissions.Pages_Master_Cmm_EngineModel_CreateEdit, L("Pages_Master_Cmm_EngineModel_CreateEdit"), multiTenancySides: MultiTenancySides.Tenant);
            //perMstCmmEngineModel.CreateChildPermission(AppPermissions.Pages_Master_Cmm_EngineModel_Delete, L("Pages_Master_Cmm_EngineModel_Delete"), multiTenancySides: MultiTenancySides.Tenant);


            //Master Cmm Engine Type 
            var perMstCmmEngineType = invtSetupCommonSetup.CreateChildPermission(AppPermissions.Pages_Master_Cmm_EngineType_View, L("Pages_InvtSetup_CommonSetup_EngineType_View"), multiTenancySides: MultiTenancySides.Tenant);
            //perMstCmmEngineType.CreateChildPermission(AppPermissions.Pages_Master_Cmm_EngineType_CreateEdit, L("Pages_Master_Cmm_EngineType_CreateEdit"), multiTenancySides: MultiTenancySides.Tenant);
            //perMstCmmEngineType.CreateChildPermission(AppPermissions.Pages_Master_Cmm_EngineType_Delete, L("Pages_Master_Cmm_EngineType_Delete"), multiTenancySides: MultiTenancySides.Tenant);

            //Master Cmm Tax
            var perMstCmmTax = invtSetupCommonSetup.CreateChildPermission(AppPermissions.Pages_Master_Cmm_Tax_View, L("Pages_InvtSetup_CommonSetup_Tax_View"), multiTenancySides: MultiTenancySides.Tenant);
            //perMstCmmTax.CreateChildPermission(AppPermissions.Pages_Master_Cmm_Tax_CreateEdit, L("Pages_Master_Cmm_Tax_CreateEdit"), multiTenancySides: MultiTenancySides.Tenant);
            //perMstCmmTax.CreateChildPermission(AppPermissions.Pages_Master_Cmm_Tax_Delete, L("Pages_Master_Cmm_Tax_Delete"), multiTenancySides: MultiTenancySides.Tenant);


            //InvtSetup_CheckingRule
            var invtSetupCheckingRule = invtSetup.CreateChildPermission(AppPermissions.Pages_InvtSetupCheckingRule, L("InvtSetup_CheckingRule"), multiTenancySides: MultiTenancySides.Tenant);
            
            //MstCmmMMCheckingRule
            var perMstCmmMMCheckingRule = invtSetupCheckingRule.CreateChildPermission(AppPermissions.Pages_Master_Cmm_MMCheckingRule_View, L("Pages_InvtSetup_CheckingRule_MMCheckingRule_View"), multiTenancySides: MultiTenancySides.Tenant);
            perMstCmmMMCheckingRule.CreateChildPermission(AppPermissions.Pages_Master_Cmm_MMCheckingRule_Import, L("Pages_InvtSetup_CheckingRule_MMCheckingRule_Import"), multiTenancySides: MultiTenancySides.Tenant);
            perMstCmmMMCheckingRule.CreateChildPermission(AppPermissions.Pages_Master_Cmm_MMCheckingRule_History, L("Pages_InvtSetup_CheckingRule_MMCheckingRule_Histo"), multiTenancySides: MultiTenancySides.Tenant);


          


            //MM Validation Result
            var perMstCmmMMValidationResult = invtSetupCheckingRule.CreateChildPermission(AppPermissions.Pages_Master_Cmm_MMValidationResult_View, L("Pages_InvtSetup_CheckingRule_MMValidationResult_View"), multiTenancySides: MultiTenancySides.Tenant);
            perMstCmmMMValidationResult = invtSetupCheckingRule.CreateChildPermission(AppPermissions.Pages_Master_Cmm_MMValidationResult_History, L("Pages_InvtSetup_CheckingRule_MMValidationResult_Hisory"), multiTenancySides: MultiTenancySides.Tenant);


            #endregion

            #region MATERIAL MASTER
            // MATERIAL MASTER 
            var materialMaster = pages.CreateChildPermission(AppPermissions.Pages_MaterialMaster, L("MaterialMaster"), multiTenancySides: MultiTenancySides.Tenant);

            // MaterialMaster_MaterialMaster
            var perMstCmmMaterialMaster = materialMaster.CreateChildPermission(AppPermissions.Pages_MaterialMaster_MaterialMaster_View, L("Pages_MaterialMaster_MaterialMaster_View"), multiTenancySides: MultiTenancySides.Tenant);
            perMstCmmMaterialMaster.CreateChildPermission(AppPermissions.Pages_MaterialMaster_MaterialMaster_Validate, L("Pages_MaterialMaster_MaterialMaster_Validate"), multiTenancySides: MultiTenancySides.Tenant);
            perMstCmmMaterialMaster.CreateChildPermission(AppPermissions.Pages_MaterialMaster_MaterialMaster_History, L("Pages_MaterialMaster_MaterialMaster_History"), multiTenancySides: MultiTenancySides.Tenant);


            //MaterialMaster_Vehicle
            var perMaterialMasterVehicle = materialMaster.CreateChildPermission(AppPermissions.Pages_MaterialMaster_Vehicle_View, L("Pages_MaterialMaster_Vehicle_View"), multiTenancySides: MultiTenancySides.Tenant);
            perMaterialMasterVehicle.CreateChildPermission(AppPermissions.Pages_MaterialMaster_Vehicle_Validate, L("Pages_MaterialMaster_Vehicle_Validate"), multiTenancySides: MultiTenancySides.Tenant);
            perMaterialMasterVehicle.CreateChildPermission(AppPermissions.Pages_MaterialMaster_Vehicle_Edit, L("Pages_MaterialMaster_Vehicle_Edit"), multiTenancySides: MultiTenancySides.Tenant);

            #endregion

            #region CPS LINKAGE
            // MATERIAL MASTER 
            var cpsLinkAge = pages.CreateChildPermission(AppPermissions.Pages_CpsLinkAge, L("CpsLinkAge"), multiTenancySides: MultiTenancySides.Tenant);

            var perInvCpsSapAssetMaster = cpsLinkAge.CreateChildPermission(AppPermissions.Pages_CpsLinkAge_SapAssetMaster_View, L("Pages_CpsLinkAge_SapAssetMaster_View"), multiTenancySides: MultiTenancySides.Tenant);

            var perInvCpsInvoiceLines = cpsLinkAge.CreateChildPermission(AppPermissions.Pages_CpsLinkAge_InvoiceHeaders_View, L("Pages_CpsLinkAge_InvoiceHeaders_View"), multiTenancySides: MultiTenancySides.Tenant);

            var perInvCpsPoHeaders = cpsLinkAge.CreateChildPermission(AppPermissions.Pages_CpsLinkAge_PoHeaders_View, L("Pages_CpsLinkAge_PoHeaders_View"), multiTenancySides: MultiTenancySides.Tenant);

            var perInvCpsRcvShipmentHeaders = cpsLinkAge.CreateChildPermission(AppPermissions.Pages_CpsLinkAge_RcvShipmentHeaders_View, L("Pages_CpsLinkAge_RcvShipmentHeaders_View"), multiTenancySides: MultiTenancySides.Tenant);
           
            var perInvCpsInventoryItemPrice = cpsLinkAge.CreateChildPermission(AppPermissions.Pages_CpsLinkAge_InventoryItemPrice_View, L("Pages_CpsLinkAge_InventoryItemPrice_View"), multiTenancySides: MultiTenancySides.Tenant);

            #endregion

            #region PROD. PLAN
            // PROD. PLAN 
            var prodPlan = pages.CreateChildPermission(AppPermissions.Pages_ProdPlan, L("ProdPlan"), multiTenancySides: MultiTenancySides.Tenant);

            //ProdPlan_ProdPlanDaily
            var perInvCkdProdPlanDaily = prodPlan.CreateChildPermission(AppPermissions.Pages_ProdPlan_ProdPlanDaily_View, L("Pages_ProdPlan_ProdPlanDaily_View"), multiTenancySides: MultiTenancySides.Tenant);
            perInvCkdProdPlanDaily = prodPlan.CreateChildPermission(AppPermissions.Pages_ProdPlan_ProdPlanDaily_History, L("Pages_ProdPlan_ProdPlanDaily_History"), multiTenancySides: MultiTenancySides.Tenant);


            //ProdPlan_ProductionPlanMonthly
            var perInvCkdProductionPlanMonthly = prodPlan.CreateChildPermission(AppPermissions.Pages_ProdPlan_ProductionPlanMonthly_View, L("Pages_ProdPlan_ProductionPlanMonthly_View"), multiTenancySides: MultiTenancySides.Tenant);
            perInvCkdProductionPlanMonthly.CreateChildPermission(AppPermissions.Pages_ProdPlan_ProductionPlanMonthly_Import, L("Pages_ProdPlan_ProductionPlanMonthly_Import"), multiTenancySides: MultiTenancySides.Tenant);


            //ProdPlan_AInPlan
            var perAsyAdoAInPlan = prodPlan.CreateChildPermission(AppPermissions.Pages_ProdPlan_AInPlan_View, L("Pages_ProdPlan_AInPlan_View"), multiTenancySides: MultiTenancySides.Tenant);
            perAsyAdoAInPlan = prodPlan.CreateChildPermission(AppPermissions.Pages_ProdPlan_AInPlan_History, L("Pages_ProdPlan_AInPlan_History"), multiTenancySides: MultiTenancySides.Tenant);

            //ProdPlan_APlanShiftBase
            var perAsyAdoAPlanShift = prodPlan.CreateChildPermission(AppPermissions.Pages_ProdPlan_APlanShiftBase_View, L("Pages_ProdPlan_APlanShiftBase_View"));

            //ProdPlan_AssemblyData
            var perAsyAdoAssemblyData = prodPlan.CreateChildPermission(AppPermissions.Pages_ProdPlan_AssemblyData_View, L("Pages_ProdPlan_AssemblyData_View"), multiTenancySides: MultiTenancySides.Tenant);

            //ProdPlan_VehicleDetails
            var perAsyAdoVehicleDetails = prodPlan.CreateChildPermission(AppPermissions.Pages_ProdPlan_VehicleDetails_View, L("Pages_ProdPlan_VehicleDetails_View"), multiTenancySides: MultiTenancySides.Tenant);
            perAsyAdoVehicleDetails = prodPlan.CreateChildPermission(AppPermissions.Pages_ProdPlan_VehicleDetails_History, L("Pages_ProdPlan_VehicleDetails_History"), multiTenancySides: MultiTenancySides.Tenant);

            //ProdPlan_WeldingPlan
            var wldAdoWeldingPlan = prodPlan.CreateChildPermission(AppPermissions.Pages_ProdPlan_WeldingPlan_View, L("Pages_ProdPlan_WeldingPlan_View"), multiTenancySides: MultiTenancySides.Tenant);
            wldAdoWeldingPlan.CreateChildPermission(AppPermissions.Pages_ProdPlan_WeldingPlan_Edit, L("Pages_ProdPlan_WeldingPlan_Edit"), multiTenancySides: MultiTenancySides.Tenant);
            wldAdoWeldingPlan.CreateChildPermission(AppPermissions.Pages_ProdPlan_WeldingPlan_Import, L("Pages_ProdPlan_WeldingPlan_Import"), multiTenancySides: MultiTenancySides.Tenant);
            wldAdoWeldingPlan.CreateChildPermission(AppPermissions.Pages_ProdPlan_WeldingPlan_History, L("Pages_ProdPlan_WeldingPlan_History"), multiTenancySides: MultiTenancySides.Tenant);

            //ProdPlan_WeldingProgress
            var wldAdoWeldingProgress = prodPlan.CreateChildPermission(AppPermissions.Pages_ProdPlan_WeldingProgress_View, L("Pages_ProdPlan_WeldingProgress_View"), multiTenancySides: MultiTenancySides.Tenant);
            wldAdoWeldingProgress.CreateChildPermission(AppPermissions.Pages_ProdPlan_WeldingProgress_Edit, L("Pages_ProdPlan_WeldingProgress_Edit"), multiTenancySides: MultiTenancySides.Tenant);
          
            //ProdPlan_PaintingData
            var perPtsAdoPaintingData = prodPlan.CreateChildPermission(AppPermissions.Pages_ProdPlan_PaintingData_View, L("Pages_ProdPlan_PaintingData_View"), multiTenancySides: MultiTenancySides.Tenant);

            //ProdPlan_ScanInfo
            var ptsAdoScanInfo = prodPlan.CreateChildPermission(AppPermissions.Pages_ProdPlan_ScanInfo_View, L("Pages_ProdPlan_ScanInfo_View"), multiTenancySides: MultiTenancySides.Tenant);
           
            //ProdPlan_ProductionMapping
            var perInvProcproducitonMapping = prodPlan.CreateChildPermission(AppPermissions.Pages_ProdPlan_ProductionMapping_View, L("Pages_ProdPlan_ProductionMapping_View"), multiTenancySides: MultiTenancySides.Tenant);
            perInvProcproducitonMapping.CreateChildPermission(AppPermissions.Pages_ProdPlan_ProductionMapping_Import, L("Pages_ProdPlan_ProductionMapping_Import"), multiTenancySides: MultiTenancySides.Tenant);
            perInvProcproducitonMapping.CreateChildPermission(AppPermissions.Pages_ProdPlan_ProductionMapping_History, L("Pages_ProdPlan_ProductionMapping_History"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion

            #region CKD
            // ckd 
            var ckd = pages.CreateChildPermission(AppPermissions.Pages_Ckd, L("Ckd"), multiTenancySides: MultiTenancySides.Tenant);
            #region Ckd_Master
            //Ckd_Master
            var ckdMaster = ckd.CreateChildPermission(AppPermissions.Pages_Ckd_Master, L("CkdMaster"), multiTenancySides: MultiTenancySides.Tenant);

            //Ckd_Master_PartList
            var perInvCkdPartList = ckdMaster.CreateChildPermission(AppPermissions.Pages_Ckd_Master_PartList_View, L("Pages_Ckd_Master_PartList_View"), multiTenancySides: MultiTenancySides.Tenant);
            perInvCkdPartList.CreateChildPermission(AppPermissions.Pages_Ckd_Master_PartList_Import, L("Pages_Ckd_Master_PartList_Import"), multiTenancySides: MultiTenancySides.Tenant);
            perInvCkdPartList.CreateChildPermission(AppPermissions.Pages_Ckd_Master_PartList_Validate, L("Pages_Ckd_Master_PartList_Validate"), multiTenancySides: MultiTenancySides.Tenant);
            perInvCkdPartList.CreateChildPermission(AppPermissions.Pages_Ckd_Master_PartList_Add, L("Pages_Ckd_Master_PartList_Add"), multiTenancySides: MultiTenancySides.Tenant);
            perInvCkdPartList.CreateChildPermission(AppPermissions.Pages_Ckd_Master_PartList_History, L("Pages_Ckd_Master_PartList_History"), multiTenancySides: MultiTenancySides.Tenant);

            //InvGpsUser
            var perInvGpsUser = ckdMaster.CreateChildPermission(AppPermissions.Pages_Gps_User, L("Pages_Gps_User"), multiTenancySides: MultiTenancySides.Tenant);


            //Ckd_Master_LotPart
            var perInvCkdLotPart = ckdMaster.CreateChildPermission(AppPermissions.Pages_Ckd_Master_LotPart_View, L("Pages_Ckd_Master_LotPart_View"), multiTenancySides: MultiTenancySides.Tenant);

			//

			var perMstInvDevanningCaseType = ckdMaster.CreateChildPermission(AppPermissions.Pages_Master_Inv_DevanningCaseType_View, L("Pages_Master_Inv_DevanningCaseType_View"), multiTenancySides: MultiTenancySides.Tenant);
			perMstInvDevanningCaseType.CreateChildPermission(AppPermissions.Pages_Master_Inv_DevanningCaseType_Edit, L("Pages_Master_Inv_DevanningCaseType_Edit"), multiTenancySides: MultiTenancySides.Tenant);
			#endregion

			#region Ckd_Intransit
			//Ckd_Intransit
			var ckdIntransit = ckd.CreateChildPermission(AppPermissions.Pages_Ckd_Intransit, L("CkdIntransit"), multiTenancySides: MultiTenancySides.Tenant);

            //Ckd_Intransit_Shipment
            var perInvCkdShipment = ckdIntransit.CreateChildPermission(AppPermissions.Pages_Ckd_Intransit_Shipment_View, L("Pages_Ckd_Intransit_Shipment_View"), multiTenancySides: MultiTenancySides.Tenant);
 
            //Ckd_Intransit_Bill
            var perInvCkdBill = ckdIntransit.CreateChildPermission(AppPermissions.Pages_Ckd_Intransit_Bill_View, L("Pages_Ckd_Intransit_Bill_View"), multiTenancySides: MultiTenancySides.Tenant);
            perInvCkdBill.CreateChildPermission(AppPermissions.Pages_Ckd_Intransit_Bill_History, L("Pages_Ckd_Intransit_Bill_History"), multiTenancySides: MultiTenancySides.Tenant);

            //Ckd_Intransit_Invoice
            var perInvCkdInvoice = ckdIntransit.CreateChildPermission(AppPermissions.Pages_Ckd_Intransit_Invoice_View, L("Pages_Ckd_Intransit_Invoice_View"), multiTenancySides: MultiTenancySides.Tenant);
            perInvCkdInvoice.CreateChildPermission(AppPermissions.Pages_Ckd_Intransit_Invoice_History, L("Pages_Ckd_Intransit_Invoice_History"), multiTenancySides: MultiTenancySides.Tenant);

            //Ckd_Intransit_InvoiceDetails
            var perInvCkdInvoiceDetails = ckdIntransit.CreateChildPermission(AppPermissions.Pages_Ckd_Intransit_InvoiceDetails_History, L("Pages_Ckd_Intransit_InvoiceDetails_History"), multiTenancySides: MultiTenancySides.Tenant);

            //Ckd_Intransit_FrameEngine
            var Pages_Ckd_Intransit_FrameEngine_View = ckdIntransit.CreateChildPermission(AppPermissions.Pages_Ckd_Intransit_FrameEngine_View, L("CkdIntransitFrameEngine"), multiTenancySides: MultiTenancySides.Tenant);

            //Ckd_Intransit_ContainerIntransit
            var perInvCkdContainerIntransit = ckdIntransit.CreateChildPermission(AppPermissions.Pages_Ckd_Intransit_ContainerIntransit_View, L("Pages_Ckd_Intransit_ContainerIntransit_View"), multiTenancySides: MultiTenancySides.Tenant);

            //Ckd_Intransit_ContainerInvoice
            var perInvCkdContainerInvoice = ckdIntransit.CreateChildPermission(AppPermissions.Pages_Ckd_Intransit_ContainerInvoice_View, L("Pages_Ckd_Intransit_ContainerInvoice_View"), multiTenancySides: MultiTenancySides.Tenant);
            perInvCkdContainerInvoice.CreateChildPermission(AppPermissions.Pages_Ckd_Intransit_ContainerInvoice_History, L("Pages_Ckd_Intransit_ContainerInvoice_History"), multiTenancySides: MultiTenancySides.Tenant);

            //Ckd_Intransit_PreCustoms
            var perCkdIntransitPreCustoms = ckdIntransit.CreateChildPermission(AppPermissions.Pages_Ckd_Intransit_PreCustoms_View, L("Pages_Ckd_Intransit_PreCustoms_View"), multiTenancySides: MultiTenancySides.Tenant);

            //Ckd_Intransit_CustomsDeclare
            var perCkdIntransitCustomsDeclare = ckdIntransit.CreateChildPermission(AppPermissions.Pages_Ckd_Intransit_CustomsDeclare_View, L("Pages_Ckd_Intransit_CustomsDeclare_View"), multiTenancySides: MultiTenancySides.Tenant);


            //Ckd_Intransit_PaymentRequest
            var perCkdIntransitPaymentRequest = ckdIntransit.CreateChildPermission(AppPermissions.Pages_Ckd_Intransit_PaymentRequest_View, L("Pages_Ckd_Intransit_PaymentRequest_View"), multiTenancySides: MultiTenancySides.Tenant);

            //Ckd_Intransit_ContainerTransitPortPlan
            var perInvCkdContainerTransitPortPlan = ckdIntransit.CreateChildPermission(AppPermissions.Pages_Ckd_Intransit_ContainerTransitPortPlan_View, L("Pages_Ckd_Intransit_ContainerTransitPortPlan_View"), multiTenancySides: MultiTenancySides.Tenant);
            perInvCkdContainerTransitPortPlan.CreateChildPermission(AppPermissions.Pages_Ckd_Intransit_ContainerTransitPortPlan_Edit, L("Pages_Ckd_Intransit_ContainerTransitPortPlan_Edit"), multiTenancySides: MultiTenancySides.Tenant);
            perInvCkdContainerTransitPortPlan.CreateChildPermission(AppPermissions.Pages_Ckd_Intransit_ContainerTransitPortPlan_Import, L("Pages_Ckd_Intransit_ContainerTransitPortPlan_Import"), multiTenancySides: MultiTenancySides.Tenant);
            perInvCkdContainerTransitPortPlan.CreateChildPermission(AppPermissions.Pages_Ckd_Intransit_ContainerTransitPortPlan_Confirm, L("Pages_Ckd_Intransit_ContainerTransitPortPlan_Confirm"), multiTenancySides: MultiTenancySides.Tenant);

            #endregion

            //đã làm tới đây
            #region Warehouse
            //Ckd_Warehouse
            var ckdWarehouse = ckd.CreateChildPermission(AppPermissions.Pages_Ckd_Warehouse, L("CkdWarehouse"), multiTenancySides: MultiTenancySides.Tenant);
            //Ckd_Warehouse_ContainerRentalWHPlan
            var perInvCkdContainerRentalWHPlan = ckdWarehouse.CreateChildPermission(AppPermissions.Pages_Ckd_Warehouse_ContainerRentalWHPlan_View, L("Pages_Ckd_Warehouse_ContainerRentalWHPlan_View"), multiTenancySides: MultiTenancySides.Tenant);
            perInvCkdContainerRentalWHPlan.CreateChildPermission(AppPermissions.Pages_Ckd_Warehouse_ContainerRentalWHPlan_Edit, L("CkdWarehouseContainerRentalWHPlanEdit"), multiTenancySides: MultiTenancySides.Tenant);
            perInvCkdContainerRentalWHPlan.CreateChildPermission(AppPermissions.Pages_Ckd_Warehouse_ContainerRentalWHPlan_Import, L("Pages_Ckd_Warehouse_ContainerRentalWHPlan_Import"), multiTenancySides: MultiTenancySides.Tenant);
            perInvCkdContainerRentalWHPlan.CreateChildPermission(AppPermissions.Pages_Ckd_Warehouse_ContainerRentalWHPlan_Confirm, L("Pages_Ckd_Warehouse_ContainerRentalWHPlan_Confirm"), multiTenancySides: MultiTenancySides.Tenant);

            //Ckd_Warehouse_ContainerRentalWHPlan
            var perInvCkdRequest = ckdWarehouse.CreateChildPermission(AppPermissions.Pages_Ckd_Warehouse_Request_View, L("Pages_Ckd_Warehouse_Request_View"), multiTenancySides: MultiTenancySides.Tenant);

            //Ckd_Warehouse_ContainerDeliveryGateIn
            var perInvCkdContainerDeliveryGateIn = ckdWarehouse.CreateChildPermission(AppPermissions.Pages_Ckd_Warehouse_ContainerDeliveryGateIn_View, L("Pages_Ckd_Warehouse_ContainerDeliveryGateIn_View"), multiTenancySides: MultiTenancySides.Tenant);

            //Ckd_Warehouse_ContainerList
            var perInvCkdContainerList = ckdWarehouse.CreateChildPermission(AppPermissions.Pages_Ckd_Warehouse_ContainerList_View, L("Pages_Ckd_Warehouse_ContainerList_View"), multiTenancySides: MultiTenancySides.Tenant);
            perInvCkdContainerList.CreateChildPermission(AppPermissions.Pages_Ckd_Warehouse_ContainerList_History, L("Pages_Ckd_Warehouse_ContainerList_History"), multiTenancySides: MultiTenancySides.Tenant);

            //Ckd_Warehouse_ModuleCase
            var perInvCkdModuleCase = ckdWarehouse.CreateChildPermission(AppPermissions.Pages_Ckd_Warehouse_ModuleCase_View, L("Pages_Ckd_Warehouse_ModuleCase_View"), multiTenancySides: MultiTenancySides.Tenant);

            //Ckd_Warehouse_PartManagement
            var perInvCkdPartManagement = ckdWarehouse.CreateChildPermission(AppPermissions.Pages_Ckd_Warehouse_PartManagement_View, L("Pages_Ckd_Warehouse_PartManagement_View"), multiTenancySides: MultiTenancySides.Tenant);

            //Ckd_Warehouse_StockIssuing
            var perInvCkdStockIssuing = ckdWarehouse.CreateChildPermission(AppPermissions.Pages_Ckd_Warehouse_StockIssuing_View, L("Pages_Ckd_Warehouse_StockIssuing_View"), multiTenancySides: MultiTenancySides.Tenant);
                perInvCkdStockIssuing = ckdWarehouse.CreateChildPermission(AppPermissions.Pages_Ckd_Warehouse_StockIssuing_History, L("Pages_Ckd_Warehouse_StockIssuing_History"), multiTenancySides: MultiTenancySides.Tenant);


            //Ckd_Warehouse_StockPart
            var perInvCkdStockPart = ckdWarehouse.CreateChildPermission(AppPermissions.Pages_Ckd_Warehouse_StockPart_View, L("Pages_Ckd_Warehouse_StockPart_View"), multiTenancySides: MultiTenancySides.Tenant);
            perInvCkdStockPart.CreateChildPermission(AppPermissions.Pages_Ckd_Warehouse_StockPart_Edit, L("Pages_Ckd_Warehouse_StockPart_Edit"), multiTenancySides: MultiTenancySides.Tenant);

            //Ckd_Warehouse_StockReceiving
            var perInvCkdStockReceiving = ckdWarehouse.CreateChildPermission(AppPermissions.Pages_Ckd_Warehouse_StockReceiving_View, L("Pages_Ckd_Warehouse_StockReceiving_View"), multiTenancySides: MultiTenancySides.Tenant);

            //Ckd_Warehouse_StockBalance
            var perInvCkdStockBalance = ckdWarehouse.CreateChildPermission(AppPermissions.Pages_Ckd_Warehouse_StockBalance_View, L("Pages_Ckd_Warehouse_StockBalance_View"), multiTenancySides: MultiTenancySides.Tenant);

            #endregion

            #region Ckd_Area
            //Ckd_Master
            var ckdArea = pages.CreateChildPermission(AppPermissions.Pages_Ckd_Area, L("ckdArea"), multiTenancySides: MultiTenancySides.Tenant);

            //Ckd_Area_Vehicle
            var perInvCkdVehicle = ckdArea.CreateChildPermission(AppPermissions.Pages_Ckd_Area_Vehicle_View, L("Pages_Ckd_Area_Vehicle_View"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion

            #region Ckd_Physical
            //Ckd_Master
            var ckdPhysical = pages.CreateChildPermission(AppPermissions.Pages_Ckd_Physical, L("ckdPhysical"), multiTenancySides: MultiTenancySides.Tenant);

            //Ckd_Physical_PhysicalStockPeriod
            var perInvCkdPhysicalStockPeriod = ckdPhysical.CreateChildPermission(AppPermissions.Pages_Ckd_Physical_PhysicalStockPeriod_View, L("Pages_Ckd_Physical_PhysicalStockPeriod_View"), multiTenancySides: MultiTenancySides.Tenant);
            ckdPhysical.CreateChildPermission(AppPermissions.Pages_Ckd_Physical_PhysicalStockPeriod_Edit, L("Pages_Ckd_Physical_PhysicalStockPeriod_Edit"), multiTenancySides: MultiTenancySides.Tenant);
            ckdPhysical.CreateChildPermission(AppPermissions.Pages_Ckd_Physical_PhysicalStockPeriod_ClosePeriod, L("Pages_Ckd_Physical_PhysicalStockPeriod_ClosePeriod"), multiTenancySides: MultiTenancySides.Tenant);

            //Ckd_Physical_PhysicalStockPart
            var perInvCkdPhysicalStockPart = ckdPhysical.CreateChildPermission(AppPermissions.Pages_Ckd_Physical_PhysicalStockPart_View, L("Pages_Ckd_Physical_PhysicalStockPart_View"), multiTenancySides: MultiTenancySides.Tenant);
            ckdPhysical.CreateChildPermission(AppPermissions.Pages_Ckd_Physical_PhysicalStockPart_Edit, L("Pages_Ckd_Physical_PhysicalStockPart_Edit"), multiTenancySides: MultiTenancySides.Tenant);
            ckdPhysical.CreateChildPermission(AppPermissions.Pages_Ckd_Physical_PhysicalStockPart_Import, L("Pages_Ckd_Physical_PhysicalStockPart_Import"), multiTenancySides: MultiTenancySides.Tenant);
            ckdPhysical.CreateChildPermission(AppPermissions.Pages_Ckd_Physical_PhysicalStockPart_Calculator, L("Pages_Ckd_Physical_PhysicalStockPart_Calculator"), multiTenancySides: MultiTenancySides.Tenant);

            //Ckd_Physical_PhysicalStockIssuing
            var perInvCkdPhysicalStockIssuing = ckdPhysical.CreateChildPermission(AppPermissions.Pages_Ckd_Physical_PhysicalStockIssuing_View, L("Pages_Ckd_Physical_PhysicalStockIssuing_View"), multiTenancySides: MultiTenancySides.Tenant);

            //Ckd_Physical_ReceivingPhysicalStock
            var perInvCkdReceivingPhysicalStock = ckdPhysical.CreateChildPermission(AppPermissions.Pages_Ckd_Physical_ReceivingPhysicalStock_View, L("Pages_Ckd_Physical_ReceivingPhysicalStock_View"), multiTenancySides: MultiTenancySides.Tenant);

            //Ckd_Physical_PhysicalConfirmLot
            var perInvCkdPhysicalConfirmLot = ckdPhysical.CreateChildPermission(AppPermissions.Pages_Ckd_Physical_PhysicalConfirmLot_View, L("Pages_Ckd_Physical_PhysicalConfirmLot_View"), multiTenancySides: MultiTenancySides.Tenant);
            ckdPhysical.CreateChildPermission(AppPermissions.Pages_Ckd_Physical_PhysicalConfirmLot_Import, L("Pages_Ckd_Physical_PhysicalConfirmLot_Import"), multiTenancySides: MultiTenancySides.Tenant);

            //Ckd_Physical_PhysicalStockPartS4
            var perInvCkdPhysicalStockPartS4 = ckdPhysical.CreateChildPermission(AppPermissions.Pages_Ckd_Physical_PhysicalStockPartS4_View, L("Pages_Ckd_Physical_PhysicalStockPartS4_View"), multiTenancySides: MultiTenancySides.Tenant);
            ckdPhysical.CreateChildPermission(AppPermissions.Pages_Ckd_Physical_PhysicalStockPartS4_Import, L("Pages_Ckd_Physical_PhysicalStockPartS4_Import"), multiTenancySides: MultiTenancySides.Tenant);


            #endregion

            #region Ckd_SMQD

            //Ckd_SMQD
            var ckdSMQD = pages.CreateChildPermission(AppPermissions.Pages_Ckd_SMQD, L("ckdSMQD"), multiTenancySides: MultiTenancySides.Tenant);

            // Inventory CKD SMQD 
            var perInvCkdSmqd = ckdSMQD.CreateChildPermission(AppPermissions.Pages_Ckd_SMQD_Management_View, L("Pages_Ckd_SMQD_Management_View"), multiTenancySides: MultiTenancySides.Tenant);
            ckdSMQD.CreateChildPermission(AppPermissions.Pages_Ckd_SMQD_Management_Import, L("Pages_Ckd_SMQD_Management_Import"), multiTenancySides: MultiTenancySides.Tenant);

            var perInvCkdSmqdOrder = ckdSMQD.CreateChildPermission(AppPermissions.Pages_Ckd_SMQD_SmqdOrder_View, L("Pages_Ckd_SMQD_SmqdOrder_View"), multiTenancySides: MultiTenancySides.Tenant);
            ckdSMQD.CreateChildPermission(AppPermissions.Pages_Ckd_SMQD_SmqdOrder_Import, L("Pages_Ckd_SMQD_SmqdOrder_Import"), multiTenancySides: MultiTenancySides.Tenant);
            ckdSMQD.CreateChildPermission(AppPermissions.Pages_Ckd_SMQD_SmqdOrder_CreateEdit, L("Pages_Ckd_SMQD_SmqdOrder_CreateEdit"), multiTenancySides: MultiTenancySides.Tenant);

            var perCkdSMQDPartRobbing = ckdSMQD.CreateChildPermission(AppPermissions.Pages_Ckd_SMQD_PartRobbing_View, L("Pages_Ckd_SMQD_PartRobbing_View"), multiTenancySides: MultiTenancySides.Tenant);
            ckdSMQD.CreateChildPermission(AppPermissions.Pages_Ckd_SMQD_PartRobbing_Import, L("Pages_Ckd_SMQD_PartRobbing_Import"), multiTenancySides: MultiTenancySides.Tenant);

            var perInvCkdSmqdOrderLeadTime = ckdSMQD.CreateChildPermission(AppPermissions.Pages_Ckd_SMQD_OrderLeadTime_View, L("Pages_Ckd_SMQD_OrderLeadTime_View"), multiTenancySides: MultiTenancySides.Tenant);
            ckdSMQD.CreateChildPermission(AppPermissions.Pages_Ckd_SMQD_OrderLeadTime_Import, L("Pages_Ckd_SMQD_OrderLeadTime_Import"), multiTenancySides: MultiTenancySides.Tenant);

            #endregion

            #region Ckd_Rundown
            var ckdRundown = pages.CreateChildPermission(AppPermissions.Pages_Ckd_Rundown, L("ckdRundown"), multiTenancySides: MultiTenancySides.Tenant);



            //Shiping Schedule
            ckdRundown.CreateChildPermission(AppPermissions.Pages_Ckd_Rundown_ShipingSchedule_View, L("Pages_Ckd_Rundown_ShipingSchedule_View"), multiTenancySides: MultiTenancySides.Tenant);
            ckdRundown.CreateChildPermission(AppPermissions.Pages_Ckd_Rundown_ShipingSchedule_Import, L("Pages_Ckd_Rundown_ShipingSchedule_Import"), multiTenancySides: MultiTenancySides.Tenant);
            //Shiping Schedule Firm
            ckdRundown.CreateChildPermission(AppPermissions.Pages_Ckd_Rundown_ShipingScheduleFirm_View, L("Pages_Ckd_Rundown_ShipingScheduleFirm_View"), multiTenancySides: MultiTenancySides.Tenant);
            ckdRundown.CreateChildPermission(AppPermissions.Pages_Ckd_Rundown_ShipingScheduleFirm_Edit, L("Pages_Ckd_Rundown_ShipingScheduleFirm_Edit"), multiTenancySides: MultiTenancySides.Tenant);

            //Stock Rundown Shipping Schedule 
            var shipScheduleRD = ckdRundown.CreateChildPermission(AppPermissions.Pages_Ckd_Stock_Rundown_ShipingSchedule_View, L("Pages_Ckd_Stock_Rundown_ShipingSchedule_View"), multiTenancySides: MultiTenancySides.Tenant);
            shipScheduleRD.CreateChildPermission(AppPermissions.Pages_Ckd_Stock_Rundown_ShipingSchedule_Calculator, L("Pages_Ckd_Stock_Rundown_ShipingSchedule_Calculator"), multiTenancySides: MultiTenancySides.Tenant);

            //Stock Rundown Shipping Result
            var shipResultRD = ckdRundown.CreateChildPermission(AppPermissions.Pages_Ckd_Stock_Rundown_ShipingResult_View, L("Pages_Ckd_Stock_Rundown_ShipingResult_View"), multiTenancySides: MultiTenancySides.Tenant);
            shipResultRD.CreateChildPermission(AppPermissions.Pages_Ckd_Stock_Rundown_ShipingResult_Calculator, L("Pages_Ckd_Stock_Rundown_ShipingResult_Calculator"), multiTenancySides: MultiTenancySides.Tenant);
            //Stock Rundown Warehosue
            var warehouseRD = ckdRundown.CreateChildPermission(AppPermissions.Pages_Ckd_Stock_Rundown_Warehouse_View, L("Pages_Ckd_Stock_Rundown_Warehouse_View"), multiTenancySides: MultiTenancySides.Tenant);
            warehouseRD.CreateChildPermission(AppPermissions.Pages_Ckd_Stock_Rundown_Warehouse_Calculator, L("Pages_Ckd_Stock_Rundown_Warehouse_Calculator"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion

            #endregion

            #region GPS
            // gps 
            var gps = pages.CreateChildPermission(AppPermissions.Pages_Gps, L("Gps"), multiTenancySides: MultiTenancySides.Tenant);

            var perInvGpsStock = gps.CreateChildPermission(AppPermissions.Pages_Gps_Warehouse_GpsStock_View, L("Pages_Gps_Warehouse_GpsStock_View"), multiTenancySides: MultiTenancySides.Tenant);

            #region Gps_Master
            //Gps_Master
            var gpsMaster = gps.CreateChildPermission(AppPermissions.Pages_Gps_Master, L("GpsMaster"), multiTenancySides: MultiTenancySides.Tenant);

            //Gps_Master_PartList
            var perGpsMstPartList = gpsMaster.CreateChildPermission(AppPermissions.Pages_Gps_Master_PartList_View, L("Pages_Gps_Master_PartList_View"), multiTenancySides: MultiTenancySides.Tenant);
            gpsMaster.CreateChildPermission(AppPermissions.Pages_Gps_Master_PartList_Import, L("Pages_Gps_Master_PartList_Import"), multiTenancySides: MultiTenancySides.Tenant);
            perGpsMstPartList.CreateChildPermission(AppPermissions.Pages_Gps_Master_PartList_Validate, L("Pages_Gps_Master_PartList_Validate"), multiTenancySides: MultiTenancySides.Tenant);

            //Gps_Master_StockConcept
            var perGpsMstStockConcept = gpsMaster.CreateChildPermission(AppPermissions.Pages_Gps_Master_StockConcept_View, L("Pages_Gps_Master_StockConcept_View"), multiTenancySides: MultiTenancySides.Tenant);
            gpsMaster.CreateChildPermission(AppPermissions.Pages_Gps_Master_StockConcept_Edit, L("Pages_Gps_Master_StockConcept_Edit"), multiTenancySides: MultiTenancySides.Tenant);
            gpsMaster.CreateChildPermission(AppPermissions.Pages_Gps_Master_StockConcept_Import, L("Pages_Gps_Master_StockConcept_Import"), multiTenancySides: MultiTenancySides.Tenant);
            
            //Gps_Master_Material
            var perGpsMstMasterial = gpsMaster.CreateChildPermission(AppPermissions.Pages_Gps_Master_Material_View, L("Pages_Gps_Master_Material_View"), multiTenancySides: MultiTenancySides.Tenant);
            gpsMaster.CreateChildPermission(AppPermissions.Pages_Gps_Master_Material_Edit, L("Pages_Gps_Master_Material_Edit"), multiTenancySides: MultiTenancySides.Tenant);
            gpsMaster.CreateChildPermission(AppPermissions.Pages_Gps_Master_Material_Import, L("Pages_Gps_Master_Material_Import"), multiTenancySides: MultiTenancySides.Tenant);
            perGpsMstMasterial.CreateChildPermission(AppPermissions.Pages_Gps_Master_Material_History, L("Pages_Gps_Master_Material_History"), multiTenancySides: MultiTenancySides.Tenant);

            //Gps_Master_Material
            var perGpsMstMaster = gpsMaster.CreateChildPermission(AppPermissions.Pages_Gps_Master_PartListByCategory_View, L("Pages_Gps_Master_PartListByCategory_View"), multiTenancySides: MultiTenancySides.Tenant);
            gpsMaster.CreateChildPermission(AppPermissions.Pages_Gps_Master_PartListByCategory_Edit, L("Pages_Gps_Master_PartListByCategory_Edit"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion

            #region Gps_Receive
            //Gps_Receive
            var gpsReceive = gps.CreateChildPermission(AppPermissions.Pages_Gps_Receive, L("GpsReceive"), multiTenancySides: MultiTenancySides.Tenant);

            //Gps_Receive_PartList
            var perGpsReceiveContentList = gpsReceive.CreateChildPermission(AppPermissions.Pages_Gps_Receive_ContentList_View, L("Pages_Gps_Receive_ContentList_View"), multiTenancySides: MultiTenancySides.Tenant);
            perGpsReceiveContentList.CreateChildPermission(AppPermissions.Pages_Gps_Receive_ContentList_Edit, L("GpsReceiveContentListEdit"), multiTenancySides: MultiTenancySides.Tenant);
        
            //Gps_Receive_Kanban
            var perGpsReceiveKanban = gpsReceive.CreateChildPermission(AppPermissions.Pages_Gps_Receive_Kanban_View, L("Pages_Gps_Receive_Kanban_View"), multiTenancySides: MultiTenancySides.Tenant);
            perGpsReceiveKanban.CreateChildPermission(AppPermissions.Pages_Gps_Receive_Kanban_Edit, L("GpsReceiveKanbanEdit"), multiTenancySides: MultiTenancySides.Tenant);

            //Gps_Receive
            var perGpsReceive= gpsReceive.CreateChildPermission(AppPermissions.Pages_Gps_Receive_View, L("Pages_Gps_Receive_View"), multiTenancySides: MultiTenancySides.Tenant);
            perGpsReceive.CreateChildPermission(AppPermissions.Pages_Gps_Receive_Edit, L("Pages_Gps_Receive_Edit"), multiTenancySides: MultiTenancySides.Tenant);
            perGpsReceive.CreateChildPermission(AppPermissions.Pages_Gps_Receive_Import, L("Pages_Gps_Receive_Import"), multiTenancySides: MultiTenancySides.Tenant);

            #endregion

            #region Gps_Issuing
            var gpsIssuing = gps.CreateChildPermission(AppPermissions.Pages_GPS_Issuing, L("Pages_GPS_Issuing"), multiTenancySides: MultiTenancySides.Tenant);
            
            var perGpsIssuing = gpsIssuing.CreateChildPermission(AppPermissions.Pages_GPS_Issuing_View, L("Pages_GPS_Issuing_View"), multiTenancySides: MultiTenancySides.Tenant);
            perGpsIssuing.CreateChildPermission(AppPermissions.Pages_GPS_Issuing_Edit, L("Pages_GPS_Issuing_Edit"), multiTenancySides: MultiTenancySides.Tenant);
            perGpsIssuing.CreateChildPermission(AppPermissions.Pages_GPS_Issuing_ImportGps, L("Pages_GPS_Issuing_ImportGps"), multiTenancySides: MultiTenancySides.Tenant);
            perGpsIssuing.CreateChildPermission(AppPermissions.Pages_GPS_Issuing_ImportShop, L("Pages_GPS_Issuing_ImportShop"), multiTenancySides: MultiTenancySides.Tenant);
            perGpsIssuing.CreateChildPermission(AppPermissions.Pages_GPS_Issuing_ConfirmGps, L("Pages_GPS_Issuing_ConfirmGps"), multiTenancySides: MultiTenancySides.Tenant);
            
            var pergpsmapping = gpsIssuing.CreateChildPermission(AppPermissions.Pages_GPS_Issuing_Mapping_View, L("Pages_GPS_Issuing_Mapping_View"), multiTenancySides: MultiTenancySides.Tenant);
            pergpsmapping.CreateChildPermission(AppPermissions.Pages_GPS_Issuing_Mapping_ReMap, L("Pages_GPS_Issuing_Mapping_ReMap"), multiTenancySides: MultiTenancySides.Tenant);

            var perGpsIssuings = gpsIssuing.CreateChildPermission(AppPermissions.Pages_GPS_Issuings_View, L("Pages_GPS_Issuings_View"), multiTenancySides: MultiTenancySides.Tenant);
            perGpsIssuings.CreateChildPermission(AppPermissions.Pages_GPS_Issuings_MemberShop, L("Pages_GPS_Issuings_MemberShop"), multiTenancySides: MultiTenancySides.Tenant);
            perGpsIssuings.CreateChildPermission(AppPermissions.Pages_GPS_Issuings_LeaderShop, L("Pages_GPS_Issuings_LeaderShop"), multiTenancySides: MultiTenancySides.Tenant);
            perGpsIssuings.CreateChildPermission(AppPermissions.Pages_GPS_Issuings_LeaderGps, L("Pages_GPS_Issuings_LeaderGps"), multiTenancySides: MultiTenancySides.Tenant);
            perGpsIssuings.CreateChildPermission(AppPermissions.Pages_GPS_Issuings_History, L("Pages_GPS_Issuings_History"), multiTenancySides: MultiTenancySides.Tenant);

            #endregion


            #region Gps_Order
            //Gps_Receive
            var gpsOrder = gps.CreateChildPermission(AppPermissions.Pages_Gps_Order, L("GpsOrder"), multiTenancySides: MultiTenancySides.Tenant);

            //Gps_Order_DailyOrder
            var perInvGpsDailyOrder = gps.CreateChildPermission(AppPermissions.Pages_Gps_Order_DailyOrder_View, L("Pages_Gps_Order_DailyOrder_View"), multiTenancySides: MultiTenancySides.Tenant);
            perInvGpsDailyOrder.CreateChildPermission(AppPermissions.Pages_Gps_Order_DailyOrder_Edit, L("InventoryGPSInvGpsDailyOrderEdit"), multiTenancySides: MultiTenancySides.Tenant);
          
            #endregion

            #region Gps_Rundown
            //Gps_Rundown
            var gpsRundown = gps.CreateChildPermission(AppPermissions.Pages_Gps_Rundown, L("GpsRundown"), multiTenancySides: MultiTenancySides.Tenant);

            // Gps_Rundown_StockRundown
            var perGpsStockRundown = gpsRundown.CreateChildPermission(AppPermissions.Pages_Gps_Rundown_StockRundown_View, L("Pages_Gps_Rundown_StockRundown_View"), multiTenancySides: MultiTenancySides.Tenant);
            perGpsStockRundown.CreateChildPermission(AppPermissions.Pages_Gps_Rundown_StockRundown_Calculator, L("Pages_Gps_Rundown_StockRundown_Calculator"), multiTenancySides: MultiTenancySides.Tenant);

            // Gps_Rundown_StockRundownTransaction
            var perGpsStockRundownTransaction = gpsRundown.CreateChildPermission(AppPermissions.Pages_Gps_Rundown_StockRundownTransaction_View, L("Pages_Gps_Rundown_StockRundownTransaction_View"), multiTenancySides: MultiTenancySides.Tenant);

          

            #endregion

            #region Gps_Warehouse

            var gpsWarehouse = gps.CreateChildPermission(AppPermissions.Pages_Gps_Warehouse, L("PagesGpsWarehouse"), multiTenancySides: MultiTenancySides.Tenant);

            //Gps Warehouse Stock Issuing Transaction Details
            var gpsWHStockIssuingTransDetails = gpsWarehouse.CreateChildPermission(AppPermissions.Pages_Gps_Warehouse_StockIssuingTransDetails_View, L("Pages_Gps_Warehouse_StockIssuingTransDetails_View"), multiTenancySides: MultiTenancySides.Tenant);
            gpsWarehouse.CreateChildPermission(AppPermissions.Pages_Gps_Warehouse_StockIssuingTransDetails_Import, L("Pages_Gps_Warehouse_StockIssuingTransDetails_Import"), multiTenancySides: MultiTenancySides.Tenant);

            //Gps Warehouse Stock Receiving Transaction Details
            var gpsWHStockReceivingTransDetails = gpsWarehouse.CreateChildPermission(AppPermissions.Pages_Gps_Warehouse_StockReceivingTransDetails_View, L("Pages_Gps_Warehouse_StockReceivingTransDetails_View"), multiTenancySides: MultiTenancySides.Tenant);
            gpsWarehouse.CreateChildPermission(AppPermissions.Pages_Gps_Warehouse_StockReceivingTransDetails_Import, L("Pages_Gps_Warehouse_StockReceivingTransDetails_Import"), multiTenancySides: MultiTenancySides.Tenant);

            //Gps Fin Stock
            var gpsWHGpsFinStock = gpsWarehouse.CreateChildPermission(AppPermissions.Pages_Gps_Warehouse_FinStock_View, L("Pages_Gps_Warehouse_FinStock_View"), multiTenancySides: MultiTenancySides.Tenant);

            #endregion


            #endregion

            #region DM&IHP
            var dm_ihp = pages.CreateChildPermission(AppPermissions.Pages_DMIHP, L("DMIHP"), multiTenancySides: MultiTenancySides.Tenant);

            //DM&IHP Master
            var mst = dm_ihp.CreateChildPermission(AppPermissions.Pages_DMIHP_Mst, L("DMIHPMaster"), multiTenancySides: MultiTenancySides.Tenant);

            //DM&IHP Master DRMPartList
            var mstDRMPartList = mst.CreateChildPermission(AppPermissions.Pages_DMIHP_Mst_DRMPartList_View, L("Pages_DMIHP_Mst_DRMPartList_View"), multiTenancySides: MultiTenancySides.Tenant);
            mstDRMPartList.CreateChildPermission(AppPermissions.Pages_DMIHP_Mst_DRMPartList_Import, L("Pages_DMIHP_Mst_DRMPartList_Import"), multiTenancySides: MultiTenancySides.Tenant);
            mstDRMPartList.CreateChildPermission(AppPermissions.Pages_DMIHP_Mst_DRMPartList_Edit, L("Pages_DMIHP_Mst_DRMPartList_Edit"), multiTenancySides: MultiTenancySides.Tenant);
            mstDRMPartList.CreateChildPermission(AppPermissions.Pages_DMIHP_Mst_DRMPartList_Asset, L("Pages_DMIHP_Mst_DRMPartList_Asset"), multiTenancySides: MultiTenancySides.Tenant);
            mstDRMPartList.CreateChildPermission(AppPermissions.Pages_DMIHP_Mst_DRMPartList_History, L("Pages_DMIHP_Mst_DRMPartList_History"), multiTenancySides: MultiTenancySides.Tenant);

            //DM&IHP Master IHPPartList
            var mstIHPPartList = mst.CreateChildPermission(AppPermissions.Pages_DMIHP_Mst_IHPPartList_View, L("Pages_DMIHP_Mst_IHPPartList_View"), multiTenancySides: MultiTenancySides.Tenant);
            mstIHPPartList.CreateChildPermission(AppPermissions.Pages_DMIHP_Master_PartList_Validate, L("Pages_DMIHP_Master_PartList_Validate"), multiTenancySides: MultiTenancySides.Tenant);
            mstIHPPartList.CreateChildPermission(AppPermissions.Pages_DMIHP_Master_PartList_Edit, L("Pages_DMIHP_Master_PartList_Edit"), multiTenancySides: MultiTenancySides.Tenant);
            mstIHPPartList.CreateChildPermission(AppPermissions.Pages_DMIHP_Master_PartList_History, L("Pages_DMIHP_Master_PartList_History"), multiTenancySides: MultiTenancySides.Tenant);

            //
            var mstIHPCustomDeclare = mst.CreateChildPermission(AppPermissions.Pages_DMIHP_Mst_InvIphMatCustomDeclare_View, L("Pages.DMIHP.Mst.InvIphMatCustomDeclare.View"), multiTenancySides: MultiTenancySides.Tenant);


            //DM&IHP GR
            var gr = dm_ihp.CreateChildPermission(AppPermissions.Pages_DMIHP_GR, L("DMIHPGR"), multiTenancySides: MultiTenancySides.Tenant);

            //DM&IHP GR ImportPlan
            var grImportPlan = gr.CreateChildPermission(AppPermissions.Pages_DMIHP_GR_ImportPlan_View, L("Pages_DMIHP_GR_ImportPlan_View"), multiTenancySides: MultiTenancySides.Tenant);
            grImportPlan.CreateChildPermission(AppPermissions.Pages_DMIHP_GR_ImportPlan_Import, L("Pages_DMIHP_GR_ImportPlan_Import"), multiTenancySides: MultiTenancySides.Tenant);
            grImportPlan.CreateChildPermission(AppPermissions.Pages_DMIHP_GR_ImportPlan_Confirm, L("Pages_DMIHP_GR_ImportPlan_Confirm"), multiTenancySides: MultiTenancySides.Tenant);
            grImportPlan.CreateChildPermission(AppPermissions.Pages_DMIHP_GR_ImportPlan_Edit, L("Pages_DMIHP_GR_ImportPlan_Edit"), multiTenancySides: MultiTenancySides.Tenant);

            //DM&IHP GR LocalPlan
            var grLocalPlan = gr.CreateChildPermission(AppPermissions.Pages_DMIHP_GR_LocalPlan_View, L("Pages_DMIHP_GR_LocalPlan_View"), multiTenancySides: MultiTenancySides.Tenant);
         

            //DM&IHP Stock
            var stock = dm_ihp.CreateChildPermission(AppPermissions.Pages_DMIHP_Stock, L("DMIHPStock"), multiTenancySides: MultiTenancySides.Tenant);


            //DM&IHP Stock Stock Part
            var drmstockpart = stock.CreateChildPermission(AppPermissions.Pages_DMIHP_Stock_StockPart_View, L("Pages_DMIHP_Stock_StockPart_View"), multiTenancySides: MultiTenancySides.Tenant);
            var invihpstockpart = stock.CreateChildPermission(AppPermissions.Pages_DMIHP_Stock_InvIphStockPart_View, L("Pages_DMIHP_Stock_InvIphStockPart_View"), multiTenancySides: MultiTenancySides.Tenant);

            var drmstockpartexcel = stock.CreateChildPermission(AppPermissions.Pages_DMIHP_Stock_StockPartExcel_View, L("Pages_DMIHP_Stock_StockPartExcel_View"), multiTenancySides: MultiTenancySides.Tenant);
            drmstockpartexcel.CreateChildPermission(AppPermissions.Pages_DMIHP_Stock_StockPartExcel_Import, L("Pages_DMIHP_Stock_StockPartExcel_Import"), multiTenancySides: MultiTenancySides.Tenant);


            //DM&IHP RUNDOWN
            var rundown = dm_ihp.CreateChildPermission(AppPermissions.Pages_DMIHP_Rundown, L("DMIHPRundown"), multiTenancySides: MultiTenancySides.Tenant);

            var perStockRundown = rundown.CreateChildPermission(AppPermissions.Pages_DMIHP_Rundown_StockRundown_View, L("Pages_DMIHP_Rundown_StockRundown_View"), multiTenancySides: MultiTenancySides.Tenant);
            perStockRundown.CreateChildPermission(AppPermissions.Pages_DMIHP_Rundown_StockRundown_Calculator, L("Pages_DMIHP_Rundown_StockRundown_Calculator"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion


            #region PIO
            var PIO = pages.CreateChildPermission(AppPermissions.Pages_PIO, L("PagesPIO"), multiTenancySides: MultiTenancySides.Tenant);
           
            #region PIO  Master
            var PIO_Master = PIO.CreateChildPermission(AppPermissions.Pages_PIO_Master, L("PagesPIOMaster"), multiTenancySides: MultiTenancySides.Tenant);

            //PIO Master Part List
            var perPioMasterPartList = PIO_Master.CreateChildPermission(AppPermissions.Pages_PIO_Master_PartList_View, L("Pages_PIO_Master_PartList_View"), multiTenancySides: MultiTenancySides.Tenant);
            perPioMasterPartList.CreateChildPermission(AppPermissions.Pages_PIO_Master_PartList_CreateEdit, L("Pages_PIO_Master_PartList_CreateEdit"), multiTenancySides: MultiTenancySides.Tenant);
            
            //PIO Master Part List Inl
            var perPioMasterPartListInl = PIO_Master.CreateChildPermission(AppPermissions.Pages_PIO_Master_PartListInl_View, L("Pages_PIO_Master_PartListInl_View"), multiTenancySides: MultiTenancySides.Tenant);
            perPioMasterPartListInl.CreateChildPermission(AppPermissions.Pages_PIO_Master_PartListInl_Validate, L("Pages_PIO_Master_PartListInl_Validate"), multiTenancySides: MultiTenancySides.Tenant);
            perPioMasterPartListInl.CreateChildPermission(AppPermissions.Pages_PIO_Master_PartListInl_Add, L("Pages_PIO_Master_PartListInl_Add"), multiTenancySides: MultiTenancySides.Tenant);
            perPioMasterPartListInl.CreateChildPermission(AppPermissions.Pages_PIO_Master_PartListInl_Import, L("Pages_PIO_Master_PartListInl_Import"), multiTenancySides: MultiTenancySides.Tenant);
            perPioMasterPartListInl.CreateChildPermission(AppPermissions.Pages_PIO_Master_PartListInl_History, L("Pages_PIO_Master_PartListInl_History"), multiTenancySides: MultiTenancySides.Tenant);

            //PIO Master Part List Off
            var perPioMasterPartListOff = PIO_Master.CreateChildPermission(AppPermissions.Pages_PIO_Master_PartListOff_View, L("Pages_PIO_Master_PartListOff_View"), multiTenancySides: MultiTenancySides.Tenant);
            perPioMasterPartListOff.CreateChildPermission(AppPermissions.Pages_PIO_Master_PartListOff_Validate, L("Pages_PIO_Master_PartListOff_Validate"), multiTenancySides: MultiTenancySides.Tenant);
            perPioMasterPartListOff.CreateChildPermission(AppPermissions.Pages_PIO_Master_PartListOff_Add, L("Pages_PIO_Master_PartListOff_Add"), multiTenancySides: MultiTenancySides.Tenant);
            perPioMasterPartListOff.CreateChildPermission(AppPermissions.Pages_PIO_Master_PartListOff_Import, L("Pages_PIO_Master_PartListOff_Import"), multiTenancySides: MultiTenancySides.Tenant);

            //PIO Master Tmss TmssDispatchPlan
            var perPioMasterTmssDispatchPlan = PIO_Master.CreateChildPermission(AppPermissions.Pages_PIO_Master_Tmss_TmssDispatchPlan_View, L("Pages_PIO_Master_Tmss_TmssDispatchPlan_View"), multiTenancySides: MultiTenancySides.Tenant);
            perPioMasterTmssDispatchPlan = PIO_Master.CreateChildPermission(AppPermissions.Pages_PIO_Master_Tmss_TmssDispatchPlan_History, L("Pages_PIO_Master_Tmss_TmssDispatchPlan_History"), multiTenancySides: MultiTenancySides.Tenant);

            //PIO Topsse Invoice
            var perPioTopsseInvoice = PIO_Master.CreateChildPermission(AppPermissions.Pages_PIO_Master_TopsseInvoice_View, L("Pages_PIO_Master_TopsseInvoice_View"), multiTenancySides: MultiTenancySides.Tenant);
            perPioTopsseInvoice.CreateChildPermission(AppPermissions.Pages_PIO_Master_TopsseInvoice_Receive, L("Pages_PIO_Master_TopsseInvoice_Receive"), multiTenancySides: MultiTenancySides.Tenant);

			// PIO MASTER SUPPLIER INFO
			var perPioMstLspSupplierInfor = PIO_Master.CreateChildPermission(AppPermissions.Pages_PIO_Master_Supplier_Info_View, L("Pages_PIO_Master_Supplier_Info_View"), multiTenancySides: MultiTenancySides.Tenant);
			perPioMstLspSupplierInfor.CreateChildPermission(AppPermissions.Pages_PIO_Master_Supplier_Info_CreateEdit, L("Pages_PIO_Master_Supplier_Info_CreateEdit"), multiTenancySides: MultiTenancySides.Tenant);
			perMstInvCpsSuppliers.CreateChildPermission(AppPermissions.Pages_PIO_Master_Supplier_Info_Delete, L("Pages_PIO_Master_Supplier_Info_Delete"), multiTenancySides: MultiTenancySides.Tenant);
            //Inventory PIO ProductionPlanMonthly
            var perPioMasterProductionPlanMonthly = PIO_Master.CreateChildPermission(AppPermissions.Pages_PIO_Master_ProductionPlanMonthly_View, L("Pages_PIO_Master_ProductionPlanMonthly_View"), multiTenancySides: MultiTenancySides.Tenant);
            perPioMasterProductionPlanMonthly.CreateChildPermission(AppPermissions.Pages_PIO_Master_ProductionPlanMonthly_Import, L("Pages_PIO_Master_ProductionPlanMonthly_Import"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion



            #region PIO  Warehouse
            var PIO_Warehouse = PIO.CreateChildPermission(AppPermissions.Pages_PIO_Warehouse, L("PagesPIOWarehouse"), multiTenancySides: MultiTenancySides.Tenant);

            //PIO Warehouse Stock Rundown
            var PIOStockRundown = PIO_Warehouse.CreateChildPermission(AppPermissions.Pages_PIO_Warehouse_StockRundown_View, L("Pages_PIO_Warehouse_StockRundown_View"), multiTenancySides: MultiTenancySides.Tenant);
            PIOStockRundown.CreateChildPermission(AppPermissions.Pages_PIO_Warehouse_StockRundown_Calculator, L("Pages_PIO_Warehouse_StockRundown_Calculator"), multiTenancySides: MultiTenancySides.Tenant);

            //PIO Warehouse Stock Rundown Transaction
            var PIOStockRundownTransaction = PIO_Warehouse.CreateChildPermission(AppPermissions.Pages_PIO_Warehouse_StockRundownTransaction_View, L("Pages_PIO_Warehouse_StockRundownTransaction_View"), multiTenancySides: MultiTenancySides.Tenant);

            //PIO Warehouse Stock Transaction
            var PIOStockTransaction = PIO_Warehouse.CreateChildPermission(AppPermissions.Pages_PIO_Warehouse_StockTransaction_View, L("Pages_PIO_Warehouse_StockTransaction_View"), multiTenancySides: MultiTenancySides.Tenant);

            //PIO Warehouse Stock 
            var PIOStockn = PIO_Warehouse.CreateChildPermission(AppPermissions.Pages_PIO_Warehouse_Stock_View, L("Pages_PIO_Warehouse_Stock_View"), multiTenancySides: MultiTenancySides.Tenant);

            //PIO Warehouse Stock_Issuing
            var PIOStockIssuing = PIO_Warehouse.CreateChildPermission(AppPermissions.Pages_PIO_Warehouse_StockIssuing_View, L("Pages_PIO_Warehouse_StockIssuing_View"), multiTenancySides: MultiTenancySides.Tenant);
            //PIO Warehouse Stock_receiving
            var PIOStockReceiving = PIO_Warehouse.CreateChildPermission(AppPermissions.Pages_PIO_Warehouse_StockReceiving_View, L("Pages_PIO_Warehouse_StockReceiving_View"), multiTenancySides: MultiTenancySides.Tenant);

            #endregion

            #endregion

            #region INTERFACE
            // INTERFACE 
            var interface1 =  pages.CreateChildPermission(AppPermissions.Pages_Interface, L("Interface"), multiTenancySides: MultiTenancySides.Tenant);

            var perBom = interface1.CreateChildPermission(AppPermissions.Pages_Interface_Bom, L("InterfaceBom"), multiTenancySides: MultiTenancySides.Tenant);

            var perGenBOMData = perBom.CreateChildPermission(AppPermissions.Pages_Interface_Bom_InvGenBOMData_View, L("Pages_Interface_Bom_InvGenBOMData_View"), multiTenancySides: MultiTenancySides.Tenant);

          

            var perIF = interface1.CreateChildPermission(AppPermissions.Pages_Interface_IF, L("InterfaceIF"), multiTenancySides: MultiTenancySides.Tenant);
          //  interface1.CreateChildPermission(AppPermissions.Pages_Interface_IF, L("InterfaceIF"), multiTenancySides: MultiTenancySides.Tenant);


            var perIF_FQF3MM01 = perIF.CreateChildPermission(AppPermissions.Pages_Interface_IF_FQF3MM01_View, L("Pages_Interface_IF_FQF3MM01_View"), multiTenancySides: MultiTenancySides.Tenant);
            perIF_FQF3MM01.CreateChildPermission(AppPermissions.Pages_Interface_IF_FQF3MM01_ReCreate, L("Pages_Interface_IF_FQF3MM01_ReCreate"), multiTenancySides: MultiTenancySides.Tenant);

            var perIF_FQF3MM02 = perIF.CreateChildPermission(AppPermissions.Pages_Interface_IF_FQF3MM02_View, L("Pages_Interface_IF_FQF3MM02_View"), multiTenancySides: MultiTenancySides.Tenant);
            perIF_FQF3MM02.CreateChildPermission(AppPermissions.Pages_Interface_IF_FQF3MM02_ReCreate, L("Pages_Interface_IF_FQF3MM02_ReCreate"), multiTenancySides: MultiTenancySides.Tenant);

            var perIF_FQF3MM_LV2 = perIF.CreateChildPermission(AppPermissions.Pages_Interface_IF_FQF3MM_LV2_View, L("Pages_Interface_IF_FQF3MM_LV2_View"), multiTenancySides: MultiTenancySides.Tenant);
            //perIF_FQF3MM_LV2.CreateChildPermission(AppPermissions.Pages_Interface_IF_FQF3MM_LV2_ReCreate, L("Pages_Interface_IF_FQF3MM_LV2_ReCreate"), multiTenancySides: MultiTenancySides.Tenant);

            var perIF_FQF3MM03 = perIF.CreateChildPermission(AppPermissions.Pages_Interface_IF_FQF3MM03_View, L("Pages_Interface_IF_FQF3MM03_View"), multiTenancySides: MultiTenancySides.Tenant);
            perIF_FQF3MM03.CreateChildPermission(AppPermissions.Pages_Interface_IF_FQF3MM03_ReCreate, L("Pages_Interface_IF_FQF3MM03_ReCreate"), multiTenancySides: MultiTenancySides.Tenant);


            var perIF_FQF3MM04 = perIF.CreateChildPermission(AppPermissions.Pages_Interface_IF_FQF3MM04_View, L("Pages_Interface_IF_FQF3MM04_View"), multiTenancySides: MultiTenancySides.Tenant);
            perIF_FQF3MM04.CreateChildPermission(AppPermissions.Pages_Interface_IF_FQF3MM04_ReCreate, L("Pages_Interface_IF_FQF3MM04_ReCreate"), multiTenancySides: MultiTenancySides.Tenant);

           
            var perIF_FQF3MM05 = perIF.CreateChildPermission(AppPermissions.Pages_Interface_IF_FQF3MM05_View, L("Pages_Interface_IF_FQF3MM05_View"), multiTenancySides: MultiTenancySides.Tenant);
            perIF_FQF3MM05.CreateChildPermission(AppPermissions.Pages_Interface_IF_FQF3MM05_ReCreate, L("Pages_Interface_IF_FQF3MM05_ReCreate"), multiTenancySides: MultiTenancySides.Tenant);

            var perIF_FQF3MM06 = perIF.CreateChildPermission(AppPermissions.Pages_Interface_IF_FQF3MM06_View, L("Pages_Interface_IF_FQF3MM06_View"), multiTenancySides: MultiTenancySides.Tenant);
            perIF_FQF3MM06.CreateChildPermission(AppPermissions.Pages_Interface_IF_FQF3MM06_ReCreate, L("Pages_Interface_IF_FQF3MM06_ReCreate"), multiTenancySides: MultiTenancySides.Tenant);

        
            var perIF_FQF3MM07 = perIF.CreateChildPermission(AppPermissions.Pages_Interface_IF_FQF3MM07_View, L("Pages_Interface_IF_FQF3MM07_View"), multiTenancySides: MultiTenancySides.Tenant);
            perIF_FQF3MM07.CreateChildPermission(AppPermissions.Pages_Interface_IF_FQF3MM07_ReCreate, L("Pages_Interface_IF_FQF3MM07_ReCreate"), multiTenancySides: MultiTenancySides.Tenant);



            #endregion

            #region SPP
            var spp = pages.CreateChildPermission(AppPermissions.Pages_SPP, L("SPP"), multiTenancySides: MultiTenancySides.Tenant);
            var sppInv = spp.CreateChildPermission(AppPermissions.Pages_SPP_Inventory, L("SPP_Inventory"), multiTenancySides: MultiTenancySides.Tenant);

            var SPP_Cost = sppInv.CreateChildPermission(AppPermissions.Pages_SPP_Cost_View, L("Pages_SPP_Inventory_Cost_View"), multiTenancySides: MultiTenancySides.Tenant);
            var SPP_InvoiceDetails = sppInv.CreateChildPermission(AppPermissions.Pages_SPP_InvoiceDetails_View, L("Pages_SPP_InvoiceDetails_View"), multiTenancySides: MultiTenancySides.Tenant);
            var SPP_Shipping = sppInv.CreateChildPermission(AppPermissions.Pages_SPP_Shipping_View, L("Pages_SPP_Shipping_View"), multiTenancySides: MultiTenancySides.Tenant);
            var SPP_Stock = sppInv.CreateChildPermission(AppPermissions.Pages_SPP_Stock_View, L("Pages_SPP_Stock_View"), multiTenancySides: MultiTenancySides.Tenant);

            var SPP_CostOfSaleSummary = sppInv.CreateChildPermission(AppPermissions.Pages_SPP_CostOfSaleSummary_View, L("Pages_SPP_Inventory_CostOfSaleSummary_View"), multiTenancySides: MultiTenancySides.Tenant);

            var sppMaster = spp.CreateChildPermission(AppPermissions.Pages_SPP_Master, L("Pages_SPP_Master"), multiTenancySides: MultiTenancySides.Tenant);
            var perMstSppCustomer = sppMaster.CreateChildPermission(AppPermissions.Pages_SPP_Master_Customer, L("Pages_SPP_Master_Customer"), multiTenancySides: MultiTenancySides.Tenant);
            var perMstSppGLAccount = sppMaster.CreateChildPermission(AppPermissions.Pages_SPP_Master_GLAccount, L("Pages_SPP_Master_GLAccount"), multiTenancySides: MultiTenancySides.Tenant);



            #endregion
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, prodConsts.LocalizationSourceName);
        }
    }
}
