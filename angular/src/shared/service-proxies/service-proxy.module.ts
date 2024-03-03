import { MstInvGpsMaterialCategoryServiceProxy } from '@shared/service-proxies/service-proxies';
import { AbpHttpInterceptor, RefreshTokenService, AbpHttpConfigurationService } from 'abp-ng2-module';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import * as ApiServiceProxies from './service-proxies';
import { ZeroRefreshTokenService } from '@account/auth/zero-refresh-token.service';
import { ZeroTemplateHttpConfigurationService } from './zero-template-http-configuration.service';

@NgModule({
    providers: [

        ApiServiceProxies.SapIFServiceProxy,
        ApiServiceProxies.AuditLogServiceProxy,
        ApiServiceProxies.CachingServiceProxy,
        ApiServiceProxies.ChatServiceProxy,
        ApiServiceProxies.CommonLookupServiceProxy,
        ApiServiceProxies.MstCmmMaterialMasterServiceProxy,
        ApiServiceProxies.EditionServiceProxy,
        ApiServiceProxies.FriendshipServiceProxy,
        ApiServiceProxies.HostSettingsServiceProxy,
        ApiServiceProxies.InstallServiceProxy,
        ApiServiceProxies.LanguageServiceProxy,
        ApiServiceProxies.NotificationServiceProxy,
        ApiServiceProxies.OrganizationUnitServiceProxy,
        ApiServiceProxies.PermissionServiceProxy,
        ApiServiceProxies.ProfileServiceProxy,
        ApiServiceProxies.RoleServiceProxy,
        ApiServiceProxies.SessionServiceProxy,
        ApiServiceProxies.TenantServiceProxy,
        ApiServiceProxies.TenantDashboardServiceProxy,
        ApiServiceProxies.TenantSettingsServiceProxy,
        ApiServiceProxies.TimingServiceProxy,
        ApiServiceProxies.UserServiceProxy,
        ApiServiceProxies.UserLinkServiceProxy,
        ApiServiceProxies.UserLoginServiceProxy,
        ApiServiceProxies.WebLogServiceProxy,
        ApiServiceProxies.AccountServiceProxy,
        ApiServiceProxies.TokenAuthServiceProxy,
        ApiServiceProxies.TenantRegistrationServiceProxy,
        ApiServiceProxies.HostDashboardServiceProxy,
        ApiServiceProxies.PaymentServiceProxy,
        ApiServiceProxies.DemoUiComponentsServiceProxy,
        ApiServiceProxies.InvoiceServiceProxy,
        ApiServiceProxies.SubscriptionServiceProxy,
        ApiServiceProxies.InstallServiceProxy,
        ApiServiceProxies.UiCustomizationSettingsServiceProxy,
        ApiServiceProxies.PayPalPaymentServiceProxy,
        ApiServiceProxies.StripePaymentServiceProxy,
        ApiServiceProxies.DashboardCustomizationServiceProxy,
        ApiServiceProxies.WebhookEventServiceProxy,
        ApiServiceProxies.WebhookSubscriptionServiceProxy,
        ApiServiceProxies.WebhookSendAttemptServiceProxy,
        ApiServiceProxies.UserDelegationServiceProxy,
        ApiServiceProxies.DynamicPropertyServiceProxy,
        ApiServiceProxies.DynamicEntityPropertyDefinitionServiceProxy,
        ApiServiceProxies.DynamicEntityPropertyServiceProxy,
        ApiServiceProxies.DynamicPropertyValueServiceProxy,
        ApiServiceProxies.DynamicEntityPropertyValueServiceProxy,
        ApiServiceProxies.TwitterServiceProxy,

        ApiServiceProxies.MstInvGpsScreenSettingServiceProxy,

        ApiServiceProxies.InvGenBOMDataServiceProxy,
        ApiServiceProxies.MstInvGenBOMDataSServiceProxy,

        //Cmm Report Request
        ApiServiceProxies.CmmReportRequestServiceProxy,

        // Assembly VehicleDetais
        ApiServiceProxies.AsyAdoVehicleDetailsServiceProxy,
        //Assembly totaldelayfinalasakai
        ApiServiceProxies.AsyAdoTotalDelayFinalAsakaiServiceProxy,
        //Assembly AInPlan
        ApiServiceProxies.AsyAdoAInPlanServiceProxy,
        //Assembly PlanShift
        ApiServiceProxies.AsyAdoAPlanShiftServiceProxy,
        //AssemblyData
        ApiServiceProxies.AsyAdoAssemblyDataServiceProxy,

        ApiServiceProxies.AsyAdoAssemblyScreenServiceProxy,

        //Mst Wpt Calendar
        ApiServiceProxies.MstWptCalendarServiceProxy,
        // Master Wpt Shop
        ApiServiceProxies.MstWptShopServiceProxy,
        //Master Wpt SeasonMonth
        ApiServiceProxies.MstWptSeasonMonthServiceProxy,
        //Master Wpt PatterH
        ApiServiceProxies.MstWptPatternHServiceProxy,
        //Master Wpt WorkingTime
        ApiServiceProxies.MstWptWorkingTimeServiceProxy,
        //Master Wpt PatternD
        ApiServiceProxies.MstWptPatternDServiceProxy,
        //Master Wpt WorkingType
        ApiServiceProxies.MstWptWorkingTypeServiceProxy,
        //Master Wpt Week
        ApiServiceProxies.MstWptWeekServiceProxy,
        // //Master Wpt DailyWorkingTime
        ApiServiceProxies.MstWptDailyWorkingTimeServiceProxy,
        // Master Common Storage Location
        ApiServiceProxies.MstCmmStorageLocationServiceProxy,

        // Master Common Material Group
        ApiServiceProxies.MstCmmMaterialGroupServiceProxy,

        // Master Common Lookup
        ApiServiceProxies.MstCmmLookupServiceProxy,
        ApiServiceProxies.MstLookupByDomainCodeServiceProxy,

        // Master Common Devanningcasetype
        ApiServiceProxies.MstCmmDevanningCaseTypeServiceProxy,
        // Master Common Shoptype
        ApiServiceProxies.MstCmmShopTypeServiceProxy,


        //Master Common Color
        ApiServiceProxies.MstCmmColorServiceProxy,


        //Master Common TaktTime
        ApiServiceProxies.MstCmmTaktTimeServiceProxy,
        //Master Common Model
        ApiServiceProxies.MstCmmModelServiceProxy,
        //Master Common Storage Location Category
        ApiServiceProxies.MstCmmStorageLocationCategoryServiceProxy,

        //Master Common VehicleColorType
        ApiServiceProxies.MstCmmVehicleColorTypeServiceProxy,

        //Master Common VehicleName
        ApiServiceProxies.MstCmmVehicleNameServiceProxy,

        //Master Common EngineMaster
        ApiServiceProxies.MstCmmEngineMasterServiceProxy,

        //Master Common EngineType
        ApiServiceProxies.MstCmmEngineTypeServiceProxy,

        //Master Common Tax
        ApiServiceProxies.MstCmmTaxServiceProxy,

        // MstCmmPlant
        ApiServiceProxies.MstCmmPlantServiceProxy,

        // Master Common Product Type
        ApiServiceProxies.MstCmmProductTypeServiceProxy,

        //Master Common CarSeries
        ApiServiceProxies.MstCmmCarSeriesServiceProxy,

        //Master Common Carfarmily
        ApiServiceProxies.MstCmmCarfamilyServiceProxy,


        //Master Common DriveTrain
        ApiServiceProxies.MstCmmDriveTrainServiceProxy,

        //brand
        ApiServiceProxies.MstCmmBrandServiceProxy,

        //Master Common FuelType
        ApiServiceProxies.MstCmmFuelTypeServiceProxy,

        //Master Common BusinessParter
        ApiServiceProxies.MstCmmBusinessParterServiceProxy,
        //Master Common EngineModel
        ApiServiceProxies.MstCmmEngineModelServiceProxy,
        //Master Common ProductGroup
        ApiServiceProxies.MstCmmProductGroupServiceProxy,

        //MstCmmMMCheckingRule
        ApiServiceProxies.MstCmmMMCheckingRuleServiceProxy,

        //MstCmmMMValidationResult
        ApiServiceProxies.MstCommonMMValidationResultServiceProxy,

        ApiServiceProxies.MstCmmLotCodeGradeServiceProxy,
        ApiServiceProxies.MstCmmTransmissionTypeServiceProxy,
        ApiServiceProxies.MstCmmExchangeRateServiceProxy,
        ApiServiceProxies.MstCmmUomServiceProxy,
        // Master Common Material Type
        ApiServiceProxies.MstCmmMaterialTypeServiceProxy,

        //Master Common Valuation Class
        ApiServiceProxies.MstCmmValuationClassServiceProxy,


        //Master Common ValuationType
        ApiServiceProxies.MstCmmValuationTypeServiceProxy,

        //Master Common Vehicle CBU
        ApiServiceProxies.MstCmmVehicleCBUServiceProxy,

        //AssemblyData
        ApiServiceProxies.AsyAdoAssemblyScreenServiceProxy,

        //Master LogA PlcSignal
        ApiServiceProxies.MstLgaPlcSignalServiceProxy,
        ApiServiceProxies.MstLgaLdsTripServiceProxy,

        //Master LogA Bp2PartList
        ApiServiceProxies.MstLgaBp2PartListServiceProxy,

        //Master LogA Bp2PartListGrade
        ApiServiceProxies.MstLgaEkbPartListGradeServiceProxy,

        //Master LogA Bp2 progcess
        ApiServiceProxies.MstLgaBp2ProcessServiceProxy,

        //Master - LogA - Bp2Ecar
        ApiServiceProxies.MstLgaBp2EcarServiceProxy,

        //Master - LogA - Bp2PartListGrade
        ApiServiceProxies.MstLgaBp2PartListGradeServiceProxy,

        //Maser - LogA - EkbPartList
        ApiServiceProxies.MstLgaEkbPartListServiceProxy,

        //Master LogA EkbProcess
        ApiServiceProxies.MstLgaEkbProcessServiceProxy,

        //Master - LogA - EkbUser
        ApiServiceProxies.MstLgaEkbUserServiceProxy,

        //Master LogA BarProcess
        ApiServiceProxies.MstLgaBarProcessServiceProxy,
        //Master LogA BarUser
        ApiServiceProxies.MstLgaBarUserServiceProxy,
        //Master LogA PcRack
        ApiServiceProxies.MstLgaPcRackServiceProxy,

        //Master LogA Sps Rack
        ApiServiceProxies.MstLgaSpsRackServiceProxy,


        //Mst Inv ContainerStatus
        ApiServiceProxies.MstInvContainerStatusServiceProxy,
        //Mst Inv ContainerDeliveryType
        ApiServiceProxies.MstInvContainerDeliveryTypeServiceProxy,
        //Mst Inv Forwarder
        ApiServiceProxies.MstInvForwarderServiceProxy,

        ApiServiceProxies.MstInvInvoiceStatusServiceProxy,
        // Mst Inv InvoiceStatus
        ApiServiceProxies.MstInvGpsTmvPicServiceProxy,
        // Mst Inv SupplierList
        ApiServiceProxies.MstInvSupplierListServiceProxy,

        // Master CKD
        ApiServiceProxies.MstCkdCustomsLeadtimeServiceProxy,

        ApiServiceProxies.MstInvCkdRentalWarehouseServiceProxy,


        // MstGpsMaterialRegisterByShopDto
        ApiServiceProxies.MstGpsMaterialRegisterByShopServiceProxy,

        // Master Inventory CustomsStatus
        ApiServiceProxies.MstInvCustomsStatusServiceProxy,
        //MstInvPIOPartTypeServiceProxy
        ApiServiceProxies.MstInvPIOPartTypeServiceProxy,

        //MstPioImpSupplier
        ApiServiceProxies.MstPioImpSupplierServiceProxy,

        //  MstInvHrGlCodeCombination
        ApiServiceProxies.MstInvHrGlCodeCombinationServiceProxy,

        //Master Inventory InvHrEmployee
        ApiServiceProxies.MstInvHrEmployeeServiceProxy,

        ApiServiceProxies.MstInvLotPartServiceProxy,
         // Master Inv Devanningcasetype
         ApiServiceProxies.MstInvDevanningCaseTypeServiceProxy,

        ApiServiceProxies.MstInvHrTitlesServiceProxy,

        ApiServiceProxies.MstInvHrPositionServiceProxy,

        ApiServiceProxies.MstInvPIOEmailServiceProxy,


        // Master frame
        ApiServiceProxies.MstFrmProcessServiceProxy,
        //Master LogW
        ApiServiceProxies.MstLgwEciPartServiceProxy,
        //Master LogW module
        ApiServiceProxies.MstLgwEciPartModuleServiceProxy,
        //Master LogW ModelGrade
        ApiServiceProxies.MstLgwModelGradeServiceProxy,
        //Master LogW PickingTablet
        ApiServiceProxies.MstLgwPickingTabletServiceProxy,
        //Master LogW PlcSignal
        ApiServiceProxies.MstLgwPlcSignalServiceProxy,


        //Master Pts bmp part list
        ApiServiceProxies.MstPtsBmpPartListServiceProxy,
        //Master Pts bmp part type
        ApiServiceProxies.MstPtsBmpPartTypeServiceProxy,
        // Mst Pts Painting
        ApiServiceProxies.MstPtsPaintingProcessServiceProxy,

        // Mst Pts Inventory
        ApiServiceProxies.MstPtsInventoryStdServiceProxy,

        //Master Plm Matrix
        ApiServiceProxies.MasterPlmMatrixServiceProxy,
        //Master Plm MatrixLotCode
        ApiServiceProxies.MasterPlmMatrixLotCodeServiceProxy,
        //Master Plm LotCodeGrade
        ApiServiceProxies.MstPlmLotCodeGradeServiceProxy,
        //Master Plm Part(service-proxy)
        ApiServiceProxies.MasterPlmPartServiceProxy,
        ApiServiceProxies.MstLgwAdoCallingLightServiceProxy,

        //Master Welding Process
        ApiServiceProxies.MstWldProcessServiceProxy,
        //Master Welding PunchIndicator
        ApiServiceProxies.MstWldPunchIndicatorServiceProxy,

        //Master LogW ContDevaningLT
        ApiServiceProxies.MstLgwContDevanningLTServiceProxy,

        //Master LogW RenbanModule
        ApiServiceProxies.MstLgwRenbanModuleServiceProxy,

        //Master LogW LayoutSetup
        ApiServiceProxies.MstLgwLayoutSetupServiceProxy,
        //Master LogW Layout
        ApiServiceProxies.MstLgwLayoutServiceProxy,
        //Master LogW RobbingLane
        ApiServiceProxies.MstLgwRobbingLaneServiceProxy,
        //Master LogW ScreenConfig
        ApiServiceProxies.MstLgwScreenConfigServiceProxy,
        //Master LogW pickingtabletsetup
        ApiServiceProxies.MstLgwPickingTabletProcessServiceProxy,
        // //Master LogW unpackingpart
        ApiServiceProxies.MstLgwUnpackingPartServiceProxy,
        ApiServiceProxies.LgwPikPickingSignalServiceProxy,
        // Master Inventory CustomsPort
        ApiServiceProxies.MstInvCustomsPortServiceProxy,
        // Master Inventory ShippingCompany
        ApiServiceProxies.MstInvShippingCompanyServiceProxy,

        // Master Inventory GpsSupplierInfo
        ApiServiceProxies.MstInvGpsSupplierInfoServiceProxy,

        //Master Inventory GpsTmvPic
        ApiServiceProxies.MstInvGpsTmvPicServiceProxy,

         //MstInvGpsCategory
         ApiServiceProxies.MstInvGpsCategoryServiceProxy,
         //MstInvGpsMasterialCategory
         ApiServiceProxies.MstInvGpsMaterialCategoryServiceProxy,
        //MstInvGpsMasterialCategoryMappiong
        ApiServiceProxies.MstGpsMaterialCategoryMappingServiceProxy,
        //MstInvGpsMasUom
        ApiServiceProxies.MstGpsUomServiceProxy,
        //MstGpsWbsCCMapping
        ApiServiceProxies.MstGpsWbsCCMappingServiceProxy,
        // MAster Inventory GpsCalendar
        ApiServiceProxies.MstInvGpsCalendarServiceProxy,

        // Master Inventory GpsSupplierTruck
        ApiServiceProxies.MstInvGpsTruckSupplierServiceProxy,

        // Master Inventory GpsSupplierPic
        ApiServiceProxies.MstInvGpsSupplierPicServiceProxy,
        //MstInvGpsCostCenter
        ApiServiceProxies.MstGpsCostCenterServiceProxy,

        //Master Inventory Group
        ApiServiceProxies.MstInvCpsInventoryGroupServiceProxy,

        // Master Inventory GpsSupplier
        ApiServiceProxies.MstInvCpsSuppliersServiceProxy,

        // Master Inventory CpsInventoryItems
        ApiServiceProxies.MstInvCpsInventoryItemsServiceProxy,

        // Master Inventory GpsSupplierOrderTime
        ApiServiceProxies.MstInvGpsSupplierOrderTimeServiceProxy,

        // Master Inventory HrOrgStructure
        ApiServiceProxies.MstInvHrOrgStructureServiceProxy,

        //Gps Issuings
        ApiServiceProxies.InvGpsIssuingsServiceProxy,

        //CustomsLeadTime
        ApiServiceProxies.MstInvCustomsLeadTimeServiceProxy,
        //DemDetFees
        ApiServiceProxies.MstInvDemDetFeesServiceProxy,
        //DemDetDays
        ApiServiceProxies.MstInvDemDetDaysServiceProxy,

        // Master Spp_Customer
        ApiServiceProxies.MstSppCustomerServiceProxy,

        // Master Spp_GiAccount
        ApiServiceProxies.MstSppGlAccountServiceProxy,

        // Gps Fin Stock
        ApiServiceProxies.InvGpsFinStockServiceProxy,

        //LogW Plc Signal
        ApiServiceProxies.LgwPlcSignalServiceProxy,

        //Inv Stock
        ApiServiceProxies.InvStockServiceProxy,

        //Inventory Ckd
        ApiServiceProxies.InvCkdBillServiceProxy,
        ApiServiceProxies.InvCkdShipmentServiceProxy,
        ApiServiceProxies.InvCkdContainerIntransitServiceProxy,
        ApiServiceProxies.InvCkdContainerDeliveryGateInServiceProxy,
        ApiServiceProxies.InvCkdFrameEngineServiceProxy,
        ApiServiceProxies.InvCkdContainerRentalWHPlanServiceProxy,
        ApiServiceProxies.InvCkdInvoiceServiceProxy,
        ApiServiceProxies.InvCkdContainerTransitPortPlanServiceProxy,
        ApiServiceProxies.InvCkdRequestServiceProxy,
        ApiServiceProxies.InvCkdPreCustomsServiceProxy,
        ApiServiceProxies.InvCkdPaymentRequestServiceProxy,
        ApiServiceProxies.InvCkdCustomsDeclareServiceProxy,
        ApiServiceProxies.InvCkdPartListServiceProxy,
        ApiServiceProxies.InvCkdPartRobbingServiceProxy,
        ApiServiceProxies.InvCkdModuleCaseServiceProxy,
        ApiServiceProxies.InvCkdPhysicalConfirmLotServiceProxy,
        ApiServiceProxies.InvCkdStockIssuingServiceProxy,
        ApiServiceProxies.InvCkdStockPartServiceProxy,
        ApiServiceProxies.InvCkdStockBalanceServiceProxy,
        ApiServiceProxies.InvCkdContainerInvoiceServiceProxy,


        ApiServiceProxies.InvCkdContainerListServiceProxy,
        ApiServiceProxies.InvCkdStockReceivingServiceProxy,

        ApiServiceProxies.InvGpsIssuingServiceProxy,


        ApiServiceProxies.InvCkdPhysicalStockPeriodServiceProxy,
        ApiServiceProxies.InvCkdPhysicalStockPartPeriodServiceProxy,

        ApiServiceProxies.InvCkdPhysicalStockPartServiceProxy,
        ApiServiceProxies.InvCkdProdPlanDailyServiceProxy,
        ApiServiceProxies.InvCkdShippingScheduleServiceProxy,
        ApiServiceProxies.InvCkdShippingScheduleFirmServiceProxy,
        //Inventory CKD ProductionPlanMonthly
        ApiServiceProxies.InvCkdProductionPlanMonthlyServiceProxy,
        //
        ApiServiceProxies.InvCkdReceivingPhysicalStockServiceProxy,


        //InvCkdPhysicalStockIssuing
        ApiServiceProxies.InvCkdPhysicalStockIssuingServiceProxy,
        //Inventory CKD
        ApiServiceProxies.InvCkdVehhicleServiceProxy,
        ApiServiceProxies.InvCkdProductionMappingServiceProxy,


        //Inventory GPS
        ApiServiceProxies.InvGpsDailyOrderServiceProxy,

        //Inventory gps content list
        ApiServiceProxies.InvGpsContentListServiceProxy,

        //Inventory GPS PartList
        ApiServiceProxies.InvGpsPartListServiceProxy,

        //Inventory GPS StockRundown
        ApiServiceProxies.InvGpsStockRundownServiceProxy,

        //Inventory GPS StockRundownTransaction
        ApiServiceProxies.InvGpsStockRundownTransactionServiceProxy,

        //Inventory GPS StockIssuingTransactionDetails
        ApiServiceProxies.InvGpsStockIssuingTransactionServiceProxy,

        //Inventory GPS StockReceivingTransactionDetails
        ApiServiceProxies.InvGpsStockReceivingTransactionServiceProxy,
        //Gps Master
        ApiServiceProxies.InvGpsPartListByCategoryServiceProxy,


        // InvGpsKanban
        ApiServiceProxies.InvGpsKanbanServiceProxy,

        ApiServiceProxies.InvGpsReceivingServiceProxy,

        //
        ApiServiceProxies.InvGpsStockConceptServiceProxy,

        ApiServiceProxies.InvGpsMaterialServiceProxy,

        // InvGpsStock
        ApiServiceProxies.InvGpsStockServiceProxy,

        // InvGpsUser
        ApiServiceProxies.InvGpsUserServiceProxy,

        // InvPIOStockRundown
        ApiServiceProxies.InvPIOStockRundownServiceProxy,

        // InvPIOStockRundownTransaction
        ApiServiceProxies.InvPIOStockRundownTransactionServiceProxy,
        //
        ApiServiceProxies.InvTmssDispatchPlanServiceProxy,

        // InvPIOStock
        ApiServiceProxies.InvPIOStockServiceProxy,

        // InvPIOStockIssuing
        ApiServiceProxies.InvPIOStockIssuingServiceProxy,

        // InvPIOStockTransaction
        ApiServiceProxies.InvPIOStockTransactionServiceProxy,
        // INVPIOPartList
        ApiServiceProxies.InvPioPartListServiceProxy,

      // INVPIOPartList
        ApiServiceProxies.InvPioPartListInlServiceProxy,

        // Inv Pio Part List Offline
        ApiServiceProxies.InvPartListOffServiceProxy,

        // Inv Topsse Invoice
        ApiServiceProxies.InvTopsseInvoiceServiceProxy,

        // InvPIOStockReceiving
        ApiServiceProxies.InvPIOStockReceivingServiceProxy,
        // LSP SUPPLIER INFO
        ApiServiceProxies.MstLspSupplierInforServiceProxy,
        //Inventory CPS
        ApiServiceProxies.InvCpsInvoiceHeadersServiceProxy,

        //Inventory CPS Sap Asset Masster
        ApiServiceProxies.InvCpsSapAssetMasterServiceProxy,


        //Inventory CPS
        ApiServiceProxies.InvCpsInventoryItemPriceServiceProxy,

        //Inventory CPS PoHeader
        ApiServiceProxies.InvCpsPoHeadersServiceProxy,

        //Inventory Cps shipped header
        ApiServiceProxies.InvCpsRcvShipmentHeadersServiceProxy,

        //Inventory CKD ProductionPlanMonthly
        ApiServiceProxies.InvCkdPartManagementServiceProxy,

        //Inventory PIO ProductionPlanMonthly
        ApiServiceProxies.InvPioProductionPlanMonthlyServiceProxy,
        //
        ApiServiceProxies.InvDrmStockPartServiceProxy,

        ApiServiceProxies.InvDrmStockPartExcelServiceProxy,

        //
        ApiServiceProxies.InvDrmStockRundownServiceProxy,

        //
        ApiServiceProxies.InvCkdSmqdOrderServiceProxy,

        //
        ApiServiceProxies.InvCkdSmqdOrderLeadTimeServiceProxy,

        //
        ApiServiceProxies.InvCkdPhysicalStockPartS4ServiceProxy,

        ApiServiceProxies.InvCkdSmqdServiceProxy,

        //Direct Material
        ApiServiceProxies.InvDrmPartListServiceProxy,

        //In House Part List
        ApiServiceProxies.InvIhpPartListServiceProxy,

        //In House Part List
        ApiServiceProxies.InvDrmStockPartServiceProxy,

        //matcustomdeclare
        ApiServiceProxies.InvIhpMatCustomDeclareServiceProxy,

        //stock part
        ApiServiceProxies.InvIhpStockPartServiceProxy,
        // Import Plan
        ApiServiceProxies.InvDrmImportPlanServiceProxy,

        // Local Plan
        ApiServiceProxies.InvDrmLocalPlanServiceProxy,


        // Inv OutLine
        ApiServiceProxies.InvOutLineOffServiceProxy,

        // Inv ImportByCont
        ApiServiceProxies.InvImportByContServiceProxy,
        // Inv InDetails
        ApiServiceProxies.InvInDetailsServiceProxy,
        //InvOutWipStock
        ApiServiceProxies.InvOutWipStockServiceProxy,

        //InvProductionMapping
        ApiServiceProxies.InvProductionMappingServiceProxy,

        //LogA Bar Scaninfo
        ApiServiceProxies.LgaBarScanInfoServiceProxy,

        //LogA Lds LotPlan
        ApiServiceProxies.LgaLdsLotPlanServiceProxy,

        //LogA Bp2 Progress
        ApiServiceProxies.LgaBp2ProgressServiceProxy,

        //Lga Bp2 ProgressScreen
        ApiServiceProxies.LgaBp2ProgressScreenServiceProxy,

        //LogA Bp2 PxPUpPlan
        ApiServiceProxies.LgaBp2PxPUpPlanServiceProxy,

        // Lga Bp2 BigPartDDL
        ApiServiceProxies.LgaBp2BigPartDDLServiceProxy,

        // Lga Bp2 BigPartDSM
        ApiServiceProxies.LgaBp2BigPartDSMServiceProxy,

        // Lga Ekb Progress
        ApiServiceProxies.LgaEkbProgressServiceProxy,

        // Lga Ekb ProgressDetail
        ApiServiceProxies.LgaEkbProgressDetailsServiceProxy,
        ApiServiceProxies.LgaEkbProgressScreenServiceProxy,

        // Lga Ekb Ekanban Progress
        ApiServiceProxies.LgaEkbEkanbanProgressScreenServiceProxy,

        // Lga Ekb Ekanban
        ApiServiceProxies.LgaEkbEkanbanServiceProxy,

        //LogA Lds Lot Direct Supply Andon
        ApiServiceProxies.LotDirectSupplyAndonServiceProxy,

        //Lga Pcs Stock
        ApiServiceProxies.LgaPcsStockServiceProxy,

        //Lga Sps Stock
        ApiServiceProxies.LgaSpsStockServiceProxy,

        //Lga Plc Signal
        ApiServiceProxies.LgaPlcSignalServiceProxy,

        // log mwh robbinglane
        ApiServiceProxies.LgwMwhRobbingLaneServiceProxy,

        // LogW MWh CaseData
        ApiServiceProxies.LgwMwhCaseDataServiceProxy,

        // LogW MWh contlist
        ApiServiceProxies.LgwMwhContListServiceProxy,
        // LogW MWh partdata
        ApiServiceProxies.LgwMwhPartDataServiceProxy,
        // LogW MWH modulecallling
        ApiServiceProxies.LgwMwhModuleCallingServiceProxy,


        //LogW Andon WH
        ApiServiceProxies.LgwMwhPxpModuleInputAndonServiceProxy,

        //LogW Andon At WH
        ApiServiceProxies.StockServiceProxy,

        //LogW Dvn ConsList
        ApiServiceProxies.LgwDvnContListServiceProxy,

        // logw Lup lotUpPlan
        ApiServiceProxies.LgwLupLotUpPlanServiceProxy,

        //LogW Lup ContModule
        ApiServiceProxies.LgwLupContModuleServiceProxy,

        //LogW Lup MakingTablet
        ApiServiceProxies.LotMakingTabletServiceProxy,

        //LogW PuP PxpUpPlan
        ApiServiceProxies.LgwPupPxPUpPlanServiceProxy,
        //LogW PuP PxpUpPlanbase
        ApiServiceProxies.LgwPupPxPUpPlanBaseServiceProxy,

        //LogW Andon LotUpacking
        ApiServiceProxies.LgwLupLotUnPackingAndonServiceProxy,

        //LogW Pik PickingProgress
        ApiServiceProxies.LgwPikPickingProgressServiceProxy,
        //LogW Pik PickingMoniteringScreen
        ApiServiceProxies.PickingMonitoringScreenServiceProxy,
        //LogW Ado CallingLightStatus
        ApiServiceProxies.LgwAdoCallingLightStatusServiceProxy,

        //Frame Andon FramePlan
        ApiServiceProxies.FrmAdoFramePlanServiceProxy,
        //Frame Andon FramePlanA1
        ApiServiceProxies.FrmAdoFramePlanA1ServiceProxy,
        //Frame Andon FramePlanBMPV
        ApiServiceProxies.FrmAdoFramePlanBMPVServiceProxy,
        //Frame Andon FrameProgress
        ApiServiceProxies.FrmAdoFrameProgressServiceProxy,

        // Painting Andon ScanInfo
        ApiServiceProxies.PtsAdoScanInfoServiceProxy,
        // Painting Andon PaintingProgress
        ApiServiceProxies.PtsAdoPaintingProgressServiceProxy,
        // Painting Andon PaintingData
        ApiServiceProxies.PtsAdoPaintingDataServiceProxy,
        // Painting Andon LineEfficiency
        ApiServiceProxies.PtsAdoLineEfficiencyServiceProxy,
        // Painting Andon LineEfficiencyDetails
        ApiServiceProxies.PtsAdoLineEfficiencyDetailsServiceProxy,
        // Painting Andon LineRealTimeControl
        ApiServiceProxies.PtsAdoLineRealTimeControlServiceProxy,
        // Painting Andon TotalDelay
        ApiServiceProxies.PtsAdoTotalDelayServiceProxy,
        // Painting Andon Bumper
        ApiServiceProxies.PtsAdoBumperServiceProxy,

        // Painting Andon Delaycontrollscreen
        ApiServiceProxies.PtsAdoDelayControlScreenServiceProxy,
        // Painting Andon NextEdIn
        ApiServiceProxies.PtsAdoWeldingServiceProxy,

        ApiServiceProxies.PtsAdoCCRMonitorServiceProxy,

        //Plan CCr Productionplan
        ApiServiceProxies.PlnCcrProductionPlanServiceProxy,

        // Welding Andon WeldingPlan
        ApiServiceProxies.WldAdoWeldingPlanServiceProxy,
        // Welding Andon Weldingprogress
        ApiServiceProxies.WldAdoWeldingProgressServiceProxy,
        // Welding Andon PunchQueue
        ApiServiceProxies.WldAdoPunchQueueServiceProxy,
        // Welding Andon WeldingSignal
        ApiServiceProxies.WldAdoWeldingSignalServiceProxy,
        // Welding Andon WldPunchQueueIndicator
        ApiServiceProxies.WldAdoPunchQueueIndicatorServiceProxy,

        ApiServiceProxies.WldAdoProcessInstructionServiceProxy,

        //interface
        ApiServiceProxies.IF_FQF3MM04ServiceProxy,

        ApiServiceProxies.IF_FQF3MM_LV2ServiceProxy,

        ApiServiceProxies.IF_FQF3MM05ServiceProxy,

        ApiServiceProxies.IF_FQF3MM01ServiceProxy,

        ApiServiceProxies.IF_FQF3MM02ServiceProxy,

        ApiServiceProxies.IF_FQF3MM06ServiceProxy,

        ApiServiceProxies.IF_FQF3MM03ServiceProxy,

        ApiServiceProxies.IF_FQF3MM07ServiceProxy,
        // spp
        ApiServiceProxies.InvSppCostOfSaleSummaryServiceProxy,

        ApiServiceProxies.InvSppCostServiceProxy,

        ApiServiceProxies.InvSppInvoiceDetailsServiceProxy,

        ApiServiceProxies.InvSppShippingServiceProxy,

        ApiServiceProxies.InvSppStockServiceProxy,
        ApiServiceProxies.InvGpsMappingServiceProxy,
        //Ckd Stock Rundown Schedule
        ApiServiceProxies.InvCkdRundownShippingResultServiceProxy,
        //Ckd Stock Rundown Schedule
        ApiServiceProxies.InvCkdRundownShippingScheduleServiceProxy,
        //Ckd Stock Rundown Werehouse
        ApiServiceProxies.InvCkdRundownWarehouseServiceProxy,

        { provide: RefreshTokenService, useClass: ZeroRefreshTokenService },
        { provide: AbpHttpConfigurationService, useClass: ZeroTemplateHttpConfigurationService },
        { provide: HTTP_INTERCEPTORS, useClass: AbpHttpInterceptor, multi: true },
    ],
})
export class ServiceProxyModule { }
