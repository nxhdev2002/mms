using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.UI;
using FastMember;
using GemBox.Spreadsheet;
using prod.Authorization;
using prod.Dto;
using prod.Inventory.DRM.StockPart.Dto;
using prod.Inventory.DRM.StockPart.Exporting;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using prod.Common;

namespace prod.Inventory.DRM.StockPart
{
    [AbpAuthorize(AppPermissions.Pages_DMIHP_Stock_StockPart_View)]
    public class InvDrmStockPartAppService : prodAppServiceBase, IInvDrmStockPartAppService
    {
        private readonly IDapperRepository<InvDrmStockPart, long> _dapperRepo;
        private readonly IRepository<InvDrmStockPart, long> _repo;
        private readonly IInvDrmStockPartExcelExporter _calendarListExcelExporter;

        public InvDrmStockPartAppService(IRepository<InvDrmStockPart, long> repo,
                                         IDapperRepository<InvDrmStockPart, long> dapperRepo,
                                        IInvDrmStockPartExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        public async Task<PagedResultDto<InvDrmStockPartDto>> GetAll(GetInvDrmStockPartInput input)
        {
            string _sql = "Exec INV_DRM_STOCK_PART_SEARCH @p_materialcode, @p_materialspec, @p_partno, @p_partname";

            IEnumerable<InvDrmStockPartDto> result = await _dapperRepo.QueryAsync<InvDrmStockPartDto>(_sql, new
            {
                p_materialcode = input.MaterialCode,
                p_materialspec = input.MaterialSpec,
                p_partno = input.PartNo,
                p_partname = input.PartName

            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvDrmStockPartDto>(
                totalCount,
                 pagedAndFiltered
            );
        }

        public async Task<FileDto> GetDrmStockPartToExcel(GetInvDrmStockPartExportInput input)
        {
            string _sql = "Exec INV_DRM_STOCK_PART_SEARCH @p_materialcode, @p_materialspec, @p_partno, @p_partname";

            IEnumerable<InvDrmStockPartDto> result = await _dapperRepo.QueryAsync<InvDrmStockPartDto>(_sql, new
            {
                p_materialcode = input.MaterialCode,
                p_materialspec = input.MaterialSpec,
                p_partno = input.PartNo,
                p_partname = input.PartName

            });

            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        public async Task<List<InvDrmStockPartImportDto>> ImportInvDRMStockPartFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<InvDrmStockPartImportDto> listImport = new List<InvDrmStockPartImportDto>();
                using (var stream = new MemoryStream(fileBytes))
                {
                    SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
                    var xlWorkBook = ExcelFile.Load(stream);
                    var v_worksheet = xlWorkBook.Worksheets[0];

                    string strGUID = Guid.NewGuid().ToString("N");

                    for (int i = 7; i < v_worksheet.Rows.Count; i++)
                    {
                        string v_supplier = (v_worksheet.Cells[i, 1]).Value?.ToString() ?? "";

                        if (v_supplier != "")
                        {
                            string v_model = (v_worksheet.Cells[i, 2]).Value?.ToString() ?? "";
                            string v_materialcode = (v_worksheet.Cells[i, 3]).Value?.ToString() ?? "";
                            string v_materialspec = (v_worksheet.Cells[i, 4]).Value?.ToString() ?? "";
                            string v_partcode = (v_worksheet.Cells[i, 5]).Value?.ToString() ?? "";
                            string v_stock = (v_worksheet.Cells[i, 6]).Value?.ToString() ?? "";
                            string v_deliverydate = (v_worksheet.Cells[i, 7]).Value?.ToString() ?? "";

                            var row = new InvDrmStockPartImportDto();
                            row.Guid = strGUID;
                            row.ErrorDescription = "";

                            if (v_supplier.Length > 10)
                            {
                                row.ErrorDescription = "Model " + v_supplier + " dài quá 10 kí tự! ";
                            }
                            else
                            {
                                row.SupplierNo = v_supplier;
                            }
                            if (v_model.Length > 4)
                            {
                                row.ErrorDescription += "Cfc " + v_model + " dài quá 4 kí tự! ";
                            }
                            else
                            {
                                row.Cfc = v_model;
                            }
                            row.MaterialCode = v_materialcode;
                            row.MaterialSpec = v_materialspec;
                            row.PartCode = v_partcode;
                            try
                            {
                                if (string.IsNullOrEmpty(v_stock))
                                {
                                    row.Qty = null;
                                }
                                else
                                {
                                    row.Qty = Convert.ToInt32(v_stock);
                                    if (row.Qty < 0)
                                    {
                                        row.ErrorDescription += "Stock Qty phải là số dương! ";
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                row.ErrorDescription += "Stock Qty " + v_stock + " không phải là số! ";
                            }
                            try
                            {
                                row.WorkingDate = DateTime.Parse(v_deliverydate);
                            }
                            catch (Exception e)
                            {
                                row.ErrorDescription += "WorkingDate " + v_deliverydate + " không đúng định dạng! ";
                            }

                            listImport.Add(row);
                        }
                    }
                    // import temp into db (bulkCopy)
                    if (listImport.Count > 0)
                    {
                        IEnumerable<InvDrmStockPartImportDto> dataE = listImport.AsEnumerable();
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
                                    bulkCopy.ColumnMappings.Add("SupplierNo", "SupplierNo");
                                    bulkCopy.ColumnMappings.Add("Cfc", "Cfc");
                                    bulkCopy.ColumnMappings.Add("MaterialCode", "MaterialCode");
                                    bulkCopy.ColumnMappings.Add("MaterialSpec", "MaterialSpec");
                                    bulkCopy.ColumnMappings.Add("PartCode", "PartCode");
                                    bulkCopy.ColumnMappings.Add("Qty", "Qty");
                                    bulkCopy.ColumnMappings.Add("WorkingDate", "WorkingDate");
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
            string _sql = "Exec INV_DRM_STOCK_PART_MERGE @Guid";
            await _dapperRepo.QueryAsync<InvDrmStockPartImportDto>(_sql, new { Guid = v_Guid });
        }

        // hiển thị lỗi import và export lỗi
        public async Task<PagedResultDto<InvDrmStockPartImportDto>> GetMessageErrorImport(string v_Guid)
        {
            string _sql = "Exec INV_DRM_STOCK_PART_GET_LIST_ERROR_IMPORT @Guid";

            IEnumerable<InvDrmStockPartImportDto> result = await _dapperRepo.QueryAsync<InvDrmStockPartImportDto>(_sql, new
            {
                Guid = v_Guid
            });

            var listResult = result.ToList();
            var totalCount = listResult.Count();

            return new PagedResultDto<InvDrmStockPartImportDto>(
                totalCount,
               listResult
               );
        }

        public async Task<FileDto> GetListErrToExcel(string v_Guid)
        {
            string _sql = "Exec INV_DRM_STOCK_PART_GET_LIST_ERROR_IMPORT @Guid";

            IEnumerable<InvDrmStockPartImportDto> result = await _dapperRepo.QueryAsync<InvDrmStockPartImportDto>(_sql, new
            {
                Guid = v_Guid
            });

            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportToFileErr(exportToExcel);
        }

    }
}
