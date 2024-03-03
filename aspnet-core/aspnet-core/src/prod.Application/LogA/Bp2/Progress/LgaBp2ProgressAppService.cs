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
using prod.LogA.Bp2.Dto;
using prod.LogA.Bp2.Exporting;

namespace prod.LogA.Bp2
{
    //  [AbpAuthorize(AppPermissions.Pages_LogA_Bp2_Progress)]
    public class LgaBp2ProgressAppService : prodAppServiceBase, ILgaBp2ProgressAppService
    {
        private readonly IDapperRepository<LgaBp2Progress, long> _dapperRepo;
        private readonly IRepository<LgaBp2Progress, long> _repo;
        private readonly ILgaBp2ProgressExcelExporter _calendarListExcelExporter;

        public LgaBp2ProgressAppService(IRepository<LgaBp2Progress, long> repo,
                                         IDapperRepository<LgaBp2Progress, long> dapperRepo,
                                        ILgaBp2ProgressExcelExporter calendarListExcelExporter)
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        public async Task<PagedResultDto<LgaBp2ProgressDto>> GetAll(GetLgaBp2ProgressInput input)
        {
            DateTime dateTime = DateTime.UtcNow.Date;

            var filtered = _repo.GetAll()
                .WhereIf(!input.WorkingDateFrom.HasValue && !input.WorkingDateTo.HasValue, t => t.WorkingDate == dateTime)
                .WhereIf(input.WorkingDateFrom.HasValue, t => input.WorkingDateFrom.Value.Date <= t.WorkingDate)
                .WhereIf(input.WorkingDateTo.HasValue, t => input.WorkingDateTo.Value.Date.AddDays(1) > t.WorkingDate)
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProdLine), e => e.ProdLine.Contains(input.ProdLine))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Shift), e => e.Shift.Contains(input.Shift));

            var pageAndFiltered = filtered.OrderByDescending(s => s.WorkingDate).ThenBy(t => t.Shift).ThenBy(t => t.NoInShift);

            var system = from o in pageAndFiltered
                         select new LgaBp2ProgressDto
                         {
                             Id = o.Id,
                             ProcessId = o.ProcessId,
                             EcarId = o.EcarId,
                             ProdLine = o.ProdLine,
                             WorkingDate = o.WorkingDate,
                             Shift = o.Shift,
                             NoInShift = o.NoInShift,                  
                             NewtaktDatetime = o.NewtaktDatetime,
                             StartDatetime = o.StartDatetime,
                             FinishDatetime = o.FinishDatetime,
                             Status = o.Status,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<LgaBp2ProgressDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetProgressToExcel(GetLgaBp2ProgressExportInput input)
        {
            DateTime dateTime = DateTime.UtcNow.Date;

            var filtered = _repo.GetAll()
                .WhereIf(!input.WorkingDateFrom.HasValue && !input.WorkingDateTo.HasValue, t => t.WorkingDate == dateTime)
                .WhereIf(input.WorkingDateFrom.HasValue, t => input.WorkingDateFrom.Value.Date <= t.WorkingDate)
                .WhereIf(input.WorkingDateTo.HasValue, t => input.WorkingDateTo.Value.Date.AddDays(1) > t.WorkingDate)
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProdLine), e => e.ProdLine.Contains(input.ProdLine))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Shift), e => e.Shift.Contains(input.Shift));

            var pageAndFiltered = filtered.OrderByDescending(s => s.WorkingDate).ThenBy(t => t.Shift).ThenBy(t => t.NoInShift);

            var query = from o in pageAndFiltered
                        select new LgaBp2ProgressDto
                        {
                            Id = o.Id,
                            ProcessId = o.ProcessId,
                            EcarId = o.EcarId,
                            ProdLine = o.ProdLine,
                            WorkingDate = o.WorkingDate,
                            Shift = o.Shift,
                            NoInShift = o.NoInShift,
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
