
using Abp.Application.Services.Dto;
using Abp.AspNetZeroCore.Net;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore;
using Abp.EntityFrameworkCore.Uow;
using Abp.UI;
using FastMember;
using GemBox.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using prod.Authorization;
using prod.Common;
using prod.Dto;
using prod.EntityFrameworkCore;
using prod.Inventory.CKD.Dto;
using prod.Inventory.CKD.ShippingSchedule.Dto;
using prod.Inventory.CKD.ShippingScheduleFirm.Dto;
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

namespace prod.Inventory.CKD.ShippingScheduleFirm 
{
    [AbpAuthorize(AppPermissions.Pages_Ckd_Rundown_ShipingScheduleFirm_View)]
    public class InvCkdShippingScheduleFirmAppService : prodAppServiceBase, IInvCkdShippingScheduleFirmAppService
    {
        private readonly IRepository<InvCkdShippingScheduleDetailsFirm, long> _repo;
        private readonly IDapperRepository<InvCkdShipment, long> _dapperRepo;
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly IDbContextProvider<prodDbContext> _dbContext;

        public InvCkdShippingScheduleFirmAppService(
                                            IRepository<InvCkdShippingScheduleDetailsFirm, long> repo,
                                            IDapperRepository<InvCkdShipment, long> dapperRepo,
                                            ITempFileCacheManager tempFileCacheManager,
                                            IDbContextProvider<prodDbContext> dbContext
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _tempFileCacheManager = tempFileCacheManager;
            _dbContext = dbContext;
        }

        [AbpAuthorize(AppPermissions.Pages_Ckd_Rundown_ShipingScheduleFirm_Edit)]
        public async Task CreateOrEdit(CreateOrEditInvCkdShippingScheduleDetailsFirmDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditInvCkdShippingScheduleDetailsFirmDto input)
        {
            input.Status = "NEW";
            var mainObj = ObjectMapper.Map<InvCkdShippingScheduleDetailsFirm>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditInvCkdShippingScheduleDetailsFirmDto input)
        {
            input.Status = "UPDATED";
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Ckd_Rundown_ShipingScheduleFirm_Edit)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        [AbpAuthorize(AppPermissions.Pages_Ckd_Rundown_ShipingScheduleFirm_Edit)]
        public async Task<bool> GetShipmentToCancel(long? id)
        {
            try
            {
                var mainObj = await _repo.GetAll()
                    .FirstOrDefaultAsync(e => e.Id == id);
                mainObj.Status = "CANCELLED";
                await _dbContext.GetDbContext().SaveChangesAsync();
                return true;
            } catch (Exception ex)
            {
                throw new UserFriendlyException(400, ex.Message);
            }
            
        }

        public async Task<List<GetPartNoDto>> GetListPartNo()
        {
            IEnumerable<GetPartNoDto> result = await _dapperRepo.QueryAsync<GetPartNoDto>("SELECT DISTINCT PartNo FROM InvCkdPartList");
            return result.ToList();
        }

        public async Task<PagedResultDto<InvCkdShippingScheduleFirmDto>> GetAll(InvCkdShippingScheduleFirmInput input)
        {

            string _sql = "Exec [INV_CKD_SHIPMENT_HEAD_FIRM_SEARCH] @p_Seller, @p_Buyer, @p_PackingMonth, @p_ValuationTypeFrom, @p_PartNo, @p_ModuleNo, @p_Cfc, @p_RenbanNo," +
                "@p_PortOfLoading,@p_VesselEtd1st,@p_VesselName1st,@p_VesselNo1st,@p_PortOfDischarge,@p_EkanbanFlag, @p_RevisionNo,@p_ShipmentNo";

            IEnumerable<InvCkdShippingScheduleFirmDto> result = await _dapperRepo.QueryAsync<InvCkdShippingScheduleFirmDto>(_sql, new
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
                p_ShipmentNo = input.ShipmentNo
            });
            var listResult = result.ToList();
            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            var totalCount = listResult.Count();

            return new PagedResultDto<InvCkdShippingScheduleFirmDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<PagedResultDto<InvCkdShipmentDetailsFirmDto>> GetAllDetails(InvCkdShipmentDetailsFirmInput input)
        {
            List<InvCkdShipmentDetailsFirmDto> totalResult = new();
            string _sql = "Exec [INV_CKD_SHIPMENT_DETAIL_FIRM_SEARCH] @p_ShipmentHeaderId, @p_PartNo, @p_Cfc, @p_status";


            if (input.ShipmentHeaderId != null)
            {
                foreach (var headId in input.ShipmentHeaderId)
                {
                    IEnumerable<InvCkdShipmentDetailsFirmDto> result = await _dapperRepo.QueryAsync<InvCkdShipmentDetailsFirmDto>(_sql, new
                    {
                        p_ShipmentHeaderId = headId,
                        p_PartNo = input.PartNo,
                        p_Cfc = input.Cfc,
                        p_status = input.Status
                    });
                    var listResult = result.ToList();
                    totalResult.AddRange(listResult);
                }
            }
            else
            {
                throw new UserFriendlyException(400, "Invalid input");
            }

            var pagedAndFiltered = totalResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            var totalCount = totalResult.Count();

            return new PagedResultDto<InvCkdShipmentDetailsFirmDto>(
                totalCount,
                pagedAndFiltered);
        }


        //public async Task<FileDto> GetInvCkdShippingToExcel(GetInvCkdShippingFirmExportInput input)
        //{
        //    string contentRootPath = "/Template/temp_InvCkdShippingPlanConvertToSS.xlsx";
        //    string webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot") + contentRootPath;
        //    string pathExcelTemp = webRootPath;
        //    string pathExcel = "/Download/";
        //    string nameExcel = "JSPPackingPlanConvertToSS" + DateTime.Now.ToString("MMddyyyy-HHmmss") + ".xlsx";
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

        //        string _sql = "Exec [INV_CKD_SHIPPING_FIRM_EXPORT] @p_shippingmonthfrom, @p_shippingmonthto";

        //        IEnumerable<GetInvCkdShippingFirmExport> Result = await _dapperRepo.QueryAsync<GetInvCkdShippingFirmExport>(_sql, new
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

        public async Task<FileDto> GetInvCkdShippingFirmToExcelNew(GetInvCkdShippingFirmExportInput input)
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
                string _sql = "Exec [INV_CKD_SHIPPING_FIRM_EXPORT] @p_packingmonth";

                IEnumerable<GetInvCkdShippingFirmExportNew> Result = await _dapperRepo.QueryAsync<GetInvCkdShippingFirmExportNew>(_sql, new
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

        public async Task<FileDto> GetInvCkdShippingJSPFirmToExcelNew(GetInvCkdShippingFirmExportInput input)
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

                string _sql = "Exec [INV_SHIPPING_SCHEDULE_FIRM_EXPORT_JSP] @p_packingmonth";

                IEnumerable<GetInvCkdShippingFirmExportNew> Result = await _dapperRepo.QueryAsync<GetInvCkdShippingFirmExportNew>(_sql, new
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

        //public async Task<FileDto> GetInvCkdShippingJSPToExcel(GetInvCkdShippingFirmExportInput input)
        //{
        //    string contentRootPath = "/Template/temp_InvCkdShippingPlanConvertToSS_JSP.xlsx";
        //    string webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot") + contentRootPath;
        //    string pathExcelTemp = webRootPath;
        //    string pathExcel = "/Download/";
        //    string nameExcel = "JSPPackingPlanConvertToSS_JSP" + DateTime.Now.ToString("MMddyyyy-HHmmss") + ".xlsx";
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
        //        if (input.ShippingMonthFrom != null)
        //        {
        //            //DateTime date = DateTime.ParseExact(input.ShippingMonth, "yyyyMM", null);
        //            RequestDate = input.ShippingMonthFrom?.ToString("MMM-yy", new CultureInfo("en-US")).ToUpper();
        //        }


        //        cr = new CellReference("H7");
        //        row = sheet.GetRow(cr.Row);
        //        cell = row.GetCell(cr.Col);

        //        cell.SetCellValue("JSP SHIPPING SCHEDULE: " + RequestDate + " PACKING");

        //        string _sql = "Exec [INV_SHIPPING_SCHEDULE_FIRM_EXPORT_JSP] @p_shippingmonthfrom, @p_shippingmonthto";

        //        IEnumerable<GetInvCkdShippingFirmExport> Result = await _dapperRepo.QueryAsync<GetInvCkdShippingFirmExport>(_sql, new
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
        [AbpAuthorize(AppPermissions.Pages_Ckd_Rundown_ShipingScheduleFirm_Edit)]
        public async Task GetShipmentFirmDetailsEdit(GetShipmentFirmDetailsEditDtoInput input)
        {
            await _dapperRepo.ExecuteAsync(@"
                  exec [dbo].[INV_CKD_SHIPMENT_DETAILS_FIRM_SUBMIT_UPDATE]  
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
    }
}
