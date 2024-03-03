
using Abp.Application.Services.Dto;
using Abp.AspNetZeroCore.Net;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.UI;
using FastMember;
using GemBox.Spreadsheet;
using Microsoft.AspNetCore.Http;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using prod.Authorization;
using prod.Common;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using prod.Inventory.CKD.ShippingSchedule.Dto;
using prod.Inventory.CKD.SMQD.Dto;
using prod.Inventory.DRM.Dto;
using prod.Inventory.IHP.StockPart.Dto;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.CKD.ShippingSchedule
{
    [AbpAuthorize(AppPermissions.Pages_Ckd_Rundown_ShipingSchedule_View)]
    public class InvCkdShippingScheduleAppService : prodAppServiceBase, IInvCkdShippingScheduleAppService
    {
        private readonly IDapperRepository<InvCkdShipment, long> _dapperRepo;
        private readonly ITempFileCacheManager _tempFileCacheManager;



        public InvCkdShippingScheduleAppService(
                                         IDapperRepository<InvCkdShipment, long> dapperRepo,
                                            ITempFileCacheManager tempFileCacheManager
            )
        {
            
            _dapperRepo = dapperRepo;
              _tempFileCacheManager = tempFileCacheManager;
          
        }


        public async Task<PagedResultDto<InvCkdShippingScheduleDto>> GetAll(InvCkdShippingScheduleInput input)
        {

            string _sql = "Exec [INV_CKD_SHIPMENT_HEAD_SEARCH] @p_Seller, @p_Buyer, @p_PackingMonth, @p_ValuationTypeFrom, @p_PartNo, @p_ModuleNo, @p_Cfc, @p_RenbanNo," +
                "@p_PortOfLoading,@p_VesselEtd1st,@p_VesselName1st,@p_VesselNo1st,@p_PortOfDischarge,@p_EkanbanFlag, @p_RevisionNo,@p_ShipmentNo, @p_DetailStatus";

            IEnumerable<InvCkdShippingScheduleDto> result = await _dapperRepo.QueryAsync<InvCkdShippingScheduleDto>(_sql, new
            {   
                p_Seller = input.Seller,
                p_Buyer =  input.Buyer,
                p_PackingMonth = input.PackingMonth,
                p_ValuationTypeFrom = input.ValuationTypeFrom,
                p_PartNo = input.PartNo,
                p_ModuleNo = input.ModuleNo,
                p_Cfc = input.Cfc,
                p_RenbanNo = input.RenbanNo,
                p_PortOfLoading = input.PortOfLoading,
                p_VesselEtd1st = input.VesselEtd1st,
                p_VesselName1st = input.VesselName1st,
                p_VesselNo1st = input.VesselNo1st,
                p_PortOfDischarge = input.PortOfDischarge,
                p_EkanbanFlag = input.EkanbanFlag,
                p_RevisionNo = input.RevisionNo,
                p_ShipmentNo = input.ShipmentNo,
                p_DetailStatus = input.DetailStatus
            });
            var listResult = result.ToList();
            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            var totalCount = listResult.Count();

            return new PagedResultDto<InvCkdShippingScheduleDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<PagedResultDto<InvCkdShipmentDetailsDto>> GetAllDetails(InvCkdShipmentDetailsInput input)
        {
            List<InvCkdShipmentDetailsDto> totalResult = new();
            string _sql = "Exec [INV_CKD_SHIPMENT_DETAIL_SEARCH] @p_ShipmentHeaderId, @p_PartNo, @p_Cfc, @p_status";

            if (input.ShipmentHeaderId != null)
            {
                foreach (var headId in input.ShipmentHeaderId)
                {
                    IEnumerable<InvCkdShipmentDetailsDto> result = await _dapperRepo.QueryAsync<InvCkdShipmentDetailsDto>(_sql, new
                    {
                        p_ShipmentHeaderId = headId,
                        p_PartNo = input.PartNo,
                        p_Cfc = input.Cfc,
                        p_status = input.Status
                    });
                    var listResult = result.ToList();
                    totalResult.AddRange(listResult);
                }
            } else
            {
                throw new UserFriendlyException(400, "Invalid input");
            }
          
           
            var pagedAndFiltered = totalResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            var totalCount = totalResult.Count();

            return new PagedResultDto<InvCkdShipmentDetailsDto>(
                totalCount,
                pagedAndFiltered);
        }


        //public async Task<FileDto> GetInvCkdShippingToExcel(GetInvCkdShippingExportInput input)
        //{
        //    string contentRootPath = "/Template/temp_InvCkdShippingPlanConvertToSS.xlsx";
        //    string webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot") + contentRootPath;
        //    string pathExcelTemp = webRootPath;
        //    string pathExcel = "/Download/";
        //    string nameExcel = "MSP_SSResult" + DateTime.Now.ToString("MMddyyyy-HHmmss") + ".xlsx";
        //    string pathDownload = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot") + pathExcel + nameExcel;
        //    var fileDto = new FileDto(nameExcel, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);


        //    FileInfo finfo = new FileInfo(pathDownload);
        //    if (finfo.Exists) { try { finfo.Delete(); } catch (Exception ex) { } }

        //    XSSFWorkbook xlsxObject = null;     //XLSX
        //    ISheet sheet = null;
        //    IRow row = null;

        //    ICell cell = null;
        //    CellRangeAddress range = null;


        //    CellReference cr = null;
        //    using (FileStream file = new FileStream(pathExcelTemp, FileMode.Open, FileAccess.Read))
        //    {
        //        xlsxObject = new XSSFWorkbook(file);
        //    }

        //    // Lấy Object Sheet  
        //    sheet = xlsxObject.GetSheetAt(0);
        //    if (sheet == null) { return null; }

        //    IFont boldFont = xlsxObject.CreateFont();
        //    boldFont.IsBold = true;

        //    ICellStyle istyle = xlsxObject.CreateCellStyle();
        //    istyle.FillPattern = FillPattern.SolidForeground;
        //    istyle.FillForegroundColor = IndexedColors.White.Index;
        //    istyle.BorderBottom = BorderStyle.Thin;
        //    istyle.BorderTop = BorderStyle.Thin;
        //    istyle.BorderLeft = BorderStyle.Thin;
        //    istyle.BorderRight = BorderStyle.Thin;


        //    try
        //    {

        //        int r = 11;

        //        string RequestDate = DateTime.Today.ToString("MM/yyyy");

        //        cr = new CellReference("K7");
        //        row = sheet.GetRow(cr.Row);
        //        cell = row.GetCell(cr.Col);

        //        cell.SetCellValue(RequestDate + " PACKING");

        //        string _sql = "Exec [INV_CKD_SHIPPING_EXPORT] @p_shippingmonthfrom, @p_shippingmonthto";

        //        IEnumerable<GetInvCkdShippingExport> Result = await _dapperRepo.QueryAsync<GetInvCkdShippingExport>(_sql, new
        //        {
        //            p_shippingmonthfrom = input.ShippingMonthFrom,
        //            p_shippingmonthto = input.ShippingMonthTo
        //        });
        //        var ListRecords = Result.ToList();

        //        if (Result.Count() > 0)
        //        {

        //            for (int i = 0; i < Result.Count(); i++)
        //            {
        //                row = sheet.CreateRow(r);
        //                // No
        //                cell = row.CreateCell(0);
        //                cell.CellStyle = istyle;
        //                cell.SetCellValue(i + 1);


        //                cell = row.CreateCell(3);
        //                cell.CellStyle = istyle;
        //                cell.SetCellValue(ListRecords[i].Exp);

        //                cell = row.CreateCell(4);
        //                cell.CellStyle = istyle;
        //                cell.SetCellValue(ListRecords[i].Etd);


        //                cell = row.CreateCell(5);
        //                cell.CellStyle = istyle;
        //                cell.SetCellValue(ListRecords[i].Eta);


        //                cell = row.CreateCell(6);
        //                cell.CellStyle = istyle;
        //                cell.SetCellValue(ListRecords[i].Etd2);


        //                cell = row.CreateCell(7);
        //                cell.CellStyle = istyle;
        //                cell.SetCellValue(ListRecords[i].Eta2);


        //                cell = row.CreateCell(8);
        //                cell.CellStyle = istyle;
        //                cell.SetCellValue(ListRecords[i].Etd3);


        //                cell = row.CreateCell(9);
        //                cell.CellStyle = istyle;
        //                cell.SetCellValue(ListRecords[i].Eta3);


        //                cell = row.CreateCell(10);
        //                cell.CellStyle = istyle;
        //                cell.SetCellValue(ListRecords[i].Etd4);


        //                cell = row.CreateCell(11);
        //                cell.CellStyle = istyle;
        //                cell.SetCellValue(ListRecords[i].Eta4);

        //                cell = row.CreateCell(12);
        //                cell.CellStyle = istyle;
        //                cell.SetCellValue(ListRecords[i].ShipmentNo);


        //                cell = row.CreateCell(13);
        //                cell.CellStyle = istyle;
        //                cell.SetCellValue(ListRecords[i].ModelCode);

        //                cell = row.CreateCell(14);
        //                cell.CellStyle = istyle;
        //                cell.SetCellValue(ListRecords[i].IdLine);


        //                cell = row.CreateCell(15);
        //                cell.CellStyle = istyle;
        //                cell.SetCellValue(ListRecords[i].PartNo);

        //                cell = row.CreateCell(16);
        //                cell.CellStyle = istyle;
        //                cell.SetCellValue(ListRecords[i].PartName);

        //                cell = row.CreateCell(17);
        //                cell.CellStyle = istyle;
        //                cell.SetCellValue(ListRecords[i].Qty);


        //                cell = row.CreateCell(18);
        //                cell.CellStyle = istyle;
        //                cell.SetCellValue(ListRecords[i].PackingMonth);

        //                r++;

        //            }

        //        }

        //        FileStream xfile = new FileStream(pathDownload, FileMode.Create, System.IO.FileAccess.Write);
        //        xlsxObject.Write(xfile);
        //        xfile.Dispose();


        //        MemoryStream downloadStream = new MemoryStream(File.ReadAllBytes(pathDownload));
        //        _tempFileCacheManager.SetFile(fileDto.FileToken, downloadStream.ToArray());
        //        File.Delete(pathDownload);
        //        downloadStream.Position = 0;

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return await Task.FromResult(fileDto);

        //}

        public async Task<FileDto> GetInvCkdShippingToExcelNew(GetInvCkdShippingExportInput input)
        {
            string contentRootPath = "/Template/temp_InvShippingScheduleMSP.xlsx";
            string webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot") + contentRootPath;
            string pathExcelTemp = webRootPath;
            string pathExcel = "/Download/";
            string nameExcel = "MSP_SSResult" + DateTime.Now.ToString("MMddyyyy-HHmmss") + ".xlsx";
            string pathDownload = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot") + pathExcel + nameExcel;
            var fileDto = new FileDto(nameExcel, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);


            FileInfo finfo = new FileInfo(pathDownload);
            if (finfo.Exists) { try { finfo.Delete(); } catch (Exception ex) { } }

            XSSFWorkbook xlsxObject = null;     //XLSX
            ISheet sheet = null;
            IRow row = null;

            ICell cell = null;
            CellRangeAddress range = null;


            CellReference cr = null;
            using (FileStream file = new FileStream(pathExcelTemp, FileMode.Open, FileAccess.Read))
            {
                xlsxObject = new XSSFWorkbook(file);
            }

            // Lấy Object Sheet  
            sheet = xlsxObject.GetSheetAt(0);
            if (sheet == null) { return null; }

            IFont boldFont = xlsxObject.CreateFont();
            boldFont.IsBold = true;

            ICellStyle istyle = xlsxObject.CreateCellStyle();
            istyle.FillPattern = FillPattern.SolidForeground;
            istyle.FillForegroundColor = IndexedColors.White.Index;
            istyle.BorderBottom = BorderStyle.Thin;
            istyle.BorderTop = BorderStyle.Thin;
            istyle.BorderLeft = BorderStyle.Thin;
            istyle.BorderRight = BorderStyle.Thin;


            try
            {
                int r = 1;
                string _sql = "Exec [INV_CKD_SHIPPING_EXPORT] @p_packingmonth";

                IEnumerable<GetInvCkdShippingExportNew> Result = await _dapperRepo.QueryAsync<GetInvCkdShippingExportNew>(_sql, new
                {
                    p_packingmonth = input.PackingMonth,
                });
                var ListRecords = Result.ToList();

                if (Result.Count() > 0)
                {
                    var culture = new CultureInfo("en-US");
                    for (int i = 0; i < Result.Count(); i++)
                    {
                        row = sheet.CreateRow(r);

                        cell = row.CreateCell(0);
                        cell.CellStyle = istyle;
                        cell.SetCellValue(ListRecords[i].Exp);

                        cell = row.CreateCell(1);
                        cell.CellStyle = istyle;
                        cell.SetCellValue(ListRecords[i].Etd?.ToString("dd-MMM-yy", culture));


                        cell = row.CreateCell(2);
                        cell.CellStyle = istyle;
                        cell.SetCellValue(ListRecords[i].Eta?.ToString("dd-MMM-yy", culture));


                        cell = row.CreateCell(3);
                        cell.CellStyle = istyle;
                        DateTime shippingMonth = DateTime.ParseExact(ListRecords[i].ShippingMonth, "yyyyMM", culture);
                        cell.SetCellValue(ListRecords[i].ShipmentNo + shippingMonth.ToString("MMM", culture));


                        cell = row.CreateCell(4);
                        cell.CellStyle = istyle;
                        cell.SetCellValue(ListRecords[i].ModelCode);


                        cell = row.CreateCell(5);
                        cell.CellStyle = istyle;
                        cell.SetCellValue(ListRecords[i].IDLine);


                        cell = row.CreateCell(6);
                        cell.CellStyle = istyle;
                        cell.SetCellValue(ListRecords[i].PartNo);


                        cell = row.CreateCell(7);
                        cell.CellStyle = istyle;
                        cell.SetCellValue(ListRecords[i].PartName);


                        cell = row.CreateCell(8);
                        cell.CellStyle = istyle;
                        cell.SetCellValue(ListRecords[i].Qty);

                        cell = row.CreateCell(9);
                        cell.CellStyle = istyle;
                        DateTime packingMonth = DateTime.ParseExact(ListRecords[i].PackingMonth, "yyyyMM", culture);
                        cell.SetCellValue(packingMonth.ToString("MMM-yy", culture));


                        cell = row.CreateCell(10);
                        cell.CellStyle = istyle;
                        cell.SetCellValue(ListRecords[i].ETADelay?.ToString("dd-MMM-yy"));

                        cell = row.CreateCell(11);
                        cell.CellStyle = istyle;
                        cell.SetCellValue('R' + ListRecords[i].Version);


                        cell = row.CreateCell(12);
                        cell.CellStyle = istyle;
                        cell.SetCellValue(ListRecords[i].Remark);


                        r++;

                    }

                }

                FileStream xfile = new FileStream(pathDownload, FileMode.Create, System.IO.FileAccess.Write);
                xlsxObject.Write(xfile);
                xfile.Dispose();


                MemoryStream downloadStream = new MemoryStream(File.ReadAllBytes(pathDownload));
                _tempFileCacheManager.SetFile(fileDto.FileToken, downloadStream.ToArray());
                File.Delete(pathDownload);
                downloadStream.Position = 0;

            }
            catch (Exception ex)
            {

            }
            return await Task.FromResult(fileDto);

        }

        public async Task<FileDto> GetInvCkdShippingJSPToExcelNew(GetInvCkdShippingExportInput input)
        {
            string contentRootPath = "/Template/temp_InvShippingScheduleMSP.xlsx";
            string webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot") + contentRootPath;
            string pathExcelTemp = webRootPath;
            string pathExcel = "/Download/";
            string nameExcel = "JSP_SSResult" + DateTime.Now.ToString("MMddyyyy-HHmmss") + ".xlsx";
            string pathDownload = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot") + pathExcel + nameExcel;
            var fileDto = new FileDto(nameExcel, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);


            FileInfo finfo = new FileInfo(pathDownload);
            if (finfo.Exists) { try { finfo.Delete(); } catch (Exception ex) { } }

            XSSFWorkbook xlsxObject = null;     //XLSX
            ISheet sheet = null;
            IRow row = null;

            ICell cell = null;
            CellRangeAddress range = null;


            CellReference cr = null;
            using (FileStream file = new FileStream(pathExcelTemp, FileMode.Open, FileAccess.Read))
            {
                xlsxObject = new XSSFWorkbook(file);
            }

            // Lấy Object Sheet  
            sheet = xlsxObject.GetSheetAt(0);
            if (sheet == null) { return null; }

            IFont boldFont = xlsxObject.CreateFont();
            boldFont.IsBold = true;

            ICellStyle istyle = xlsxObject.CreateCellStyle();
            istyle.FillPattern = FillPattern.SolidForeground;
            istyle.FillForegroundColor = IndexedColors.White.Index;
            istyle.BorderBottom = BorderStyle.Thin;
            istyle.BorderTop = BorderStyle.Thin;
            istyle.BorderLeft = BorderStyle.Thin;
            istyle.BorderRight = BorderStyle.Thin;


            try
            {

                int r = 1;

                string _sql = "Exec [INV_SHIPPING_SCHEDULE_EXPORT_JSP] @p_packingmonth";

                IEnumerable<GetInvCkdShippingExportNew> Result = await _dapperRepo.QueryAsync<GetInvCkdShippingExportNew>(_sql, new
                {
                    p_packingmonth = input.PackingMonth,
                });
                var ListRecords = Result.ToList();

                var culture = new CultureInfo("en-US");

                if (Result.Count() > 0)
                {

                    for (int i = 0; i < Result.Count(); i++)
                    {
                        row = sheet.CreateRow(r);

                        cell = row.CreateCell(0);
                        cell.CellStyle = istyle;
                        cell.SetCellValue(ListRecords[i].Exp);

                        cell = row.CreateCell(1);
                        cell.CellStyle = istyle;
                        cell.SetCellValue(ListRecords[i].Etd?.ToString("dd-MMM-yy", culture));


                        cell = row.CreateCell(2);
                        cell.CellStyle = istyle;
                        cell.SetCellValue(ListRecords[i].Eta?.ToString("dd-MMM-yy", culture));


                        cell = row.CreateCell(3);
                        cell.CellStyle = istyle;
                        DateTime shippingMonth = DateTime.ParseExact(ListRecords[i].ShippingMonth, "yyyyMM", culture);
                        cell.SetCellValue(ListRecords[i].ShipmentNo + '-' + shippingMonth.ToString("MMM", culture));


                        cell = row.CreateCell(4);
                        cell.CellStyle = istyle;
                        cell.SetCellValue(ListRecords[i].ModelCode);


                        cell = row.CreateCell(5);
                        cell.CellStyle = istyle;
                        cell.SetCellValue(ListRecords[i].IDLine);


                        cell = row.CreateCell(6);
                        cell.CellStyle = istyle;
                        cell.SetCellValue(ListRecords[i].PartNo);


                        cell = row.CreateCell(7);
                        cell.CellStyle = istyle;
                        cell.SetCellValue(ListRecords[i].PartName);


                        cell = row.CreateCell(8);
                        cell.CellStyle = istyle;
                        cell.SetCellValue(ListRecords[i].Qty);

                        cell = row.CreateCell(9);
                        cell.CellStyle = istyle;
                        DateTime packingMonth = DateTime.ParseExact(ListRecords[i].PackingMonth, "yyyyMM", culture);
                        cell.SetCellValue(packingMonth.ToString("MMM-yy", culture));


                        cell = row.CreateCell(10);
                        cell.CellStyle = istyle;
                        cell.SetCellValue(ListRecords[i].ETADelay?.ToString("dd-MMM-yy", culture));

                        cell = row.CreateCell(11);
                        cell.CellStyle = istyle;
                        cell.SetCellValue('R' + ListRecords[i].Version);


                        cell = row.CreateCell(12);
                        cell.CellStyle = istyle;
                        cell.SetCellValue(ListRecords[i].Remark);


                        r++;

                    }

                }

                FileStream xfile = new FileStream(pathDownload, FileMode.Create, System.IO.FileAccess.Write);
                xlsxObject.Write(xfile);
                xfile.Dispose();


                MemoryStream downloadStream = new MemoryStream(File.ReadAllBytes(pathDownload));
                _tempFileCacheManager.SetFile(fileDto.FileToken, downloadStream.ToArray());
                File.Delete(pathDownload);
                downloadStream.Position = 0;

            }
            catch (Exception ex)
            {

            }
            return await Task.FromResult(fileDto);

        }
        //public async Task<FileDto> GetInvCkdShippingJSPToExcel(GetInvCkdShippingExportInput input)
        //{
        //    string contentRootPath = "/Template/temp_InvCkdShippingPlanConvertToSS_JSP.xlsx";
        //    string webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot") + contentRootPath;
        //    string pathExcelTemp = webRootPath;
        //    string pathExcel = "/Download/";
        //    string nameExcel = "JSP_SSResult" + DateTime.Now.ToString("MMddyyyy-HHmmss") + ".xlsx";
        //    string pathDownload = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot") + pathExcel + nameExcel;
        //    var fileDto = new FileDto(nameExcel, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);


        //    FileInfo finfo = new FileInfo(pathDownload);
        //    if (finfo.Exists) { try { finfo.Delete(); } catch (Exception ex) { } }

        //    XSSFWorkbook xlsxObject = null;     //XLSX
        //    ISheet sheet = null;
        //    IRow row = null;

        //    ICell cell = null;
        //    CellRangeAddress range = null;


        //    CellReference cr = null;
        //    using (FileStream file = new FileStream(pathExcelTemp, FileMode.Open, FileAccess.Read))
        //    {
        //        xlsxObject = new XSSFWorkbook(file);
        //    }

        //    // Lấy Object Sheet  
        //    sheet = xlsxObject.GetSheetAt(0);
        //    if (sheet == null) { return null; }

        //    IFont boldFont = xlsxObject.CreateFont();
        //    boldFont.IsBold = true;

        //    ICellStyle istyle = xlsxObject.CreateCellStyle();
        //    istyle.FillPattern = FillPattern.SolidForeground;
        //    istyle.FillForegroundColor = IndexedColors.White.Index;
        //    istyle.BorderBottom = BorderStyle.Thin;
        //    istyle.BorderTop = BorderStyle.Thin;
        //    istyle.BorderLeft = BorderStyle.Thin;
        //    istyle.BorderRight = BorderStyle.Thin;


        //    try
        //    {

        //        int r = 12;
        //        string RequestDate = DateTime.Now.ToString("M-yyyy");


        //        cr = new CellReference("H7");
        //        row = sheet.GetRow(cr.Row);
        //        cell = row.GetCell(cr.Col);

        //        cell.SetCellValue("JSP SHIPPING SCHEDULE: " + RequestDate + " PACKING");

        //        string _sql = "Exec [INV_SHIPPING_SCHEDULE_EXPORT_JSP] @p_shippingmonthfrom, @p_shippingmonthto";

        //        IEnumerable<GetInvCkdShippingExport> Result = await _dapperRepo.QueryAsync<GetInvCkdShippingExport>(_sql, new
        //        {
        //            p_shippingmonthfrom = input.ShippingMonthFrom,
        //            p_shippingmonthto = input.ShippingMonthTo

        //        });
        //        var ListRecords = Result.ToList();

        //        if (Result.Count() > 0)
        //        {

        //            for (int i = 0; i < Result.Count(); i++)
        //            {
        //                row = sheet.CreateRow(r);
        //                // No
        //                cell = row.CreateCell(1);
        //                cell.CellStyle = istyle;
        //                cell.SetCellValue(i + 1);

        //                cell = row.CreateCell(2);
        //                cell.CellStyle = istyle;
        //                cell.SetCellValue(ListRecords[i].ShipmentNo);

        //                cell = row.CreateCell(4);
        //                cell.CellStyle = istyle;
        //                cell.SetCellValue(ListRecords[i].Exp);

        //                cell = row.CreateCell(5);
        //                cell.CellStyle = istyle;
        //                cell.SetCellValue(ListRecords[i].Etd);


        //                cell = row.CreateCell(6);
        //                cell.CellStyle = istyle;
        //                cell.SetCellValue(ListRecords[i].Eta);


        //                cell = row.CreateCell(7);
        //                cell.CellStyle = istyle;
        //                cell.SetCellValue(ListRecords[i].ShipmentNo);


        //                cell = row.CreateCell(8);
        //                cell.CellStyle = istyle;
        //                cell.SetCellValue(ListRecords[i].ModelCode);

        //                cell = row.CreateCell(9);
        //                cell.CellStyle = istyle;
        //                cell.SetCellValue(ListRecords[i].IdLine);


        //                cell = row.CreateCell(10);
        //                cell.CellStyle = istyle;
        //                cell.SetCellValue(ListRecords[i].PartNo);

        //                cell = row.CreateCell(11);
        //                cell.CellStyle = istyle;
        //                cell.SetCellValue(ListRecords[i].PartName);

        //                cell = row.CreateCell(12);
        //                cell.CellStyle = istyle;
        //                cell.SetCellValue(ListRecords[i].Qty);


        //                cell = row.CreateCell(13);
        //                cell.CellStyle = istyle;
        //                cell.SetCellValue(ListRecords[i].PackingMonth);

        //                r++;

        //            }

        //        }

        //        FileStream xfile = new FileStream(pathDownload, FileMode.Create, System.IO.FileAccess.Write);
        //        xlsxObject.Write(xfile);
        //        xfile.Dispose();


        //        MemoryStream downloadStream = new MemoryStream(File.ReadAllBytes(pathDownload));
        //        _tempFileCacheManager.SetFile(fileDto.FileToken, downloadStream.ToArray());
        //        File.Delete(pathDownload);
        //        downloadStream.Position = 0;

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return await Task.FromResult(fileDto);

        //}


        [AbpAuthorize(AppPermissions.Pages_Ckd_Rundown_ShipingSchedule_Import)]
        public async Task<List<InvShippingScheduleImportDto>> ImportInvShippingScheduleFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<InvShippingScheduleImportDto> listImport = new List<InvShippingScheduleImportDto>();
                using (var stream = new MemoryStream(fileBytes))
                {
                    SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
                    var xlWorkBook = ExcelFile.Load(stream);
                    string strGUID = Guid.NewGuid().ToString("N");

                    ExcelWorksheet v_worksheet_p1 = null;
                    ExcelWorksheet v_worksheet_p2 = null;
                    
                    /// đọc các sheet dựa theo tên sheet
                    /// 
                    for (int i = 0; i < xlWorkBook.Worksheets.Count; i++)
                    {
                        if (xlWorkBook.Worksheets[i].Name == "BPPP")
                        {
                            v_worksheet_p1 = xlWorkBook.Worksheets[i];
                        }
                        if (xlWorkBook.Worksheets[i].Name == "JSP - SS")
                        {
                            v_worksheet_p2 = xlWorkBook.Worksheets[i];
                        }
                    }

                    /// nếu k đủ 2 sheet -> throw exception;
                    if (v_worksheet_p1 == null || v_worksheet_p2 == null)
                    {   
                        throw new UserFriendlyException(400, "Invalid template");
                    }

                   

                    int row, col;
                    /// xử lý trang 1 trong template -> cho dữ liệu vào bảng InvShippingSchedule_T1

                    /// lấy danh sách các ngày
                    List<string> days = new List<string>();
                    row = 0; // hàng đầu tiên
                    col = 7; // ngày đầu tiên bắt đầu từ cột index 7
                    while (true)
                    {
                        try
                        {
                            var day = v_worksheet_p1.Cells[row, col].Value;
                            if(day != null)
                            {
                                string day1 = v_worksheet_p1.Cells[row, col].Value.ToString();
                                days.Add(day1);
                                col++;
                            }
                            else {
                                break; 
                            }
                            

                        }
                        catch
                        {
                            break;
                        }
                    }

                    /// đọc từng dòng
                    /// bắt đầu từ dòng 2 -> index = 1
                    row = 1;
                    List<InvShippingScheduleSheet1ImportDto> listData = new List<InvShippingScheduleSheet1ImportDto>();
                    while (true)
                    {
                        try
                        {
                            // lấy các trường từ des -> totalbox. sau đó chạy for để lấy từng ngày.
                            var dest1 = v_worksheet_p1.Cells[row, 0].Value;
                            if(dest1 == null) { break; }
                            string dest = dest1.ToString();
                            string importer = v_worksheet_p1.Cells[row, 1].Value.ToString();
                            string model = v_worksheet_p1.Cells[row, 2].Value.ToString();
                            string partNo = v_worksheet_p1.Cells[row, 3].Value.ToString();
                            string mod = v_worksheet_p1.Cells[row, 4].Value.ToString();
                            int orderLot = Int32.Parse(v_worksheet_p1.Cells[row, 5].Value.ToString());
                            int totalBox = Int32.Parse(v_worksheet_p1.Cells[row, 6].Value.ToString());


                            for (int i = 7; i < days.Count + 7; i++) // bắt đầu ngày từ ô index 7
                            {

                                // invShippingScheduleSheet1ImportDto sẽ import vào bảng T1
                                InvShippingScheduleSheet1ImportDto invShippingScheduleSheet1ImportDto = new InvShippingScheduleSheet1ImportDto();
                                invShippingScheduleSheet1ImportDto.Dest = dest;
                                invShippingScheduleSheet1ImportDto.Importer = importer;
                                invShippingScheduleSheet1ImportDto.Model = model;
                                invShippingScheduleSheet1ImportDto.PartNo = partNo;
                                invShippingScheduleSheet1ImportDto.Mod = mod;
                                invShippingScheduleSheet1ImportDto.OrderLot = orderLot;
                                invShippingScheduleSheet1ImportDto.TotalBox = totalBox;
                                invShippingScheduleSheet1ImportDto.ValuationTypeFrom = "JSP";
                                // xử lý dữ liệu
                                int qty = 0;
                                try
                                {
                                    qty = Int32.Parse(v_worksheet_p1.Cells[row, i].Value.ToString());
                                }
                                catch { }

                                DateTime date;
                                DateTime.TryParseExact(v_worksheet_p1.Cells[0, i].Value.ToString(), "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out date);

                                invShippingScheduleSheet1ImportDto.Date = date;
                                invShippingScheduleSheet1ImportDto.BoxQty = qty;
                                invShippingScheduleSheet1ImportDto.Guid = strGUID;

                                listData.Add(invShippingScheduleSheet1ImportDto);
                            }
                        }
                        catch
                        {
                            break;
                        }
                        row++;
                    }



                    // import temp into db (bulkCopy)
                    if (listData.Count > 0)
                    {
                        IEnumerable<InvShippingScheduleSheet1ImportDto> dataE = listData.AsEnumerable();
                        DataTable table = new DataTable();
                        using (var reader = ObjectReader.Create(dataE))
                        {
                            table.Load(reader);
                        }
                        string connectionString = Commons.getConnectionString();
                        using (Microsoft.Data.SqlClient.SqlConnection conn = new Microsoft.Data.SqlClient.SqlConnection(connectionString))
                        {
                            await conn.OpenAsync();

                            using (Microsoft.Data.SqlClient.SqlTransaction tran = conn.BeginTransaction(IsolationLevel.ReadCommitted))
                            {
                                using (var bulkCopy = new Microsoft.Data.SqlClient.SqlBulkCopy(conn, Microsoft.Data.SqlClient.SqlBulkCopyOptions.Default, tran))
                                {
                                    bulkCopy.DestinationTableName = "InvShippingSchedule_T1";
                                    bulkCopy.ColumnMappings.Add("Dest", "Dest");
                                    bulkCopy.ColumnMappings.Add("Importer", "Importer");
                                    bulkCopy.ColumnMappings.Add("Model", "Model");
                                    bulkCopy.ColumnMappings.Add("PartNo", "PartNo");
                                    bulkCopy.ColumnMappings.Add("Mod", "Mod");
                                    bulkCopy.ColumnMappings.Add("OrderLot", "OrderLot");
                                    bulkCopy.ColumnMappings.Add("TotalBox", "TotalBox");
                                    bulkCopy.ColumnMappings.Add("Date", "Date");
                                    bulkCopy.ColumnMappings.Add("BoxQty", "BoxQty");
                                    bulkCopy.ColumnMappings.Add("Guid", "Guid");
                                    bulkCopy.ColumnMappings.Add("ValuationTypeFrom", "ValuationTypeFrom");
                                    bulkCopy.WriteToServer(table);
                                    tran.Commit();
                                }
                            }
                            await conn.CloseAsync();
                        }
                    }


                    /// xử lý trang 2 trong template -> cho dữ liệu vào bảng InvShippingSchedule_T2
                    ///
                    /// Lấy Shipping Month
                    row = 14;
                    col = 5;

                    bool inLastYear = false; /// để xem có phải cuối năm hay không
                    var shippingMonth = v_worksheet_p2.Cells[row, col].Value.ToString();
                    var toPort = v_worksheet_p2.Cells[row + 1, col].Value.ToString();

                    row = 7;
                    col = 8;
                    int countDaysInMonth = 0; // đếm các ngày trong tháng
                    int countIndex = 1;
                    string format = "MM/dd";
                    List<InvShippingScheduleSheet2ImportDto> listHaisen = new List<InvShippingScheduleSheet2ImportDto>();
                    while (true)
                    {
                        /// lấy dữ liệu
                        if (countDaysInMonth >= 31)
                        {
                            break;
                        }
                        InvShippingScheduleSheet2ImportDto invShippingScheduleSheet2ImportDto = new InvShippingScheduleSheet2ImportDto();

                        invShippingScheduleSheet2ImportDto.Haisen = v_worksheet_p2.Cells[row, col].Value.ToString();

                        var ETD = v_worksheet_p2.Cells[row + 1, col].Value.ToString();
                        var ETA = v_worksheet_p2.Cells[row + 2, col].Value.ToString();

                        /// parse từ cell -> datetime 
                        DateTime parsedETD = DateTime.ParseExact(ETD, format, CultureInfo.InvariantCulture);
                        DateTime parsedETA = DateTime.ParseExact(ETA, format, CultureInfo.InvariantCulture);
                        /// set năm của datetime là năm từ shippingmonth
                        parsedETD = parsedETD.AddYears(int.Parse(shippingMonth.Substring(0,4)) - parsedETD.Year);
                        parsedETA = parsedETA.AddYears(int.Parse(shippingMonth.Substring(0,4)) - parsedETA.Year);

                        if (parsedETA.Month == 12 || parsedETD.Month == 12)
                        {
                            inLastYear = true;
                        }
                        if (inLastYear)
                        {
                            parsedETA = parsedETA.AddMonths((parsedETA.Month >= 1 && parsedETA.Month < 12) ? 12 : 0);
                            parsedETD = parsedETD.AddMonths((parsedETD.Month >= 1 && parsedETD.Month < 12) ? 12 : 0);
                        }


                        invShippingScheduleSheet2ImportDto.ETA = parsedETA.ToString("yyyyMMdd");
                        invShippingScheduleSheet2ImportDto.ETD = parsedETD.ToString("yyyyMMdd");

                        invShippingScheduleSheet2ImportDto.Vessel = v_worksheet_p2.Cells[row + 3, col].Value.ToString();
                        invShippingScheduleSheet2ImportDto.Carrier = v_worksheet_p2.Cells[row + 4, col].Value.ToString();
                        invShippingScheduleSheet2ImportDto.VNo = v_worksheet_p2.Cells[row + 5, col].Value.ToString();
                        invShippingScheduleSheet2ImportDto.CSign = v_worksheet_p2.Cells[row + 6, col].Value.ToString();
                        invShippingScheduleSheet2ImportDto.BNo = v_worksheet_p2.Cells[row + 7, col].Value.ToString();
                        invShippingScheduleSheet2ImportDto.Kakutei = v_worksheet_p2.Cells[row + 8, col].Value.ToString();
                        invShippingScheduleSheet2ImportDto.Index = countIndex;
                        invShippingScheduleSheet2ImportDto.ToPort = toPort;

                        invShippingScheduleSheet2ImportDto.ValuationTypeFrom = "JSP";
                        invShippingScheduleSheet2ImportDto.PackingMonth = shippingMonth;



                        invShippingScheduleSheet2ImportDto.FromDate = v_worksheet_p2.Cells[row + 11, col].Value.ToString();

                        int countDays = 0; // đếm các ngày trong 1 shipment
                        while (true)
                        {
                            if (v_worksheet_p2.Cells[row + 11, col + countDays].Value.ToString() == "TTL")
                            {
                                break;
                            }
                            countDays++;
                        }

                        invShippingScheduleSheet2ImportDto.ToDate = v_worksheet_p2.Cells[row + 11, col + countDays - 1].Value.ToString();

                        listHaisen.Add(invShippingScheduleSheet2ImportDto);


                        // check xem có bị cancel hay không
                        var style = v_worksheet_p2.Cells[row, col].Style;
                        if (style.FillPattern.PatternStyle == FillPatternStyle.ThinDiagonalStripe)
                        {
                            invShippingScheduleSheet2ImportDto.IsCancelled = 1;
                        } else
                        {
                            invShippingScheduleSheet2ImportDto.IsCancelled = 0;
                        }
                        invShippingScheduleSheet2ImportDto.Guid = strGUID;

                        col += countDays + 1; // cộng 1 thêm cột TTL
                        countDaysInMonth += countDays;
                        countIndex++;
                    }
                        
                    // lưu dữ liệu vào bảng T2
                    if (listHaisen.Count > 0)
                    {
                        IEnumerable<InvShippingScheduleSheet2ImportDto> dataE = listHaisen.AsEnumerable();
                        DataTable table = new DataTable();
                        using (var reader = ObjectReader.Create(dataE))
                        {
                            table.Load(reader);
                        }
                        string connectionString = Commons.getConnectionString();
                        using (Microsoft.Data.SqlClient.SqlConnection conn = new Microsoft.Data.SqlClient.SqlConnection(connectionString))
                        {
                            await conn.OpenAsync();

                            using (Microsoft.Data.SqlClient.SqlTransaction tran = conn.BeginTransaction(IsolationLevel.ReadCommitted))
                            {
                                using (var bulkCopy = new Microsoft.Data.SqlClient.SqlBulkCopy(conn, Microsoft.Data.SqlClient.SqlBulkCopyOptions.Default, tran))
                                {
                                    bulkCopy.DestinationTableName = "InvShippingSchedule_T2";
                                    bulkCopy.ColumnMappings.Add("Haisen", "Haisen");
                                    bulkCopy.ColumnMappings.Add("ETD", "ETD");
                                    bulkCopy.ColumnMappings.Add("ETA", "ETA");
                                    bulkCopy.ColumnMappings.Add("Vessel", "Vessel");
                                    bulkCopy.ColumnMappings.Add("Carrier", "Carrier");
                                    bulkCopy.ColumnMappings.Add("VNo", "VNo");
                                    bulkCopy.ColumnMappings.Add("CSign", "CSign");
                                    bulkCopy.ColumnMappings.Add("BNo", "BNo");
                                    bulkCopy.ColumnMappings.Add("Kakutei", "Kakutei");
                                    bulkCopy.ColumnMappings.Add("FromDate", "FromDate");
                                    bulkCopy.ColumnMappings.Add("ToDate", "ToDate");
                                    bulkCopy.ColumnMappings.Add("IsCancelled", "IsCancelled");
                                    bulkCopy.ColumnMappings.Add("PackingMonth", "PackingMonth");
                                    bulkCopy.ColumnMappings.Add("Guid", "Guid");
                                    bulkCopy.ColumnMappings.Add("Index", "Index");
                                    bulkCopy.ColumnMappings.Add("ToPort", "ToPort");
                                    bulkCopy.ColumnMappings.Add("ValuationTypeFrom", "ValuationTypeFrom");



                                    bulkCopy.WriteToServer(table);
                                    tran.Commit();
                                }
                            }
                            await conn.CloseAsync();
                        }
                    }

                    /// merge vào bảng chính
                    var _sqlMerge = "Exec INV_SHIPPING_SCHEDULE_MERGE_NEW @p_guid";
                    _dapperRepo.Execute(_sqlMerge, new
                    {
                        p_guid = strGUID,
                    });


                    return listImport;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(400, ex.Message);
            }
        }
    
        public async Task<Boolean> GetInvCkdShippingJSPToCancel(GetInvCkdShippingCancelInput input)
        {
            //check exist
            string _sql = "Exec [INV_SHIPPING_SCHEDULE_CANCEL_CHECK_EXIST] @p_shipmentno, @p_shippingmonth, @p_vesseletd1st, @p_index";

            IEnumerable<InvCkdShippingScheduleDto> result = await _dapperRepo.QueryAsync<InvCkdShippingScheduleDto>(_sql, new
            {
                p_shipmentno = input.ShipmentNo,
                p_shippingmonth = input.ShippingMonth,
                p_vesseletd1st = input.VesselETD1st,
                p_index = input.Index
            }); 
            var listResult = result.ToList();
            //nếu có shipment sau
            if (listResult.Count() > 0)
            {
                try
                {
                    var _sqlMerge = "Exec [INV_SHIPPING_SCHEDULE_CANCEL_NEW] @p_shipmentno, @p_shippingmonth, @p_vesseletd1st, @p_index";
                    await _dapperRepo.ExecuteAsync(_sqlMerge, new
                    {
                        p_shipmentno = input.ShipmentNo,
                        p_shippingmonth = input.ShippingMonth,
                        p_vesseletd1st = input.VesselETD1st,
                        p_index = input.Index
                    });
                    return true;
                }
                catch (Exception ex)
                {
                    throw new UserFriendlyException(400, ex.Message);
                }
            } else
            {
                // nếu không có
                throw new UserFriendlyException(400, "Shipment cannot be cancelled");
            }
        }

        public async Task<Boolean> GetInvCkdShippingToFirm(GetInvCkdShippingFirmInput input)
        {
            string _sql = "Exec [INV_SHIPPING_SCHEDULE_FIRM] @p_header_id, @p_type";

            if (input.ShipmentHeaderId != null)
            {
                foreach (var headId in input.ShipmentHeaderId)
                {
                    _ = await _dapperRepo.ExecuteAsync(_sql, new
                    {
                        p_header_id = headId,
                        p_type = input.Type
                    });
                }
            }
            return true;
        }

        public async Task GetShipmentDetailsEdit(InvShipmentDetailInput input)
        {
            await _dapperRepo.ExecuteAsync(@"
                  exec [dbo].[INV_CKD_SHIPMENT_DETAILS_SUBMIT_UPDATE]  
                                @id,
                                @p_Etd,
                                @p_Eta,
                                @p_Qty,
                                @p_EtaDelay,
                                @p_Remark,
                                @p_UserId
                                                                         
                ", new
            {
                id = input.Id,
                p_Etd = input.ETD,
                p_Eta = input.ETA,
                p_Qty = input.Quantity,
                p_EtaDelay = input.ETADelay,
                p_Remark = input.Remark,
                p_UserId = AbpSession.UserId
            });
        }

        public async Task CancelManualUpdate()
        {
            await _dapperRepo.ExecuteAsync(@" exec [dbo].[INV_CKD_SHIPMENT_DETAILS_CANCEL_UPDATE]", new{});
        }
    }
}
