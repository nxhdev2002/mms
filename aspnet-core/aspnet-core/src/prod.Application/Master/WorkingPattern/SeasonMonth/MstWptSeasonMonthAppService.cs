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
using prod;
using prod.Authorization;
using prod.Dto;
using prod.EntityFrameworkCore;
using prod.Master.WorkingPattern;
using prod.Master.WorkingPattern.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using prod.Master.WorkingPattern.Exporting;

namespace prod.Master.WorkingPattern
{
      [AbpAuthorize(AppPermissions.Pages_WorkingPattern_SeasonMonth_View)]
    public class MstWptSeasonMonthAppService : prodAppServiceBase, IMstWptSeasonMonthAppService
    {
        private readonly IDapperRepository<MstWptSeasonMonth, long> _dapperRepo;
        private readonly IRepository<MstWptSeasonMonth, long> _repo;
        private readonly IMstWptSeasonMonthExcelExporter _calendarListExcelExporter;

        public MstWptSeasonMonthAppService(IRepository<MstWptSeasonMonth, long> repo,
                                         IDapperRepository<MstWptSeasonMonth, long> dapperRepo,
                                        IMstWptSeasonMonthExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

          [AbpAuthorize(AppPermissions.Pages_WorkingPattern_SeasonMonth_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstWptSeasonMonthDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstWptSeasonMonthDto input)
        {
            var mainObj = ObjectMapper.Map<MstWptSeasonMonth>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstWptSeasonMonthDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

          [AbpAuthorize(AppPermissions.Pages_WorkingPattern_SeasonMonth_Edit)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstWptSeasonMonthDto>> GetAll(GetMstWptSeasonMonthInput input)
        {
            DateTime dateTime = DateTime.UtcNow.Date;

            var filtered = _repo.GetAll()
                .WhereIf(!input.SeasonMonth.HasValue, t => t.SeasonMonth <= dateTime)
                .WhereIf(input.SeasonMonth.HasValue,t => t.SeasonMonth.Month == input.SeasonMonth.Value.Month && t.SeasonMonth.Year == input.SeasonMonth.Value.Year)
                .WhereIf(!string.IsNullOrWhiteSpace(input.SeasonType), e => e.SeasonType.Contains(input.SeasonType));

            var pageAndFiltered = filtered.OrderByDescending(s => s.SeasonMonth);

            var system = from o in pageAndFiltered
                         select new MstWptSeasonMonthDto
                         {
                             Id = o.Id,
                             SeasonMonth = o.SeasonMonth,
                             SeasonType = o.SeasonType,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstWptSeasonMonthDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetSeasonMonthToExcel(MstWptSeasonMonthExportInput input)
        {
            DateTime dateTime = DateTime.UtcNow.Date;

            var filtered = _repo.GetAll()
                .WhereIf(!input.SeasonMonth.HasValue, t => t.SeasonMonth <= dateTime)
                .WhereIf(input.SeasonMonth.HasValue, t => t.SeasonMonth.Month == input.SeasonMonth.Value.Month && t.SeasonMonth.Year == input.SeasonMonth.Value.Year)
                .WhereIf(!string.IsNullOrWhiteSpace(input.SeasonType), e => e.SeasonType.Contains(input.SeasonType));

            var pageAndFiltered = filtered.OrderByDescending(s => s.SeasonMonth);

            var query = from o in pageAndFiltered
                         select new MstWptSeasonMonthDto
                        {
                            Id = o.Id,
                            SeasonMonth = o.SeasonMonth,
                            SeasonType = o.SeasonType,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

    }
}
