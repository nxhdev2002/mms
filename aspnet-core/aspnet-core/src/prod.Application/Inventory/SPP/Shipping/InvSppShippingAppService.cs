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
using prod.Inventory.SPP.Shipping.Exporting;
using prod.Inventory.SPP.Shipping.Dto;
using Abp.AspNetZeroCore.Net;
using System.IO;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using prod.Storage;
using NPOI.SS.Util;
using Twilio.Rest.Api.V2010.Account;
using GemBox.Spreadsheet;
using Stripe.Reporting;

namespace prod.Inventory.SPP.Shipping
{
    [AbpAuthorize(AppPermissions.Pages_SPP_Shipping_View)]
    public class InvSppShippingAppService : prodAppServiceBase, IInvSppShippingAppService
    {
        private readonly IDapperRepository<InvSppShipping, long> _dapperRepo;
        private readonly IRepository<InvSppShipping, long> _repo;
        private readonly IInvSppShippingExcelExporter _shippingExcelExporter;
        private readonly ITempFileCacheManager _tempFileCacheManager;

        public InvSppShippingAppService(IRepository<InvSppShipping, long> repo,
                                         IDapperRepository<InvSppShipping, long> dapperRepo,
                                         IInvSppShippingExcelExporter shippingExcelExporter,
                                         ITempFileCacheManager tempFileCacheManager
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _shippingExcelExporter = shippingExcelExporter;
            _tempFileCacheManager = tempFileCacheManager;
        }

        public async Task<PagedResultDto<InvSppShippingDto>> GetAll(GetInvSppShippingInput input)
        {
            string reportType = input.repoType != null ? string.Join(",", input.repoType) : "";
            string _sql = "Exec INV_SPP_SHIPPING_SEARCH_NEW @p_partno, @p_customerno, @p_customerorderno, @p_invoiceno, @p_stock, @p_frommonthyear, @p_tomonthyear, @p_report_type";

            IEnumerable<InvSppShippingDto> result = await _dapperRepo.QueryAsync<InvSppShippingDto>(_sql, new
            {
                p_partno = input.PartNo,
                p_customerno = input.CustomerNo,
                p_customerorderno = input.CustomerOrderNo,
                p_invoiceno = input.InvoiceNo,
                p_stock = input.Stock,
                p_frommonthyear = input.FromMonthYear,
                p_tomonthyear = input.ToMonthYear,
                p_report_type = reportType
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvSppShippingDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<FileDto> GetShippingToExcel(GetInvSppShippingExportInput input)
        {
            string reportType = input.repoType != null ? string.Join(",", input.repoType) : "";
            string _sql = "Exec INV_SPP_SHIPPING_SEARCH_NEW @p_partno, @p_customerno, @p_customerorderno, @p_invoiceno, @p_stock, @p_frommonthyear, @p_tomonthyear, @p_report_type";

            IEnumerable<InvSppShippingDto> result = await _dapperRepo.QueryAsync<InvSppShippingDto>(_sql, new
            {
                p_partno = input.PartNo,
                p_customerno = input.CustomerNo,
                p_customerorderno = input.CustomerOrderNo,
                p_invoiceno = input.InvoiceNo,
                p_stock = input.Stock,
                p_frommonthyear = input.FromMonthYear,
                p_tomonthyear = input.ToMonthYear,
                p_report_type = reportType
            });

            var exportToExcel = result.ToList();
            return _shippingExcelExporter.ExportToFile(exportToExcel);
        }

        public async Task<FileDto> GetShippingCostOfSaleSummaryToExcel(GetInvSppShippingCostOfSaleExportInput input)
        {

            string contentRootPath = "/Template/temp_InvSppShippingCostOfSaleSummary.xlsx";
            string webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot") + contentRootPath;
            string pathExcelTemp = webRootPath;
            string pathExcel = "/Download/";
            string nameExcel = "InvSppShippingCostOfSaleSummary" + DateTime.Now.ToString("MMddyyyy-HHmmss") + ".xlsx";
            string pathDownload = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot") + pathExcel + nameExcel;
            var fileDto = new FileDto(nameExcel, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);


            FileInfo finfo = new FileInfo(pathDownload);
            if (finfo.Exists) { try { finfo.Delete(); } catch (Exception ex) { } }

            XSSFWorkbook xlsxObject = null;     //XLSX
            ISheet sheet = null;
            IRow row = null;

            ICell cell = null;
            CellRangeAddress range = null;


            int totalQty1 = 0;
            double totalCost1 = 0;
            double totalSaleAmount1 = 0;
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
            istyle.DataFormat = sheet.Workbook.CreateDataFormat().GetFormat("#,##0");

            ICellStyle istyleBold = xlsxObject.CreateCellStyle();
            istyleBold.FillPattern = FillPattern.SolidForeground;
            istyleBold.FillForegroundColor = IndexedColors.White.Index;
            istyleBold.BorderBottom = BorderStyle.Thin;
            istyleBold.BorderTop = BorderStyle.Thin;
            istyleBold.BorderLeft = BorderStyle.Thin;
            istyleBold.BorderRight = BorderStyle.Thin;
            istyleBold.SetFont(boldFont);


            ICellStyle istyleBold1 = xlsxObject.CreateCellStyle();
            istyleBold1.FillPattern = FillPattern.SolidForeground;
            istyleBold1.FillForegroundColor = IndexedColors.White.Index;
            istyleBold1.BorderBottom = BorderStyle.Thin;
            istyleBold1.BorderTop = BorderStyle.Thin;
            istyleBold1.BorderLeft = BorderStyle.Thin;
            istyleBold1.BorderRight = BorderStyle.Thin;
            istyleBold1.DataFormat = sheet.Workbook.CreateDataFormat().GetFormat("#,##0");
            istyleBold1.SetFont(boldFont);

            /// A4 theo template
            totalQty1 = 0;
            totalCost1 = 0;
            totalSaleAmount1 = 0;

            try
            {

                if (input.ToMonthYear == null)
                {
                    input.ToMonthYear = input.FromMonthYear;
                }
                int r = 5;
                /// From - To (Ex:  From: 6/2023     To: 6/2023)
                /// Fill vào 2 ô D3 và F16
                string fromDate = input.FromMonthYear.ToString("MM/yyyy");
                string toDate = input.ToMonthYear != null ? input.ToMonthYear?.ToString("MM/yyyy")
                                                : DateTime.Today.AddMonths(-1).ToString("MM/yyyy");
                cr = new CellReference("D3");
                row = sheet.GetRow(cr.Row);
                cell = row.GetCell(cr.Col);
                if (input.CurrencyType == 0)
                {
                    cell.SetCellValue("From: " + fromDate + "      To: " + toDate);

                }
                else
                {
                    cell.SetCellValue("Từ tháng: " + fromDate + "      Đến tháng: " + toDate);

                }

                if (input.repoType != null && input.repoType.Count > 0)
                {
                    for (int items = 0; items < input.repoType.Count(); items++)
                    {
                        string _sql = "Exec INV_SPP_SHIPPING_COST_OF_SALE_SUMMARY_NEW @p_frommonth, @p_tomonth, @p_report_type, @p_stock, @p_currency_type";

                        IEnumerable<InvSppShippingCostOfSaleSummaryDto> summaryResult = await _dapperRepo.QueryAsync<InvSppShippingCostOfSaleSummaryDto>(_sql, new
                        {
                            p_frommonth = input.FromMonthYear,
                            p_tomonth = input.ToMonthYear,
                            p_report_type = input.repoType[items],
                            p_stock = input.Stock,
                            p_currency_type = input.CurrencyType
                        });

                        if (summaryResult.Count() > 0)
                        {


                            var summaryRecords = summaryResult.FirstOrDefault();

                            #region Cost of Sale Summary


                            row = sheet.CreateRow(r);
                            // Report Type
                            cell = row.CreateCell(0);
                            cell.CellStyle = istyleBold;
                            cell.SetCellValue(summaryRecords.ReportType);


                            range = new CellRangeAddress(r, r, 0, 2);
                            sheet.AddMergedRegion(range);
                            RegionUtil.SetBorderBottom(1, range, sheet);
                            RegionUtil.SetBorderLeft(1, range, sheet);
                            RegionUtil.SetBorderRight(1, range, sheet);
                            RegionUtil.SetBorderTop(1, range, sheet);


                            if (summaryRecords.Qty != null)
                            {

                                // Qty   
                                cell = row.CreateCell(3);
                                cell.CellStyle = istyle;
                                cell.SetCellValue(summaryRecords.Qty);
                                totalQty1 += summaryRecords.Qty;
                            }

                            // Cost
                            if (summaryRecords.Cost != null)
                            {
                                cell = row.CreateCell(4);
                                cell.CellStyle = istyle;
                                cell.SetCellValue((double)summaryRecords.Cost);
                                totalCost1 += (double)summaryRecords.Cost;
                            }

                            // SaleAmount
                            if (summaryRecords.SaleAmount != null)
                            {
                                cell = row.CreateCell(5);
                                cell.CellStyle = istyle;
                                cell.SetCellValue((double)summaryRecords.SaleAmount);
                                totalSaleAmount1 += (double)summaryRecords.SaleAmount;
                            }

                            r++;
                            // add total row 

                            #endregion

                        }

                    }

                    row = sheet.CreateRow(r);
                    cell = row.CreateCell(0);
                    cell.SetCellValue("Summary");
                    cell.CellStyle = istyleBold;

                    range = new CellRangeAddress(r, r, 0, 2);
                    sheet.AddMergedRegion(range);
                    RegionUtil.SetBorderBottom(1, range, sheet);
                    RegionUtil.SetBorderLeft(1, range, sheet);
                    RegionUtil.SetBorderRight(1, range, sheet);
                    RegionUtil.SetBorderTop(1, range, sheet);

                    cell = row.CreateCell(3);
                    cell.CellStyle = istyleBold1;
                    cell.SetCellValue(totalQty1);

                    cell = row.CreateCell(4);
                    cell.CellStyle = istyleBold1;
                    cell.SetCellValue(totalCost1);

                    cell = row.CreateCell(5);
                    cell.CellStyle = istyleBold1;
                    cell.SetCellValue(totalSaleAmount1);

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

        public async Task<FileDto> GetShippingCostOfSaleToExcel(GetInvSppShippingCostOfSaleExportInput input)
        {

            string contentRootPath = "/Template/temp_InvSppShippingCostOfSale_.xlsx";
            string webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot") + contentRootPath;
            string pathExcelTemp = webRootPath;
            string pathExcel = "/Download/";
            string nameExcel = "InvSppShippingCostOfSale" + DateTime.Now.ToString("MMddyyyy-HHmmss") + ".xlsx";
            string pathDownload = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot") + pathExcel + nameExcel;
            var fileDto = new FileDto(nameExcel, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);


            FileInfo finfo = new FileInfo(pathDownload);
            if (finfo.Exists) { try { finfo.Delete(); } catch (Exception ex) { } }

            XSSFWorkbook xlsxObject = null;     //XLSX
            ISheet sheet = null;
            IRow row = null;

            ICell cell = null;
            CellRangeAddress range = null;

            int totalQty = 0;
            double totalCost = 0;
            double totalSaleAmount = 0;


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

            ICellStyle istyleBold1 = xlsxObject.CreateCellStyle();
            istyleBold1.FillPattern = FillPattern.SolidForeground;
            istyleBold1.FillForegroundColor = IndexedColors.White.Index;
            istyleBold1.BorderBottom = BorderStyle.Thin;
            istyleBold1.BorderTop = BorderStyle.Thin;
            istyleBold1.BorderLeft = BorderStyle.Thin;
            istyleBold1.BorderRight = BorderStyle.Thin;
            istyleBold1.DataFormat = sheet.Workbook.CreateDataFormat().GetFormat("#,##0");

            var font = xlsxObject.CreateFont();
            font.IsBold = true;
            font.FontHeightInPoints = (short)15;
            font.Color = IndexedColors.DarkBlue.Index;

            var fontDate = xlsxObject.CreateFont();
            fontDate.IsBold = true;
            fontDate.FontHeightInPoints = (short)20;
            fontDate.Color = IndexedColors.DarkBlue.Index;

            ICellStyle cellStyleInt = sheet.Workbook.CreateCellStyle();
            cellStyleInt.FillPattern = FillPattern.SolidForeground;
            cellStyleInt.FillForegroundColor = IndexedColors.White.Index;
            cellStyleInt.BorderBottom = BorderStyle.Thin;
            cellStyleInt.BorderTop = BorderStyle.Thin;
            cellStyleInt.BorderLeft = BorderStyle.Thin;
            cellStyleInt.BorderRight = BorderStyle.Thin;
            cellStyleInt.SetFont(boldFont);
            cellStyleInt.DataFormat = sheet.Workbook.CreateDataFormat().GetFormat("#,##0");



            try
            {
                if (input.ToMonthYear == null)
                {
                    input.ToMonthYear = input.FromMonthYear;
                }
                int r = 3;

                if (input.repoType != null && input.repoType.Count > 0)
                {
                    for (int j = 0; j < input.repoType.Count(); j++)
                    {

                        string repoType = input.repoType[j];
                        row = sheet.CreateRow(r - 2);
                        cell = row.CreateCell(0);
                        cell.SetCellValue(repoType);
                        cell.CellStyle.SetFont(font);

                        string _sql = "Exec INV_SPP_SHIPPING_COST_OF_SALE_NEW @p_frommonth, @p_tomonth, @p_report_type, @p_stock, @p_currency_type";

                        IEnumerable<InvSppShippingCostOfSaleDetailDto> detailResult = await _dapperRepo.QueryAsync<InvSppShippingCostOfSaleDetailDto>(_sql, new
                        {
                            p_frommonth = input.FromMonthYear,
                            p_tomonth = input.ToMonthYear,
                            p_report_type = input.repoType[j],
                            p_stock = input.Stock,
                            p_currency_type = input.CurrencyType
                        });



                        var detailRecords = detailResult.ToList();

                        /// From - To (Ex:  From: 6/2023     To: 6/2023)
                        /// Fill vào 2 ô D3 và F16
                        string fromDate = input.FromMonthYear.ToString("MM/yyyy");
                        string toDate = input.ToMonthYear != null ? input.ToMonthYear?.ToString("MM/yyyy")
                                                        : DateTime.Today.AddMonths(-1).ToString("MM/yyyy");
                        cr = new CellReference("E1");
                        row = sheet.GetRow(cr.Row);
                        cell = row.GetCell(cr.Col);
                        if (input.CurrencyType == 0)
                        {
                            cell.SetCellValue("From: " + fromDate + "      To: " + toDate);
                            cell.CellStyle.SetFont(fontDate);
                        }
                        else
                        {
                            cell.SetCellValue("Từ tháng: " + fromDate + "      Đến tháng: " + toDate);
                            cell.CellStyle.SetFont(fontDate);
                        }
                


             





                        #region Cost Of Sale detail		
                        /// 
                        /// A18 theo template
                        // A18 theo template
                        totalQty = 0;
                        totalCost = 0;
                        totalSaleAmount = 0;
                        for (int i = 0; i < detailRecords.Count; i++)
                        {
                            row = sheet.CreateRow(r);
                            // No
                            cell = row.CreateCell(0);
                            cell.CellStyle = istyle;
                            cell.SetCellValue(i + 1);
                            cell.CellStyle = istyleBold;

                            // CustomerNo
                            cell = row.CreateCell(1);
                            cell.CellStyle = istyle;
                            cell.SetCellValue(detailRecords[i].CustomerNo);

                            // Report Type
                            cell = row.CreateCell(2);
                            cell.CellStyle = istyle;
                            cell.SetCellValue(detailRecords[i].ReportType);

                            // Part No
                            cell = row.CreateCell(3);
                            cell.CellStyle = istyle;
                            cell.SetCellValue(detailRecords[i].PartNo);

                            // Qty
                            if (detailRecords[i].Qty != null)
                            {
                                cell = row.CreateCell(4);
                                cell.CellStyle = istyle;
                                cell.SetCellValue(detailRecords[i].Qty);
                                totalQty += detailRecords[i].Qty;
                                cell.CellStyle = istyleBold1;
                            }
                            else
                            {
                                cell = row.CreateCell(4);
                                cell.CellStyle = istyle;
                                cell.SetCellValue(0);
                            }
                 

                            // Cost 
                            if (detailRecords[i].Cost != null)
                            {
                                cell = row.CreateCell(5);
                                cell.CellStyle = istyle;
                                cell.SetCellValue((double)detailRecords[i].Cost);
                                totalCost += (double)detailRecords[i].Cost;
                                cell.CellStyle = istyleBold1;

                            }
                            else
                            {
                                cell = row.CreateCell(5);
                                cell.CellStyle = istyle;
                                cell.SetCellValue(0);
                            }

                            // Sale Amount
                            if (detailRecords[i].SaleAmount != null)
                            {
                                cell = row.CreateCell(6);
                                cell.CellStyle = istyle;
                                cell.SetCellValue((double)detailRecords[i].SaleAmount);
                                totalSaleAmount += (double)detailRecords[i].SaleAmount;
                                cell.CellStyle = istyleBold1;

                            }
                            else
                            {
                                cell = row.CreateCell(6);
                                cell.CellStyle = istyle;
                                cell.SetCellValue(0);
                            }
                            r++;

                        }

                        // add total row    

                        row = sheet.CreateRow(r);
                        cell = row.CreateCell(0);
                        cell.SetCellValue("Grand Total");
                        cell.CellStyle = istyleBold;

                        range = new CellRangeAddress(r, r, 0, 3);
                        sheet.AddMergedRegion(range);
                        RegionUtil.SetBorderBottom(1, range, sheet);
                        RegionUtil.SetBorderLeft(1, range, sheet);
                        RegionUtil.SetBorderRight(1, range, sheet);
                        RegionUtil.SetBorderTop(1, range, sheet);



                        cell = row.CreateCell(4);
                        cell.CellStyle = istyleBold;
                        cell.SetCellValue(totalQty);
                        cell.CellStyle = cellStyleInt;


                        cell = row.CreateCell(5);
                        cell.CellStyle = istyleBold;
                        cell.SetCellValue(totalCost);
                        cell.CellStyle = cellStyleInt;

                        cell = row.CreateCell(6);
                        cell.CellStyle = istyleBold;
                        cell.SetCellValue(totalSaleAmount);
                        cell.CellStyle = cellStyleInt;

                        //
                        //sheet.Cells.CopyRow(sheet.Cells, 1, r+1);
                        #endregion

                        if(j < input.repoType.Count - 1)
                        {
                            row = sheet.CreateRow(r + 2);
                            cell = row.CreateCell(0);
                            cell.SetCellValue("No");
                            cell.CellStyle = istyleBold;


                            cell = row.CreateCell(1);
                            cell.SetCellValue("Customer No");
                            cell.CellStyle = istyleBold;

                            cell = row.CreateCell(2);
                            cell.SetCellValue("Type");
                            cell.CellStyle = istyleBold;

                            cell = row.CreateCell(3);
                            cell.SetCellValue("Part No");
                            cell.CellStyle = istyleBold;

                            cell = row.CreateCell(4);
                            cell.SetCellValue("Qty");
                            cell.CellStyle = istyleBold;


                            cell = row.CreateCell(5);
                            cell.SetCellValue("Cost (sum)");
                            cell.CellStyle = istyleBold;


                            cell = row.CreateCell(6);
                            cell.SetCellValue("Sale Amount");
                            cell.CellStyle = istyleBold;

                        }


                        FileStream xfile = new FileStream(pathDownload, FileMode.Create, System.IO.FileAccess.Write);
                        xlsxObject.Write(xfile);
                        xfile.Dispose();



                        r = r + 3;
                    }
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

        public async Task<FileDto> GetShippingGLTransactionToExcel(GetInvSppShippingCostOfSaleExportInput input)
        {

            string contentRootPath = "/Template/temp_InvSppShippingGLTransaction.xlsx";
            string webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot") + contentRootPath;
            string pathExcelTemp = webRootPath;
            string pathExcel = "/Download/";
            string nameExcel = "InvSppShippingGLTransaction" + DateTime.Now.ToString("MMddyyyy-HHmmss") + ".xlsx";
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

            ICellStyle istyleBold = xlsxObject.CreateCellStyle();
            istyleBold.FillPattern = FillPattern.SolidForeground;
            istyleBold.FillForegroundColor = IndexedColors.White.Index;
            istyleBold.BorderBottom = BorderStyle.Thin;
            istyleBold.BorderTop = BorderStyle.Thin;
            istyleBold.BorderLeft = BorderStyle.Thin;
            istyleBold.BorderRight = BorderStyle.Thin;
            istyleBold.SetFont(boldFont);

            ICellStyle istyleBold1 = xlsxObject.CreateCellStyle();
            istyleBold1.FillPattern = FillPattern.SolidForeground;
            istyleBold1.FillForegroundColor = IndexedColors.White.Index;
            istyleBold1.BorderBottom = BorderStyle.Thin;
            istyleBold1.BorderTop = BorderStyle.Thin;
            istyleBold1.BorderLeft = BorderStyle.Thin;
            istyleBold1.BorderRight = BorderStyle.Thin;
            istyleBold1.DataFormat = sheet.Workbook.CreateDataFormat().GetFormat("#,##0");

            var font = xlsxObject.CreateFont();
            font.IsBold = true;
            font.FontHeightInPoints = (short)15;
            font.Color = IndexedColors.DarkBlue.Index;

            var fontDate = xlsxObject.CreateFont();
            fontDate.IsBold = true;
            fontDate.FontHeightInPoints = (short)20;

            ICellStyle cellStyleInt = sheet.Workbook.CreateCellStyle();
            cellStyleInt.FillPattern = FillPattern.SolidForeground;
            cellStyleInt.FillForegroundColor = IndexedColors.White.Index;
            cellStyleInt.BorderBottom = BorderStyle.Thin;
            cellStyleInt.BorderTop = BorderStyle.Thin;
            cellStyleInt.BorderLeft = BorderStyle.Thin;
            cellStyleInt.BorderRight = BorderStyle.Thin;
            cellStyleInt.SetFont(boldFont);
            cellStyleInt.DataFormat = sheet.Workbook.CreateDataFormat().GetFormat("#,##0");

            try
            {
                if (input.ToMonthYear == null)
                {
                    input.ToMonthYear = input.FromMonthYear;
                }

                 /// From - To (Ex:  From: 6/2023     To: 6/2023)
                /// Fill vào 2 ô D3 và F16
                string fromDate = input.FromMonthYear.ToString("MM/yyyy");
                string toDate = input.ToMonthYear != null ? input.ToMonthYear?.ToString("MM/yyyy")
                                                : DateTime.Today.AddMonths(-1).ToString("MM/yyyy");


                cr = new CellReference("B2");
                row = sheet.GetRow(cr.Row);
                cell = row.CreateCell(cr.Col);
                if(input.CurrencyType == 1){
                    cell.SetCellValue("GL Transaction    Từ tháng: " + fromDate + "     Đến tháng: " + toDate);
                }else{
                    cell.SetCellValue("GL Transaction    From: " + fromDate + "     To: " + toDate);
                }


                cell.CellStyle.SetFont(fontDate);


                range = new CellRangeAddress(1, 1, 1, 4);
                sheet.AddMergedRegion(range);

                int r = 4;
                string _sql = "Exec INV_SPP_SHIPPING_GL_TRANSACTION @p_frommonth, @p_tomonth, @p_stock, @p_curency";

                IEnumerable<InvSppShippingGLTransactionDto> detailResult = await _dapperRepo.QueryAsync<InvSppShippingGLTransactionDto>(_sql, new
                {
                    p_frommonth = input.FromMonthYear,
                    p_tomonth = input.ToMonthYear,
                    p_stock = input.Stock,
                    p_curency = input.CurrencyType
                });

                var detailRecords = detailResult.ToList();

                #region fill	
                /// 
                for (int i = 0; i < detailRecords.Count; i++)
                {
                    row = sheet.CreateRow(r);

                    // Tai khoan
                    cell = row.CreateCell(0);
                    cell.CellStyle = istyle;
                    cell.SetCellValue(detailRecords[i].TaiKhoan);

                    // Ghi no
                    cell = row.CreateCell(1);
                    cell.CellStyle = cellStyleInt;
                    cell.SetCellValue((double)(detailRecords[i].GhiNo != null ? detailRecords[i].GhiNo : 0));

                    // Ghi co
                    cell = row.CreateCell(2);
                    cell.CellStyle = cellStyleInt;
                    cell.SetCellValue((double)(detailRecords[i].GhiCo != null ? detailRecords[i].GhiCo : 0));

                    // Mo ta
                    cell = row.CreateCell(3);
                    cell.CellStyle = istyle;
                    cell.SetCellValue(detailRecords[i].MoTa + " (Từ tháng " + fromDate + " - Đến tháng " + toDate + ")");

                    r++;

                }

                //
                //sheet.Cells.CopyRow(sheet.Cells, 1, r+1);
                #endregion


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
        /*    public async Task<FileDto> GetShippingCostOfSaleSummaryToExcel(GetInvSppShippingCostOfSaleExportInput input)
            {

                string contentRootPath = "/Template/temp_InvSppShippingCostOfSale.xlsx";
                string webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot") + contentRootPath;
                string pathExcelTemp = webRootPath;
                string pathExcel = "/Download/";
                string nameExcel = "InvSppShippingCostOfSale" + DateTime.Now.ToString("MMddyyyy-HHmmss") + ".xlsx";
                string pathDownload = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot") + pathExcel + nameExcel;
                var fileDto = new FileDto(nameExcel, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);


                FileInfo finfo = new FileInfo(pathDownload);
                if (finfo.Exists) { try { finfo.Delete(); } catch (Exception ex) { } }

                XSSFWorkbook xlsxObject = null;     //XLSX
                ISheet sheet = null;
                IRow row = null;

                ICell cell = null;
                CellRangeAddress range = null;

                int totalQty = 0;
                double totalCost = 0;
                double totalSaleAmount = 0;

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

                try
                {

                    for (int j = 0; j < input.repoType.Count(); j++)
                    {
                        if (input.ToMonthYear == null)
                        {
                            input.ToMonthYear = input.FromMonthYear;
                        }

                        string _sql = "Exec INV_SPP_SHIPPING_COST_OF_SALE_SUMMARY @p_frommonth, @p_tomonth, @p_report_type, @p_stock, @p_currency_type";

                        IEnumerable<InvSppShippingCostOfSaleSummaryDto> summaryResult = await _dapperRepo.QueryAsync<InvSppShippingCostOfSaleSummaryDto>(_sql, new
                        {
                            p_frommonth = input.FromMonthYear,
                            p_tomonth = input.ToMonthYear,
                            p_report_type = input.repoType[j],
                            p_stock = input.Stock,
                            p_currency_type = input.CurrencyType
                        });


                        _sql = "Exec INV_SPP_SHIPPING_COST_OF_SALE @p_frommonth, @p_tomonth, @p_report_type, @p_stock, @p_currency_type";

                        IEnumerable<InvSppShippingCostOfSaleDetailDto> detailResult = await _dapperRepo.QueryAsync<InvSppShippingCostOfSaleDetailDto>(_sql, new
                        {
                            p_frommonth = input.FromMonthYear,
                            p_tomonth = input.ToMonthYear,
                            p_report_type = input.repoType[j],
                            p_stock = input.Stock,
                            p_currency_type = input.CurrencyType
                        });


                        var summaryRecords = summaryResult.ToList();
                        var detailRecords = detailResult.ToList();

                        /// From - To (Ex:  From: 6/2023     To: 6/2023)
                        /// Fill vào 2 ô D3 và F16
                        string fromDate = input.FromMonthYear.ToString("MM/yyyy");
                        string toDate = input.ToMonthYear != null ? input.ToMonthYear?.ToString("MM/yyyy")
                                                        : DateTime.Today.AddMonths(-1).ToString("MM/yyyy");

                        cr = new CellReference("D3");
                        row = sheet.GetRow(cr.Row);
                        cell = row.GetCell(cr.Col);
                        cell.SetCellValue("From: " + fromDate + "     To: " + toDate);

                        cr = new CellReference("F16");
                        row = sheet.GetRow(cr.Row);
                        cell = row.GetCell(cr.Col);
                        cell.SetCellValue("From: " + fromDate + "     To: " + toDate);

                        #region Cost of Sale Summary

                        int r = 5; /// A4 theo template
                        totalQty = 0;
                        totalCost = 0;
                        totalSaleAmount = 0;
                        for (int i = 0; i < summaryRecords.Count; i++)
                        {
                            row = sheet.CreateRow(r);
                            // Report Type
                            cell = row.CreateCell(0);
                            cell.CellStyle = istyle;
                            cell.SetCellValue(summaryRecords[i].ReportType);


                            range = new CellRangeAddress(r, r, 0, 2);
                            sheet.AddMergedRegion(range);
                            RegionUtil.SetBorderBottom(1, range, sheet);
                            RegionUtil.SetBorderLeft(1, range, sheet);
                            RegionUtil.SetBorderRight(1, range, sheet);
                            RegionUtil.SetBorderTop(1, range, sheet);


                            // Qty
                            cell = row.CreateCell(3);
                            cell.CellStyle = istyle;
                            cell.SetCellValue(summaryRecords[i].Qty);
                            totalQty += summaryRecords[i].Qty;

                            // Cost
                            if (summaryRecords[i].Cost != null)
                            {
                                cell = row.CreateCell(4);
                                cell.CellStyle = istyle;
                                cell.SetCellValue((double)summaryRecords[i].Cost);
                                totalCost += (double)summaryRecords[i].Cost;
                            }

                            // SaleAmount
                            if (summaryRecords[i].SaleAmount != null)
                            {
                                cell = row.CreateCell(5);
                                cell.CellStyle = istyle;
                                cell.SetCellValue((double)summaryRecords[i].SaleAmount);
                                totalSaleAmount += (double)summaryRecords[i].SaleAmount;
                            }

                            r++;
                        }
                        // add total row 
                        row = sheet.CreateRow(r);
                        cell = row.CreateCell(0);
                        cell.SetCellValue("Summary");
                        cell.CellStyle = istyleBold;

                        range = new CellRangeAddress(r, r, 0, 2);
                        sheet.AddMergedRegion(range);
                        RegionUtil.SetBorderBottom(1, range, sheet);
                        RegionUtil.SetBorderLeft(1, range, sheet);
                        RegionUtil.SetBorderRight(1, range, sheet);
                        RegionUtil.SetBorderTop(1, range, sheet);

                        cell = row.CreateCell(3);
                        cell.CellStyle = istyleBold;
                        cell.SetCellValue(totalQty);

                        cell = row.CreateCell(4);
                        cell.CellStyle = istyleBold;
                        cell.SetCellValue(totalCost);

                        cell = row.CreateCell(5);
                        cell.CellStyle = istyleBold;
                        cell.SetCellValue(totalSaleAmount);

                        #endregion

                        #region Cost Of Sale detail		
                        /// 
                        r = 17; /// A18 theo template
                        totalQty = 0;
                        totalCost = 0;
                        totalSaleAmount = 0;
                        for (int i = 0; i < detailRecords.Count; i++)
                        {
                            row = sheet.CreateRow(r);
                            // No
                            cell = row.CreateCell(0);
                            cell.CellStyle = istyle;
                            cell.SetCellValue(i + 1);

                            // CustomerNo
                            cell = row.CreateCell(1);
                            cell.CellStyle = istyle;
                            cell.SetCellValue(detailRecords[i].CustomerNo);

                            // Report Type
                            cell = row.CreateCell(2);
                            cell.CellStyle = istyle;
                            cell.SetCellValue(detailRecords[i].ReportType);

                            // Part No
                            cell = row.CreateCell(3);
                            cell.CellStyle = istyle;
                            cell.SetCellValue(detailRecords[i].PartNo);

                            // Qty
                            cell = row.CreateCell(4);
                            cell.CellStyle = istyle;
                            cell.SetCellValue(detailRecords[i].Qty);
                            totalQty += detailRecords[i].Qty;

                            // Cost 
                            if (detailRecords[i].Cost != null)
                            {
                                cell = row.CreateCell(5);
                                cell.CellStyle = istyle;
                                cell.SetCellValue((double)detailRecords[i].Cost);
                                totalCost += (double)detailRecords[i].Cost;
                            }

                            // Sale Amount
                            if (detailRecords[i].SaleAmount != null)
                            {
                                cell = row.CreateCell(6);
                                cell.CellStyle = istyle;
                                cell.SetCellValue((double)detailRecords[i].SaleAmount);
                                totalSaleAmount += (double)detailRecords[i].SaleAmount;
                            }
                            r = r + 2;

                        }

                        // add total row    

                        row = sheet.CreateRow(r);
                        cell = row.CreateCell(0);
                        cell.SetCellValue("Grand Total");
                        cell.CellStyle = istyleBold;

                        range = new CellRangeAddress(r, r, 0, 3);
                        sheet.AddMergedRegion(range);
                        RegionUtil.SetBorderBottom(1, range, sheet);
                        RegionUtil.SetBorderLeft(1, range, sheet);
                        RegionUtil.SetBorderRight(1, range, sheet);
                        RegionUtil.SetBorderTop(1, range, sheet);



                        cell = row.CreateCell(4);
                        cell.CellStyle = istyleBold;
                        cell.SetCellValue(totalQty);

                        cell = row.CreateCell(5);
                        cell.CellStyle = istyleBold;
                        cell.SetCellValue(totalCost);

                        cell = row.CreateCell(6);
                        cell.CellStyle = istyleBold;
                        cell.SetCellValue(totalSaleAmount);
                        //

                        #endregion

                        FileStream xfile = new FileStream(pathDownload, FileMode.Create, System.IO.FileAccess.Write);
                        xlsxObject.Write(xfile);
                        xfile.Dispose();

                        MemoryStream downloadStream = new MemoryStream(File.ReadAllBytes(pathDownload));
                        _tempFileCacheManager.SetFile(fileDto.FileToken, downloadStream.ToArray());
                        File.Delete(pathDownload);
                        downloadStream.Position = 0;

                    }


                } catch (Exception ex) {
                }



                return await Task.FromResult(fileDto);
            }

    */
        //public async Task<FileDto> GetShippingCostOfSaleVndSummaryToExcel(GetInvSppShippingCostOfSaleExportInput input)
        //{

        //    string contentRootPath = "/Template/temp_InvSppShippingCostOfSaleVnd.xlsx";
        //    string webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot") + contentRootPath;
        //    string pathExcelTemp = webRootPath;
        //    string pathExcel = "/Download/";
        //    string nameExcel = "InvSppShippingCostOfSaleVnd" + DateTime.Now.ToString("MMddyyyy-HHmmss") + ".xlsx";
        //    string pathDownload = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot") + pathExcel + nameExcel;
        //    var fileDto = new FileDto(nameExcel, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);


        //    FileInfo finfo = new FileInfo(pathDownload);
        //    if (finfo.Exists) { try { finfo.Delete(); } catch (Exception ex) { } }

        //    XSSFWorkbook xlsxObject = null;     //XLSX
        //    ISheet sheet = null;
        //    IRow row = null;

        //    ICell cell = null;
        //    CellRangeAddress range = null;

        //    int totalQty = 0;
        //    double totalCost = 0;
        //    double totalSaleAmount = 0;

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

        //    ICellStyle istyleBold = xlsxObject.CreateCellStyle();
        //    istyleBold.FillPattern = FillPattern.SolidForeground;
        //    istyleBold.FillForegroundColor = IndexedColors.White.Index;
        //    istyleBold.BorderBottom = BorderStyle.Thin;
        //    istyleBold.BorderTop = BorderStyle.Thin;
        //    istyleBold.BorderLeft = BorderStyle.Thin;
        //    istyleBold.BorderRight = BorderStyle.Thin;
        //    istyleBold.SetFont(boldFont);

        //    try
        //    {

        //        if (input.ToMonthYear == null)
        //        {
        //            input.ToMonthYear = input.FromMonthYear;
        //        }

        //        for (int j = 0; j < input.repoType.Count(); j++)
        //        {

        //            string _sql = "Exec INV_SPP_SHIPPING_COST_OF_SALE_SUMMARY @p_frommonth, @p_tomonth, @p_report_type, @p_stock, @p_currency_type";

        //            IEnumerable<InvSppShippingCostOfSaleSummaryDto> summaryResult = await _dapperRepo.QueryAsync<InvSppShippingCostOfSaleSummaryDto>(_sql, new
        //            {
        //                p_frommonth = input.FromMonthYear,
        //                p_tomonth = input.ToMonthYear,
        //                p_report_type = input.repoType[j],
        //                p_stock = input.Stock,
        //                p_currency_type = input.CurrencyType
        //            });


        //            _sql = "Exec INV_SPP_SHIPPING_COST_OF_SALE @p_frommonth, @p_tomonth, @p_report_type, @p_stock, @p_currency_type";

        //            IEnumerable<InvSppShippingCostOfSaleDetailDto> detailResult = await _dapperRepo.QueryAsync<InvSppShippingCostOfSaleDetailDto>(_sql, new
        //            {
        //                p_frommonth = input.FromMonthYear,
        //                p_tomonth = input.ToMonthYear,
        //                p_report_type = input.repoType[j],
        //                p_stock = input.Stock,
        //                p_currency_type = input.CurrencyType
        //            });


        //            var summaryRecords = summaryResult.ToList();
        //            var detailRecords = detailResult.ToList();

        //            /// From - To (Ex:  From: 6/2023     To: 6/2023)
        //            /// Fill vào 2 ô D3 và F16
        //            string fromDate = input.FromMonthYear.ToString("MM/yyyy");
        //            string toDate = input.ToMonthYear != null ? input.ToMonthYear?.ToString("MM/yyyy")
        //                                            : DateTime.Today.AddMonths(-1).ToString("MM/yyyy");

        //            cr = new CellReference("D3");
        //            row = sheet.GetRow(cr.Row);
        //            cell = row.GetCell(cr.Col);
        //            cell.SetCellValue("Từ tháng: " + fromDate + "     Đến tháng: " + toDate);

        //            cr = new CellReference("F16");
        //            row = sheet.GetRow(cr.Row);
        //            cell = row.GetCell(cr.Col);
        //            cell.SetCellValue("Từ tháng: " + fromDate + "     Đến tháng: " + toDate);

        //            #region Cost of Sale Summary

        //            int r = 5; /// A4 theo template
        //            totalQty = 0;
        //            totalCost = 0;
        //            totalSaleAmount = 0;
        //            for (int i = 0; i < summaryRecords.Count; i++)
        //            {
        //                row = sheet.CreateRow(r);
        //                // Report Type
        //                cell = row.CreateCell(0);
        //                cell.CellStyle = istyle;
        //                cell.SetCellValue(summaryRecords[i].ReportType);


        //                range = new CellRangeAddress(r, r, 0, 2);
        //                sheet.AddMergedRegion(range);
        //                RegionUtil.SetBorderBottom(1, range, sheet);
        //                RegionUtil.SetBorderLeft(1, range, sheet);
        //                RegionUtil.SetBorderRight(1, range, sheet);
        //                RegionUtil.SetBorderTop(1, range, sheet);


        //                // Qty
        //                cell = row.CreateCell(3);
        //                cell.CellStyle = istyle;
        //                cell.SetCellValue(summaryRecords[i].Qty);
        //                totalQty += summaryRecords[i].Qty;

        //                // Cost
        //                if (summaryRecords[i].Cost != null)
        //                {
        //                    cell = row.CreateCell(4);
        //                    cell.CellStyle = istyle;
        //                    cell.SetCellValue((double)summaryRecords[i].Cost);
        //                    totalCost += (double)summaryRecords[i].Cost;
        //                }

        //                // SaleAmount
        //                if (summaryRecords[i].SaleAmount != null)
        //                {
        //                    cell = row.CreateCell(5);
        //                    cell.CellStyle = istyle;
        //                    cell.SetCellValue((double)summaryRecords[i].SaleAmount);
        //                    totalSaleAmount += (double)summaryRecords[i].SaleAmount;
        //                }

        //                r++;
        //            }
        //            // add total row 
        //            row = sheet.CreateRow(r);
        //            cell = row.CreateCell(0);
        //            cell.SetCellValue("Summary");
        //            cell.CellStyle = istyleBold;

        //            range = new CellRangeAddress(r, r, 0, 2);
        //            sheet.AddMergedRegion(range);
        //            RegionUtil.SetBorderBottom(1, range, sheet);
        //            RegionUtil.SetBorderLeft(1, range, sheet);
        //            RegionUtil.SetBorderRight(1, range, sheet);
        //            RegionUtil.SetBorderTop(1, range, sheet);

        //            cell = row.CreateCell(3);
        //            cell.CellStyle = istyleBold;
        //            cell.SetCellValue(totalQty);

        //            cell = row.CreateCell(4);
        //            cell.CellStyle = istyleBold;
        //            cell.SetCellValue(totalCost);

        //            cell = row.CreateCell(5);
        //            cell.CellStyle = istyleBold;
        //            cell.SetCellValue(totalSaleAmount);

        //            #endregion

        //            #region Cost Of Sale detail		
        //            /// 
        //            r = 17; /// A18 theo template
        //            totalQty = 0;
        //            totalCost = 0;
        //            totalSaleAmount = 0;
        //            for (int i = 0; i < detailRecords.Count; i++)
        //            {
        //                row = sheet.CreateRow(r);
        //                // No
        //                cell = row.CreateCell(0);
        //                cell.CellStyle = istyle;
        //                cell.SetCellValue(i + 1);

        //                // CustomerNo
        //                cell = row.CreateCell(1);
        //                cell.CellStyle = istyle;
        //                cell.SetCellValue(detailRecords[i].CustomerNo);

        //                // Report Type
        //                cell = row.CreateCell(2);
        //                cell.CellStyle = istyle;
        //                cell.SetCellValue(detailRecords[i].ReportType);

        //                // Part No
        //                cell = row.CreateCell(3);
        //                cell.CellStyle = istyle;
        //                cell.SetCellValue(detailRecords[i].PartNo);

        //                // Qty
        //                cell = row.CreateCell(4);
        //                cell.CellStyle = istyle;
        //                cell.SetCellValue(detailRecords[i].Qty);
        //                totalQty += detailRecords[i].Qty;

        //                // Cost 
        //                if (detailRecords[i].CostVn != null)
        //                {
        //                    cell = row.CreateCell(5);
        //                    cell.CellStyle = istyle;
        //                    cell.SetCellValue((double)detailRecords[i].CostVn);
        //                    totalCost += (double)detailRecords[i].CostVn;
        //                }

        //                // Sale Amount
        //                if (detailRecords[i].SaleAmount != null)
        //                {
        //                    cell = row.CreateCell(6);
        //                    cell.CellStyle = istyle;
        //                    cell.SetCellValue((double)detailRecords[i].SaleAmount);
        //                    totalSaleAmount += (double)detailRecords[i].SaleAmount;
        //                }
        //                r++;

        //            }

        //            // add total row    

        //            row = sheet.CreateRow(r);
        //            cell = row.CreateCell(0);
        //            cell.SetCellValue("Grand Total");
        //            cell.CellStyle = istyleBold;

        //            range = new CellRangeAddress(r, r, 0, 3);
        //            sheet.AddMergedRegion(range);
        //            RegionUtil.SetBorderBottom(1, range, sheet);
        //            RegionUtil.SetBorderLeft(1, range, sheet);
        //            RegionUtil.SetBorderRight(1, range, sheet);
        //            RegionUtil.SetBorderTop(1, range, sheet);



        //            cell = row.CreateCell(4);
        //            cell.CellStyle = istyleBold;
        //            cell.SetCellValue(totalQty);

        //            cell = row.CreateCell(5);
        //            cell.CellStyle = istyleBold;
        //            cell.SetCellValue(totalCost);

        //            cell = row.CreateCell(6);
        //            cell.CellStyle = istyleBold;
        //            cell.SetCellValue(totalSaleAmount);
        //            //

        //            #endregion

        //            FileStream xfile = new FileStream(pathDownload, FileMode.Create, System.IO.FileAccess.Write);
        //            xlsxObject.Write(xfile);
        //            xfile.Dispose();

        //            MemoryStream downloadStream = new MemoryStream(File.ReadAllBytes(pathDownload));
        //            _tempFileCacheManager.SetFile(fileDto.FileToken, downloadStream.ToArray());
        //            File.Delete(pathDownload);
        //            downloadStream.Position = 0;

        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //    }



        //    return await Task.FromResult(fileDto);
        //}



    }
}
