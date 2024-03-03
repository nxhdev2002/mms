using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.Linq.Extensions;
using Abp.UI;
using FastMember;
using GemBox.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using prod.Authorization;
using prod.Common;
using prod.Dto;
using prod.EntityFrameworkCore;
using prod.Master.CKD.Dto;
using prod.Master.CKD.Exporting;
using prod.Master.Inventory.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Master.Inventory
{
    [AbpAuthorize(AppPermissions.Pages_Master_CKD_CustomsLeadTimeMaster_View)]
    public class MstInvCustomsLeadTimeAppService : prodAppServiceBase, IMstInvCustomsLeadTimeAppService
    {
        private readonly IDapperRepository<MstInvCustomsLeadTimeMaster, long> _dapperRepo;
        private readonly IRepository<MstInvCustomsLeadTimeMaster, long> _repo;
        private readonly IMstInvCustomsLeadTimeExcelExporter _calendarListExcelExporter;

        public MstInvCustomsLeadTimeAppService(IRepository<MstInvCustomsLeadTimeMaster, long> repo,
                                         IDapperRepository<MstInvCustomsLeadTimeMaster, long> dapperRepo,
                                        IMstInvCustomsLeadTimeExcelExporter calenderListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calenderListExcelExporter;
        }

        [AbpAuthorize(AppPermissions.Pages_Master_CKD_CustomsLeadTimeMaster_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstInvCustomsLeadTimeDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstInvCustomsLeadTimeDto input)
        {
            var mainObj = ObjectMapper.Map<MstInvCustomsLeadTimeMaster>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstInvCustomsLeadTimeDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstInvCustomsLeadTimeDto>> GetAll(GetMstInvCustomsLeadTimeInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Exporter), e => e.Exporter.Contains(input.Exporter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Carrier), e => e.Carrier.Contains(input.Carrier))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstInvCustomsLeadTimeDto
                         {
                             Id = o.Id,
                             Exporter = o.Exporter,
                             Carrier = o.Carrier,
                             Leadtime = o.Leadtime,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstInvCustomsLeadTimeDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetCustomsLeadTimeToExcel(GetMstInvCustomsLeadTimeInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Exporter), e => e.Exporter.Contains(input.Exporter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Carrier), e => e.Carrier.Contains(input.Carrier))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);
            var query = from o in pageAndFiltered
                        select new MstInvCustomsLeadTimeDto
            {
                Id = o.Id,
                Exporter = o.Exporter,
                Carrier = o.Carrier,
                Leadtime = o.Leadtime,
            };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }
        public async Task<List<MstInvCustomsLeadTimeImportDto>> ImportCustomsLeadTimeMasterFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<MstInvCustomsLeadTimeImportDto> listImport = new List<MstInvCustomsLeadTimeImportDto>();
                using (var stream = new MemoryStream(fileBytes))
                {
                    SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
                    var xlWorkBook = ExcelFile.Load(stream);
                    string strGUID = Guid.NewGuid().ToString("N");

                    ExcelWorksheet v_worksheet_p1 = xlWorkBook.Worksheets[0];

                    int row = 2;
                    int col = 0;

                    while (true)
                    {
                        if (v_worksheet_p1.Cells[row, col].Value == null) { break; }
                        MstInvCustomsLeadTimeImportDto mstInvCustomsLeadTimeImportDto = new MstInvCustomsLeadTimeImportDto();
                        mstInvCustomsLeadTimeImportDto.Exporter = v_worksheet_p1.Cells[row, col].Value != null ? v_worksheet_p1.Cells[row, col].Value.ToString() : null;
                        mstInvCustomsLeadTimeImportDto.Carrier = v_worksheet_p1.Cells[row, col + 1].Value != null ? v_worksheet_p1.Cells[row, col + 1].Value.ToString() : null;
                        mstInvCustomsLeadTimeImportDto.Leadtime = Convert.ToInt32(v_worksheet_p1.Cells[row, col + 2].Value != null ? v_worksheet_p1.Cells[row, col + 2].Value.ToString() : null);
                        if (mstInvCustomsLeadTimeImportDto.Leadtime < 0) throw new UserFriendlyException(400, "Leadtime không được < 0"); 
                        mstInvCustomsLeadTimeImportDto.Guid = strGUID;
                        listImport.Add(mstInvCustomsLeadTimeImportDto);
                        row++;
                    }

                    // import temp into db (bulkCopy)
                    if (listImport.Count > 0)
                    {
                        IEnumerable<MstInvCustomsLeadTimeImportDto> dataE = listImport.AsEnumerable();
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
                                    bulkCopy.DestinationTableName = "MstInvCustomsLeadTimeMaster_T";
                                    bulkCopy.ColumnMappings.Add("Exporter", "Exporter");
                                    bulkCopy.ColumnMappings.Add("Carrier", "Carrier");
                                    bulkCopy.ColumnMappings.Add("Leadtime", "Leadtime");

                                    bulkCopy.ColumnMappings.Add("Guid", "Guid");
                                    bulkCopy.WriteToServer(table);
                                    tran.Commit();
                                }
                            }
                            await conn.CloseAsync();
                        }
                    }
                    /// merge vào bảng chính
                    var _sqlMerge = "Exec [MST_INV_MATERIAL_CUSTOMS_LEAD_TIME_MASTER_MERGE] @p_guid";
                    _dapperRepo.Execute(_sqlMerge, new
                    {
                        p_guid = strGUID,
                    });


                    return listImport;
                }

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(400, ex.Message);
            }
        }

    }
}
