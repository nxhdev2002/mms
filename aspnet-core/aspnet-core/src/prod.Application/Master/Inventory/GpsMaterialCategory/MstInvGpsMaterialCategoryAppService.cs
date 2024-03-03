

using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using prod.Authorization;
using prod.Dto;
using prod.EntityFrameworkCore;
using prod.Master.Inventory.GpsMaterialCategory.Dto;
using prod.Master.Inventory.GpsMaterialCategory.Exporting;
using System.Linq;
using System.Threading.Tasks;

namespace prod.Master.Inventory.GpsMaterialCategory
{

        [AbpAuthorize(AppPermissions.Pages_Master_Gps_MaterialCategory_View)]
        public class MstInvGpsMaterialCategoryAppService : prodAppServiceBase, IMstInvGpsMaterialCategoryAppService
        {
            private readonly IDapperRepository<MstInvGpsMaterialCategory, long> _dapperRepo;
            private readonly IRepository<MstInvGpsMaterialCategory, long> _repo;
            private readonly IMstInvGpsMaterialCategoryExcelExporter _calendarListExcelExporter;

            public MstInvGpsMaterialCategoryAppService(IRepository<MstInvGpsMaterialCategory, long> repo,
                                             IDapperRepository<MstInvGpsMaterialCategory, long> dapperRepo,
                                            IMstInvGpsMaterialCategoryExcelExporter calendarListExcelExporter
                )
            {
                _repo = repo;
                _dapperRepo = dapperRepo;
                _calendarListExcelExporter = calendarListExcelExporter;
            }

            [AbpAuthorize(AppPermissions.Pages_Master_Gps_MaterialCategory_CreateEdit)]
            public async Task CreateOrEdit(CreateOrEditMstInvGpsMaterialCategoryDto input)
            {
                if (input.Id == null) await Create(input);
                else await Update(input);
            }

            //CREATE
            private async Task Create(CreateOrEditMstInvGpsMaterialCategoryDto input)
            {
                var mainObj = ObjectMapper.Map<MstInvGpsMaterialCategory>(input);

                await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
            }

            // EDIT
            private async Task Update(CreateOrEditMstInvGpsMaterialCategoryDto input)
            {
                using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
                {
                    var mainObj = await _repo.GetAll()
                    .FirstOrDefaultAsync(e => e.Id == input.Id);

                    var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
                }
            }

            [AbpAuthorize(AppPermissions.Pages_Master_Gps_MaterialCategory_CreateEdit)]
            public async Task Delete(EntityDto input)
            {
                var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
                _repo.HardDelete(mainObj);
                CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
            }

        public async Task<PagedResultDto<MstInvGpsMaterialCategoryDto>> GetAll(GetMstInvGpsMaterialCategoryInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.Name.Contains(input.Name));
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstInvGpsMaterialCategoryDto
                         {
                             Id = o.Id,
                             Name = o.Name,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstInvGpsMaterialCategoryDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }



        public async Task<FileDto> GetMaterialCategoryToExcel(GetMstInvGpsMaterialCategoryInput input)
        {
            var query = from o in _repo.GetAll().WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.Name.Contains(input.Name))
            select new MstInvGpsMaterialCategoryDto
                        {
                           Id = o.Id,
                           Name = o.Name,
                           IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

    }
}

