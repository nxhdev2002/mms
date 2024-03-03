using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Dapper;
using Microsoft.EntityFrameworkCore;
using NUglify.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using prod.Authorization;
using prod.Dto;
using prod.EntityFrameworkCore;
using prod.Inventory.GPS.Dto;
using prod.Inventory.GPS.Exporting;
using FastMember;
using GemBox.Spreadsheet;
using prod.Inventory.CKD.Dto;
using System.Data;
using System.IO;
using prod.Common;

namespace prod.Inventory.GPS
{
    //  [AbpAuthorize(AppPermissions.Pages_Gps_Master_StockConcept_View)]
    public class InvGpsStockConceptAppService : prodAppServiceBase, IInvGpsStockConceptAppService
    {
        private readonly IDapperRepository<InvGpsStockConcept, long> _dapperRepo;
        private readonly IRepository<InvGpsStockConcept, long> _repo;
        private readonly IInvGpsStockConceptExcelExporter _calendarListExcelExporter;

        public InvGpsStockConceptAppService(IRepository<InvGpsStockConcept, long> repo,
                                         IDapperRepository<InvGpsStockConcept, long> dapperRepo,
                                        IInvGpsStockConceptExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

      //  [AbpAuthorize(AppPermissions.Pages_Gps_Master_StockConcept_Edit)]
        public async Task CreateOrEdit(CreateOrEditInvGpsStockConceptDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditInvGpsStockConceptDto input)
        {
            var mainObj = ObjectMapper.Map<InvGpsStockConcept>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditInvGpsStockConceptDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

       //   [AbpAuthorize(AppPermissions.Pages_Gps_Master_StockConcept_Edit)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<InvGpsStockConceptDto>> GetAll(GetInvGpsStockConceptInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.SupplierCode), e => e.SupplierCode.Contains(input.SupplierCode))
                .WhereIf(!string.IsNullOrWhiteSpace(input.StkConcept), e => e.StkConcept.Contains(input.StkConcept));
                
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new InvGpsStockConceptDto
                         {
                             Id = o.Id,
                             SupplierCode = o.SupplierCode,
                             MonthStk = o.MonthStk,
                             MinStk1 = o.MinStk1,
                             MinStk2 = o.MinStk2,
                             MinStk3 = o.MinStk3,
                             MinStk4 = o.MinStk4,
                             MinStk5 = o.MinStk5,
                             MinStk6 = o.MinStk6,
                             MinStk7 = o.MinStk7,
                             MinStk8 = o.MinStk8,
                             MinStk9 = o.MinStk9,
                             MinStk10 = o.MinStk10,
                             MinStk11 = o.MinStk11,
                             MinStk12 = o.MinStk12,
                             MinStk13 = o.MinStk13,
                             MinStk14 = o.MinStk14,
                             MinStk15 = o.MinStk15,
                             MaxStk1 = o.MaxStk1,
                             MaxStk2 = o.MaxStk2,
                             MaxStk3 = o.MaxStk3,
                             MaxStk4 = o.MaxStk4,
                             MaxStk5 = o.MaxStk5,
                             MinStkConcept = o.MinStkConcept,
                             MaxStkConcept = o.MaxStkConcept,
                             StkConcept = o.StkConcept,
                             StkConceptFrq = o.StkConceptFrq,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<InvGpsStockConceptDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetInvGpsStockConceptToExcel(InvGpsStockConceptExportInput input)
        {
            var filtered = _repo.GetAll()
               .WhereIf(!string.IsNullOrWhiteSpace(input.SupplierCode), e => e.SupplierCode.Contains(input.SupplierCode))
               .WhereIf(!string.IsNullOrWhiteSpace(input.StkConcept), e => e.StkConcept.Contains(input.StkConcept));

            var pageAndFiltered = filtered.OrderBy(s => s.Id);

            var query = from o in pageAndFiltered
                        select new InvGpsStockConceptDto
                        {
                            Id = o.Id,
                            SupplierCode = o.SupplierCode,
                            MonthStk = o.MonthStk,
                          //  StkType = o.StkType,
                      //      Frequency = o.Frequency,
                       //     StkFrequency = o.StkFrequency,
                            MinStk1 = o.MinStk1,
                            MinStk2 = o.MinStk2,
                            MinStk3 = o.MinStk3,
                            MinStk4 = o.MinStk4,
                            MinStk5 = o.MinStk5,
                            MinStk6 = o.MinStk6,
                            MinStk7 = o.MinStk7,
                            MinStk8 = o.MinStk8,
                            MinStk9 = o.MinStk9,
                            MinStk10 = o.MinStk10,
                            MinStk11 = o.MinStk11,
                            MinStk12 = o.MinStk12,
                            MinStk13 = o.MinStk13,
                            MinStk14 = o.MinStk14,
                            MinStk15 = o.MinStk15,
                            MaxStk1 = o.MaxStk1,
                            MaxStk2 = o.MaxStk2,
                            MaxStk3 = o.MaxStk3,
                            MaxStk4 = o.MaxStk4,
                            MaxStk5 = o.MaxStk5,
                            MinStkConcept = o.MinStkConcept,
                            MaxStkConcept = o.MaxStkConcept,
                            StkConcept = o.StkConcept,
                            StkConceptFrq = o.StkConceptFrq,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

      //  [AbpAuthorize(AppPermissions.Pages_Gps_Master_StockConcept_Import)]
        public async Task<List<InvGpsStockConceptDto>> ImportData_InvGpsStockConcept_FromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<InvGpsStockConceptDto> listImport = new List<InvGpsStockConceptDto>();
                using (var stream = new MemoryStream(fileBytes))
                {
                    SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
                    var xlWorkBook = ExcelFile.Load(stream);
                    var v_worksheet = xlWorkBook.Worksheets[0];

                    //string v_devanning_date = (v_worksheet.Cells[4, 2]).Value?.ToString() ?? "";
                    string strGUID = Guid.NewGuid().ToString("N");

                    string _sql = @"EXEC CMM_IMPORT_USER_GUID_INSERT @Guid, @p_UserId";
                    _dapperRepo.ExecuteAsync(_sql, new
                    {
                        Guid = strGUID,
                        p_UserId = AbpSession.UserId
                    });

                    for (int i = 1; i < v_worksheet.Rows.Count; i++)
                    {
                        var row = new InvGpsStockConceptDto();


                        //reader data
                        row.Guid = strGUID;

                        string v_supplier_code = (v_worksheet.Cells[i, 1]).Value?.ToString() ?? "";

                        if (v_supplier_code == "") break;

                        string v_stk_month = (v_worksheet.Cells[i, 2]).Value?.ToString() ?? "";
                        string v_stk_type = (v_worksheet.Cells[i, 3]).Value?.ToString() ?? "";
                        int v_frequency = Int32.Parse((v_worksheet.Cells[i, 4]).Value?.ToString() ?? "0");
                        decimal v_stk_frequency = decimal.Parse((v_worksheet.Cells[i, 5]).Value?.ToString() ?? "0");
                        decimal v_min_stk1 = decimal.Parse((v_worksheet.Cells[i, 6]).Value?.ToString() ?? "0");
                        decimal v_min_stk2 = decimal.Parse((v_worksheet.Cells[i, 7]).Value?.ToString() ?? "0");
                        decimal v_min_stk3 = decimal.Parse((v_worksheet.Cells[i, 8]).Value?.ToString() ?? "0");
                        decimal v_min_stk4 = decimal.Parse((v_worksheet.Cells[i, 9]).Value?.ToString() ?? "0");
                        decimal v_min_stk5 = decimal.Parse((v_worksheet.Cells[i, 10]).Value?.ToString() ?? "0");
                        decimal v_min_stk6 = decimal.Parse((v_worksheet.Cells[i, 11]).Value?.ToString() ?? "0");
                        decimal v_min_stk7 = decimal.Parse((v_worksheet.Cells[i, 12]).Value?.ToString() ?? "0");
                        decimal v_min_stk8 = decimal.Parse((v_worksheet.Cells[i, 13]).Value?.ToString() ?? "0");
                        decimal v_min_stk9 = decimal.Parse((v_worksheet.Cells[i, 14]).Value?.ToString() ?? "0");
                        decimal v_min_stk10 = decimal.Parse((v_worksheet.Cells[i, 15]).Value?.ToString() ?? "0");
                        decimal v_min_stk11 = decimal.Parse((v_worksheet.Cells[i, 16]).Value?.ToString() ?? "0");
                        decimal v_min_stk12 = decimal.Parse((v_worksheet.Cells[i, 17]).Value?.ToString() ?? "0");
                        decimal v_min_stk13 = decimal.Parse((v_worksheet.Cells[i, 18]).Value?.ToString() ?? "0");
                        decimal v_min_stk14 = decimal.Parse((v_worksheet.Cells[i, 19]).Value?.ToString() ?? "0");
                        decimal v_min_stk15 = decimal.Parse((v_worksheet.Cells[i, 20]).Value?.ToString() ?? "0");
                        decimal v_max_stk1 = decimal.Parse((v_worksheet.Cells[i, 21]).Value?.ToString() ?? "0");
                        decimal v_max_stk2 = decimal.Parse((v_worksheet.Cells[i, 22]).Value?.ToString() ?? "0");
                        decimal v_max_stk3 = decimal.Parse((v_worksheet.Cells[i, 23]).Value?.ToString() ?? "0");
                        decimal v_max_stk4 = decimal.Parse((v_worksheet.Cells[i, 24]).Value?.ToString() ?? "0");
                        decimal v_max_stk5 = decimal.Parse((v_worksheet.Cells[i, 25]).Value?.ToString() ?? "0");
                        decimal v_min_stk_concept = decimal.Parse((v_worksheet.Cells[i, 26]).Value?.ToString() ?? "0");
                        decimal v_max_stk_concept = decimal.Parse((v_worksheet.Cells[i, 27]).Value?.ToString() ?? "0");


                        //add data in row

                        row.SupplierCode = v_supplier_code;
                        row.StkType = v_stk_type;
                        row.Frequency = v_frequency;
                        row.StkFrequency = v_stk_frequency;
                        row.MinStkConcept = v_min_stk_concept;
                        row.MaxStkConcept = v_max_stk_concept;

                        DateTime.Parse(v_stk_month);
                        row.MonthStk = DateTime.Parse(v_stk_month);

                        row.MinStk1 = v_min_stk1;
                        row.MinStk2 = v_min_stk2;
                        row.MinStk3 = v_min_stk3;
                        row.MinStk4 = v_min_stk4;
                        row.MinStk5 = v_min_stk5;
                        row.MinStk6 = v_min_stk6;
                        row.MinStk7 = v_min_stk7;
                        row.MinStk8 = v_min_stk8;
                        row.MinStk9 = v_min_stk9;
                        row.MinStk10 = v_min_stk10;
                        row.MinStk11 = v_min_stk11;
                        row.MinStk12 = v_min_stk12;
                        row.MinStk13 = v_min_stk13;
                        row.MinStk14 = v_min_stk14;
                        row.MinStk15 = v_min_stk15;
                        row.MaxStk1 = v_max_stk1;
                        row.MaxStk2 = v_max_stk2;
                        row.MaxStk3 = v_max_stk3;
                        row.MaxStk4 = v_max_stk4;
                        row.MaxStk5 = v_max_stk5;


                        listImport.Add(row);
                    }
                    // import temp into db (bulkCopy)
                    if (listImport.Count > 0)
                    {
                        IEnumerable<InvGpsStockConceptDto> dataE = listImport.AsEnumerable();
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
                                    bulkCopy.DestinationTableName = "InvGpsStockConcept_T";
                                    bulkCopy.ColumnMappings.Add("Guid", "Guid");
                                    bulkCopy.ColumnMappings.Add("SupplierCode", "SupplierCode");
                                    bulkCopy.ColumnMappings.Add("MonthStk", "MonthStk");
                                    bulkCopy.ColumnMappings.Add("MinStk1", "MinStk1");
                                    bulkCopy.ColumnMappings.Add("MinStk2", "MinStk2");
                                    bulkCopy.ColumnMappings.Add("MinStk3", "MinStk3");
                                    bulkCopy.ColumnMappings.Add("MinStk4", "MinStk4");
                                    bulkCopy.ColumnMappings.Add("MinStk5", "MinStk5");
                                    bulkCopy.ColumnMappings.Add("MinStk6", "MinStk6");
                                    bulkCopy.ColumnMappings.Add("MinStk7", "MinStk7");
                                    bulkCopy.ColumnMappings.Add("MinStk8", "MinStk8");
                                    bulkCopy.ColumnMappings.Add("MinStk9", "MinStk9");
                                    bulkCopy.ColumnMappings.Add("MinStk10", "MinStk10");
                                    bulkCopy.ColumnMappings.Add("MinStk11", "MinStk11");
                                    bulkCopy.ColumnMappings.Add("MinStk12", "MinStk12");
                                    bulkCopy.ColumnMappings.Add("MinStk13", "MinStk13");
                                    bulkCopy.ColumnMappings.Add("MinStk14", "MinStk14");
                                    bulkCopy.ColumnMappings.Add("MinStk15", "MinStk15");
                                    bulkCopy.ColumnMappings.Add("MaxStk1", "MaxStk1");
                                    bulkCopy.ColumnMappings.Add("MaxStk2", "MaxStk2");
                                    bulkCopy.ColumnMappings.Add("MaxStk3", "MaxStk3");
                                    bulkCopy.ColumnMappings.Add("MaxStk4", "MaxStk4");
                                    bulkCopy.ColumnMappings.Add("MaxStk5", "MaxStk5");
                                    bulkCopy.ColumnMappings.Add("MinStkConcept", "MinStkConcept");
                                    bulkCopy.ColumnMappings.Add("MaxStkConcept", "MaxStkConcept");
                                    bulkCopy.ColumnMappings.Add("StkConcept", "StkConcept");
                                    bulkCopy.ColumnMappings.Add("StkConceptFrq", "StkConceptFrq");
                                 //   bulkCopy.ColumnMappings.Add("RowNumber", "RowNumber");
                                //    bulkCopy.ColumnMappings.Add("ErrorDescription", "ErrorDescription");
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
        public async Task MergeDataInvGpsStockConcept(string v_Guid)
        {
           string _sql = "Exec INV_GPS_STOCK_CONCEPT_MERGE @Guid";
           await _dapperRepo.QueryAsync<InvCkdContainerRentalWHPlanDto>(_sql, new { Guid = v_Guid });
        }
    }
}
