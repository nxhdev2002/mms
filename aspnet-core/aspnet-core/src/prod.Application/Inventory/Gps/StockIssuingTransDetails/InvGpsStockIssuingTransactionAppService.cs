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
using prod.Authorization;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using prod.Inventory.Gps.StockIssuingTransDetails;
using prod.Inventory.Gps.StockIssuingTransDetails.Dto;
using prod.Inventory.Gps.StockIssuingTransDetails.Exporting;
using prod.Inventory.Gps.StockReceivingTransDetails.Dto;
using prod.Inventory.GPS;
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
    [AbpAuthorize(AppPermissions.Pages_Gps_Warehouse_StockIssuingTransDetails_View)]
    public class InvGpsStockIssuingTransactionAppService : prodAppServiceBase, IInvGpsStockIssuingTransactionAppService
    {
        private readonly IDapperRepository<InvGpsStockTransaction, long> _dapperRepo;
        private readonly IRepository<InvGpsStockTransaction, long> _repo;
        private readonly IInvGpsStockIssuingTransactionExcelExporter _stockIssuingTransExcelExporter;

        public InvGpsStockIssuingTransactionAppService(IRepository<InvGpsStockTransaction, long> repo,
                                         IDapperRepository<InvGpsStockTransaction, long> dapperRepo,
                                        IInvGpsStockIssuingTransactionExcelExporter stockIssuingTransExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _stockIssuingTransExcelExporter = stockIssuingTransExcelExporter;
        }

        public async Task<PagedResultDto<InvGpsStockIssuingTransactionDto>> GetAll(GetStockIssuingTransactionInput input)
        {
            string _sql = "Exec INV_GPS_STOCK_TRANSACTION_SEARCH_ISSUE @p_part_no, @p_working_date_from, @p_working_date_to";//, @p_supplier_no, @p_vin_no, @p_cfc, @p_lot_no, @p_no_in_lot";

            IEnumerable<InvGpsStockIssuingTransactionDto> result = await _dapperRepo.QueryAsync<InvGpsStockIssuingTransactionDto>(_sql, new
            {
                p_part_no = input.PartNo,
                p_working_date_from = input.WorkingDateFrom,
                p_working_date_to = input.WorkingDateTo,
                //p_supplier_no = input.SupplierNo,
                //p_vin_no = input.Vin,
                //p_cfc = input.Cfc,
                //p_lot_no = input.LotNo,
                //p_no_in_lot = input.NoInLot
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            if (listResult.Count > 0) listResult[0].GrandTotal = listResult.Sum(e => e.Qty);

            return new PagedResultDto<InvGpsStockIssuingTransactionDto>(
               totalCount,
               pagedAndFiltered);
        }


        public async Task<FileDto> GetGpsStockIssuingTransToExcel(GetStockIssuingTransactionExportInput input)
        {
            string _sql = "Exec INV_GPS_STOCK_TRANSACTION_SEARCH_ISSUE @p_part_no, @p_working_date_from, @p_working_date_to";//, @p_supplier_no, @p_vin_no, @p_cfc, @p_lot_no, @p_no_in_lot";

            IEnumerable<InvGpsStockIssuingTransactionDto> result = await _dapperRepo.QueryAsync<InvGpsStockIssuingTransactionDto>(_sql, new
            {
                p_part_no = input.PartNo,
                p_working_date_from = input.WorkingDateFrom,
                p_working_date_to = input.WorkingDateTo,
                //p_supplier_no = input.SupplierNo,
                //p_vin_no = input.Vin,
                //p_cfc = input.Cfc,
                //p_lot_no = input.LotNo,
                //p_no_in_lot = input.NoInLot
            });

            var exportToExcel = result.ToList();
            return _stockIssuingTransExcelExporter.ExportToFile(exportToExcel);
        }


        //Import Issuing
        [AbpAuthorize(AppPermissions.Pages_Gps_Warehouse_StockIssuingTransDetails_Import)]
        public async Task<List<InvGpsStockIssuingTransactionDto>> ImportInvGpsStockReceivingTransFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<InvGpsStockIssuingTransactionDto> rowList = new List<InvGpsStockIssuingTransactionDto>();

                ISheet sheet;
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
                        var v_PartNo = Convert.ToString(v_worksheet.Cells[i, 1].Value);
                        if (v_PartNo != null && v_PartNo != "")
                        {
                      
                            var v_PUOM = v_worksheet.Cells[i, 2].Value;
                            var v_Qty = v_worksheet.Cells[i, 3].Value;
                            var v_CostCenter = v_worksheet.Cells[i, 4].Value;

                            InvGpsStockIssuingTransactionDto importData = new InvGpsStockIssuingTransactionDto();


                            importData.Guid = strGUID;
                            importData.PartNo = Convert.ToString(v_PartNo);
                            importData.Puom = Convert.ToString(v_PUOM);
                            try
                            {
                                if (Convert.ToInt32(v_Qty) > 0)
                                {
                                    importData.Qty = Convert.ToInt32(v_Qty);
                                }
                                else
                                {
                                    importData.ErrorDescription += "Tổng số lượng phải là dương ";
                                }

                            }
                            catch
                            {
                                importData.ErrorDescription += "Tổng số lượng không phải là số ";
                            }

                            try
                            {
                                if (Convert.ToInt32(v_CostCenter) > 0)
                                {
                                    importData.CostCenter = Convert.ToInt32(v_CostCenter);
                                }
                                else
                                {
                                    importData.ErrorDescription += "CostCenter phải là dương ";
                                }

                            }
                            catch
                            {
                                importData.ErrorDescription += "CostCenter không phải là số ";
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
                    IEnumerable<InvGpsStockIssuingTransactionDto> dataE = rowList.AsEnumerable();
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
                                bulkCopy.ColumnMappings.Add("PartNo", "PartNo");
                                bulkCopy.ColumnMappings.Add("Puom", "Puom");
                                bulkCopy.ColumnMappings.Add("Qty", "Qty");
                                bulkCopy.ColumnMappings.Add("CostCenter", "CostCenter");
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
        public async Task MergeDataInvGpsStockIssuing(string v_Guid)
        {
            string _sql = "Exec INV_GPS_STOCK_ISSUING_MERGE @Guid";
            await _dapperRepo.QueryAsync<InvGpsStockIssuingTransactionDto>(_sql, new { Guid = v_Guid });
        }

        // hiển thị lỗi import và export lỗi
        public async Task<PagedResultDto<InvGpsStockIssuingTransactionDto>> GetMessageErrorImport(string v_Guid)
        {
            string _sql = "Exec INV_GPS_STOCK_ISSUING_GET_LIST_ERROR_IMPORT @Guid";

            IEnumerable<InvGpsStockIssuingTransactionDto> result = await _dapperRepo.QueryAsync<InvGpsStockIssuingTransactionDto>(_sql, new
            {
                Guid = v_Guid
            });

            var listResult = result.ToList();
            var totalCount = listResult.Count();

            return new PagedResultDto<InvGpsStockIssuingTransactionDto>(
                totalCount,
               listResult
               );
        }

        public async Task<FileDto> GetListErrToExcel(string v_Guid)
        {
            string _sql = "Exec INV_GPS_STOCK_ISSUING_GET_LIST_ERROR_IMPORT @Guid";

            IEnumerable<InvGpsStockIssuingTransactionDto> result = await _dapperRepo.QueryAsync<InvGpsStockIssuingTransactionDto>(_sql, new
            {
                Guid = v_Guid
            });

            var exportToExcel = result.ToList();
            return _stockIssuingTransExcelExporter.ExportErrToFile(exportToExcel);
        }
    }
}
