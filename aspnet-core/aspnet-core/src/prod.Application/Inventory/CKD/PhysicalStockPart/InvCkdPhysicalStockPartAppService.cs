using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.Linq.Extensions;
using Abp.UI;
using Dapper;
using FastMember;
using GemBox.Spreadsheet;
using IdentityServer4.Models;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.UserModel;
using prod.Authorization;
using prod.Dto;
using prod.EntityFrameworkCore;
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
      [AbpAuthorize(AppPermissions.Pages_Ckd_Physical_PhysicalStockPart_View)]
    public class InvCkdPhysicalStockPartAppService : prodAppServiceBase, IInvCkdPhysicalStockPartAppService
    {
        private readonly IDapperRepository<InvCkdPhysicalStockPart, long> _dapperRepo;
        private readonly IRepository<InvCkdPhysicalStockPart, long> _repo;
        private readonly IInvCkdPhysicalStockPartExcelExporter _errorListExcelExporter;

        public InvCkdPhysicalStockPartAppService(IRepository<InvCkdPhysicalStockPart, long> repo,
                                         IDapperRepository<InvCkdPhysicalStockPart, long> dapperRepo,
                                        IInvCkdPhysicalStockPartExcelExporter errorListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _errorListExcelExporter = errorListExcelExporter;
        }     

          [AbpAuthorize(AppPermissions.Pages_Ckd_Physical_PhysicalStockPart_Edit)]
        public async Task<string> CreateOrEdit(CreateOrEditInvCkdPhysicalStockPartDto input)

        {
            if (input.Id == null)
            {                
                return await Create(input);

            }
            else
            {
                await Update(input);
                return "";
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Ckd_Physical_PhysicalStockPart_Edit)]
        private async Task<string> Create(CreateOrEditInvCkdPhysicalStockPartDto input)
        {

            string _insert = "Exec INV_CKD_PHYSICAL_STOCKPART_INSERT @p_partNo,@p_ColorSfx," +
                "@p_Part_name,@p_Part_no_s4,@p_lot_no,@p_cfc,@p_supplier_no,@p_material_id," +
                "@p_begin_qty,@p_receive_qty,@p_issue_qty,@p_calculator_qty,@p_actual_qty," +
                "@p_last_cal_datetime,@p_UserId,@p_ResultMessage OUTPUT";
            var parameters = new DynamicParameters(new
            {
                p_partNo = input.PartNo,
                p_ColorSfx = input.ColorSfx,
                p_Part_name = input.PartName,
                p_Part_no_s4 = input.PartNoNormalizedS4,
                p_lot_no = input.LotNo,
                p_cfc = input.Cfc,
                p_supplier_no = input.SupplierNo,
                p_material_id = input.MaterialId,
                p_begin_qty = input.BeginQty, 
                p_receive_qty = input.ReceiveQty,
                p_issue_qty = input.IssueQty,
                p_calculator_qty = input.CalculatorQty,
                p_actual_qty = input.ActualQty,
                p_last_cal_datetime = input.LastCalDatetime,
                p_UserId = AbpSession.UserId,
                p_ResultMessage = ""
            });
            parameters.Add("@p_ResultMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);

            await _dapperRepo.QueryAsync<CreateOrEditInvCkdPhysicalStockPartDto>(_insert, parameters);
   
            string resultMessage = parameters.Get<string>("@p_ResultMessage");

            string errorMessage = resultMessage ?? "";            

            return errorMessage;
        }

        // EDIT
        private async Task<string> Update(CreateOrEditInvCkdPhysicalStockPartDto input)
        {
            try
            {
                string _sql = @"EXEC INV_CKD_PHYSICAL_STOCKPART_UPDATE @p_id, @p_actual_qty,@p_status,@p_UserId";
                var filtered = await _dapperRepo.ExecuteAsync(_sql, new
                {
                    p_id = input.Id,
                    p_actual_qty = input.ActualQty,
                    p_status = input.IsActive,
                    p_UserId = AbpSession.UserId
                });
                string a = filtered.ToString();
                return a;    
            }
            catch (Exception E)
            {
                return "";
            }


            //var mainObj = await _repo.GetAll()
            //.FirstOrDefaultAsync(e => e.Id == input.Id);

            //var mainObjToUpdate = ObjectMapper.Map(input, mainObj);

            //string errorMessage = ""; 
            //return errorMessage;
        }


        public async Task<PagedResultDto<InvCkdPhysicalStockPartDto>> GetAll(GetInvCkdPhysicalStockPartInput input)
        {
            string _sql = "Exec INV_CKD_PHYSICAL_STOCK_PART_GETS @p_part_no, @p_cfc, @p_supplier_no, @p_color_sfx, @p_lot_no, @p_mode, @p_period_id";

            IEnumerable<InvCkdPhysicalStockPartDto> result = await _dapperRepo.QueryAsync<InvCkdPhysicalStockPartDto>(_sql, new
            {
                p_part_no = input.PartNo,
                p_cfc = input.Cfc,
                p_supplier_no = input.SupplierNo,
                p_color_sfx = input.ColorSfx,
                p_lot_no = input.LotNo,
                p_mode = input.p_mode,
                p_period_id = input.PeriodId
            });

            var listResult = result.ToList();           


            if (listResult.Count > 0)
            {
                listResult[0].TotalBeginQty = listResult.Sum(e => e.BeginQty);
                listResult[0].TotalReceiveQty = listResult.Sum(e => e.ReceiveQty);
                listResult[0].TotalIssueQty = listResult.Sum(e => e.IssueQty);
                listResult[0].TotalCalculatorQty = listResult.Sum(e => e.CalculatorQty);
                listResult[0].TotalActualQty = listResult.Sum(e => e.ActualQty);
            }
            var totalCount = result.ToList().Count();
            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            return new PagedResultDto<InvCkdPhysicalStockPartDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<FileDto> GetPhysicalStockPartToExcel(InvCkdPhysicalStockPartExportInput input)
        {
            string _sql = "Exec INV_CKD_PHYSICAL_STOCK_PART_GETS @p_part_no, @p_cfc, @p_supplier_no, @p_color_sfx, @p_lot_no, @p_mode, @p_period_id";

            IEnumerable<InvCkdPhysicalStockPartDto> result = await _dapperRepo.QueryAsync<InvCkdPhysicalStockPartDto>(_sql, new
            {
                p_part_no = input.PartNo,
                p_cfc = input.Cfc,
                p_supplier_no = input.SupplierNo,
                p_color_sfx = input.ColorSfx,
                p_lot_no = input.LotNo,
                p_mode = input.p_mode,
                p_period_id = input.PeriodId
            });

            var exportToExcel = result.ToList();
            return _errorListExcelExporter.ExportToFile(exportToExcel);
        }

        public async Task<FileDto> GetPhysicalStockPartToExcelGroup(InvCkdPhysicalStockPartExportInput input)
        {
            string _sql = "Exec INV_CKD_PHYSICAL_STOCK_PART_GETS_GROUP @p_part_no, @p_cfc, @p_supplier_no, @p_color_sfx, @p_lot_no, @p_mode, @p_period_id";

            IEnumerable<InvCkdPhysicalStockPartDto> result = await _dapperRepo.QueryAsync<InvCkdPhysicalStockPartDto>(_sql, new
            {
                p_part_no = input.PartNo,
                p_cfc = input.Cfc,
                p_supplier_no = input.SupplierNo,
                p_color_sfx = input.ColorSfx,
                p_lot_no = input.LotNo,
                p_mode = input.p_mode,
                p_period_id = input.PeriodId
            });

            var exportToExcel = result.ToList();
            return _errorListExcelExporter.ExportToFile(exportToExcel);
        }

        [AbpAuthorize(AppPermissions.Pages_Ckd_Physical_PhysicalStockPart_Import)]
        public async Task<List<InvCkdPhysicalStockPartDto_T>> ImportDataInvCkdPhysicalStockPartFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<InvCkdPhysicalStockPartDto_T> listImport = new List<InvCkdPhysicalStockPartDto_T>();
                using (var stream = new MemoryStream(fileBytes))
                {
                    SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
                    var xlWorkBook = ExcelFile.Load(stream);
                    var v_worksheet = xlWorkBook.Worksheets[0];
                    DataFormatter formatter = new DataFormatter();
                    DateTime dateTime = DateTime.Now;

                    string strGUID = Guid.NewGuid().ToString("N");

                    for (int i = 1; i < v_worksheet.Rows.Count; i++)
                    {
                        string v_partNo = (v_worksheet.Cells[i, 0]).Value?.ToString() ?? "";

                        if (v_partNo != "")
                        {
                            var row = new InvCkdPhysicalStockPartDto_T();
                            row.Guid = strGUID;
                            row.PartNo = v_partNo;
                            row.Cfc = (v_worksheet.Cells[i, 1]).Value?.ToString() ?? "";
                            row.SupplierNo = (v_worksheet.Cells[i, 2]).Value?.ToString() ?? "";
                            row.Qty = Convert.ToInt32((v_worksheet.Cells[i, 3]).Value?.ToString() ?? "0");
                            row.ErrorDescription = "";
                            row.CreatorUserId = AbpSession.UserId;
                            listImport.Add(row);
                        }
                    }

                }
                // import temp into db (bulkCopy)
                if (listImport.Count > 0)
                {
                    IEnumerable<InvCkdPhysicalStockPartDto_T> dataE = listImport.AsEnumerable();
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
                                bulkCopy.DestinationTableName = "InvCkdPhysicalStockPart_T";
                                bulkCopy.ColumnMappings.Add("Guid", "Guid");
                                bulkCopy.ColumnMappings.Add("PartNo", "PartNo");
                                bulkCopy.ColumnMappings.Add("Cfc", "Cfc");
                                bulkCopy.ColumnMappings.Add("SupplierNo", "SupplierNo");              
                                bulkCopy.ColumnMappings.Add("Qty", "Qty");
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
                throw new UserFriendlyException(400, ex.Message);
    }
}

        [AbpAuthorize(AppPermissions.Pages_Ckd_Physical_PhysicalStockPart_Import)]
        public async Task<List<InvCkdPhysicalStockLotDto_T>> ImportDataInvCkdPhysicalStockLotFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<InvCkdPhysicalStockLotDto_T> listImport = new List<InvCkdPhysicalStockLotDto_T>();
                using (var stream = new MemoryStream(fileBytes))
                {
                    SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
                    var xlWorkBook = ExcelFile.Load(stream);
                    var v_worksheet = xlWorkBook.Worksheets[0];
                    DataFormatter formatter = new DataFormatter();
                    DateTime dateTime = DateTime.Now;

                    string strGUID = Guid.NewGuid().ToString("N");

                    for (int i = 1; i < v_worksheet.Rows.Count; i++)
                    {
                        string v_LotNo = (v_worksheet.Cells[i, 0]).Value?.ToString() ?? "";

                        if (v_LotNo != "")
                        {
                            var row = new InvCkdPhysicalStockLotDto_T();
                            row.Guid = strGUID;
                            row.LotNo = v_LotNo;
                            row.SupplierNo = (v_worksheet.Cells[i, 1]).Value?.ToString() ?? "";
                            row.Shop = (v_worksheet.Cells[i, 2]).Value?.ToString() ?? "";                       
                            row.Qty = Convert.ToInt32((v_worksheet.Cells[i, 3]).Value?.ToString() ?? "0");
                            row.ErrorDescription = "";
                            row.CreatorUserId = AbpSession.UserId;
                            listImport.Add(row);
                        }
                    }

                }
                // import temp into db (bulkCopy)
                if (listImport.Count > 0)
                {
                    IEnumerable<InvCkdPhysicalStockLotDto_T> dataE = listImport.AsEnumerable();
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
                                bulkCopy.DestinationTableName = "InvCkdPhysicalStockLot_T";
                                bulkCopy.ColumnMappings.Add("Guid", "Guid");
                                bulkCopy.ColumnMappings.Add("LotNo", "LotNo");
                                bulkCopy.ColumnMappings.Add("SupplierNo", "SupplierNo");
                                bulkCopy.ColumnMappings.Add("Shop", "Shop");
                                bulkCopy.ColumnMappings.Add("Qty", "Qty");
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
                throw new UserFriendlyException(400, ex.Message);
            }
        }
       
        public async Task MergeDataInvCkdPhysicalStockPart(string v_Guid, long? id)
        {

            string _merge = "Exec INV_CKD_PHYSICAL_STOCKPART_MERGE @Guid,@periodId";
            await _dapperRepo.QueryAsync<ImportCkdPartListDto>(_merge, new { Guid = v_Guid , periodId = id});
        }

        public async Task MergeDataInvCkdPhysicalStockLot(string v_Guid, long? id)
        {

            string _merge = "Exec INV_CKD_PHYSICAL_STOCKLOT_MERGE @Guid,@periodId";
            await _dapperRepo.QueryAsync<ImportCkdPartListDto>(_merge, new { Guid = v_Guid, periodId = id });
        }

        public async Task<PagedResultDto<InvCkdPhysicalStockErrDto>> GetMessageErrorImport(string v_Guid , string v_Screen)
        {
            string _sql = "Exec INV_CKD_PHYSICAL_STOCK_PART_GET_LIST_ERROR_IMPORT @Guid, @Screen";

            IEnumerable<InvCkdPhysicalStockErrDto> result = await _dapperRepo.QueryAsync<InvCkdPhysicalStockErrDto>(_sql, new
            {
                Guid = v_Guid,
                Screen = v_Screen
            });

            var listResult = result.ToList();
            var totalCount = listResult.Count();

            return new PagedResultDto<InvCkdPhysicalStockErrDto>(
                totalCount,
               listResult
               );
        }

        public async Task<FileDto> GetListErrToExcel(string v_Guid, string v_Screen)
        {
            FileDto a = new FileDto();
            string _sql = "Exec INV_CKD_PHYSICAL_STOCK_PART_GET_LIST_ERROR_IMPORT @Guid, @Screen";

            IEnumerable<InvCkdPhysicalStockErrDto> result = await _dapperRepo.QueryAsync<InvCkdPhysicalStockErrDto>(_sql, new
            {
                Guid = v_Guid,
                Screen = v_Screen
            });

            var exportToExcel = result.ToList();
            if(v_Screen == "P")
            {
                 a = _errorListExcelExporter.ExportListErrToFile(exportToExcel);
            }
            else if (v_Screen == "L")
            {
                a = _errorListExcelExporter.ExportListLotErrToFile(exportToExcel);
            }

            return a;      
        }


        [AbpAuthorize(AppPermissions.Pages_Ckd_Physical_PhysicalStockPart_Calculator)]
        public async Task CalculatorStockPart()
        {
            Dapper.SqlMapper.Settings.CommandTimeout = 900;
            string _calculator = "Exec JOB_INV_CKD_PHYSICAL_STOCK_PART_CACULATOR";
            await _dapperRepo.QueryAsync<InvCkdPhysicalStockPartDto>(_calculator, new {});
        }
        public async Task SendMailCalCulate(string startTime,string endTime)
        {          
            string _sendmail = "Exec Inv_Ckd_Send_Mail_Caculator @start_time,@end_time";
            await _dapperRepo.ExecuteAsync(_sendmail, new 
            {
                start_time = startTime,
                end_time = endTime
            });
        }

        public async Task<FileDto> GetPhysicalStockPartDetailsToExcel(InvCkdPhysicalStockPartExportInput input)
        {
            Dapper.SqlMapper.Settings.CommandTimeout = 900;

            string _sql = "Exec INV_CKD_PHYSICAL_STOCK_PART_EXPORT_DETAILS_DATA @p_part_no, @p_cfc, @p_supplier_no, @p_color_sfx, @p_lot_no, @p_mode, @p_period_id";

            IEnumerable<InvCkdPhysicalStockPartDto> result = await _dapperRepo.QueryAsync<InvCkdPhysicalStockPartDto>(_sql, new
            {
                p_part_no = input.PartNo,
                p_cfc = input.Cfc,
                p_supplier_no = input.SupplierNo,
                p_color_sfx = input.ColorSfx,
                p_lot_no = input.LotNo,
                p_mode = input.p_mode,
                p_period_id = input.PeriodId
            });

            var exportToExcel = result.ToList();
            return _errorListExcelExporter.ExportToFile(exportToExcel);
        }


        public async Task<FileDto> GetPhysicalSummaryStockByPart(InvCkdPhysicalStockPartExportInput input)
        {
            string _sql = "Exec INV_CKD_PHYSICAL_STOCK_PART_SUMMARY_STOCK_BY_PART @p_PartNo, @p_SupplierNo, @p_ColorSfx, @p_LotNo, @p_Mode, @p_PeriodId";

            IEnumerable<InvCkdPhysicalStockPartDto> result = await _dapperRepo.QueryAsync<InvCkdPhysicalStockPartDto>(_sql, new
            {
                p_PartNo = input.PartNo,
                p_SupplierNo = input.SupplierNo,
                p_ColorSfx = input.ColorSfx,
                p_LotNo = input.LotNo,
                p_Mode = input.p_mode,
                p_PeriodId = input.PeriodId
            });

            var exportToExcel = result.ToList();
            return _errorListExcelExporter.ExportSummaryStockByPart(exportToExcel);
        }
    }
}
