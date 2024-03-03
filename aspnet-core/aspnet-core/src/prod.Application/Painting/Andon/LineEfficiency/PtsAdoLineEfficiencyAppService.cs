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
using System.Globalization;
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
    //  [AbpAuthorize(AppPermissions.Pages_Painting_Andon_LineEfficiency)]
    public class PtsAdoLineEfficiencyAppService : prodAppServiceBase, IPtsAdoLineEfficiencyAppService
    {
        private readonly IDapperRepository<PtsAdoLineEfficiency, long> _dapperRepo;
        private readonly IRepository<PtsAdoLineEfficiency, long> _repo;
        private readonly IPtsAdoLineEfficiencyExcelExporter _calendarListExcelExporter;

        public PtsAdoLineEfficiencyAppService(IRepository<PtsAdoLineEfficiency, long> repo,
                                         IDapperRepository<PtsAdoLineEfficiency, long> dapperRepo,
                                        IPtsAdoLineEfficiencyExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        //  [AbpAuthorize(AppPermissions.Pages_Painting_Andon_LineEfficiency_Edit)]
        public async Task CreateOrEdit(CreateOrEditPtsAdoLineEfficiencyDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditPtsAdoLineEfficiencyDto input)
        {
            var mainObj = ObjectMapper.Map<PtsAdoLineEfficiency>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditPtsAdoLineEfficiencyDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        //  [AbpAuthorize(AppPermissions.Pages_Painting_Andon_LineEfficiency_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<PtsAdoLineEfficiencyDto>> GetAll(GetPtsAdoLineEfficiencyInput input)
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
                         select new PtsAdoLineEfficiencyDto
                         {
                             Id = o.Id,
                             Line = o.Line,
                             Shift =
                             (
                             o.Shift == "1" ? "Shift 1" :
                             o.Shift == "2" ? "Shift 2":
                             o.Shift == "3" ? "Shift 3" :"unknown"
                             ),
                             WorkingDate = o.WorkingDate,
                             VolTarget = o.VolTarget,
                             VolActual = o.VolActual,
                             VolBalance = o.VolBalance,
                             StopTime = o.StopTime,
                             Efficiency = o.Efficiency,
                             TaktTime = o.TaktTime,
                             Overtime = o.Overtime,
                             NonProdAct = o.NonProdAct,
                             OffLine1 = o.OffLine1,
                             OffLine2 = o.OffLine2,
                             OffLine3 = o.OffLine3,
                             ShiftVolPlan = o.ShiftVolPlan,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<PtsAdoLineEfficiencyDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetLineEfficiencyToExcel(GetPtsAdoLineEfficiencyExportInput input)
        {
            DateTime dateTime = DateTime.UtcNow.Date;
            var filtered = _repo.GetAll()
                .WhereIf(!input.WorkingDateFrom.HasValue && !input.WorkingDateTo.HasValue, t => t.WorkingDate == dateTime)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Line), e => e.Line.Contains(input.Line))
                .WhereIf(input.WorkingDate.HasValue, t => t.WorkingDate.Value.Date <= input.WorkingDate.Value.Date)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Shift), e => e.Shift.Contains(input.Shift))
                .WhereIf(input.WorkingDateFrom.HasValue, t => input.WorkingDateFrom.Value.Date <= t.WorkingDate)
                .WhereIf(input.WorkingDateTo.HasValue, t => input.WorkingDateTo.Value.Date.AddDays(1) > t.WorkingDate);

            ;
            var pageAndFiltered = filtered.OrderByDescending(s => s.WorkingDate).ThenBy(a => a.Line).ThenBy(a => a.Shift);


            var query = from o in pageAndFiltered
                         select new PtsAdoLineEfficiencyDto
                        {
                            Id = o.Id,
                            Line = o.Line,
                            Shift = o.Shift,
                            WorkingDate = o.WorkingDate,
                            VolTarget = o.VolTarget,
                            VolActual = o.VolActual,
                            VolBalance = o.VolBalance,
                            StopTime = o.StopTime,
                            Efficiency = o.Efficiency,
                            TaktTime = o.TaktTime,
                            Overtime = o.Overtime,
                            NonProdAct = o.NonProdAct,
                            OffLine1 = o.OffLine1,
                            OffLine2 = o.OffLine2,
                            OffLine3 = o.OffLine3,
                            ShiftVolPlan = o.ShiftVolPlan,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        // public async Task GenerateAsync()
        //  {
        //    await _dapperRepo.ExecuteAsync(PtsAdoLineEfficiencyConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}
