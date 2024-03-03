using Abp.MultiTenancy;

namespace prod.Authorization
{
    /// <summary>
    /// Defines string constants for application's permission names.
    /// <see cref="AppAuthorizationProvider"/> for permission definitions.
    /// </summary>
    public static class AppPermissions
    {
        //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)

        public const string Pages = "Pages";

        public const string Pages_DemoUiComponents = "Pages.DemoUiComponents";
        public const string Pages_Administration = "Pages.Administration";

        public const string Pages_Administration_Roles = "Pages.Administration.Roles";
        public const string Pages_Administration_Roles_Create = "Pages.Administration.Roles.Create";
        public const string Pages_Administration_Roles_Edit = "Pages.Administration.Roles.Edit";
        public const string Pages_Administration_Roles_Delete = "Pages.Administration.Roles.Delete";

        public const string Pages_Administration_Users = "Pages.Administration.Users";
        public const string Pages_Administration_Users_Create = "Pages.Administration.Users.Create";
        public const string Pages_Administration_Users_Edit = "Pages.Administration.Users.Edit";
        public const string Pages_Administration_Users_Delete = "Pages.Administration.Users.Delete";
        public const string Pages_Administration_Users_ChangePermissions = "Pages.Administration.Users.ChangePermissions";
        public const string Pages_Administration_Users_Impersonation = "Pages.Administration.Users.Impersonation";
        public const string Pages_Administration_Users_Unlock = "Pages.Administration.Users.Unlock";
        public const string Pages_Administration_Users_ChangeProfilePicture = "Pages.Administration.Users.ChangeProfilePicture";

        public const string Pages_Administration_Languages = "Pages.Administration.Languages";
        public const string Pages_Administration_Languages_Create = "Pages.Administration.Languages.Create";
        public const string Pages_Administration_Languages_Edit = "Pages.Administration.Languages.Edit";
        public const string Pages_Administration_Languages_Delete = "Pages.Administration.Languages.Delete";
        public const string Pages_Administration_Languages_ChangeTexts = "Pages.Administration.Languages.ChangeTexts";
        public const string Pages_Administration_Languages_ChangeDefaultLanguage = "Pages.Administration.Languages.ChangeDefaultLanguage";

        public const string Pages_Administration_AuditLogs = "Pages.Administration.AuditLogs";

        public const string Pages_Administration_OrganizationUnits = "Pages.Administration.OrganizationUnits";
        public const string Pages_Administration_OrganizationUnits_ManageOrganizationTree = "Pages.Administration.OrganizationUnits.ManageOrganizationTree";
        public const string Pages_Administration_OrganizationUnits_ManageMembers = "Pages.Administration.OrganizationUnits.ManageMembers";
        public const string Pages_Administration_OrganizationUnits_ManageRoles = "Pages.Administration.OrganizationUnits.ManageRoles";

        public const string Pages_Administration_HangfireDashboard = "Pages.Administration.HangfireDashboard";

        public const string Pages_Administration_UiCustomization = "Pages.Administration.UiCustomization";

        public const string Pages_Administration_WebhookSubscription = "Pages.Administration.WebhookSubscription";
        public const string Pages_Administration_WebhookSubscription_Create = "Pages.Administration.WebhookSubscription.Create";
        public const string Pages_Administration_WebhookSubscription_Edit = "Pages.Administration.WebhookSubscription.Edit";
        public const string Pages_Administration_WebhookSubscription_ChangeActivity = "Pages.Administration.WebhookSubscription.ChangeActivity";
        public const string Pages_Administration_WebhookSubscription_Detail = "Pages.Administration.WebhookSubscription.Detail";
        public const string Pages_Administration_Webhook_ListSendAttempts = "Pages.Administration.Webhook.ListSendAttempts";
        public const string Pages_Administration_Webhook_ResendWebhook = "Pages.Administration.Webhook.ResendWebhook";

        public const string Pages_Administration_DynamicProperties = "Pages.Administration.DynamicProperties";
        public const string Pages_Administration_DynamicProperties_Create = "Pages.Administration.DynamicProperties.Create";
        public const string Pages_Administration_DynamicProperties_Edit = "Pages.Administration.DynamicProperties.Edit";
        public const string Pages_Administration_DynamicProperties_Delete = "Pages.Administration.DynamicProperties.Delete";

        public const string Pages_Administration_DynamicPropertyValue = "Pages.Administration.DynamicPropertyValue";
        public const string Pages_Administration_DynamicPropertyValue_Create = "Pages.Administration.DynamicPropertyValue.Create";
        public const string Pages_Administration_DynamicPropertyValue_Edit = "Pages.Administration.DynamicPropertyValue.Edit";
        public const string Pages_Administration_DynamicPropertyValue_Delete = "Pages.Administration.DynamicPropertyValue.Delete";

        public const string Pages_Administration_DynamicEntityProperties = "Pages.Administration.DynamicEntityProperties";
        public const string Pages_Administration_DynamicEntityProperties_Create = "Pages.Administration.DynamicEntityProperties.Create";
        public const string Pages_Administration_DynamicEntityProperties_Edit = "Pages.Administration.DynamicEntityProperties.Edit";
        public const string Pages_Administration_DynamicEntityProperties_Delete = "Pages.Administration.DynamicEntityProperties.Delete";

        public const string Pages_Administration_DynamicEntityPropertyValue = "Pages.Administration.DynamicEntityPropertyValue";
        public const string Pages_Administration_DynamicEntityPropertyValue_Create = "Pages.Administration.DynamicEntityPropertyValue.Create";
        public const string Pages_Administration_DynamicEntityPropertyValue_Edit = "Pages.Administration.DynamicEntityPropertyValue.Edit";
        public const string Pages_Administration_DynamicEntityPropertyValue_Delete = "Pages.Administration.DynamicEntityPropertyValue.Delete";
        
        public const string Pages_Administration_MassNotification = "Pages.Administration.MassNotification";
        public const string Pages_Administration_MassNotification_Create = "Pages.Administration.MassNotification.Create";
        
        public const string Pages_Administration_NewVersion_Create = "Pages_Administration_NewVersion_Create";
        
        //TENANT-SPECIFIC PERMISSIONS

        public const string Pages_Tenant_Dashboard = "Pages.Tenant.Dashboard";

        public const string Pages_Administration_Tenant_Settings = "Pages.Administration.Tenant.Settings";

        public const string Pages_Administration_Tenant_SubscriptionManagement = "Pages.Administration.Tenant.SubscriptionManagement";

        //HOST-SPECIFIC PERMISSIONS

        public const string Pages_Editions = "Pages.Editions";
        public const string Pages_Editions_Create = "Pages.Editions.Create";
        public const string Pages_Editions_Edit = "Pages.Editions.Edit";
        public const string Pages_Editions_Delete = "Pages.Editions.Delete";
        public const string Pages_Editions_MoveTenantsToAnotherEdition = "Pages.Editions.MoveTenantsToAnotherEdition";

        public const string Pages_Tenants = "Pages.Tenants";
        public const string Pages_Tenants_Create = "Pages.Tenants.Create";
        public const string Pages_Tenants_Edit = "Pages.Tenants.Edit";
        public const string Pages_Tenants_ChangeFeatures = "Pages.Tenants.ChangeFeatures";
        public const string Pages_Tenants_Delete = "Pages.Tenants.Delete";
        public const string Pages_Tenants_Impersonation = "Pages.Tenants.Impersonation";

        public const string Pages_Administration_Host_Maintenance = "Pages.Administration.Host.Maintenance";
        public const string Pages_Administration_Host_Settings = "Pages.Administration.Host.Settings";
        public const string Pages_Administration_Host_Dashboard = "Pages.Administration.Host.Dashboard";


        //NEW

        #region WORKING PATTERN
        //WORKING PATTERN      
        public const string Pages_WorkingPattern = "Pages.WorkingPattern";

        //Working Pattern - Calendar
        public const string Pages_WorkingPattern_Calendar_View = "Pages.WorkingPattern.Calendar.View";
        public const string Pages_WorkingPattern_Calendar_Edit = "Pages.WorkingPattern.Calendar.Edit";

        //Working Pattern - SeasonMonth
        public const string Pages_WorkingPattern_SeasonMonth_View = "Pages.WorkingPattern.SeasonMonth.View";
        public const string Pages_WorkingPattern_SeasonMonth_Edit = "Pages.WorkingPattern.SeasonMonth.Edit";

        //Working Pattern - PatternH
        public const string Pages_WorkingPattern_PatternH_View = "Pages.WorkingPattern.PatternH.View";
        public const string Pages_WorkingPattern_PatternH_Edit = "Pages.WorkingPattern.PatternH.Edit";

        //Working Pattern -Patternd
        public const string Pages_WorkingPattern_PatternD_View = "Pages.WorkingPattern.PatternD.View";
        public const string Pages_WorkingPattern_PatternD_Edit = "Pages.WorkingPattern.PatternD.Edit";

        //Working Pattern - WorkingTime
        public const string Pages_WorkingPattern_WorkingTime_View = "Pages.WorkingPattern.WorkingTime.View";
        public const string Pages_WorkingPattern_WorkingTime_Edit = "Pages.WorkingPattern.WorkingTime.Edit";

        //Working Pattern - WorkingType
        public const string Pages_WorkingPattern_WorkingType_View = "Pages.WorkingPattern.WorkingType.View";
        public const string Pages_WorkingPattern_WorkingType_Edit = "Pages.WorkingPattern.WorkingType.Edit";

        //Working Pattern - DailyWorkingTime
        public const string Pages_WorkingPattern_DailyWorkingTime_View = "Pages.WorkingPattern.DailyWorkingTime.View";
        public const string Pages_WorkingPattern_DailyWorkingTime_Edit = "Pages.WorkingPattern.DailyWorkingTime.Edit";

        //Working Pattern - Week
        public const string Pages_WorkingPattern_Week_View = "Pages.WorkingPattern.Week.View";
        public const string Pages_WorkingPattern_Week_Edit = "Pages.WorkingPattern.Week.Edit";

        #endregion

        #region MASTER
        //MASTER         
        public const string Pages_Master = "Pages.Master";

        #region Common
        // Master - Common
        public const string Pages_Master_Common = "Pages.Master.Common";

        //Master_Common_ExchangeRate
        public const string Pages_Master_Common_ExchangeRate_View = "Pages.Master.Common.ExchangeRate.View";
     //   public const string Pages_Master_Common_ExchangeRate_Check = "Pages.Master.Common.ExchangeRate.Check";
        public const string Pages_Master_Common_ExchangeRate_Approve = "Pages.Master.Common.ExchangeRate.Approve";
        public const string Pages_Master_Common_ExchangeRate_History = "Pages.Master.Common.ExchangeRate.History";

        //Master - Common - Model
        public const string Pages_Master_Common_Model_View = "Pages.Master.Common.Model.View";
        public const string Pages_Master_Common_Model_Edit = "Pages.Master.Common.Model.Edit";

        // Master - Working Pattern - Shop
        public const string Pages_Master_Common_Shop_View = "Pages.Master.Common.Shop.View";
        public const string Pages_Master_Common_Shop_Edit = "Pages.Master.Common.Shop.Edit";

        //Master_Common_DevanningCaseType
        public const string Pages_Master_Common_DevanningCaseType_View = "Pages.Master.Common.DevanningCaseType.View";
        public const string Pages_Master_Common_DevanningCaseType_Edit = "Pages.Master.Common.DevanningCaseType.Edit";
        public const string Pages_Master_Common_DevanningCaseType_History = "Pages.Master.Common.DevanningCaseType.History";

        //Master_Common_Shoptype
        public const string Pages_Master_Common_ShopType_View = "Pages.Master.Common.ShopType.View";
        public const string Pages_Master_Common_ShopType_Edit = "Pages.Master.Common.ShopType.Edit";

        //Master_Common_Color
        public const string Pages_Master_Common_Color_View = "Pages.Master.Common.Color.View";
        public const string Pages_Master_Common_Color_Edit = "Pages.Master.Common.Color.Edit";
        public const string Pages_Master_Common_Color_History = "Pages.Master.Common.Color.History";

        //Master_Common_GradeColor
        public const string Pages_Master_Common_MstCmmGradeColor_History = "Pages.Master.Common.GradeColor.History";

        //Master_Common_VehicleColorType
        public const string Pages_Master_Common_VehicleColorType_View = "Pages.Master.Common.VehicleColorType.View";
         
        //Master_Common_VehicleColorType
        public const string Pages_Master_Common_VehicleName_View = "Pages.Master.Common.VehicleName.View";

        //Master - Common - TaktTime
        public const string Pages_Master_Common_TaktTime_View = "Pages.Master.Common.TaktTime.View";
        public const string Pages_Master_Common_TaktTime_Edit = "Pages.Master.Common.TaktTime.Edit";

        // Master - Common - Lookup
        public const string Pages_Master_Common_Lookup_View = "Pages.Master.Common.Lookup.View";
        public const string Pages_Master_Common_Lookup_Edit = "Pages.Master.Common.Lookup.Edit";
        public const string Pages_Master_Common_Lookup_History = "Pages.Master.Common.Lookup.History";

        // Master - Common - MMCheckingRule
        public const string Pages_Master_Cmm_MMCheckingRule_View = "Pages.Master.Cmm.MMCheckingRule.View";
        public const string Pages_Master_Cmm_MMCheckingRule_Import = "Pages.Master.Cmm.MMCheckingRule.Import";
        public const string Pages_Master_Cmm_MMCheckingRule_History = "Pages.Master.Cmm.MMCheckingRule.History";

        // Master - Common - LotCodeGrad
        public const string Pages_Master_Cmm_LotCodeGrade_History = "Pages.Master.Cmm.LotCodeGrade.History";

        // Master - Common - MMValidationResult
        public const string Pages_Master_Cmm_MMValidationResult_View = "Pages.Master.Cmm.MMValidationResult.View";
        public const string Pages_Master_Cmm_MMValidationResult_History = "Pages.Master.Cmm.MMValidationResult.History";


        //Master - Common - Vehicle CBU
        public const string Pages_Master_Cmm_VehicleCBU_View = "Pages.Master.Cmm.VehicleCBU.View";
        public const string Pages_Master_Cmm_VehicleCBU_Validate = "Pages.Master.Cmm.VehicleCBU.Validate";
        public const string Pages_Master_Cmm_VehicleCBU_History = "Pages.Master.Cmm.VehicleCBU.History";
        public const string Pages_Master_Cmm_VehicleCBU_CreateMat = "Pages.Master.Cmm.VehicleCBU.CreateMat";

        //Master - Common - EngineMaster
        public const string Pages_Master_Cmm_EngineMaster_View = "Pages.Master.Cmm.EngineMaster.View";
        public const string Pages_Master_Cmm_EngineMaster_CreateEdit = "Pages.Master.Cmm.EngineMaster.CreateEdit";
        public const string Pages_Master_Cmm_EngineMaster_Delete = "Pages.Master.Cmm.EngineMaster.Delete";
        public const string Pages_Master_Cmm_EngineMaster_History = "Pages.Master.Cmm.EngineMaster.History";
  

        //Master - Common - BusinessParter
        public const string Pages_Master_Cmm_BusinessParter_View = "Pages.Master.Cmm.BusinessParter.View";
        public const string Pages_Master_Cmm_BusinessParter_CreateEdit = "Pages.Master.Cmm.BusinessParter.CreateEdit";
        public const string Pages_Master_Cmm_BusinessParter_Delete = "Pages.Master.Cmm.BusinessParter.Delete";

        //Master - Common - EngineModel
        public const string Pages_Master_Cmm_EngineModel_View = "Pages.Master.Cmm.EngineModel.View";
        public const string Pages_Master_Cmm_EngineModel_CreateEdit = "Pages.Master.Cmm.EngineModel.CreateEdit";
        public const string Pages_Master_Cmm_EngineModel_Delete = "Pages.Master.Cmm.EngineModel.Delete";

        //Master - Common - EngineType
        public const string Pages_Master_Cmm_EngineType_View = "Pages.Master.Cmm.EngineType.View";
        public const string Pages_Master_Cmm_EngineType_CreateEdit = "Pages.Master.Cmm.EngineType.CreateEdit";
        public const string Pages_Master_Cmm_EngineType_Delete = "Pages.Master.Cmm.EngineType.Delete";

        //Master - Common - ProductGroup
        public const string Pages_Master_Cmm_ProductGroup_View = "Pages.Master.Cmm.ProductGroup.View";
        public const string Pages_Master_Cmm_ProductGroup_CreateEdit = "Pages.Master.Cmm.ProductGroup.CreateEdit";
        public const string Pages_Master_Cmm_ProductGroup_Delete = "Pages.Master.Cmm.ProductGroup.Delete";

        //Master - Common - Tax
        public const string Pages_Master_Cmm_Tax_View = "Pages.Master.Cmm.Tax.View";
        public const string Pages_Master_Cmm_Tax_CreateEdit = "Pages.Master.Cmm.Tax.CreateEdit";
        public const string Pages_Master_Cmm_Tax_Delete = "Pages.Master.Cmm.Tax.Delete";
        #endregion

        #region CKD
        // Master - CKD
        public const string Pages_Master_Ckd = "Pages.Master.Ckd";

        //Master_CKD_CustomsPort
        public const string Pages_Master_Ckd_CustomsPort_View = "Pages.Master.Ckd.CustomsPort.View";

        //Master_CKD_ShippingCompany
        public const string Pages_Master_Ckd_ShippingCompany_View = "Pages.Master.Ckd.ShippingCompany.View";

        // Master_CKD_ContainerStatus
        public const string Pages_Master_Ckd_ContainerStatus_View = "Pages.Master.Ckd.ContainerStatus.View";

        //Master_CKD_ContainerDeliveryType
        public const string Pages_Master_Ckd_ContainerDeliveryType_View = "Pages.Master.Ckd.ContainerDeliveryType.View";

        //Master_CKD_Forwarder
        public const string Pages_Master_Ckd_Forwarder_View = "Pages.Master.Ckd.Forwarder.View";

        //Master_CKD_ShippingCompany
        public const string Pages_Master_Ckd_InvoiceStatus_View = "Pages.Master.Ckd.InvoiceStatus.View";

        //Master_CKD_CustomsStatus
        public const string Pages_Master_Ckd_CustomsStatus_View = "Pages.Master.Ckd.CustomsStatus.View";

        // Master_CKD_SupplierList
        public const string Pages_Master_Inventory_SupplierList_View = "Pages.Master.Inventory.SupplierList.View";


        // PIO PartType 
        public const string Pages_Master_Inventory_PIOPartType_View = "Pages.Master.Inventory.PIOPartType.View";
        public const string Pages_Master_Inventory_PIOPartType_Edit = "Pages.Master.Inventory.PIOPartType.Edit";
          // PIO ImpSupplier 
        public const string Pages_Master_MstPioImpSupplier_View = "Pages.Master.MstPioImpSupplier.View";
        public const string Pages_Master_MstPioImpSupplier_Edit = "Pages.Master.MstPioImpSupplier.Edit";


        //Master_CKD_CustomsLeadtime
        public const string Pages_Master_CKD_CustomsLeadtime_View = "Pages.Master.CKD.CustomsLeadtime.View";
        public const string Pages_Master_CKD_CustomsLeadtime_Edit = "Pages.Master.CKD.CustomsLeadtime.Edit";
        //ckd_mst_CustomsLeadTimeMaster
        public const string Pages_Master_CKD_CustomsLeadTimeMaster_View = "Pages.Master.CKD.CustomsLeadTimeMaster.View";
        public const string Pages_Master_CKD_CustomsLeadTimeMaster_Edit = "Pages.Master.CKD.CustomsLeadTimeMaster.Edit";
        public const string Pages_Master_CKD_CustomsLeadTimeMaster_Import = "Pages.Master.CKD.CustomsLeadTimeMaster.Import";


        //Master_CKD_PIOEmail
        public const string Pages_Master_Inventory_PIOEmail_View = "Pages.Master.Inventory.PIOEmail.View";
        public const string Pages_Master_Inventory_PIOEmail_Edit = "Pages.Master.Inventory.PIOEmail.Edit";


        //DEMDETfees
        public const string Pages_Master_CKD_DemDetFees_View = "Pages.Master.CKD.DemDetFees.View";
        public const string Pages_Master_CKD_DemDetFees_Edit = "Pages.Master.CKD.DemDetFees.Edit";
        public const string Pages_Master_CKD_DemDetFees_Import = "Pages.Master.CKD.DemDetFees.Import";
		//Master
		// MstInvCkdRentalWarehouse
		public const string Pages_Master_Inventory_CkdRentalWarehouse = "Pages.Master.Inventory.CkdRentalWarehouse";
		public const string Pages_Master_Inventory_CkdRentalWarehouse_CreateEdit = "Pages.Master.Inventory.CkdRentalWarehouse.CreateEdit";
		public const string Pages_Master_Inventory_CkdRentalWarehouse_Delete = "Pages.Master.Inventory.CkdRentalWarehouse.Delete";

		//MASTER DEM DET DAY
		public const string Pages_Master_CKD_DemDetDays_View = "Pages.Master.CKD.DemDetDays.View";
		public const string Pages_Master_CKD_DemDetDays_Edit = "Pages.Master.CKD.DemDetDays.Edit";
		public const string Pages_Master_CKD_DemDetDays_Import = "Pages.Master.CKD.DemDetDays.Import";


        #endregion
        #region GPS
        // Master - GPS
        public const string Pages_Master_Gps = "Pages.Master.Gps";

        //Master_Gps_GpsSupplierInfo
        public const string Pages_Master_Gps_GpsSupplierInfo_View = "Pages.Master.Gps.GpsSupplierInfo.View";
        public const string Pages_Master_Gps_GpsSupplierInfo_Edit = "Pages.Master.Gps.GpsSupplierInfo.Edit";

        //Master_Gps_GpsCalendar
        public const string Pages_Master_Gps_GpsCalendar_View = "Pages.Master.Gps.GpsCalendar.View";
        public const string Pages_Master_Gps_GpsCalendar_Edit = "Pages.Master.Gps.GpsCalendar.Edit";


        //Master_Gps_GpsTmvPic
        public const string Pages_Master_Gps_GpsTmvPic_View = "Pages.Master.Gps.GpsTmvPic.View";
        public const string Pages_Master_Gps_GpsTmvPic_Edit = "Pages.Master.Gps.GpsTmvPic.Edit";


        //Master_Gps_GpsTruckSupplier
        public const string Pages_Master_Gps_GpsTruckSupplier_View = "Pages.Master.Gps.GpsTruckSupplier.View";
        public const string Pages_Master_Gps_GpsTruckSupplier_Edit = "Pages.Master.Gps.GpsTruckSupplier.Edit";


        //Master_Gps_GpsSupplierPic 
        public const string Pages_Master_Gps_GpsSupplierPic_View = "Pages.Master.Gps.GpsSupplierPic.View";
        public const string Pages_Master_Gps_GpsSupplierPic_Edit = "Pages.Master.Gps.GpsSupplierPic.Edit";


        // Master_Gps_GpsScreenSetting
        public const string Pages_Master_Gps_GpsScreenSetting_View = "Pages.Master.Gps.GpsScreenSetting.View";
        public const string Pages_Master_Gps_GpsScreenSetting_Edit = "Pages.Master.Gps.GpsScreenSetting.Edit";

        // Master_Gps_GpsCategory
        public const string Pages_Master_Gps_GpsCategory_View = "Pages.Master.Gps.GpsCategory.View";

        // Master_Gps_MaterialCategory
        public const string Pages_Master_Gps_MaterialCategory_View = "Pages.Master.Gps.MaterialCategory.View";
        public const string Pages_Master_Gps_MaterialCategory_CreateEdit = "Pages.Master.Gps.MaterialCategory.CreateEdit";
        public const string Pages_Master_Gps_MaterialCategory_Import = "Pages.Master.Gps.MaterialCategory.Import";

		// Master_Gps_MaterialCategoryMapping
		public const string Pages_Master_Gps_MaterialCategoryMapping_View = "Pages.Master.Gps.MaterialCategoryMapping.View";
		public const string Pages_Master_Gps_MaterialCategoryMapping_CreateEdit = "Pages.Master.Gps.MaterialCategoryMapping.CreateEdit";
		public const string Pages_Master_Gps_MaterialCategoryMapping_Import = "Pages.Master.Gps.MaterialCategoryMapping.Import";
        public const string Pages_Master_Gps_MaterialCategoryMapping_History = "Pages.Master.Gps.MaterialCategoryMapping.History";


		//Mst_Gps_CostCenter
		public const string Pages_Master_Gps_CostCenter_View = "Pages.Master.Gps.CostCenter.View";
        public const string Pages_Master_Gps_CostCenter_CreateEdit = "Pages.Master.Gps.CostCenter.CreateEdit";
        public const string Pages_Master_Gps_CostCenter_Import = "Pages.Master.Gps.CostCenter.Import";

        // Master_Gps_GpsMaterialRegisterByShop
        public const string Pages_Master_Gps_GpsMaterialRegisterByShop_View = "Pages.Master.Gps.GpsMaterialRegisterByShop.View";
        public const string Pages_Master_Gps_GpsMaterialRegisterByShop_Edit = "Pages.Master.Gps.GpsMaterialRegisterByShop.Edit";

        //Master_Gps_Uom
        public const string Pages_Master_Gps_Uom_View = "Pages.Master.Gps.Uom.View";
        public const string Pages_Master_Gps_Uom_Edit = "Pages.Master.Gps.Uom.Edit";

		// Master Gps Wbs CC Mapping 
		public const string Pages_Master_Gps_WbsCCMapping_View = "Pages.Master.Gps.WbsCCMapping.View";
		public const string Pages_Master_Gps_WbsCCMapping_CreateEdit = "Pages.Master.Gps.WbsCCMapping.CreateEdit";
		public const string Pages_Master_Gps_WbsCCMapping_Import = "Pages.Master.Gps.WbsCCMapping.Import";
		public const string Pages_Master_Gps_WbsCCMapping_History = "Pages.Master.Gps.WbsCCMapping.History";

		#endregion
		#region CPS
		// Master - CPS
		public const string Pages_Master_Cps = "Pages.Master.Cps";

        //Master_Cps_CpsSuppliers 
        public const string Pages_Master_Cps_CpsSuppliers_View = "Pages.Master.Cps.CpsSuppliers.View";

        // Master_Cps_Group
        public const string Pages_Master_Cps_CpsInventoryGroup_View = "Pages.Master.Cps.CpsInventoryGroup.View";

        //Pages_Master_Inventory_CpsInventoryItems
        public const string Pages_Master_Inventory_CpsInventoryItems_View = "Pages.Master.Inventory.CpsInventoryItems.View";
        #endregion
        #region HR
        // Master - CPS
        public const string Pages_Master_Hr = "Pages.Master.Hr";

        //Pages_Master_Hr_HrOrgStructure
        public const string Pages_Master_Hr_HrOrgStructure_View = "Pages.Master.Hr.HrOrgStructure.View";

        //Master_Inv_Hr_Employee
        public const string Pages_Master_Hr_HrEmployee_View = "Pages.Master.Hr.HrEmployee.View";

        //Master_Inv_Hr_HrGlcodeCombination
        public const string Pages_Master_Hr_HrGlcodeCombination_View = "Pages.Master.Hr.HrGlcodeCombination.View";

        //Master_Inv_Hr_HrPosition
        public const string Pages_Master_Hr_HrPosition_View = "Pages.Master.Hr.HrPosition.View";

        //Master_Inv_Hr_HrTitles
        public const string Pages_Master_Hr_HrTitles_View = "Pages.Master.Hr.HrTitles.View";

        #endregion
        
        #endregion

        #region INVT. SETUP
        //MASTER         
        public const string Pages_InvtSetup = "Pages.InvtSetup";

        public const string Pages_InvtSetupCommonSetup = "Pages.InvtSetupCommonSetup";

        public const string Pages_InvtSetupCheckingRule = "Pages.InvtSetupCheckingRule";

        //InvtSetup - Materrial Group
        public const string Pages_InvtSetup_MaterialGroup_View = "Pages.InvtSetup.MaterialGroup.View";

        //InvtSetup_Plant 
        public const string Pages_InvtSetup_Plant_View = "Pages.InvtSetup.Plant.View";

        //InvtSetup - Storage Location
        public const string Pages_InvtSetup_StorageLocation_View = "Pages.InvtSetup.StorageLocation.View";

        //InvtSetup - StorageLocationCategory
        public const string Pages_InvtSetup_StorageLocationCategory_View = "Pages.InvtSetup.StorageLocationCategory.View";

        //InvtSetup - Brand
        public const string Pages_InvtSetup_Brand_View = "Pages.InvtSetup.Brand.View";

        //InvtSetup_FuelType
        public const string Pages_InvtSetup_FuelType_View = "Pages.InvtSetup.FuelType.View";

        //InvtSetup_ProductType
        public const string Pages_InvtSetup_ProductType_View = "Pages.InvtSetup.ProductType.View";

        //InvtSetup_CarSeries
        public const string Pages_InvtSetup_CarSeries_View = "Pages.InvtSetup.CarSeries.View";

        public const string Pages_InvtSetup_Carfamily_View = "Pages.InvtSetup.Carfamily.View";

        //InvtSetup_CarSeries
        public const string Pages_InvtSetup_DriveTrain_View = "Pages.InvtSetup.DriveTrain.View";


        //InvtSetup_TransmissionType
        public const string Pages_InvtSetup_TransmissionType_View = "Pages.InvtSetup.TransmissionType.View";

        //InvtSetup_Uom
        public const string Pages_InvtSetup_Uom_View = "Pages.InvtSetup.Uom.View";
        public const string Pages_InvtSetup_Uom_Edit = "Pages.InvtSetup.Uom.Edit";

        //InvtSetup_MaterialType
        public const string Pages_InvtSetup_MaterialType_View = "Pages.InvtSetup.MaterialType.View";

        //InvtSetup_ValuationClass
        public const string Pages_InvtSetup_ValuationClass_View = "Pages.InvtSetup.ValuationClass.View";

        //InvtSetup_ValuationType
        public const string Pages_InvtSetup_ValuationType_View = "Pages.InvtSetup.ValuationType.View";

        //InvtSetup_MstInvGenBOMData
        public const string Pages_InvtSetup_MstInvGenBOMData_View = "Pages.InvtSetup.MstInvGenBOMData.View";


        #endregion
       
        #region MATERIAL MASTER
        //MATERIAL MASTER      
        public const string Pages_MaterialMaster= "Pages.MaterialMaster";

        // MATERIAL MASTER_MaterialMaster
        public const string Pages_MaterialMaster_MaterialMaster_View = "Pages.MaterialMaster.MaterialMaster.View";
        public const string Pages_MaterialMaster_MaterialMaster_Validate = "Pages.MaterialMaster.MaterialMaster.Validate";
        public const string Pages_MaterialMaster_MaterialMaster_History = "Pages.MaterialMaster.MaterialMaster.History";

        //MATERIAL MASTER - Vehicle
        public const string Pages_MaterialMaster_Vehicle_View = "Pages.MaterialMaster.Vehicle.View";
        public const string Pages_MaterialMaster_Vehicle_Validate = "Pages.MaterialMaster.Vehicle.Validate";
        public const string Pages_MaterialMaster_Vehicle_Edit = "Pages.MaterialMaster.Vehicle.Edit";
        #endregion

        #region CPS LINKAGE
        //CPS LINKAGE      
        public const string Pages_CpsLinkAge = "Pages.CpsLinkAge";
        //CpsLinkAge_SapAssetMaster
        public const string Pages_CpsLinkAge_SapAssetMaster_View = "Pages.CpsLinkAge.SapAssetMaster.View";
        //CpsLinkAge_InvoiceHeaders
        public const string Pages_CpsLinkAge_InvoiceHeaders_View = "Pages.CpsLinkAge.InvoiceHeaders.View";

        //CpsLinkAge_PoHeaders
        public const string Pages_CpsLinkAge_PoHeaders_View = "Pages.CpsLinkAge.PoHeaders.View";

        //CpsLinkAge_RcvShipmentHeaders
        public const string Pages_CpsLinkAge_RcvShipmentHeaders_View = "Pages.CpsLinkAge.RcvShipmentHeaders.View";
        //InvCpsInventoryItemPrice
        public const string Pages_CpsLinkAge_InventoryItemPrice_View = "Pages.CpsLinkAge.InventoryItemPrice.View";

        #endregion

        #region PROD. PLAN
        //PROD. PLAN     
        public const string Pages_ProdPlan = "Pages.ProdPlan";

        //Inventory - CKD ProductionPlanMonthly
        public const string Pages_ProdPlan_ProductionPlanMonthly_View = "Pages.ProdPlan.ProductionPlanMonthly.View";
        public const string Pages_ProdPlan_ProductionPlanMonthly_Import = "Pages.ProdPlan.ProductionPlanMonthly.Import";
       
        //
        public const string Pages_ProdPlan_ProdPlanDaily_View = "Pages.ProdPlan.ProdPlanDaily.View";
        public const string Pages_ProdPlan_ProdPlanDaily_History = "Pages.ProdPlan.ProdPlanDaily.History";


        // ASSEMBLY AINPLAN
        public const string Pages_ProdPlan_AInPlan_View = "Pages.ProdPlan.AInPlan.View";
        public const string Pages_ProdPlan_AInPlan_History = "Pages.ProdPlan.AInPlan.History";

        //ASSEMBLY PLANSHIFT
        public const string Pages_ProdPlan_APlanShiftBase_View = "Pages.ProdPlan.APlanShiftBase.View";

        //AssemblyData
        public const string Pages_ProdPlan_AssemblyData_View = "Pages.ProdPlan.AssemblyData.View";

        // Welding - Andon - WeldingPlan
        public const string Pages_ProdPlan_WeldingPlan_View = "Pages.ProdPlan.WeldingPlan.View";
        public const string Pages_ProdPlan_WeldingPlan_Edit = "Pages.ProdPlann.WeldingPlan.Edit";
        public const string Pages_ProdPlan_WeldingPlan_Import = "Pages.ProdPlan.WeldingPlan.Import";
        public const string Pages_ProdPlan_WeldingPlan_History = "Pages.ProdPlan.WeldingPlan.History";

        // Welding - Andon - WeldingProgress
        public const string Pages_ProdPlan_WeldingProgress_View = "Pages.ProdPlan.WeldingProgress.View";
        public const string Pages_ProdPlan_WeldingProgress_Edit = "Pages.ProdPlan.WeldingProgress.Edit";

        //Painting - Adon - PaintingData
        public const string Pages_ProdPlan_PaintingData_View = "Pages.ProdPlan.PaintingData.View";

        // Painting - Andon - Scan Info
        public const string Pages_ProdPlan_ScanInfo_View = "Pages.ProdPlan.ScanInfo.View";

        //Inventory CKD ProductionMapping
        public const string Pages_ProdPlan_ProductionMapping_View = "Pages.ProdPlan.ProductionMapping.View";
        public const string Pages_ProdPlan_ProductionMapping_Import = "Pages.ProdPlan.ProductionMapping.Import";
        public const string Pages_ProdPlan_ProductionMapping_History = "Pages.ProdPlan.ProductionMapping.History";

        //
        public const string Pages_ProdPlan_VehicleDetails_View = "Pages.ProdPlan.VehicleDetails.View";
        public const string Pages_ProdPlan_VehicleDetails_History = "Pages.ProdPlan.VehicleDetails.History";

        #endregion

        #region CKD
        //CKD      
        public const string Pages_Ckd = "Pages.Ckd";

        #region Ckd_Master
        //Ckd_Master
        public const string Pages_Ckd_Master = "Pages.Ckd.Master";

        //Ckd_Master_PartList
        public const string Pages_Ckd_Master_PartList_View = "Pages.Ckd.Master.PartList.View";
        public const string Pages_Ckd_Master_PartList_Import = "Pages.Ckd.Master.PartList.Import";
        public const string Pages_Ckd_Master_PartList_Validate = "Pages.Ckd.Master.PartList.Validate";
        public const string Pages_Ckd_Master_PartList_Add = "Pages.Ckd.Master.PartList.Add";
        public const string Pages_Ckd_Master_PartList_History = "Pages.Ckd.Master.PartList.History";

        //Ckd_Master_LotPart
        public const string Pages_Ckd_Master_LotPart_View = "Pages.Ckd.Master.LotPart.View";

        public const string Pages_Ckd_SMQD_OrderLeadTime_View = "Pages.Ckd.SMQD.OrderLeadTime.View";
        public const string Pages_Ckd_SMQD_OrderLeadTime_Import = "Pages.Ckd.SMQD.OrderLeadTime.Import";

		//CKD Master Mst Inv Devanning Case Type
		public const string Pages_Master_Inv_DevanningCaseType_View = "Pages.Master.Inv.DevanningCaseType.View";
		public const string Pages_Master_Inv_DevanningCaseType_Edit = "Pages.Master.Inv.DevanningCaseType.Edit";
		#endregion
		#region Ckd_Intransit
		//Ckd_Intransit
		public const string Pages_Ckd_Intransit = "Pages.Ckd.Intransit";

        //Ckd_Intransit_Shipment
        public const string Pages_Ckd_Intransit_Shipment_View = "Pages.Ckd.Intransit.Shipment.View";
      
     

        //Ckd_Intransit_Bill
        public const string Pages_Ckd_Intransit_Bill_View = "Pages.Ckd.Intransit.Bill.View";
        public const string Pages_Ckd_Intransit_Bill_History = "Pages.Ckd.Intransit.Bill.History";

        //Ckd_Intransit_Invoice
        public const string Pages_Ckd_Intransit_Invoice_View = "Pages.Ckd.Intransit.Invoice.View";
        public const string Pages_Ckd_Intransit_Invoice_History = "Pages.Ckd.Intransit.Invoice.History";

        //
        public const string Pages_Ckd_Intransit_InvoiceDetails_History = "Pages.Ckd.Intransit.InvoiceDetails.History";


        //Ckd_Intransit_FrameEngine
        public const string Pages_Ckd_Intransit_FrameEngine_View = "Pages.Ckd.Intransit.FrameEngine.View";

        //Ckd_Intransit_ContainerIntransit
        public const string Pages_Ckd_Intransit_ContainerIntransit_View = "Pages.Ckd.Intransit.ContainerIntransit.View";

        //Ckd_Intransit_ContainerInvoice
        public const string Pages_Ckd_Intransit_ContainerInvoice_View = "Pages.Ckd.Intransit.ContainerInvoice.View";
        public const string Pages_Ckd_Intransit_ContainerInvoice_History = "Pages.Ckd.Intransit.ContainerInvoice.History";

        //Ckd_Intransit_PreCustoms
        public const string Pages_Ckd_Intransit_PreCustoms_View = "Pages.Ckd.Intransit.PreCustoms.View";

        //Ckd_Intransit_CustomsDeclare
        public const string Pages_Ckd_Intransit_CustomsDeclare_View = "Pages.Ckd.Intransit.CustomsDeclare.View";


        //Ckd_Intransit_PaymentRequest
        public const string Pages_Ckd_Intransit_PaymentRequest_View = "Pages.Ckd.Intransit.PaymentRequest.View";

        //Ckd_Intransit_ContainerTransitPortPlan
        public const string Pages_Ckd_Intransit_ContainerTransitPortPlan_View = "Pages.Ckd.Intransit.ContainerTransitPortPlan.View";
        public const string Pages_Ckd_Intransit_ContainerTransitPortPlan_Edit = "Pages.Ckd.Intransit.ContainerTransitPortPlan.Edit";
        public const string Pages_Ckd_Intransit_ContainerTransitPortPlan_Import = "Pages.Ckd.Intransit.ContainerTransitPortPlan.Import";
        public const string Pages_Ckd_Intransit_ContainerTransitPortPlan_Confirm = "Pages.Ckd.Intransit.ContainerTransitPortPlan.Confirm";

        #endregion    
        #region Ckd_Warehouse
        //Ckd_Warehouse
        public const string Pages_Ckd_Warehouse = "Pages.Ckd.Warehouse";

        //Ckd_Warehouse_ContainerRentalWHPlan
        public const string Pages_Ckd_Warehouse_ContainerRentalWHPlan_View = "Pages.Ckd.Warehouse.ContainerRentalWHPlan.View";
        public const string Pages_Ckd_Warehouse_ContainerRentalWHPlan_Edit = "Pages.Ckd.Warehouse.ContainerRentalWHPlan.Edit";
        public const string Pages_Ckd_Warehouse_ContainerRentalWHPlan_Import = "Pages.Ckd.Warehouse.ContainerRentalWHPlan.Import";
        public const string Pages_Ckd_Warehouse_ContainerRentalWHPlan_Confirm = "Pages.Ckd.Warehouse.ContainerRentalWHPlan.Confirm";

        //Ckd_Warehouse_Request
        public const string Pages_Ckd_Warehouse_Request_View = "Pages.Ckd.Warehouse.Request.View";

        //Ckd_Warehouse_ContainerDeliveryGateIn
        public const string Pages_Ckd_Warehouse_ContainerDeliveryGateIn_View = "Pages.Ckd.Warehouse.ContainerDeliveryGateIn.View";

        //Ckd_Warehouse_ContainerList
        public const string Pages_Ckd_Warehouse_ContainerList_View = "Pages.Ckd.Warehouse.ContainerList.View";
        public const string Pages_Ckd_Warehouse_ContainerList_History = "Pages.Ckd.Warehouse.ContainerList.History";

        //Ckd_Warehouse_ModuleCase
        public const string Pages_Ckd_Warehouse_ModuleCase_View = "Pages.Ckd.Warehouse.ModuleCase.View";


        //Ckd_Warehouse_PartManagement
        public const string Pages_Ckd_Warehouse_PartManagement_View = "Pages.Ckd.Warehouse.PartManagement.View";

        //Ckd_Warehouse_StockPart
        public const string Pages_Ckd_Warehouse_StockPart_View = "Pages.Ckd.Warehouse.StockPart.View";
        public const string Pages_Ckd_Warehouse_StockPart_Edit = "Pages.Ckd.Warehouse.StockPart.Edit";

        //Ckd_Warehouse_StockReceiving
        public const string Pages_Ckd_Warehouse_StockReceiving_View = "Pages.Ckd.Warehouse.StockReceiving.View";

        //Ckd_Warehouse_StockIssuing
        public const string Pages_Ckd_Warehouse_StockIssuing_View = "Pages.Ckd.Warehouse.StockIssuing.View";
        public const string Pages_Ckd_Warehouse_StockIssuing_History = "Pages.Ckd.Warehouse.StockIssuing.History";

        //Ckd_Warehouse_StockBalance
        public const string Pages_Ckd_Warehouse_StockBalance_View = "Pages.Ckd.Warehouse.StockBalance.View";

        #endregion
      
        #region Prod. Area
        //Ckd_Master
        public const string Pages_Ckd_Area = "Pages.Ckd.Area";

        public const string Pages_Ckd_Area_Vehicle_View = "Pages.Ckd.Area.Vehicle.View";
        public const string Pages_Ckd_Area_Vehicle_Edit = "Pages.Ckd.Area.Vehicle.Edit";
        #endregion

        #region Physical Count
        //Ckd_Master
        public const string Pages_Ckd_Physical = "Pages.Ckd.Physical";

        // Inventory CKD PhysicalStockPeriod
        public const string Pages_Ckd_Physical_PhysicalStockPeriod_View = "Pages.Ckd.Physical.PhysicalStockPeriod.View";
        public const string Pages_Ckd_Physical_PhysicalStockPeriod_Edit = "Pages.Ckd.Physical.PhysicalStockPeriod.Edit";
        public const string Pages_Ckd_Physical_PhysicalStockPeriod_ClosePeriod = "Pages.Ckd.Physical.PhysicalStockPeriod.ClosePeriod";

        //Inventory - CKD PhysicalStockPart
        public const string Pages_Ckd_Physical_PhysicalStockPart_View = "Pages.Ckd.Physical.PhysicalStockPart.View";
        public const string Pages_Ckd_Physical_PhysicalStockPart_Edit = "Pages.Ckd.Physical.PhysicalStockPart.Edit";
        public const string Pages_Ckd_Physical_PhysicalStockPart_Import = "Pages.Ckd.Physical.PhysicalStockPart.Import";
        public const string Pages_Ckd_Physical_PhysicalStockPart_Calculator = "Pages.Ckd.Physical.PhysicalStockPart.Calculator";

        //Ckd_Physical_PhysicalStockIssuing
        public const string Pages_Ckd_Physical_PhysicalStockIssuing_View = "Pages.Ckd.Physical.PhysicalStockIssuing.View";

        //Ckd_Physical_PhysicalStockReceiving
        public const string Pages_Ckd_Physical_ReceivingPhysicalStock_View = "Pages.Ckd.Physical.ReceivingPhysicalStock.View";

        //Ckd_Physical_PhysicalConfirmLot
        public const string Pages_Ckd_Physical_PhysicalConfirmLot_View = "Pages.Ckd.Physical.PhysicalConfirmLot.View";
        public const string Pages_Ckd_Physical_PhysicalConfirmLot_Import = "Pages.Ckd.Physical.PhysicalConfirmLot.Import";

        //Ckd_Physical_PhysicalStockPartS4
        public const string Pages_Ckd_Physical_PhysicalStockPartS4_View = "Pages.Ckd.Physical.PhysicalStockPartS4.View";
        public const string Pages_Ckd_Physical_PhysicalStockPartS4_Import = "Pages.Ckd.Physical.PhysicalStockPartS4.Import";



        #endregion

        #region SMQD

        //Ckd_SMQD
        public const string Pages_Ckd_SMQD = "Pages.Ckd.SMQD";

        //Ckd_SMQD Management_View
        public const string Pages_Ckd_SMQD_Management_View = "Pages.Ckd.SMQD.Management.View";
        public const string Pages_Ckd_SMQD_Management_Import = "Pages.Ckd.SMQD.Management.Import";

        //Ckd_SMQD SmqdOrder
        public const string Pages_Ckd_SMQD_SmqdOrder_View = "Pages.Ckd.SMQD.SmqdOrder.View";
        public const string Pages_Ckd_SMQD_SmqdOrder_Import = "Pages.Ckd.SMQD.SmqdOrder.Import";
        public const string Pages_Ckd_SMQD_SmqdOrder_CreateEdit = "Pages.Ckd.SMQD.SmqdOrder.CreateEdit";


        //Ckd_SMQD PartRobbing
        public const string Pages_Ckd_SMQD_PartRobbing_View = "Pages.Ckd.SMQD.PartRobbing.View";
        public const string Pages_Ckd_SMQD_PartRobbing_Import = "Pages.Ckd.SMQD.PartRobbing.Import";
        #endregion
        #region Run Down
        //Ckd_shiping_Schedule
        public const string Pages_Ckd_Rundown = "Pages.Ckd.Rundown";
        public const string Pages_Ckd_Rundown_ShipingSchedule_View = "Pages.Ckd.Rundown.ShipingSchedule.View";
        public const string Pages_Ckd_Rundown_ShipingSchedule_Import = "Pages.Ckd.Rundown.ShipingSchedule.Import";
        //Ckd_shiping_Schedule_Firm
        public const string Pages_Ckd_Rundown_ShipingScheduleFirm_View = "Pages.Ckd.Rundown.ShipingScheduleFirm.View";
        public const string Pages_Ckd_Rundown_ShipingScheduleFirm_Edit = "Pages.Ckd.Rundown.ShipingScheduleFirm.Edit";
        public const string Pages_Ckd_Rundown_ShipingScheduleFirm_Import = "Pages.Ckd.Rundown.ShipingScheduleFirm.Import";
        // Ckd Stock Rundown Shipping Schedule
        public const string Pages_Ckd_Stock_Rundown_ShipingSchedule_View = "Pages.Ckd.Rundown.StockRundownShipingSchedule.View";
        public const string Pages_Ckd_Stock_Rundown_ShipingSchedule_Calculator = "Pages.Ckd.Rundown.StockRundownShipingSchedule.Calculator";

        //Ckd Stock Rundown Shipping Result
        public const string Pages_Ckd_Stock_Rundown_ShipingResult_View = "Pages.Ckd.Rundown.StockRundownShipingResult.View";
        public const string Pages_Ckd_Stock_Rundown_ShipingResult_Calculator = "Pages.Ckd.Rundown.StockRundownShipingResult.Calculator";

        //Ckd Stock Rundowbn Shipping Warehouse
        public const string Pages_Ckd_Stock_Rundown_Warehouse_View = "Pages.Ckd.Rundown.StockRundownShipingWarehouse.View";
        public const string Pages_Ckd_Stock_Rundown_Warehouse_Calculator = "Pages.Ckd.Rundown.StockRundownShipingWarehouse.Calculator";

        #endregion

        #endregion

        #region GPS
        //GPS      
        public const string Pages_Gps = "Pages.Gps";

        #region Gps_Master
        //Gps_Master
        public const string Pages_Gps_Master = "Pages.Gps.Master";

        //Gps_Master_PartList
        public const string Pages_Gps_Master_PartList_View = "Pages.Gps.Master.PartList.View";
        public const string Pages_Gps_Master_PartList_Import = "Pages.Gps.Master.PartList.Import";
        public const string Pages_Gps_Master_PartList_Validate = "Pages.Gps.Master.PartList.Validate";


        //Gps_Master_StockConcept
        public const string Pages_Gps_Master_StockConcept_View = "Pages.Gps.Master.StockConcept.View";
        public const string Pages_Gps_Master_StockConcept_Edit = "Pages.Gps.Master.StockConcept.Edit";
        public const string Pages_Gps_Master_StockConcept_Import = "Pages.Gps.Master.StockConcept.Import";

        //Gps_Master_Material
        public const string Pages_Gps_Master_Material_View = "Pages.Gps.Master.Material.View";
        public const string Pages_Gps_Master_Material_Edit = "Pages.Gps.Master.Material.Edit";
        public const string Pages_Gps_Master_Material_Import = "Pages.Gps.Master.Material.Import";
        public const string Pages_Gps_Master_Material_History = "Pages.Gps.Master.Material.History";

        //Gps_InvGpsUser
        public const string Pages_Gps_User = "Pages.Gps.User";

        //Gps_Master
        public const string Pages_Gps_Master_PartListByCategory_View = "Pages.Gps.Master.PartListByCategory.View";
        public const string Pages_Gps_Master_PartListByCategory_Edit = "Pages.Gps.Master.PartListByCategory.Edit";

        #endregion

        #region Gps_Receive
        //Gps_Receive
        public const string Pages_Gps_Receive = "Pages.Gps.Receive";

        //Gps_Receive_ContentList
        public const string Pages_Gps_Receive_ContentList_View = "Pages.Gps.Receive.ContentList.View";
        public const string Pages_Gps_Receive_ContentList_Edit = "Pages.Gps.Receive.ContentList.Edit";

        //Gps_Receive_Kanban
        public const string Pages_Gps_Receive_Kanban_View = "Pages.Gps.Receive.Kanban.View";
        public const string Pages_Gps_Receive_Kanban_Edit = "Pages.Gps.Receive.Kanban.Edit";

        //Gps_Receive_Stock
        public const string Pages_Gps_Receive_View = "Pages.Gps.Receive.View";
        public const string Pages_Gps_Receive_Edit = "Pages.Gps.Receive.Edit";
        public const string Pages_Gps_Receive_Import = "Pages.Gps.Receive.Import";

        #endregion
        #region Gps_Issuing
        public const string Pages_GPS_Issuing = "Pages.GPS.Issuing";

        public const string Pages_GPS_Issuing_View = "Pages.GPS.Issuing.View";
		public const string Pages_GPS_Issuing_Edit = "Pages.GPS.Issuing.Edit";
		public const string Pages_GPS_Issuing_ImportGps = "Pages.GPS.Issuing.ImportGps";
        public const string Pages_GPS_Issuing_ImportShop = "Pages.GPS.Issuing.ImportShop";
        public const string Pages_GPS_Issuing_ConfirmGps = "Pages.GPS.Issuing.ConfirmGps";
        

        public const string Pages_GPS_Issuings_View = "Pages.GPS.Issuings.View";
        public const string Pages_GPS_Issuings_MemberShop = "Pages.GPS.Issuings.MemberShop";
        public const string Pages_GPS_Issuings_LeaderShop = "Pages.GPS.Issuings.LeaderShop";
        public const string Pages_GPS_Issuings_LeaderGps = "Pages.GPS.Issuings.LeaderGps";
        public const string Pages_GPS_Issuings_History = "Pages.GPS.Issuings.History";


        public const string Pages_GPS_Issuing_Mapping_View = "Pages.GPS.Issuing.Mapping.View";
        public const string Pages_GPS_Issuing_Mapping_ReMap = "Pages.GPS.Issuing.Mapping.ReMap";

        #endregion
        #region Gps_Order
        //Gps_Order
        public const string Pages_Gps_Order = "Pages.Gps.Order";

        //Gps_Order_DailyOrder
        public const string Pages_Gps_Order_DailyOrder_View = "Pages.Gps.Order.DailyOrder.View";
        public const string Pages_Gps_Order_DailyOrder_Edit = "Pages.Gps.Order.DailyOrder.Edit";

        #endregion

       
        #region Gps_Rundown
        //Gps_Rundown
        public const string Pages_Gps_Rundown = "Pages.Gps.Rundown";

        // Gps_Rundown_StockRundown
        public const string Pages_Gps_Rundown_StockRundown_View = "Pages.Gps.Rundown.GpsStockRundown.View";
        public const string Pages_Gps_Rundown_StockRundown_Calculator = "Pages.Gps.Rundown.GpsStockRundown.Calculator";

        // Gps_Rundown_StockRundownTransaction
        public const string Pages_Gps_Rundown_StockRundownTransaction_View = "Pages.Gps.Rundown.GpsStockRundownTransaction";


        #endregion

        #region WareHouse

        public const string Pages_Gps_Warehouse = "Pages.Gps.Warehouse";

        public const string Pages_Gps_Warehouse_FinStock_View = "Pages.Gps.Warehouse.FinStock.View";
        public const string Pages_Gps_Warehouse_GpsStock_View = "Pages.Gps.Warehouse.GpsStock.View";

        //Gps Warehouse Stock Issuing Transaction Details
        public const string Pages_Gps_Warehouse_StockIssuingTransDetails_View = "Pages.Gps.Warehouse.StockIssuingTransDetails.View";
        public const string Pages_Gps_Warehouse_StockIssuingTransDetails_Import = "Pages.Gps.Warehouse.StockIssuingTransDetails.Import";

        //Gps Warehouse Stock Receiving Transaction Details
        public const string Pages_Gps_Warehouse_StockReceivingTransDetails_View = "Pages.Gps.Warehouse.StockReceivingTransDetails.View";
        public const string Pages_Gps_Warehouse_StockReceivingTransDetails_Import = "Pages.Gps.Warehouse.StockReceivingTransDetails.Import";
      
        #endregion

        #endregion

        #region DM&IHP
        public const string Pages_DMIHP = "Pages.DMIHP";

        //Master 
        public const string Pages_DMIHP_Mst = "Pages.DMIHP.Mst";

        //Master  DRM Part List
        public const string Pages_DMIHP_Mst_DRMPartList_View = "Pages.DMIHP.Mst.DRMPartList.View";
        public const string Pages_DMIHP_Mst_DRMPartList_Import = "Pages.DMIHP.Mst.DRMPartList.Import";
        public const string Pages_DMIHP_Mst_DRMPartList_Edit = "Pages.DMIHP.Mst.DRMPartList.Edit";
        public const string Pages_DMIHP_Mst_DRMPartList_Asset = "Pages.DMIHP.Mst.DRMPartList.Asset";
        public const string Pages_DMIHP_Mst_DRMPartList_History = "Pages.DMIHP.Mst.DRMPartList.History";

        //Master  IHP Part List
        public const string Pages_DMIHP_Mst_IHPPartList_View = "Pages.DMIHP.Mst.IHPPartList.View";
        public const string Pages_DMIHP_Master_PartList_Validate = "Pages.DMIHP.Master.PartList.Validate";
        public const string Pages_DMIHP_Master_PartList_Edit = "Pages.DMIHP.Master.PartList.Edit";
        public const string Pages_DMIHP_Master_PartList_History = "Pages.DMIHP.Master.PartList.History";

        //
        public const string Pages_DMIHP_Mst_InvIphMatCustomDeclare_View = "Pages.DMIHP.Mst.InvIphMatCustomDeclare.View";
  

        //GR 
        public const string Pages_DMIHP_GR = "Pages.DMIHP.GR";

        // GR_ImportPlan
        public const string Pages_DMIHP_GR_ImportPlan_View = "Pages.DMIHP.GR.ImportPlan.View";
        public const string Pages_DMIHP_GR_ImportPlan_Import = "Pages.DMIHP.GR.ImportPlan.Import";
        public const string Pages_DMIHP_GR_ImportPlan_Confirm = "Pages.DMIHP.GR.ImportPlan.Confirm";
        public const string Pages_DMIHP_GR_ImportPlan_Edit = "Pages.DMIHP.GR.ImportPlan.Edit";

        //GR_LocalPlan
        public const string Pages_DMIHP_GR_LocalPlan_View = "Pages.DMIHP.GR.LocalPlan.View";




        //GI 
        public const string Pages_DMIHP_GI = "Pages.DMIHP.GI";

        //Stock 
        public const string Pages_DMIHP_Stock = "Pages.DMIHP.Stock";

        //StockPart
        public const string Pages_DMIHP_Stock_StockPart_View = "Pages.DMIHP.Stock.StockPart.View";
        //IHP Stock Part
        public const string Pages_DMIHP_Stock_InvIphStockPart_View = "Pages.DMIHP.Stock.InvIphStockPart.View";

        //StockPartExcel
        public const string Pages_DMIHP_Stock_StockPartExcel_View = "Pages.DMIHP.Stock.StockPartExcel.View";
        public const string Pages_DMIHP_Stock_StockPartExcel_Import = "Pages.DMIHP.Stock.StockPartExcel.Import";

        //Rundown
        public const string Pages_DMIHP_Rundown = "Pages.DMIHP.Rundow";

        //StockRundown
        public const string Pages_DMIHP_Rundown_StockRundown_View = "Pages.DMIHP.Rundow.StockRundown.View";
        public const string Pages_DMIHP_Rundown_StockRundown_Calculator = "Pages.DMIHP.Rundow.StockRundown.Calculator";


        //Order
        public const string Pages_DMIHP_Order_View = "Pages.DMIHP.Order.View";

        #endregion

        #region PIO

        public const string Pages_PIO = "Pages.PIO";

        //PIO Master
        public const string Pages_PIO_Master = "Pages.PIO.Master";

        //PIO Master Part List
        public const string Pages_PIO_Master_PartList_View = "Pages.PIO.Master.PartList.View";
        public const string Pages_PIO_Master_PartList_CreateEdit = "Pages.PIO.Master.PartList.CreateEdit";

        // PIO MASTER Supplier Infor 
        public const string Pages_PIO_Master_Supplier_Info_View = "Pages.PIO.Master.SupplierInfo.View";
		public const string Pages_PIO_Master_Supplier_Info_CreateEdit = "Pages.PIO.Master.SupplierInfo.CreateEdit";
		public const string Pages_PIO_Master_Supplier_Info_Delete = "Pages.PIO.Master.SupplierInfo.Delete";

		//PIO Master Part List Inl
		public const string Pages_PIO_Master_PartListInl_View = "Pages.PIO.Master.PartListInl.View";
        public const string Pages_PIO_Master_PartListInl_Validate = "Pages.PIO.Master.PartListInl.Validate";
		public const string Pages_PIO_Master_PartListInl_Add = "Pages.PIO.Master.PartListInl.Add";
        public const string Pages_PIO_Master_PartListInl_Import = "Pages.PIO.Master.PartListInl.Import";
        public const string Pages_PIO_Master_PartListInl_History = "Pages.PIO.Master.PartListInl.History";

        //PIO Master Part List Off
        public const string Pages_PIO_Master_PartListOff_View = "Pages.PIO.Master.PartListOff.View";
        public const string Pages_PIO_Master_PartListOff_Validate = "Pages.PIO.Master.PartListOff.Validate";
        public const string Pages_PIO_Master_PartListOff_Add = "Pages.PIO.Master.PartListOff.Add";
        public const string Pages_PIO_Master_PartListOff_Import = "Pages.PIO.Master.PartListOff.Import";

        //PIO Master Tmss_TmssDispatchPlan
        public const string Pages_PIO_Master_Tmss_TmssDispatchPlan_View = "Pages.PIO.Master.Tmss.TmssDispatchPlan.View";
        public const string Pages_PIO_Master_Tmss_TmssDispatchPlan_History = "Pages.PIO.Master.Tmss.TmssDispatchPlan.History";

        //PIO Topsse Invoice
        public const string Pages_PIO_Master_TopsseInvoice_View = "Pages.PIO.Master.TopsseInvoice.View";
        public const string Pages_PIO_Master_TopsseInvoice_Receive = "Pages.PIO.Master.TopsseInvoice.Receive";

        //PIO Warehouse
        public const string Pages_PIO_Warehouse = "Pages.PIO.Warehouse";

        //PIO Stock
        public const string Pages_PIO_Warehouse_Stock_View = "Pages.PIO.Warehouse.Stock.View";

        //PIO StockTransaction
        public const string Pages_PIO_Warehouse_StockTransaction_View = "Pages.PIO.Warehouse.StockTransaction.View";

        //PIO Stock_issuing
        public const string Pages_PIO_Warehouse_StockIssuing_View = "Pages.PIO.Warehouse.StockIssuing.View";

        //PIO StockRundown
        public const string Pages_PIO_Warehouse_StockRundown_View = "Pages.PIO.Warehouse.StockRundown.View";
        public const string Pages_PIO_Warehouse_StockRundown_Calculator = "Pages.PIO.Warehouse.StockRundown.Calculator";

        //PIO StockRundownTransaction
        public const string Pages_PIO_Warehouse_StockRundownTransaction_View = "Pages.PIO.Warehouse.StockRundownTransaction.View";

        // PIO Stock_receiving

        public const string Pages_PIO_Warehouse_StockReceiving_View = "Pages.PIO.Warehouse.StockReceiving.View";
        //InvPioProductionPlanMonthly
        public const string Pages_PIO_Master_ProductionPlanMonthly_View = "Pages.PIO.Master.ProductionPlanMonthly.View";
        public const string Pages_PIO_Master_ProductionPlanMonthly_Import = "Pages.PIO.Master.ProductionPlanMonthly.Import";
        #endregion

        #region INTERFACE
        //INTERFACE
        public const string Pages_Interface = "Pages.Interface";

        public const string Pages_Interface_Bom = "Pages.Interface.Bom";

        //INTERFACE_Bom_InvGenBOMData
        public const string Pages_Interface_Bom_InvGenBOMData_View = "Pages.Interface.Bom.InvGenBOMData.View";

        //Interface_IF
        public const string Pages_Interface_IF = "Pages.Interface.IF";

        //Interface_IF_FQF3MM01
        public const string Pages_Interface_IF_FQF3MM01_View = "Pages.Interface.IF.FQF3MM01.View";
        public const string Pages_Interface_IF_FQF3MM01_ReCreate = "Pages.Interface.IF.FQF3MM01.ReCreate";

        //Interface_IF_FQF3MM02
        public const string Pages_Interface_IF_FQF3MM02_View = "Pages.Interface.IF.FQF3MM02.View";
        public const string Pages_Interface_IF_FQF3MM02_ReCreate = "Pages.Interface.IF.FQF3MM02.ReCreate";

        //Inventory_IF_FQF3MM_LV2
        public const string Pages_Interface_IF_FQF3MM_LV2_View = "Pages.Interface.IF.FQF3MM_LV2.View";
        //public const string Pages_Interface_IF_FQF3MM_LV2_ReCreate = "Pages.Interface.IF.FQF3MM_LV2.ReCreate";

        //Interface_IF_FQF3MM03
        public const string Pages_Interface_IF_FQF3MM03_View = "Pages.Interface.IF.FQF3MM03.View";
        public const string Pages_Interface_IF_FQF3MM03_ReCreate = "Pages.Interface.IF.FQF3MM03.ReCreate";

        //Interface_IF_FQF3MM04
        public const string Pages_Interface_IF_FQF3MM04_View = "Pages.Interface.IF.FQF3MM04.View";
        public const string Pages_Interface_IF_FQF3MM04_ReCreate = "Pages.Interface.IF.FQF3MM04.ReCreate";

        //Interface_IF_FQF3MM05
        public const string Pages_Interface_IF_FQF3MM05_View = "Pages.Interface.IF.FQF3MM05.View";
        public const string Pages_Interface_IF_FQF3MM05_ReCreate = "Pages.Interface.IF.FQF3MM05.ReCreate";

        //Interface_IF_FQF3MM06
        public const string Pages_Interface_IF_FQF3MM06_View = "Pages.Interface.IF.FQF3MM06.View";
        public const string Pages_Interface_IF_FQF3MM06_ReCreate = "Pages.Interface.IF.FQF3MM06.ReCreate";

        //Interface_IF_FQF3MM07
        public const string Pages_Interface_IF_FQF3MM07_View = "Pages.Interface.IF.FQF3MM07.View";
        public const string Pages_Interface_IF_FQF3MM07_ReCreate = "Pages.Interface.IF.FQF3MM07.ReCreate";

        #endregion

        #region SPP
        public const string Pages_SPP = "Pages.SPP";
        public const string Pages_SPP_Inventory = "Pages.SPP.Inventory";

        public const string Pages_SPP_Cost_View = "Pages.SPP.Inventory.Cost.View";
        public const string Pages_SPP_InvoiceDetails_View = "Pages.SPP.Inventory.InvoiceDetails.View";
        public const string Pages_SPP_Shipping_View = "Pages.SPP.Inventory.Shipping.View";
        public const string Pages_SPP_Stock_View = "Pages.SPP.Inventory.Stock.View";

        public const string Pages_SPP_CostOfSaleSummary_View = "Pages.SPP.Inventory.CostOfSaleSummary.View";

        
        /// </summary>
        public const string Pages_SPP_Master = "Pages.SPP.Master";
        public const string Pages_SPP_Master_Customer = "Pages.SPP.Master.Customer";
        public const string Pages_SPP_Master_GLAccount = "Pages.SPP.Master.GLAccount";

        #endregion

        //đã làm tới đây

    }
}
