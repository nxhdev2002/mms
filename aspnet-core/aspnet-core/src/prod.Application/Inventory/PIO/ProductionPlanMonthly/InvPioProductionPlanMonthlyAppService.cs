using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.UI;
using FastMember;
using GemBox.Spreadsheet;
using prod.Authorization;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using prod.Inventory.CKD.PartRobbing.Dto;
using prod.Inventory.CKD.PaymentRequest.Exporting;
using prod.Inventory.CKD.ProductionPlanMonthly.Dto;
using prod.Inventory.CKD.ProductionPlanMonthly.Exporting;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prod.Common;
using Twilio.Rest.Api.V2010.Account;
using static System.Runtime.InteropServices.JavaScript.JSType;
using prod.Inventory.PIO;
using prod.Inventory.PIO.InvPioProductionPlanMonthly.Exporting;
using prod.Inventory.PIO.InvPioProductionPlanMonthly.Dto;

namespace prod.Inventory.ProductionPlanMonthly
{
    [AbpAuthorize(AppPermissions.Pages_ProdPlan_ProductionPlanMonthly_View)]
    public class InvPioProductionPlanMonthlyAppService : prodAppServiceBase, IInvPioProductionPlanMonthlyAppService
    {

        private readonly IDapperRepository<InvPioProductionPlanMonthly, long> _dapperRepo;
        private readonly IInvPioProductionPlanMonthlyExcelExporter _calendarListExcelExporter;

        public InvPioProductionPlanMonthlyAppService(IDapperRepository<InvPioProductionPlanMonthly, long> dapperRepo,
                                         IInvPioProductionPlanMonthlyExcelExporter calendarListExcelExporter
            )
        {
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }
        public async Task<PagedResultDto<InvPioProductionPlanMonthlyDto>> GetAll(InvPioProductionPlanMonthlyInput input)
        {
            string _ProdMonth = (input.ProdMonth.ToString("dd-MM-yyyy") != "01-01-0001") ? input.ProdMonth.ToString("yyyy-MM") : "";
            string _sql = "Exec INV_PIO_PRODUCTION_PLAN_MONTHLY_SEARCH @p_cfc, @p_grade, @prodMonth";

            IEnumerable<InvPioProductionPlanMonthlyDto> result = await _dapperRepo.QueryAsync<InvPioProductionPlanMonthlyDto>(_sql, new
            {
                p_cfc = input.Cfc,
                p_grade = input.Grade,
                prodMonth = _ProdMonth
            });

            var listResult = result.ToList();

            if (listResult.Count > 0)
            {
                listResult[0].Total_N_3 = listResult.Sum(e => e.N_3);
                listResult[0].Total_N_2 = listResult.Sum(e => e.N_2);
                listResult[0].Total_N_1 = listResult.Sum(e => e.N_1);
                listResult[0].Total_N = listResult.Sum(e => e.N);
                listResult[0].Total_N1 = listResult.Sum(e => e.N1);
                listResult[0].Total_N2 = listResult.Sum(e => e.N2);
                listResult[0].Total_N3 = listResult.Sum(e => e.N3);
                listResult[0].Total_N4 = listResult.Sum(e => e.N4);
                listResult[0].Total_N5 = listResult.Sum(e => e.N5);
                listResult[0].Total_N6 = listResult.Sum(e => e.N6);
                listResult[0].Total_N7 = listResult.Sum(e => e.N7);
                listResult[0].Total_N8 = listResult.Sum(e => e.N8);
                listResult[0].Total_N9 = listResult.Sum(e => e.N9);
                listResult[0].Total_N10 = listResult.Sum(e => e.N10);
                listResult[0].Total_N11 = listResult.Sum(e => e.N11);
                listResult[0].Total_N12 = listResult.Sum(e => e.N12);
            }


            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvPioProductionPlanMonthlyDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<FileDto> GetInvPioProdPlanMonthlyToExcel(InvPioProductionPlanMonthlyInput input)
        {
            string _ProdMonth = (input.ProdMonth.ToString("dd-MM-yyyy") != "01-01-0001") ? input.ProdMonth.ToString("yyyy-MM") : "";
            string _sql = "Exec INV_PIO_PRODUCTION_PLAN_MONTHLY_SEARCH @p_cfc, @p_grade, @prodMonth";

            IEnumerable<InvPioProductionPlanMonthlyDto> result = await _dapperRepo.QueryAsync<InvPioProductionPlanMonthlyDto>(_sql, new
            {
                p_cfc = input.Cfc,
                p_grade = input.Grade,
                prodMonth = _ProdMonth
            });

            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }


        [AbpAuthorize(AppPermissions.Pages_PIO_Master_ProductionPlanMonthly_Import)]
        public async Task<List<InvPioProductionPlanMonthlyImportDto>> ImportPioProductionPlanMonthlyFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<InvPioProductionPlanMonthlyImportDto> listImport = new List<InvPioProductionPlanMonthlyImportDto>();
                using (var stream = new MemoryStream(fileBytes))
                {
                    SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
                    var xlWorkBook = ExcelFile.Load(stream);
                    var v_worksheet = xlWorkBook.Worksheets[0];

                    string strGUID = Guid.NewGuid().ToString("N");

                    string _sql = @"EXEC CMM_IMPORT_USER_GUID_INSERT @Guid, @p_UserId";
                    await _dapperRepo.ExecuteAsync(_sql, new
                    {
                        Guid = strGUID,
                        p_UserId = AbpSession.UserId
                    });

                    int countGrade = 0;
                    int startcol = 5;
                    int startrow = 9;
                    int x = 8;
                    DateTime date = DateTime.Now;
                    int m = date.Month;

                    //Đếm số lượng bản ghi Grade
                    for (int i = startcol; i <= 500; i++)
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(v_worksheet.Cells[startrow, i].Value)))
                        {
                            countGrade++;
                        }
                        else { break; }

                    }
                    //tìm cột ứng với tháng hiện tại
                    for (int i = startcol; i <= 500; i++)
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(v_worksheet.Cells[6, i].Value)))
                        {
                            if (v_worksheet.Cells[6, i].Value.ToString().Substring(
                                v_worksheet.Cells[6, i].Value.ToString().Length - 1, 1) == m.ToString())
                            {
                                x = i;
                                break;
                            }

                        }
                        else { break; }

                    }

                    for (int i = startrow; i < v_worksheet.Rows.Count; i++)
                    {
                        string v_grade = (v_worksheet.Cells[i, 3]).Value?.ToString() ?? "";

                        if (v_grade != "")
                        {
                            string v_model = (v_worksheet.Cells[i, 2]).Value?.ToString() ?? "";
                            int v_n_3 = Int32.Parse((v_worksheet.Cells[i, x - 3]).Value?.ToString() ?? "0");
                            int v_n_2 = Int32.Parse((v_worksheet.Cells[i, x - 2]).Value?.ToString() ?? "0");
                            int v_n_1 = Int32.Parse((v_worksheet.Cells[i, x - 1]).Value?.ToString() ?? "0");
                            int v_n = Int32.Parse((v_worksheet.Cells[i, x]).Value?.ToString() ?? "0");
                            int v_n1 = Int32.Parse((v_worksheet.Cells[i, x + 1]).Value?.ToString() ?? "0");
                            int v_n2 = Int32.Parse((v_worksheet.Cells[i, x + 2]).Value?.ToString() ?? "0");
                            int v_n3 = Int32.Parse((v_worksheet.Cells[i, x + 3]).Value?.ToString() ?? "0");
                            int v_n4 = Int32.Parse((v_worksheet.Cells[i, x + 4]).Value?.ToString() ?? "0");
                            int v_n5 = Int32.Parse((v_worksheet.Cells[i, x + 5]).Value?.ToString() ?? "0");
                            int v_n6 = Int32.Parse((v_worksheet.Cells[i, x + 6]).Value?.ToString() ?? "0");
                            int v_n7 = Int32.Parse((v_worksheet.Cells[i, x + 7]).Value?.ToString() ?? "0");
                            int v_n8 = Int32.Parse((v_worksheet.Cells[i, x + 8]).Value?.ToString() ?? "0");
                            int v_n9 = Int32.Parse((v_worksheet.Cells[i, x + 9]).Value?.ToString() ?? "0");
                            int v_n10 = Int32.Parse((v_worksheet.Cells[i, x + 10]).Value?.ToString() ?? "0");
                            int v_n11 = Int32.Parse((v_worksheet.Cells[i, x + 11]).Value?.ToString() ?? "0");
                            int v_n12 = Int32.Parse((v_worksheet.Cells[i, x + 12]).Value?.ToString() ?? "0");


                            var row = new InvPioProductionPlanMonthlyImportDto();
                            row.Guid = strGUID;
                            row.Cfc = v_model;
                            row.Grade = v_grade;
                            row.ErrorDescription = "";

                            row.N_3 = Convert.ToInt32(v_n_3);
                            row.N_2 = Convert.ToInt32(v_n_2);
                            row.N_1 = Convert.ToInt32(v_n_1);
                            row.N = Convert.ToInt32(v_n);
                            row.N1 = Convert.ToInt32(v_n1);
                            row.N2 = Convert.ToInt32(v_n2);
                            row.N3 = Convert.ToInt32(v_n3);
                            row.N4 = Convert.ToInt32(v_n4);
                            row.N5 = Convert.ToInt32(v_n5);
                            row.N6 = Convert.ToInt32(v_n6);
                            row.N7 = Convert.ToInt32(v_n7);
                            row.N8 = Convert.ToInt32(v_n8);
                            row.N9 = Convert.ToInt32(v_n9);
                            row.N10 = Convert.ToInt32(v_n10);
                            row.N11 = Convert.ToInt32(v_n11);
                            row.N12 = Convert.ToInt32(v_n12);

                            if (row.N_3 < 0 || row.N_2 < 0 || row.N_1 < 0 || row.N < 0 || row.N1 < 0 ||
                                row.N2 < 0 || row.N3 < 0 || row.N4 < 0 || row.N5 < 0 || row.N6 < 0 ||
                                row.N7 < 0 || row.N8 < 0 || row.N9 < 0 || row.N10 < 0 || row.N11 < 0 ||
                                row.N12 < 0)
                            {
                                row.ErrorDescription = "Số lượng xe nhập về không được âm";
                            }

                            listImport.Add(row);
                        }

                    }
                    // import temp into db (bulkCopy)
                    if (listImport.Count > 0)
                    {
                        IEnumerable<InvPioProductionPlanMonthlyImportDto> dataE = listImport.AsEnumerable();
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
                                    bulkCopy.DestinationTableName = "InvPioProductionPlanMonthly_T";
                                    bulkCopy.ColumnMappings.Add("Guid", "Guid");
                                    bulkCopy.ColumnMappings.Add("Cfc", "Cfc");
                                    bulkCopy.ColumnMappings.Add("Grade", "Grade");
                                    bulkCopy.ColumnMappings.Add("N_3", "N_3");
                                    bulkCopy.ColumnMappings.Add("N_2", "N_2");
                                    bulkCopy.ColumnMappings.Add("N_1", "N_1");
                                    bulkCopy.ColumnMappings.Add("N", "N");
                                    bulkCopy.ColumnMappings.Add("N1", "N1");
                                    bulkCopy.ColumnMappings.Add("N2", "N2");
                                    bulkCopy.ColumnMappings.Add("N3", "N3");
                                    bulkCopy.ColumnMappings.Add("N4", "N4");
                                    bulkCopy.ColumnMappings.Add("N5", "N5");
                                    bulkCopy.ColumnMappings.Add("N6", "N6");
                                    bulkCopy.ColumnMappings.Add("N7", "N7");
                                    bulkCopy.ColumnMappings.Add("N8", "N8");
                                    bulkCopy.ColumnMappings.Add("N9", "N9");
                                    bulkCopy.ColumnMappings.Add("N10", "N10");
                                    bulkCopy.ColumnMappings.Add("N11", "N11");
                                    bulkCopy.ColumnMappings.Add("N12", "N12");

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
        public async Task MergeDataInvPioProdPlanMonthly(string v_Guid)
        {
            string _sql = "Exec INV_PIO_PRODUCTION_PLAN_MONTHLY_MERGE @Guid";
            await _dapperRepo.QueryAsync<InvPioProductionPlanMonthlyImportDto>(_sql, new { Guid = v_Guid });
        }

        // hiển thị lỗi import và export lỗi
        public async Task<PagedResultDto<InvPioProductionPlanMonthlyImportDto>> GetMessageErrorPioProductionPlanMonthlyImport(string v_Guid)
        {
            string _sql = "Exec INV_PIO_PRODUCTION_PLAN_MONTHLY_GET_LIST_ERROR_IMPORT @Guid";

            IEnumerable<InvPioProductionPlanMonthlyImportDto> result = await _dapperRepo.QueryAsync<InvPioProductionPlanMonthlyImportDto>(_sql, new
            {
                Guid = v_Guid
            });

            var listResult = result.ToList();
            var totalCount = listResult.Count();

            return new PagedResultDto<InvPioProductionPlanMonthlyImportDto>(
                totalCount,
               listResult
               );
        }

        public async Task<FileDto> GetPioProductionPlanMonthlyListErrToExcel(string v_Guid)
        {
            string _sql = "Exec INV_PIO_PRODUCTION_PLAN_MONTHLY_GET_LIST_ERROR_IMPORT @Guid";

            IEnumerable<InvPioProductionPlanMonthlyImportDto> result = await _dapperRepo.QueryAsync<InvPioProductionPlanMonthlyImportDto>(_sql, new
            {
                Guid = v_Guid
            });

            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportToFileErr(exportToExcel);
        }

    }
}
