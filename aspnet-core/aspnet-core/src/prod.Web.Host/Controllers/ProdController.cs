using prod.Frame.Andon.FramePlanA1;
using prod.Frame.Andon.FramePlanBMPV;
using prod.Inv.CKD;
using prod.Inventory.CKD;
using prod.Inventory.CKD.PartRobbing;
using prod.Inventory.CKD.ShippingSchedule;
using prod.Inventory.CKD.SMQD;
using prod.Inventory.CKD.SmqdOrderLeadTime;
using prod.Inventory.DRM;
using prod.Inventory.DRM.StockPart;
using prod.Inventory.DRM.StockPartExcel;
using prod.Inventory.Gps.Issuings;
using prod.Inventory.Gps.PartListByCategory;
using prod.Inventory.Gps.PartList;
using prod.Inventory.Gps.StockIssuingTransDetails;
using prod.Inventory.Gps.StockReceivingTransDetails;
using prod.Inventory.GPS;
using prod.LogA.Lds.LotPlan;
using prod.LogW.Lup.LotUpPlan;
using prod.LogW.Pup;
using prod.Master.Cmm;
using prod.Master.Inventory;
using prod.Master.LogA;
using prod.Master.LogA.Bp2Process;
using prod.Master.LogW;
using prod.Master.LogW.Bp2PartList;
using prod.Master.LogW.ModelGrade;
using prod.Master.LogW.RenbanModule;
using prod.Master.LogW.UnPackingPart;
using prod.Master.Painting;
using prod.Plan.Ccr.ProductionPlan;
using prod.Welding.Andon;
using prod.Inventory.PIO.PartListInl;
using prod.Inventory.PIO.PartList;
using prod.Inventory.PIO.PartListOff;
using prod.Inventory.Gps.FinStock;
using prod.Inventory.PIO;

namespace prod.Web.Controllers
{
    public class ProdController : ProductControllerBase
    {
        public ProdController(
            IFrmAdoFramePlanA1ExcelDataReader importFramePlanA1ExcelDataReader,
            IFrmAdoFramePlanExcelDataReader importFramePlanExcelDataReader,
            IPxPUpPlanExcelDataReader importPxPUpPlanExcelDataReader,
            IMstLgwUnpackingPartExcelDataReader importUnpackingPartExcelDataReader,
            ILgwLupLotUpPlanExcelDataReader importLotupPlanDatareader,
            IMstLgwModelGradeExcelDataReader importMstLgwModelGradeDatareader,
            IMstLgwRenbanModuleExcelDataReader importMstLgwRenbanModuleExcelDataReader,
            IMstLgwContDevanningLTExcelDataReader imporContDevanningLTDatareader,
            IMstLgwEciPartExcelDataReader importEciPartExcelDataReader,
            ILgaLdsLotPlanExcelDataReader importLgaLdsLotPlanDataReader,
            IPlnCcrProductionPlanExcelDataReader importPlnCcrProductionPlanDataReader,
            IFrmAdoFramePlanBMPVExcelDataReader importFramePlanBMPVExcelDataReader,
            IMstLgaBp2ProcessExcelDataReader importMstLgaBp2ProcessDataReader,
            IMstLgaBp2PartListGradeExcelDataReader importMstLgaBp2PartListGradeExcelDataReader,
            IMstLgaBp2PartListExcelDataReader importMstLgaBp2PartListDataReader,
            IWldAdoWeldingPlanExcelDataReader importWldAdoWeldingPlanExcelDataReader,
            IMstLgaEkbPartListGradeExcelDataReader importMstLgaEkbPartListGradeExcelDataReader,
            IMstPtsBmpPartListExcelDataReader importMstPtsBmpPartListDataReader,
            IMstGpsMaterialRegisterByShopAppService importMstGpsMaterialRegisterByShopExcel,
            IMstGpsMaterialCategoryMappingAppService importMstGpsMaterialCategoryMappingExcel,
            IMstGpsCostCenterAppService importMstGpsCostCenterExcel,
            IInvGpsPartListByCategoryAppService importGpsMasterExcel,
            IInvCkdContainerRentalWHPlanAppService impInvCkdContainerRentalWHPlan,
            IInvGpsStockConceptAppService impInvGpsStockConcept,
            IInvCkdContainerTransitPortPlanAppService impGpsContainerTransitPortPlan,
            IInvGpsPartListAppService impInvGpsPartListFromExcel,
            IInvCkdPartListAppService impInvCkdPartList,
            IInvCkdPartRobbingAppService impInvCkdPartRobbing,
            IInvCkdPhysicalStockPartAppService impPhysicalStockPart,
            IInvCkdPhysicalConfirmLotAppService impPhysicalConfirmLotExcel,
            IInvCkdProductionPlanMonthlyAppService impProductionPlanMonthlyExcel,
            IInvCkdProductionMappingAppService impProductionMappingExcel,
            IInvCkdPhysicalStockPartS4AppService impPhysicalStockPartS4Excel,
            IInvDrmPartListAppService impDrmPartListExcel,
            IInvDrmStockPartAppService impDrmStockPartExcel,
            IInvDrmImportPlanAppService impDrmImportPlanExcel,
            IInvCkdSmqdOrderAppService impSmqdOrderExcel,
            IInvCkdSmqdAppService impInvCkdSmqdExcel,
            IInvCkdShippingScheduleAppService impInvCkdShippingScheduleExcel,
            IMstCmmMMCheckingRuleAppService impMstCmmCheckingRuleExcel,
            IInvCkdSmqdOrderLeadTimeAppService impInvCkdSmqdOrderLeadTimeExcel,
            IInvDrmStockPartExcelAppService impDrmStockPartExcelExcel,
            IInvGpsStockReceivingTransactionAppService impGpsStockReceivingTransactionExcel,
            IInvGpsStockIssuingTransactionAppService impGpsStockIssuingTransactionExcel,
            IInvGpsMaterialAppService impGpsMaterialExcel,
            IInvGpsReceivingAppService impGpsStockReceiving,
            IInvGpsIssuingAppService impGpsStockIssuingExcel,
            IInvGpsIssuingsAppService impGpsStockIssuingsExcel,
            IMstInvCustomsLeadTimeAppService impMstInvCustomsLeadTimeExcel,
            IInvPioPartListAppService iInvPioPartListAppService,
            IInvPioPartListInlAppService iInvPioPartListInlAppService,
            IMstInvDemDetFeesAppService impMstInvDemDetFeesFromExcel,
			IMstInvDemDetDaysAppService impMstInvDemDetDaysFromExcel,
            IInvPartListOffAppService  iInvPartListOffAppService,
            IInvGpsFinStock iInvGpsFinStockPartAppService,
			IMstGpsWbsCCMappingAppService iMstGpsWbsCCMappingAppService,
            IInvPioProductionPlanMonthlyAppService iInvPioProductionPlanMonthlyFromExcel

            ) : base(
                  importFramePlanA1ExcelDataReader,
                  importFramePlanExcelDataReader,
                  importPxPUpPlanExcelDataReader,
                  importUnpackingPartExcelDataReader,
                  importLotupPlanDatareader,
                  importMstLgwModelGradeDatareader,
                  importMstLgwRenbanModuleExcelDataReader,
                  imporContDevanningLTDatareader,
                  importEciPartExcelDataReader,
                  importLgaLdsLotPlanDataReader,
                  importPlnCcrProductionPlanDataReader,
                  importFramePlanBMPVExcelDataReader,
                  importMstLgaBp2ProcessDataReader,
                  importMstLgaBp2PartListGradeExcelDataReader,
                  importMstLgaBp2PartListDataReader,
                  importWldAdoWeldingPlanExcelDataReader,
                  importMstLgaEkbPartListGradeExcelDataReader,
                  importMstPtsBmpPartListDataReader,
                  importMstGpsMaterialRegisterByShopExcel,
                  importMstGpsMaterialCategoryMappingExcel,
                  importMstGpsCostCenterExcel,
                  importGpsMasterExcel,
                  impInvCkdContainerRentalWHPlan,
                  impInvGpsStockConcept,
                  impGpsContainerTransitPortPlan,
                  impInvGpsPartListFromExcel,
                  impInvCkdPartList,
                  impInvCkdPartRobbing,
                  impPhysicalStockPart,
                  impPhysicalConfirmLotExcel,
                  impProductionPlanMonthlyExcel,
                  impProductionMappingExcel,
                  impPhysicalStockPartS4Excel,
                  impDrmPartListExcel,
                  impDrmStockPartExcel,
                  impDrmImportPlanExcel,
                  impSmqdOrderExcel,
                  impInvCkdSmqdExcel,
                  impInvCkdShippingScheduleExcel,
                  impMstCmmCheckingRuleExcel,
                  impInvCkdSmqdOrderLeadTimeExcel,
                  impDrmStockPartExcelExcel,
                  impGpsStockReceivingTransactionExcel,
                  impGpsStockIssuingTransactionExcel,
                  impGpsMaterialExcel,
                  impGpsStockReceiving,
                  impGpsStockIssuingExcel,
                  impGpsStockIssuingsExcel,
                  impMstInvCustomsLeadTimeExcel,
                  iInvPioPartListAppService,
                  iInvPioPartListInlAppService,
                  impMstInvDemDetFeesFromExcel,
                  impMstInvDemDetDaysFromExcel,
                  iInvPartListOffAppService,
                  iInvGpsFinStockPartAppService,
				  iMstGpsWbsCCMappingAppService,
                  iInvPioProductionPlanMonthlyFromExcel
                  ) 
        { }
    }
}
