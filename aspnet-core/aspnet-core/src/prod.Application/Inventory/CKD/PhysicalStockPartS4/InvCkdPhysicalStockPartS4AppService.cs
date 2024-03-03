using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using FastMember;
using GemBox.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.UserModel;
using prod;
using prod.Authorization;
using prod.Dto;
using prod.Inv.CKD;
using prod.Inv.CKD.Dto;
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

namespace prod.Inventory.CKD
{
    [AbpAuthorize(AppPermissions.Pages_Ckd_Physical_PhysicalStockPartS4_View)]
    public class InvCkdPhysicalStockPartS4AppService : prodAppServiceBase, IInvCkdPhysicalStockPartS4AppService
    {
        private readonly IDapperRepository<InvCkdPhysicalStockPartS4, long> _dapperRepo;
        private readonly IRepository<InvCkdPhysicalStockPartS4, long> _repo;
        private readonly InvCkdPhysicalStockPartS4ExcelExporter _invCkdPhysicalStockPartS4ListExcelExporter;

        public InvCkdPhysicalStockPartS4AppService(IRepository<InvCkdPhysicalStockPartS4, long> repo,
                                         IDapperRepository<InvCkdPhysicalStockPartS4, long> dapperRepo,
                                         InvCkdPhysicalStockPartS4ExcelExporter invCkdPhysicalStockPartS4ListExcelExporter)
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _invCkdPhysicalStockPartS4ListExcelExporter = invCkdPhysicalStockPartS4ListExcelExporter;

        }
        public async Task<PagedResultDto<InvCkdPhysicalStockPartS4Dto>> GetAll(GetInvCkdPhysicalStockPartS4Input input)
        {
            string _sql = "Exec INV_CKD_PHYSICAL_STOCK_PART_S4_SEARCH @materialCode, @periodId";

            IEnumerable<InvCkdPhysicalStockPartS4Dto> result = await _dapperRepo.QueryAsync<InvCkdPhysicalStockPartS4Dto>(_sql, new
            {
                materialCode = input.MaterialCode,
                periodId = input.PeriodId
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = listResult.Count();

            return new PagedResultDto<InvCkdPhysicalStockPartS4Dto>(
                totalCount,
                pagedAndFiltered);
           
        }

        public async Task<FileDto> GetPhysicalStockCPartS4CompareToExcel(int? periodId)
        {
            Dapper.SqlMapper.Settings.CommandTimeout = 900;
            string _sql = "Exec INV_CKD_PHYSICAL_STOCK_PART_S4_COMPARE @periodId";
            IEnumerable<InvCkdPhysicalStockPartS4Dto> result = await _dapperRepo.QueryAsync<InvCkdPhysicalStockPartS4Dto>(_sql, new
            {
                periodId = periodId
            });
            var exportToExcel = result.ToList();
            return _invCkdPhysicalStockPartS4ListExcelExporter.ExportToFile(exportToExcel);
        }


        [AbpAuthorize(AppPermissions.Pages_Ckd_Physical_PhysicalStockPartS4_Import)]
        public async Task<List<InvCkdPhysicalStockPartS4Dto>> ImportDataPhysicalStockCPartS4FromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<InvCkdPhysicalStockPartS4Dto> listImport = new List<InvCkdPhysicalStockPartS4Dto>();
                using (var stream = new MemoryStream(fileBytes))
                {
                    SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
                    var xlWorkBook = ExcelFile.Load(stream);
                    var v_worksheet = xlWorkBook.Worksheets[0];
                    DataFormatter formatter = new DataFormatter();
                    DateTime dateTime = DateTime.Now;

                    string strGUID = Guid.NewGuid().ToString("N");

                    string _sql = @"EXEC CMM_IMPORT_USER_GUID_INSERT @Guid, @p_UserId";
                    _dapperRepo.ExecuteAsync(_sql, new
                    {
                        Guid = strGUID,
                        p_UserId = AbpSession.UserId
                    });

                    for (int i = 1; i < v_worksheet.Rows.Count; i++)
                    {
                        string v_MaterialCode = (v_worksheet.Cells[i, 0]).Value?.ToString() ?? "";

                        if (v_MaterialCode != "")
                        {
                            var row = new InvCkdPhysicalStockPartS4Dto();
                            row.Guid = strGUID;
                            row.MaterialCode = v_MaterialCode;
                            row.PeriodId = Convert.ToInt32((v_worksheet.Cells[i, 1]).Value?.ToString() ?? "0");
                            row.Qty = Convert.ToInt32((v_worksheet.Cells[i, 2]).Value?.ToString() ?? "0");
                            row.ErrorDescription = "";

                            listImport.Add(row);
                        }
                    }

                }
                // import temp into db (bulkCopy)
                if (listImport.Count > 0)
                {
                    IEnumerable<InvCkdPhysicalStockPartS4Dto> dataE = listImport.AsEnumerable();
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
                                bulkCopy.DestinationTableName = "InvCkdPhysicalStockPartS4_T";
                                bulkCopy.ColumnMappings.Add("Guid", "Guid");
                                bulkCopy.ColumnMappings.Add("MaterialCode", "MaterialCode");
                                bulkCopy.ColumnMappings.Add("PeriodId", "PeriodId");
                                bulkCopy.ColumnMappings.Add("Qty", "Qty");
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

        public async Task MergeDataInvCkdPhysicalStockPartS4(string v_Guid)
        {

            string _merge = "Exec INV_CKD_PHYSICAL_STOCK_PART_S4_MERGE @Guid";
            await _dapperRepo.QueryAsync<InvCkdPhysicalStockPartS4Dto>(_merge, new { Guid = v_Guid });
        }

        public async Task<PagedResultDto<InvCkdPhysicalStockPartS4Dto>> GetMessageErrorImport(string v_Guid)
        {
            string _sql = "Exec INV_CKD_PHYSICAL_STOCK_PART_S4_GET_LIST_ERROR_IMPORT @Guid";

            IEnumerable<InvCkdPhysicalStockPartS4Dto> result = await _dapperRepo.QueryAsync<InvCkdPhysicalStockPartS4Dto>(_sql, new
            {
                Guid = v_Guid
            });

            var listResult = result.ToList();
            var totalCount = listResult.Count();

            return new PagedResultDto<InvCkdPhysicalStockPartS4Dto>(
                totalCount,
               listResult
               );
        }

        public async Task<FileDto> GetListErrToExcel(string v_Guid)
        {
            FileDto a = new FileDto();
            string _sql = "Exec INV_CKD_PHYSICAL_STOCK_PART_S4_GET_LIST_ERROR_IMPORT @Guid";

            IEnumerable<InvCkdPhysicalStockPartS4Dto> result = await _dapperRepo.QueryAsync<InvCkdPhysicalStockPartS4Dto>(_sql, new
            {
                Guid = v_Guid
            });

            var exportToExcel = result.ToList();

            return _invCkdPhysicalStockPartS4ListExcelExporter.ExportListErrToFile(exportToExcel);
            }

      

    }
}