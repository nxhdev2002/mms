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
using NPOI.OpenXmlFormats.Dml.Diagram;
using prod.Authorization;
using prod.Common;
using prod.Dto;
using prod.EntityFrameworkCore;
using prod.Inventory.CKD.ShippingSchedule.Dto;
using prod.Master.CKD;
using prod.Master.CKD.Dto;
using prod.Master.CKD.Exporting;
using prod.Master.Inventory;
using prod.Master.Inventory.Dto;
using prod.Master.Inventory.Exporting;
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
    [AbpAuthorize(AppPermissions.Pages_Master_Gps_GpsMaterialRegisterByShop_View)]
    public class MstGpsMaterialRegisterByShopAppService : prodAppServiceBase, IMstGpsMaterialRegisterByShopAppService
    {
        private readonly IDapperRepository<MstGpsMaterialRegisterByShop, long> _dapperRepo;
        private readonly IRepository<MstGpsMaterialRegisterByShop, long> _repo;
        private readonly IMstGpsMaterialRegisterByShopExcelExporter _materialRegisterByShopListExcelExporter;
        private readonly IRepository<MstInvGpsMaterialCategory, long> _cbxCategory;

        public MstGpsMaterialRegisterByShopAppService(IRepository<MstGpsMaterialRegisterByShop, long> repo,
                                         IDapperRepository<MstGpsMaterialRegisterByShop, long> dapperRepo,
                                         IRepository<MstInvGpsMaterialCategory, long> cbxCategory,
                                         IMstGpsMaterialRegisterByShopExcelExporter materialRegisterByShopListExcelExporter
            )
        {
            _repo = repo;
            _cbxCategory = cbxCategory;
            _dapperRepo = dapperRepo;
            _materialRegisterByShopListExcelExporter = materialRegisterByShopListExcelExporter;
        }


        [AbpAuthorize(AppPermissions.Pages_Master_Gps_GpsMaterialRegisterByShop_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstGpsMaterialRegisterByShopDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstGpsMaterialRegisterByShopDto input)
        {
            var r = _repo.FirstOrDefault(x => x.PartNo == input.PartNo && x.CostCenter == input.CostCenter && x.Category == input.Category);
            if (r == null) {
                var mainObj = ObjectMapper.Map<MstGpsMaterialRegisterByShop>(input);
                await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
            } else
            {
                input.Id = r.Id;
                await Update(input);
            }
        }

        // EDIT
        private async Task Update(CreateOrEditMstGpsMaterialRegisterByShopDto input)
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

        public async Task<PagedResultDto<MstGpsMaterialRegisterByShopDto>> GetAll(GetMstGpsMaterialRegisterByShopInput input)
        {
            string _sql = "EXEC MST_GPS_MATERIAL_REGISTER_BY_SHOP_SEARCH @p_partno, @p_uom,@p_category,@p_expenseAccount,@p_shop,@p_costCenter";
            var result = await _dapperRepo.QueryAsync<MstGpsMaterialRegisterByShopDto>(_sql, new
            {
                p_partno = input.PartNo,
                p_uom = input.Uom,
                p_category = input.Category,
                p_expenseAccount = input.ExpenseAccount,
                p_shop = input.Shop,
                p_costCenter = input.CostCenter,
            });
            var listResult= result.ToList();
            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            var totalCount = result.ToList().Count();

            return new PagedResultDto<MstGpsMaterialRegisterByShopDto>(
                totalCount,
                pagedAndFiltered
            );
        }

        public async Task<FileDto> GetMaterialRegisterByShopToExcel(GetMstGpsMaterialRegisterByShopExcelInput input)
        {
            string _sql = "EXEC MST_GPS_MATERIAL_REGISTER_BY_SHOP_SEARCH @p_partno, @p_uom,@p_category,@p_expenseAccount,@p_shop,@p_costCenter";
            var result = await _dapperRepo.QueryAsync<MstGpsMaterialRegisterByShopDto>(_sql, new
            {
                p_partno = input.PartNo,
                p_uom = input.Uom,
                p_category = input.Category,
                p_expenseAccount = input.ExpenseAccount,
                p_shop = input.Shop,
                p_costCenter = input.CostCenter,
            });
            var listResult = result.ToList();
            return _materialRegisterByShopListExcelExporter.ExportToFile(listResult);
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

        public async Task<List<MstGpsMaterialRegisterByShopImportDto>> ImportMaterialRegisterByShopFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<MstGpsMaterialRegisterByShopImportDto> listImport = new List<MstGpsMaterialRegisterByShopImportDto>();
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
                        MstGpsMaterialRegisterByShopImportDto mstGpsMaterialRegisterByShopImportDto = new MstGpsMaterialRegisterByShopImportDto();
                        mstGpsMaterialRegisterByShopImportDto.PartNo = v_worksheet_p1.Cells[row, col].Value.ToString();
                        mstGpsMaterialRegisterByShopImportDto.Description = v_worksheet_p1.Cells[row, col + 1].Value.ToString();
                        mstGpsMaterialRegisterByShopImportDto.Uom = v_worksheet_p1.Cells[row, col + 2].Value.ToString();
                        mstGpsMaterialRegisterByShopImportDto.Category = v_worksheet_p1.Cells[row, col + 3].Value.ToString();
                        mstGpsMaterialRegisterByShopImportDto.ExpenseAccount = v_worksheet_p1.Cells[row, col + 4].Value.ToString();
                        mstGpsMaterialRegisterByShopImportDto.Shop = v_worksheet_p1.Cells[row, col + 5].Value.ToString();
                        mstGpsMaterialRegisterByShopImportDto.CostCenter = v_worksheet_p1.Cells[row, col + 6].Value.ToString();
                        mstGpsMaterialRegisterByShopImportDto.Guid = strGUID;
                        listImport.Add(mstGpsMaterialRegisterByShopImportDto);
                        row++;
                    }

                    // import temp into db (bulkCopy)
                    if (listImport.Count > 0)
                    {
                        IEnumerable<MstGpsMaterialRegisterByShopImportDto> dataE = listImport.AsEnumerable();
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
                                    bulkCopy.DestinationTableName = "MstGpsMaterialRegisterByShop_T";
                                    bulkCopy.ColumnMappings.Add("PartNo", "PartNo");
                                    bulkCopy.ColumnMappings.Add("Description", "Description");
                                    bulkCopy.ColumnMappings.Add("Uom", "Uom");
                                    bulkCopy.ColumnMappings.Add("Category", "Category");
                                    bulkCopy.ColumnMappings.Add("ExpenseAccount", "ExpenseAccount");
                                    bulkCopy.ColumnMappings.Add("Shop", "Shop");
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
                    var _sqlMerge = "Exec MST_GPS_MATERIAL_REGISTER_BY_SHOP_MERGE @p_guid";
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
