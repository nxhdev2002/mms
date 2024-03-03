using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.UI;
using FastMember;
using GemBox.Spreadsheet;
using prod.Authorization;
using prod.Dto;
using prod.Master.Cmm.Dto;
using prod.Master.Cmm.Exporting;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using prod.Common;
using prod.HistoricalData;
using prod.Inventory.GPS.Dto;

namespace prod.Master.Cmm
{
    [AbpAuthorize(AppPermissions.Pages_Master_Cmm_MMCheckingRule_View)]
    public class MstCmmMMCheckingRuleAppService : prodAppServiceBase, IMstCmmMMCheckingRuleAppService
    {
        private readonly IDapperRepository<MstCmmMMCheckingRule, long> _dapperRepo;
        private readonly IMstCmmMMCheckingRuleExcelExporter _checkingRuleExcelExporter;
        private readonly IHistoricalDataAppService _historicalDataAppService;

        public MstCmmMMCheckingRuleAppService(IDapperRepository<MstCmmMMCheckingRule, long> dapperRepo,
                                        IMstCmmMMCheckingRuleExcelExporter checkingRuleExcelExporter,
                                        IHistoricalDataAppService historicalDataAppService
            )
        {
            _dapperRepo = dapperRepo;
            _checkingRuleExcelExporter = checkingRuleExcelExporter;
            _historicalDataAppService = historicalDataAppService;
        }

        public async Task<List<string>> GetMstCmmMMCheckingRuleHistory(GetMstCmmMMCheckingRuleHistoryInput input)
        {
            return await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
        }

        public async Task<FileDto> GetHistoricalDataToExcel(GetMstCmmMMCheckingRuleHistoryExcelInput input)
        {
            var data = await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
            return _checkingRuleExcelExporter.ExportToHistoricalFile(data);
        }

        public async Task<List<long?>> GetChangedRecords()
        {
            return await _historicalDataAppService.GetChangedRecordIds("MstCmmMMCheckingRule");
        }

        public async Task<PagedResultDto<MstCmmMMCheckingRuleDto>> GetAll(GetMstCmmMMCheckingRuleInput input)
        {
            string _sql = "Exec MST_CMM_MM_CHEKINGRULE_SEARCH @p_RuleCode, @p_RuleItem, @p_FieldName, @p_Resultfield, @p_IsActive";

            IEnumerable<MstCmmMMCheckingRuleDto> result = await _dapperRepo.QueryAsync<MstCmmMMCheckingRuleDto>(_sql, new
            {
                p_RuleCode = input.RuleCode,
                p_RuleItem = input.RuleItem,
                p_FieldName = input.FieldName,
                p_Resultfield = input.Resultfield,
                p_IsActive = input.IsActive
            });

            var listResult = result.ToList().Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            var totalCount = result.Count();

            return new PagedResultDto<MstCmmMMCheckingRuleDto>(
                totalCount,
                listResult
               );
        }


        public async Task<FileDto> GetCmmMMCheckingRuleToExcel(MstCmmMMCheckingRuleExportInput input)
        {
            string _sql = "Exec MST_CMM_MM_CHEKINGRULE_SEARCH @p_RuleCode, @p_RuleItem, @p_FieldName, @p_Resultfield, @p_IsActive";

            IEnumerable<MstCmmMMCheckingRuleDto> result = await _dapperRepo.QueryAsync<MstCmmMMCheckingRuleDto>(_sql, new
            {
                p_RuleCode = input.RuleCode,
                p_RuleItem = input.RuleItem,
                p_FieldName = input.FieldName,
                p_Resultfield = input.Resultfield,
                p_IsActive = input.IsActive
            });

            var listResult = result.ToList();
            return _checkingRuleExcelExporter.ExportToFile(listResult);
        }

        [AbpAuthorize(AppPermissions.Pages_Master_Cmm_MMCheckingRule_Import)]
        public async Task<List<MstCmmMMCheckingRuleImportDto>> ImportMstCmmMMCheckingRuleFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<MstCmmMMCheckingRuleImportDto> listImport = new List<MstCmmMMCheckingRuleImportDto>();
                using (var stream = new MemoryStream(fileBytes))
                {
                    SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
                    var xlWorkBook = ExcelFile.Load(stream);
                    string nameSheet = "";
                    string strGUID = Guid.NewGuid().ToString("N");

                    string _sql = @"EXEC CMM_IMPORT_USER_GUID_INSERT @Guid, @p_UserId";
                    _dapperRepo.ExecuteAsync(_sql, new
                    {
                        Guid = strGUID,
                        p_UserId = AbpSession.UserId
                    });

                    //sheet
                    foreach (ExcelWorksheet worksheet in xlWorkBook.Worksheets)
                    {
                        nameSheet = worksheet.Name;

                        //Read
                        if (nameSheet == "Header")  // check sheet frame
                        {
                            for (int i = 1; i < worksheet.Rows.Count; i++)
                            {
                                var row = new MstCmmMMCheckingRuleImportDto();
                                string v_rulecode = (worksheet.Cells[i, 1]).Value?.ToString() ?? "";
                                if (v_rulecode != "")
                                {
                                    row.Guid = strGUID;
                                    row.RuleCode = v_rulecode;
                                    row.Field1Name = (worksheet.Cells[i, 2]).Value?.ToString() ?? "";
                                    row.Field2Name = (worksheet.Cells[i, 3]).Value?.ToString() ?? "";
                                    row.Field3Name = (worksheet.Cells[i, 4]).Value?.ToString() ?? "";
                                    row.Field4Name = (worksheet.Cells[i, 5]).Value?.ToString() ?? "";
                                    row.Field5Name = (worksheet.Cells[i, 6]).Value?.ToString() ?? "";
                                    row.Resultfield = (worksheet.Cells[i, 7]).Value?.ToString() ?? "";
                                    row.IsDetail = "N";
                                    listImport.Add(row);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        else if (nameSheet == "Details")
                        {
                            for (int i = 1; i < worksheet.Rows.Count; i++)
                            {
                                var row = new MstCmmMMCheckingRuleImportDto();
                                string v_rulecode = (worksheet.Cells[i, 1]).Value?.ToString() ?? "";

                                if (v_rulecode != "")
                                {
                                    row.Guid = strGUID;
                                    row.RuleCode = v_rulecode;
                                    row.RuleDescription = (worksheet.Cells[i, 2]).Value?.ToString() ?? "";
                                    row.RuleItem = (worksheet.Cells[i, 3]).Value?.ToString() ?? "";
                                    row.Field1Value = (worksheet.Cells[i, 4]).Value?.ToString() ?? "";
                                    row.Field2Value = (worksheet.Cells[i, 5]).Value?.ToString() ?? "";
                                    row.Field3Value = (worksheet.Cells[i, 6]).Value?.ToString() ?? "";
                                    row.Field4Value = (worksheet.Cells[i, 7]).Value?.ToString() ?? "";
                                    row.Field5Value = (worksheet.Cells[i, 8]).Value?.ToString() ?? "";
                                    row.Option = (worksheet.Cells[i, 9]).Value?.ToString() ?? "";
                                    row.Expectedresult = (worksheet.Cells[i, 10]).Value?.ToString() ?? "";
                                    row.Checkoption = (worksheet.Cells[i, 11]).Value?.ToString() ?? "";
                                    row.Errormessage = (worksheet.Cells[i, 12]).Value?.ToString() ?? "";
                                    row.IsDetail = "Y";
                                    listImport.Add(row);
                                }
                                else
                                {
                                    break;
                                }

                            }
                        }
                    }
                    // import temp into db (bulkCopy)
                    if (listImport.Count > 0)
                    {
                        IEnumerable<MstCmmMMCheckingRuleImportDto> dataE = listImport.AsEnumerable();
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
                                    bulkCopy.DestinationTableName = "MstCmmMMCheckingRule_T";
                                    bulkCopy.ColumnMappings.Add("Guid", "Guid");
                                    bulkCopy.ColumnMappings.Add("RuleCode", "RuleCode");
                                    bulkCopy.ColumnMappings.Add("RuleDescription", "RuleDescription");
                                    bulkCopy.ColumnMappings.Add("RuleItem", "RuleItem");
                                    bulkCopy.ColumnMappings.Add("Field1Name", "Field1Name");
                                    bulkCopy.ColumnMappings.Add("Field2Name", "Field2Name");
                                    bulkCopy.ColumnMappings.Add("Field3Name", "Field3Name");
                                    bulkCopy.ColumnMappings.Add("Field4Name", "Field4Name");
                                    bulkCopy.ColumnMappings.Add("Field5Name", "Field5Name");
                                    bulkCopy.ColumnMappings.Add("Field1Value", "Field1Value");
                                    bulkCopy.ColumnMappings.Add("Field2Value", "Field2Value");
                                    bulkCopy.ColumnMappings.Add("Field3Value", "Field3Value");
                                    bulkCopy.ColumnMappings.Add("Field4Value", "Field4Value");
                                    bulkCopy.ColumnMappings.Add("Field5Value", "Field5Value");
                                    bulkCopy.ColumnMappings.Add("Option", "Option");
                                    bulkCopy.ColumnMappings.Add("Resultfield", "Resultfield");
                                    bulkCopy.ColumnMappings.Add("Expectedresult", "Expectedresult");
                                    bulkCopy.ColumnMappings.Add("Checkoption", "Checkoption");
                                    bulkCopy.ColumnMappings.Add("Errormessage", "Errormessage");
                                    bulkCopy.ColumnMappings.Add("IsDetail", "IsDetail");
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
        public async Task MergeDataMstCmmMMCheckingRuleImport(string v_Guid)
        {

            string _merge = "Exec MST_CMM_MM_CHECKING_RULE_MERGE @Guid";
            await _dapperRepo.QueryAsync<MstCmmMMCheckingRuleImportDto>(_merge, new { Guid = v_Guid });
        }
        public async Task<PagedResultDto<MstCmmMMCheckingRuleImportDto>> GetMessageErrorImport(string v_Guid)
        {
            string _sql = "Exec MST_CMM_MM_CHECKING_RULE_GET_LIST_ERROR_IMPORT @Guid";

            IEnumerable<MstCmmMMCheckingRuleImportDto> result = await _dapperRepo.QueryAsync<MstCmmMMCheckingRuleImportDto>(_sql, new
            {
                Guid = v_Guid
            });

            var listResult = result.ToList();
            var totalCount = listResult.Count();

            return new PagedResultDto<MstCmmMMCheckingRuleImportDto>(
                totalCount,
               listResult
               );
        }
        public async Task<FileDto> GetListErrMMCheckingRuleToExcel(string v_Guid)
        {
            string _sql = "Exec MST_CMM_MM_CHECKING_RULE_GET_LIST_ERROR_IMPORT  @Guid";
            IEnumerable<MstCmmMMCheckingRuleImportDto> result = await _dapperRepo.QueryAsync<MstCmmMMCheckingRuleImportDto>(_sql, new
            {
                Guid = v_Guid
            });

            var exportToExcel = result.ToList();
            return _checkingRuleExcelExporter.ExportToFileErr(exportToExcel);

        }

    }
}
