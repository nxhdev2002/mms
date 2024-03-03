using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.UI;
using FastMember;
using GemBox.Spreadsheet;
using NPOI.SS.UserModel;
using prod.Authorization;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using prod.Inventory.CKD.Exporting;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using prod.Common;
using prod.HistoricalData;

namespace prod.Inventory.CKD
{
    [AbpAuthorize(AppPermissions.Pages_ProdPlan_ProductionMapping_View)]
    public class InvCkdProductionMappingAppService : prodAppServiceBase, IInvCkdProductionMappingAppService
    {
        private readonly IDapperRepository<InvCkdProductionMapping, long> _dapperRepo;
        private readonly IRepository<InvCkdProductionMapping, long> _repo;
        private readonly IInvCkdProductionMappingExcelExporter _prodMaptExcelExporter;
        private readonly IHistoricalDataAppService _historicalDataAppService;

        public InvCkdProductionMappingAppService(IRepository<InvCkdProductionMapping, long> repo,
                                         IDapperRepository<InvCkdProductionMapping, long> dapperRepo,
                                        IInvCkdProductionMappingExcelExporter prodMaptExcelExporter,
                                        IHistoricalDataAppService historicalDataAppService

            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _prodMaptExcelExporter = prodMaptExcelExporter;
            _historicalDataAppService = historicalDataAppService;
        }
        public async Task<List<string>> GetProductionMappingHistory(GetInvProductionMappingHistoryInput input)
        {
            return await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
        }

        public async Task<FileDto> GetHistoricalDataToExcel(GetInvCkdProductionMappingHistoryExcelInput input)
        {
            var data = await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
            return _prodMaptExcelExporter.ExportToHistoricalFile(data);
        }

        public async Task<List<long?>> GetChangedRecords()
        {
            var data = await _historicalDataAppService.GetChangedRecordIds("InvCkdProductionMapping");
            return data;
        }
        public async Task<PagedResultDto<InvCkdProductionMappingDto>> GetAll(GetInvCkdProductionMappingInput input)
        {
            string _sql = "Exec INV_CKD_PRODUCTION_MAPPING_GETDATA @PeriodId, @Shop, @BodyNo, @LotNo, @p_UseLotNo, @p_SupplierNo, @Date_in_from, @Date_in_to";

            IEnumerable<InvCkdProductionMappingDto> result = await _dapperRepo.QueryAsync<InvCkdProductionMappingDto>(_sql, new
            {
                PeriodId = input.PeriodId,
                Shop = input.Shop,
                BodyNo = input.BodyNo,
                LotNo = input.LotNo,
                p_UseLotNo = input.UseLotNo,
                p_SupplierNo = input.SupplierNo,
                Date_in_from = input.DateInFrom,
                Date_in_to = input.DateInTo
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvCkdProductionMappingDto>(
                totalCount,
                pagedAndFiltered);
        }


        public async Task<FileDto> GetProductionMappingToExcel(GetInvCkdProductionMappingInput input)
        {
            string _sql = "Exec INV_CKD_PRODUCTION_MAPPING_GETDATA @PeriodId, @Shop, @BodyNo, @LotNo, @p_UseLotNo, @p_SupplierNo, @Date_in_from, @Date_in_to";

            IEnumerable<InvCkdProductionMappingDto> result = await _dapperRepo.QueryAsync<InvCkdProductionMappingDto>(_sql, new
            {
                PeriodId = input.PeriodId,
                Shop = input.Shop,
                BodyNo = input.BodyNo,
                LotNo = input.LotNo,
                p_UseLotNo = input.UseLotNo,
                p_SupplierNo = input.SupplierNo,
                Date_in_from = input.DateInFrom,
                Date_in_to = input.DateInTo
            });

            var exportToExcel = result.ToList();
            return _prodMaptExcelExporter.ExportToFile(exportToExcel);
        }

        [AbpAuthorize(AppPermissions.Pages_ProdPlan_ProductionMapping_Import)]
        public async Task<List<InvCkdProductionMappingDto>> ImportInvCkdProductionMappingFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<InvCkdProductionMappingDto> listImport = new List<InvCkdProductionMappingDto>();
                using (var stream = new MemoryStream(fileBytes))
                {
                    SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
                    var xlWorkBook = ExcelFile.Load(stream);
                    //  var v_worksheet = xlWorkBook.Worksheets[0];
                    DataFormatter formatter = new DataFormatter();
                    DateTime dateTime = DateTime.Now;
                    string nameSheet = "";
                    string strGUID = Guid.NewGuid().ToString("N");
                    int CountSheetX = xlWorkBook.Worksheets.Count;
                    //sheet
                    for (int s = 0; s < CountSheetX; s++)
                    {
                        var v_worksheet = xlWorkBook.Worksheets[s];
                        nameSheet = xlWorkBook.Worksheets[s].Name;
                        string v_Shop = nameSheet.Substring(0, 2);
                        // Read W1 Plan
                        if (v_Shop == "W1" || v_Shop == "W2" || v_Shop == "W3" || v_Shop == "A1" || v_Shop == "A2")  // check sheet 
                        {
                            for (int i = 4; i < v_worksheet.Rows.Count; i++)
                            {
                                string v_Model = (v_worksheet.Cells[i, 2]).Value?.ToString() ?? "";
                                if (v_Model != "")
                                {
                                    var row = new InvCkdProductionMappingDto();
                                    string v_datein = (v_worksheet.Cells[i, 7]).Value?.ToString() ?? "";
                                    string v_timein = (v_worksheet.Cells[i, 8]).Value?.ToString() ?? "";
                                    row.ErrorDescription = "";
                                    row.Guid = strGUID;
                                    row.Model = v_Model;
                                    row.Shop = v_Shop;
                                    row.LotNo = (v_worksheet.Cells[i, 3]).Value?.ToString() ?? "";
                                    row.NoInLot = (v_worksheet.Cells[i, 4]).Value?.ToString() ?? "";
                                    row.Grade = (v_worksheet.Cells[i, 5]).Value?.ToString() ?? "";
                                    row.BodyNo = (v_worksheet.Cells[i, 6]).Value?.ToString() ?? "";
                                    row.CreatorUserId = AbpSession.UserId;
                                    try
                                    {
                                        row.PlanSequence = int.Parse((v_worksheet.Cells[i, 1]).Value?.ToString());
                                    }
                                    catch
                                    {
                                        row.ErrorDescription = "PlanSequence không hợp lệ! ";
                                    }
                                    

                                    //Date In
                                    try
                                    {
                                        DateTime.Parse(v_datein);
                                        row.DateIn = DateTime.Parse(v_datein);
                                    }
                                    catch
                                    {
                                        row.ErrorDescription = "Date In không hợp lệ! ";
                                    }
                                    //Time In
                                    try
                                    {
                                        /* string v_timeincheck = (v_timein.Substring(v_timein.Length - 8, 8)) ?? "";
                                         TimeSpan.Parse(v_timeincheck);*/

                                        v_timein = DateTime.Parse(v_timein).ToString("HH:mm:ss");
                                        row.TimeIn = v_timein;
                                    }
                                    catch
                                    {
                                        row.ErrorDescription += "Time In không hợp lệ! ";
                                    }
                                    row.SupplierNo = (v_worksheet.Cells[3, 10]).Value?.ToString() ?? "";
                                    row.UseLotNo = (v_worksheet.Cells[i, 10]).Value?.ToString() ?? "";
                              
                                    listImport.Add(row);
                                }
                                else
                                {
                                    break;
                                }
                            }
                            var spNo = v_worksheet.Cells[3, 11].Value?.ToString() ?? "";
                            if (spNo == "TMT" || spNo == "TMI")
                            {
                                for (int i = 4; i < v_worksheet.Rows.Count; i++)
                                {
                                    string v_Model = (v_worksheet.Cells[i, 2]).Value?.ToString() ?? "";
                                    if (v_Model != "")
                                    {
                                        var row = new InvCkdProductionMappingDto();
                                        string v_datein = (v_worksheet.Cells[i, 7]).Value?.ToString() ?? "";
                                        string v_timein = (v_worksheet.Cells[i, 8]).Value?.ToString() ?? "";

                                        row.Guid = strGUID;
                                        row.Model = v_Model;
                                        row.Shop = v_Shop;
                                        row.LotNo = (v_worksheet.Cells[i, 3]).Value?.ToString() ?? "";
                                        row.NoInLot = (v_worksheet.Cells[i, 4]).Value?.ToString() ?? "";
                                        row.Grade = (v_worksheet.Cells[i, 5]).Value?.ToString() ?? "";
                                        row.BodyNo = (v_worksheet.Cells[i, 6]).Value?.ToString() ?? "";
                                        row.CreatorUserId = AbpSession.UserId;
                                        try
                                        {
                                            row.PlanSequence = int.Parse((v_worksheet.Cells[i, 1]).Value?.ToString());
                                        }
                                        catch
                                        {
                                            row.ErrorDescription = "PlanSequence không hợp lệ! ";
                                        }
                                        row.ErrorDescription = "";

                                        //Date In
                                        try
                                        {
                                            DateTime.Parse(v_datein);
                                            row.DateIn = DateTime.Parse(v_datein);
                                        }
                                        catch
                                        {
                                            row.ErrorDescription = "Date In không hợp lệ! ";
                                        }
                                        //Time In
                                        try
                                        {
                                            /*string v_timeincheck = (v_timein.Substring(v_timein.Length - 8, 8)) ?? "";
                                            TimeSpan.Parse(v_timeincheck);*/

                                            v_timein = DateTime.Parse(v_timein).ToString("HH:mm:ss");                                        
                                            row.TimeIn = v_timein;
                                        }
                                        catch
                                        {
                                            row.ErrorDescription += "Time In không hợp lệ! ";
                                        }
                                        row.SupplierNo = (v_worksheet.Cells[3, 11]).Value?.ToString() ?? "";
                                        row.UseLotNo = (v_worksheet.Cells[i, 11]).Value?.ToString() ?? "";

                                        listImport.Add(row);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                // import temp into db(bulkCopy)
                if (listImport.Count > 0)
                {
                    IEnumerable<InvCkdProductionMappingDto> dataE = listImport.AsEnumerable();
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
                                bulkCopy.DestinationTableName = "InvCkdProductionMapping_T";
                                bulkCopy.ColumnMappings.Add("PlanSequence", "PlanSequence");
                                bulkCopy.ColumnMappings.Add("Guid", "Guid");
                                bulkCopy.ColumnMappings.Add("Model", "Model");
                                bulkCopy.ColumnMappings.Add("Shop", "Shop");
                                bulkCopy.ColumnMappings.Add("LotNo", "LotNo");
                                bulkCopy.ColumnMappings.Add("NoInLot", "NoInLot");
                                bulkCopy.ColumnMappings.Add("Grade", "Grade");
                                bulkCopy.ColumnMappings.Add("BodyNo", "BodyNo");
                                bulkCopy.ColumnMappings.Add("DateIn", "DateIn");
                                bulkCopy.ColumnMappings.Add("TimeIn", "TimeIn");
                                bulkCopy.ColumnMappings.Add("SupplierNo", "SupplierNo");
                                bulkCopy.ColumnMappings.Add("UseLotNo", "UseLotNo");
                                bulkCopy.ColumnMappings.Add("ErrorDescription", "ErrorDescription");
                                bulkCopy.ColumnMappings.Add("CreatorUserId", "CreatorUserId");
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

                throw new UserFriendlyException(ex.ToString());
                // return ex;
            }
        }

        public async Task MergeDataInvCkdProductionMapping(string v_Guid)
        {

            string _merge = "Exec INV_CKD_PRODUCTION_MAPPING_MERGE @Guid";
            await _dapperRepo.QueryAsync<InvCkdProductionMappingDto>(_merge, new { Guid = v_Guid });
        }

        // hiển thị lỗi import và export lỗi
        public async Task<PagedResultDto<InvCkdProductionMappingDto>> GetMessageErrorImport(string v_Guid)
        {
            string _sql = "Exec INV_CKD_PRODUCTION_MAPPING_GET_LIST_ERROR_IMPORT @Guid";

            IEnumerable<InvCkdProductionMappingDto> result = await _dapperRepo.QueryAsync<InvCkdProductionMappingDto>(_sql, new
            {
                Guid = v_Guid
            });

            var listResult = result.ToList();
            var totalCount = listResult.Count();

            return new PagedResultDto<InvCkdProductionMappingDto>(
                totalCount,
               listResult
               );
        }

        public async Task<FileDto> GetListErrToExcel(string v_Guid)
        {
            string _sql = "Exec INV_CKD_PRODUCTION_MAPPING_GET_LIST_ERROR_IMPORT @Guid";

            IEnumerable<InvCkdProductionMappingDto> result = await _dapperRepo.QueryAsync<InvCkdProductionMappingDto>(_sql, new
            {
                Guid = v_Guid
            });

            var exportToExcel = result.ToList();
            return _prodMaptExcelExporter.ExportToFileErr(exportToExcel);
        }

    }
}
