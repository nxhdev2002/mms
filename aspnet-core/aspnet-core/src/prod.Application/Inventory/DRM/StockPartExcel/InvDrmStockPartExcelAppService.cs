using Abp.Application.Services.Dto;
using Abp.AspNetZeroCore.Net;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.UI;
using FastMember;
using GemBox.Spreadsheet;
using prod.Authorization;
using prod.Dto;
using prod.Inventory.DRM.StockPartExcel.Dto;
using prod.Inventory.DRM.StockPartExcel.Exporting;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using prod.Common;

namespace prod.Inventory.DRM.StockPartExcel
{
    [AbpAuthorize(AppPermissions.Pages_DMIHP_Stock_StockPartExcel_View)]
    public class InvDrmStockPartExcelAppService : prodAppServiceBase, IInvDrmStockPartExcelAppService
    {
        private readonly IDapperRepository<InvDrmStockPart, long> _dapperRepo;
        private readonly IRepository<InvDrmStockPart, long> _repo;
        private readonly IInvDrmStockPartExcelExcelExporter _calendarListExcelExporter;
        private readonly ITempFileCacheManager _tempFileCacheManager;

        public InvDrmStockPartExcelAppService(IRepository<InvDrmStockPart, long> repo,
                                         IDapperRepository<InvDrmStockPart, long> dapperRepo,
                                        IInvDrmStockPartExcelExcelExporter calendarListExcelExporter,
                                        ITempFileCacheManager tempFileCacheManager
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
            _tempFileCacheManager = tempFileCacheManager;
        }

        public async Task<PagedResultDto<InvDrmStockPartExcelDetailDto>> GetInvDrmStockPartExcelDetail(InvDrmStockPartExcelDetailInputDto input)
        {
            string _sql = "Exec  INV_DRM_PARTLIST_EXPORT_DETAILS @p_date";

            var filtered = await _dapperRepo.QueryAsync<InvDrmStockPartExcelDetailDto>(_sql, new
            {
                p_date = input.WorkingDate,
            });

            var totalCount = filtered.Count();

            return new PagedResultDto<InvDrmStockPartExcelDetailDto>(
                totalCount,
                 filtered.ToList()
            ); 
        }

        public async Task<PagedResultDto<InvDrmStockPartExcelDetailDto>> GetInvDrmStockPartExcelDetailSearch(InvDrmStockPartExcelDetailSearchInputDto input)
        {
            string _sql = "Exec  INV_DRM_PARTLIST_EXPORT_DETAILS_SEARCH @p_date, @p_model, @p_part_code, @p_material_code, @p_part_no";

            var filtered = await _dapperRepo.QueryAsync<InvDrmStockPartExcelDetailDto>(_sql, new
            {
                p_date = input.WorkingDate,
                p_model = input.Model,
                p_part_code = input.PartCode,
                p_material_code = input.Materialcode,
                p_part_no = input.PartNo,
            });

            var totalCount = filtered.Count();

            return new PagedResultDto<InvDrmStockPartExcelDetailDto>(
                totalCount,
                 filtered.ToList()
            );
        }
         
        public async Task<List<InvDrmStockPartDetailGridviewRowDto>> GetInvDrmStockPartStockUpdateTrans(InvDrmStockPartStockUpdateTransDto input)
        {  
                string _sql = "Exec  INV_DRM_STOCK_PART_STOCK_UPDATE_TRANS_NEW @p_date, @p_part_id, @p_drm_part_id, @p_item_order, @p_shift, @p_value";

                var filtered = await _dapperRepo.QueryAsync<InvDrmStockPartDetailGridviewRowDto>(_sql, new
                {
                    p_date = input.WorkingDate,
                    p_part_id = input.PartId,
                    p_drm_part_id = input.DrmPartListId,
                    p_item_order = input.ItemOrder,
                    p_shift = input.Shift,
                    p_value = input.Value,
                }); 

            return filtered.ToList();   
        }


        [AbpAuthorize(AppPermissions.Pages_DMIHP_Stock_StockPartExcel_Import)]
        public async Task<List<InvDrmStockPartExcelImportDto>> ImportInvDRMStockPartExcelFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<InvDrmStockPartExcelImportDto> listImport = new List<InvDrmStockPartExcelImportDto>();
                using (var stream = new MemoryStream(fileBytes))
                {
                    SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
                    var xlWorkBook = ExcelFile.Load(stream);
                    var v_worksheet = xlWorkBook.Worksheets[0];

                    string strGUID = Guid.NewGuid().ToString("N");

                    string str_monthYear = (v_worksheet.Cells[2, 6]).Value?.ToString() ?? "";

                    int index = str_monthYear.IndexOf("Tháng");
                    int index2 = str_monthYear.IndexOf("Năm");
                    string v_month = str_monthYear.Substring(index + 5, 3).Trim();
                    string v_year = str_monthYear.Substring(index2 + 3, 5).Trim();

                    int days = DateTime.DaysInMonth(Convert.ToInt32(v_year), Convert.ToInt32(v_month));

                    for (int j = 6; j < (6 + days * 2); j++)
                    {
                        string v_day = (v_worksheet.Cells[3, j]).Value?.ToString() ?? "";
                        if (Convert.ToInt32(v_day) <= days)
                        {
                            string v_shift = (v_worksheet.Cells[5, j]).Value?.ToString() ?? "";
                            string v_workingdate = v_month + "/" + v_day + "/" + v_year;
                            string v_model = "";
                            string v_partno = "";
                            string v_partcode_1 = "";
                            string v_partcode_2 = "";
                            string v_gradename = "";
                            string v_gradename_firt = "";
                            string v_UsePress = "";
                            string v_Press = "";
                            string v_IhpOh = "";
                            string v_PressBroken = "";
                            string v_Hand = "";
                            string v_HandOh = "";
                            string v_HandBroken = "";
                            string v_Qty = "";
                            string v_MaterialIn = "";
                            string v_MaterialInAddition = "";
                            string v_QtyLastStock = "";
                            string v_IhpOhLastStock = "";
                            string v_HandOhStock = "";



                            for (int i = 6; i < v_worksheet.Rows.Count; i++)
                            {
                                v_partcode_1 = (i == 6) ? ((v_worksheet.Cells[i, 2]).Value?.ToString() ?? "") : ((v_worksheet.Cells[i - 1, 2]).Value?.ToString() ?? "");
                                v_partcode_2 = (v_worksheet.Cells[i, 2]).Value?.ToString() ?? "";

                                string v_categorytracking = ((v_worksheet.Cells[i, 4]).Value?.ToString() ?? "").ToUpper();

                                var v_gradename_t = (v_worksheet.Cells[i, 3]).Value?.ToString() ?? "";
                                var v_gradename_firt_t = (v_worksheet.Cells[i - 1, 3]).Value?.ToString() ?? "";

                                int index1 = v_gradename_t.IndexOf("\n") == -1 ? v_gradename_t.Length : v_gradename_t.IndexOf("\n");
                                v_gradename = v_gradename_t.Substring(0, index1).Trim();

                                int index_f = v_gradename_firt_t.IndexOf("\n") == -1 ? v_gradename_firt_t.Length : v_gradename_firt_t.IndexOf("\n");
                                v_gradename_firt = v_gradename_firt_t.Substring(0, index_f).Trim();

                               

                                if (v_partcode_1 == v_partcode_2 && v_gradename_firt == v_gradename)
                                {
                                    v_model = (v_worksheet.Cells[i, 0]).Value?.ToString() ?? "";
                                    v_partno = (v_worksheet.Cells[i, 1]).Value?.ToString() ?? "";
                                    


                                    if (v_partcode_1 == "I3")
                                    {
                                        var a = 1;
                                    }

                                    if (v_categorytracking == "CẤP")
                                    {
                                        v_UsePress = v_gradename_firt != v_gradename ? ((v_worksheet.Cells[i, j]).Value?.ToString() ?? "") : ((v_worksheet.Cells[i-1, j]).Value?.ToString() ?? "");
                                    }
                                    else if (v_categorytracking == "DẬP (+ CẢ DẬP HỎNG)")
                                    {
                                        v_Press = (v_worksheet.Cells[i, j]).Value?.ToString() ?? "";
                                    }
                                    else if (v_categorytracking == "STOCK DẬP")
                                    {
                                        v_IhpOhLastStock = (v_worksheet.Cells[i, 5]).Value?.ToString() ?? "";
                                        v_IhpOh = (v_worksheet.Cells[i, j]).Value?.ToString() ?? "";
                                    }
                                    else if (v_categorytracking == "DẬP HỎNG")
                                    {
                                        v_PressBroken = (v_worksheet.Cells[i, j]).Value?.ToString() ?? "";
                                    }
                                    else if (v_categorytracking == "HAND (+ CẢ HAND HỎNG)")
                                    {
                                        v_Hand = (v_worksheet.Cells[i, j]).Value?.ToString() ?? "";
                                    }
                                    else if (v_categorytracking == "STOCK HAND")
                                    {
                                        v_HandOhStock = (v_worksheet.Cells[i, 5]).Value?.ToString() ?? "";
                                        v_HandOh = (v_worksheet.Cells[i, j]).Value?.ToString() ?? "";
                                    }
                                    else if (v_categorytracking == "HAND HỎNG")
                                    {
                                        v_HandBroken = (v_worksheet.Cells[i, j]).Value?.ToString() ?? "";
                                    }
                                    else if (v_categorytracking == "MATERIAL(CUỐI NGÀY)")
                                    {
                                        v_QtyLastStock = (v_worksheet.Cells[i, 5]).Value?.ToString() ?? "";
                                        v_Qty = (v_worksheet.Cells[i, j]).Value?.ToString() ?? "";
                                    }
                                    else if (v_categorytracking == "NEW  MATERIAL")
                                    {
                                        v_MaterialIn = (v_worksheet.Cells[i, j]).Value?.ToString() ?? "";
                                    }
                                    else if (v_categorytracking == "TẤM TẬN DỤNG")
                                    {
                                        v_MaterialInAddition = (v_worksheet.Cells[i, j]).Value?.ToString() ?? "";
                                    }

                                }
                                if (v_partcode_1 != v_partcode_2 || i == (v_worksheet.Rows.Count - 1) || v_gradename_firt != v_gradename)
                                {


                                    var row = new InvDrmStockPartExcelImportDto();

                                    row.Guid = strGUID;
                                    row.ErrorDescription = "";
                                    row.Model = v_model;
                                    row.PartNo = v_partno;
                                    row.PartCode = v_partcode_1;
                                    row.GradeName = (i != 6 && v_gradename_firt != v_gradename) ? v_gradename_firt : v_gradename;
                                    row.WorkingDate = DateTime.Parse(v_workingdate, new CultureInfo("en-US", true));
                                    row.Shift = v_shift;
                                    row.UsePress = Convert.ToInt32(v_UsePress != "" ? v_UsePress : 0);
                                    row.Press = Convert.ToInt32(v_Press != "" ? v_Press : 0);
                                    row.IhpOh = Convert.ToInt32(v_IhpOh != "" ? v_IhpOh : 0);
                                    row.PressBroken = Convert.ToInt32(v_PressBroken != "" ? v_PressBroken : 0);
                                    row.Hand = Convert.ToInt32(v_Hand != "" ? v_Hand : 0);
                                    row.HandOh = Convert.ToInt32(v_HandOh != "" ? v_HandOh : 0);
                                    row.HandBroken = Convert.ToInt32(v_HandBroken != "" ? v_HandBroken : 0);
                                    row.Qty = Convert.ToInt32(v_Qty != "" ? v_Qty : 0);
                                    row.MaterialIn = Convert.ToInt32(v_MaterialIn != "" ? v_MaterialIn : 0);
                                    row.MaterialInAddition = Convert.ToInt32(v_MaterialInAddition != "" ? v_MaterialInAddition : 0);

                                    row.QtyLastStock = Convert.ToInt32(v_QtyLastStock != "" ? v_QtyLastStock : 0);
                                    row.IhpOhLastStock = Convert.ToInt32(v_IhpOhLastStock != "" ? v_IhpOhLastStock : 0);
                                    row.HandOhLastStock = Convert.ToInt32(v_HandOhStock != "" ? v_HandOhStock : 0);

                                    listImport.Add(row);


                                    if (v_categorytracking == "CẤP")
                                    {
                                        v_UsePress = (v_worksheet.Cells[i, j]).Value?.ToString() ?? "";
                                    }

                                    v_Press = "";
                                    v_IhpOh = "";
                                    v_PressBroken = "";
                                    v_Hand = "";
                                    v_HandOh = "";
                                    v_HandBroken = "";
                                    v_Qty = "";
                                    v_MaterialIn = "";
                                    v_MaterialInAddition = "";
                                    v_QtyLastStock = "";
                                    v_IhpOhLastStock = "";
                                    v_HandOhStock = "";


                                }

                            }


                        }

                    }


                    // import temp into db (bulkCopy)
                    if (listImport.Count > 0)
                    {
                        IEnumerable<InvDrmStockPartExcelImportDto> dataE = listImport.AsEnumerable();
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
                                    bulkCopy.DestinationTableName = "InvDrmStockPart_T";
                                    bulkCopy.ColumnMappings.Add("Guid", "Guid");
                                    bulkCopy.ColumnMappings.Add("Model", "Model");
                                    bulkCopy.ColumnMappings.Add("PartNo", "PartNo");
                                    bulkCopy.ColumnMappings.Add("PartCode", "PartCode");
                                    bulkCopy.ColumnMappings.Add("GradeName", "GradeName");
                                    bulkCopy.ColumnMappings.Add("WorkingDate", "WorkingDate");
                                    bulkCopy.ColumnMappings.Add("Shift", "Shift");
                                    bulkCopy.ColumnMappings.Add("Qty", "Qty");
                                    bulkCopy.ColumnMappings.Add("UsePress", "UsePress");
                                    bulkCopy.ColumnMappings.Add("Press", "Press");
                                    bulkCopy.ColumnMappings.Add("IhpOh", "IhpOh");
                                    bulkCopy.ColumnMappings.Add("PressBroken", "PressBroken");
                                    bulkCopy.ColumnMappings.Add("Hand", "Hand");
                                    bulkCopy.ColumnMappings.Add("HandOh", "HandOh");
                                    bulkCopy.ColumnMappings.Add("HandBroken", "HandBroken");
                                    bulkCopy.ColumnMappings.Add("MaterialIn", "MaterialIn");
                                    bulkCopy.ColumnMappings.Add("MaterialInAddition", "MaterialInAddition");
                                    bulkCopy.ColumnMappings.Add("QtyLastStock", "QtyLastStock");
                                    bulkCopy.ColumnMappings.Add("IhpOhLastStock", "IhpOhLastStock");
                                    bulkCopy.ColumnMappings.Add("HandOhLastStock", "HandOhLastStock");
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


        //Merge Data 
        public async Task MergeDataInvDrmStockPart(string v_Guid)
        {
            string _sql = "Exec INV_DRM_STOCK_PART_EXCEL_MERGE_NEW @Guid";
            await _dapperRepo.QueryAsync<InvDrmStockPartExcelImportDto>(_sql, new { Guid = v_Guid });
        }

        // hiển thị lỗi import và export lỗi
        public async Task<PagedResultDto<InvDrmStockPartExcelImportDto>> GetMessageErrorImport(string v_Guid)
        {
            string _sql = "Exec INV_DRM_STOCK_PART_GET_LIST_ERROR_IMPORT @Guid";

            IEnumerable<InvDrmStockPartExcelImportDto> result = await _dapperRepo.QueryAsync<InvDrmStockPartExcelImportDto>(_sql, new
            {
                Guid = v_Guid
            });

            var listResult = result.ToList();
            var totalCount = listResult.Count();

            return new PagedResultDto<InvDrmStockPartExcelImportDto>(
                totalCount,
               listResult
               );
        }

        public async Task<FileDto> GetListErrToExcel(string v_Guid)
        {
            string _sql = "Exec INV_DRM_STOCK_PART_GET_LIST_ERROR_IMPORT @Guid";

            IEnumerable<InvDrmStockPartExcelImportDto> result = await _dapperRepo.QueryAsync<InvDrmStockPartExcelImportDto>(_sql, new
            {
                Guid = v_Guid
            });

            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportToFileErr(exportToExcel);
        }

        public async Task<FileDto> GetExportStockPartExcelToExcel(GetInvDrmStockPartExcelExportInput input)
        {
            var file = new FileDto("DrmStockPartExcel.xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
            string fileName = "temp_DrmStockPartExcel";
            string template = "wwwroot/Template";
            string path = "";
            path = Path.Combine(Directory.GetCurrentDirectory(), template, fileName);
            var xlWorkBook = ExcelFile.Load(path + ".xlsx");
            var workSheet = xlWorkBook.Worksheets[0];
            int modelRange = 7;
            int partnoRange = 7;
            int partcodeRange = 7;
            int gradeRange = 7;
            var style = new CellStyle();
            style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            style.VerticalAlignment = VerticalAlignmentStyle.Center;
            style.Borders.SetBorders(MultipleBorders.All, SpreadsheetColor.FromArgb(0, 0, 0), GemBox.Spreadsheet.LineStyle.Thin);
            style.Font.Weight = 700;

            var stylemodel = new CellStyle();
            stylemodel.Rotation = 90;
            stylemodel.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            stylemodel.VerticalAlignment = VerticalAlignmentStyle.Center;
            stylemodel.Borders.SetBorders(MultipleBorders.All, SpreadsheetColor.FromArgb(0, 0, 0), GemBox.Spreadsheet.LineStyle.Thin);
            stylemodel.Font.Weight = 700;

            var styleitem = new CellStyle();
            styleitem.Borders.SetBorders(MultipleBorders.All, SpreadsheetColor.FromArgb(0, 0, 0), GemBox.Spreadsheet.LineStyle.Thin);

            var styleca = new CellStyle();
            styleca.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            styleca.Borders.SetBorders(MultipleBorders.All, SpreadsheetColor.FromArgb(0, 0, 0), GemBox.Spreadsheet.LineStyle.Thin);

            var stylemonth = new CellStyle();
            stylemonth.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            stylemonth.VerticalAlignment = VerticalAlignmentStyle.Center;
            stylemonth.Borders.SetBorders(MultipleBorders.All, SpreadsheetColor.FromArgb(0, 0, 0), GemBox.Spreadsheet.LineStyle.Thin);
            stylemonth.FillPattern.SetSolid(SpreadsheetColor.FromArgb(255, 204, 255));
            stylemonth.Font.Weight = 700;

            string _sql = "Exec INV_DRM_PARTLIST_EXPORT_DETAILS @p_date";

            IEnumerable<InvDrmStockPartExcelDetailDto> result = await _dapperRepo.QueryAsync<InvDrmStockPartExcelDetailDto>(_sql, new
            {
                p_date = input.WorkingDate
            });

            List<InvDrmStockPartExcelDetailDto> listResult = result.ToList();

            string _getshift = "Exec INV_DRM_PARTLIST_EXCEL_GET_SHIFT ";
            IEnumerable<GetShiftDto> rs = await _dapperRepo.QueryAsync<GetShiftDto>(_getshift, new { });
            List<GetShiftDto> shift = rs.ToList();

            int ca = (int)shift[0].Shift;

            for (int i = 0; i < listResult.Count() - 1; i++)
            {
                if (listResult[i].Model != listResult[i + 1].Model)
                {
                    var range = workSheet.Cells.GetSubrange("A" + modelRange + ":A" + (i + 7) + "");

                    // Merge cells in the current range.
                    range.Merged = true;

                    // Set the value of the merged range.
                    range.Value = listResult[i].Model;

                    range.Style = stylemodel;

                    modelRange = i + 8;



                    int check = 0;
                    for (int j = partnoRange - 7; j < i - 1; j++)
                    {
                        if (listResult[j].PartNo != listResult[j + 1].PartNo)
                        {
                            range = workSheet.Cells.GetSubrange("B" + partnoRange + ":B" + (j + 7) + "");

                            // Merge cells in the current range.
                            range.Merged = true;

                            // Set the value of the merged range.
                            range.Value = listResult[j].PartNo;

                            range.Style = style;

                            partnoRange = j + 8;
                        }
                        else { check++; }
                    }
                    if (check > 0)
                    {
                        range = workSheet.Cells.GetSubrange("B" + partnoRange + ":B" + (i + 7) + "");

                        // Merge cells in the current range.
                        range.Merged = true;

                        // Set the value of the merged range.
                        range.Value = listResult[i].PartNo;

                        range.Style = style;

                        partnoRange = i + 8;
                    }
                }
                else if (i == listResult.Count() - 2
                    && listResult[i].Model == listResult[i + 1].Model)
                {
                    var range = workSheet.Cells.GetSubrange("A" + modelRange + ":A" + (i + 8) + "");

                    // Merge cells in the current range.
                    range.Merged = true;

                    // Set the value of the merged range.
                    range.Value = listResult[i + 1].Model;

                    range.Style = stylemodel;


                    int check = 0;
                    for (int j = partnoRange - 7; j < i - 1; j++)
                    {
                        if (listResult[j].PartNo != listResult[j + 1].PartNo)
                        {
                            range = workSheet.Cells.GetSubrange("B" + partnoRange + ":B" + (j + 7) + "");

                            range.Merged = true;

                            range.Value = listResult[j].PartNo;

                            range.Style = style;

                            partnoRange = j + 8;
                        }
                        else { check++; }
                    }
                    if (check > 0)
                    {
                        range = workSheet.Cells.GetSubrange("B" + partnoRange + ":B" + (i + 8) + "");

                        range.Merged = true;

                        range.Value = listResult[i + 1].PartNo;

                        range.Style = style;
                    }
                }


                if (listResult[i].PartCode != listResult[i + 1].PartCode)
                {
                    var range = workSheet.Cells.GetSubrange("C" + partcodeRange + ":C" + (i + 7) + "");

                    // Merge cells in the current range.
                    range.Merged = true;

                    // Set the value of the merged range.
                    range.Value = listResult[i].PartCode;

                    range.Style = style;

                    partcodeRange = i + 8;

                    int check = 0;
                    for (int j = gradeRange - 7; j < i - 1; j++)
                    {
                        if (listResult[j].Grade != listResult[j + 1].Grade)
                        {
                            range = workSheet.Cells.GetSubrange("D" + gradeRange + ":D" + (j + 7) + "");

                            // Merge cells in the current range.
                            range.Merged = true;

                            // Set the value of the merged range.
                            range.Value = listResult[j].Grade;

                            range.Style = style;

                            gradeRange = j + 8;
                        }
                        else { check++; }
                    }
                    if (check > 0)
                    {
                        range = workSheet.Cells.GetSubrange("D" + gradeRange + ":D" + (i + 7) + "");

                        // Merge cells in the current range.
                        range.Merged = true;

                        // Set the value of the merged range.
                        range.Value = listResult[i].Grade;

                        range.Style = style;

                        gradeRange = i + 8;
                    }
                }
                else if (i == listResult.Count() - 2
                    && listResult[i].PartCode == listResult[i + 1].PartCode)
                {
                    var range = workSheet.Cells.GetSubrange("C" + partcodeRange + ":C" + (i + 8) + "");

                    // Merge cells in the current range.
                    range.Merged = true;

                    // Set the value of the merged range.
                    range.Value = listResult[i].PartCode;

                    range.Style = style;


                    range = workSheet.Cells.GetSubrange("D" + gradeRange + ":D" + (i + 8) + "");

                    // Merge cells in the current range.
                    range.Merged = true;

                    // Set the value of the merged range.
                    range.Value = listResult[i].Grade;

                    range.Style = style;
                }
            }

            int day = DateTime.DaysInMonth(input.WorkingDate.Value.Year, input.WorkingDate.Value.Month);
            var rangeheader = workSheet.Cells.GetSubrange("G3:" + workSheet.Cells[2, day * ca + 5].Name);

            rangeheader.Merged = true;
            rangeheader.Value = "Tháng " + input.WorkingDate.Value.Month + " Năm " + input.WorkingDate.Value.Year;
            rangeheader.Style = stylemonth;

            style.Font.Weight = 400;

            for (int i = 0; i < listResult.Count(); i++)
            {
                switch(listResult[i].ItemCode.ToUpper())
                {
                    case "CẤP":
                        styleitem.FillPattern.SetSolid(SpreadsheetColor.FromArgb(255, 219, 81));
                        workSheet.Cells[6 + i, 4].Value = listResult[i].ItemCode;
                        workSheet.Cells[6 + i, 4].Style = styleitem;
                        break;
                    case "DẬP (+ CẢ DẬP HỎNG)":
                        styleitem.FillPattern.SetSolid(SpreadsheetColor.FromArgb(130, 255, 180));
                        workSheet.Cells[6 + i, 4].Value = listResult[i].ItemCode;
                        workSheet.Cells[6 + i, 4].Style = styleitem;
                        break;
                    case "STOCK DẬP":
                        styleitem.FillPattern.SetSolid(SpreadsheetColor.FromArgb(255, 157, 210));
                        workSheet.Cells[6 + i, 4].Value = listResult[i].ItemCode;
                        workSheet.Cells[6 + i, 4].Style = styleitem;
                        break;
                    case "DẬP HỎNG":
                        styleitem.FillPattern.SetSolid(SpreadsheetColor.FromArgb(203, 185, 255));
                        workSheet.Cells[6 + i, 4].Value = listResult[i].ItemCode;
                        workSheet.Cells[6 + i, 4].Style = styleitem;
                        break;
                    case "HAND (+ CẢ HAND HỎNG)":
                        styleitem.FillPattern.SetSolid(SpreadsheetColor.FromArgb(141, 238, 255));
                        workSheet.Cells[6 + i, 4].Value = listResult[i].ItemCode;
                        workSheet.Cells[6 + i, 4].Style = styleitem;
                        break;
                    case "STOCK HAND":
                        styleitem.FillPattern.SetSolid(SpreadsheetColor.FromArgb(209, 166, 145));
                        workSheet.Cells[6 + i, 4].Value = listResult[i].ItemCode;
                        workSheet.Cells[6 + i, 4].Style = styleitem;
                        break;
                    case "HAND HỎNG":
                        styleitem.FillPattern.SetSolid(SpreadsheetColor.FromArgb(252, 154, 91));
                        workSheet.Cells[6 + i, 4].Value = listResult[i].ItemCode;
                        workSheet.Cells[6 + i, 4].Style = styleitem;
                        break;
                    case "MATERIAL(CUỐI NGÀY)":
                        styleitem.FillPattern.SetSolid(SpreadsheetColor.FromArgb(109, 198, 255));
                        workSheet.Cells[6 + i, 4].Value = listResult[i].ItemCode;
                        workSheet.Cells[6 + i, 4].Style = styleitem;
                        break;
                    case "NEW  MATERIAL":
                        styleitem.FillPattern.SetSolid(SpreadsheetColor.FromArgb(155, 171, 75));
                        workSheet.Cells[6 + i, 4].Value = listResult[i].ItemCode;
                        workSheet.Cells[6 + i, 4].Style = styleitem;
                        break;
                    case "TẤM TẬN DỤNG":
                        styleitem.FillPattern.SetSolid(SpreadsheetColor.FromArgb(176, 227, 207));
                        workSheet.Cells[6 + i, 4].Value = listResult[i].ItemCode;
                        workSheet.Cells[6 + i, 4].Style = styleitem;
                        break;
                }
                workSheet.Cells[6 + i, 4].Value = listResult[i].ItemCode;
                workSheet.Cells[6 + i, 4].Style = styleitem;

                // Ca 2 cuối tháng trước
                workSheet.Cells[6 + i, 5].Value = "";
                workSheet.Cells[6 + i, 5].Style = styleca;

                if ((ca * i + 5 + ca) < (day * ca + 6))
                {
                    //ngày trong tháng
                    style.FillPattern.SetSolid(SpreadsheetColor.FromArgb(255, 255, 255));
                    var rangeday = workSheet.Cells.GetSubrange(workSheet.Cells[3, ca * i + 6].Name + ":" + workSheet.Cells[3, ca * i + 5 + ca].Name);
                    rangeday.Merged = true;
                    rangeday.Value = i + 1;
                    rangeday.Style = style;

                    //???
                    style.FillPattern.SetSolid(SpreadsheetColor.FromArgb(255, 0, 0));
                    workSheet.Cells[4, ca * i + 6].Value = "R";
                    workSheet.Cells[4, ca * i + 6].Style = style;

                    //ca/ngày
                    workSheet.Cells[5, ca * i + 6].Value = "Ca1";
                    workSheet.Cells[5, ca * i + 6].Style = styleitem;

                    switch (ca)
                    {
                        case 2:
                            style.FillPattern.SetSolid(SpreadsheetColor.FromArgb(255, 255, 0));
                            workSheet.Cells[4, ca * i + 7].Value = "Y";
                            workSheet.Cells[4, ca * i + 7].Style = style;

                            workSheet.Cells[5, ca * i + 7].Value = "Ca2";
                            workSheet.Cells[5, ca * i + 7].Style = styleitem;
                            break;
                        case 3:
                            style.FillPattern.SetSolid(SpreadsheetColor.FromArgb(255, 255, 0));
                            workSheet.Cells[4, ca * i + 7].Value = "Y";
                            workSheet.Cells[4, ca * i + 7].Style = style;
                            style.FillPattern.SetSolid(SpreadsheetColor.FromArgb(0, 255, 0));
                            workSheet.Cells[4, ca * i + 8].Value = "T";
                            workSheet.Cells[4, ca * i + 8].Style = style;

                            workSheet.Cells[5, ca * i + 7].Value = "Ca2";
                            workSheet.Cells[5, ca * i + 7].Style = styleitem;
                            workSheet.Cells[5, ca * i + 8].Value = "Ca3";
                            workSheet.Cells[5, ca * i + 8].Style = styleitem;
                            break;
                        default:
                            break;
                    }
                }

                switch (ca)
                {
                    case 1:
                        workSheet.Cells[6 + i, 6].Value = listResult[i].N01_Ca1;
                        workSheet.Cells[6 + i, 6].Style = styleca;
                        workSheet.Cells[6 + i, 7].Value = listResult[i].N02_Ca1;
                        workSheet.Cells[6 + i, 7].Style = styleca;
                        workSheet.Cells[6 + i, 8].Value = listResult[i].N03_Ca1;
                        workSheet.Cells[6 + i, 8].Style = styleca;
                        workSheet.Cells[6 + i, 9].Value = listResult[i].N04_Ca1;
                        workSheet.Cells[6 + i, 9].Style = styleca;
                        workSheet.Cells[6 + i, 10].Value = listResult[i].N05_Ca1;
                        workSheet.Cells[6 + i, 10].Style = styleca;
                        workSheet.Cells[6 + i, 11].Value = listResult[i].N06_Ca1;
                        workSheet.Cells[6 + i, 11].Style = styleca;
                        workSheet.Cells[6 + i, 12].Value = listResult[i].N07_Ca1;
                        workSheet.Cells[6 + i, 12].Style = styleca;
                        workSheet.Cells[6 + i, 13].Value = listResult[i].N08_Ca1;
                        workSheet.Cells[6 + i, 13].Style = styleca;
                        workSheet.Cells[6 + i, 14].Value = listResult[i].N09_Ca1;
                        workSheet.Cells[6 + i, 14].Style = styleca;
                        workSheet.Cells[6 + i, 15].Value = listResult[i].N10_Ca1;
                        workSheet.Cells[6 + i, 15].Style = styleca;
                        workSheet.Cells[6 + i, 16].Value = listResult[i].N11_Ca1;
                        workSheet.Cells[6 + i, 16].Style = styleca;
                        workSheet.Cells[6 + i, 17].Value = listResult[i].N12_Ca1;
                        workSheet.Cells[6 + i, 17].Style = styleca;
                        workSheet.Cells[6 + i, 18].Value = listResult[i].N13_Ca1;
                        workSheet.Cells[6 + i, 18].Style = styleca;
                        workSheet.Cells[6 + i, 19].Value = listResult[i].N14_Ca1;
                        workSheet.Cells[6 + i, 19].Style = styleca;
                        workSheet.Cells[6 + i, 20].Value = listResult[i].N15_Ca1;
                        workSheet.Cells[6 + i, 20].Style = styleca;
                        workSheet.Cells[6 + i, 21].Value = listResult[i].N16_Ca1;
                        workSheet.Cells[6 + i, 21].Style = styleca;
                        workSheet.Cells[6 + i, 22].Value = listResult[i].N17_Ca1;
                        workSheet.Cells[6 + i, 22].Style = styleca;
                        workSheet.Cells[6 + i, 23].Value = listResult[i].N18_Ca1;
                        workSheet.Cells[6 + i, 23].Style = styleca;
                        workSheet.Cells[6 + i, 24].Value = listResult[i].N19_Ca1;
                        workSheet.Cells[6 + i, 24].Style = styleca;
                        workSheet.Cells[6 + i, 25].Value = listResult[i].N20_Ca1;
                        workSheet.Cells[6 + i, 25].Style = styleca;
                        workSheet.Cells[6 + i, 26].Value = listResult[i].N21_Ca1;
                        workSheet.Cells[6 + i, 26].Style = styleca;
                        workSheet.Cells[6 + i, 27].Value = listResult[i].N22_Ca1;
                        workSheet.Cells[6 + i, 27].Style = styleca;
                        workSheet.Cells[6 + i, 28].Value = listResult[i].N23_Ca1;
                        workSheet.Cells[6 + i, 28].Style = styleca;
                        workSheet.Cells[6 + i, 29].Value = listResult[i].N24_Ca1;
                        workSheet.Cells[6 + i, 29].Style = styleca;
                        workSheet.Cells[6 + i, 30].Value = listResult[i].N25_Ca1;
                        workSheet.Cells[6 + i, 30].Style = styleca;
                        workSheet.Cells[6 + i, 31].Value = listResult[i].N26_Ca1;
                        workSheet.Cells[6 + i, 31].Style = styleca;
                        workSheet.Cells[6 + i, 32].Value = listResult[i].N27_Ca1;
                        workSheet.Cells[6 + i, 32].Style = styleca;
                        workSheet.Cells[6 + i, 33].Value = listResult[i].N28_Ca1;
                        workSheet.Cells[6 + i, 33].Style = styleca;
                        if (day >= 29)
                        {
                            workSheet.Cells[6 + i, 34].Value = listResult[i].N29_Ca1;
                            workSheet.Cells[6 + i, 34].Style = styleca;
                        }
                        if (day >= 30)
                        {
                            workSheet.Cells[6 + i, 35].Value = listResult[i].N30_Ca1;
                            workSheet.Cells[6 + i, 35].Style = styleca;
                        }
                        if (day == 31)
                        {
                            workSheet.Cells[6 + i, 36].Value = listResult[i].N31_Ca1;
                            workSheet.Cells[6 + i, 36].Style = styleca;
                        }

                        break;
                    case 2:
                        workSheet.Cells[6 + i, 6].Value = listResult[i].N01_Ca1;
                        workSheet.Cells[6 + i, 6].Style = styleca;
                        workSheet.Cells[6 + i, 7].Value = listResult[i].N01_Ca2;
                        workSheet.Cells[6 + i, 7].Style = styleca;

                        workSheet.Cells[6 + i, 8].Value = listResult[i].N02_Ca1;
                        workSheet.Cells[6 + i, 8].Style = styleca;
                        workSheet.Cells[6 + i, 9].Value = listResult[i].N02_Ca2;
                        workSheet.Cells[6 + i, 9].Style = styleca;

                        workSheet.Cells[6 + i, 10].Value = listResult[i].N03_Ca1;
                        workSheet.Cells[6 + i, 10].Style = styleca;
                        workSheet.Cells[6 + i, 11].Value = listResult[i].N03_Ca2;
                        workSheet.Cells[6 + i, 11].Style = styleca;

                        workSheet.Cells[6 + i, 12].Value = listResult[i].N04_Ca1;
                        workSheet.Cells[6 + i, 12].Style = styleca;
                        workSheet.Cells[6 + i, 13].Value = listResult[i].N04_Ca2;
                        workSheet.Cells[6 + i, 13].Style = styleca;

                        workSheet.Cells[6 + i, 14].Value = listResult[i].N05_Ca1;
                        workSheet.Cells[6 + i, 14].Style = styleca;
                        workSheet.Cells[6 + i, 15].Value = listResult[i].N05_Ca2;
                        workSheet.Cells[6 + i, 15].Style = styleca;

                        workSheet.Cells[6 + i, 16].Value = listResult[i].N06_Ca1;
                        workSheet.Cells[6 + i, 16].Style = styleca;
                        workSheet.Cells[6 + i, 17].Value = listResult[i].N06_Ca2;
                        workSheet.Cells[6 + i, 17].Style = styleca;

                        workSheet.Cells[6 + i, 18].Value = listResult[i].N07_Ca1;
                        workSheet.Cells[6 + i, 18].Style = styleca;
                        workSheet.Cells[6 + i, 19].Value = listResult[i].N07_Ca2;
                        workSheet.Cells[6 + i, 19].Style = styleca;

                        workSheet.Cells[6 + i, 20].Value = listResult[i].N08_Ca1;
                        workSheet.Cells[6 + i, 20].Style = styleca;
                        workSheet.Cells[6 + i, 21].Value = listResult[i].N08_Ca2;
                        workSheet.Cells[6 + i, 21].Style = styleca;

                        workSheet.Cells[6 + i, 22].Value = listResult[i].N09_Ca1;
                        workSheet.Cells[6 + i, 22].Style = styleca;
                        workSheet.Cells[6 + i, 23].Value = listResult[i].N09_Ca2;
                        workSheet.Cells[6 + i, 23].Style = styleca;

                        workSheet.Cells[6 + i, 24].Value = listResult[i].N10_Ca1;
                        workSheet.Cells[6 + i, 24].Style = styleca;
                        workSheet.Cells[6 + i, 25].Value = listResult[i].N10_Ca2;
                        workSheet.Cells[6 + i, 25].Style = styleca;

                        workSheet.Cells[6 + i, 26].Value = listResult[i].N11_Ca1;
                        workSheet.Cells[6 + i, 26].Style = styleca;
                        workSheet.Cells[6 + i, 27].Value = listResult[i].N11_Ca2;
                        workSheet.Cells[6 + i, 27].Style = styleca;

                        workSheet.Cells[6 + i, 28].Value = listResult[i].N12_Ca1;
                        workSheet.Cells[6 + i, 28].Style = styleca;
                        workSheet.Cells[6 + i, 29].Value = listResult[i].N12_Ca2;
                        workSheet.Cells[6 + i, 29].Style = styleca;

                        workSheet.Cells[6 + i, 30].Value = listResult[i].N13_Ca1;
                        workSheet.Cells[6 + i, 30].Style = styleca;
                        workSheet.Cells[6 + i, 31].Value = listResult[i].N13_Ca2;
                        workSheet.Cells[6 + i, 31].Style = styleca;

                        workSheet.Cells[6 + i, 32].Value = listResult[i].N14_Ca1;
                        workSheet.Cells[6 + i, 32].Style = styleca;
                        workSheet.Cells[6 + i, 33].Value = listResult[i].N14_Ca2;
                        workSheet.Cells[6 + i, 33].Style = styleca;

                        workSheet.Cells[6 + i, 34].Value = listResult[i].N15_Ca1;
                        workSheet.Cells[6 + i, 34].Style = styleca;
                        workSheet.Cells[6 + i, 35].Value = listResult[i].N15_Ca2;
                        workSheet.Cells[6 + i, 35].Style = styleca;

                        workSheet.Cells[6 + i, 36].Value = listResult[i].N16_Ca1;
                        workSheet.Cells[6 + i, 36].Style = styleca;
                        workSheet.Cells[6 + i, 37].Value = listResult[i].N16_Ca2;
                        workSheet.Cells[6 + i, 37].Style = styleca;

                        workSheet.Cells[6 + i, 38].Value = listResult[i].N17_Ca1;
                        workSheet.Cells[6 + i, 38].Style = styleca;
                        workSheet.Cells[6 + i, 39].Value = listResult[i].N17_Ca2;
                        workSheet.Cells[6 + i, 39].Style = styleca;

                        workSheet.Cells[6 + i, 40].Value = listResult[i].N18_Ca1;
                        workSheet.Cells[6 + i, 40].Style = styleca;
                        workSheet.Cells[6 + i, 41].Value = listResult[i].N18_Ca2;
                        workSheet.Cells[6 + i, 41].Style = styleca;

                        workSheet.Cells[6 + i, 42].Value = listResult[i].N19_Ca1;
                        workSheet.Cells[6 + i, 42].Style = styleca;
                        workSheet.Cells[6 + i, 43].Value = listResult[i].N19_Ca2;
                        workSheet.Cells[6 + i, 43].Style = styleca;

                        workSheet.Cells[6 + i, 44].Value = listResult[i].N20_Ca1;
                        workSheet.Cells[6 + i, 44].Style = styleca;
                        workSheet.Cells[6 + i, 45].Value = listResult[i].N20_Ca2;
                        workSheet.Cells[6 + i, 45].Style = styleca;

                        workSheet.Cells[6 + i, 46].Value = listResult[i].N21_Ca1;
                        workSheet.Cells[6 + i, 46].Style = styleca;
                        workSheet.Cells[6 + i, 47].Value = listResult[i].N21_Ca2;
                        workSheet.Cells[6 + i, 47].Style = styleca;

                        workSheet.Cells[6 + i, 48].Value = listResult[i].N22_Ca1;
                        workSheet.Cells[6 + i, 48].Style = styleca;
                        workSheet.Cells[6 + i, 49].Value = listResult[i].N22_Ca2;
                        workSheet.Cells[6 + i, 49].Style = styleca;

                        workSheet.Cells[6 + i, 50].Value = listResult[i].N23_Ca1;
                        workSheet.Cells[6 + i, 50].Style = styleca;
                        workSheet.Cells[6 + i, 51].Value = listResult[i].N23_Ca2;
                        workSheet.Cells[6 + i, 51].Style = styleca;

                        workSheet.Cells[6 + i, 52].Value = listResult[i].N24_Ca1;
                        workSheet.Cells[6 + i, 52].Style = styleca;
                        workSheet.Cells[6 + i, 53].Value = listResult[i].N24_Ca2;
                        workSheet.Cells[6 + i, 53].Style = styleca;

                        workSheet.Cells[6 + i, 54].Value = listResult[i].N25_Ca1;
                        workSheet.Cells[6 + i, 54].Style = styleca;
                        workSheet.Cells[6 + i, 55].Value = listResult[i].N25_Ca2;
                        workSheet.Cells[6 + i, 55].Style = styleca;

                        workSheet.Cells[6 + i, 56].Value = listResult[i].N26_Ca1;
                        workSheet.Cells[6 + i, 56].Style = styleca;
                        workSheet.Cells[6 + i, 57].Value = listResult[i].N26_Ca2;
                        workSheet.Cells[6 + i, 57].Style = styleca;

                        workSheet.Cells[6 + i, 58].Value = listResult[i].N27_Ca1;
                        workSheet.Cells[6 + i, 58].Style = styleca;
                        workSheet.Cells[6 + i, 59].Value = listResult[i].N27_Ca2;
                        workSheet.Cells[6 + i, 59].Style = styleca;

                        workSheet.Cells[6 + i, 60].Value = listResult[i].N28_Ca1;
                        workSheet.Cells[6 + i, 60].Style = styleca;
                        workSheet.Cells[6 + i, 61].Value = listResult[i].N28_Ca2;
                        workSheet.Cells[6 + i, 61].Style = styleca;

                        if (day >= 29)
                        {
                            workSheet.Cells[6 + i, 62].Value = listResult[i].N29_Ca1;
                            workSheet.Cells[6 + i, 62].Style = styleca;
                            workSheet.Cells[6 + i, 63].Value = listResult[i].N29_Ca2;
                            workSheet.Cells[6 + i, 63].Style = styleca;
                        }

                        if (day >= 30)
                        {
                            workSheet.Cells[6 + i, 64].Value = listResult[i].N30_Ca1;
                            workSheet.Cells[6 + i, 64].Style = styleca;
                            workSheet.Cells[6 + i, 65].Value = listResult[i].N30_Ca2;
                            workSheet.Cells[6 + i, 65].Style = styleca;
                        }

                        if (day == 31)
                        {
                            workSheet.Cells[6 + i, 66].Value = listResult[i].N31_Ca1;
                            workSheet.Cells[6 + i, 66].Style = styleca;
                            workSheet.Cells[6 + i, 67].Value = listResult[i].N31_Ca2;
                            workSheet.Cells[6 + i, 67].Style = styleca;
                        }
                        break;
                    case 3:
                        workSheet.Cells[6 + i, 6].Value = listResult[i].N01_Ca1;
                        workSheet.Cells[6 + i, 6].Style = styleca;
                        workSheet.Cells[6 + i, 7].Value = listResult[i].N01_Ca2;
                        workSheet.Cells[6 + i, 7].Style = styleca;
                        workSheet.Cells[6 + i, 8].Value = listResult[i].N01_Ca3;
                        workSheet.Cells[6 + i, 8].Style = styleca;

                        workSheet.Cells[6 + i, 9].Value = listResult[i].N02_Ca1;
                        workSheet.Cells[6 + i, 9].Style = styleca;
                        workSheet.Cells[6 + i, 10].Value = listResult[i].N02_Ca2;
                        workSheet.Cells[6 + i, 10].Style = styleca;
                        workSheet.Cells[6 + i, 11].Value = listResult[i].N02_Ca3;
                        workSheet.Cells[6 + i, 11].Style = styleca;

                        workSheet.Cells[6 + i, 12].Value = listResult[i].N03_Ca1;
                        workSheet.Cells[6 + i, 12].Style = styleca;
                        workSheet.Cells[6 + i, 13].Value = listResult[i].N03_Ca2;
                        workSheet.Cells[6 + i, 13].Style = styleca;
                        workSheet.Cells[6 + i, 14].Value = listResult[i].N03_Ca3;
                        workSheet.Cells[6 + i, 14].Style = styleca;

                        workSheet.Cells[6 + i, 15].Value = listResult[i].N04_Ca1;
                        workSheet.Cells[6 + i, 15].Style = styleca;
                        workSheet.Cells[6 + i, 16].Value = listResult[i].N04_Ca2;
                        workSheet.Cells[6 + i, 16].Style = styleca;
                        workSheet.Cells[6 + i, 17].Value = listResult[i].N04_Ca3;
                        workSheet.Cells[6 + i, 17].Style = styleca;

                        workSheet.Cells[6 + i, 18].Value = listResult[i].N05_Ca1;
                        workSheet.Cells[6 + i, 18].Style = styleca;
                        workSheet.Cells[6 + i, 19].Value = listResult[i].N05_Ca2;
                        workSheet.Cells[6 + i, 19].Style = styleca;
                        workSheet.Cells[6 + i, 20].Value = listResult[i].N05_Ca3;
                        workSheet.Cells[6 + i, 20].Style = styleca;

                        workSheet.Cells[6 + i, 21].Value = listResult[i].N06_Ca1;
                        workSheet.Cells[6 + i, 21].Style = styleca;
                        workSheet.Cells[6 + i, 22].Value = listResult[i].N06_Ca2;
                        workSheet.Cells[6 + i, 22].Style = styleca;
                        workSheet.Cells[6 + i, 23].Value = listResult[i].N06_Ca3;
                        workSheet.Cells[6 + i, 23].Style = styleca;

                        workSheet.Cells[6 + i, 24].Value = listResult[i].N07_Ca1;
                        workSheet.Cells[6 + i, 24].Style = styleca;
                        workSheet.Cells[6 + i, 25].Value = listResult[i].N07_Ca2;
                        workSheet.Cells[6 + i, 25].Style = styleca;
                        workSheet.Cells[6 + i, 26].Value = listResult[i].N07_Ca3;
                        workSheet.Cells[6 + i, 26].Style = styleca;

                        workSheet.Cells[6 + i, 27].Value = listResult[i].N08_Ca1;
                        workSheet.Cells[6 + i, 27].Style = styleca;
                        workSheet.Cells[6 + i, 28].Value = listResult[i].N08_Ca2;
                        workSheet.Cells[6 + i, 28].Style = styleca;
                        workSheet.Cells[6 + i, 29].Value = listResult[i].N08_Ca3;
                        workSheet.Cells[6 + i, 29].Style = styleca;

                        workSheet.Cells[6 + i, 30].Value = listResult[i].N09_Ca1;
                        workSheet.Cells[6 + i, 30].Style = styleca;
                        workSheet.Cells[6 + i, 31].Value = listResult[i].N09_Ca2;
                        workSheet.Cells[6 + i, 31].Style = styleca;
                        workSheet.Cells[6 + i, 32].Value = listResult[i].N09_Ca3;
                        workSheet.Cells[6 + i, 32].Style = styleca;

                        workSheet.Cells[6 + i, 33].Value = listResult[i].N10_Ca1;
                        workSheet.Cells[6 + i, 33].Style = styleca;
                        workSheet.Cells[6 + i, 34].Value = listResult[i].N10_Ca2;
                        workSheet.Cells[6 + i, 34].Style = styleca;
                        workSheet.Cells[6 + i, 35].Value = listResult[i].N10_Ca3;
                        workSheet.Cells[6 + i, 35].Style = styleca;

                        workSheet.Cells[6 + i, 36].Value = listResult[i].N11_Ca1;
                        workSheet.Cells[6 + i, 36].Style = styleca;
                        workSheet.Cells[6 + i, 37].Value = listResult[i].N11_Ca2;
                        workSheet.Cells[6 + i, 37].Style = styleca;
                        workSheet.Cells[6 + i, 38].Value = listResult[i].N11_Ca3;
                        workSheet.Cells[6 + i, 38].Style = styleca;

                        workSheet.Cells[6 + i, 39].Value = listResult[i].N12_Ca1;
                        workSheet.Cells[6 + i, 39].Style = styleca;
                        workSheet.Cells[6 + i, 40].Value = listResult[i].N12_Ca2;
                        workSheet.Cells[6 + i, 40].Style = styleca;
                        workSheet.Cells[6 + i, 41].Value = listResult[i].N12_Ca3;
                        workSheet.Cells[6 + i, 41].Style = styleca;

                        workSheet.Cells[6 + i, 42].Value = listResult[i].N13_Ca1;
                        workSheet.Cells[6 + i, 42].Style = styleca;
                        workSheet.Cells[6 + i, 43].Value = listResult[i].N13_Ca2;
                        workSheet.Cells[6 + i, 43].Style = styleca;
                        workSheet.Cells[6 + i, 44].Value = listResult[i].N13_Ca3;
                        workSheet.Cells[6 + i, 44].Style = styleca;

                        workSheet.Cells[6 + i, 45].Value = listResult[i].N14_Ca1;
                        workSheet.Cells[6 + i, 45].Style = styleca;
                        workSheet.Cells[6 + i, 46].Value = listResult[i].N14_Ca2;
                        workSheet.Cells[6 + i, 46].Style = styleca;
                        workSheet.Cells[6 + i, 47].Value = listResult[i].N14_Ca3;
                        workSheet.Cells[6 + i, 47].Style = styleca;

                        workSheet.Cells[6 + i, 48].Value = listResult[i].N15_Ca1;
                        workSheet.Cells[6 + i, 48].Style = styleca;
                        workSheet.Cells[6 + i, 49].Value = listResult[i].N15_Ca2;
                        workSheet.Cells[6 + i, 49].Style = styleca;
                        workSheet.Cells[6 + i, 50].Value = listResult[i].N15_Ca3;
                        workSheet.Cells[6 + i, 50].Style = styleca;

                        workSheet.Cells[6 + i, 51].Value = listResult[i].N16_Ca1;
                        workSheet.Cells[6 + i, 51].Style = styleca;
                        workSheet.Cells[6 + i, 52].Value = listResult[i].N16_Ca2;
                        workSheet.Cells[6 + i, 52].Style = styleca;
                        workSheet.Cells[6 + i, 53].Value = listResult[i].N16_Ca3;
                        workSheet.Cells[6 + i, 53].Style = styleca;

                        workSheet.Cells[6 + i, 54].Value = listResult[i].N17_Ca1;
                        workSheet.Cells[6 + i, 54].Style = styleca;
                        workSheet.Cells[6 + i, 55].Value = listResult[i].N17_Ca2;
                        workSheet.Cells[6 + i, 55].Style = styleca;
                        workSheet.Cells[6 + i, 56].Value = listResult[i].N17_Ca3;
                        workSheet.Cells[6 + i, 56].Style = styleca;

                        workSheet.Cells[6 + i, 57].Value = listResult[i].N18_Ca1;
                        workSheet.Cells[6 + i, 57].Style = styleca;
                        workSheet.Cells[6 + i, 58].Value = listResult[i].N18_Ca2;
                        workSheet.Cells[6 + i, 58].Style = styleca;
                        workSheet.Cells[6 + i, 59].Value = listResult[i].N18_Ca3;
                        workSheet.Cells[6 + i, 59].Style = styleca;

                        workSheet.Cells[6 + i, 60].Value = listResult[i].N19_Ca1;
                        workSheet.Cells[6 + i, 60].Style = styleca;
                        workSheet.Cells[6 + i, 61].Value = listResult[i].N19_Ca2;
                        workSheet.Cells[6 + i, 61].Style = styleca;
                        workSheet.Cells[6 + i, 62].Value = listResult[i].N19_Ca3;
                        workSheet.Cells[6 + i, 62].Style = styleca;

                        workSheet.Cells[6 + i, 63].Value = listResult[i].N20_Ca1;
                        workSheet.Cells[6 + i, 63].Style = styleca;
                        workSheet.Cells[6 + i, 64].Value = listResult[i].N20_Ca2;
                        workSheet.Cells[6 + i, 64].Style = styleca;
                        workSheet.Cells[6 + i, 65].Value = listResult[i].N20_Ca3;
                        workSheet.Cells[6 + i, 65].Style = styleca;

                        workSheet.Cells[6 + i, 66].Value = listResult[i].N21_Ca1;
                        workSheet.Cells[6 + i, 66].Style = styleca;
                        workSheet.Cells[6 + i, 67].Value = listResult[i].N21_Ca2;
                        workSheet.Cells[6 + i, 67].Style = styleca;
                        workSheet.Cells[6 + i, 68].Value = listResult[i].N21_Ca3;
                        workSheet.Cells[6 + i, 68].Style = styleca;

                        workSheet.Cells[6 + i, 69].Value = listResult[i].N22_Ca1;
                        workSheet.Cells[6 + i, 69].Style = styleca;
                        workSheet.Cells[6 + i, 70].Value = listResult[i].N22_Ca2;
                        workSheet.Cells[6 + i, 70].Style = styleca;
                        workSheet.Cells[6 + i, 71].Value = listResult[i].N22_Ca3;
                        workSheet.Cells[6 + i, 71].Style = styleca;

                        workSheet.Cells[6 + i, 72].Value = listResult[i].N23_Ca1;
                        workSheet.Cells[6 + i, 72].Style = styleca;
                        workSheet.Cells[6 + i, 73].Value = listResult[i].N23_Ca2;
                        workSheet.Cells[6 + i, 73].Style = styleca;
                        workSheet.Cells[6 + i, 74].Value = listResult[i].N23_Ca3;
                        workSheet.Cells[6 + i, 74].Style = styleca;

                        workSheet.Cells[6 + i, 75].Value = listResult[i].N24_Ca1;
                        workSheet.Cells[6 + i, 75].Style = styleca;
                        workSheet.Cells[6 + i, 76].Value = listResult[i].N24_Ca2;
                        workSheet.Cells[6 + i, 76].Style = styleca;
                        workSheet.Cells[6 + i, 77].Value = listResult[i].N24_Ca3;
                        workSheet.Cells[6 + i, 77].Style = styleca;

                        workSheet.Cells[6 + i, 78].Value = listResult[i].N25_Ca1;
                        workSheet.Cells[6 + i, 78].Style = styleca;
                        workSheet.Cells[6 + i, 79].Value = listResult[i].N25_Ca2;
                        workSheet.Cells[6 + i, 79].Style = styleca;
                        workSheet.Cells[6 + i, 80].Value = listResult[i].N25_Ca3;
                        workSheet.Cells[6 + i, 80].Style = styleca;

                        workSheet.Cells[6 + i, 81].Value = listResult[i].N26_Ca1;
                        workSheet.Cells[6 + i, 81].Style = styleca;
                        workSheet.Cells[6 + i, 82].Value = listResult[i].N26_Ca2;
                        workSheet.Cells[6 + i, 82].Style = styleca;
                        workSheet.Cells[6 + i, 83].Value = listResult[i].N26_Ca3;
                        workSheet.Cells[6 + i, 83].Style = styleca;

                        workSheet.Cells[6 + i, 84].Value = listResult[i].N27_Ca1;
                        workSheet.Cells[6 + i, 84].Style = styleca;
                        workSheet.Cells[6 + i, 85].Value = listResult[i].N27_Ca2;
                        workSheet.Cells[6 + i, 85].Style = styleca;
                        workSheet.Cells[6 + i, 86].Value = listResult[i].N27_Ca3;
                        workSheet.Cells[6 + i, 86].Style = styleca;

                        workSheet.Cells[6 + i, 87].Value = listResult[i].N28_Ca1;
                        workSheet.Cells[6 + i, 87].Style = styleca;
                        workSheet.Cells[6 + i, 88].Value = listResult[i].N28_Ca2;
                        workSheet.Cells[6 + i, 88].Style = styleca;
                        workSheet.Cells[6 + i, 89].Value = listResult[i].N28_Ca3;
                        workSheet.Cells[6 + i, 89].Style = styleca;

                        if (day >= 29)
                        {
                            workSheet.Cells[6 + i, 90].Value = listResult[i].N29_Ca1;
                            workSheet.Cells[6 + i, 90].Style = styleca;
                            workSheet.Cells[6 + i, 91].Value = listResult[i].N29_Ca2;
                            workSheet.Cells[6 + i, 91].Style = styleca;
                            workSheet.Cells[6 + i, 92].Value = listResult[i].N29_Ca3;
                            workSheet.Cells[6 + i, 92].Style = styleca;
                        }

                        if (day >= 30)
                        {
                            workSheet.Cells[6 + i, 93].Value = listResult[i].N30_Ca1;
                            workSheet.Cells[6 + i, 93].Style = styleca;
                            workSheet.Cells[6 + i, 94].Value = listResult[i].N30_Ca2;
                            workSheet.Cells[6 + i, 94].Style = styleca;
                            workSheet.Cells[6 + i, 95].Value = listResult[i].N30_Ca3;
                            workSheet.Cells[6 + i, 95].Style = styleca;
                        }

                        if (day == 31)
                        {
                            workSheet.Cells[6 + i, 96].Value = listResult[i].N31_Ca1;
                            workSheet.Cells[6 + i, 96].Style = styleca;
                            workSheet.Cells[6 + i, 97].Value = listResult[i].N31_Ca2;
                            workSheet.Cells[6 + i, 97].Style = styleca;
                            workSheet.Cells[6 + i, 98].Value = listResult[i].N31_Ca3;
                            workSheet.Cells[6 + i, 98].Style = styleca;
                        }
                        break;
                }
            }

            var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx");
            xlWorkBook.Save(tempFile);
            MemoryStream obj_stream = new MemoryStream();
            obj_stream = new MemoryStream(System.IO.File.ReadAllBytes(tempFile));
            _tempFileCacheManager.SetFile(file.FileToken, obj_stream.ToArray());
            System.IO.File.Delete(tempFile);
            obj_stream.Position = 0;
            return file;
        }
    }
}
