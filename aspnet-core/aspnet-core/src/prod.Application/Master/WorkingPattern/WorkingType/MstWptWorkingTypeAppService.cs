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
using prod.Master.WorkingPattern.Dto;
using prod.Master.WorkingPattern.Exporting;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Master.WorkingPattern
{
    [AbpAuthorize(AppPermissions.Pages_WorkingPattern_WorkingType_View)]
    public class MstWptWorkingTypeAppService : prodAppServiceBase, IMstWptWorkingTypeAppService
    {
        private readonly IDapperRepository<MstWptWorkingType, long> _dapperRepo;
        private readonly IRepository<MstWptWorkingType, long> _repo;
        private readonly IMstWptWorkingTypeExcelExporter _calendarListExcelExporter;

        public MstWptWorkingTypeAppService(IRepository<MstWptWorkingType, long> repo,
                                         IDapperRepository<MstWptWorkingType, long> dapperRepo,
                                        IMstWptWorkingTypeExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        [AbpAuthorize(AppPermissions.Pages_WorkingPattern_WorkingType_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstWptWorkingTypeDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstWptWorkingTypeDto input)
        {
            var mainObj = ObjectMapper.Map<MstWptWorkingType>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstWptWorkingTypeDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_WorkingPattern_WorkingType_Edit)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstWptWorkingTypeDto>> GetAll(GetMstWptWorkingTypeInput input)
        {
            var filtered = _repo.GetAll()
                 .WhereIf(!string.IsNullOrWhiteSpace(input.Description), e => e.Description.Contains(input.Description));

            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstWptWorkingTypeDto
                         {
                             Id = o.Id,
                             WorkingType = o.WorkingType,
                             Description = o.Description,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstWptWorkingTypeDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetWorkingTypeToExcel(MstWptWorkingTypeExportInput input)
        {
            var filtered = _repo.GetAll()
                 .WhereIf(!string.IsNullOrWhiteSpace(input.Description), e => e.Description.Contains(input.Description));

            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var query = from o in pageAndFiltered
                        select new MstWptWorkingTypeDto
                        {
                            Id = o.Id,
                            WorkingType = o.WorkingType,
                            Description = o.Description,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }


    }
}
