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
using prod.Inventory.DRM.StockPartExcel.Dto;
using prod.Inventory.Gps.PartList.Dto;
using prod.Inventory.Gps.StockReceivingTransDetails.Dto;
using prod.Inventory.Gps.StockReceivingTransDetails.Exporting;
using prod.Inventory.GPS;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using prod.Common;

namespace prod.Inventory.Gps.StockReceivingTransDetails
{
    [AbpAuthorize(AppPermissions.Pages_Gps_Warehouse_StockReceivingTransDetails_View)]
    public class InvGpsStockReceivingTransactionAppService : prodAppServiceBase, IInvGpsStockReceivingTransactionAppService
    {
        private readonly IDapperRepository<InvGpsStockTransaction, long> _dapperRepo;
        private readonly IRepository<InvGpsStockTransaction, long> _repo;
        private readonly IInvGpsStockReceivingTransactionExcelExporter _receivingTransExcelExporter;

        public InvGpsStockReceivingTransactionAppService(IRepository<InvGpsStockTransaction, long> repo,
                                         IDapperRepository<InvGpsStockTransaction, long> dapperRepo,
                                        IInvGpsStockReceivingTransactionExcelExporter receivingTransExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _receivingTransExcelExporter = receivingTransExcelExporter;
        }

        public async Task<PagedResultDto<InvGpsStockReceivingTransactionDto>> GetAll(GetStockReceivingTransactionInput input)
        {
            string _sql = "Exec INV_GPS_STOCK_TRANSACTION_SEARCH_RECEIVE @p_partno, @p_workingdate_from, @p_workingdate_to";

            IEnumerable<InvGpsStockReceivingTransactionDto> result = await _dapperRepo.QueryAsync<InvGpsStockReceivingTransactionDto>(_sql, new
            {
                p_partno = input.PartNo,
                p_workingdate_from = input.WorkingDateFrom,
                p_workingdate_to = input.WorkingDateTo
            });

            var listResult = result.ToList();

            if (listResult.Count > 0) listResult[0].GrandQty = listResult.Sum(e => e.Qty);

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvGpsStockReceivingTransactionDto>(
               totalCount,
               pagedAndFiltered);
        }


        public async Task<FileDto> GetGpsStockReceivingTransToExcel(GetStockReceivingTransactionExportInput input)
        {
            string _sql = "Exec INV_GPS_STOCK_TRANSACTION_SEARCH_RECEIVE @p_partno, @p_workingdate_from, @p_workingdate_to";

            IEnumerable<InvGpsStockReceivingTransactionDto> result = await _dapperRepo.QueryAsync<InvGpsStockReceivingTransactionDto>(_sql, new
            {
                p_partno = input.PartNo,
                p_workingdate_from = input.WorkingDateFrom,
                p_workingdate_to = input.WorkingDateTo
            });

            var exportToExcel = result.ToList();
            return _receivingTransExcelExporter.ExportToFile(exportToExcel);
        }

        //Import Receiving
        [AbpAuthorize(AppPermissions.Pages_Gps_Warehouse_StockReceivingTransDetails_Import)]
        public async Task<List<InvGpsStockReceivingTransactionDto>> ImportInvGpsStockReceivingTransFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<InvGpsStockReceivingTransactionDto> rowList = new List<InvGpsStockReceivingTransactionDto>();
                CommonFunction fn = new CommonFunction();

                using (var stream = new MemoryStream(fileBytes))
                {
                    SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
                    var xlWorkBook = ExcelFile.Load(stream);
                    var v_worksheet = xlWorkBook.Worksheets[0];
                    DataFormatter formatter = new DataFormatter();
                    DateTime dateTimeCurent = DateTime.Now;

                    //  string v_devanning_date = (v_worksheet.Cells[4, 2]).Value?.ToString() ?? "";
                    string strGUID = Guid.NewGuid().ToString("N");

                    var v_WorkingDate = v_worksheet.Cells[1, 1].Value;

                    for (int i = 3; i <= v_worksheet.Rows.Count; i++)
                    {

                        var v_PartNo = Convert.ToString(v_worksheet.Cells[i, 2].Value);
                        if (v_PartNo != null && v_PartNo != "")
                        {
                            var v_PoNo = v_worksheet.Cells[i, 1].Value;
                            var v_PartName = v_worksheet.Cells[i, 3].Value;
                            var v_PUOM = v_worksheet.Cells[i, 4].Value;
                            var v_Qty = v_worksheet.Cells[i, 7].Value;

                            InvGpsStockReceivingTransactionDto importData = new InvGpsStockReceivingTransactionDto();


                            importData.Guid = strGUID;
                            importData.PoNo = Convert.ToString(v_PoNo);
                            importData.PartNo = Convert.ToString(v_PartNo);
                            importData.PartName = Convert.ToString(v_PartName);
                            importData.Puom = Convert.ToString(v_PUOM);
                            try {
                                if (Convert.ToInt32(v_Qty) > 0)
                                {
                                    importData.Qty = Convert.ToInt32(v_Qty);
                                }
                                else
                                {
                                    importData.ErrorDescription += "Tổng số lượng phải là số dương ";
                                }     
                                
                            }
                            catch
                            {
                                importData.ErrorDescription += "Tổng số lượng không phải là số ";
                            }
                            importData.WorkingDate = Convert.ToDateTime(v_WorkingDate);
                            importData.TransactionDate = dateTimeCurent;
                            importData.CreatorUserId = (int)AbpSession.UserId;
                            rowList.Add(importData);

                                 
                        }

                    }

                }
                    
                

                // import temp into db (bulkCopy)
                if (rowList.Count > 0)
                {
                    IEnumerable<InvGpsStockReceivingTransactionDto> dataE = rowList.AsEnumerable();
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
                                bulkCopy.DestinationTableName = "InvGpsStockTransaction_T";

                                bulkCopy.ColumnMappings.Add("Guid", "Guid");
                                bulkCopy.ColumnMappings.Add("PoNo", "PoNo");
                                bulkCopy.ColumnMappings.Add("PartNo", "PartNo");
                                bulkCopy.ColumnMappings.Add("PartName", "PartName");
                                bulkCopy.ColumnMappings.Add("Puom", "Puom");
                                bulkCopy.ColumnMappings.Add("Qty", "Qty");                        
                                bulkCopy.ColumnMappings.Add("WorkingDate", "WorkingDate");
                                bulkCopy.ColumnMappings.Add("TransactionDate", "TransactionDate");
                                bulkCopy.ColumnMappings.Add("CreatorUserId", "CreatorUserId");
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
        public async Task MergeDataInvGpsStockReceiving(string v_Guid)
        {
            string _sql = "Exec INV_GPS_STOCK_RECEIVING_MERGE @Guid";
            await _dapperRepo.QueryAsync<InvGpsStockReceivingTransactionDto>(_sql, new { Guid = v_Guid });
        }

        // hiển thị lỗi import và export lỗi
        public async Task<PagedResultDto<InvGpsStockReceivingTransactionDto>> GetMessageErrorImport(string v_Guid)
        {
            string _sql = "Exec INV_GPS_STOCK_RECEIVING_GET_LIST_ERROR_IMPORT @Guid";

            IEnumerable<InvGpsStockReceivingTransactionDto> result = await _dapperRepo.QueryAsync<InvGpsStockReceivingTransactionDto>(_sql, new
            {
                Guid = v_Guid
            });

            var listResult = result.ToList();
            var totalCount = listResult.Count();

            return new PagedResultDto<InvGpsStockReceivingTransactionDto>(
                totalCount,
               listResult
               );
        }

        public async Task<FileDto> GetListErrToExcel(string v_Guid)
        {
            string _sql = "Exec INV_GPS_STOCK_RECEIVING_GET_LIST_ERROR_IMPORT @Guid";

            IEnumerable<InvGpsStockReceivingTransactionDto> result = await _dapperRepo.QueryAsync<InvGpsStockReceivingTransactionDto>(_sql, new
            {
                Guid = v_Guid
            });

            var exportToExcel = result.ToList();
            return _receivingTransExcelExporter.ExportErrToFile(exportToExcel);
        }


    }
}
