using Abp.Application.Services.Dto;
using Abp.AspNetZeroCore.Net;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.UI;
using FastMember;
using GemBox.Spreadsheet;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using prod.Authorization;
using prod.Common;
using prod.Dto;
using prod.HistoricalData;
using prod.Inventory.CKD.Dto;
using prod.Inventory.PIO.PartListInl;
using prod.Inventory.PIO.PartListInl.Dto;
using prod.Inventory.PIO.PartListInl.Export;
using prod.Inventory.PIO.PartListOff.Dto;
using prod.Inventory.PIO.PartListOff.Export;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.PIO.PartListOff
{
    [AbpAuthorize(AppPermissions.Pages_PIO_Master_PartListOff_View)]
    public class InvPartListOffAppService : prodAppServiceBase, IInvPartListOffAppService
    {

        private readonly IDapperRepository<InvPioPartListOff, long> _dapperRepo;
        private readonly IDapperRepository<InvPioPartGradeOff, long> _partGradeInl;
        private readonly IHistoricalDataAppService _historicalDataAppService;
        private readonly IInvPioPartListOffExcelExporter _calendarListExcelExporter;
        private readonly ITempFileCacheManager _tempFileCacheManager;
        public InvPartListOffAppService(
                                     IDapperRepository<InvPioPartListOff, long> dapperRepo,
                                    IDapperRepository<InvPioPartGradeOff, long> partGradeInl,
                                      IHistoricalDataAppService historicalDataAppService,
                                      IInvPioPartListOffExcelExporter calendarListExcelExporter,
                                      ITempFileCacheManager tempFileCacheManager
                                  )

        {
            _dapperRepo = dapperRepo;
            _partGradeInl = partGradeInl;
            _historicalDataAppService = historicalDataAppService;
            _calendarListExcelExporter = calendarListExcelExporter;
            _tempFileCacheManager = tempFileCacheManager;
        }



        public async Task<PagedResultDto<InvPioPartListOffDto>> GetAll(GetInvPioPartListOffInput input)
        {
            string _sql = "Exec INV_PIO_PART_LIST_OFFLINE_SEARCH @p_part_no,@p_cfc, @p_model, @p_grade, @p_shop, @p_supplier_no,@p_order_pattern,@p_active";

            IEnumerable<InvPioPartListOffDto> result = await _dapperRepo.QueryAsync<InvPioPartListOffDto>(_sql, new
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

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvPioPartListOffDto>(
                totalCount,
                pagedAndFiltered);
        }


        public async Task<ChangedRecordIdsDto> GetChangedRecords()
        {
            var listPartNo = await _historicalDataAppService.GetChangedRecordIds("InvPioPartListOff");
            var listPartGrade = await _historicalDataAppService.GetChangedRecordIds("InvPioPartGradeOff");

            ChangedRecordIdsDto result = new ChangedRecordIdsDto();
            result.PartList = listPartNo;
            result.PartGrade = listPartGrade;
            return result;
        }


        public async Task<PagedResultDto<InvPioPartGradeOffDto>> GetPartGradeInl(GetInvPioPartListGradeOffInput input)
        {
            string _sqlSearch = "Exec [INV_PIO_PART_GRADE_OFF] @p_part_id";

            IEnumerable<InvPioPartGradeOffDto> result = await _partGradeInl.QueryAsync<InvPioPartGradeOffDto>(_sqlSearch,
                  new
                  {
                      p_part_id = input.PartId

                  });
            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvPioPartGradeOffDto>(
                totalCount,
                pagedAndFiltered);
        }


    
        public async Task<List<GetCfcPartListOffDto>> GetListCfcs()
        {
            IEnumerable<GetCfcPartListOffDto> result = await _dapperRepo.QueryAsync<GetCfcPartListOffDto>(@"exec [dbo].[MST_CMM_LOT_CODE_GRADE_CFC_GETS] ");
            return result.ToList();
        }


        public async Task<List<GetGradeDto>> GetListGrades()
        {
            IEnumerable<GetGradeDto> result = await _dapperRepo.QueryAsync<GetGradeDto>(@"exec [dbo].[MST_CMM_LOT_CODE_GRADE_GRADE_GETS] ");
            return result.ToList();
        }
        public async Task<List<GetSupplierPartListInlDto>> GetListSupplierNos()
        {
            IEnumerable<GetSupplierPartListInlDto> result = await _dapperRepo.QueryAsync<GetSupplierPartListInlDto>(@"exec [dbo].[MST_INV_PIO_SUPPLIER_LIST_GETS]");
            return result.ToList();
        }
        public async Task<List<GetPartNoPartListOffDto>> GetListPartNo()
        {
            IEnumerable<GetPartNoPartListOffDto> result = await _dapperRepo.QueryAsync<GetPartNoPartListOffDto>("SELECT DISTINCT PartNo FROM InvPioPartListOff");
            return result.ToList();
        }
        public async Task<PagedResultDto<CheckExistPartListOffDto>> CheckExistPartNo(string PartNo, string Cfc)
        {
            string _sqlSearch = "Exec INV_PIO_PARTLIST_INL_EXIST_PARTNO @partno, @cfc";

            IEnumerable<CheckExistPartListOffDto> result = await _partGradeInl.QueryAsync<CheckExistPartListOffDto>(_sqlSearch, new { partno = PartNo, cfc = Cfc });
            var listResult = result.ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<CheckExistPartListOffDto>(
                totalCount,
                result.ToList());
        }


        [AbpAuthorize(AppPermissions.Pages_PIO_Master_PartListOff_Add)]
        public async Task PartListOffInsert(GetPartListGradePartListOffDto input)
        {
            var result = (await _dapperRepo.QueryAsync<GetPartListOffId>(@"
                   exec [dbo].[INV_PIO_PART_LIST_OFF_INSERT]
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
                      exec [dbo].[INV_PIO_PART_GRADE_OFF_INSERT]                                      
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

        [AbpAuthorize(AppPermissions.Pages_PIO_Master_PartListOff_Add)]
        public async Task PartListOffEdit(GetPartListGradePartListOffDto input)
        {
            string listGradeSelected = "";
            await _dapperRepo.ExecuteAsync(@"
                   exec [dbo].[INV_PIO_PART_LIST_OFF_EDIT]
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
                  exec [dbo].[INV_PIO_PART_GRADE_OFF_EDITS]  
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
                });
            }
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

        [AbpAuthorize(AppPermissions.Pages_PIO_Master_PartListOff_Add)]
        public async Task PartGradeDel(long? v_id)
        {
            await _dapperRepo.ExecuteAsync(@" exec [dbo].[INV_PIO_PART_GRADE_OFF_DELETE] @id ,@p_UserId", new
            {
                id = v_id,
                p_UserId = AbpSession.UserId
            });
        }



        [AbpAuthorize(AppPermissions.Pages_PIO_Master_PartListOff_Import)]
        public async Task<List<ImportPioPartListOffDto>> ImportDataInvPioPartListOffFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<ImportPioPartListOffDto> listImport = new List<ImportPioPartListOffDto>();
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
                    for (int i = 9; i <= 500; i++)
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

                        string v_partNo = (v_worksheet.Cells[i, 3]).Value?.ToString() ?? "";

                        if (v_partNo != "")
                        {
                            var v_remark = Convert.ToString(v_worksheet.Cells[i, 8 + countGrade + 1].Value);
                            string v_model = (v_worksheet.Cells[i, 0]).Value?.ToString() ?? "";
                            string v_shop = (v_worksheet.Cells[i, 1]).Value?.ToString() ?? "";
                            string v_idLine = (v_worksheet.Cells[i, 2]).Value?.ToString() ?? "";
                            string v_partName = (v_worksheet.Cells[i, 4]).Value?.ToString() ?? "";
                            string v_exporter = (v_worksheet.Cells[i, 5]).Value?.ToString() ?? "";
                            string v_exporterCode = (v_worksheet.Cells[i, 6]).Value?.ToString() ?? "";
                            string v_bodyColor = (v_worksheet.Cells[i, 7]).Value?.ToString() ?? "";

                            flag_HasRecord = false;
                            for (int j = 9; j < countGrade + 9; j++)
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(v_worksheet.Cells[i, j].Value)) && Convert.ToString(v_worksheet.Cells[i, j].Value) != "0")
                                {
                                    flag_HasRecord = true;
                                    var row = new ImportPioPartListOffDto();
                                    row.Guid = strGUID;

                                    if (v_model.Length > 4) row.ErrorDescription = "Độ dài Model : " + v_model + " không hợp lệ! , ";
                                    else row.Cfc = v_model;

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
                                var row = new ImportPioPartListOffDto();
                                row.Guid = strGUID;


                                if (v_shop.Length > 1) row.ErrorDescription += "Độ dài Shop : " + v_shop + " không hợp lệ! , ";
                                else row.Shop = v_shop;

                                if (v_idLine.Length > 10) row.ErrorDescription += "Độ dài IdLine : " + v_idLine + " không hợp lệ! , ";
                                else row.IdLine = v_idLine;

                                if (v_partNo.Length > 50) row.ErrorDescription += "Độ dài PartNo : " + v_partNo + " không hợp lệ! , ";

                                if (v_exporter.Length > 50) row.ErrorDescription += "Độ dài SupplierNo : " + v_exporter + " không hợp lệ! , ";
                                else row.SupplierNo = v_exporter;

                                if (v_exporterCode.Length > 50) row.ErrorDescription += "Độ dài SupplierCd : " + v_exporterCode + " không hợp lệ! , ";
                                else row.SupplierCd = v_exporterCode;

                                if (v_bodyColor.Length > 100) row.ErrorDescription += "Độ dài BodyColor : " + v_bodyColor + " không hợp lệ! , ";
                                else row.BodyColor = v_bodyColor;



                                row.ErrorDescription = "";


                                row.Remark = v_remark;

                                listImport.Add(row);
                            }

                        }

                    }
                    // import temp into db (bulkCopy)
                    if (listImport.Count > 0)
                    {
                        IEnumerable<ImportPioPartListOffDto> dataE = listImport.AsEnumerable();
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
                                    bulkCopy.DestinationTableName = "InvPioPart_T";
                                    bulkCopy.ColumnMappings.Add("Guid", "Guid");
                                    bulkCopy.ColumnMappings.Add("Cfc", "Cfc");
                                    bulkCopy.ColumnMappings.Add("Shop", "Shop");
                                    bulkCopy.ColumnMappings.Add("IdLine", "IdLine");
                                    bulkCopy.ColumnMappings.Add("PartNo", "PartNo");
                                    bulkCopy.ColumnMappings.Add("PartName", "PartName");
                                    bulkCopy.ColumnMappings.Add("SupplierNo", "SupplierNo");
                                    bulkCopy.ColumnMappings.Add("SupplierCd", "SupplierCd");
                                    bulkCopy.ColumnMappings.Add("BodyColor", "BodyColor");

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



        public async Task MergeDataInvPioPartListOff(string v_Guid)
        {

            string _merge = "Exec [INV_PIO_PART_LIST_OFF_MERGE] @Guid";
            await _dapperRepo.QueryAsync<ImportCkdPartListDto>(_merge, new { Guid = v_Guid });
        }
        public async Task<PagedResultDto<ImportCkdPartListDto>> GetMessageErrorImport(string v_Guid)
        {
            string _sql = "Exec [INV_PIO_PART_LIST_OFF_ERROR_IMPORT] @Guid";

            IEnumerable<ImportCkdPartListDto> result = await _dapperRepo.QueryAsync<ImportCkdPartListDto>(_sql, new
            {
                Guid = v_Guid
           
            });

            var listResult = result.ToList();
            var totalCount = listResult.Count();

            return new PagedResultDto<ImportCkdPartListDto>(
                totalCount,
               listResult
               );
        }


        public async Task<FileDto> GetListErrPartListOffToExcel(string v_Guid)
        {
            FileDto a = new FileDto();
            string _sql = "Exec INV_PIO_PART_LIST_OFF_ERROR_IMPORT @Guid";

            IEnumerable<ImportPioPartListOffDto> result = await _dapperRepo.QueryAsync<ImportPioPartListOffDto>(_sql, new
            {
                Guid = v_Guid
            });

            var exportToExcel = result.ToList();
        
            return _calendarListExcelExporter.ExportToFileErr(exportToExcel);

        }


        [AbpAuthorize(AppPermissions.Pages_PIO_Master_PartListOff_Add)]
        public async Task PartGradeOffEdit(InvCkdPartGradeDto input)
        {
            await _dapperRepo.ExecuteAsync(@"
                  exec [dbo].[INV_PIO_PART_GRADE_OFF_EDIT]
                                @id,
                                @usageQty,
                                @shop,
                                @grade,
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
                id = input.Id,
                usageQty = input.UsageQty,
                shop = input.Shop,
                grade = input.Grade,
                bodyColor = input.BodyColor,
                startLot = input.StartLot,
                endLot = input.EndLot,
                startRun = input.StartRun,
                endRun = input.EndRun,
                efFromPart = input.EfFromPart,
                efToPart = input.EfToPart,
                orderPattern = input.OrderPattern,
                remark = input.Remark,
                p_UserId = AbpSession.UserId
            });
        }


        public async Task<List<string>> GetPartListOffHistory(GetInvCkdPartListHistoryInput input)
        {
            return await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
        }


        public async Task<List<GetGradeDto>> GetListGradesByCfc(string cfc)
        {
            IEnumerable<GetGradeDto> result = await _dapperRepo.QueryAsync<GetGradeDto>("Exec MST_CMM_LOT_CODE_GRADE_GET_BY_CFC @p_Cfc", new { p_Cfc = cfc });
            return result.ToList();
        }

        // Excel
        public async Task<FileDto> GetPioPartExportToFile(InvCkdPartListExportInput input)
        {
            var file = new FileDto("PIOPartListOffline.xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
            string fileName = "temp_PIOPartListInl";
            string template = "wwwroot/Template";
            string path = "";
            path = Path.Combine(Directory.GetCurrentDirectory(), template, fileName);
            var xlWorkBook = ExcelFile.Load(path + ".xlsx");
            var workSheet = xlWorkBook.Worksheets[0];

            List<string> listExport = new List<string>();
            listExport.Add("Id");
            listExport.Add("Cfc");
            listExport.Add("Model");
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
            listExport.Add("Blank");
            listExport.Add("Remark");
            listExport.Add("Value");

            string[] properties = listExport.ToArray();
            string _sql = "Exec [INV_PIO_PART_LIST_OFF_EXCEL] @part_no,@cfc, @p_model, @p_grade, @p_shop,@supplierNo,@p_order_pattern";
            var listDataHeader = (await _dapperRepo.QueryAsync<ExportPioPartListDto>(_sql, new
            {
                part_no = input.PartNo,
                cfc = input.Cfc,
                p_model = input.Model,
                p_grade = input.Grade,
                p_shop = input.Shop,
                supplierNo = input.SupplierNo,
                p_order_pattern = input.OrderPattern,
            })).ToList();

            _sql = "Exec [INV_PIO_PART_GRADE_OFF_BY_GRADE] @p_part_no";
            var listGrade = (await _partGradeInl.QueryAsync<ExportCkdPartListGradeDto>(_sql, new { p_part_no = input.PartNo })).ToList();


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

                workSheet.Cells[0, 18 + i].Value = listGrade[i].Grade;
                workSheet.Cells[0, 18 + i].Style = styleheader;
            }
            workSheet.Cells[0, 18 + listGrade.Count].Value = "Remark";
            workSheet.Cells[0, 18 + listGrade.Count].Style = styleheader;




            for (int i = 0; i < table.Rows.Count; i++)
            {
                DataRow dr = table.Rows[i];

                /*     foreach (string x in dr["Value"].ToString().Split(","))
                     {
                         if (x != null && x != "")
                             dr[x.Split("-")[0]] = x.Split("-")[1] == "" ? 0 : int.Parse(x.Split("-")[1]);
                     }*/

                //set style excel with Gembox 
                for (int column_index = 0; column_index <= (18 + listGrade.Count); column_index++)
                {
                    if (column_index == 17) { continue; }
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

        public async Task<FileDto> GetPioPartGroupBodyColorExportToFile(InvCkdPartListExportInput input)
        {
            var file = new FileDto("PIOPartListGroupBodyColor.xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
            string fileName = "temp_PIOPartListInl";
            string template = "wwwroot/Template";
            string path = "";
            path = Path.Combine(Directory.GetCurrentDirectory(), template, fileName);
            var xlWorkBook = ExcelFile.Load(path + ".xlsx");
            var workSheet = xlWorkBook.Worksheets[0];

            List<string> listExport = new List<string>();
            listExport.Add("Id");
            listExport.Add("Cfc");
            listExport.Add("Model");
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
            listExport.Add("Blank");
            listExport.Add("Remark");
            listExport.Add("Value");

            string[] properties = listExport.ToArray();
            string _sql = "Exec [INV_PIO_PART_LIST_OFF_GROUP_BODY_COLOR_SEARCH] @part_no,@cfc,@p_model, @p_grade, @p_shop,@supplierNo,@p_order_pattern";
            var listDataHeader = (await _dapperRepo.QueryAsync<ExportPioPartListDto>(_sql, new
            {
                part_no = input.PartNo,
                cfc = input.Cfc,
                p_model = input.Model,
                p_grade = input.Grade,
                p_shop = input.Shop,
                supplierNo = input.SupplierNo,
                p_order_pattern = input.OrderPattern,
            })).ToList();

            _sql = "Exec INV_PIO_PART_GRADE_OFF_BY_GRADE @p_part_no";
            var listGrade = (await _partGradeInl.QueryAsync<ExportCkdPartListGradeDto>(_sql, new { p_part_no = input.PartNo })).ToList();


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
                workSheet.Cells[0, 18 + i].Value = listGrade[i].Grade;
                workSheet.Cells[0, 18 + i].Style = styleheader;
            }
            workSheet.Cells[0, 18 + listGrade.Count].Value = "Remark";
            workSheet.Cells[0, 18 + listGrade.Count].Style = styleheader;



            for (int i = 0; i < table.Rows.Count; i++)
            {
                DataRow dr = table.Rows[i];

                /*   foreach (string x in dr["Value"].ToString().Split(","))
                   {
                       if (x != null && x != "")
                           dr[x.Split("-")[0]] = x.Split("-")[1] == "" ? 0 : int.Parse(x.Split("-")[1]);
                   }*/

                //set style excel with Gembox 
                for (int column_index = 0; column_index <= (18 + listGrade.Count); column_index++)
                {
                    if (column_index == 17) { continue; }
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



        [AbpAuthorize(AppPermissions.Pages_PIO_Master_PartListOff_Validate)]
        public async Task<PagedResultDto<ValidatePartListDto>> GetValidateInvPioPartList(PagedAndSortedResultRequestDto input)
        {
            string _sqlSearch = "Exec [INV_PIO_PART_LIST_OFF_VALIDATE]";

            IEnumerable<ValidatePartListDto> result = await _partGradeInl.QueryAsync<ValidatePartListDto>(_sqlSearch, new { });
            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<ValidatePartListDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<FileDto> GetValidateInvPioPartListExcel()
        {

            string _sql = "Exec [INV_PIO_PART_LIST_OFF_VALIDATE]";

            IEnumerable<ValidatePartListDto> result = await _partGradeInl.QueryAsync<ValidatePartListDto>(_sql, new
            { });

            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportValidateToFile(exportToExcel);
        }


        [AbpAuthorize(AppPermissions.Pages_PIO_Master_PartListOff_Validate)]
        public async Task EciPartGrade(long? v_id, string v_StartLot, string v_StartRun, string v_Grade, int? v_usageQty)
        {
            await _dapperRepo.ExecuteAsync(@" exec [dbo].[INV_PIO_PART_GRADE_OFF_ECI] @id, @p_StartLot, @p_StartRun, @p_Grade , @p_UserId, @p_usageQty", new
            {
                id = v_id,
                p_StartLot = v_StartLot,
                p_StartRun = v_StartRun,
                p_Grade = v_Grade,
                p_UserId = AbpSession.UserId,
                p_usageQty = v_usageQty
            });
        }

    }
}
