using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.UI;
using FastMember;
using GemBox.Spreadsheet;
using prod.Authorization;
using prod.Dto;
using prod.Inventory.CKD.SmqdOrderLeadTime.Dto;
using prod.Inventory.CKD.SmqdOrderLeadTime.Exporting;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using prod.Common;

namespace prod.Inventory.CKD.SmqdOrderLeadTime
{
    [AbpAuthorize(AppPermissions.Pages_Ckd_SMQD_OrderLeadTime_View)]
    public class InvCkdSmqdOrderLeadTimeAppService : prodAppServiceBase, IInvCkdSmqdOrderLeadTimeAppService
    {
        private readonly IDapperRepository<InvCkdSmqdOrderLeadTime, long> _dapperRepo;
        private readonly IRepository<InvCkdSmqdOrderLeadTime, long> _repo;
        private readonly IInvCkdSmqdOrderLeadTimeExcelExporter _calendarListExcelExporter;

        public InvCkdSmqdOrderLeadTimeAppService(IRepository<InvCkdSmqdOrderLeadTime, long> repo,
                                         IDapperRepository<InvCkdSmqdOrderLeadTime, long> dapperRepo,
                                        IInvCkdSmqdOrderLeadTimeExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        public async Task<PagedResultDto<InvCkdSmqdOrderLeadTimeDto>> GetAll(GetInvCkdSmqdOrderLeadTimeInput input)
        {
            string _sql = "Exec INV_CKD_SMQD_ORDER_LEADTIME_SEARCH @p_exp_code, @p_supplier_no";

            IEnumerable<InvCkdSmqdOrderLeadTimeDto> result = await _dapperRepo.QueryAsync<InvCkdSmqdOrderLeadTimeDto>(_sql, new
            {
                p_exp_code = input.ExpCode,
                p_supplier_no = input.SupplierNo,
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvCkdSmqdOrderLeadTimeDto>(
                totalCount,
                pagedAndFiltered);
        }


        public async Task<FileDto> GetSmqdOrderLeadTimeToExcel(GetInvCkdSmqdOrderLeadTimeExportInput input)
        {
            string _sql = "Exec INV_CKD_SMQD_ORDER_LEADTIME_SEARCH @p_exp_code, @p_supplier_no";

            IEnumerable<InvCkdSmqdOrderLeadTimeDto> result = await _dapperRepo.QueryAsync<InvCkdSmqdOrderLeadTimeDto>(_sql, new
            {
                p_exp_code = input.ExpCode,
                p_supplier_no = input.SupplierNo,
            });

            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }


        [AbpAuthorize(AppPermissions.Pages_Ckd_SMQD_OrderLeadTime_Import)]
        public async Task<List<InvCkdSmqdOrderLeadImportDto>> ImportSmqdOrderLeadTimeFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<InvCkdSmqdOrderLeadImportDto> listImport = new List<InvCkdSmqdOrderLeadImportDto>();
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

                    for (int i = 1; i < v_worksheet.Rows.Count; i++)
                    {
                        string v_expname = (v_worksheet.Cells[i, 0]).Value?.ToString() ?? "";

                        if (v_expname != "")
                        {
                            string v_model = (v_worksheet.Cells[i, 1]).Value?.ToString() ?? "";
                            string v_expcode = (v_worksheet.Cells[i, 2]).Value?.ToString() ?? "";
                            string v_sea = (v_worksheet.Cells[i, 3]).Value?.ToString() ?? "";
                            string v_air = (v_worksheet.Cells[i, 4]).Value?.ToString() ?? "";

                            var row = new InvCkdSmqdOrderLeadImportDto();
                            row.Guid = strGUID;
                            //Check Cfc
                            if (v_model.Length > 4)
                            {
                                row.ErrorDescription += "Cfc" + v_model + " dài quá 4 kí tự! ";
                            }
                            else
                            {
                                row.Cfc = v_model;
                            }

                            row.SupplierNo = v_expname;
                            row.ExpCode = v_expcode;

                            //Check Sea
                            try
                            {
                                if (string.IsNullOrEmpty(v_sea))
                                {
                                    row.Sea = null;
                                }
                                else
                                {
                                    row.Sea = Convert.ToInt32(v_sea);
                                    if (row.Sea < 0)
                                    {
                                        row.ErrorDescription += "Sea phải là số dương! ";
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                row.ErrorDescription += "Sea " + v_sea + " không phải là số! ";
                            }
                            //Check Air
                            try
                            {
                                if (string.IsNullOrEmpty(v_air))
                                {
                                    row.Air = null;
                                }
                                else
                                {
                                    row.Air = Convert.ToInt32(v_air);
                                    if (row.Air < 0)
                                    {
                                        row.ErrorDescription += "Air phải là số dương! ";
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                row.ErrorDescription += "Air " + v_air + " không phải là số! ";
                            }

                            listImport.Add(row);
                        }
                    }
                    // import temp into db (bulkCopy)
                    if (listImport.Count > 0)
                    {
                        IEnumerable<InvCkdSmqdOrderLeadImportDto> dataE = listImport.AsEnumerable();
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
                                    bulkCopy.DestinationTableName = "InvCkdSmqdOrderLeadTime_T";
                                    bulkCopy.ColumnMappings.Add("Guid", "Guid");
                                    bulkCopy.ColumnMappings.Add("SupplierNo", "SupplierNo");
                                    bulkCopy.ColumnMappings.Add("Cfc", "Cfc");
                                    bulkCopy.ColumnMappings.Add("ExpCode", "ExpCode");
                                    bulkCopy.ColumnMappings.Add("Sea", "Sea");
                                    bulkCopy.ColumnMappings.Add("Air", "Air");
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
        public async Task MergeDataInvCkdSmqdOrderLeadTime(string v_Guid)
        {
            string _sql = "Exec INV_CKD_SMQD_ORDER_LEAD_TIME_MERGE @Guid";
            await _dapperRepo.QueryAsync<InvCkdSmqdOrderLeadImportDto>(_sql, new { Guid = v_Guid });
        }

        public async Task<PagedResultDto<InvCkdSmqdOrderLeadImportDto>> GetMessageErrorImportOrderLeadTime(string v_Guid)
        {
            string _sql = "Exec INV_CKD_SMQD_ORDER_LEAD_TIME_GET_LIST_ERROR_IMPORT @Guid";

            IEnumerable<InvCkdSmqdOrderLeadImportDto> result = await _dapperRepo.QueryAsync<InvCkdSmqdOrderLeadImportDto>(_sql, new
            {
                Guid = v_Guid,
            });

            var listResult = result.ToList();
            var totalCount = listResult.Count();

            return new PagedResultDto<InvCkdSmqdOrderLeadImportDto>(
                totalCount,
                listResult
               );
        }

        public async Task<FileDto> GetSmqdOrderLeadTimeListErrToExcel(string v_Guid)
        {
            string _sql = "Exec INV_CKD_SMQD_ORDER_LEAD_TIME_GET_LIST_ERROR_IMPORT @Guid";

            IEnumerable<InvCkdSmqdOrderLeadImportDto> result = await _dapperRepo.QueryAsync<InvCkdSmqdOrderLeadImportDto>(_sql, new
            {
                Guid = v_Guid,
            });

            var listResult = result.ToList();
            return _calendarListExcelExporter.ExportToFileErrOrderLeadTime(listResult);
        }
    }

}
