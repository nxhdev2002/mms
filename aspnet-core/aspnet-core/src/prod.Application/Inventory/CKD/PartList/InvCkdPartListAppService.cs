using Abp.Application.Services.Dto;
using Abp.AspNetZeroCore.Net;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.UI;
using FastMember;
using GemBox.Spreadsheet;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using Microsoft.Data.SqlClient;
using NPOI.HSSF.Model;
using NPOI.SS.Formula.Functions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using PayPalCheckoutSdk.Orders;
using prod.Authorization;
using prod.Common;
using prod.Dto;
using prod.HistoricalData;
using prod.Inventory.CKD.Dto;
using prod.Inventory.CKD.Exporting;
using prod.Inventory.CKD.SMQD.Dto;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Twilio.Rest.Api.V2010.Account;
using System.Dynamic;
using Abp.Json;

namespace prod.Inventory.CKD
{
    [AbpAuthorize(AppPermissions.Pages_Ckd_Master_PartList_View)]
    public class InvCkdPartListAppService : prodAppServiceBase, IInvCkdPartListAppService
    {
        private readonly IDapperRepository<InvCkdPartList, long> _dapperRepo;
        private readonly IDapperRepository<InvCkdPartGrade, long> _partGrade;
        private readonly IRepository<InvCkdPartList, long> _repo;
        private readonly IInvCkdPartListExcelExporter _calendarListExcelExporter;
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly IHistoricalDataAppService _historicalDataAppService;

        public InvCkdPartListAppService(IRepository<InvCkdPartList, long> repo,
                                         IDapperRepository<InvCkdPartList, long> dapperRepo,
                                          IDapperRepository<InvCkdPartGrade, long> partGrade,
                                          IInvCkdPartListExcelExporter calendarListExcelExporter,
                                        ITempFileCacheManager tempFileCacheManager,
                                        IHistoricalDataAppService historicalDataAppService)
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _partGrade = partGrade;
            _calendarListExcelExporter = calendarListExcelExporter;
            _tempFileCacheManager = tempFileCacheManager;
            _historicalDataAppService = historicalDataAppService;
        }
        public async Task<List<string>> GetPartListHistory(GetInvCkdPartListHistoryInput input)
        {
            return await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
        }

        public async Task<FileDto> GetHistoricalDataToExcel(GetInvCkdPartListHistoryExcelInput input)
        {
            var data = await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
            return _calendarListExcelExporter.ExportToHistoricalFile(data);
        }
        
        public async Task<List<string>> GetPartListHistoryFilter(GetInvCkdPartListHistoryFilterInput input)
        {
            List<string> includeCols = new List<string> { "PartNo", "LastModifierUserId" };
            return await _historicalDataAppService.GetHistoricalDataByFilter("InvCkdPartList", input.FromDate, input.ToDate, input.CDCAction, includeCols.ToArray());
        }

        public async Task<FileDto> GetHistoricalDataFilterToExcel(GetInvCkdPartListHistoryFilterExcelExporterInput input)
        {
            List<string> includeCols = new List<string> { "PartNo", "LastModifierUserId" };
            var data = await _historicalDataAppService.GetHistoricalDataByFilter("InvCkdPartList", input.FromDate, input.ToDate, input.CDCAction, includeCols.ToArray());
            return _calendarListExcelExporter.ExportToHistoricalFile(data);
        }



        public async Task<ChangedRecordIdsDto> GetChangedRecords()
        {
            var listPartNo = await _historicalDataAppService.GetChangedRecordIds("InvCkdPartList");
            var listPartGrade = await _historicalDataAppService.GetChangedRecordIds("InvCkdPartGrade");
            var listPartPacking = await _historicalDataAppService.GetChangedRecordIds("InvCkdPartPackingDetails");

            ChangedRecordIdsDto result = new ChangedRecordIdsDto();
            result.PartList = listPartNo;
            result.PartGrade = listPartGrade;
            result.PartPacking = listPartPacking;
            return result;
        }


        public async Task<PagedResultDto<InvCkdPartListDto>> GetAll(GetInvCkdPartListInput input)
        {
            string _sql = "Exec INV_CKD_PART_LIST_GET_BY_PARTNO @p_part_no,@p_cfc, @p_model, @p_grade, @p_shop, @p_supplier_no,@p_order_pattern,@p_active";

            IEnumerable<InvCkdPartListDto> result = await _dapperRepo.QueryAsync<InvCkdPartListDto>(_sql, new
            {
                p_part_no = input.PartNo,
                p_cfc = input.Cfc,
                p_model = input.Model,
                p_grade = input.Grade,
                p_shop = input.Shop,
                p_supplier_no = input.SupplierNo,
                p_order_pattern = input.OrderPattern,
                p_active = input.IsActive,
            });
            var listResult = result.ToList();
            //var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvCkdPartListDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<PagedResultDto<InvCkdPartGradeDto>> GetPartGrade(GetInvCkdPartListGradeInput input)
        {
            string _sqlSearch = "Exec INV_CKD_PART_GRADE_BY_PARTNO @p_part_id";

            IEnumerable<InvCkdPartGradeDto> result = await _partGrade.QueryAsync<InvCkdPartGradeDto>(_sqlSearch,
                  new
                  {
                      p_part_id = input.PartId

                  });
            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvCkdPartGradeDto>(
                totalCount,
                pagedAndFiltered);
        }
        public async Task<PagedResultDto<InvPartPackingDetailDto>> GetPartPackingDetail(GetInvCkdPartListDetailInput input)
        {
            string _sqlSearch = "Exec INV_CKD_PART_PACKING_DETAIL_BY_PARTNO @p_part_id, @p_order_pattern";

            IEnumerable<InvPartPackingDetailDto> result = await _partGrade.QueryAsync<InvPartPackingDetailDto>(_sqlSearch,
                  new
                  {
                      p_part_id = input.PartId,
                      p_order_pattern = input.OrderPatern

                  });
            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvPartPackingDetailDto>(
                totalCount,
                pagedAndFiltered);
        }


        [AbpAuthorize(AppPermissions.Pages_Ckd_Master_PartList_Validate)]
        public async Task<PagedResultDto<ValidatePartListDto>> GetValidateInvCkdPartList(PagedAndSortedResultRequestDto input)
        {
            string _sqlSearch = "Exec [INV_CKD_PART_LIST_VALIDATE]";

            IEnumerable<ValidatePartListDto> result = await _partGrade.QueryAsync<ValidatePartListDto>(_sqlSearch, new { });
            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<ValidatePartListDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<FileDto> GetValidateInvCkdPartListExcel()
        {

            string _sql = "Exec [INV_CKD_PART_LIST_VALIDATE]";

            IEnumerable<ValidatePartListDto> result = await _dapperRepo.QueryAsync<ValidatePartListDto>(_sql, new
            { });

            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportValidateToFile(exportToExcel);
        }

        public async Task<FileDto> GetCkdPartExportToFile(InvCkdPartListExportInput input)
        {
            var file = new FileDto("CKDPartList.xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
            string fileName = "temp_CKDPartList";
            string template = "wwwroot/Template";
            string path = "";
            path = Path.Combine(Directory.GetCurrentDirectory(), template, fileName);
            var xlWorkBook = ExcelFile.Load(path + ".xlsx");
            var workSheet = xlWorkBook.Worksheets[0];

            List<string> listExport = new List<string>();
            listExport.Add("Common");
            listExport.Add("Type");
            listExport.Add("Cfc");
            listExport.Add("Shop");
            listExport.Add("IdLine");
            listExport.Add("PartNo");
            listExport.Add("PartName");
            listExport.Add("SupplierNo");
            listExport.Add("SupplierCd");
            listExport.Add("BodyColor");
            listExport.Add("StartLot");
            listExport.Add("EndLot");
            listExport.Add("StartRun");
            listExport.Add("EndRun");
            listExport.Add("BackNo");
            listExport.Add("ModuleNo");
            listExport.Add("Renban");

            listExport.Add("BoxSize");
            listExport.Add("ReExportCd");
            listExport.Add("IcoFlag");
            listExport.Add("StartPackingMonth");
            listExport.Add("EndPackingMonth");
            listExport.Add("StartProductionMonth");
            listExport.Add("EndProductionMonth");
            listExport.Add("Blank");
            listExport.Add("Remark");
            listExport.Add("Value");

            string[] properties = listExport.ToArray();
            string _sql = "Exec INV_CKD_PART_LIST_SEARCH @part_no,@cfc, @p_model, @p_grade, @p_shop,@supplierNo,@p_order_pattern";
            var listDataHeader = (await _dapperRepo.QueryAsync<ExportCkdPartListDto>(_sql, new
            {
                part_no = input.PartNo,
                cfc = input.Cfc,
                p_model = input.Model,
                p_grade = input.Grade,
                p_shop = input.Shop,
                supplierNo = input.SupplierNo,
                p_order_pattern = input.OrderPattern,
            })).ToList();

            _sql = "Exec INV_CKD_PART_GRADE_BY_GRADE @p_part_no";
            var listGrade = (await _partGrade.QueryAsync<ExportCkdPartListGradeDto>(_sql, new { p_part_no = input.PartNo })).ToList();


            DataTable table = new DataTable();
            using (var reader = ObjectReader.Create(listDataHeader, properties))
            {
                table.Load(reader);
            }
            var style = new CellStyle();
            style.Borders.SetBorders(MultipleBorders.All, SpreadsheetColor.FromArgb(0, 0, 0), GemBox.Spreadsheet.LineStyle.Thin);
            style.FillPattern.SetSolid(SpreadsheetColor.FromArgb(255, 255, 102));

            var styleheader = new CellStyle();
            styleheader.Borders.SetBorders(MultipleBorders.All, SpreadsheetColor.FromArgb(0, 0, 0), GemBox.Spreadsheet.LineStyle.Thin);
            styleheader.FillPattern.SetSolid(SpreadsheetColor.FromArgb(89, 89, 89));
            styleheader.Font.Color = SpreadsheetColor.FromName(ColorName.White);
            styleheader.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            styleheader.VerticalAlignment = VerticalAlignmentStyle.Center;

            for (int i = 0; i < listGrade.Count; i++)
            {
                table.Columns.Add(listGrade[i].Grade, typeof(int));
                workSheet.Cells[0, 25 + i].Value = listGrade[i].Grade;
                workSheet.Cells[0, 25 + i].Style = styleheader;
            }
            workSheet.Cells[0, 25 + listGrade.Count].Value = "Remark";
            workSheet.Cells[0, 25 + listGrade.Count].Style = styleheader;



            for (int i = 0; i < table.Rows.Count; i++)
            {
                DataRow dr = table.Rows[i];

                foreach (string x in dr["Value"].ToString().Split(","))
                {
                    if (x != null && x != "")
                        dr[x.Split("-")[0]] = x.Split("-")[1] == "" ? 0 : int.Parse(x.Split("-")[1]);
                }

                //set style excel with Gembox 
                for (int column_index = 0; column_index <= (25 + listGrade.Count); column_index++)
                {
                    if (column_index == 24) { continue; }
                    var cell = workSheet.Cells[i + 2, column_index];
                    cell.Style = style;
                }

            }

            if (table.Rows.Count > 0)
            {
                table.Columns.Remove("Value");
                table.Columns["Remark"].SetOrdinal(table.Columns.Count - 1);
                InsertDataTableOptions ins = new InsertDataTableOptions(2, 0);
                workSheet.InsertDataTable(table, ins);
            }


            var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx");
            xlWorkBook.Save(tempFile);
            MemoryStream obj_stream = new MemoryStream();
            obj_stream = new MemoryStream(File.ReadAllBytes(tempFile));
            _tempFileCacheManager.SetFile(file.FileToken, obj_stream.ToArray());
            File.Delete(tempFile);
            obj_stream.Position = 0;
            return file;
        }


        public async Task<FileDto> GetCkdPartGroupBodyColorExportToFile(InvCkdPartListExportInput input)
        {
            var file = new FileDto("CKDPartListGroupBodyColor.xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
            string fileName = "temp_CKDPartList";
            string template = "wwwroot/Template";
            string path = "";
            path = Path.Combine(Directory.GetCurrentDirectory(), template, fileName);
            var xlWorkBook = ExcelFile.Load(path + ".xlsx");
            var workSheet = xlWorkBook.Worksheets[0];

            List<string> listExport = new List<string>();
            listExport.Add("Common");
            listExport.Add("Type");
            listExport.Add("Cfc");
            listExport.Add("Shop");
            listExport.Add("IdLine");
            listExport.Add("PartNo");
            listExport.Add("PartName");
            listExport.Add("SupplierNo");
            listExport.Add("SupplierCd");
            listExport.Add("BodyColor");
            listExport.Add("StartLot");
            listExport.Add("EndLot");
            listExport.Add("StartRun");
            listExport.Add("EndRun");
            listExport.Add("BackNo");
            listExport.Add("ModuleNo");
            listExport.Add("Renban");

            listExport.Add("BoxSize");
            listExport.Add("ReExportCd");
            listExport.Add("IcoFlag");
            listExport.Add("StartPackingMonth");
            listExport.Add("EndPackingMonth");
            listExport.Add("StartProductionMonth");
            listExport.Add("EndProductionMonth");
            listExport.Add("Blank");
            listExport.Add("Remark");
            listExport.Add("Value");

            string[] properties = listExport.ToArray();
            string _sql = "Exec INV_CKD_PART_LIST_GROUP_BODY_COLOR_SEARCH @part_no,@cfc,@p_model, @p_grade, @p_shop,@supplierNo,@p_order_pattern";
            var listDataHeader = (await _dapperRepo.QueryAsync<ExportCkdPartListDto>(_sql, new
            {
                part_no = input.PartNo,
                cfc = input.Cfc,
                p_model = input.Model,
                p_grade = input.Grade,
                p_shop = input.Shop,
                supplierNo = input.SupplierNo,
                p_order_pattern = input.OrderPattern,
            })).ToList();

            _sql = "Exec INV_CKD_PART_GRADE_BY_GRADE @p_part_no";
            var listGrade = (await _partGrade.QueryAsync<ExportCkdPartListGradeDto>(_sql, new { p_part_no = input.PartNo })).ToList();


            DataTable table = new DataTable();
            using (var reader = ObjectReader.Create(listDataHeader, properties))
            {
                table.Load(reader);
            }
            var style = new CellStyle();
            style.Borders.SetBorders(MultipleBorders.All, SpreadsheetColor.FromArgb(0, 0, 0), GemBox.Spreadsheet.LineStyle.Thin);
            style.FillPattern.SetSolid(SpreadsheetColor.FromArgb(255, 255, 102));

            var styleheader = new CellStyle();
            styleheader.Borders.SetBorders(MultipleBorders.All, SpreadsheetColor.FromArgb(0, 0, 0), GemBox.Spreadsheet.LineStyle.Thin);
            styleheader.FillPattern.SetSolid(SpreadsheetColor.FromArgb(89, 89, 89));
            styleheader.Font.Color = SpreadsheetColor.FromName(ColorName.White);
            styleheader.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            styleheader.VerticalAlignment = VerticalAlignmentStyle.Center;

            for (int i = 0; i < listGrade.Count; i++)
            {
                table.Columns.Add(listGrade[i].Grade, typeof(int));
                workSheet.Cells[0, 25 + i].Value = listGrade[i].Grade;
                workSheet.Cells[0, 25 + i].Style = styleheader;
            }
            workSheet.Cells[0, 25 + listGrade.Count].Value = "Remark";
            workSheet.Cells[0, 25 + listGrade.Count].Style = styleheader;



            for (int i = 0; i < table.Rows.Count; i++)
            {
                DataRow dr = table.Rows[i];

                foreach (string x in dr["Value"].ToString().Split(","))
                {
                    if (x != null && x != "")
                        dr[x.Split("-")[0]] = x.Split("-")[1] == "" ? 0 : int.Parse(x.Split("-")[1]);
                }

                //set style excel with Gembox 
                for (int column_index = 0; column_index <= (25 + listGrade.Count); column_index++)
                {
                    if (column_index == 24) { continue; }
                    var cell = workSheet.Cells[i + 2, column_index];
                    cell.Style = style;
                }

            }

            if (table.Rows.Count > 0)
            {
                table.Columns.Remove("Value");
                table.Columns["Remark"].SetOrdinal(table.Columns.Count - 1);
                InsertDataTableOptions ins = new InsertDataTableOptions(2, 0);
                workSheet.InsertDataTable(table, ins);
            }


            var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx");
            xlWorkBook.Save(tempFile);
            MemoryStream obj_stream = new MemoryStream();
            obj_stream = new MemoryStream(File.ReadAllBytes(tempFile));
            _tempFileCacheManager.SetFile(file.FileToken, obj_stream.ToArray());
            File.Delete(tempFile);
            obj_stream.Position = 0;
            return file;
        }

        [AbpAuthorize(AppPermissions.Pages_Ckd_Master_PartList_Import)]
        public async Task<List<ImportCkdPartListDto>> ImportDataInvCkdPartListFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<ImportCkdPartListDto> listImport = new List<ImportCkdPartListDto>();
                using (var stream = new MemoryStream(fileBytes))
                {
                    SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
                    var xlWorkBook = ExcelFile.Load(stream);
                    var v_worksheet = xlWorkBook.Worksheets[0];
                    DataFormatter formatter = new DataFormatter();
                    DateTime dateTime = DateTime.Now;

                    //string v_devanning_date = (v_worksheet.Cells[4, 2]).Value?.ToString() ?? "";
                    string strGUID = Guid.NewGuid().ToString("N");

                    string _sql = @"EXEC CMM_IMPORT_USER_GUID_INSERT @Guid, @p_UserId";
                    await _dapperRepo.ExecuteAsync(_sql, new
                    {
                        Guid = strGUID,
                        p_UserId = AbpSession.UserId
                    });

                    int countGrade = 0;

                    //Đếm số lượng bản ghi Grade
                    for (int i = 21; i <= 500; i++)
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(v_worksheet.Cells[7, i].Value)))
                        {
                            countGrade++;
                        }
                        else { break; }

                    }

                    bool flag_HasRecord = false;
                    for (int i = 9; i < v_worksheet.Rows.Count; i++)
                    {

                        string v_partNo = (v_worksheet.Cells[i, 5]).Value?.ToString() ?? "";

                        if (v_partNo != "")
                        {
                            var v_remark = Convert.ToString(v_worksheet.Cells[i, 21 + countGrade + 1].Value);

                            string v_common = (v_worksheet.Cells[i, 0]).Value?.ToString() ?? "";
                            string v_type = (v_worksheet.Cells[i, 1]).Value?.ToString() ?? "";
                            string v_cfc = (v_worksheet.Cells[i, 2]).Value?.ToString() ?? "";
                            string v_shop = (v_worksheet.Cells[i, 3]).Value?.ToString() ?? "";
                            string v_idLine = (v_worksheet.Cells[i, 4]).Value?.ToString() ?? "";
                            string v_partName = (v_worksheet.Cells[i, 6]).Value?.ToString() ?? "";
                            string v_exporter = (v_worksheet.Cells[i, 7]).Value?.ToString() ?? "";
                            string v_exporterCode = (v_worksheet.Cells[i, 8]).Value?.ToString() ?? "";
                            string v_bodyColor = (v_worksheet.Cells[i, 9]).Value?.ToString() ?? "";
                            string v_backNo = (v_worksheet.Cells[i, 10]).Value?.ToString() ?? "";
                            string v_moduleNo = (v_worksheet.Cells[i, 11]).Value?.ToString() ?? "";
                            string v_renban = (v_worksheet.Cells[i, 12]).Value?.ToString() ?? "";
                            int v_boxSize = Int32.Parse((v_worksheet.Cells[i, 13]).Value?.ToString() ?? "0");
                            string v_reExporterCode = (v_worksheet.Cells[i, 14]).Value?.ToString() ?? "";
                            string v_icoFlag = (v_worksheet.Cells[i, 15]).Value?.ToString() ?? "";
                            DateTime? v_startPackingMonth = v_worksheet.Cells[i, 16].Value != null ? Convert.ToDateTime(v_worksheet.Cells[i, 16].Value) : (DateTime?)null;
                            DateTime? v_endPackingMonth = v_worksheet.Cells[i, 17].Value != null ? Convert.ToDateTime(v_worksheet.Cells[i, 17].Value) : (DateTime?)null;
                            DateTime? v_startProductionMonth = v_worksheet.Cells[i, 18].Value != null ? Convert.ToDateTime(v_worksheet.Cells[i, 18].Value) : (DateTime?)null;
                            DateTime? v_endProductionMonth = v_worksheet.Cells[i, 19].Value != null ? Convert.ToDateTime(v_worksheet.Cells[i, 19].Value) : (DateTime?)null;
                            flag_HasRecord = false;
                            for (int j = 21; j < countGrade + 21; j++)
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(v_worksheet.Cells[i, j].Value)) && Convert.ToString(v_worksheet.Cells[i, j].Value) != "0")
                                {
                                    flag_HasRecord = true;
                                    var row = new ImportCkdPartListDto();
                                    row.Guid = strGUID;
                                    /*                                    decimal eUsageQty = 0;
                                                                        decimal.TryParse(Convert.ToString(v_worksheet.Cells[i, j].Value), out eUsageQty);
                                                                        if (eUsageQty > 0)
                                                                        {*/
                                    row.Guid = strGUID;

                                    if (v_common.Length > 50) row.ErrorDescription += "Độ dài Comon : " + v_common + " không hợp lệ! , ";
                                    else row.Common = v_common;

                                    if (v_type.Length > 50) row.ErrorDescription += "Độ dài Type : " + v_type + " không hợp lệ! , ";
                                    else row.Type = v_type;

                                    if (v_cfc.Length > 4) row.ErrorDescription += "Độ dài Cfc : " + v_cfc + " không hợp lệ! , ";
                                    else row.Cfc = v_cfc;


                                    if (v_shop.Length > 1) row.ErrorDescription += "Độ dài Shop : " + v_shop + " không hợp lệ! , ";
                                    else row.Shop = v_shop;

                                    if (v_idLine.Length > 10) row.ErrorDescription += "Độ dài IdLine : " + v_idLine + " không hợp lệ! , ";
                                    else row.IdLine = v_idLine;


                                    if (v_partNo.Length > 50) row.ErrorDescription += "Độ dài PartNo : " + v_partNo + " không hợp lệ! , ";
                                    else row.PartNo = v_partNo;

                                    if (v_partName.Length > 500) row.ErrorDescription += "Độ dài PartName : " + v_partName + " không hợp lệ! , ";
                                    else row.PartName = v_partName;

                                    if (v_exporter.Length > 50) row.ErrorDescription += "Độ dài SupplierNo : " + v_exporter + " không hợp lệ! , ";
                                    else row.SupplierNo = v_exporter;

                                    if (v_exporterCode.Length > 50) row.ErrorDescription += "Độ dài SupplierCd : " + v_exporterCode + " không hợp lệ! , ";
                                    else row.SupplierCd = v_exporterCode;

                                    if (v_bodyColor.Length > 100) row.ErrorDescription += "Độ dài BodyColor : " + v_bodyColor + " không hợp lệ! , ";
                                    else row.BodyColor = v_bodyColor;

                                    if (v_backNo.Length > 10) row.ErrorDescription += "Độ dài BackNo : " + v_backNo + " không hợp lệ! , ";
                                    else row.BackNo = v_backNo;

                                    if (v_moduleNo.Length > 10) row.ErrorDescription += "Độ dài ModuleNo : " + v_moduleNo + " không hợp lệ! , ";
                                    else row.ModuleNo = v_moduleNo;

                                    if (v_renban.Length > 10) row.ErrorDescription += "Độ dài Renban : " + v_renban + " không hợp lệ! , ";
                                    else row.Renban = v_renban;

                                    //if (v_boxSize.GetType() == typeof(int)) row.ErrorDescription = "Kiểu dữ liệu BoxSize : " + v_boxSize + " không hợp lệ! , ";
                                    row.BoxSize = v_boxSize;

                                    if (v_reExporterCode.Length > 10) row.ErrorDescription += "Độ dài ReExportCd : " + v_renban + " không hợp lệ! , ";
                                    else row.ReExportCd = v_reExporterCode;

                                    if (v_icoFlag.Length > 50) row.ErrorDescription += "Độ dài IcoFlag : " + v_renban + " không hợp lệ! , ";
                                    else row.IcoFlag = v_icoFlag;


                                    try
                                    {
                                        row.StartPackingMonth = v_startPackingMonth;
                                    }

                                    catch
                                    {
                                        row.ErrorDescription += "StartPackingMonth không hợp lệ!";
                                    }
                                    //
                                    try
                                    {
                                        row.EndPackingMonth = v_endPackingMonth;
                                    }

                                    catch
                                    {
                                        row.ErrorDescription += "EndPackingMonth không hợp lệ!";
                                    }
                                    //
                                    try
                                    {
                                        row.StartProductionMonth = v_startPackingMonth;
                                    }

                                    catch
                                    {
                                        row.ErrorDescription += "StartProductionMonth không hợp lệ!";
                                    }
                                    //
                                    try
                                    {
                                        row.EndProductionMonth = v_endProductionMonth;
                                    }

                                    catch
                                    {
                                        row.ErrorDescription += "EndProductionMonth không hợp lệ!";
                                    }

                                    row.Grade = Convert.ToString(v_worksheet.Cells[7, j].Value);

                                    if (Convert.ToDecimal(v_worksheet.Cells[i, j].Value) < 0)
                                    {
                                        row.ErrorDescription += "Usage Qty không được âm";
                                    }
                                    else
                                    {
                                        row.UsageQty = Convert.ToDecimal(v_worksheet.Cells[i, j].Value);

                                    }

                                    row.Remark = v_remark;

                                    listImport.Add(row);
                                }


                            }
                            if (flag_HasRecord == false)
                            {
                                var row = new ImportCkdPartListDto();
                                row.Guid = strGUID;
                                if (v_common.Length > 50) row.ErrorDescription += "Độ dài Comon : " + v_common + " không hợp lệ! , ";
                                else row.Common = v_common;

                                if (v_type.Length > 50) row.ErrorDescription += "Độ dài Type : " + v_type + " không hợp lệ! , ";
                                else row.Type = v_type;

                                if (v_cfc.Length > 4) row.ErrorDescription += "Độ dài Cfc : " + v_cfc + " không hợp lệ! , ";
                                else row.Cfc = v_cfc;


                                if (v_shop.Length > 1) row.ErrorDescription += "Độ dài Shop : " + v_shop + " không hợp lệ! , ";
                                else row.Shop = v_shop;

                                if (v_idLine.Length > 10) row.ErrorDescription += "Độ dài IdLine : " + v_idLine + " không hợp lệ! , ";
                                else row.IdLine = v_idLine;


                                if (v_partNo.Length > 50) row.ErrorDescription += "Độ dài PartNo : " + v_partNo + " không hợp lệ! , ";
                                else row.PartNo = v_partNo;

                                if (v_partName.Length > 500) row.ErrorDescription += "Độ dài PartName : " + v_partName + " không hợp lệ! , ";
                                else row.PartName = v_partName;

                                if (v_exporter.Length > 50) row.ErrorDescription += "Độ dài SupplierNo : " + v_exporter + " không hợp lệ! , ";
                                else row.SupplierNo = v_exporter;

                                if (v_exporterCode.Length > 50) row.ErrorDescription += "Độ dài SupplierCd : " + v_exporterCode + " không hợp lệ! , ";
                                else row.SupplierCd = v_exporterCode;

                                if (v_bodyColor.Length > 100) row.ErrorDescription += "Độ dài BodyColor : " + v_bodyColor + " không hợp lệ! , ";
                                else row.BodyColor = v_bodyColor;

                                if (v_backNo.Length > 10) row.ErrorDescription += "Độ dài BackNo : " + v_backNo + " không hợp lệ! , ";
                                else row.BackNo = v_backNo;

                                if (v_moduleNo.Length > 10) row.ErrorDescription += "Độ dài ModuleNo : " + v_moduleNo + " không hợp lệ! , ";
                                else row.ModuleNo = v_moduleNo;

                                if (v_renban.Length > 10) row.ErrorDescription += "Độ dài Renban : " + v_renban + " không hợp lệ! , ";
                                else row.Renban = v_renban;

                                //if (v_boxSize.GetType() == typeof(int)) row.ErrorDescription = "Kiểu dữ liệu BoxSize : " + v_boxSize + " không hợp lệ! , ";
                                row.BoxSize = v_boxSize;

                                if (v_reExporterCode.Length > 10) row.ErrorDescription += "Độ dài ReExportCd : " + v_renban + " không hợp lệ! , ";
                                else row.ReExportCd = v_reExporterCode;

                                if (v_icoFlag.Length > 50) row.ErrorDescription += "Độ dài IcoFlag : " + v_renban + " không hợp lệ! , ";
                                else row.IcoFlag = v_icoFlag;


                                try
                                {
                                    row.StartPackingMonth = v_startPackingMonth;
                                }

                                catch
                                {
                                    row.ErrorDescription += "StartPackingMonth không hợp lệ!";
                                }
                                //
                                try
                                {
                                    row.EndPackingMonth = v_endPackingMonth;
                                }

                                catch
                                {
                                    row.ErrorDescription += "EndPackingMonth không hợp lệ!";
                                }
                                //
                                try
                                {
                                    row.StartProductionMonth = v_startPackingMonth;
                                }

                                catch
                                {
                                    row.ErrorDescription += "StartProductionMonth không hợp lệ!";
                                }
                                //
                                try
                                {
                                    row.EndProductionMonth = v_endProductionMonth;
                                }

                                catch
                                {
                                    row.ErrorDescription += "EndProductionMonth không hợp lệ!";
                                }

                                row.ErrorDescription = "";


                                row.Remark = v_remark;

                                listImport.Add(row);
                            }

                        }

                    }
                    // import temp into db (bulkCopy)
                    if (listImport.Count > 0)
                    {
                        IEnumerable<ImportCkdPartListDto> dataE = listImport.AsEnumerable();
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
                                    bulkCopy.DestinationTableName = "InvCkdPart_T";
                                    bulkCopy.ColumnMappings.Add("Guid", "Guid");
                                    bulkCopy.ColumnMappings.Add("Common", "Common");
                                    bulkCopy.ColumnMappings.Add("Type", "Type");
                                    bulkCopy.ColumnMappings.Add("Cfc", "Cfc");
                                    bulkCopy.ColumnMappings.Add("Shop", "Shop");
                                    bulkCopy.ColumnMappings.Add("IdLine", "IdLine");
                                    bulkCopy.ColumnMappings.Add("PartNo", "PartNo");
                                    bulkCopy.ColumnMappings.Add("PartName", "PartName");
                                    bulkCopy.ColumnMappings.Add("SupplierNo", "SupplierNo");
                                    bulkCopy.ColumnMappings.Add("SupplierCd", "SupplierCd");
                                    bulkCopy.ColumnMappings.Add("BodyColor", "BodyColor");
                                    bulkCopy.ColumnMappings.Add("BackNo", "BackNo");
                                    bulkCopy.ColumnMappings.Add("ModuleNo", "ModuleNo");
                                    bulkCopy.ColumnMappings.Add("Renban", "Renban");
                                    bulkCopy.ColumnMappings.Add("BoxSize", "BoxSize");
                                    bulkCopy.ColumnMappings.Add("ReExportCd", "ReExportCd");
                                    bulkCopy.ColumnMappings.Add("IcoFlag", "IcoFlag");
                                    bulkCopy.ColumnMappings.Add("StartPackingMonth", "StartPackingMonth");
                                    bulkCopy.ColumnMappings.Add("EndPackingMonth", "EndPackingMonth");
                                    bulkCopy.ColumnMappings.Add("StartProductionMonth", "StartProductionMonth");
                                    bulkCopy.ColumnMappings.Add("EndProductionMonth", "EndProductionMonth");
                                    bulkCopy.ColumnMappings.Add("Grade", "Grade");
                                    bulkCopy.ColumnMappings.Add("UsageQty", "UsageQty");

                                    bulkCopy.ColumnMappings.Add("Remark", "Remark");

                                    bulkCopy.ColumnMappings.Add("ErrorDescription", "ErrorDescription");

                                    bulkCopy.WriteToServer(table);
                                    tran.Commit();
                                }
                            }
                            await conn.CloseAsync();
                        }
                    }
                    return listImport;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(400, ex.Message);
            }
        }
        public async Task MergeDataInvCkdPartList(string v_Guid)
        {

            string _merge = "Exec INV_CKD_PART_LIST_MERGE @Guid";
            await _dapperRepo.QueryAsync<ImportCkdPartListDto>(_merge, new { Guid = v_Guid });
        }
        public async Task<PagedResultDto<ImportCkdPartListDto>> GetMessageErrorImport(string v_Guid, string v_screen)
        {
            string _sql = "Exec INV_CKD_PART_LIST_GET_LIST_ERROR_IMPORT @Guid, @Screen";

            IEnumerable<ImportCkdPartListDto> result = await _dapperRepo.QueryAsync<ImportCkdPartListDto>(_sql, new
            {
                Guid = v_Guid,
                Screen = v_screen
            });

            var listResult = result.ToList();
            var totalCount = listResult.Count();

            return new PagedResultDto<ImportCkdPartListDto>(
                totalCount,
               listResult
               );
        }
        public async Task<FileDto> GetListErrPartListToExcel(string v_Guid, string v_Screen)
        {
            FileDto a = new FileDto();
            string _sql = "Exec INV_CKD_PART_LIST_GET_LIST_ERROR_IMPORT @Guid, @Screen";

            IEnumerable<ImportCkdPartListDto> result = await _dapperRepo.QueryAsync<ImportCkdPartListDto>(_sql, new
            {
                Guid = v_Guid,
                Screen = v_Screen
            });

            var exportToExcel = result.ToList();
            if (v_Screen == "PxP")
            {
                a = _calendarListExcelExporter.ExportToFileErr(exportToExcel);
            }
            else if (v_Screen == "L")
            {
                a = _calendarListExcelExporter.ExportToFileLotErr(exportToExcel);
            }

            return a;

        }
        public async Task<List<ImportCkdPartListDto>> ImportDataInvCkdPartListLotFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<ImportCkdPartListDto> listImport = new List<ImportCkdPartListDto>();
                using (var stream = new MemoryStream(fileBytes))
                {
                    SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
                    var xlWorkBook = ExcelFile.Load(stream);
                    var v_worksheet = xlWorkBook.Worksheets[0];
                    DataFormatter formatter = new DataFormatter();
                    DateTime dateTime = DateTime.Now;

                    //string v_devanning_date = (v_worksheet.Cells[4, 2]).Value?.ToString() ?? "";
                    string strGUID = Guid.NewGuid().ToString("N");

                    string _sql = @"EXEC CMM_IMPORT_USER_GUID_INSERT @Guid, @p_UserId";
                    await _dapperRepo.ExecuteAsync(_sql, new
                    {
                        Guid = strGUID,
                        p_UserId = AbpSession.UserId
                    });

                    int countGrade = 0;

                    //Đếm số lượng bản ghi Grade
                    for (int i = 10; i <= 500; i++)
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(v_worksheet.Cells[0, i].Value)))
                        {
                            countGrade++;
                        }
                        else { break; }

                    }

                    bool flag_HasRecord = false;
                    for (int i = 2; i < v_worksheet.Rows.Count; i++)
                    {

                        string v_partNo = (v_worksheet.Cells[i, 4]).Value?.ToString().Replace("-", "") ?? "";

                        if (v_partNo != "")
                        {

                            string v_orderPattern = (v_worksheet.Cells[i, 0]).Value?.ToString() ?? "";
                            string v_model = (v_worksheet.Cells[i, 1]).Value?.ToString() ?? "";
                            string v_shop = (v_worksheet.Cells[i, 3]).Value?.ToString() ?? "";
                            string v_partName = (v_worksheet.Cells[i, 5]).Value?.ToString() ?? "";
                            string v_supplierNo = (v_worksheet.Cells[i, 8]).Value?.ToString() ?? "";

                            flag_HasRecord = false;
                            for (int j = 10; j < countGrade + 10; j++)
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(v_worksheet.Cells[i, j].Value)) && Convert.ToString(v_worksheet.Cells[i, j].Value) != "0")
                                {
                                    flag_HasRecord = true;
                                    var row = new ImportCkdPartListDto();
                                    row.Guid = strGUID;
                                    //decimal eUsageQty = 0;
                                    //decimal.TryParse(Convert.ToString(v_worksheet.Cells[i, j].Value), out eUsageQty);

                                    row.Guid = strGUID;

                                    if (v_orderPattern.Length > 50) row.ErrorDescription = "Độ dài Order Pattern : " + v_orderPattern + " không hợp lệ! , ";
                                    else row.OrderPattern = v_orderPattern.ToUpper();

                                    if (v_model.Length > 4) row.ErrorDescription = "Độ dài Model : " + v_model + " không hợp lệ! , ";
                                    else row.Cfc = v_model;



                                    if (v_shop.Length > 1) row.ErrorDescription = "Độ dài Shop : " + v_shop + " không hợp lệ! , ";
                                    else row.Shop = v_shop;

                                    if (v_partNo.Length > 50) row.ErrorDescription = "Độ dài PartNo : " + v_partNo + " không hợp lệ! , ";
                                    else row.PartNo = v_partNo;

                                    if (v_partName.Length > 500) row.ErrorDescription = "Độ dài PartName : " + v_partName + " không hợp lệ! , ";
                                    else row.PartName = v_partName;


                                    if (v_supplierNo.Length > 100) row.ErrorDescription = "Độ dài Source : " + v_supplierNo + " không hợp lệ! , ";
                                    else row.SupplierNo = v_supplierNo;



                                    row.Grade = Convert.ToString(v_worksheet.Cells[0, j].Value);

                                    if (Convert.ToDecimal(v_worksheet.Cells[i, j].Value) < 0)
                                    {
                                        row.ErrorDescription = "Usage Qty không được âm";
                                    }
                                    else
                                    {
                                        row.UsageQty = Convert.ToDecimal(v_worksheet.Cells[i, j].Value);

                                    }


                                    listImport.Add(row);


                                }
                            }
                            if (flag_HasRecord == false)
                            {
                                var row = new ImportCkdPartListDto();
                                row.Guid = strGUID;
                                if (v_orderPattern.Length > 50) row.ErrorDescription = "Độ dài Order Pattern : " + v_orderPattern + " không hợp lệ! , ";
                                else row.OrderPattern = v_orderPattern.ToUpper();

                                if (v_model.Length > 50) row.ErrorDescription = "Độ dài Model : " + v_model + " không hợp lệ! , ";
                                else row.Cfc = v_model;

                                if (v_shop.Length > 1) row.ErrorDescription = "Độ dài Shop : " + v_shop + " không hợp lệ! , ";
                                else row.Shop = v_shop;


                                if (v_partNo.Length > 50) row.ErrorDescription = "Độ dài PartNo : " + v_partNo + " không hợp lệ! , ";
                                else row.PartNo = v_partNo;

                                if (v_partName.Length > 500) row.ErrorDescription = "Độ dài PartName : " + v_partName + " không hợp lệ! , ";
                                else row.PartName = v_partName;

                                if (v_supplierNo.Length > 100) row.ErrorDescription = "Độ dài Source : " + v_supplierNo + " không hợp lệ! , ";
                                else row.SupplierNo = v_supplierNo;


                                listImport.Add(row);
                            }

                        }

                    }
                    if (listImport.Count > 0)
                    {
                        IEnumerable<ImportCkdPartListDto> dataE = listImport.AsEnumerable();
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
                                    bulkCopy.DestinationTableName = "InvCkdPart_T";
                                    bulkCopy.ColumnMappings.Add("Guid", "Guid");
                                    bulkCopy.ColumnMappings.Add("OrderPattern", "OrderPattern");
                                    bulkCopy.ColumnMappings.Add("Cfc", "Cfc");
                                    bulkCopy.ColumnMappings.Add("Shop", "Shop");
                                    bulkCopy.ColumnMappings.Add("PartNo", "PartNo");
                                    bulkCopy.ColumnMappings.Add("PartName", "PartName");
                                    bulkCopy.ColumnMappings.Add("SupplierNo", "SupplierNo");

                                    bulkCopy.ColumnMappings.Add("Grade", "Grade");
                                    bulkCopy.ColumnMappings.Add("UsageQty", "UsageQty");

                                    bulkCopy.ColumnMappings.Add("ErrorDescription", "ErrorDescription");

                                    bulkCopy.WriteToServer(table);
                                    tran.Commit();
                                }
                            }
                            await conn.CloseAsync();
                        }
                    }
                    return listImport;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(400, ex.Message);
            }
        }
     /*   public async Task<List<InvCkdPartGradeDto>> ImportDataInvCkdPartGradeFromExcel(byte[] fileBytes, string fileName)
        {
            
            try
            {
                List<InvCkdPartGradeDto> listImport = new List<InvCkdPartGradeDto>();
                using (var stream = new MemoryStream(fileBytes))
                {
                    SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
                    var xlWorkBook = ExcelFile.Load(stream);
                    var v_worksheet = xlWorkBook.Worksheets[0];

                    string strGUID = Guid.NewGuid().ToString("N");

                    string _sql = @"EXEC CMM_IMPORT_USER_GUID_INSERT @Guid, @p_UserId";
                    _dapperRepo.ExecuteAsync(_sql, new
                    {
                        Guid = strGUID,
                        p_UserId = AbpSession.UserId
                    });

                    for (int i = 4; i < v_worksheet.Rows.Count; i++)
                    {
                        string v_partNo = (v_worksheet.Cells[i, 5]).Value?.ToString().Replace("-", "") ?? "";

                        if (v_partNo != "")
                        {
                            string v_maintenaceCode = (v_worksheet.Cells[i, 0]).Value?.ToString() ?? "";
                            string v_partStatus = (v_worksheet.Cells[i, 1]).Value?.ToString() ?? "";
                            string v_model = (v_worksheet.Cells[i, 2]).Value?.ToString() ?? "";
                            string v_shop = (v_worksheet.Cells[i, 3]).Value?.ToString() ?? "";
                            string v_idLine = (v_worksheet.Cells[i, 4]).Value?.ToString() ?? "";
                            string v_partName = (v_worksheet.Cells[i, 6]).Value?.ToString() ?? "";
                            string v_bodyColor = (v_worksheet.Cells[i, 9]).Value?.ToString() ?? "";
                            string v_grade = (v_worksheet.Cells[i, 10]).Value?.ToString() ?? "";
                            string v_usageQty = (v_worksheet.Cells[i, 11]).Value?.ToString() ?? "";
                            string v_startLot = (v_worksheet.Cells[i, 12]).Value?.ToString() ?? "";
                            string v_startNoInLot = (v_worksheet.Cells[i, 13]).Value?.ToString() ?? "";
                            string v_endLot = (v_worksheet.Cells[i, 14]).Value?.ToString() ?? "";
                            string v_endNoInLot = (v_worksheet.Cells[i, 15]).Value?.ToString() ?? "";
                            string v_eciFromPart = (v_worksheet.Cells[i, 16]).Value?.ToString() ?? "";
                            string v_eciToPart = (v_worksheet.Cells[i, 17]).Value?.ToString() ?? "";
                            string v_orderPattern = (v_worksheet.Cells[i, 18]).Value?.ToString() ?? "";
                            string v_updateDate = (v_worksheet.Cells[i, 20]).Value?.ToString() ?? "";
                            string v_updateUser = (v_worksheet.Cells[i, 21]).Value?.ToString() ?? "";

                            var row = new InvCkdPartGradeDto();
                            row.Guid = strGUID;
                            row.ErrorDescription = "";
                            row.PartNo = v_partNo;
                            row.PartName = v_partName;
                            row.Shop = v_shop;
                            //check Smqd Date
                            row.BodyColor = v_bodyColor;
                            row.IdLine = v_idLine;
                            row.Grade = v_grade;
                            row.StartLot = v_startLot;
                            row.EndLot = v_endLot;
                            listImport.Add(row);

                            listImport.Add(row);
                        }
                    }
                    // import temp into db (bulkCopy)
                    if (listImport.Count > 0)
                    {
                        IEnumerable<InvCkdPartGradeDto> dataE = listImport.AsEnumerable();
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
                                    bulkCopy.DestinationTableName = "InvCkdGrade_T";
                                    bulkCopy.ColumnMappings.Add("Guid", "Guid");
                                    bulkCopy.ColumnMappings.Add("PartNo", "PartNo");
                                    bulkCopy.ColumnMappings.Add("PartName", "PartName");
                                    bulkCopy.ColumnMappings.Add("Model", "Model");
                                    bulkCopy.ColumnMappings.Add("Cfc", "Cfc");
                                    bulkCopy.ColumnMappings.Add("Grade", "Grade");
                                    bulkCopy.ColumnMappings.Add("IdLine", "IdLine");
                                    bulkCopy.ColumnMappings.Add("Shop", "Shop");
                                    bulkCopy.ColumnMappings.Add("BodyColor", "BodyColor");
                                    bulkCopy.ColumnMappings.Add("UsageQty", "UsageQty");
                                    bulkCopy.ColumnMappings.Add("StartLot", "StartLot");
                                    bulkCopy.ColumnMappings.Add("EndLot", "EndLot");
                                    bulkCopy.ColumnMappings.Add("ErrorDescription", "ErrorDescription");
                                    bulkCopy.WriteToServer(table);
                                    tran.Commit();
                                }
                            }
                            await conn.CloseAsync();
                        }
                    }
                    return listImport;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(400, ex.Message);
            }
        }
        */
        public async Task<List<GetPartNoDto>> GetListPartNo()
        {
            IEnumerable<GetPartNoDto> result = await _dapperRepo.QueryAsync<GetPartNoDto>("SELECT DISTINCT PartNo FROM InvCkdPartList");
            return result.ToList();
        }

        public async Task<List<GetCfcDto>> GetListCfcs()
        {
            IEnumerable<GetCfcDto> result = await _dapperRepo.QueryAsync<GetCfcDto>(@"exec [dbo].[MST_CMM_LOT_CODE_GRADE_CFC_GETS] ");
            return result.ToList();
        }

        public async Task<List<GetGradeDto>> GetListGrades()
        {
            IEnumerable<GetGradeDto> result = await _dapperRepo.QueryAsync<GetGradeDto>(@"exec [dbo].[MST_CMM_LOT_CODE_GRADE_GRADE_GETS] ");
            return result.ToList();
        }

        public async Task<List<GetGradeDto>> GetListGradesByCfc(string cfc)
        {
            IEnumerable<GetGradeDto> result = await _dapperRepo.QueryAsync<GetGradeDto>("Exec MST_CMM_LOT_CODE_GRADE_GET_BY_CFC @p_Cfc", new { p_Cfc = cfc });
            return result.ToList();
        }

        public async Task<List<GetColorDto>> GetListColors(string cfc, string grade)
        {
            string _sql = "Exec MST_CMM_LOT_CODE_GRADE_COLOR_BY_GRADE @cfc, @grade";
            IEnumerable<GetColorDto> result = await _dapperRepo.QueryAsync<GetColorDto>(_sql, new
            {
                cfc = cfc,
                grade = grade
            });

            return result.ToList();
        }

        public async Task<List<GetSupplierDto>> GetListSupplierNos()
        {
            IEnumerable<GetSupplierDto> result = await _dapperRepo.QueryAsync<GetSupplierDto>(@"exec [dbo].[MST_INV_SUPPLIER_LIST_GETS]");
            return result.ToList();
        }

        public async Task<PagedResultDto<CheckExistDto>> CheckExistPartNo(string PartNo, string Cfc)
        {
            string _sqlSearch = "Exec INV_CKD_PARTLIST_EXIST_PARTNO @partno, @cfc";

            IEnumerable<CheckExistDto> result = await _partGrade.QueryAsync<CheckExistDto>(_sqlSearch, new { partno = PartNo, cfc = Cfc });
            var listResult = result.ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<CheckExistDto>(
                totalCount,
                result.ToList());
        }

        public async Task<PagedResultDto<CheckExistDto>> CheckExistBodyColor(ValidatePartGradeBodyColorInput input)
        {
            string _sqlSearch = "Exec INV_CKD_PART_GRADE_CHECK_EXIST_BODYCOLOR @p_Id, @p_Cfc, @p_Shop, @p_Model, @p_Grade, @p_BodyColor";

            IEnumerable<CheckExistDto> result = await _partGrade.QueryAsync<CheckExistDto>(_sqlSearch, new
            {
                p_Id = input.Id,
                p_Cfc = input.Cfc,
                p_Shop = input.Shop,
                p_Model = input.Model,
                p_Grade = input.Grade,
                p_BodyColor = input.BodyColor
            });
            var listResult = result.ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<CheckExistDto>(
                totalCount,
                result.ToList());
        }

        [AbpAuthorize(AppPermissions.Pages_Ckd_Master_PartList_Add)]
        public async Task PartListInsert(GetPartListGradeDto input)
        {
            var result = (await _dapperRepo.QueryAsync<GetPartListId>(@"
                   exec [dbo].[INV_CKD_PART_LIST_INSERT]
                                            @partNo,
                                            @partName,
                                            @cfc,
                                            @supplierNo ,
                                            @orderPattern,
                                            @p_UserId
                ", new
            {
                partNo = input.PartNo,
                partName = input.PartName,
                cfc = input.Cfc,
                supplierNo = input.SupplierNo,
                orderPattern = input.OrderPattern,
                p_UserId = AbpSession.UserId
            })).FirstOrDefault();

            if (result != null)
            {
                for (int i = 0; i < input.listGrade.Count; i++)
                {
                    await _dapperRepo.ExecuteAsync(@"
                      exec [dbo].[INV_CKD_PART_GRADE_INSERT]                                      
                                            @partId,
                                            @partNo,
                                            @cfc,
                                            @model,
                                            @grade,
                                            @usageQty,
                                            @shop,
                                            @bodyColor,
                                            @startLot,
                                            @endLot,
                                            @startRun,
                                            @endRun,
                                            @efFromPart,
                                            @efToPart,
                                            @orderPattern,
                                            @remark,
                                            @p_UserId
                                       
                    ", new
                    {
                        partId = result.PartListId,
                        partNo = input.listGrade[i].PartNo,
                        cfc = input.Cfc,
                        model = input.listGrade[i].Model,
                        grade = input.listGrade[i].Grade,
                        usageQty = input.listGrade[i].UsageQty,
                        shop = input.listGrade[i].Shop,
                        bodyColor = input.listGrade[i].BodyColor,
                        startLot = input.listGrade[i].StartLot,
                        endLot = input.listGrade[i].EndLot,
                        startRun = input.listGrade[i].StartRun,
                        endRun = input.listGrade[i].EndRun,
                        efFromPart = input.listGrade[i].EfFromPart,
                        efToPart = input.listGrade[i].EfToPart,
                        orderPattern = input.listGrade[i].OrderPattern,
                        remark = input.listGrade[i].Remark,
                        p_UserId = AbpSession.UserId
                    });
                }
            }
            else
            {
                throw new UserFriendlyException(400, "PartNo is exist in Cfc");
            }
        }


        [AbpAuthorize(AppPermissions.Pages_Ckd_Master_PartList_Add)]
        public async Task PartListEdit(GetPartListGradeDto input)
        {
            string listGradeSelected = "";
            await _dapperRepo.ExecuteAsync(@"
                   exec [dbo].[INV_CKD_PART_LIST_EDIT]
                                            @id,
                                            @partNo,
                                            @partName,
                                            @cfc,
                                            @supplierNo ,
                                            @orderPattern,
                                            @p_UserId 
            ", new
            {
                id = input.Id,
                partNo = input.PartNo,
                partName = input.PartName,
                cfc = input.Cfc,
                supplierNo = input.SupplierNo,
                orderPattern = input.OrderPattern,
                p_UserId = AbpSession.UserId
            });

            for (int i = 0; i < input.listGrade.Count; i++)
            {
                listGradeSelected = listGradeSelected + "," + input.listGrade[i].Grade;
            }

            for (int i = 0; i < input.listGrade.Count; i++)
            {

                await _dapperRepo.ExecuteAsync(@"
                  exec [dbo].[INV_CKD_PART_GRADE_EDITS]  
                                        @id,
                                        @partId,
                                        @partNo,
                                        @cfc,
                                        @model,
                                        @grade,
                                        @usageQty,
                                        @shop,
                                        @startLot,
                                        @endLot,
                                        @startRun,
                                        @endRun,
                                        @efFromPart,
                                        @efToPart,
                                        @orderPattern,
                                        @remark,
                                        @listGrade,
                                        @p_UserId
                ", new
                {
                    id = input.listGrade[i].Id,
                    partId = input.listGrade[i].PartId,
                    partNo = input.listGrade[i].PartNo,
                    cfc = input.Cfc,
                    model = input.listGrade[i].Model,
                    grade = input.listGrade[i].Grade,
                    usageQty = input.listGrade[i].UsageQty,
                    shop = input.listGrade[i].Shop,
                    startLot = input.listGrade[i].StartLot,
                    endLot = input.listGrade[i].EndLot,
                    startRun = input.listGrade[i].StartRun,
                    endRun = input.listGrade[i].EndRun,
                    efFromPart = input.listGrade[i].EfFromPart,
                    efToPart = input.listGrade[i].EfToPart,
                    orderPattern = input.listGrade[i].OrderPattern,
                    remark = input.listGrade[i].Remark,
                    listGrade = listGradeSelected,
                    p_UserId = AbpSession.UserId
                }); ;
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Ckd_Master_PartList_Add)]
        public async Task PartGradeEdit(InvCkdPartGradeDto input)
        {
            await _dapperRepo.ExecuteAsync(@"
                  exec [dbo].[INV_CKD_PART_GRADE_EDIT]  
                                @id,
                                @usageQty,
                                @shop,
                                @grade,
                                @bodyColor,
                                @startLot,
                                @endLot,
                                @startRun,
                                @endRun,
                                @eciFromPart,
                                @eciToPart,
                                @orderPattern,
                                @remark,
                                @p_UserId
                                                                         
                ", new
            {
                id = input.Id,
                usageQty = input.UsageQty,
                shop = input.Shop,
                grade = input.Grade,
                bodyColor = input.BodyColor,
                startLot = input.StartLot,
                endLot = input.EndLot,
                startRun = input.StartRun,
                endRun = input.EndRun,
                eciFromPart = input.ECIFromPart,
                eciToPart = input.ECIToPart,
                orderPattern = input.OrderPattern,
                remark = input.Remark,
                p_UserId = AbpSession.UserId
            });
        }

        [AbpAuthorize(AppPermissions.Pages_Ckd_Master_PartList_Add)]
        public async Task<int> PartGradeDel(long? v_id)
        {
            try
            {
                string _sql = @"EXEC INV_CKD_PART_GRADE_DELETE @id ,@p_UserId";
                var filtered = await _dapperRepo.ExecuteAsync(_sql, new
                {
                    id = v_id,
                    p_UserId = AbpSession.UserId
                });
                return filtered;
            }
            catch (Exception E)
            {
                return 0;
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Ckd_Master_PartList_Add)]
        public async Task EciPartGrade(long? v_id, string v_StartLot, string v_StartRun, string v_Grade)
        {
            await _dapperRepo.ExecuteAsync(@" exec [dbo].[INV_CKD_PART_GRADE_ECI] @id, @p_StartLot, @p_StartRun, @p_Grade , @p_UserId", new
            {
                id = v_id,
                p_StartLot = v_StartLot,
                p_StartRun = v_StartRun,
                p_Grade = v_Grade,
                p_UserId = AbpSession.UserId
            });
        }

        private async Task<List<ColumnChange>> GetChangedColumn(int id, string tableName)
        {
            var result = new List<ColumnChange>();
            string connectionString = "Server=tcp:192.168.6.20,1433;Database=MMS_TEST;User Id=sa;Password=Prod@mms@20@##;Trust Server Certificate=true;";

            using (SqlConnection connection = new(connectionString))
            {
                connection.Open();

                // Execute a simple query
                var queryBuilder = new StringBuilder("SELECT distinct __$command_id ,\r\n        ( SELECT    CC.column_name + ','\r\n          FROM      cdc.captured_columns CC\r\n                    INNER JOIN cdc.change_tables CT ON CC.[object_id] = CT.[object_id]\r\n          WHERE     capture_instance = 'dbo_");
                queryBuilder.Append(tableName);
                queryBuilder.Append("'                     AND sys.fn_cdc_is_bit_set(CC.column_ordinal,\r\n                                              PD.__$update_mask) = 1\r\n        FOR\r\n          XML PATH('')\r\n        ) AS changedcolumns\r\nFROM cdc.dbo_");
                queryBuilder.Append(tableName);
                queryBuilder.Append("_CT PD WHERE Id = ");
                queryBuilder.Append(id);
                string query = queryBuilder.ToString();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                    {
                        // Process the results
                        while (reader.Read())
                        {
                            result.Add(new ColumnChange
                            {
                                Id = id,
                                TableName = tableName,
                                CommandId = (int)reader["__$command_id"],
                                ColumnChanged = reader["changedcolumns"].ToString(),
                            });
                        }
                        connection.Close();
                    }
                }
            }
            return result;
        }

        public class ColumnChange
        {
            public int Id { get; set; }
            public string TableName { get; set; }
            public int CommandId { get; set; }
            public string ColumnChanged { get; set; }
        }


        // New Part List Export Details format

        public async Task<FileDto> GetCkdPartExportDetailsToFile(InvCkdPartListDetailsExportInput input)
        {
            var file = new FileDto("CKDPartListDetails.xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
            string fileName = "temp_CKDPartListDetails";
            string template = "wwwroot/Template";
            string path = "";
            path = Path.Combine(Directory.GetCurrentDirectory(), template, fileName);
            var xlWorkBook = ExcelFile.Load(path + ".xlsx");
            var workSheet = xlWorkBook.Worksheets[0];

            List<string> listExport = new List<string>();
            listExport.Add("PartStatus");
            listExport.Add("Cfc");
            listExport.Add("Shop");
            listExport.Add("IdLine");
            listExport.Add("PartNo");
            listExport.Add("PartName");
            listExport.Add("Exporter");
            listExport.Add("ExporterCode");
            listExport.Add("BodyColor");
            listExport.Add("Grade");
            listExport.Add("UsageQty");
            listExport.Add("StartLot");
            listExport.Add("StartNoInLot");
            listExport.Add("EndLot");
            listExport.Add("EndNoInLot");
            listExport.Add("ECIFromPart");
            listExport.Add("ECIToPart");
            listExport.Add("OrderPattern");
            listExport.Add("Remark");
            listExport.Add("UserUpdate");
            listExport.Add("UserName");

            string[] properties = listExport.ToArray();
            string _sql = "Exec INV_CKD_PART_GRADE_BY_PARTNO_EXPORT @part_no,@cfc, @p_model, @p_grade, @p_shop,@supplierNo,@p_order_pattern, @p_active";
            var listData = (await _dapperRepo.QueryAsync<InvCkdPartDetailsDto>(_sql, new
            {
                part_no = input.PartNo,
                cfc = input.Cfc,
                p_model = input.Model,
                p_grade = input.Grade,
                p_shop = input.Shop,
                supplierNo = input.SupplierNo,
                p_order_pattern = input.OrderPattern,
                p_active = input.IsActive
            })).ToList();


            DataTable table = new DataTable();
            using (var reader = ObjectReader.Create(listData, properties))
            {
                table.Load(reader);
            }

            if (table.Rows.Count > 0)
            {
                InsertDataTableOptions ins = new InsertDataTableOptions(2, 0);
                workSheet.InsertDataTable(table, ins);
            }


            var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx");
            xlWorkBook.Save(tempFile);
            MemoryStream obj_stream = new MemoryStream();
            obj_stream = new MemoryStream(File.ReadAllBytes(tempFile));
            _tempFileCacheManager.SetFile(file.FileToken, obj_stream.ToArray());
            File.Delete(tempFile);
            obj_stream.Position = 0;
            return file;
        }



        public async Task<FileDto> GetCkdPartExportNormalToFile(InvCkdPartListDetailsExportInput input)
        {
            var file = new FileDto("CKDPartListNormal.xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
            string fileName = "temp_CKDPartListNormal";
            string template = "wwwroot/Template";
            string path = "";
            path = Path.Combine(Directory.GetCurrentDirectory(), template, fileName);
            var xlWorkBook = ExcelFile.Load(path + ".xlsx");
            var workSheet = xlWorkBook.Worksheets[0];

            List<string> listExport = new List<string>();
            listExport.Add("PartStatus");
            listExport.Add("Model");
            listExport.Add("Shop");
            listExport.Add("IdLine");
            listExport.Add("PartNo");
            listExport.Add("PartName");
            listExport.Add("Exporter");
            listExport.Add("ExporterCode");
            listExport.Add("BodyColor");
            listExport.Add("BackNo");
            listExport.Add("ModuleNo");
            listExport.Add("Renban");
            listExport.Add("BoxSize");
            listExport.Add("ReExportCd");
            listExport.Add("IcoFlag");
            listExport.Add("StartPackingMonth");
            listExport.Add("EndPackingMonth");
            listExport.Add("Value");
            // listExport.Add("Remark");
            //listExport.Add("UserUpdate");
            //listExport.Add("UserName");



            string[] properties = listExport.ToArray();
            string _sql = "Exec INV_CKD_PART_LIST_GET_BY_PARTNO_EXPORT @part_no,@cfc, @p_model, @p_grade, @p_shop,@supplierNo,@p_order_pattern,@p_active";
            var listDataHeader = (await _dapperRepo.QueryAsync<InvCkdPartNormalDto>(_sql, new
            {
                part_no = input.PartNo,
                cfc = input.Cfc,
                p_model = input.Model,
                p_grade = input.Grade,
                p_shop = input.Shop,
                supplierNo = input.SupplierNo,
                p_order_pattern = input.OrderPattern,
                p_active = input.IsActive
            })).ToList();

            _sql = "Exec INV_CKD_PART_GRADE_BY_GRADE @p_part_no";
            var listGrade = (await _partGrade.QueryAsync<ExportCkdPartListGradeDto>(_sql, new { p_part_no = input.PartNo })).ToList();


            DataTable table = new DataTable();
            using (var reader = ObjectReader.Create(listDataHeader, properties))
            {
                table.Load(reader);
            }
            var style = new CellStyle();
            style.Borders.SetBorders(MultipleBorders.All, SpreadsheetColor.FromArgb(0, 0, 0), GemBox.Spreadsheet.LineStyle.Thin);
            style.FillPattern.SetSolid(SpreadsheetColor.FromArgb(255, 255, 102));

            var styleheader = new CellStyle();
            styleheader.Borders.SetBorders(MultipleBorders.All, SpreadsheetColor.FromArgb(0, 0, 0), GemBox.Spreadsheet.LineStyle.Thin);
            styleheader.FillPattern.SetSolid(SpreadsheetColor.FromArgb(89, 89, 89));
            styleheader.Font.Color = SpreadsheetColor.FromName(ColorName.White);
            styleheader.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            styleheader.VerticalAlignment = VerticalAlignmentStyle.Center;


            for (int i = 0; i < listGrade.Count; i++)
            {
                table.Columns.Add(listGrade[i].Grade, typeof(int));
                workSheet.Cells[0, 17 + i].Value = listGrade[i].Grade;
                workSheet.Cells[0, 17 + i].Style = styleheader;
            }

            workSheet.Cells[0, 17 + listGrade.Count].Value = "Remark";
            workSheet.Cells[0, 17 + listGrade.Count].Style = styleheader;
            workSheet.Cells[0, 17 + listGrade.Count].Column.Width = 2400;
            workSheet.Cells[0, 18 + listGrade.Count].Value = "Update date";
            workSheet.Cells[0, 18 + listGrade.Count].Style = styleheader;
            workSheet.Cells[0, 18 + listGrade.Count].Column.Width = 4200;
            workSheet.Cells[0, 19 + listGrade.Count].Value = "Update user";
            workSheet.Cells[0, 19 + listGrade.Count].Style = styleheader;
            workSheet.Cells[0, 19 + listGrade.Count].Column.Width = 4200;



            for (int i = 0; i < table.Rows.Count; i++)
            {
                DataRow dr = table.Rows[i];
                for (int j = 0; j < listGrade.Count; j++)

                {
                    foreach (string x in dr["Value"].ToString().Split(","))
                    {
                        string a = x.Split("-")[0];
                        if (x != null && x != "" && x != "-")
                            if (listGrade[j].Grade == a)
                            {
                                dr[x.Split("-")[0]] = x.Split("-")[1] == "" ? 0 : int.Parse(x.Split("-")[1]);
                            }

                    }
                }

            }

            if (table.Rows.Count > 0)
            {
                table.Columns.Remove("Value");
              /*  table.Columns["Remark"].ColumnName = "Remark";
                table.Columns["UserUpdate"].ColumnName = "Update date";*/
               // table.Columns["UserName"].SetOrdinal(16 + listGrade.Count + 3);
                InsertDataTableOptions ins = new InsertDataTableOptions(2, 0);
                workSheet.InsertDataTable(table, ins);
            }


            var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx");
            xlWorkBook.Save(tempFile);
            MemoryStream obj_stream = new MemoryStream();
            obj_stream = new MemoryStream(File.ReadAllBytes(tempFile));
            _tempFileCacheManager.SetFile(file.FileToken, obj_stream.ToArray());
            File.Delete(tempFile);
            obj_stream.Position = 0;
            return file;
        }



        // Import Normal 

        [AbpAuthorize(AppPermissions.Pages_Ckd_Master_PartList_Import)]
        public async Task<List<ImportCkdPartListNormalDto>> ImportDataInvCkdPartListNormalFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<ImportCkdPartListNormalDto> listImport = new List<ImportCkdPartListNormalDto>();
                using (var stream = new MemoryStream(fileBytes))
                {
                    SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
                    var xlWorkBook = ExcelFile.Load(stream);
                    var v_worksheet = xlWorkBook.Worksheets[0];
                    DataFormatter formatter = new DataFormatter();
                    DateTime dateTime = DateTime.Now;

                    string strGUID = Guid.NewGuid().ToString("N");

                    //string _sql = @"EXEC CMM_IMPORT_USER_GUID_INSERT @Guid, @p_UserId";
                    //await _dapperRepo.ExecuteAsync(_sql, new
                    //{
                    //    Guid = strGUID,
                    //    p_UserId = AbpSession.UserId
                    //});



                    for (int i = 3; i <= v_worksheet.Rows.Count; i++)
                    {

                        string v_partNo = (v_worksheet.Cells[i, 5]).Value?.ToString().Replace("-", "") ?? "";

                        if (v_partNo != "")
                        {
                            string v_manintenance = (v_worksheet.Cells[i, 0]).Value?.ToString() ?? "";
                            string v_partStatus = (v_worksheet.Cells[i, 1]).Value?.ToString() ?? "";
                            string v_model = (v_worksheet.Cells[i, 2]).Value?.ToString() ?? "";
                            string v_shop = (v_worksheet.Cells[i, 3]).Value?.ToString() ?? "";
                            string v_IdLine = (v_worksheet.Cells[i, 4]).Value?.ToString() ?? "";
                            string v_partName = (v_worksheet.Cells[i, 6]).Value?.ToString() ?? "";
                            string v_supplierNo = (v_worksheet.Cells[i, 7]).Value?.ToString() ?? "";
                            string v_supplierCd = (v_worksheet.Cells[i, 8]).Value?.ToString() ?? "";
                            string v_bodycolor = (v_worksheet.Cells[i, 9]).Value?.ToString() ?? "";
                            string v_backNo = (v_worksheet.Cells[i, 10]).Value?.ToString() ?? "";
                            string v_moduleNo = (v_worksheet.Cells[i, 11]).Value?.ToString() ?? "";
                            string v_renban = (v_worksheet.Cells[i, 12]).Value?.ToString() ?? "";
                            string v_boxsize = (v_worksheet.Cells[i, 13]).Value?.ToString() ?? "";
                            string v_reExporterCode = (v_worksheet.Cells[i, 14]).Value?.ToString() ?? "";
                            string v_IcoFlag = (v_worksheet.Cells[i, 15]).Value?.ToString() ?? "";
                            string v_startPackingMonth = (v_worksheet.Cells[i, 16]).Value?.ToString() ?? "";
                            string v_endPackingMonth = (v_worksheet.Cells[i, 17]).Value?.ToString() ?? "";
                            string v_remark = (v_worksheet.Cells[i, 57]).Value?.ToString() ?? "";

                            string[] bodyColorArray = v_bodycolor.Split(",");

                            for (int j = 0; j < bodyColorArray.Length; j++)
                            {

                                for (var k = 18; k <= 56; k++)
                                {
                                    var row = new ImportCkdPartListNormalDto();
                                    row.Guid = strGUID;

                                    row.MaintenanceCode = v_manintenance;
                                    row.IsActive = (v_partStatus == "Active" ? "Y" : "N");
                                    if (v_model.Length > 4) row.ErrorDescription += "Độ dài Order Model : " + v_model + " không hợp lệ! , ";
                                    else row.Cfc = v_model;

                                    if (v_shop.Length > 1) row.ErrorDescription += "Độ dài Shop : " + v_shop + " không hợp lệ! , ";
                                    else row.Shop = v_shop;

                                    if (v_IdLine.Length > 10) row.ErrorDescription += "Độ dài Idline : " + v_IdLine + " không hợp lệ! , ";
                                    else row.IdLine = v_IdLine;


                                    if (string.IsNullOrEmpty(v_partNo))
                                    {
                                        row.ErrorDescription += " PartNo không được để trống! ";
                                    }
                                    else
                                    {
                                        if (v_partNo.Length > 50)
                                        {
                                            row.ErrorDescription += "Độ dài PartNo : " + v_partNo + " không hợp lệ! , ";
                                        }
                                        else
                                        {
                                            row.PartNo = v_partNo;
                                        }
                                    }


                                    if (v_partName.Length > 500) row.ErrorDescription += "Độ dài PartName : " + v_partName + " không hợp lệ! , ";
                                    else row.PartName = v_partName;

                                    if (v_supplierNo.Length > 50) row.ErrorDescription += "Độ dài Exporter : " + v_supplierNo + " không hợp lệ! , ";
                                    else row.SupplierNo = v_supplierNo;

                                    if (v_supplierCd.Length > 50) row.ErrorDescription += "Độ dài Exporter Code : " + v_supplierCd + " không hợp lệ! , ";
                                    else row.SupplierCd = v_supplierCd;

                                    row.BodyColor = bodyColorArray[j];

                                    if (v_backNo.Length > 10) row.ErrorDescription += "Độ dài Back no : " + v_supplierCd + " không hợp lệ! , ";
                                    else row.BackNo = v_backNo;


                                    if (v_renban.Length > 10) row.ErrorDescription += "Độ dài Reban : " + v_renban + " không hợp lệ! , ";
                                    else row.Renban = v_renban;

                                    if (v_moduleNo.Length > 10) row.ErrorDescription += "Độ dài Module No : " + v_moduleNo + " không hợp lệ! , ";
                                    else row.ModuleNo = v_moduleNo;

                                    try
                                    {
                                        row.BoxSize = Convert.ToInt32(v_boxsize);
                                        if (row.BoxSize < 0)
                                        {
                                            row.ErrorDescription += "BoxSize phải là số dương! ";
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        row.ErrorDescription += "BoxSize phải là số! ";
                                    }
                                    row.ReExportCd = v_reExporterCode;
                                    row.IcoFlag = v_IcoFlag;

                                    try
                                    {
                                        if (v_startPackingMonth != "")
                                        {
                                            row.StartPackingMonth = DateTime.Parse(v_startPackingMonth);
                                        }

                                    }
                                    catch
                                    {
                                        row.ErrorDescription += "Ngày StartPackingMonth  không hợp lệ! ";
                                    }

                                    try
                                    {
                                        if (v_endPackingMonth != "")
                                        {
                                            row.EndPackingMonth = DateTime.Parse(v_endPackingMonth);
                                        }

                                    }
                                    catch
                                    {
                                        row.ErrorDescription += "Ngày EndPackingMonth không hợp lệ! ";
                                    }
                                    row.Remark = v_remark;

                                    var check = (v_worksheet.Cells[i, k]).Value?.ToString() ?? "";

                                    if (check != "" && check != "0")
                                    {
                                        row.Grade = v_worksheet.Cells[2, k].Value?.ToString();

                                        try
                                        {
                                            row.UsageQty = Convert.ToInt32((v_worksheet.Cells[i, k]).Value?.ToString());
                                            if (row.UsageQty < 0)
                                            {
                                                row.ErrorDescription += "UsageQty phải là số dương! ";
                                                //continue;
                                            }
                                            listImport.Add(row);
                                        }
                                        catch (Exception ex)
                                        {
                                            row.ErrorDescription += "UsageQty phải là số! ";
                                        }

                                    }
                                    else
                                    {
                                        continue;
                                    }


                                }



                            }


                        }


                    }

                }
                if (listImport.Count > 0)
                {
                    IEnumerable<ImportCkdPartListNormalDto> dataE = listImport.AsEnumerable();
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
                                bulkCopy.DestinationTableName = "InvCkdPart_T";
                                bulkCopy.ColumnMappings.Add("Guid", "Guid");
                                bulkCopy.ColumnMappings.Add("MaintenanceCode", "MaintenanceCode");
                                bulkCopy.ColumnMappings.Add("IsActive", "IsActive");
                                bulkCopy.ColumnMappings.Add("Cfc", "Cfc");
                                bulkCopy.ColumnMappings.Add("Grade", "Grade");
                                bulkCopy.ColumnMappings.Add("Shop", "Shop");
                                bulkCopy.ColumnMappings.Add("IdLine", "IdLine");
                                bulkCopy.ColumnMappings.Add("PartNo", "PartNo");
                                bulkCopy.ColumnMappings.Add("PartName", "PartName");
                                bulkCopy.ColumnMappings.Add("SupplierNo", "SupplierNo");
                                bulkCopy.ColumnMappings.Add("SupplierCd", "SupplierCd");
                                bulkCopy.ColumnMappings.Add("BodyColor", "BodyColor");
                                bulkCopy.ColumnMappings.Add("BackNo", "BackNo");
                                bulkCopy.ColumnMappings.Add("ModuleNo", "ModuleNo");
                                bulkCopy.ColumnMappings.Add("BoxSize", "BoxSize");
                                bulkCopy.ColumnMappings.Add("ReExportCd", "ReExportCd");
                                bulkCopy.ColumnMappings.Add("IcoFlag", "IcoFlag");
                                bulkCopy.ColumnMappings.Add("StartPackingMonth", "StartPackingMonth");
                                bulkCopy.ColumnMappings.Add("EndPackingMonth", "EndPackingMonth");
                                bulkCopy.ColumnMappings.Add("UsageQty", "UsageQty");
                                bulkCopy.ColumnMappings.Add("Remark", "Remark");
                                bulkCopy.ColumnMappings.Add("Renban", "Renban");
                                bulkCopy.ColumnMappings.Add("ErrorDescription", "ErrorDescription");

                                bulkCopy.WriteToServer(table);
                                tran.Commit();
                            }
                        }
                        await conn.CloseAsync();
                    }
                }
                return listImport;
            }

            catch (Exception ex)
            {
                throw new UserFriendlyException(400, ex.Message);
            }
        }



        [AbpAuthorize(AppPermissions.Pages_Ckd_Master_PartList_Import)]
        public async Task<List<ImportInvCkdPartGradeDto>> ImportDataInvCkdPartGradeFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<ImportInvCkdPartGradeDto> listImport = new List<ImportInvCkdPartGradeDto>();
                using (var stream = new MemoryStream(fileBytes))
                {
                    SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
                    var xlWorkBook = ExcelFile.Load(stream);
                    var v_worksheet = xlWorkBook.Worksheets[0];
                    DataFormatter formatter = new DataFormatter();
                    DateTime dateTime = DateTime.Now;

                    string strGUID = Guid.NewGuid().ToString("N");

                    string _sql = @"EXEC CMM_IMPORT_USER_GUID_INSERT @Guid, @p_UserId";
                    await _dapperRepo.ExecuteAsync(_sql, new
                    {
                        Guid = strGUID,
                        p_UserId = AbpSession.UserId
                    });

                    int count = 0;

                    for (int i = 4; i < v_worksheet.Rows.Count; i++)
                    {

                        string v_partNo = (v_worksheet.Cells[i, 5]).Value?.ToString().Replace("-", "") ?? "";

                        if (v_partNo != "")
                        {
                            string v_manintenance = (v_worksheet.Cells[i, 0]).Value?.ToString() ?? "";
                            string v_partStatus = (v_worksheet.Cells[i, 1]).Value?.ToString() ?? "";
                            string v_model = (v_worksheet.Cells[i, 2]).Value?.ToString() ?? "";
                            string v_shop = (v_worksheet.Cells[i, 3]).Value?.ToString() ?? "";
                            string v_IdLine = (v_worksheet.Cells[i, 4]).Value?.ToString() ?? "";
                            string v_partName = (v_worksheet.Cells[i, 6]).Value?.ToString() ?? "";
                            string v_exporter = (v_worksheet.Cells[i, 7]).Value?.ToString() ?? "";
                            string v_exporterCode = (v_worksheet.Cells[i, 8]).Value?.ToString() ?? "";
                            string v_bodycolor = (v_worksheet.Cells[i, 9]).Value?.ToString() ?? "";
                            string v_grade = (v_worksheet.Cells[i, 10]).Value?.ToString() ?? "";
                            string v_usageQty = (v_worksheet.Cells[i, 11]).Value?.ToString() ?? "";
                            string v_startLot = (v_worksheet.Cells[i, 12]).Value?.ToString() ?? "";
                            string v_startNoInLot = (v_worksheet.Cells[i, 13]).Value?.ToString() ?? "";
                            string v_endLot = (v_worksheet.Cells[i, 14]).Value?.ToString() ?? "";
                            string v_endNoInLot = (v_worksheet.Cells[i, 15]).Value?.ToString() ?? "";
                            string v_eciFromPart = (v_worksheet.Cells[i, 16]).Value?.ToString() ?? "";
                            string v_eciToPart = (v_worksheet.Cells[i, 17]).Value?.ToString() ?? "";
                            string v_orderPattern = (v_worksheet.Cells[i, 18]).Value?.ToString() ?? "";
                            string v_remark = (v_worksheet.Cells[i, 19]).Value?.ToString() ?? "";
                            string v_updateDate = (v_worksheet.Cells[i, 20]).Value?.ToString() ?? "";
                            string v_updateUser = (v_worksheet.Cells[i, 21]).Value?.ToString() ?? "";
                            string[] bodyColorArray = v_bodycolor.Split(",");



                            for (int j = 0; j < bodyColorArray.Length; j++)
                            {
                               
                                    var row = new ImportInvCkdPartGradeDto();
                                    row.Guid = strGUID;

                                    row.MaintenanceCode = v_manintenance;
                                    if (v_partStatus.Length > 10) row.ErrorDescription += "Độ dài PartStatus : " + v_partStatus + " không hợp lệ! , ";
                                    else row.PartStatus = v_partStatus;

                                    if (v_model.Length > 4) row.ErrorDescription += "Độ dài Order Model : " + v_model + " không hợp lệ! , ";
                                    else row.Cfc = v_model;

                                    if (v_shop.Length > 1) row.ErrorDescription += "Độ dài Shop : " + v_shop + " không hợp lệ! , ";
                                    else row.Shop = v_shop;

                                    if (v_IdLine.Length > 10) row.ErrorDescription += "Độ dài Idline : " + v_IdLine + " không hợp lệ! , ";
                                    else row.IdLine = v_IdLine;

                                    if (v_partNo.Length > 50) row.ErrorDescription += "Độ dài PartNo : " + v_partNo + " không hợp lệ! , ";
                                    else row.PartNo = v_partNo;

                                    if (v_partName.Length > 50) row.ErrorDescription += "Độ dài PartName : " + v_partName + " không hợp lệ! , ";
                                    else row.PartName = v_partName;

                                     if (v_startLot.Length > 10) row.ErrorDescription += "Độ dài StartLot : " + v_startLot + " không hợp lệ! , ";
                                    else row.StartLot = v_startLot;

                                      if (v_startNoInLot.Length > 10) row.ErrorDescription += "Độ dài StartNoInLot : " + v_startNoInLot + " không hợp lệ! , ";
                                    else row.StartNoInLot = v_startNoInLot;

                                      if (v_endLot.Length > 10) row.ErrorDescription += "Độ dài EndLot : " + v_endLot + " không hợp lệ! , ";
                                    else row.EndLot = v_endLot;

                                      if (v_endNoInLot.Length > 10) row.ErrorDescription += "Độ dài EndNoInLot : " + v_endNoInLot + " không hợp lệ! , ";
                                    else row.EndNoInLot = v_endNoInLot;

                                     if (v_updateUser.Length > 50) row.ErrorDescription += "Độ dài UpdateUser : " + v_updateUser + " không hợp lệ! , ";
                                    else row.UpdateUser = v_updateUser;

                                    row.BodyColor = bodyColorArray[j];

                                    if (v_grade.Length > 3) row.ErrorDescription += "Độ dài Grade : " + v_grade + " không hợp lệ! , ";
                                    else row.Grade = v_grade;

                                    try
                                    {
                                        if (v_updateDate != "") row.UpdateDate = DateTime.Parse(v_updateDate);

                                    }
                                    catch
                                    {
                                        row.ErrorDescription += "Ngày UpdateDate  không hợp lệ! ";
                                    }
                                    try
                                    {
                                        if (string.IsNullOrEmpty(v_usageQty))
                                        {
                                            row.UsageQty = null;
                                        }
                                        else
                                        {
                                            row.UsageQty = Convert.ToInt32(v_usageQty);
                                            if (row.UsageQty < 0)
                                            {
                                                row.ErrorDescription += "Qty phải là số dương! ";
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        row.ErrorDescription += "Qty " + v_usageQty + " không phải là số! ";
                                    }

                                    if (v_exporter.Length > 50) row.ErrorDescription += "Độ dài Exporter : " + v_exporter + " không hợp lệ! , ";
                                    else row.Exporter = v_exporter;

                                    if (v_exporterCode.Length > 50) row.ErrorDescription += "Độ dài ExporterCode : " + v_exporterCode + " không hợp lệ! , ";
                                    else row.ExporterCode = v_exporterCode;

                               
                                    if (v_orderPattern.Length > 50) row.ErrorDescription = "Độ dài Order Pattern : " + v_orderPattern + " không hợp lệ! , ";
                                    else row.OrderPattern = v_orderPattern.ToUpper();

                                    row.Remark = v_remark;

                                    row.CreatorUserId = AbpSession.UserId;
                                    row.ECIFromPart = v_eciFromPart;
                                    row.ECIToPart = v_eciToPart;

                                    listImport.Add(row);
                            }
                      

                        }

                   
                    }

                }
                if (listImport.Count > 0)
                {
                    IEnumerable<ImportInvCkdPartGradeDto> dataE = listImport.AsEnumerable();
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
                                bulkCopy.DestinationTableName = "InvCkdGrade_T";
                                bulkCopy.ColumnMappings.Add("Guid", "Guid");
                                bulkCopy.ColumnMappings.Add("PartNo", "PartNo");
                                bulkCopy.ColumnMappings.Add("PartName", "PartName");
                                bulkCopy.ColumnMappings.Add("Model", "Model");
                                bulkCopy.ColumnMappings.Add("Cfc", "Cfc");
                                bulkCopy.ColumnMappings.Add("Grade", "Grade");
                                bulkCopy.ColumnMappings.Add("IdLine", "IdLine");
                                bulkCopy.ColumnMappings.Add("Shop", "Shop");
                                bulkCopy.ColumnMappings.Add("BodyColor", "BodyColor");
                                bulkCopy.ColumnMappings.Add("UsageQty", "UsageQty");
                                bulkCopy.ColumnMappings.Add("StartLot", "StartLot");
                                bulkCopy.ColumnMappings.Add("StartNoInLot", "StartNoInLot");    
                                bulkCopy.ColumnMappings.Add("EndLot", "EndLot");
                                bulkCopy.ColumnMappings.Add("EndNoInLot", "EndNoInLot");
                                bulkCopy.ColumnMappings.Add("MaintenanceCode", "MaintenanceCode");
                                bulkCopy.ColumnMappings.Add("PartStatus", "PartStatus");
                                bulkCopy.ColumnMappings.Add("Exporter", "Exporter");
                                bulkCopy.ColumnMappings.Add("ExporterCode", "ExporterCode");                              
                                bulkCopy.ColumnMappings.Add("ECIFromPart", "ECIFromPart");
                                bulkCopy.ColumnMappings.Add("ECIToPart", "ECIToPart");
                                bulkCopy.ColumnMappings.Add("OrderPattern", "OrderPattern");
                                bulkCopy.ColumnMappings.Add("Remark", "Remark");
                                bulkCopy.ColumnMappings.Add("CreatorUserId", "CreatorUserId");
                                bulkCopy.ColumnMappings.Add("ErrorDescription", "ErrorDescription");
                                bulkCopy.ColumnMappings.Add("UpdateDate", "UpdateDate");
                                bulkCopy.ColumnMappings.Add("UpdateUser", "UpdateUser");

                                bulkCopy.WriteToServer(table);
                                tran.Commit();
                            }
                        }
                        await conn.CloseAsync();
                    }
                }
                return listImport;
            }

            catch (Exception ex)
            {
                throw new UserFriendlyException(400, ex.Message);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Ckd_Master_PartList_Import)]
        public async Task MergeDataInvCkdPartListNormal(string v_Guid)
        {

            string _merge = "Exec INV_CKD_PART_LIST_NORMAL_MERGE @Guid";
            await _dapperRepo.QueryAsync<ImportCkdPartListNormalDto>(_merge, new { Guid = v_Guid });
        }
       
        [AbpAuthorize(AppPermissions.Pages_Ckd_Master_PartList_Import)]
        public async Task MergeDataInvCkdPartGradeNormal(string v_Guid)
        {

            string _merge = "Exec INV_CKD_PART_GRADE_MERGE @Guid";
            await _dapperRepo.QueryAsync<ImportInvCkdPartGradeDto>(_merge, new { Guid = v_Guid });
        }

        public async Task<FileDto> GetListErrPartListNormalToExcel(string v_Guid, string v_Screen)
        {
            FileDto a = new FileDto();
            string _sql = "Exec INV_CKD_PART_LIST_GET_LIST_ERROR_IMPORT @Guid, @Screen";

            IEnumerable<ImportCkdPartListDto> result = await _dapperRepo.QueryAsync<ImportCkdPartListDto>(_sql, new
            {
                Guid = v_Guid,
                Screen = v_Screen
            });

            var exportToExcel = result.ToList();
       
            return _calendarListExcelExporter.ExportToFileErr(exportToExcel); ;

        }

        public async Task<PagedResultDto<ImportInvCkdPartGradeDto>> GetMessageErrorImportGrade(string v_Guid)
        {
            string _sql = "Exec INV_CKD_PART_GRADE_GET_LIST_ERROR_IMPORT @Guid";

            IEnumerable<ImportInvCkdPartGradeDto> result = await _dapperRepo.QueryAsync<ImportInvCkdPartGradeDto>(_sql, new
            {
                Guid = v_Guid
            });

            var listResult = result.ToList();
            var totalCount = listResult.Count();

            return new PagedResultDto<ImportInvCkdPartGradeDto>(totalCount, listResult);
        }

        public async Task<FileDto> GetListErrPartGradeToExcel(string v_Guid)
        {
            string _sql = "Exec INV_CKD_PART_GRADE_GET_LIST_ERROR_IMPORT @Guid";

            IEnumerable<ImportInvCkdPartGradeDto> result = await _dapperRepo.QueryAsync<ImportInvCkdPartGradeDto>(_sql, new
            {
                Guid = v_Guid
            });

            var exportToExcel = result.ToList();

            return _calendarListExcelExporter.ExportToFileErrGrade(exportToExcel);

        }

        public async Task<FileDto> GetCkdPartExportPackingDetails(InvCkdPartListDetailsExportInput input)
        {
            var file = new FileDto("CKDPartListPackingDetails.xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
            string fileName = "temp_CKDPartListPackingDetails";
            string template = "wwwroot/Template";
            string path = "";
            path = Path.Combine(Directory.GetCurrentDirectory(), template, fileName);
            var xlWorkBook = ExcelFile.Load(path + ".xlsx");
            var workSheet = xlWorkBook.Worksheets[0];

            List<string> listExport = new List<string>();
            listExport.Add("PartStatus");
            listExport.Add("Model");
            listExport.Add("Shop");
            listExport.Add("IdLine");
            listExport.Add("PartNo");
            listExport.Add("PartName");
            listExport.Add("Cfc");
            listExport.Add("Grade");
            listExport.Add("Exporter");
            listExport.Add("ExporterCode");
            listExport.Add("BodyColor");
            listExport.Add("BackNo");
            listExport.Add("ModuleNo");
            listExport.Add("Renban");
            listExport.Add("Boxsize");
            listExport.Add("ReExportCd");
            listExport.Add("IcoFlag");
            listExport.Add("StartPackingMonth");
            listExport.Add("EndProductionMonth");
     

            string[] properties = listExport.ToArray();
            string _sql = "Exec INV_CKD_PART_LIST_UNPACKING_DETAILS_EXPORT @part_no,@cfc, @p_model, @p_grade, @p_shop,@supplierNo,@p_order_pattern, @p_active";
            var listData = (await _dapperRepo.QueryAsync<InvCkdPartPackingDetailsDto>(_sql, new
            {
                part_no = input.PartNo,
                cfc = input.Cfc,
                p_model = input.Model,
                p_grade = input.Grade,
                p_shop = input.Shop,
                supplierNo = input.SupplierNo,
                p_order_pattern = input.OrderPattern,
                p_active = input.IsActive
            })).ToList();


            DataTable table = new DataTable();
            using (var reader = ObjectReader.Create(listData, properties))
            {
                table.Load(reader);
            }

            if (table.Rows.Count > 0)
            {
                InsertDataTableOptions ins = new InsertDataTableOptions(2, 0);
                workSheet.InsertDataTable(table, ins);
            }


            var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx");
            xlWorkBook.Save(tempFile);
            MemoryStream obj_stream = new MemoryStream();
            obj_stream = new MemoryStream(File.ReadAllBytes(tempFile));
            _tempFileCacheManager.SetFile(file.FileToken, obj_stream.ToArray());
            File.Delete(tempFile);
            obj_stream.Position = 0;
            return file;
        }

    }
}
