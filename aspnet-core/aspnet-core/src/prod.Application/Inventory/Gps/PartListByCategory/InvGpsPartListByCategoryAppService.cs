using Abp.Application.Services.Dto;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using prod.Dto;
using System.Linq;
using System.Threading.Tasks;
using prod.Inventory.GPS;
using prod.Inventory.Gps.PartListByCategory.Exporting;
using prod.Inventory.Gps.PartListByCategory.Dto;
using Microsoft.EntityFrameworkCore;
using Abp.Linq.Extensions;
using Abp.Authorization;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using prod.Authorization;
using prod.EntityFrameworkCore;
using prod.Master.Inventory.GpsMaterialCategory.Dto;
using prod.Master.Inventory;
using Abp.UI;
using FastMember;
using GemBox.Spreadsheet;
using prod.Common;
using prod.Master.Inventory.Dto;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System;
using prod.Inventory.Gps.PartListByCategory;

namespace prod.Inventory.Gps
{
    public class InvGpsPartListByCategoryAppService : prodAppServiceBase, IInvGpsPartListByCategoryAppService
    {
        private readonly IDapperRepository<InvGpsPartListByCategory, long> _dapperRepo;
        private readonly IRepository<InvGpsPartListByCategory, long> _repo;
        private readonly IInvGpsPartListByCategoryExcelExporter _gpsmasterListExcelExporter;
        private readonly IRepository<MstInvGpsMaterialCategory, long> _cbxCategory;
        public InvGpsPartListByCategoryAppService(IRepository<InvGpsPartListByCategory, long> repo,
                                         IDapperRepository<InvGpsPartListByCategory, long> dapperRepo,
                                         IRepository<MstInvGpsMaterialCategory, long> cbxCategory,
                                       IInvGpsPartListByCategoryExcelExporter gpsmasterListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _cbxCategory = cbxCategory;
            _gpsmasterListExcelExporter = gpsmasterListExcelExporter;
        }
        [AbpAuthorize(AppPermissions.Pages_Gps_Master_PartListByCategory_Edit)]
        public async Task CreateOrEdit(CreateOrEditInvGpsPartListByCategoryDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditInvGpsPartListByCategoryDto input)
        {
            var mainObj = ObjectMapper.Map<InvGpsPartListByCategory>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditInvGpsPartListByCategoryDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Gps_Master_PartListByCategory_Edit)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }


        public async Task<PagedResultDto<InvGpsPartListByCategoryDto>> GetAll(GetInvGpsPartListByCategoryInput input)
        {
            string _sql = "Exec INV_GPS_PARTLIST_BY_CATEGORY_SEARCH @p_Item, @p_Category, @p_Location, @p_PartType";

            IEnumerable<InvGpsPartListByCategoryDto> result = await _dapperRepo.QueryAsync<InvGpsPartListByCategoryDto>(_sql, new
            {
                p_Item = input.Item,
                p_Category = input.Category,
                p_Location = input.Location,
                p_PartType = input.PartType,
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvGpsPartListByCategoryDto>(totalCount, pagedAndFiltered);
        }

        public async Task<List<CbxCategory>> GetCbxCategory()
        {
            var filtered = _cbxCategory.GetAll();
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new CbxCategory
                         {
                             Id = o.Id,
                             Name = o.Name,
                             IsActive = o.IsActive,
                         };
            var result = await system.ToListAsync();

            return result;
        }

        public async Task<FileDto> GetInvGpsPartListByCategoryToExcel(GetInvGpsPartListByCategoryInput input)
        {
            string _sql = "Exec INV_GPS_PARTLIST_BY_CATEGORY_SEARCH @p_Item, @p_Category, @p_Location, @p_PartType";

            IEnumerable<InvGpsPartListByCategoryDto> result = await _dapperRepo.QueryAsync<InvGpsPartListByCategoryDto>(_sql, new
            {
                p_Item = input.Item,
                p_Category = input.Category,
                p_Location = input.Location,
                p_PartType = input.PartType,
            });

            var exportToExcel = result.ToList();
            return _gpsmasterListExcelExporter.ExportToFile(exportToExcel);
        }

        public async Task<List<InvGpsPartListByCategoryImportDto>> ImportInvGpsPartListByCategoryFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<InvGpsPartListByCategoryImportDto> listImport = new List<InvGpsPartListByCategoryImportDto>();
                using (var stream = new MemoryStream(fileBytes))
                {
                    SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
                    var xlWorkBook = ExcelFile.Load(stream);
                    string strGUID = Guid.NewGuid().ToString("N");

                    ExcelWorksheet v_worksheet_p1 = xlWorkBook.Worksheets[0];

                    int row = 1;
                    int col = 1;

                    while (true)
                    {
                        if (v_worksheet_p1.Cells[row, col].Value == null) { break; }
                        InvGpsPartListByCategoryImportDto mstGpsMaterialRegisterByShopImportDto = new InvGpsPartListByCategoryImportDto();
                        mstGpsMaterialRegisterByShopImportDto.Item = v_worksheet_p1.Cells[row, col].Value.ToString();
                        mstGpsMaterialRegisterByShopImportDto.Description = v_worksheet_p1.Cells[row, col + 1].Value.ToString();
                        mstGpsMaterialRegisterByShopImportDto.Uom = v_worksheet_p1.Cells[row, col + 2].Value.ToString();
                        mstGpsMaterialRegisterByShopImportDto.Category = v_worksheet_p1.Cells[row, col + 3].Value.ToString();
                        mstGpsMaterialRegisterByShopImportDto.Location = v_worksheet_p1.Cells[row, col + 4].Value.ToString();
                        mstGpsMaterialRegisterByShopImportDto.ExpenseAccount = v_worksheet_p1.Cells[row, col + 5].Value.ToString();
                        mstGpsMaterialRegisterByShopImportDto.Group = v_worksheet_p1.Cells[row, col + 6].Value.ToString();
                        mstGpsMaterialRegisterByShopImportDto.CurrentCategory = v_worksheet_p1.Cells[row, col + 7].Value.ToString();
                        mstGpsMaterialRegisterByShopImportDto.PartType = v_worksheet_p1.Cells[row, col + 8].Value.ToString();
                        mstGpsMaterialRegisterByShopImportDto.Guid = strGUID;
                        listImport.Add(mstGpsMaterialRegisterByShopImportDto);
                        row++;
                    }

                    // import temp into db (bulkCopy)
                    if (listImport.Count > 0)
                    {
                        IEnumerable<InvGpsPartListByCategoryImportDto> dataE = listImport.AsEnumerable();
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
                                    bulkCopy.DestinationTableName = "InvGpsPartListByCategory_T";
                                    bulkCopy.ColumnMappings.Add("Item", "Item");
                                    bulkCopy.ColumnMappings.Add("Description", "Description");
                                    bulkCopy.ColumnMappings.Add("Uom", "Uom");
                                    bulkCopy.ColumnMappings.Add("Category", "Category");
                                    bulkCopy.ColumnMappings.Add("Location", "Location");
                                    bulkCopy.ColumnMappings.Add("ExpenseAccount", "ExpenseAccount");
                                    bulkCopy.ColumnMappings.Add("Group", "Group");
                                    bulkCopy.ColumnMappings.Add("CurrentCategory", "CurrentCategory");
                                    bulkCopy.ColumnMappings.Add("PartType", "PartType");
                                    bulkCopy.ColumnMappings.Add("Guid", "Guid");
                                    bulkCopy.WriteToServer(table);
                                    tran.Commit();
                                }
                            }
                            await conn.CloseAsync();
                        }
                    }


                    /// merge vào bảng chính
                    var _sqlMerge = "Exec INV_GPS_PARTLIST_BY_CATEGORY_MERGE @p_guid";
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

