using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using prod.Inventory.GPS.Exporting;
using prod.Inventory.GPS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prod.Inventory.Gps.FinStock.Exporting;
using Abp.Application.Services.Dto;
using prod.Inventory.GPS.Dto;
using prod.Inventory.Gps.FinStock.Dto;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using prod.Inventory.Gps.Mapping.Dto;
using Abp.UI;
using FastMember;
using GemBox.Spreadsheet;
using NPOI.SS.UserModel;
using prod.Common;
using System.Data;
using System.IO;
using NPOI.SS.Formula.Functions;
using Abp.Authorization;
using prod.Authorization;

namespace prod.Inventory.Gps.FinStock
{
    [AbpAuthorize(AppPermissions.Pages_Gps_Warehouse_FinStock_View)]
    public class InvGpsFinStockAppService : prodAppServiceBase, IInvGpsFinStock
    {
        private readonly IDapperRepository<InvGpsFinStock, long> _dapperRepo;
        private readonly IRepository<InvGpsFinStock, long> _repo;
        private readonly IInvGpsFinStockExcelExport _finStockExcelExporter;

        public InvGpsFinStockAppService(IRepository<InvGpsFinStock, long> repo,
                                         IDapperRepository<InvGpsFinStock, long> dapperRepo,
                                          IInvGpsFinStockExcelExport finStockExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _finStockExcelExporter = finStockExcelExporter;
        }


        public async Task<PagedResultDto<InvGpsFinStockDto>> GetAll(InvGpsFinStockImput input)
        {
            string _sql = "Exec [INV_GPS_FIN_STOCK_SEARCH] @p_partno, @p_location";

            IEnumerable<InvGpsFinStockDto> result = await _dapperRepo.QueryAsync<InvGpsFinStockDto>(_sql, new
            {
                p_partno = input.PartNo,
                p_Location = input.Location
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvGpsFinStockDto>(
               totalCount,
               pagedAndFiltered);
        }


        public async Task<FileDto> GetInvGpsFinStockToExcel(InvGpsFinStockImput input)
        {
            string _sql = "Exec INV_GPS_FIN_STOCK_SEARCH @p_partno, @p_location";

            IEnumerable<InvGpsFinStockDto> result = await _dapperRepo.QueryAsync<InvGpsFinStockDto>(_sql, new
            {
                p_partno = input.PartNo,
                p_Location = input.Location
            });

            var exportToExcel = result.ToList();
            return _finStockExcelExporter.ExportToFile(exportToExcel);
        }


        public async Task<List<InvGpsFinStockImportDto>> ImportDataInvGpsFinStockFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<InvGpsFinStockImportDto> listImport = new List<InvGpsFinStockImportDto>();
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



                    for (int i = 1; i <= v_worksheet.Rows.Count; i++)
                    {

                        string v_partNo = (v_worksheet.Cells[i, 0]).Value?.ToString() ?? "";
                        var row = new InvGpsFinStockImportDto();
                        if (v_partNo != "")
                        {
                            string v_qty = (v_worksheet.Cells[i, 1]).Value?.ToString() ?? "";
                            string v_location = (v_worksheet.Cells[i, 2]).Value?.ToString() ?? "";

                            row.Guid = strGUID;

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

                            try
                            {
                                row.Qty = Convert.ToInt32(v_qty);
                                if (row.Qty < 0)
                                {
                                    row.ErrorDescription += "UsageQty phải là số dương! ";
                                    //continue;
                                }
                     
                            }
                            catch (Exception ex)
                            {
                                row.ErrorDescription += "Qty phải là số! ";
                            }

                         
                            if(v_location != "GPS" && v_location != "T")
                            {
                                row.ErrorDescription += "Location chỉ có thể là GPS hoặc T!";
                         
                            }
                            else
                            {
                                row.Location = v_location;
                            }
                          
                  
                            listImport.Add(row);

                        }

                    }

                }
                if (listImport.Count > 0)
                {
                    IEnumerable<InvGpsFinStockImportDto> dataE = listImport.AsEnumerable();
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
                                bulkCopy.DestinationTableName = "InvGpsFinStock_T";
                                bulkCopy.ColumnMappings.Add("Guid", "Guid");
                                bulkCopy.ColumnMappings.Add("PartNo", "PartNo");
                                bulkCopy.ColumnMappings.Add("Qty", "Qty");
                                bulkCopy.ColumnMappings.Add("Location", "Location");
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

        public async Task MergeDataInvGpsFinStock(string v_Guid)
        {

            string _merge = "Exec [INV_GPS_FIN_STOCK_MERGE] @Guid";
            await _dapperRepo.QueryAsync<InvGpsFinStockImportDto>(_merge, new { Guid = v_Guid });
        }

  

        public async Task<PagedResultDto<InvGpsFinStockImportDto>> GetMessageErrorImport(string v_Guid)
        {
            string _sql = "Exec INV_GPS_FIN_STOCK_GET_LIST_ERROR_IMPORT @Guid";

            IEnumerable<InvGpsFinStockImportDto> result = await _dapperRepo.QueryAsync<InvGpsFinStockImportDto>(_sql, new
            {
                Guid = v_Guid
            });

            var listResult = result.ToList();
            var totalCount = listResult.Count();

            return new PagedResultDto<InvGpsFinStockImportDto>(
                totalCount,
               listResult
               );
        }

        public async Task<FileDto> GetInvGpsFinStockErrToExcel(string v_Guid)
        {
            string _sql = "Exec INV_GPS_FIN_STOCK_GET_LIST_ERROR_IMPORT @Guid";

            IEnumerable<InvGpsFinStockImportDto> result = await _dapperRepo.QueryAsync<InvGpsFinStockImportDto>(_sql, new
            {
                Guid = v_Guid
            });

            var exportToExcel = result.ToList();
            return _finStockExcelExporter.ExportToFileLotErr(exportToExcel);
        }

    }
}
