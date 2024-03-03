using Abp.IdentityServer4vNext;
using Abp.Zero.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using prod.Assy.Andon;
using prod.Asy.Plm;
using prod.Authorization.Delegation;
using prod.Authorization.Roles;
using prod.Authorization.Users;
using prod.Chat;
using prod.Common;
using prod.Editions;
using prod.Frame.Andon;
using prod.Friendships;
using prod.Inv.CKD;
using prod.Inv.D125;
using prod.Inv.Dmr;
using prod.Inv.Proc;
using prod.Inventory.BOM;
using prod.Inventory.CKD;
using prod.Inventory.CPS;
using prod.Inventory.DRM;
using prod.Inventory.GPS;
using prod.Inventory.IF;
using prod.Inventory.IHP;
using prod.Inventory.PIO;
using prod.Inventory.SPP;
using prod.Inventory.Tmss;
using prod.LogA.Bar;
using prod.LogA.Bp2;
using prod.LogA.Ekb;
using prod.LogA.Lds;
using prod.LogA.Pcs;
using prod.LogA.Plc;
using prod.LogA.Sps;
using prod.LogW.Ado;
using prod.LogW.Dvn;
using prod.LogW.Lup;
using prod.LogW.Mwh;
using prod.LogW.Pik;
using prod.LogW.Plc;
using prod.LogW.Pup;
using prod.Master.Assy;
using prod.Master.CKD;
using prod.Master.Cmm;
using prod.Master.Common;
using prod.Master.Frame;
using prod.Master.Inv;
using prod.Master.Inventory;
using prod.Master.LogA;
using prod.Master.LogW;
using prod.Master.Painting;
using prod.Master.Pio;
using prod.Master.Plm;
using prod.Master.Spp;
using prod.Master.Welding;
using prod.Master.WorkingPattern;
using prod.MultiTenancy;
using prod.MultiTenancy.Accounting;
using prod.MultiTenancy.Payments;
using prod.Painting.Andon;
using prod.Plan.Ccr;
using prod.SapIF;
using prod.Storage;
using prod.Welding.Andon;

namespace prod.EntityFrameworkCore
{
    public class prodDbContext : AbpZeroDbContext<Tenant, Role, User, prodDbContext>, IAbpPersistedGrantDbContext
    {
        /* Define an IDbSet for each entity of the application */

        public virtual DbSet<BinaryObject> BinaryObjects { get; set; }

        public virtual DbSet<Friendship> Friendships { get; set; }

        public virtual DbSet<ChatMessage> ChatMessages { get; set; }

        public virtual DbSet<SubscribableEdition> SubscribableEditions { get; set; }

        public virtual DbSet<SubscriptionPayment> SubscriptionPayments { get; set; }

        public virtual DbSet<Invoice> Invoices { get; set; }

        public virtual DbSet<PersistedGrantEntity> PersistedGrants { get; set; }

        public virtual DbSet<SubscriptionPaymentExtensionData> SubscriptionPaymentExtensionDatas { get; set; }

        public virtual DbSet<UserDelegation> UserDelegations { get; set; }

        public virtual DbSet<RecentPassword> RecentPasswords { get; set; }

        public virtual DbSet<CmmReportRequest> CmmReportRequests { get; set; }

        public virtual DbSet<MstCmmMaterialMaster> MstCmmMaterialMasters { get; set; }


        #region ASSEMBLY  
        // Vehicle Details
        public virtual DbSet<AsyAdoVehicleDetails> AsyAdoVehicleDetails { get; set; }

        // Total Delay All Shop
        public virtual DbSet<AsyAdoTotalDelayFinalAsakai> AsyAdoTotalDelayFinalAsakai { get; set; }

        // A In Plan - interface with andon to get data 
        public virtual DbSet<AsyAdoAInPlan> AsyAdoAInPlans { get; set; }

        // A Plan Shift Base - Sync from Server31
        public virtual DbSet<AsyAdoAPlanShift> AsyAdoAPlanShifts { get; set; }

        //Assembly
        public virtual DbSet<AsyAdoAssemblyData> AsyAdoAssemblyDatas { get; set; }

        public virtual DbSet<AsyPlmAssemblyData> AsyPlmAssemblyDatas { get; set; }

        public virtual DbSet<AsyPlmQrcodeAssembly> AsyPlmQrcodeAssemblys { get; set; }

        public virtual DbSet<AsyPlmAssemblyProcessBase> AsyPlmAssemblyProcessBases { get; set; }

        #endregion ASSEMBLY

        // MASTER 
        #region MASTER 

        #region MASTER >> ASSY
        public virtual DbSet<MstAsyProcess> MstAsyProcesss { get; set; }

        public virtual DbSet<MstAsySpsAssemblyScreenConfig> MstAsySpsAssemblyScreenConfigs { get; set; }


        #endregion

        #region MASTER >> FRAME

        // Frame Progcess
        public virtual DbSet<MstFrmProcess> MstFrmProcesses { get; set; }

        #endregion MASTER >> FRAME

        #region MASTER >> WORKING PATTERN
        // WORKING PATTERN
        // Calendar
        public virtual DbSet<MstWptCalendar> MstWptCalendars { get; set; }

        // Shop
        public virtual DbSet<MstWptShop> MstWptShops { get; set; }

        // Season Month
        public virtual DbSet<MstWptSeasonMonth> MstWptSeasonMonths { get; set; }

        // Working Type
        public virtual DbSet<MstWptWorkingType> MstWptWorkingTypes { get; set; }

        // Week
        public virtual DbSet<MstWptWeek> MstWptWeeks { get; set; }

        // PatternH
        public virtual DbSet<MstWptPatternH> MstWptPatternHs { get; set; }

        // PatternD
        public virtual DbSet<MstWptPatternD> MstWptPatternDs { get; set; }

        // Working Time
        public virtual DbSet<MstWptWorkingTime> MstWptWorkingTimes { get; set; }

        // Daily Working Time (main table)
        public virtual DbSet<MstWptDailyWorkingTime> MstWptDailyWorkingTimes { get; set; }

        #endregion MASTER >> WORKING PATTERN

        #region MASTER >> PAINTING
        // PAINTING
        // Painting Process
        public virtual DbSet<MstPtsPaintingProcess> MstPtsPaintingProcesses { get; set; }

        public virtual DbSet<MstPtsBmpPartList> MstPtsBmpPartLists { get; set; }

        public virtual DbSet<MstPtsBmpPartList_T> MstPtsBmpPartList_T { get; set; }

        public virtual DbSet<MstPtsBmpPartType> MstPtsBmpPartTypes { get; set; }

        public virtual DbSet<MstPtsInventoryStd> MstPtsInventoryStds { get; set; }

        #endregion MASTER >> PAINTING

        #region MASTER >> WELDING
        // WELDING 
        // Process 
        public virtual DbSet<MstWldProcess> MstWldProcesses { get; set; }

        // Punch Indicator
        public virtual DbSet<MstWldPunchIndicator> MstWldPunchIndicators { get; set; }

        #endregion MASTER >> WELDING

        #region MASTER >> LOGISTIC WELDING (LogW - LW)
        // LOGISTIC WELDING (LogW - LW) 
        // Eci Part 
        public virtual DbSet<MstLgwEciPart> MstLgwEciParts { get; set; }

        public virtual DbSet<MstLgwEciPart_T> MstLgwEciPart_T { get; set; }

        // Eci Part Module
        public virtual DbSet<MstLgwEciPartModule> MstLgwEciPartModules { get; set; }

        // Container Devanning Leadtime
        public virtual DbSet<MstLgwContDevanningLT> MstLgwContDevanningLTs { get; set; }

        // Master LogW LOT UP/MK Model Grade
        public virtual DbSet<MstLgwModelGrade> MstLgwModelGrades { get; set; }

        // Master LogW LOT UP/MK Model Grade_T
        public virtual DbSet<MstLgwModelGrade_T> MstLgwModelGrades_T { get; set; }

        // Renban Module
        public virtual DbSet<MstLgwRenbanModule> MstLgwRenbanModules { get; set; }

        // Renban Module_T
        public virtual DbSet<MstLgwRenbanModule_T> MstLgwRenbanModule_T { get; set; }

        // Layout 
        public virtual DbSet<MstLgwLayout> MstLgwLayouts { get; set; }

        // Layout Setup 
        public virtual DbSet<MstLgwLayoutSetup> MstLgwLayoutSetups { get; set; }

        // Robbing Lane
        public virtual DbSet<MstLgwRobbingLane> MstLgwRobbingLanes { get; set; }

        // Screen Config to flexible Color Setting - mainly for PxP Unpacking
        public virtual DbSet<MstLgwScreenConfig> MstLgwScreenConfigs { get; set; }

        // Picking Tablet Processes
        public virtual DbSet<MstLgwPickingTabletProcess> MstLgwPickingTabletProcesss { get; set; }

        // Picing Tablet
        public virtual DbSet<MstLgwPickingTablet> MstLgwPickingTablets { get; set; }

        // Unpacking Part Master to Call Module for UP
        public virtual DbSet<MstLgwUnpackingPart> MstLgwUnpackingParts { get; set; }

        public virtual DbSet<MstLgwUnpackingPart_T> MstLgwUnpackingParts_T { get; set; }

        //MstLgwContDevanning
        public virtual DbSet<MstLgwContDevanningLT_T> MstLgwContDevanningLT_T { get; set; }


        // PLC Signal Master Setup
        public virtual DbSet<MstLgwPlcSignal> MstLgwPlcSignals { get; set; }

        public virtual DbSet<MstLgwAdoCallingLight> MstLgwAdoCallingLights { get; set; }

        #endregion MASTER >> LOGISTIC WELDING (LogW - LW)

        #region MASTER >> LOGISTIC ASSEMBLY (LogA - LA)
        // PLC Signal Master Setup
        public virtual DbSet<MstLgaPlcSignal> MstLgaPlcSignals { get; set; }

        // Lot Direct Supply Trip Master
        public virtual DbSet<MstLgaLdsTrip> MstLgaLdsTrips { get; set; }

        // Mst Lga Bp2 Ecar
        public virtual DbSet<MstLgaBp2Ecar> MstLgaBp2Ecars { get; set; }

        public virtual DbSet<MstLgaBp2Ecar_T> MstLgaBp2Ecars_T { get; set; }

        // Mst Lga Bp2 PartList
        public virtual DbSet<MstLgaBp2PartList> MstLgaBp2PartLists { get; set; }

        public virtual DbSet<MstLgaBp2PartList_T> MstLgaBp2PartLists_T { get; set; }

        // Mst Lga Bp2 PartListGrade
        public virtual DbSet<MstLgaBp2PartListGrade> MstLgaBp2PartListGrades { get; set; }

        public virtual DbSet<MstLgaBp2PartListGrade_T> MstLgaBp2PartListGrades_T { get; set; }

        // Mst Lga Bp2 Process
        public virtual DbSet<MstLgaBp2Process> MstLgaBp2Processs { get; set; }

        // Mst Lga Module UpTable
        public virtual DbSet<MstLgaModuleUpTable> MstLgaModuleUpTables { get; set; }


        public virtual DbSet<MstLgaBp2Process_T> MstLgaBp2Processs_T { get; set; }

        //22/12/2022
        // Mst Lga BarUsers
        public virtual DbSet<MstLgaBarUser> MstLgaBarUsers { get; set; }

        //Mst Lga BarProcesss
        public virtual DbSet<MstLgaBarProcess> MstLgaBarProcesss { get; set; }

        // Mst Lga EkbPartLists
        public virtual DbSet<MstLgaEkbPartList> MstLgaEkbPartLists { get; set; }

        //Mst Lga EkbPartListGrades
        public virtual DbSet<MstLgaEkbPartListGrade> MstLgaEkbPartListGrades { get; set; }

        //Mst Lga EkbPartListGrade_T
        public virtual DbSet<MstLgaEkbPartListGrade_T> MstLgaEkbPartListGrades_T { get; set; }

        // Ekanban Process
        public virtual DbSet<MstLgaEkbProcess> MstLgaEkbProcesss { get; set; }

        // 09/03/2023
        //Mst Lga PcRack
        public virtual DbSet<MstLgaPcRack> MstLgaPcRacks { get; set; }

        //Mst Lga SpsRacks
        public virtual DbSet<MstLgaSpsRack> MstLgaSpsRacks { get; set; }

        //Mst Lga EkbUser
        public virtual DbSet<MstLgaEkbUser> MstLgaEkbUsers { get; set; }

        #endregion MASTER >> LOGISTIC ASSEMBLY (LogA - LA)

        #region MASTER >> COMMON
        // COMMON
        // Lookup
        public virtual DbSet<MstCmmLookup> MstCmmLookups { get; set; }

        // Takt Time
        public virtual DbSet<MstCmmTaktTime> MstCmmTaktTimes { get; set; }

        // Model
        public virtual DbSet<MstCmmModel> MstCmmModels { get; set; }


        // Lot Code Grade
        public virtual DbSet<MstCmmLotCodeGrade> MstCmmLotCodeGrades { get; set; }

        // Devanning Case Type
        public virtual DbSet<MstCmmDevanningCaseType> MstCmmDevanningCaseTypes { get; set; }

        // Shop Type Code
        public virtual DbSet<MstCmmShopType> MstCmmShopTypes { get; set; }

        // Grade Color
        public virtual DbSet<MstCmmGradeColor> MstCmmGradeColors { get; set; }

        // Color
        public virtual DbSet<MstCmmColor> MstCmmColors { get; set; }

        //Mst Cmm Uoms
        public virtual DbSet<MstCmmUom> MstCmmUoms { get; set; }

        //Mst Cmm Transmission Types
        public virtual DbSet<MstCmmTransmissionType> MstCmmTransmissionTypes { get; set; }

        //Mst Cmm Fuel Type
        public virtual DbSet<MstCmmFuelType> MstCmmFuelTypes { get; set; }

        //Mst Cmm Vehicle Color Types
        public virtual DbSet<MstCmmVehicleColorType> MstCmmVehicleColorTypes { get; set; }

        //Mst Cmm Vehicle Name
        public virtual DbSet<MstCmmVehicle> MstCmmVehicleNames { get; set; }


        // Storage Location
        public virtual DbSet<MstCmmStorageLocation> MstCmmStorageLocations { get; set; }

        //StorageLocationCategory
        public virtual DbSet<MstCmmStorageLocationCategory> MstCmmStorageLocationCategorys { get; set; }

        //MstCmmMaterialGroups
        public virtual DbSet<MstCmmMaterialGroup> MstCmmMaterialGroups { get; set; }

        //MstCmmProductTypes
        public virtual DbSet<MstCmmProductType> MstCmmProductTypes { get; set; }

        // MstCmmPlant
        public virtual DbSet<MstCmmPlant> MstCmmPlants { get; set; }

        //MstCmmBrand
        public virtual DbSet<MstCmmBrand> MstCmmBrands { get; set; }

        // Mst Cmm ExchangeRates
        public virtual DbSet<MstCmmExchangeRate> MstCmmExchangeRates { get; set; }

        // Mst Cmm ExchangeRates
        public virtual DbSet<MstCmmExchangeRate_T1> MstCmmExchangeRate_T1 { get; set; }

        // Mst Cmm ExchangeRates
        public virtual DbSet<MstCmmExchangeRate_T2> MstCmmExchangeRate_T2 { get; set; }

        //Mst Cmm MaterialType
        public virtual DbSet<MstCmmMaterialType> MstCmmMaterialTypes { get; set; }

        //Mst Cmm ValuationClass
        public virtual DbSet<MstCmmValuationClass> MstCmmValuationClasss { get; set; }

        //Mst Cmm ValuationType
        public virtual DbSet<MstCmmValuationType> MstCmmValuationTypes { get; set; }

        //Mst Cmm MMCheckingRule
        public virtual DbSet<MstCmmMMCheckingRule> MstCmmMMCheckingRules { get; set; }
        //Mst Cmm MMCheckingRule_T
        public virtual DbSet<MstCmmMMCheckingRule_T> MstCmmMMCheckingRule_Ts { get; set; }
        //Mst Cmm MMValidationResult
        public virtual DbSet<MstCmmMMValidationResult> MstCmmMMValidationResults { get; set; }
        //Mst Cmm EngineMaster
        public virtual DbSet<MstCmmEngineMaster> MstCmmEngineMasters { get; set; }
        //Mst Cmm BusinessParter
        public virtual DbSet<MstCmmBusinessParter> MstCmmBusinessParters { get; set; }


        //Mst Cmm Tax
        public virtual DbSet<MstCmmTax> MstCmmTaxs { get; set; }
        //Mst Cmm Engine Model
        public virtual DbSet<MstCmmEngineModel> MstCmmEngineModels { get; set; }
        //Mst Cmm Product Groups
        public virtual DbSet<MstCmmProductGroup> MstCmmProductGroups { get; set; }
       // Mst Cmm Engine Types
        public virtual DbSet<MstCmmEngineType> MstCmmEngineTypes { get; set; }
        //Mst Cmm Car Seriess
        public virtual DbSet<MstCmmCarSeries> MstCmmCarSeriess { get; set; }
        //Mst Cmm Car Seriess
        public virtual DbSet<MstCmmDriveTrain> MstCmmDriveTrains { get; set; }
        //Mst Cmm Car Seriess
        public virtual DbSet<MstCmmVehicle> MstCmmVehicles { get; set; }
        //Mst Cmm Carfamily
        public virtual DbSet<MstCmmCarfamily> MstCmmCarfamilys { get; set; }


        #endregion MASTER >> COMMON

        #region MASTER >> Plm
        public virtual DbSet<MstPlmLotCodeGrade> MstPlmLotCodeGrades { get; set; }

        public virtual DbSet<MasterPlmMatrix> MasterPlmMatrixs { get; set; }

        public virtual DbSet<MasterPlmScreen> MasterPlmScreens { get; set; }

        public virtual DbSet<MasterPlmMatrixLotCode> MasterPlmMatrixLotCodes { get; set; }

        public virtual DbSet<MasterPlmPart> MasterPlmParts { get; set; }



        #endregion

        #region MASTER INVENTORY

        public virtual DbSet<MstInvCkdRentalWarehouse> MstInvCkdRentalWarehouses { get; set; }
        public virtual DbSet<MstInvInvoiceStatus> MstInvInvoiceStatuss { get; set; }

        public virtual DbSet<MstInvContainerDeliveryType> MstInvContainerDeliveryTypes { get; set; }

        public virtual DbSet<MstInvCustomsPort> MstInvCustomsPorts { get; set; }

        public virtual DbSet<MstInvCustomsStatus> MstInvCustomsStatuss { get; set; }

        public virtual DbSet<MstInvDevanningCaseType> MstInvDevanningCaseTypes { get; set; }

        public virtual DbSet<MstInvForwarder> MstInvForwarders { get; set; }

        public virtual DbSet<MstInvShippingCompany> MstInvShippingCompanys { get; set; }

        public virtual DbSet<MstInvContainerStatus> MstInvContainerStatuss { get; set; }

        public virtual DbSet<MstInvHrTitles> MstInvHrTitless { get; set; }

        public virtual DbSet<MstInvHrPosition> MstInvHrPositions { get; set; }

        public virtual DbSet<MstInvHrOrgStructure> MstInvHrOrgStructures { get; set; }

        public virtual DbSet<MstInvGpsCalendar> MstInvGpsCalendars { get; set; }


        public virtual DbSet<MstInvGpsScreenSetting> MstInvGpsScreenSettings { get; set; }

        public virtual DbSet<MstInvGpsSupplierInfo> MstInvGpsSupplierInfos { get; set; }

        public virtual DbSet<MstInvGpsTmvPic> MstInvGpsTmvPics { get; set; }

        public virtual DbSet<MstInvGpsTruckSupplier> MstInvGpsTruckSuppliers { get; set; }

        public virtual DbSet<MstInvGpsSupplierPic> MstInvGpsSupplierPics { get; set; }

        public virtual DbSet<MstInvGpsSupplierOrderTime> MstInvGpsSupplierOrderTimes { get; set; }

        public virtual DbSet<MstInvGpsCategory> MstInvGpsCategorys { get; set; }
        public virtual DbSet<MstInvGpsMaterialCategory> MstInvGpsMaterialCategorys { get; set; }

		public virtual DbSet<MstGpsMaterialCategoryMapping> MstGpsMaterialCategoryMappings { get; set; }

		public virtual DbSet<MstInvCpsInventoryGroup> MstInvCpsInventoryGroups { get; set; }

        public virtual DbSet<MstInvCpsSuppliers> MstInvCpsSupplierss { get; set; }

        public virtual DbSet<MstInvHrGlCodeCombination> MstInvHrGlCodeCombinations { get; set; }

        public virtual DbSet<MstInvGenBOMDatas> MstInvGenBOMDatass { get; set; }

        public virtual DbSet<MstInvInvoiceDetailsStatus> MstInvInvoiceDetailsStatuss { get; set; }

        public virtual DbSet<MstInvLotPart> MstInvLotParts { get; set; }

        public virtual DbSet<MstInvSupplierList> MstInvSupplierLists { get; set; }

        public virtual DbSet<MstCkdCustomsLeadtime> MstCkdCustomsLeadtimes { get; set; }

        public virtual DbSet<MstInvPIOEmail> MstInvPIOEmails { get; set; }

        public virtual DbSet<MstInvPIOPartType> MstInvPIOPartTypes { get; set; }
        public virtual DbSet<MstPioImpSupplier> MstPioImpSupplier { get; set; }

        public virtual DbSet<InvPioPartList> InvPioPartLists { get; set; }

        public virtual DbSet<InvPioGrGl_T> InvPioGrGl_T { get; set; }

        public virtual DbSet<MstLspSupplierInfor> MstLspSupplierInfors { get; set; }

        public virtual DbSet<MstInvCpsInventoryItems> MstInvCpsInventoryItemss { get; set; }
        public virtual DbSet<MstInvCustomsLeadTimeMaster> MstInvCustomsLeadTimes { get; set; }

        public virtual DbSet<MstInvDemDetFees> MstInvDemDetFeess { get; set; }
		public virtual DbSet<MstInvDemDetDays> MstInvDemDetDayss { get; set; }
		public virtual DbSet<InvTopsseInvoice> InvTopsseInvoices { get; set; }

        public virtual DbSet<InvTopsseInvoiceDetails> InvTopsseInvoiceDetailss { get; set; }


		#endregion

		#region MASTER SPP
		public virtual DbSet<MstSppCustomer> MstSppCustomers { get; set; }

        public virtual DbSet<MstSppGlAccount> MstSppGlAccounts { get; set; }


        #endregion

        #region MASTER GPS

        public virtual DbSet<MstGpsMaterialRegisterByShop> MstGpsMaterialRegisterByShops { get; set; }

        public virtual DbSet<MstGpsCostCenterStructure> MstGpsCostCenters { get; set; }
        public virtual DbSet<MstGpsUom> MstGpsUom { get; set; }
		public virtual DbSet<MstGpsWbsCCMapping> MstGpsWbsCCMapping { get; set; }

		#endregion

		#endregion MASTER 

		#region FRAME

		// Frame Progress
		public virtual DbSet<FrmAdoFrameProgress> FrmAdoFrameProgresses { get; set; }

        // Frame Plan
        public virtual DbSet<FrmAdoFramePlan> FrmAdoFramePlans { get; set; }

        public virtual DbSet<FrmAdoFramePlan_T> FrmAdoFramePlans_T { get; set; }

        // Frame Plan A1
        public virtual DbSet<FrmAdoFramePlanA1> FrmAdoFramePlanA1s { get; set; }

        public virtual DbSet<FrmAdoFramePlanA1_T> FrmAdoFramePlanA1_T { get; set; }

        // Frame Plan BMPV
        public virtual DbSet<FrmAdoFramePlanBMPV> FrmAdoFramePlanBMPVs { get; set; }

        public virtual DbSet<FrmAdoFramePlanBMPV_T> FrmAdoFramePlanBMPV_T { get; set; }

        #endregion FRAME >> ANDON 

        #region LOGISTIC WELDING

        // Robbing Lane
        public virtual DbSet<LgwMwhRobbingLane> LgwMwhRobbingLanes { get; set; }

        // Case Data
        public virtual DbSet<LgwMwhCaseData> LgwMwhCaseDatas { get; set; }

        // Part Data
        public virtual DbSet<LgwMwhPartData> LgwMwhPartDatas { get; set; }

        // Container List
        public virtual DbSet<LgwMwhContList> LgwMwhContLists { get; set; }

        // Container List for Devanning Schedule
        public virtual DbSet<LgwDvnContList> LgwDvnContLists { get; set; }

        // Lot Unpacking Plan
        public virtual DbSet<LgwLupLotUpPlan> LgwLupLotUpPlans { get; set; }
        //lot upplan_T
        public virtual DbSet<LgwLupLotUpPlan_T> LgwLupLotUpPlan_T { get; set; }

        // Container Module 
        public virtual DbSet<LgwLupContModule> LgwLupContModules { get; set; }

        // PxP Unpacking Plan
        public virtual DbSet<LgwPupPxPUpPlan> LgwPupPxPUpPlans { get; set; }

        public virtual DbSet<LgwPupPxPUpPlan_T> LgwPupPxPUpPlans_T { get; set; }

        // PxP Unpacking Plan Base - Production Volumn (Weld) summary by Shift - CCR Import
        public virtual DbSet<LgwPupPxPUpPlanBase> LgwPupPxPUpPlanBases { get; set; }

        public virtual DbSet<LgwPupPxPUpPlanBase_T> LgwPupPxPUpPlanBases_T { get; set; }

        // Tablet Picking Progress
        public virtual DbSet<LgwPikPickingProgress> LgwPikPickingProgresss { get; set; }

        // Picking Signal - Receive from Picking Tablet
        public virtual DbSet<LgwPikPickingSignal> LgwPikPickingSignals { get; set; }

        public virtual DbSet<LgwAdoCallingLightStatus> LgwAdoCallingLightStatuss { get; set; }

        #endregion LOGISTIC WELDING 

        #region LOGISTIC ASSEMBLY


        // Lot Direct Supply Plan
        public virtual DbSet<LgaLdsLotPlan> LgaLdsLotPlans { get; set; }
        // Lot Direct Supply Plan Temp
        public virtual DbSet<LgaLdsLotPlan_T> LgaLdsLotPlans_T { get; set; }

        public virtual DbSet<LgaPcsStock> LgaPcsStocks { get; set; }

        public virtual DbSet<LgaPcsStock_T> LgaPcsStocks_T { get; set; }

        public virtual DbSet<LgaSpsStock> LgaSpsStocks { get; set; }

        public virtual DbSet<LgaSpsStock_T> LgaSpsStocks_T { get; set; }

        // Lga Bp2 Progress
        public virtual DbSet<LgaBp2Progress> LgaBp2Progresss { get; set; }

        //Lga Bp2 ProgressDetails
        public virtual DbSet<LgaBp2ProgressDetails> LgaBp2ProgressDetailss { get; set; }

        // Lga Bp2 PxPUpPlan
        public virtual DbSet<LgaBp2PxPUpPlan> LgaBp2PxPUpPlans { get; set; }

        // Lga Bp2 PxpUpPlanBase
        public virtual DbSet<LgaBp2PxpUpPlanBase> LgaBp2PxpUpPlanBases { get; set; }

        //Lga Bar ScanInfos
        public virtual DbSet<LgaBarScanInfo> LgaBarScanInfos { get; set; }

        //Lga Ekb Progresss
        public virtual DbSet<LgaEkbProgress> LgaEkbProgresss { get; set; }

        //Lga Ekb ProgressDetailss
        public virtual DbSet<LgaEkbProgressDetails> LgaEkbProgressDetailss { get; set; }

        //Lga Ekb  Ekanban 
        public virtual DbSet<LgaEkbEkanban> LgaEkbEkanbans { get; set; }

        // Lgw Plc Signals
        public virtual DbSet<LgwPlcSignal> LgwPlcSignals { get; set; }

        // Lga Plc Signals
        public virtual DbSet<LgaPlcSignal> LgaPlcSignals { get; set; }


        #endregion LOGISTIC ASSEMBLY  

        #region INV 

        //Inv_Stock
        public virtual DbSet<InvStock> InvStocks { get; set; }

        //Inv_OutLineOffs
        public virtual DbSet<InvOutLineOff> InvOutLineOffs { get; set; }

        //Inv_OutWipStocks
        public virtual DbSet<InvOutWipStock> InvOutWipStocks { get; set; }

        //Inv_lnDetails
        public virtual DbSet<InvInDetails> InvInDetails { get; set; }

        //InvImportByCont
        public virtual DbSet<InvImportByCont> InvImportByConts { get; set; }

        //InvProductionMapping
        public virtual DbSet<InvProductionMapping> InvProductionMapping { get; set; }


        #endregion INV

        #region Inventory
        //INV IF
        public virtual DbSet<IF_FQF3MM04> IF_FQF3MM04s { get; set; }
        public virtual DbSet<IF_HEADER> IF_HEADERs { get; set; }
        public virtual DbSet<IF_HEADER_FWG> IF_HEADER_FWGs { get; set; }
        public virtual DbSet<IF_TRAILER> IF_TRAILERs { get; set; }
        public virtual DbSet<IF_FQF3MM_LV2> IF_FQF3MM_LV2s { get; set; }
        public virtual DbSet<IF_FQF3MM05> IF_FQF3MM05s { get; set; }
        public virtual DbSet<IF_FQF3MM01> IF_FQF3MM01s { get; set; }



        //Inv Ckd
        public virtual DbSet<InvCkdBill> InvCkdBills { get; set; }
        public virtual DbSet<InvCkdShipment> InvCkdShipments { get; set; }
        public virtual DbSet<InvCkdInvoice> InvCkdInvoices { get; set; }
        public virtual DbSet<InvCkdContainerDeliveryGateIn> InvCkdContainerDeliveryGateIns { get; set; }
        public virtual DbSet<InvCkdCustomsDeclare> InvCkdCustomsDeclares { get; set; }
        public virtual DbSet<InvCkdInvoiceDetails> InvCkdInvoiceDetailss { get; set; }
        public virtual DbSet<InvCkdInvoiceLot> InvCkdInvoiceLots { get; set; }
        public virtual DbSet<InvCkdPreCustoms> InvCkdPreCustomss { get; set; }
        public virtual DbSet<InvCkdContainerDeliverySche> InvCkdContainerDeliverySches { get; set; }
        public virtual DbSet<InvCkdContainerIntransit> InvCkdContainerIntransits { get; set; }
        public virtual DbSet<InvCkdContainerRentalWhRepack> InvCkdContainerRentalWhRepacks { get; set; }
        public virtual DbSet<InvCkdContainerInvoice> InvCkdContainerInvoices { get; set; }
        public virtual DbSet<InvCkdModuleCase> InvCkdModuleCases { get; set; }
        public virtual DbSet<InvCkdRequest> InvCkdRequests { get; set; }
        public virtual DbSet<InvCkdFrameEngine> InvCkdFrameEngines { get; set; }
        public virtual DbSet<InvCkdContainerRentalWHPlan> InvCkdContainerRentalWHPlans { get; set; }
        public virtual DbSet<InvCkdRequestContentByPxP> InvCkdRequestContentByPxPs { get; set; }
        public virtual DbSet<InvCkdRequestContentByLot> InvCkdRequestContentByLots { get; set; }
        public virtual DbSet<InvCkdPaymentRequest> InvCkdPaymentRequests { get; set; }
        public virtual DbSet<InvCkdPaymentDetails> InvCkdPaymentDetailss { get; set; }

        public virtual DbSet<InvCkdPartPackingDetails> InvCkdPartPackingDetailss { get; set; }
        public virtual DbSet<InvCkdPartList> InvCkdPartLists { get; set; }
        public virtual DbSet<InvCkdPartGrade> InvCkdPartGrades { get; set; }
        public virtual DbSet<InvCkdPart_T> InvCkdPart_T { get; set; }

        public virtual DbSet<InvCkdPartRobbing> InvCkdPartRobbings { get; set; }
        public virtual DbSet<InvCkdPartRobbingDetails> InvCkdPartRobbingDetailss { get; set; }
        public virtual DbSet<InvCkdPartRobbing_T> InvCkdPartRobbing_T { get; set; }


        public virtual DbSet<InvCkdStockReceiving> InvCkdStockReceivings { get; set; }
        public virtual DbSet<InvCkdStockIssuing> InvCkdStockIssuings { get; set; }
        public virtual DbSet<InvCkdStockPart> InvCkdStockParts { get; set; }
        public virtual DbSet<InvCkdContainerList> InvCkdContainerLists { get; set; }

        public virtual DbSet<InvCkdPhysicalStockTransaction> InvCkdPhysicalStockTransactions { get; set; }
        public virtual DbSet<InvCkdPhysicalStockPart> InvCkdPhysicalStockParts { get; set; }
        public virtual DbSet<InvCkdPhysicalStockPeriod> InvCkdPhysicalStockPeriods { get; set; }
        public virtual DbSet<InvCkdPhysicalStockPartPeriod> InvCkdPhysicalStockPartPeriods { get; set; }
        public virtual DbSet<InvCkdPhysicalStockPart_T> InvCkdPhysicalStockParts_T { get; set; }
        public virtual DbSet<InvCkdPhysicalStockLot_T> InvCkdPhysicalStockLots_T { get; set; }

        public virtual DbSet<InvCkdPhysicalConfirmLot> InvCkdPhysicalConfirmLots { get; set; }

        public virtual DbSet<InvCkdPhysicalConfirmLot_T> InvCkdPhysicalConfirmLots_T { get; set; }

        public virtual DbSet<InvCkdProductionPlanMonthly> InvCkdProductionPlanMonthlys { get; set; }

        public virtual DbSet<InvCkdProductionMapping> InvCkdProductionMappings { get; set; }

        public virtual DbSet<InvCkdProductionMapping_T> InvCkdProductionMappings_T { get; set; }

        public virtual DbSet<InvCkdProdPlanDaily> InvCkdProdPlanDailys { get; set; }
        public virtual DbSet<InvCkdPhysicalStockPartS4> InvCkdPhysicalStockPartS4s { get; set; }

        public virtual DbSet<InvCkdShippingScheduleDetailsFirm> InvCkdShippingScheduleDetailsFirms { get; set; }

        public virtual DbSet<InvCkdPhysicalStockPartS4_T> InvCkdPhysicalStockPartS4_Ts { get; set; }


		// Inv Gps
		public virtual DbSet<InvGpsContentList> InvGpsContentLists { get; set; }
        public virtual DbSet<InvGpsDailyOrder> InvGpsDailyOrders { get; set; }
        public virtual DbSet<InvGpsKanban> InvGpsKanbans { get; set; }
        public virtual DbSet<InvGpsStockConcept> InvGpsStockConcepts { get; set; }
        public virtual DbSet<InvGpsStockConcept_T> InvGpsStockConcepts_T { get; set; }
        public virtual DbSet<InvGpsPartList> InvGpsPartLists { get; set; }
        public virtual DbSet<InvGpsPartGrade> InvGpsPartGrades { get; set; }
        public virtual DbSet<InvGpsStockRundown> InvGpsStockRundowns { get; set; }
        public virtual DbSet<InvGpsStockRundownTransaction> InvGpsStockRundownTransactions { get; set; }
        public virtual DbSet<InvGpsStock> InvGpsStocks { get; set; }
        public virtual DbSet<InvGpsStockTransaction> InvGpsStockTransactions { get; set; }

        public virtual DbSet<InvGpsStockTransaction_T> InvGpsStockTransaction_Ts { get; set; }

        public virtual DbSet<InvGpsMaterial> InvGpsMaterials { get; set; }

        public virtual DbSet<InvGpsMaterial_T> InvGpsMaterial_Ts { get; set; }
        public virtual DbSet<InvGpsPartListByCategory> InvGpsPartListByCategory { get; set; }

        public virtual DbSet<InvGpsMapping> InvGpsMappings { get; set; }
        //
        public virtual DbSet<InvGpsIssuing_T> InvGpsIssuing_T { get; set; }
        public virtual DbSet<InvGpsIssuing> InvGpsIssuing { get; set; }
        public virtual DbSet<InvGpsReceiving_T> InvGpsReceiving_T { get; set; }
        public virtual DbSet<InvGpsReceiving> InvGpsReceiving { get; set; }
        public virtual DbSet<InvGpsUser> InvGpsUser { get; set; }

        public virtual DbSet<InvGpsFinStock> InvGpsFinStocks { get; set; }



        // Inv Cps 
        public virtual DbSet<InvCpsSapAssetMaster> InvCpsSapAssetMasters { get; set; }
        public virtual DbSet<InvCpsInvoiceHeaders> InvCpsInvoiceHeaderss { get; set; }
        public virtual DbSet<InvCpsInvoiceLines> InvCpsInvoiceLiness { get; set; }
        public virtual DbSet<InvCpsPoHeaders> InvCpsPoHeaderss { get; set; }
        public virtual DbSet<InvCpsPoLines> InvCpsPoLiness { get; set; }
        public virtual DbSet<InvCpsInventoryItemPrice> InvCpsInventoryItemPrice { get; set; }
        public virtual DbSet<InvCpsRcvShipmentHeaders> InvCpsRcvShipmentHeaderss { get; set; }
        public virtual DbSet<InvCpsRcvShipmentLines> InvCpsRcvShipmentLiness { get; set; }
        public virtual DbSet<InvCkdContainerTransitPortPlan> InvCkdContainerTransitPortPlans { get; set; }
        public virtual DbSet<InvCkdContainerTransitPortPlan_T> InvCkdContainerTransitPortPlans_T { get; set; }


        // Inv Bom
        public virtual DbSet<InvGenBOMData> InvGenBOMDatas { get; set; }

        // Drm
        public virtual DbSet<InvDrmPartList> InvDrmPartLists { get; set; }
        public virtual DbSet<InvDrmIhpPartList_T> InvDrmIhpPartList_Ts { get; set; }
        public virtual DbSet<InvDrmStockPart> InvDrmStockParts { get; set; }
        public virtual DbSet<InvDrmStockPartTransaction> InvDrmStockPartTransactions { get; set; }
        public virtual DbSet<InvDrmStockRundown> InvDrmStockRundowns { get; set; }
        public virtual DbSet<InvDrmStockRundownTransaction> InvDrmStockRundownTransactions { get; set; }
        public virtual DbSet<InvDrmImportPlan> InvDrmImportPlans { get; set; }

        public virtual DbSet<InvDrmImportPlan_T> InvDrmImportPlan_Ts { get; set; }
        public virtual DbSet<InvDrmStockPart_T> InvDrmStockPart_Ts { get; set; }

        public virtual DbSet<InvDrmLocalPlan> InvDrmLocalPlans { get; set; }

        

        // Ihp
        public virtual DbSet<InvIhpPartList> InvIhpPartLists { get; set; }
        public virtual DbSet<InvIhpPartGrade> InvIhpPartGrades { get; set; }
        public virtual DbSet<InvIhpStockPart> InvIhpStockParts { get; set; }


        public virtual DbSet<InvIphMatCustomDeclare> InvIphMatCustomDeclares { get; set; }

        public virtual DbSet<InvIphMatCustomDeclareDetails> InvIphMatCustomDeclareDetailss { get; set; }
        //If
        public virtual DbSet<IF_FQF3MM06> IF_FQF3MM06s { get; set; }

        public virtual DbSet<IF_FQF3MM03> IF_FQF3MM03s { get; set; }

        public virtual DbSet<InvCkdSmqd> InvCkdSmqds { get; set; }

        public virtual DbSet<InvCkdSmqd_T> InvCkdSmqd_Ts { get; set; }
        public virtual DbSet<InvCkdSmqdOrder> InvCkdSmqdOrders { get; set; }

        public virtual DbSet<InvCkdSmqdOrderLeadTime> InvCkdSmqdOrderLeadTimes { get; set; }

        public virtual DbSet<InvCkdSmqdOrderLeadTime_T> InvCkdSmqdOrderLeadTime_Ts { get; set; }

        public virtual DbSet<IF_FQF3MM02> IF_FQF3MM02s { get; set; }

        public virtual DbSet<IF_FQF3MM07> IF_FQF3MM07s { get; set; }

        //PIO
        public virtual DbSet<InvPIOStock> InvPIOStocks { get; set; }
        public virtual DbSet<InvPIOStockRundown> InvPIOStockRundowns { get; set; }
        public virtual DbSet<InvPIOStockRundownTransaction> InvPIOStockRundownTransactions { get; set; }
        public virtual DbSet<InvPIOStockTransaction> InvPIOStockTransactions { get; set; }

        public virtual DbSet<InvTmssDispatchPlan> InvTmssDispatchPlans { get; set; }

        public virtual DbSet<MstCmmVehicleCBU> MstCmmVehicleCBUs { get; set; }

        public virtual DbSet<MstCmmVehicleCBUColor> MstCmmVehicleCBUColors { get; set; }

        public virtual DbSet<InvPIOStockReceiving> InvPIOStockReceivings { get; set; }

        public virtual DbSet<InvPioPartListInl> InvPioPartListInls { get; set; }
        public virtual DbSet<InvPioPartGradeInl> InvPioPartGradeInls { get; set; }

        public virtual DbSet<InvPioPartListOff> InvPioPartListOffs { get; set; }
        public virtual DbSet<InvPioPartGradeOff> InvPioPartGradeOffs { get; set; }
        public virtual DbSet<InvPioProductionPlanMonthly> InvPioProductionPlanMonthlys { get; set; }

        // SPP
        public virtual DbSet<InvSppCostOfSaleSummary> InvSppCostOfSaleSummarys { get; set; }
        public virtual DbSet<InvSppCost> InvSppCosts { get; set; }
        public virtual DbSet<InvSppInvoiceDetails> InvSppInvoiceDetailss { get; set; }
        public virtual DbSet<InvSppShipping> InvSppShippings { get; set; }
        public virtual DbSet<InvSppStock> InvSppStocks { get; set; }
        #endregion

        #region PLANNING
        public virtual DbSet<PlnCcrProductionPlan> PlnCcrProductionPlans { get; set; }

        public virtual DbSet<PlnCcrProductionPlan_T> PlnCcrProductionPlans_T { get; set; }
        #endregion PLANNING

        #region PAINTING
        //Painting Shop Tracking
        //public virtual DbSet<PtPaintingShopTracking> PtShopTracking { get; set; }

        //SCAN INFO
        public virtual DbSet<PtsAdoScanInfo> PtsAdoScanInfos { get; set; }

        //Painting Data
        public virtual DbSet<PtsAdoPaintingData> PtsAdoPaintingDatas { get; set; }

        // Painting Progresss
        public virtual DbSet<PtsAdoPaintingProgress> PtsAdoPaintingProgresss { get; set; }

        //Painting adon BumperGetdataClrIndicator
        public virtual DbSet<PtsAdoBumperGetdataClrIndicator> PtsAdoBumperGetdataClrIndicators { get; set; }

        // Line Efficiency (for All Shop - but located in Painting Module)
        public virtual DbSet<PtsAdoLineEfficiency> PtsAdoLineEfficiency { get; set; }

        // Line Efficiency Details (for All Shop - but located in Painting Module)
        public virtual DbSet<PtsAdoLineEfficiencyDetails> PtsAdoLineEfficiencyDetails { get; set; }

        //Totaldelay
        public virtual DbSet<PtsAdoTotalDelay> PtsAdoTotalDelays { get; set; }

        // Bumper
        public virtual DbSet<PtsAdoBumper> PtsAdoBumpers { get; set; }

        // Pts Bmp Progresss
        public virtual DbSet<PtsBmpProgress> PtsBmpProgresss { get; set; }


        #endregion PAINTING >> ANDON

        #region WELDING
        // Welding Plan Temp
        public virtual DbSet<WldAdoWeldingPlan_T> WldAdoWeldingPlans_T { get; set; }

        // Welding Plan
        public virtual DbSet<WldAdoWeldingPlan> WldAdoWeldingPlans { get; set; }

        // WeldingProgress
        public virtual DbSet<WldAdoWeldingProgress> WldAdoWeldingProgresses { get; set; }

        // Punch Queue
        public virtual DbSet<WldAdoPunchQueue> WldAdoPunchQueues { get; set; }

        // Welding Signal
        public virtual DbSet<WldAdoWeldingSignal> WldAdoWeldingSignals { get; set; }

        // Welding ED IN 
        public virtual DbSet<WldAdoEdIn> WldAdoEdIns { get; set; }

        public virtual DbSet<WldAdoEdIn_T> WldAdoEdIns_T { get; set; }

        #endregion WELDING >> ANDON

        public virtual DbSet<SapIFLogging> SapIFLogging { get; set; }
        public virtual DbSet<SapIFLoggingResponseDetailsOnlineBudgetCheck> SapIFLoggingResponseDetailsOnlineBudgetCheck { get; set; }
        public virtual DbSet<SapIFLoggingResponseDetailsFundCommitment> SapIFLoggingResponseDetailsFundCommitment { get; set; }
        public virtual DbSet<SapIFFundCommitmentHeader> SapIFFundCommitmentHeader { get; set; }
        public virtual DbSet<SapIFFundCommitmentItem> SapIFFundCommitmentItem { get; set; }
        public virtual DbSet<SapIFFundCommitmentHeaderDM> SapIFFundCommitmentHeaderDM { get; set; }
        public virtual DbSet<SapIFFundCommitmentItemDM> SapIFFundCommitmentItemDM { get; set; }

        public virtual DbSet<InvCkdStockRundown> InvCkdStockRundowns { get; set; }

        public prodDbContext(DbContextOptions<prodDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BinaryObject>(b =>
            {
                b.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<ChatMessage>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId, e.ReadState });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.UserId, e.ReadState });
            });

            modelBuilder.Entity<Friendship>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId });
                b.HasIndex(e => new { e.TenantId, e.FriendUserId });
                b.HasIndex(e => new { e.FriendTenantId, e.UserId });
                b.HasIndex(e => new { e.FriendTenantId, e.FriendUserId });
            });

            modelBuilder.Entity<Tenant>(b =>
            {
                b.HasIndex(e => new { e.SubscriptionEndDateUtc });
                b.HasIndex(e => new { e.CreationTime });
            });

            modelBuilder.Entity<SubscriptionPayment>(b =>
            {
                b.HasIndex(e => new { e.Status, e.CreationTime });
                b.HasIndex(e => new { PaymentId = e.ExternalPaymentId, e.Gateway });
            });

            modelBuilder.Entity<SubscriptionPaymentExtensionData>(b =>
            {
                b.HasQueryFilter(m => !m.IsDeleted)
                    .HasIndex(e => new { e.SubscriptionPaymentId, e.Key, e.IsDeleted })
                    .IsUnique()
                    .HasFilter("[IsDeleted] = 0");
            });

            modelBuilder.Entity<UserDelegation>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.SourceUserId });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId });
            });

            modelBuilder.ConfigurePersistedGrantEntity();
        }
    }
}
