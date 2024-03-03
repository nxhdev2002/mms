using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using prod.Authorization;
using prod.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prod.Inventory.SPP.Stock.Exporting;
using prod.Inventory.SPP.Stock.Dto;
using System.IO;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using Abp.AspNetZeroCore.Net;
using prod.Storage;
using Stripe.Reporting;
using NUglify.JavaScript.Syntax;

namespace prod.Inventory.SPP.Stock
{
    [AbpAuthorize(AppPermissions.Pages_SPP_Stock_View)]
    public class InvSppStockAppService : prodAppServiceBase, IInvSppStockAppService
    {
        private readonly IDapperRepository<InvSppStock, long> _dapperRepo;
        private readonly IRepository<InvSppStock, long> _repo;
        private readonly IInvSppStockExcelExporter _stockExcelExporter;
        private readonly ITempFileCacheManager _tempFileCacheManager;

        public InvSppStockAppService(IRepository<InvSppStock, long> repo,
                                         IDapperRepository<InvSppStock, long> dapperRepo,
                                         IInvSppStockExcelExporter stockExcelExporter,
                                         ITempFileCacheManager tempFileCacheManager
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _stockExcelExporter = stockExcelExporter;
            _tempFileCacheManager = tempFileCacheManager;
        }

        public async Task<PagedResultDto<InvSppStockDto>> GetAll(GetInvSppStockInput input)
        {
            string reportType = input.repoType != null ? string.Join(",", input.repoType) : "";
            string _sql = "Exec INV_SPP_STOCK_SEARCH_NEW @p_partno, @p_warehouse, @p_frommonthyear, @p_tomonthyear, @p_reporttype";

            IEnumerable<InvSppStockDto> result = await _dapperRepo.QueryAsync<InvSppStockDto>(_sql, new
            {
                p_partno = input.PartNo,
                p_warehouse = input.Warehouse,
                p_frommonthyear = input.FromMonthYear,
                p_tomonthyear = input.ToMonthYear,
                p_reporttype = reportType,
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvSppStockDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<FileDto> GetStockToExcel(GetInvSppStockExportInput input)
        {
            string reportType = input.repoType != null ? string.Join(",", input.repoType) : "";
            string _sql = "Exec INV_SPP_STOCK_SEARCH_NEW @p_partno, @p_warehouse, @p_frommonthyear, @p_tomonthyear, @p_reporttype";

            IEnumerable<InvSppStockDto> result = await _dapperRepo.QueryAsync<InvSppStockDto>(_sql, new
            {
                p_partno = input.PartNo,
                p_warehouse = input.Warehouse,
                p_frommonthyear = input.FromMonthYear,
                p_tomonthyear = input.ToMonthYear,
                p_reporttype = reportType,
            });

            var exportToExcel = result.ToList();
            return _stockExcelExporter.ExportToFile(exportToExcel);
        }

        public async Task<FileDto> GetShippingBalanceReportToExcelNew(GetInvSppStockBalanceReportInput input)
        {
            string contentRootPath = (input.CurrencyType == 0) ? "/Template/temp_InvSppShippingBalanceReport_US.xlsx" : "/Template/temp_InvSppShippingBalanceReport_VN.xlsx";
            string webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot") + contentRootPath;
            string pathExcelTemp = webRootPath;
            string pathExcel = "/Download/";
            string nameExcel = "InvSppShippingBalanceReport" + DateTime.Now.ToString("MMddyyyy-HHmmss") + ".xlsx";
            string pathDownload = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot") + pathExcel + nameExcel;
            var fileDto = new FileDto(nameExcel, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);


            FileInfo finfo = new FileInfo(pathDownload);
            if (finfo.Exists) { try { finfo.Delete(); } catch (Exception ex) { } }

            XSSFWorkbook xlsxObject = null;     //XLSX
            ISheet sheet = null;
            IRow row = null;

            ICell cell = null;
            CellRangeAddress range = null;

            int totalPreQty = 0;
            int totalInQty = 0;
            int totalOutQty = 0;
            int totalQty = 0;

            double totalPreAmount = 0;
            double totalInAmount = 0;
            double totalOutAmount = 0;
            double totalAmount = 0;


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

            ICellStyle istyleBold = xlsxObject.CreateCellStyle();
            istyleBold.FillPattern = FillPattern.SolidForeground;
            istyleBold.FillForegroundColor = IndexedColors.White.Index;
            istyleBold.BorderBottom = BorderStyle.Thin;
            istyleBold.BorderTop = BorderStyle.Thin;
            istyleBold.BorderLeft = BorderStyle.Thin;
            istyleBold.BorderRight = BorderStyle.Thin;
            istyleBold.SetFont(boldFont);


            ICellStyle cellStyleInt = sheet.Workbook.CreateCellStyle();
            cellStyleInt.FillPattern = FillPattern.SolidForeground;
            cellStyleInt.FillForegroundColor = IndexedColors.White.Index;
            cellStyleInt.BorderBottom = BorderStyle.Thin;
            cellStyleInt.BorderTop = BorderStyle.Thin;
            cellStyleInt.BorderLeft = BorderStyle.Thin;
            cellStyleInt.BorderRight = BorderStyle.Thin;
            cellStyleInt.DataFormat = sheet.Workbook.CreateDataFormat().GetFormat("#,##0");



            ICellStyle istyleBold1 = xlsxObject.CreateCellStyle();
            istyleBold1.FillPattern = FillPattern.SolidForeground;
            istyleBold1.FillForegroundColor = IndexedColors.White.Index;
            istyleBold1.BorderBottom = BorderStyle.Thin;
            istyleBold1.BorderTop = BorderStyle.Thin;
            istyleBold1.BorderLeft = BorderStyle.Thin;
            istyleBold1.BorderRight = BorderStyle.Thin;
            istyleBold1.DataFormat = sheet.Workbook.CreateDataFormat().GetFormat("#,##0");
            istyleBold1.SetFont(boldFont);

            try
            {
                int totalPreQty1 = 0;
                int totalInQty1 = 0;
                int totalOutQty1 = 0;
                int totalQty1 = 0;

                double totalPreAmount1 = 0;
                double totalInAmount1 = 0;
                double totalOutAmount1 = 0;
                double totalAmount1 = 0;
                int r = 5; /// A6 theo template
                if (input.ToMonthYear == null)
                {
                    input.ToMonthYear = input.FromMonthYear;
                }

            //    var repoTypeList = new List<string> { "Sale Parts", "Sale C&A", "Sale Chemical (CHE)", "Sale Optional warranty (OPT)", "Sale Export", "Onhand Adjustment", "Internal", "Others" };

        //        input.repoType = (input.repoType != null) ? input.repoType : repoTypeList;

                if (input.repoType != null && input.repoType.Count > 0)
                {
                    for (int j = 0; j < input.repoType.Count(); j++)
                    {
                        string _sql = "Exec INV_SPP_SHIPPING_BALANCE_REPORT_NEW @p_frommonth, @p_tomonth, @p_stock, @p_report_type, @p_curency";

                        IEnumerable<InvSppStockBalanceReporttDto> detailResult = await _dapperRepo.QueryAsync<InvSppStockBalanceReporttDto>(_sql, new
                        {
                            p_frommonth = input.FromMonthYear,
                            p_tomonth = input.ToMonthYear,
                            p_report_type = input.repoType[j],
                            p_stock = input.Stock,
                            p_curency = input.CurrencyType
                        });


                        var detailRecords = detailResult.ToList();

                        /// From - To (Ex:  From: 6/2023     To: 6/2023)
                        /// Fill vào ô G2
                        string fromDate = input.FromMonthYear.ToString("MM/yyyy");
                        string toDate = input.ToMonthYear != null ? input.ToMonthYear?.ToString("MM/yyyy")
                                                        : DateTime.Today.AddMonths(-1).ToString("MM/yyyy");

                        cr = new CellReference("G2");
                        row = sheet.GetRow(cr.Row);
                        cell = row.GetCell(cr.Col);
                        if (input.CurrencyType == 1) {
                            cell.SetCellValue("Từ tháng: " + fromDate + "     Đến tháng: " + toDate);
                        }
                        else {
                            cell.SetCellValue("From: " + fromDate + "     To: " + toDate);
                        }


                        #region Cost Of Sale detail		
                        /// 

                        for (int i = 0; i < detailRecords.Count; i++)
                        {
                            row = sheet.CreateRow(r);
                            // No
                            cell = row.CreateCell(0);
                            cell.CellStyle = istyleBold;
                            cell.SetCellValue(i + 1);

                            // Part No
                            cell = row.CreateCell(1);
                            cell.CellStyle = istyle;
                            cell.SetCellValue(detailRecords[i].PartNo);

                            // Report Type
                            cell = row.CreateCell(2);
                            cell.CellStyle = istyle;
                            cell.SetCellValue(detailRecords[i].ReportType);

                            // Pre Qty
                            cell = row.CreateCell(3);
                            cell.CellStyle = istyle;
                            cell.SetCellValue(detailRecords[i].PreQty);
                            totalPreQty += detailRecords[i].PreQty;

                            // Pre Amount
                            cell = row.CreateCell(4);
                            cell.CellStyle = istyle;
                            cell.SetCellValue((double)detailRecords[i].PreAmount);
                            totalPreAmount += (double)detailRecords[i].PreAmount;
                            cell.CellStyle = cellStyleInt;

                            // In Qty
                            cell = row.CreateCell(5);
                            cell.CellStyle = istyle;
                            cell.SetCellValue(detailRecords[i].InQty);
                            totalInQty += detailRecords[i].InQty;

                            // In Amount
                            cell = row.CreateCell(6);
                            cell.CellStyle = istyle;
                            cell.SetCellValue((double)detailRecords[i].InAmount);
                            totalInAmount += (double)detailRecords[i].InAmount;
                            cell.CellStyle = cellStyleInt;

                            // Out Qty
                            cell = row.CreateCell(7);
                            cell.CellStyle = istyle;
                            cell.SetCellValue(detailRecords[i].OutQty);
                            totalOutQty += detailRecords[i].OutQty;


                            // Out Amount
                            cell = row.CreateCell(8);
                            cell.CellStyle = istyle;
                            cell.SetCellValue((double)detailRecords[i].OutAmount);
                            totalOutAmount += (double)detailRecords[i].OutAmount;
                            cell.CellStyle = cellStyleInt;

                            // Qty
                            cell = row.CreateCell(9);
                            cell.CellStyle = istyle;
                            cell.SetCellValue(detailRecords[i].Qty);
                            totalQty += detailRecords[i].Qty;

                            // Amount
                            cell = row.CreateCell(10);
                            cell.CellStyle = istyle;
                            cell.SetCellValue((double)detailRecords[i].Amount);
                            totalAmount += (double)detailRecords[i].Amount;
                            cell.CellStyle = cellStyleInt;

                            r++;

                        }

                        // add total row    

                        if (detailRecords.Count > 0)
                        {
                            row = sheet.CreateRow(r);
                            cell = row.CreateCell(0);
                            cell.SetCellValue("Tổng");
                            cell.CellStyle = istyleBold;

                            range = new CellRangeAddress(r, r, 0, 2);
                            sheet.AddMergedRegion(range);
                            RegionUtil.SetBorderBottom(1, range, sheet);
                            RegionUtil.SetBorderLeft(1, range, sheet);
                            RegionUtil.SetBorderRight(1, range, sheet);
                            RegionUtil.SetBorderTop(1, range, sheet);



                            cell = row.CreateCell(3);
                            cell.SetCellValue(totalPreQty);
                            cell.CellStyle = istyleBold1;

                            cell = row.CreateCell(4);
                            cell.SetCellValue(totalPreAmount);
                            cell.CellStyle = istyleBold1;

                            cell = row.CreateCell(5);
                            cell.SetCellValue(totalInQty);
                            cell.CellStyle = istyleBold1;

                            cell = row.CreateCell(6);
                            cell.SetCellValue(totalInAmount);
                            cell.CellStyle = istyleBold1;

                            cell = row.CreateCell(7);
                            cell.SetCellValue(totalOutQty);
                            cell.CellStyle = istyleBold1;

                            cell = row.CreateCell(8);
                            cell.SetCellValue(totalOutAmount);
                            cell.CellStyle = istyleBold1;

                            cell = row.CreateCell(9);
                            cell.SetCellValue(totalQty);
                            cell.CellStyle = istyleBold1;

                            cell = row.CreateCell(10);
                            cell.SetCellValue(totalAmount);
                            cell.CellStyle = istyleBold1;
                            //

                            #endregion

                            totalPreQty1 += totalPreQty;
                            totalPreAmount1 += totalPreAmount;
                            totalInQty1 += totalInQty;
                            totalInAmount1 += totalInAmount;
                            totalOutQty1 += totalOutQty;
                            totalOutAmount1 += totalOutAmount;
                            totalQty1 += totalQty;
                            totalAmount1 += totalAmount;

                            r = r + 1;

                            //reset total = 0
                            totalPreQty = 0;
                            totalPreAmount = 0;
                            totalInQty = 0;
                            totalInAmount = 0;
                            totalOutQty = 0;
                            totalOutAmount = 0;
                            totalQty = 0;
                            totalAmount = 0;
                        }
                    
                    }


                    row = sheet.CreateRow(r + 2);
                    cell = row.CreateCell(0);
                    cell.SetCellValue("Tổng chung");
                    cell.CellStyle = istyleBold;

                    range = new CellRangeAddress(r + 2, r + 2, 0, 2);
                    sheet.AddMergedRegion(range);
                    RegionUtil.SetBorderBottom(1, range, sheet);
                    RegionUtil.SetBorderLeft(1, range, sheet);
                    RegionUtil.SetBorderRight(1, range, sheet);
                    RegionUtil.SetBorderTop(1, range, sheet);



                    cell = row.CreateCell(3);
                    cell.SetCellValue(totalPreQty1);
                    cell.CellStyle = istyleBold1;

                    cell = row.CreateCell(4);
                    cell.SetCellValue(totalPreAmount1);
                    cell.CellStyle = istyleBold1;

                    cell = row.CreateCell(5);
                    cell.SetCellValue(totalInQty1);
                    cell.CellStyle = istyleBold1;

                    cell = row.CreateCell(6);
                    cell.SetCellValue(totalInAmount1);
                    cell.CellStyle = istyleBold1;

                    cell = row.CreateCell(7);
                    cell.SetCellValue(totalOutQty1);
                    cell.CellStyle = istyleBold1;

                    cell = row.CreateCell(8);
                    cell.SetCellValue(totalOutAmount1);
                    cell.CellStyle = istyleBold1;

                    cell = row.CreateCell(9);
                    cell.SetCellValue(totalQty1);
                    cell.CellStyle = istyleBold1;

                    cell = row.CreateCell(10);
                    cell.SetCellValue(totalAmount1);
                    cell.CellStyle = istyleBold1;
                    //
                    FileStream xfile = new FileStream(pathDownload, FileMode.Create, System.IO.FileAccess.Write);
                    xlsxObject.Write(xfile);
                    xfile.Dispose();

               
                }
                else
                {
                    FileStream xfile = new FileStream(pathDownload, FileMode.Create, System.IO.FileAccess.Write);
                    xlsxObject.Write(xfile);
                    xfile.Dispose();
                }
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
    }
}
