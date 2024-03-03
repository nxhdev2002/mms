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
using prod.Painting.Andon.Dto;
using prod.Painting.Andon.Exporting;

namespace prod.Painting.Andon
{
    //  [AbpAuthorize(AppPermissions.Pages_Painting_Andon_LineEfficiencyDetails)]
    public class PtsAdoLineEfficiencyDetailsAppService : prodAppServiceBase, IPtsAdoLineEfficiencyDetailsAppService
    {
        private readonly IDapperRepository<PtsAdoLineEfficiencyDetails, long> _dapperRepo;
        private readonly IRepository<PtsAdoLineEfficiencyDetails, long> _repo;
        private readonly IPtsAdoLineEfficiencyDetailsExcelExporter _calendarListExcelExporter;

        public PtsAdoLineEfficiencyDetailsAppService(IRepository<PtsAdoLineEfficiencyDetails, long> repo,
                                         IDapperRepository<PtsAdoLineEfficiencyDetails, long> dapperRepo,
                                        IPtsAdoLineEfficiencyDetailsExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        //  [AbpAuthorize(AppPermissions.Pages_Painting_Andon_LineEfficiencyDetails_Edit)]
        public async Task CreateOrEdit(CreateOrEditPtsAdoLineEfficiencyDetailsDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditPtsAdoLineEfficiencyDetailsDto input)
        {
            var mainObj = ObjectMapper.Map<PtsAdoLineEfficiencyDetails>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditPtsAdoLineEfficiencyDetailsDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        //  [AbpAuthorize(AppPermissions.Pages_Painting_Andon_LineEfficiencyDetails_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<PtsAdoLineEfficiencyDetailsDto>> GetAll(GetPtsAdoLineEfficiencyDetailsInput input)
        {

            DateTime dateTime = DateTime.UtcNow.Date;

            var filtered = _repo.GetAll()
                .WhereIf(!input.RequestDateFrom.HasValue && !input.RequestDateTo.HasValue, t => t.WorkingDate == dateTime)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Line), e => e.Line.Contains(input.Line))
                .WhereIf(input.WorkingDate.HasValue, t => t.WorkingDate.Value.Date <= input.WorkingDate.Value.Date)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Shift), e => e.Shift.Contains(input.Shift))
                .WhereIf(input.RequestDateFrom.HasValue, e => input.RequestDateFrom <= e.WorkingDate)
                .WhereIf(input.RequestDateTo.HasValue, e => input.RequestDateTo >= e.WorkingDate);

            ;
            var pageAndFiltered = filtered.OrderByDescending(s => s.WorkingDate).ThenBy(a => a.Line).ThenBy(a => a.Shift);


            var system = from o in pageAndFiltered
                         select new PtsAdoLineEfficiencyDetailsDto
                         {
                             Id = o.Id,
                             Line = o.Line,
                             VolActual = o.VolActual,
                             LineStopTime = o.LineStopTime,
                             LineEfficiency = o.LineEfficiency,
                             WorkingDate = o.WorkingDate,
                             Shift =
                             (
                             o.Shift == "1" ? "Shift 1" :
                             o.Shift == "2" ? "Shift 2" :
                             o.Shift == "3" ? "Shift 3" : "unknown"
                             ),
                             Status = o.Status,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<PtsAdoLineEfficiencyDetailsDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetLineEfficiencyDetailsToExcel(GetPtsAdoLineEfficiencyDetailsExportInput input)
        {
            DateTime dateTime = DateTime.UtcNow.Date;

            var filtered = _repo.GetAll()
                .WhereIf(!input.RequestDateFrom.HasValue && !input.RequestDateTo.HasValue, t => t.WorkingDate == dateTime)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Line), e => e.Line.Contains(input.Line))
                .WhereIf(input.WorkingDate.HasValue, t => t.WorkingDate.Value.Date <= input.WorkingDate.Value.Date)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Shift), e => e.Shift.Contains(input.Shift))
                .WhereIf(input.RequestDateFrom.HasValue, e => input.RequestDateFrom <= e.WorkingDate)
                .WhereIf(input.RequestDateTo.HasValue, e => input.RequestDateTo >= e.WorkingDate);

            ;
            var pageAndFiltered = filtered.OrderByDescending(s => s.WorkingDate).ThenBy(a => a.Line).ThenBy(a=> a.LineEfficiency).ThenBy(a => a.LineStopTime).ThenBy(a => a.Shift);

            var query = from o in pageAndFiltered
                         select new PtsAdoLineEfficiencyDetailsDto
                        {
                            Id = o.Id,
                            Line = o.Line,
                            VolActual = o.VolActual,
                            LineStopTime = o.LineStopTime,
                            LineEfficiency = o.LineEfficiency,
                            WorkingDate = o.WorkingDate,
                            Shift = o.Shift,
                            Status = o.Status,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        // public async Task GenerateAsync()
        //  {
        //    await _dapperRepo.ExecuteAsync(PtsAdoLineEfficiencyDetailsConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}
