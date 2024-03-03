using Abp.IO.Extensions;
using Abp.UI;
using Abp.Web.Models;
using Microsoft.AspNetCore.Mvc;
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
using System.Linq;
using System.Threading.Tasks;
using prod.Inventory.PIO.PartListInl;

using prod.Inventory.PIO.PartList;
using prod.Inventory.PIO.PartListOff;
using prod.Inventory.Gps.FinStock;
using prod.Inventory.PIO;

namespace prod.Web.Controllers
{
    public class ProductControllerBase : prodControllerBase
    {
        private readonly IFrmAdoFramePlanA1ExcelDataReader _importFrame;
        private readonly IFrmAdoFramePlanExcelDataReader _importFrameplan;
        private readonly IPxPUpPlanExcelDataReader _importPxPUpPlan;
        private readonly IMstLgwUnpackingPartExcelDataReader _importUnpackingPart;
        private readonly ILgwLupLotUpPlanExcelDataReader _importLotUpPlan;
        private readonly IMstLgwModelGradeExcelDataReader _importMstLgwModelGrade;
        private readonly IMstLgwRenbanModuleExcelDataReader _importMstLgwRenbanModule;
        private readonly IMstLgwContDevanningLTExcelDataReader _importContDevanningLT;
        private readonly IMstLgwEciPartExcelDataReader _importEciPart;
        private readonly ILgaLdsLotPlanExcelDataReader _importLgaLdsLotPlan;
        private readonly IPlnCcrProductionPlanExcelDataReader _importPlnCcrProductionPlan;
        private readonly IFrmAdoFramePlanBMPVExcelDataReader _importFrameBMPV;
        private readonly IMstLgaBp2ProcessExcelDataReader _importMstLgaBp2Process;
        private readonly IMstLgaBp2PartListGradeExcelDataReader _importMstLgaBp2PartListGrade;
        private readonly IMstLgaBp2PartListExcelDataReader _importMstLgaBp2PartList;
        private readonly IWldAdoWeldingPlanExcelDataReader _importWldAdoWeldingPlan;
        private readonly IMstLgaEkbPartListGradeExcelDataReader _importMstLgaEkbPartListGrade;
        private readonly IMstPtsBmpPartListExcelDataReader _importMstPtsBmpPartList;
        private readonly IMstGpsMaterialRegisterByShopAppService _importMstGpsMaterialRegisterByShopExcel;
        private readonly IMstGpsMaterialCategoryMappingAppService _importMstGpsMaterialCategoryMappingExcel;
        private readonly IMstGpsCostCenterAppService _importMstGpsCostCenterExcel;
        private readonly IInvGpsPartListByCategoryAppService _importInvGpsMasterExcel;
        private readonly IInvCkdContainerRentalWHPlanAppService _impInvCkdContainerRentalWHPlan;
        private readonly IInvGpsStockConceptAppService _impInvGpsStockConcept;
        private readonly IInvCkdContainerTransitPortPlanAppService _impInvCkdContainerTransitPortPlan;
        private readonly IInvGpsPartListAppService _impInvGpsPartListFromExcel;
        private readonly IInvCkdPartListAppService _impInvCkdPartListExcel;
        private readonly IInvCkdPartRobbingAppService _impInvCkdPartRobbingExcel;
        private readonly IInvCkdPhysicalStockPartAppService _impPhysicalStockPartExcel;
        private readonly IInvCkdPhysicalConfirmLotAppService _impPhysicalConfirmLotExcel;
        private readonly IInvCkdProductionPlanMonthlyAppService _impProductionPlanMonthlyExcel;
        private readonly IInvCkdProductionMappingAppService _impProductionMappingExcel;
        private readonly IInvCkdPhysicalStockPartS4AppService _impPhysicalStockPartS4Excel;
        private readonly IInvDrmPartListAppService _impDrmPartListExcel;
        private readonly IInvDrmStockPartAppService _impDrmStockPartExcel;
        private readonly IInvDrmImportPlanAppService _impImportPlanExcel;
        private readonly IInvCkdSmqdOrderAppService _impSmqdOrderExcel;
        private readonly IInvCkdSmqdAppService _impInvCkdSmqdExcel;
        private readonly IInvCkdShippingScheduleAppService _impInvCkdShippingScheduleExcel;
        private readonly IMstCmmMMCheckingRuleAppService _impMstCmmCheckingRuleExcel;
        private readonly IInvCkdSmqdOrderLeadTimeAppService _impInvCkdSmqdOrderLeadTimeExcel;
        private readonly IInvDrmStockPartExcelAppService _impDrmStockPartExcelExcel;
        private readonly IInvGpsStockReceivingTransactionAppService _impGpsStockReceivingTransactionExcel;
        private readonly IInvGpsStockIssuingTransactionAppService _impGpsStockIssuingTransactionExcel;
        private readonly IInvGpsMaterialAppService _impGpsMaterialExcel;
        private readonly IInvGpsReceivingAppService _impGpsStockReceiving;
        private readonly IInvGpsIssuingAppService _impGpsStockIssuingExcel;
        private readonly IInvGpsIssuingsAppService _impGpsStockIssuingsExcel;
        private readonly IMstInvCustomsLeadTimeAppService _impMstInvCustomsLeadTimeExcel;
        private readonly IInvPioPartListAppService _iInvPioPartListAppService;
        private readonly IInvPioPartListInlAppService _iInvPioPartListInlAppService;
        private readonly IMstInvDemDetFeesAppService _impMstInvDemDetFeesFromExcel;
        private readonly IInvPartListOffAppService _iInvPartListOffAppService;
        private readonly IInvGpsFinStock _iInvGpsFinStockPartAppService;
        private readonly IInvPioProductionPlanMonthlyAppService _iInvPioProductionPlanMonthlyFromExcel;


        private readonly IMstInvDemDetDaysAppService _impMstInvDemDetDaysFromExcel;
		private readonly IMstGpsWbsCCMappingAppService _iMstGpsWbsCCMappingAppService;
		protected ProductControllerBase(
                            IFrmAdoFramePlanA1ExcelDataReader importFrame,
                            IFrmAdoFramePlanExcelDataReader importFrameplan,
                            IPxPUpPlanExcelDataReader importPxPUpPlan,
                            IMstLgwUnpackingPartExcelDataReader importUnpackingPart,
                            ILgwLupLotUpPlanExcelDataReader importLotUpPlan,
                            IMstLgwModelGradeExcelDataReader importMstLgwModelGrade,
                            IMstLgwRenbanModuleExcelDataReader importMstLgwRenbanModule,
                            IMstLgwContDevanningLTExcelDataReader importContDevanningLT,
                            IMstLgwEciPartExcelDataReader importEciPart,
                            ILgaLdsLotPlanExcelDataReader importLgaLdsLotPlan,
                            IPlnCcrProductionPlanExcelDataReader importPlnCcrProductionPlan,
                            IFrmAdoFramePlanBMPVExcelDataReader importFrameBMPV,
                            IMstLgaBp2ProcessExcelDataReader importMstLgaBp2Process,
                            IMstLgaBp2PartListGradeExcelDataReader importMstLgaBp2PartListGrade,
                            IMstLgaBp2PartListExcelDataReader importMstLgaBp2PartListDataReader,
                            IWldAdoWeldingPlanExcelDataReader importWldAdoWeldingPlan,
                            IMstLgaEkbPartListGradeExcelDataReader importMstLgaEkbPartListGrade,
                            IMstPtsBmpPartListExcelDataReader importMstPtsBmpPartList,
                            IMstGpsMaterialRegisterByShopAppService importMstGpsMaterialRegisterByShopExcel,
                            IMstGpsMaterialCategoryMappingAppService importMstGpsMaterialCategoryMappingExcel,
                            IMstGpsCostCenterAppService importMstGpsCostCenterExcel,
                            IInvGpsPartListByCategoryAppService importInvGpsMasterExcel,
                            IInvCkdContainerRentalWHPlanAppService impInvCkdContainerRentalWHPlan,
                            IInvGpsStockConceptAppService impInvGpsStockConcept,
                            IInvCkdContainerTransitPortPlanAppService impInvCkdContainerTransitPortPlan,
                            IInvGpsPartListAppService impInvGpsPartListFromExcel,
                            IInvCkdPartListAppService impInvCkdPartListExcel,
                            IInvCkdPartRobbingAppService impInvCkdPartRobbingExcel,
                            IInvCkdPhysicalStockPartAppService impPhysicalStockPartExcel,
                            IInvCkdPhysicalConfirmLotAppService impPhysicalConfirmLotExcel,
                            IInvCkdProductionPlanMonthlyAppService impProductionPlanMonthlyExcel,
                            IInvCkdProductionMappingAppService impProductionMappingExcel,
                            IInvCkdPhysicalStockPartS4AppService impPhysicalStockPartS4Excel,
                            IInvDrmPartListAppService impDrmPartListExcel,
                            IInvDrmStockPartAppService impDrmStockPartExcel,
                            IInvDrmImportPlanAppService impImportPlanExcel,
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
                            IInvPartListOffAppService iInvPartListOffAppService,
                            IInvGpsFinStock iInvGpsFinStockPartAppService,
							IMstGpsWbsCCMappingAppService iMstGpsWbsCCMappingAppService,
                            IInvPioProductionPlanMonthlyAppService iInvPioProductionPlanMonthlyFromExcel
            )
        {
            _importFrame = importFrame;
            _importFrameplan = importFrameplan;
            _importPxPUpPlan = importPxPUpPlan;
            _importUnpackingPart = importUnpackingPart;
            _importLotUpPlan = importLotUpPlan;
            _importMstLgwModelGrade = importMstLgwModelGrade;
            _importMstLgwRenbanModule = importMstLgwRenbanModule;
            _importContDevanningLT = importContDevanningLT;
            _importEciPart = importEciPart;
            _importLgaLdsLotPlan = importLgaLdsLotPlan;
            _importPlnCcrProductionPlan = importPlnCcrProductionPlan;
            _importFrameBMPV = importFrameBMPV;
            _importMstLgaBp2Process = importMstLgaBp2Process;
            _importMstLgaBp2PartListGrade = importMstLgaBp2PartListGrade;
            _importMstLgaBp2PartList = importMstLgaBp2PartListDataReader;
            _importWldAdoWeldingPlan = importWldAdoWeldingPlan;
            _importMstLgaEkbPartListGrade = importMstLgaEkbPartListGrade;
            _importMstPtsBmpPartList = importMstPtsBmpPartList;
            _importMstGpsMaterialRegisterByShopExcel = importMstGpsMaterialRegisterByShopExcel;
            _importMstGpsMaterialCategoryMappingExcel = importMstGpsMaterialCategoryMappingExcel;
            _importMstGpsCostCenterExcel = importMstGpsCostCenterExcel;
            _importInvGpsMasterExcel = importInvGpsMasterExcel;
            _impInvCkdContainerRentalWHPlan = impInvCkdContainerRentalWHPlan;
            _impInvGpsStockConcept = impInvGpsStockConcept;
            _impInvCkdContainerTransitPortPlan = impInvCkdContainerTransitPortPlan;
            _impInvGpsPartListFromExcel = impInvGpsPartListFromExcel;
            _impInvCkdPartListExcel = impInvCkdPartListExcel;
            _impInvCkdPartRobbingExcel = impInvCkdPartRobbingExcel;
            _impPhysicalStockPartExcel = impPhysicalStockPartExcel;
            _impPhysicalConfirmLotExcel = impPhysicalConfirmLotExcel;
            _impProductionPlanMonthlyExcel = impProductionPlanMonthlyExcel;
            _impProductionMappingExcel = impProductionMappingExcel;
            _impPhysicalStockPartS4Excel = impPhysicalStockPartS4Excel;
            _impDrmPartListExcel = impDrmPartListExcel;
            _impDrmStockPartExcel = impDrmStockPartExcel;
            _impImportPlanExcel = impImportPlanExcel;
            _impSmqdOrderExcel = impSmqdOrderExcel;
            _impInvCkdShippingScheduleExcel = impInvCkdShippingScheduleExcel;
            _impInvCkdSmqdExcel = impInvCkdSmqdExcel;
            _impMstCmmCheckingRuleExcel = impMstCmmCheckingRuleExcel;
            _impInvCkdSmqdOrderLeadTimeExcel = impInvCkdSmqdOrderLeadTimeExcel;
            _impDrmStockPartExcelExcel = impDrmStockPartExcelExcel;
            _impGpsStockReceivingTransactionExcel = impGpsStockReceivingTransactionExcel;
            _impGpsStockIssuingTransactionExcel = impGpsStockIssuingTransactionExcel;
            _impGpsMaterialExcel = impGpsMaterialExcel;
            _impGpsStockReceiving = impGpsStockReceiving;
            _impGpsStockIssuingExcel = impGpsStockIssuingExcel;
            _impGpsStockIssuingsExcel = impGpsStockIssuingsExcel;
            _impMstInvCustomsLeadTimeExcel = impMstInvCustomsLeadTimeExcel;
            _iInvPioPartListAppService = iInvPioPartListAppService;
            _iInvPioPartListInlAppService = iInvPioPartListInlAppService;
            _impMstInvDemDetDaysFromExcel = impMstInvDemDetDaysFromExcel;
            _impMstInvDemDetFeesFromExcel = impMstInvDemDetFeesFromExcel;
            _iInvPartListOffAppService = iInvPartListOffAppService;
            _iInvGpsFinStockPartAppService = iInvGpsFinStockPartAppService;
            _iMstGpsWbsCCMappingAppService = iMstGpsWbsCCMappingAppService;
            _iInvPioProductionPlanMonthlyFromExcel = iInvPioProductionPlanMonthlyFromExcel;
        }
        [HttpPost]
        public async Task<JsonResult> ImportGpsStockReceivingTransactionFromExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var gpsStockReceiving = await _impGpsStockReceivingTransactionExcel.ImportInvGpsStockReceivingTransFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { gpsStockReceiving }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [HttpPost]
        public async Task<JsonResult> ImportGpsStockIssuingTransactionFromExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var gpsIssuingTrans = await _impGpsStockIssuingTransactionExcel.ImportInvGpsStockReceivingTransFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { gpsIssuingTrans }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }
        [HttpPost]
        public async Task<JsonResult> ImportMstInvCustomsLeadTimeFromExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var partExc = await _impMstInvCustomsLeadTimeExcel.ImportCustomsLeadTimeMasterFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { partExc }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [HttpPost]
        public async Task<JsonResult> ImportDrmStockPartExcelFromExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var partExc = await _impDrmStockPartExcelExcel.ImportInvDRMStockPartExcelFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { partExc }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [HttpPost]
        public async Task<JsonResult> ImportInvShippingScheduleFromExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var smqdOrder = await _impInvCkdShippingScheduleExcel.ImportInvShippingScheduleFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { smqdOrder }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [HttpPost]
        public async Task<JsonResult> ImportSmqdOrderFromExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var smqdOrder = await _impSmqdOrderExcel.ImportSmqdOrderNormalFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { smqdOrder }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [HttpPost]
        public async Task<JsonResult> ImportSmqdOrderLeadTimeFromExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var smqdOrderLeadTime = await _impInvCkdSmqdOrderLeadTimeExcel.ImportSmqdOrderLeadTimeFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { smqdOrderLeadTime }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [HttpPost]
        public async Task<JsonResult> ImportFramePlanA1FromExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var framPlanA1 = await _importFrame.GetFramePlanA1FromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { framPlanA1 }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [HttpPost]
        public async Task<JsonResult> ImportFramePlanFromExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var framPlan = await _importFrameplan.GetFramePlanFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { framPlan }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [HttpPost]
        public async Task<JsonResult> ImportPxPUpPlanFromExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var framPlan = await _importPxPUpPlan.GetPxPUpPlanFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { framPlan }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [HttpPost]
        public async Task<JsonResult> ImportUnpackingPartFromExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var unpackingPart = await _importUnpackingPart.GetUnpackingPartFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { unpackingPart }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [HttpPost]
        public async Task<JsonResult> ImportLotUpPlanFromExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var unpackingPart = await _importLotUpPlan.GetLotUpPlanFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { unpackingPart }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [HttpPost]
        public async Task<JsonResult> ImportEciPartFromExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var eicPart = await _importEciPart.GetEciPartFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { eicPart }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [HttpPost]
        public async Task<JsonResult> ImportContDevanningLTFromExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var unpackingPart = await _importContDevanningLT.GetContDevanningLTFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { unpackingPart }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [HttpPost]
        public async Task<JsonResult> ImportMstLgwModelGradeFromExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var unpackingPart = await _importMstLgwModelGrade.GetMstLgwModelgradeFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { unpackingPart }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [HttpPost]
        public async Task<JsonResult> ImportMstLgwRenbanModuleFromExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var unpackingPart = await _importMstLgwRenbanModule.GetMstLgwRenbanModuleFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { unpackingPart }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [HttpPost]
        public async Task<JsonResult> ImportLgaLdsLotPlanFromExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var unpackingPart = await _importLgaLdsLotPlan.GetLgaLdsLotPlanFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { unpackingPart }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [HttpPost]
        public async Task<JsonResult> ImportPlnCcrProductionPlanFromExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var plnCcrProductionPlan = await _importPlnCcrProductionPlan.GetPlnCcrProductionPlanFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { plnCcrProductionPlan }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        public async Task<JsonResult> ImportFramePlanBMPVFromExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var framPlanBMPV = await _importFrameBMPV.GetFramePlanBMPVFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { framPlanBMPV }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [HttpPost]
        public async Task<JsonResult> ImportMstLgaBp2ProcessFromExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var MstLgaBp2Process = await _importMstLgaBp2Process.GetMstLgaBp2ProcessFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { MstLgaBp2Process }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [HttpPost]
        public async Task<JsonResult> ImportMstBmpProcessFromExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var MstPtsBmpPartList = await _importMstPtsBmpPartList.GetBmpPartListFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { MstPtsBmpPartList }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [HttpPost]
        public async Task<JsonResult> ImportMaterialRegisterByShopFromExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var MstPtsBmpPartList = await _importMstGpsMaterialRegisterByShopExcel.ImportMaterialRegisterByShopFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { MstPtsBmpPartList }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }
        [HttpPost]
        public async Task<JsonResult> ImportMaterialCategoryMappingFromExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var MstGpsMasterialCaMapping = await _importMstGpsMaterialCategoryMappingExcel.ImportMaterialCategoryMappingFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { MstGpsMasterialCaMapping }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [HttpPost]
        public async Task<JsonResult> ImportGpsCostCenterFromExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var MstPtsBmpPartList = _importMstGpsCostCenterExcel.ImportGpsCostCenterFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { MstPtsBmpPartList }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [HttpPost]
        public async Task<JsonResult> ImportInvGpsPartListByCategoryFromExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var MstPtsBmpPartList = _importInvGpsMasterExcel.ImportInvGpsPartListByCategoryFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { MstPtsBmpPartList }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [HttpPost]
        public async Task<JsonResult> ImportBp2PartListFromExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var MstLgaBp2PartList = await _importMstLgaBp2PartList.GetBp2PartListFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { MstLgaBp2PartList }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        public async Task<JsonResult> ImportMstLgaBp2PartListGradeFromExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var partListGrade = await _importMstLgaBp2PartListGrade.GetMstLgaBp2PartListGradeFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { partListGrade }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        //

        [HttpPost]
        public async Task<JsonResult> ImportWldAdoWeldingPlanFromExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var weldingPlan = await _importWldAdoWeldingPlan.GetWldAdoWeldingPlanFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { weldingPlan }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }


        public async Task<JsonResult> ImportMstLgaEkbPartListGradeFromExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var partListGrade = await _importMstLgaEkbPartListGrade.GetMstLgaEkbPartListGradeFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { partListGrade }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [HttpPost]
        public async Task<JsonResult> ImportInvCkdContainerRentalWHPlanFromExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var containerRenta = await _impInvCkdContainerRentalWHPlan.GetImportDataFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { containerRenta }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [HttpPost]
        public async Task<JsonResult> ImportInvCkdContainerRepackTransferFromExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var repack = await _impInvCkdContainerRentalWHPlan.ImportRepackTransferFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { repack }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [HttpPost]
        public async Task<JsonResult> ImportInvGpsStockConceptFromExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var stockConcept = await _impInvGpsStockConcept.ImportData_InvGpsStockConcept_FromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { stockConcept }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [HttpPost]
        public async Task<JsonResult> ImportInvCkdContainerTransitPortPlanFromExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var containerRenta = await _impInvCkdContainerTransitPortPlan.GetImportDataFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { containerRenta }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }
        [HttpPost]
        public async Task<JsonResult> ImportMstInvDemDetFeesFromExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var gpsStockReceiving = await _impMstInvDemDetFeesFromExcel.ImportMstInvDemDetFeesFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { gpsStockReceiving }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [HttpPost]
        public async Task<JsonResult> ImportPartListFromExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var gentani = await _impInvGpsPartListFromExcel.GetInvGpsPartListFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { gentani }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }


        [HttpPost]
        public async Task<JsonResult> ImportPartListNoColorFromExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var partListNoColor = await _impInvGpsPartListFromExcel.GetInvGpsPartListNoColorFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { partListNoColor }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [HttpPost]
        public async Task<JsonResult> ImportCkdPartListExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var gentani = await _impInvCkdPartListExcel.ImportDataInvCkdPartListFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { gentani }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [HttpPost]
        public async Task<JsonResult> ImportCkdPartGradeExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var grade = await _impInvCkdPartListExcel.ImportDataInvCkdPartGradeFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { grade }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [HttpPost]
        public async Task<JsonResult> ImportCkdPartLotListExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var gentani = await _impInvCkdPartListExcel.ImportDataInvCkdPartListLotFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { gentani }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }
        
        //[HttpPost]
        //public async Task<JsonResult> ImportDataInvCkdPartGradeFromExcel()
        //{
        //    try
        //    {
        //        var file = Request.Form.Files.First();
        //        if (file == null)
        //        {
        //            throw new UserFriendlyException(L("File_Empty_Error"));
        //        }
        //        if (file.Length > 1048576 * 100) //100 MB
        //        {
        //            throw new UserFriendlyException(L("File_SizeLimit_Error"));
        //        }
        //        byte[] fileBytes;
        //        using (var stream = file.OpenReadStream())
        //        {
        //            fileBytes = stream.GetAllBytes();
        //        }
        //        var gentani = await _impInvCkdPartListExcel.ImportDataInvCkdPartGradeFromExcel(fileBytes, file.FileName);
        //        return Json(new AjaxResponse(new { gentani }));

        //    }
        //    catch (UserFriendlyException ex)
        //    {
        //        return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
        //    }
        //}

        [HttpPost]
        public async Task<JsonResult> ImportCkdPartRobbingExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var gentani = await _impInvCkdPartRobbingExcel.ImportPartRobbingFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { gentani }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [HttpPost]
        public async Task<JsonResult> ImportCkdPhysicalStockPartExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var stockpart = await _impPhysicalStockPartExcel.ImportDataInvCkdPhysicalStockPartFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { stockpart }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }
        [HttpPost]
        public async Task<JsonResult> ImportDrmImportPlanExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var plan = await _impImportPlanExcel.ImportDataInvDrmImportPlanFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { plan }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [HttpPost]
        public async Task<JsonResult> ImportCkdPhysicalStockLotExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var stocklot = await _impPhysicalStockPartExcel.ImportDataInvCkdPhysicalStockLotFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { stocklot }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }


        [HttpPost]
        public async Task<JsonResult> ImportPhysicalConfirmLotExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var confirmlot = await _impPhysicalConfirmLotExcel.ImportDataInvCkdPhysicalConfirmFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { confirmlot }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [HttpPost]
        public async Task<JsonResult> ImportProductionPlanMonthlyExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var planMonthly = await _impProductionPlanMonthlyExcel.ImportProductionPlanMonthlyFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { planMonthly }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [HttpPost]
        public async Task<JsonResult> ImportProductionMappinFromExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var prodMap = await _impProductionMappingExcel.ImportInvCkdProductionMappingFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { prodMap }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }


        [HttpPost]
        public async Task<JsonResult> ImportPhysicalStockPartS4FromExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var physSPs4 = await _impPhysicalStockPartS4Excel.ImportDataPhysicalStockCPartS4FromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { physSPs4 }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }



        [HttpPost]
        public async Task<JsonResult> ImportDrmIhpPartListFromExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var drm_ihp = await _impDrmPartListExcel.ImportInvDRMIHPPartFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { drm_ihp }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }


        [HttpPost]
        public async Task<JsonResult> ImportDrmStockPartFromExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var drmstockpart = await _impDrmStockPartExcel.ImportInvDRMStockPartFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { drmstockpart }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }


      /*  [HttpPost]
        public async Task<JsonResult> ImportInvCkdSmqdPxPFromExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                var importType = Request.Form["importType"];
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }

                switch (importType)
                {
                    case "PxP In":
                        var pxpIn = await _impInvCkdSmqdExcel.ImportPXPINFromExcel(fileBytes, file.FileName);
                        return Json(new AjaxResponse(new { pxpIn }));
                    case "PxP Out":
                        var pxpOut = await _impInvCkdSmqdExcel.ImportPXPReturnFromExcel(fileBytes, file.FileName);
                        return Json(new AjaxResponse(new { pxpOut }));
                    case "PxP Return":
                        var pxpReturn = await _impInvCkdSmqdExcel.ImportPXPReturnFromExcel(fileBytes, file.FileName);
                        return Json(new AjaxResponse(new { pxpReturn }));
                    case "In Other":
                        var pxpInOther = await _impInvCkdSmqdExcel.ImportPXPReturnFromExcel(fileBytes, file.FileName);
                        return Json(new AjaxResponse(new { pxpInOther }));
                    case "Out Other":
                        var pxpOutOther = await _impInvCkdSmqdExcel.ImportPXPReturnFromExcel(fileBytes, file.FileName);
                        return Json(new AjaxResponse(new { pxpOutOther }));
                    default:
                        break;
                }
                return null;
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }*/

        [HttpPost]
        public async Task<JsonResult> ImportInvCkdSmqdFromExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var smqd = await _impInvCkdSmqdExcel.ImportInvCkdSmqdFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { smqd }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }
        [HttpPost]
        public async Task<JsonResult> ImportMstCmmMMCheckingRuleFromExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var rule = await _impMstCmmCheckingRuleExcel.ImportMstCmmMMCheckingRuleFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { rule }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }
        [HttpPost]
        public async Task<JsonResult> ImportGpsMaterialFromExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var material = await _impGpsMaterialExcel.ImportInvGpsMaterialExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { material }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [HttpPost]

        public async Task<JsonResult> ImportGpsStockReceivingFromExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var gpsreceive = await _impGpsStockReceiving.ImportDataInvGpsReceiveFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { gpsreceive }));


            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [HttpPost]

        public async Task<JsonResult> ImportGpsIssuingFromExcel()

        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }

                var material = await _impGpsStockIssuingExcel.ImportDataInvGpsIssuingListFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { material }));


            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }


        [HttpPost]

        public async Task<JsonResult> ImportGpsIssuingsFromExcel()

        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }

                var material = _impGpsStockIssuingsExcel.ImportDataInvGpsIssuingsListFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { material }));


            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }


        //[HttpPost]

        //public async Task<JsonResult> ImportGpsIssuingGentaniFromExcel()

        //{
        //    try
        //    {
        //        var file = Request.Form.Files.First();
        //        if (file == null)
        //        {
        //            throw new UserFriendlyException(L("File_Empty_Error"));
        //        }
        //        if (file.Length > 1048576 * 100) //100 MB
        //        {
        //            throw new UserFriendlyException(L("File_SizeLimit_Error"));
        //        }
        //        byte[] fileBytes;
        //        using (var stream = file.OpenReadStream())
        //        {
        //            fileBytes = stream.GetAllBytes();
        //        }

        //        var material = _impGpsStockIssuingExcel.ImportDataInvGpsIssuingGentaniListFromExcel(fileBytes, file.FileName);
        //        return Json(new AjaxResponse(new { material }));


        //    }
        //    catch (UserFriendlyException ex)
        //    {
        //        return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
        //    }
        //}

        [HttpPost]
        public async Task<JsonResult> ImportPioPartListFromExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var gentani = await _iInvPioPartListAppService.ImportPartListFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { gentani }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [HttpPost]
        public async Task<JsonResult> ImportPioPartListExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var gentani = await _iInvPioPartListInlAppService.ImportDataInvPioPartListFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { gentani }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }
        [HttpPost]
        public async Task<JsonResult> ImportPioPartLotListExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var gentani = await _iInvPioPartListInlAppService.ImportDataInvPioPartListLotFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { gentani }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

		[HttpPost]
		public async Task<JsonResult> ImportMstInvDemDetDaysFromExcel()
		{
			try
			{
				var file = Request.Form.Files.First();
				if (file == null)
				{
					throw new UserFriendlyException(L("File_Empty_Error"));
				}
				if (file.Length > 1048576 * 100) //100 MB
				{
					throw new UserFriendlyException(L("File_SizeLimit_Error"));
				}
				byte[] fileBytes;
				using (var stream = file.OpenReadStream())
				{
					fileBytes = stream.GetAllBytes();
				}
				var gpsStockReceiving = await _impMstInvDemDetDaysFromExcel.ImportMstInvDemDetDaysFromExcel(fileBytes, file.FileName);
				return Json(new AjaxResponse(new { gpsStockReceiving }));

			}
			catch (UserFriendlyException ex)
			{
				return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
			}
		}


        [HttpPost]
        public async Task<JsonResult> ImportPioPartListOffExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var gentani = await _iInvPartListOffAppService.ImportDataInvPioPartListOffFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { gentani }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }



        [HttpPost]
        public async Task<JsonResult> ImportSmqdPxPOtherOutFromExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var smqd = await _impInvCkdSmqdExcel.ImportPXPOtherOutFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { smqd }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }


        }



        [HttpPost]
        public async Task<JsonResult> ImportSmqdPxPReturnFromExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var smqd = await _impInvCkdSmqdExcel.ImportPXPReturnExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { smqd }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }


        }




        [HttpPost]
        public async Task<JsonResult> ImportSmqdPxPInFromExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var smqd = await _impInvCkdSmqdExcel.ImportPXPInExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { smqd }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }


        }


        [HttpPost]
        public async Task<JsonResult> ImportCkdPartListNormalExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var normal = await _impInvCkdPartListExcel.ImportDataInvCkdPartListNormalFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { normal }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }
        // Gps Fin Stock

        [HttpPost]
        public async Task<JsonResult> ImportGpsFinStockExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var finstock = await _iInvGpsFinStockPartAppService.ImportDataInvGpsFinStockFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { finstock }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

		[HttpPost]
		public async Task<JsonResult> ImportMstGpsWbsCCMappingFromExcel()
		{
			try
			{
				var file = Request.Form.Files.First();
				if (file == null)
				{
					throw new UserFriendlyException(L("File_Empty_Error"));
				}
				if (file.Length > 1048576 * 100) //100 MB
				{
					throw new UserFriendlyException(L("File_SizeLimit_Error"));
				}
				byte[] fileBytes;
				using (var stream = file.OpenReadStream())
				{
					fileBytes = stream.GetAllBytes();
				}
				var gpsStockReceiving = await _iMstGpsWbsCCMappingAppService.ImportGpsWbsCCMappingFromExcel(fileBytes, file.FileName);
				return Json(new AjaxResponse(new { gpsStockReceiving }));

			}
			catch (UserFriendlyException ex)
			{
				return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
			}
		}
        [HttpPost]
        public async Task<JsonResult> ImportPioProductionPlanMonthlyExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var planMonthly = await _iInvPioProductionPlanMonthlyFromExcel.ImportPioProductionPlanMonthlyFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { planMonthly }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }
    }
}
