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
    [AbpAuthorize(AppPermissions.Pages_WorkingPattern_Week_View)]
    public class MstWptWeekAppService : prodAppServiceBase, IMstWptWeekAppService
    {
        private readonly IDapperRepository<MstWptWeek, long> _dapperRepo;
        private readonly IRepository<MstWptWeek, long> _repo;
        private readonly IMstWptWeekExcelExporter _calendarListExcelExporter;

        public MstWptWeekAppService(IRepository<MstWptWeek, long> repo,
                                    IDapperRepository<MstWptWeek, long> dapperRepo,
                                    IMstWptWeekExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        [AbpAuthorize(AppPermissions.Pages_WorkingPattern_Week_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstWptWeekDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstWptWeekDto input)
        {
            var mainObj = ObjectMapper.Map<MstWptWeek>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstWptWeekDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_WorkingPattern_Week_Edit)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstWptWeekDto>> GetAll(GetMstWptWeekInput input)
        {
            input.WorkingYear = (input.WorkingYear == null) ?  0 : input.WorkingYear;
            var filtered = _repo.GetAll()
                 .WhereIf(input.WorkingYear != 0, t => t.WorkingYear == input.WorkingYear);
                
            var pageAndFiltered = filtered.OrderBy(s => s.WorkingYear);


            var system = from o in pageAndFiltered
                         select new MstWptWeekDto
                         {
                             Id = o.Id,
                             WorkingYear = o.WorkingYear,
                             WeekNumber = o.WeekNumber,
                             WorkingDays = o.WorkingDays,
                             IsActive = o.IsActive
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstWptWeekDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetWeekToExcel(MstWptWeekExportInput input)
        {
            input.WorkingYear = (input.WorkingYear == null) ? 0 : input.WorkingYear;
            var filtered = _repo.GetAll()
                 .WhereIf(input.WorkingYear != 0, t => t.WorkingYear == input.WorkingYear);

            var pageAndFiltered = filtered.OrderBy(s => s.WorkingYear);


            var query = from o in pageAndFiltered
                         select new MstWptWeekDto
                        {
                            Id = o.Id,
                            WorkingYear = o.WorkingYear,
                            WeekNumber = o.WeekNumber,
                            WorkingDays = o.WorkingDays,
                            IsActive = o.IsActive
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

    }
}
