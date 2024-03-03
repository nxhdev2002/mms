using Abp.Application.Services.Dto;
using Abp.AspNetZeroCore.Net;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Abp.UI;
using FastMember;
using GemBox.Spreadsheet;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using prod.Authorization;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using prod.Inventory.CKD.Exporting;
using prod.Inventory.Gps.PartList.Dto;
using prod.Inventory.Gps.PartList.Exporting;
using prod.Inventory.GPS;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using prod.Common;

namespace prod.Inventory.Gps.PartList
{
    [AbpAuthorize(AppPermissions.Pages_Gps_Master_PartList_View)]
    public class InvGpsPartListAppService : prodAppServiceBase, IInvGpsPartListAppService
    {
        private readonly IDapperRepository<InvGpsPartList, long> _dapperRepo;
        private readonly IDapperRepository<InvGpsPartGrade, long> _dapperRepoGrade;
        private readonly IRepository<InvGpsPartList, long> _repo;
        private readonly IInvGpsPartListExcelExporter _calendarListExcelExporter;
        private readonly ITempFileCacheManager _tempFileCacheManager;


        public InvGpsPartListAppService(IRepository<InvGpsPartList, long> repo,
                                        IDapperRepository<InvGpsPartList, long> dapperRepo,
                                        IDapperRepository<InvGpsPartGrade, long> dapperRepoGrade,
                                        IInvGpsPartListExcelExporter calendarListExcelExporter,
                                        ITempFileCacheManager tempFileCacheManager
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter =  calendarListExcelExporter;
            _dapperRepoGrade = dapperRepoGrade;
            _tempFileCacheManager = tempFileCacheManager;
        }
        public async Task<PagedResultDto<InvGpsPartListDto>> GetGpsPartList(GetInvGpsPartListInput input)
        {
            string _sql = "Exec INV_GPS_PART_LIST_SEARCH @p_partno, @p_supplier_no, @p_ispartcolor";

            IEnumerable<InvGpsPartListDto> result = await _dapperRepo.QueryAsync<InvGpsPartListDto>(_sql, new
            {
                p_partno = input.PartNo,
                p_supplier_no = input.SupplierNo,
                p_ispartcolor = input.IsPartColor
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvGpsPartListDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<PagedResultDto<InvGpsPartGradeByPartListDto>> GetGpsPartGradeByPartList(GetInvGpsPartGradeByPartListInput input)
        {
            string _sql = "Exec INV_GPS_PART_GRADE_BY_PARTLIST @PartListId";

            IEnumerable<InvGpsPartGradeByPartListDto> result = await _dapperRepoGrade.QueryAsync<InvGpsPartGradeByPartListDto>(_sql, new
            {
                PartListId = input.PartListId
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvGpsPartGradeByPartListDto>(
                totalCount,
                pagedAndFiltered);
        }

        [AbpAuthorize(AppPermissions.Pages_Gps_Master_PartList_Import)]
        public async Task<List<InvGpsPartListImportDto>> GetInvGpsPartListFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<InvGpsPartListImportDto> rowList = new List<InvGpsPartListImportDto>();
                CommonFunction fn = new CommonFunction();

                using (var stream = new MemoryStream(fileBytes))
                {
                    SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
                    var xlWorkBook = ExcelFile.Load(stream);
                    var v_worksheet = xlWorkBook.Worksheets[0];
                    DataFormatter formatter = new DataFormatter();
                    DateTime dateTime = DateTime.Now;

                    //  string v_devanning_date = (v_worksheet.Cells[4, 2]).Value?.ToString() ?? "";
                    string strGUID = Guid.NewGuid().ToString("N");
                    int countGrade = 0;
                    for (int g = 4; g <= 500; g++)
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(v_worksheet.Cells[5, g].Value)))
                        {
                            countGrade++;
                        }
                        else { break; }

                    }

                    for (int i = 8; i <= v_worksheet.Rows.Count; i++)
                    {

                        var v_PartNo = Convert.ToString(v_worksheet.Cells[i, 1].Value);
                        if (v_PartNo != null && v_PartNo != "")
                        {
                            var v_PartName = Convert.ToString(v_worksheet.Cells[i, 2].Value);

                            var v_UOM = Convert.ToString(v_worksheet.Cells[i, countGrade + 5].Value);
                            var v_BoxQty = Convert.ToInt32(v_worksheet.Cells[i, countGrade + 6].Value);
                            var v_Remark1 = Convert.ToString(v_worksheet.Cells[i, countGrade + 7].Value);
                            var v_ProcessUse = Convert.ToString(v_worksheet.Cells[i, countGrade + 8].Value);
                            var v_Type = Convert.ToString(v_worksheet.Cells[i, countGrade + 9].Value);

                            var v_SeasonType = Convert.ToString(v_worksheet.Cells[i, countGrade + 11].Value);
                            var v_WinterRadio = Convert.ToDecimal(v_worksheet.Cells[i, countGrade + 12].Value);
                            var v_SummerRatio = Convert.ToDecimal(v_worksheet.Cells[i, countGrade + 13].Value);
                            var v_DiffRatio = Convert.ToDecimal(v_worksheet.Cells[i, countGrade + 15].Value);
                            var v_Remark = Convert.ToString(v_worksheet.Cells[i, countGrade + 16].Value);

                            for (int j = 4; j < countGrade + 4; j++)
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(v_worksheet.Cells[5, j].Value)))
                                {
                                    InvGpsPartListImportDto importData = new InvGpsPartListImportDto();

                                    decimal eUsageQty = 0;
                                    decimal.TryParse(Convert.ToString(v_worksheet.Cells[i, j].Value), out eUsageQty);
                                   
                                        importData.Guid = strGUID;
                                        importData.PartNo = v_PartNo;
                                        importData.PartNoNormalized = v_PartNo.Replace("-", "");
                                        importData.PartName = v_PartName;
                                        importData.SupplierNo = "";

                                        importData.Uom = v_UOM;
                                        importData.BoxQty = v_BoxQty;
                                        importData.Remark1 = v_Remark1;
                                        importData.ProcessUse = v_ProcessUse;
                                        importData.Type = v_Type;

                                        importData.SeasonType = v_SeasonType;
                                        importData.WinterRatio = v_WinterRadio;
                                        importData.SummerRadio = v_SummerRatio;
                                        importData.DiffRatio = v_DiffRatio;
                                        importData.Remark = v_Remark;

                                        importData.Grade = Convert.ToString(v_worksheet.Cells[5, j].Value);
                                        importData.BodyColor = Convert.ToString(v_worksheet.Cells[6, j].Value);

                                        if (Convert.ToDecimal(v_worksheet.Cells[i, j].Value) < 0)
                                        {
                                            importData.ErrorDescription = "Usage Qty không được âm";
                                        }
                                        else
                                        {
                                            importData.UsageQty = Convert.ToDecimal(v_worksheet.Cells[i, j].Value);
                                        }

                                        importData.IsPartColor = "Y";

                                        importData.CreationTime = dateTime;
                                        importData.CreatorUserId = (int)AbpSession.UserId;
                                        importData.IsDeleted = 0;

                                        rowList.Add(importData);
                                    
                                    //else
                                    //{
                                    //    importData.Guid = strGUID;
                                    //    importData.PartNo = v_PartNo;
                                    //    importData.PartNoNormalized = v_PartNo.Replace("-", "");
                                    //    importData.PartName = v_PartName;
                                    //    importData.SupplierNo = "";

                                    //    importData.Uom = v_UOM;
                                    //    importData.BoxQty = v_BoxQty;
                                    //    importData.Remark1 = v_Remark1;
                                    //    importData.ProcessUse = v_ProcessUse;
                                    //    importData.Type = v_Type;

                                    //    importData.SeasonType = v_SeasonType;
                                    //    importData.WinterRatio = v_WinterRadio;
                                    //    importData.SummerRadio = v_SummerRatio;
                                    //    importData.DiffRatio = v_DiffRatio;
                                    //    importData.Remark = v_Remark;
                                    //    importData.IsPartColor = "Y";

                                    //    importData.Grade = Convert.ToString(v_worksheet.Cells[5, j].Value);

                                    //    importData.ErrorDescription = "";

                                    //    importData.CreationTime = dateTime;
                                    //    importData.CreatorUserId = (int)AbpSession.UserId;
                                    //    importData.IsDeleted = 0;

                                    //    rowList.Add(importData);
                                    //}
                                }

                            }

                        }
                    }
                }

                // import temp into db (bulkCopy)
                if (rowList.Count > 0)
                {
                    IEnumerable<InvGpsPartListImportDto> dataE = rowList.AsEnumerable();
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
                                bulkCopy.DestinationTableName = "InvGpsGentani_T";

                                bulkCopy.ColumnMappings.Add("Guid", "Guid");
                                bulkCopy.ColumnMappings.Add("PartNo", "PartNo");
                                bulkCopy.ColumnMappings.Add("PartNoNormalized", "PartNoNormalized");
                                bulkCopy.ColumnMappings.Add("PartName", "PartName");
                                bulkCopy.ColumnMappings.Add("SupplierNo", "SupplierNo");

                                bulkCopy.ColumnMappings.Add("Uom", "Uom");
                                bulkCopy.ColumnMappings.Add("BoxQty", "BoxQty");
                                bulkCopy.ColumnMappings.Add("Remark1", "Remark1");
                                bulkCopy.ColumnMappings.Add("ProcessUse", "ProcessUse");
                                bulkCopy.ColumnMappings.Add("Type", "Type");

                                bulkCopy.ColumnMappings.Add("SeasonType", "SeasonType");
                                bulkCopy.ColumnMappings.Add("WinterRatio", "WinterRatio");
                                bulkCopy.ColumnMappings.Add("SummerRadio", "SummerRadio");
                                bulkCopy.ColumnMappings.Add("DiffRatio", "DiffRatio");
                                bulkCopy.ColumnMappings.Add("Remark", "Remark");
                                bulkCopy.ColumnMappings.Add("IsPartColor", "IsPartColor");

                                bulkCopy.ColumnMappings.Add("Grade", "Grade");
                                bulkCopy.ColumnMappings.Add("BodyColor", "BodyColor");
                                bulkCopy.ColumnMappings.Add("UsageQty", "UsageQty");

                                bulkCopy.ColumnMappings.Add("CreationTime", "CreationTime");
                                bulkCopy.ColumnMappings.Add("CreatorUserId", "CreatorUserId");
                                bulkCopy.ColumnMappings.Add("IsDeleted", "IsDeleted");

                                bulkCopy.ColumnMappings.Add("ErrorDescription", "ErrorDescription");
                                bulkCopy.WriteToServer(table);
                                tran.Commit();
                            }
                        }
                        await conn.CloseAsync();
                    }
                }
                return rowList;

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.ToString());
                // return ex;
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Gps_Master_PartList_Import)]
        public async Task<List<InvGpsPartListImportDto>> GetInvGpsPartListNoColorFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<InvGpsPartListImportDto> rowList = new List<InvGpsPartListImportDto>();

                using (var stream = new MemoryStream(fileBytes))
                {
                    SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
                    var xlWorkBook = ExcelFile.Load(stream);
                    var v_worksheet = xlWorkBook.Worksheets[0];
                    DataFormatter formatter = new DataFormatter();
                    DateTime dateTime = DateTime.Now;

                    //  string v_devanning_date = (v_worksheet.Cells[4, 2]).Value?.ToString() ?? "";
                    string strGUID = Guid.NewGuid().ToString("N");
                    int countGrade = 0;
                    for (int g = 13; g <= 500; g++)
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(v_worksheet.Cells[2, g].Value)))
                        {
                            countGrade++;
                        }
                        else { break; }

                    }

                    for (int i = 4; i <= v_worksheet.Rows.Count; i++)
                    {

                        var v_PartNo = Convert.ToString(v_worksheet.Cells[i, 3].Value);
                        if (v_PartNo != null && v_PartNo != "")
                        {
                            var v_PartName = Convert.ToString(v_worksheet.Cells[i, 4].Value);
                            var v_Supplier = Convert.ToString(v_worksheet.Cells[i, 5].Value);
                            var v_UOM = Convert.ToString(v_worksheet.Cells[i, 6].Value);
                            var v_MinLot = 0;
                            try
                            {
                                v_MinLot = int.Parse(Convert.ToString(v_worksheet.Cells[i, 7].Value));
                            }
                            catch
                            {

                            }
                            var v_Type = Convert.ToString(v_worksheet.Cells[i, 8].Value);

                            var v_Category = Convert.ToString(v_worksheet.Cells[i, 9].Value);
                            DateTime? v_start_date = null;
                            DateTime? v_to_date = null;

                            try
                            {
                                v_start_date = DateTime.Parse(Convert.ToString(v_worksheet.Cells[i, 10].Value));
                            }
                            catch
                            {

                            }

                            try
                            {
                                v_to_date = DateTime.Parse(Convert.ToString(v_worksheet.Cells[i, 11].Value));
                            }
                            catch
                            {

                            }



                            for (int j = 13; j < countGrade + 13; j++)
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(v_worksheet.Cells[5, j].Value)))
                                {
                                    InvGpsPartListImportDto importData = new InvGpsPartListImportDto();
                                    /*
                                                                        decimal eUsageQty = 0;
                                                                        decimal.TryParse(Convert.ToString(v_worksheet.Cells[i, j].Value), out eUsageQty);*/

                                    importData.Guid = strGUID;
                                    importData.PartNo = v_PartNo;
                                    importData.PartNoNormalized = v_PartNo.Replace("-", "");
                                    importData.PartName = v_PartName;
                                    importData.SupplierNo = v_Supplier;
                                    importData.Uom = v_UOM;
                                    importData.MinLot = v_MinLot;
                                    importData.Type = v_Type;
                                    importData.Category = v_Category;

                                    importData.StartDate = v_start_date;
                                    importData.EndDate = v_to_date;
                                    importData.Grade = Convert.ToString(v_worksheet.Cells[2, j].Value);
                                    importData.CreatorUserId = (int)AbpSession.UserId;
                                    importData.CreationTime = dateTime;
                                    importData.ErrorDescription = "";
                                    importData.IsDeleted = 0;
                                    importData.IsPartColor = "N";


                                    if (Convert.ToDecimal(v_worksheet.Cells[i, j].Value) < 0)
                                    {
                                        importData.ErrorDescription = "Usage Qty không được âm";
                                    }
                                    else
                                    {
                                        importData.UsageQty = Convert.ToDecimal(v_worksheet.Cells[i, j].Value);

                                    }

                                    rowList.Add(importData);
                                }

                            }

                        }
                    }
                }

                // import temp into db (bulkCopy)
                if (rowList.Count > 0)
                {
                    IEnumerable<InvGpsPartListImportDto> dataE = rowList.AsEnumerable();
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
                                bulkCopy.DestinationTableName = "InvGpsGentani_T";

                                bulkCopy.ColumnMappings.Add("Guid", "Guid");
                                bulkCopy.ColumnMappings.Add("PartNo", "PartNo");
                                bulkCopy.ColumnMappings.Add("PartNoNormalized", "PartNoNormalized");
                                bulkCopy.ColumnMappings.Add("PartName", "PartName");
                                bulkCopy.ColumnMappings.Add("SupplierNo", "SupplierNo");
                                bulkCopy.ColumnMappings.Add("Uom", "Uom");
                                bulkCopy.ColumnMappings.Add("MinLot", "MinLot");
                                bulkCopy.ColumnMappings.Add("Type", "Type");
                                bulkCopy.ColumnMappings.Add("Category", "Category");
                                bulkCopy.ColumnMappings.Add("StartDate", "StartDate");
                                bulkCopy.ColumnMappings.Add("EndDate", "EndDate");
                                bulkCopy.ColumnMappings.Add("Grade", "Grade");
                                bulkCopy.ColumnMappings.Add("UsageQty", "UsageQty");
                                bulkCopy.ColumnMappings.Add("IsPartColor", "IsPartColor");
                                bulkCopy.ColumnMappings.Add("CreationTime", "CreationTime");
                                bulkCopy.ColumnMappings.Add("CreatorUserId", "CreatorUserId");
                                bulkCopy.ColumnMappings.Add("IsDeleted", "IsDeleted");

                                bulkCopy.ColumnMappings.Add("ErrorDescription", "ErrorDescription");
                                bulkCopy.WriteToServer(table);
                                tran.Commit();
                            }
                        }
                        await conn.CloseAsync();
                    }
                }
                return rowList;

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.ToString());
                // return ex;
            }
        }


        //Merge Data 
        public async Task MergeDataInvGpsPartList(string v_Guid)
        {
            string _sql = "Exec INV_GPS_PART_LIST_MERGE @Guid";
            await _dapperRepo.QueryAsync<InvGpsPartListImportDto>(_sql, new { Guid = v_Guid });
        }

        public async Task MergeDataInvGpsPartListNoColor(string v_Guid)
        {
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            var userId = user.Id;
            var userName = user.UserName;

            string _sql = "Exec INV_GPS_PART_LIST_MERGE_NOCOLOR @Guid, @p_user";
            await _dapperRepo.QueryAsync<InvGpsPartListImportDto>(_sql, new
            {
                Guid = v_Guid,
                @p_user = userId
            });
        }

        //INV_GPS_GENTANI_GET_LIST_ERROR_IMPORT

        public async Task<FileDto> getExportInvGpsPartList(GetInvGpsPartListInputExport input)
        {
            string contentRootPath = "/Template/InvGpsPartList_.xlsx";
            string webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot") + contentRootPath;
            string pathExcelTemp = webRootPath;
            string pathExcel = "/Download/";
            string nameExcel = "InvGpsPartList" + DateTime.Now.ToString("MMddyyyy-HHmmss") + ".xlsx";
            string pathDownload = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot") + pathExcel + nameExcel;
            var fileDto = new FileDto(nameExcel, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);

            FileInfo finfo = new FileInfo(pathDownload);
            if (finfo.Exists) { try { finfo.Delete(); } catch (Exception ex) { } }

            XSSFWorkbook xlsxObject = null;     //XLSX
            ISheet sheet = null;
            IRow row;
            IRow rowModel;
            IRow rowGrade;
            IRow rowBodyColor;
            ICell cellData;
            ICell cellModel;
            ICell cellGrade;
            ICell cellBdColor;
            ICell cellNote;
            //
            ICell cellPartNo;
            ICell cellPartName;
            ICell cellUom;
            ICell cellBoxQty;
            ICell cellReamrk1;
            ICell cellProcessUse;
            ICell cellType;
            //
            ICell cellSeasonType;
            ICell cellWinterRadio;
            ICell cellSummerRadio;
            ICell cellDiffRadio;
            ICell cellDiffRadio1;
            ICell cellReamrk;


            // Lấy Object Excel (giữa xls và xlsx)
            using (FileStream file = new FileStream(pathExcelTemp, FileMode.Open, FileAccess.Read))
            {
                xlsxObject = new XSSFWorkbook(file);
            }

            // Lấy Object Sheet  
            sheet = xlsxObject.GetSheetAt(0);
            if (sheet == null) { return null; }

            ICellStyle istyle = xlsxObject.CreateCellStyle();
            istyle.FillPattern = FillPattern.SolidForeground;
            istyle.FillForegroundColor = IndexedColors.White.Index;
            istyle.BorderBottom = BorderStyle.Thin;
            istyle.BorderTop = BorderStyle.Thin;
            istyle.BorderLeft = BorderStyle.Thin;
            istyle.BorderRight = BorderStyle.Thin;

            try
            {
                string _sqlTitle = "Exec INV_GPS_PART_GRADE_EXPORT_TITLE @PartNo, @Grade";
                var data1 = await Task.Run(() => _dapperRepo.QueryAsync<InvGpsPartListTitleExportDto>(_sqlTitle, new
                {
                    PartNo = input.PartNo,
                    Grade = input.Grade
                }));
                var lstTitle = data1.ToList();

                string _sqlContent = "Exec INV_GPS_PART_LIST_EXPORT_DATA @PartNo, @Grade";
                var data2 = await Task.Run(() => _dapperRepo.QueryAsync<InvGpsPartListContentExportDto>(_sqlContent, new
                {
                    PartNo = input.PartNo,
                    Grade = input.Grade
                }));
                var lstContend = data2.ToList();

                InvGpsPartListContentExportDto[] arrFieldData = lstContend.ToArray();

                InvGpsPartListTitleExportDto[] arrFieldTitle = lstTitle.ToArray();

                if (arrFieldData.Length > 0)
                {
                    rowModel = sheet.GetRow(0);
                    rowGrade = sheet.GetRow(1);
                    rowBodyColor = sheet.GetRow(2);

                    int c = 4;
                    int r = 4;
                    int cTitle = arrFieldTitle.Length;

                    for (int i = 0; i < cTitle; i++)
                    {
                        cellModel = rowModel.CreateCell(c + i, CellType.String); cellModel.CellStyle = istyle;
                        cellGrade = rowGrade.CreateCell(c + i, CellType.String); cellGrade.CellStyle = istyle;
                        cellBdColor = rowBodyColor.CreateCell(c + i, CellType.String); cellBdColor.CellStyle = istyle;

                        cellModel.SetCellValue(arrFieldTitle[i].Model);
                        cellGrade.SetCellValue(arrFieldTitle[i].Grade);
                        cellBdColor.SetCellValue(arrFieldTitle[i].BodyColor);
                    }

                    //
                    //sheet.addMergedRegion(new CellRangeAddress(
                    //        1, //first row (0-based)
                    //        1, //last row  (0-based)
                    //        1, //first column (0-based)
                    //        2  //last column  (0-based)
                    //));

                    int a = 0; // số part + 1
                    for (int j = 0; j < arrFieldData.Length; j++)
                    {

                        row = sheet.GetRow(r + a);
                        if (j > 0 && arrFieldData[j].PartNo != arrFieldData[j - 1].PartNo)
                        {
                            a = a + 1;
                            //create row
                            row = sheet.CreateRow(r + a);
                        }

                        // data static
                        cellPartNo = row.CreateCell(1, CellType.String); cellPartNo.CellStyle = istyle;
                        cellPartNo.SetCellValue(arrFieldData[j].PartNo);
                        cellPartName = row.CreateCell(2, CellType.String); cellPartName.CellStyle = istyle;
                        cellPartName.SetCellValue(arrFieldData[j].PartName);
                        cellNote = row.CreateCell(3, CellType.String); cellNote.CellStyle = istyle;
                        cellNote.SetCellValue("");

                        cellUom = row.CreateCell(cTitle + 5, CellType.String); cellUom.CellStyle = istyle;
                        cellUom.SetCellValue(arrFieldData[j].Uom);
                        cellBoxQty = row.CreateCell(cTitle + 6, CellType.String); cellBoxQty.CellStyle = istyle;
                        cellBoxQty.SetCellValue(arrFieldData[j].BoxQty);
                        cellReamrk1 = row.CreateCell(cTitle + 7, CellType.String); cellReamrk1.CellStyle = istyle;
                        cellReamrk1.SetCellValue(arrFieldData[j].Remark1);
                        cellProcessUse = row.CreateCell(cTitle + 8, CellType.String); cellProcessUse.CellStyle = istyle;
                        cellProcessUse.SetCellValue(arrFieldData[j].ProcessUse);
                        cellType = row.CreateCell(cTitle + 9, CellType.String); cellType.CellStyle = istyle;
                        cellType.SetCellValue(arrFieldData[j].Type);

                        cellSeasonType = row.CreateCell(cTitle + 11, CellType.String); cellSeasonType.CellStyle = istyle;
                        cellSeasonType.SetCellValue(arrFieldData[j].SeasonType);
                        cellWinterRadio = row.CreateCell(cTitle + 12, CellType.String); cellWinterRadio.CellStyle = istyle;
                        cellWinterRadio.SetCellValue(arrFieldData[j].WinterRatio);
                        cellSummerRadio = row.CreateCell(cTitle + 13, CellType.String); cellSummerRadio.CellStyle = istyle;
                        cellSummerRadio.SetCellValue(arrFieldData[j].SummerRadio);
                        cellDiffRadio1 = row.CreateCell(cTitle + 14, CellType.String); cellDiffRadio1.CellStyle = istyle;
                        cellDiffRadio1.SetCellValue("");
                        cellDiffRadio = row.CreateCell(cTitle + 15, CellType.String); cellDiffRadio.CellStyle = istyle;
                        cellDiffRadio.SetCellValue(arrFieldData[j].DiffRatio);
                        cellReamrk = row.CreateCell(cTitle + 16, CellType.String); cellReamrk.CellStyle = istyle;
                        cellReamrk.SetCellValue(arrFieldData[j].Remark);

                        for (int i = 0; i < cTitle; i++)
                        {
                            if ((j == 0) || (j > 0 && arrFieldData[j].PartNo != arrFieldData[j - 1].PartNo))
                            {
                                // create cell
                                if (arrFieldData[j].Grade == arrFieldTitle[i].Grade
                                    && arrFieldData[j].BodyColor == arrFieldTitle[i].BodyColor)
                                {
                                    cellData = row.CreateCell(c + i, CellType.String); cellData.CellStyle = istyle;
                                    cellData.SetCellValue(arrFieldData[j].UsageQty);
                                }
                                else
                                {
                                    cellData = row.CreateCell(c + i, CellType.String); cellData.CellStyle = istyle;
                                    cellData.SetCellValue("");
                                }
                            }
                            else
                            {
                                // get cell
                                if (arrFieldData[j].Grade == arrFieldTitle[i].Grade
                                   && arrFieldData[j].BodyColor == arrFieldTitle[i].BodyColor)
                                {

                                    cellData = row.GetCell(c + i);
                                    cellData.SetCellValue(arrFieldData[j].UsageQty);
                                }

                            }

                        }
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
            catch (Exception ex) { }

            return await Task.FromResult(fileDto);
        }
        public async Task<PagedResultDto<InvGpsPartListImportDto>> GetMessageErrorImport(string v_Guid, string v_Screen)
        {
            string _sql = "Exec INV_GPS_PART_LIST_GET_LIST_ERROR_IMPORT @Guid";

            IEnumerable<InvGpsPartListImportDto> result = await _dapperRepo.QueryAsync<InvGpsPartListImportDto>(_sql, new
            {
                Guid = v_Guid
            });

            var listResult = result.ToList();
            var totalCount = listResult.Count();

            return new PagedResultDto<InvGpsPartListImportDto>(
                totalCount,
                listResult
               );
        }
        public async Task<FileDto> GetListErrToExcel(string v_Guid, string v_Screen)
        {
            FileDto a = new FileDto();
            string _sql = "Exec INV_GPS_PART_LIST_GET_LIST_ERROR_IMPORT @Guid";

            IEnumerable<InvGpsPartListImportDto> result = await _dapperRepo.QueryAsync<InvGpsPartListImportDto>(_sql, new
            {
                Guid = v_Guid
            });

            var exportToExcel = result.ToList();

            if (v_Screen == "Color")
            {
                a = _calendarListExcelExporter.ExportToFileErr(exportToExcel);
            }
            else if (v_Screen == "NoColor")
            {
                a = _calendarListExcelExporter.ExportToFileNoColorErr(exportToExcel);
            }

            return a;
            
        }


        [AbpAuthorize(AppPermissions.Pages_Gps_Master_PartList_Validate)]
        public async Task<PagedResultDto<ValidateGpsPartListDto>> GetValidateInvGpsPartList(PagedAndSortedResultRequestDto input)
        {
            string _sqlSearch = "Exec [INV_GPS_PART_LIST_VALIDATE]";

            IEnumerable<ValidateGpsPartListDto> result = await _dapperRepoGrade.QueryAsync<ValidateGpsPartListDto>(_sqlSearch, new { });
            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<ValidateGpsPartListDto>(
                totalCount,
                pagedAndFiltered);
        }


        public async Task<FileDto> GetValidateInvGpsPartListExcel()
        {

            string _sql = "Exec [INV_GPS_PART_LIST_VALIDATE]";

            IEnumerable<ValidateGpsPartListDto> result = await _dapperRepo.QueryAsync<ValidateGpsPartListDto>(_sql, new
            { });

            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportValidateToFile(exportToExcel);
        }

    }


}
