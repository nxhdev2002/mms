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
using prod.LogA.Ekb.Dto;
using prod.LogA.Ekb.Exporting;

namespace prod.LogA.Ekb
{
    //  [AbpAuthorize(AppPermissions.Pages_LogA_Ekb_Progress)]
    public class LgaEkbProgressAppService : prodAppServiceBase, ILgaEkbProgressAppService
    {
        private readonly IDapperRepository<LgaEkbProgress, long> _dapperRepo;
        private readonly IRepository<LgaEkbProgress, long> _repo;
        private readonly ILgaEkbProgressExcelExporter _calendarListExcelExporter;

        public LgaEkbProgressAppService(IRepository<LgaEkbProgress, long> repo,
                                         IDapperRepository<LgaEkbProgress, long> dapperRepo,
                                        ILgaEkbProgressExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        //  [AbpAuthorize(AppPermissions.Pages_LogA_Ekb_Progress_Edit)]
        public async Task CreateOrEdit(CreateOrEditLgaEkbProgressDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditLgaEkbProgressDto input)
        {
            var mainObj = ObjectMapper.Map<LgaEkbProgress>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditLgaEkbProgressDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        //  [AbpAuthorize(AppPermissions.Pages_LogA_Ekb_Progress_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<LgaEkbProgressDto>> GetAll(GetLgaEkbProgressInput input)
        {
            DateTime dateTime = DateTime.Now.Date;

            var filtered = _repo.GetAll()
                .WhereIf(!input.WorkingDateFrom.HasValue && !input.WorkingDateTo.HasValue, t => t.WorkingDate == dateTime)
                .WhereIf(input.WorkingDateFrom.HasValue, t => input.WorkingDateFrom.Value.Date <= t.WorkingDate)
                .WhereIf(input.WorkingDateTo.HasValue, t => input.WorkingDateTo.Value.Date.AddDays(1) > t.WorkingDate)
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProdLine), e => e.ProdLine.Contains(input.ProdLine))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Shift), e => e.Shift.Contains(input.Shift))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProcessCode), e => e.ProcessCode.Contains(input.ProcessCode))
                .WhereIf(input.StartDatetime.HasValue, t => t.StartDatetime >= input.StartDatetime.Value)
                .WhereIf(input.FinishDatetime.HasValue, t => t.FinishDatetime <= input.FinishDatetime.Value)
                ;
            var pageAndFiltered = filtered.OrderByDescending(s => s.WorkingDate).ThenBy(s => s.ProdLine).ThenBy(t => t.Shift).ThenBy(a => a.NoInShift);
            var system = from o in pageAndFiltered
                         select new LgaEkbProgressDto
                         {
                             Id = o.Id,
                             ProdLine = o.ProdLine,
                             WorkingDate = o.WorkingDate,
                             Shift = o.Shift,
                             NoInShift = o.NoInShift,
                             NoInDate = o.NoInDate,
                             ProcessId = o.ProcessId,
                             ProcessCode = o.ProcessCode,
                             NewtaktDatetime = o.NewtaktDatetime,
                             StartDatetime = o.StartDatetime,
                             FinishDatetime = o.FinishDatetime,
                             Status = o.Status,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<LgaEkbProgressDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }

        public async Task<FileDto> GetProgressToExcel(GetLgaEkbProgressExportInput input)
        {
            DateTime dateTime = DateTime.Now.Date;

            var filtered = _repo.GetAll()
                  .WhereIf(!input.WorkingDateFrom.HasValue && !input.WorkingDateTo.HasValue, t => t.WorkingDate == dateTime)
                  .WhereIf(input.WorkingDateFrom.HasValue, t => input.WorkingDateFrom.Value.Date <= t.WorkingDate)
                  .WhereIf(input.WorkingDateTo.HasValue, t => input.WorkingDateTo.Value.Date.AddDays(1) > t.WorkingDate)
                  .WhereIf(!string.IsNullOrWhiteSpace(input.ProdLine), e => e.ProdLine.Contains(input.ProdLine))
                  .WhereIf(!string.IsNullOrWhiteSpace(input.Shift), e => e.Shift.Contains(input.Shift))
                  .WhereIf(!string.IsNullOrWhiteSpace(input.ProcessCode), e => e.ProcessCode.Contains(input.ProcessCode))
                  .WhereIf(input.StartDatetime.HasValue, t => t.StartDatetime >= input.StartDatetime.Value)
                  .WhereIf(input.FinishDatetime.HasValue, t => t.FinishDatetime <= input.FinishDatetime.Value)
                  ;
            var pageAndFiltered = filtered.OrderByDescending(s => s.WorkingDate).ThenBy(s => s.ProdLine).ThenBy(t => t.Shift).ThenBy(a => a.NoInShift);
            var query = from o in pageAndFiltered
                        select new LgaEkbProgressDto
                        {
                            Id = o.Id,
                            ProdLine = o.ProdLine,
                            WorkingDate = o.WorkingDate,
                            Shift = o.Shift,
                            NoInShift = o.NoInShift,
                            NoInDate = o.NoInDate,
                            ProcessId = o.ProcessId,
                            ProcessCode = o.ProcessCode,
                            NewtaktDatetime = o.NewtaktDatetime,
                            StartDatetime = o.StartDatetime,
                            FinishDatetime = o.FinishDatetime,
                            Status = o.Status,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }
    }
}
