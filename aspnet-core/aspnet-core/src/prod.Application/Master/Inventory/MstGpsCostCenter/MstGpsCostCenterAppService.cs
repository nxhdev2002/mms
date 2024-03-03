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
using prod.Inventory.CKD.ShippingSchedule.Dto;
using prod.Master.Inventory.Dto;
using prod.Master.Inventory.Exporting;
using prod.Master.Inventory.GpsMaterialCategory.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NPOI.HSSF.Util.HSSFColor;

namespace prod.Master.Inventory
{
    [AbpAuthorize(AppPermissions.Pages_Master_Gps_CostCenter_View)]
    public class MstGpsCostCenterAppService : prodAppServiceBase, IMstGpsCostCenterAppService
    {
        private readonly IDapperRepository<MstGpsCostCenterStructure, long> _dapperRepo;
        private readonly IRepository<MstGpsCostCenterStructure, long> _repo;
        private readonly IMstGpsCostCenterExcelExporter _calendarListExcelExporter;

        public MstGpsCostCenterAppService(IRepository<MstGpsCostCenterStructure, long> repo,
                                         IDapperRepository<MstGpsCostCenterStructure, long> dapperRepo,
                                        IMstGpsCostCenterExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        [AbpAuthorize(AppPermissions.Pages_Master_Gps_CostCenter_CreateEdit)]
        public async Task CreateOrEdit(CreateOrEditMstGpsCostCenterDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstGpsCostCenterDto input)
        {
            var mainObj = ObjectMapper.Map<MstGpsCostCenterStructure>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstGpsCostCenterDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }


        [AbpAuthorize(AppPermissions.Pages_Master_Gps_CostCenter_CreateEdit)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstGpsCostCenterDto>> GetAll(GetMstGpsCostCenterInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Group), e => e.Group.Contains(input.Group))
                .WhereIf(!string.IsNullOrWhiteSpace(input.SubGroup), e => e.SubGroup.Contains(input.SubGroup))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Division), e => e.Division.Contains(input.Division))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Dept), e => e.Dept.Contains(input.Dept))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Shop), e => e.Shop.Contains(input.Shop))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstGpsCostCenterDto
                         {
                             Id = o.Id,
                             Group = o.Group,
                             SubGroup = o.SubGroup,
                             Division = o.Division,
                             Dept = o.Dept,
                             Shop = o.Shop,
                             Team = o.Team,
							 CostCenterCategory = o.CostCenterCategory,
							 BudgetOwner = o.BudgetOwner,
                             CostCenter = o.CostCenter,
                             CostCenterCurrent = o.CostCenterCurrent,
							 CostCenterGroup = o.CostCenterGroup,
							 IsDirectCostCenter = o.IsDirectCostCenter,
                         }; 
        var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstGpsCostCenterDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }
        public async Task<FileDto> GetMstGpsCostCenterToExcel(GetMstGpsCostCenterExcelInput input)
        {
            var query = from o in _repo.GetAll()

                .WhereIf(!string.IsNullOrWhiteSpace(input.Group), e => e.Group.Contains(input.Group))
                .WhereIf(!string.IsNullOrWhiteSpace(input.SubGroup), e => e.SubGroup.Contains(input.SubGroup))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Division), e => e.Division.Contains(input.Division))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Dept), e => e.Dept.Contains(input.Dept))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Shop), e => e.Shop.Contains(input.Shop))

            select new MstGpsCostCenterDto
            {
                Id = o.Id,
                Group = o.Group,
                SubGroup = o.SubGroup,
                Division = o.Division,
                Dept = o.Dept,
                Shop = o.Shop,
                Team = o.Team,
                CostCenterCategory = o.CostCenterCategory,
                GroupSubGroup = o.GroupSubGroup,
                DivDept = o.DivDept,
                Process = o.Process,
                BudgetOwner = o.BudgetOwner,
                CostCenter = o.CostCenter,
                CostCenterCurrent = o.CostCenterCurrent,
                CostCenterGroup = o.CostCenterGroup,
				IsDirectCostCenter = o.IsDirectCostCenter,
			};
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        public async Task<List<MstGpsCostCenterImportDto>> ImportGpsCostCenterFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<MstGpsCostCenterImportDto> listImport = new List<MstGpsCostCenterImportDto>();
                using (var stream = new MemoryStream(fileBytes))
                {
                    SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
                    var xlWorkBook = ExcelFile.Load(stream);
                    string strGUID = Guid.NewGuid().ToString("N");

                    ExcelWorksheet v_worksheet_p1 = xlWorkBook.Worksheets[0];

                    int row, col;

                    row = 4;
                    col = 0;


                    String group = null; 
                    String subGroup = null;
                    String division = null;

                    while (true)
                    {

                        // check tất cả các hàng trong 2 dòng == null -> break;
                        bool isDone = true;
                        for (int i = 0; i<17; i++)
                        {
                            if (v_worksheet_p1.Cells[row, i].Value != null || v_worksheet_p1.Cells[row + 1, i].Value != null)
                            {
                                isDone = false;
                            }
                        }
                        if (isDone) { 
                            break; 
                        }
                        
                        //

                        MstGpsCostCenterImportDto mstGpsCostCenterImport = new MstGpsCostCenterImportDto();


                        if (v_worksheet_p1.Cells[row, col].Value != null)
                        {
                            group = v_worksheet_p1.Cells[row, col].Value.ToString();
                        }

                        if (v_worksheet_p1.Cells[row, col + 1].Value != null)
                        {
                            subGroup = v_worksheet_p1.Cells[row, col + 1].Value.ToString();
                        }

                        if (v_worksheet_p1.Cells[row, col + 2].Value != null)
                        {
                            division = v_worksheet_p1.Cells[row, col + 2].Value.ToString();
                        }

                        mstGpsCostCenterImport.Group = group;
                        mstGpsCostCenterImport.SubGroup = subGroup;
                        mstGpsCostCenterImport.Division = division;


                        mstGpsCostCenterImport.Dept = v_worksheet_p1.Cells[row, col + 3].Value == null ? "" : v_worksheet_p1.Cells[row, col + 3].Value.ToString();
                        mstGpsCostCenterImport.Shop = v_worksheet_p1.Cells[row, col + 4].Value == null ? "" : v_worksheet_p1.Cells[row, col + 4].Value.ToString();
                        mstGpsCostCenterImport.Team = v_worksheet_p1.Cells[row, col + 5].Value == null ? "" : v_worksheet_p1.Cells[row, col + 5].Value.ToString();
                        mstGpsCostCenterImport.CostCenterCategory = v_worksheet_p1.Cells[row, col + 6].Value == null ? "" : v_worksheet_p1.Cells[row, col + 6].Value.ToString();
                        mstGpsCostCenterImport.GroupSubGroup = v_worksheet_p1.Cells[row, col + 7].Value == null ? "" : v_worksheet_p1.Cells[row, col + 7].Value.ToString();
                        mstGpsCostCenterImport.DivDept = v_worksheet_p1.Cells[row, col + 9].Value == null ? "" : v_worksheet_p1.Cells[row, col + 9].Value.ToString();
                        mstGpsCostCenterImport.Process = v_worksheet_p1.Cells[row, col + 10].Value == null ? "" : v_worksheet_p1.Cells[row, col + 10].Value.ToString();
                        mstGpsCostCenterImport.BudgetOwner = v_worksheet_p1.Cells[row, col + 11].Value == null ? "" : v_worksheet_p1.Cells[row, col + 11].Value.ToString();
                        mstGpsCostCenterImport.CostCenterGroup = v_worksheet_p1.Cells[row, col + 12].Value == null ? "" : v_worksheet_p1.Cells[row, col + 12].Value.ToString();
                        mstGpsCostCenterImport.CostCenterCurrent = v_worksheet_p1.Cells[row, col + 14].Value == null ? "" : v_worksheet_p1.Cells[row, col + 14].Value.ToString();
                        mstGpsCostCenterImport.CostCenter = v_worksheet_p1.Cells[row, col + 15].Value == null ? "" : v_worksheet_p1.Cells[row, col + 15].Value.ToString();
                        mstGpsCostCenterImport.Guid = strGUID;

                        listImport.Add(mstGpsCostCenterImport);

                        row++;
                    }


                    // import temp into db (bulkCopy)
                    if (listImport.Count > 0)
                    {
                        IEnumerable<MstGpsCostCenterImportDto> dataE = listImport.AsEnumerable();
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
                                    bulkCopy.DestinationTableName = "MstGpsCostCenterStructure_T";
                                    bulkCopy.ColumnMappings.Add("Group", "Group");
                                    bulkCopy.ColumnMappings.Add("SubGroup", "SubGroup");
                                    bulkCopy.ColumnMappings.Add("Division", "Division");
                                    bulkCopy.ColumnMappings.Add("Dept", "Dept");
                                    bulkCopy.ColumnMappings.Add("Shop", "Shop");
                                    bulkCopy.ColumnMappings.Add("Team", "Team");
                                    bulkCopy.ColumnMappings.Add("CostCenterCategory", "CostCenterCategory");
                                    bulkCopy.ColumnMappings.Add("GroupSubGroup", "GroupSubGroup");
                                    bulkCopy.ColumnMappings.Add("DivDept", "DivDept");
                                    bulkCopy.ColumnMappings.Add("Process", "Process");
                                    bulkCopy.ColumnMappings.Add("BudgetOwner", "BudgetOwner");
                                    bulkCopy.ColumnMappings.Add("CostCenterGroup", "CostCenterGroup");
                                    bulkCopy.ColumnMappings.Add("CostCenterCurrent", "CostCenterCurrent");
                                    bulkCopy.ColumnMappings.Add("CostCenter", "CostCenter");
                                    bulkCopy.ColumnMappings.Add("Guid", "Guid");
                                    bulkCopy.WriteToServer(table);
                                    tran.Commit();
                                }
                            }
                            await conn.CloseAsync();
                        }
                    }

                    /// merge vào bảng chính
                    var _sqlMerge = "Exec MST_GPS_COST_CENTER_MERGE @p_guid";
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
