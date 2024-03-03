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
using prod.Master.Welding.Dto;
using prod.Master.Welding.Exporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Master.Welding
{
    //  [AbpAuthorize(AppPermissions.Pages_Master_Welding_PunchIndicator)]
    public class MstWldPunchIndicatorAppService : prodAppServiceBase, IMstWldPunchIndicatorAppService
    {
        private readonly IDapperRepository<MstWldPunchIndicator, long> _dapperRepo;
        private readonly IRepository<MstWldPunchIndicator, long> _repo;
        private readonly IMstWldPunchIndicatorExcelExporter _calendarListExcelExporter;

        public MstWldPunchIndicatorAppService(IRepository<MstWldPunchIndicator, long> repo,
                                         IDapperRepository<MstWldPunchIndicator, long> dapperRepo,
                                        IMstWldPunchIndicatorExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_Welding_PunchIndicator_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstWldPunchIndicatorDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstWldPunchIndicatorDto input)
        {
            var mainObj = ObjectMapper.Map<MstWldPunchIndicator>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstWldPunchIndicatorDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_Welding_PunchIndicator_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstWldPunchIndicatorDto>> GetAll(GetMstWldPunchIndicatorInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Grade), e => e.Grade.Contains(input.Grade))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Indicator), e => e.Indicator.Contains(input.Indicator))
                .WhereIf(!string.IsNullOrWhiteSpace(input.IsActive), e => e.IsActive.Contains(input.IsActive))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstWldPunchIndicatorDto
                         {
                             Id = o.Id,
                             Grade = o.Grade,
                             Indicator = o.Indicator,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstWldPunchIndicatorDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetPunchIndicatorToExcel(MstWldPunchIndicatorExportInput input)
        {
            var filtered = _repo.GetAll()
               .WhereIf(!string.IsNullOrWhiteSpace(input.Grade), e => e.Grade.Contains(input.Grade))
               .WhereIf(!string.IsNullOrWhiteSpace(input.Indicator), e => e.Indicator.Contains(input.Indicator))
               .WhereIf(!string.IsNullOrWhiteSpace(input.IsActive), e => e.IsActive.Contains(input.IsActive))
               ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var query = from o in pageAndFiltered
                        select new MstWldPunchIndicatorDto
                        {
                            Id = o.Id,
                            Grade = o.Grade,
                            Indicator = o.Indicator,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        // public async Task GenerateAsync()
        //  {
        //    await _dapperRepo.ExecuteAsync(MstWldPunchIndicatorConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}
