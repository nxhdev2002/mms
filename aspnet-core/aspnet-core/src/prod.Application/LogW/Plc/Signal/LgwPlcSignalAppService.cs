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
using prod.LogW.Dto;
using prod.LogW.Plc.Signal.Exporting;

namespace prod.LogW.Plc
{
    //  [AbpAuthorize(AppPermissions.Pages_LogW_Plc_Signal)]
    public class LgwPlcSignalAppService : prodAppServiceBase, ILgwPlcSignalAppService
    {
        private readonly IRepository<LgwPlcSignal, long> _repo;
        private readonly ILgwPlcSignalExcelExporter _calendarListExcelExporter;

        public LgwPlcSignalAppService(IRepository<LgwPlcSignal, long> repo,
                                      ILgwPlcSignalExcelExporter calendarListExcelExporter)
        {
            _repo = repo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        public async Task<PagedResultDto<LgwPlcSignalDto>> GetAll(GetLgwPlcSignalInput input)
        {
            DateTime dateTime = DateTime.UtcNow.Date;
            var filtered = _repo.GetAll()
                .WhereIf(!input.SignalTimeFrom.HasValue && !input.SignalTimeTo.HasValue, t => t.SignalTime.Value.Date == dateTime)
                .WhereIf(input.SignalTimeFrom.HasValue, t => t.SignalTime.Value.Date >= input.SignalTimeFrom.Value.Date)
                .WhereIf(input.SignalTimeTo.HasValue, t => input.SignalTimeTo.Value.Date >= t.SignalTime.Value.Date)
                .WhereIf(!string.IsNullOrWhiteSpace(input.SignalPattern), e => e.SignalPattern.Contains(input.SignalPattern))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProdLine), e => e.ProdLine.Contains(input.ProdLine))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Process), e => e.Process.Contains(input.Process))
                .WhereIf(input.RefId.HasValue, e => e.RefId == input.RefId);

            var pageAndFiltered = filtered.OrderByDescending(s => s.SignalTime);

            var system = from o in pageAndFiltered
                         select new LgwPlcSignalDto
                         {
                             Id = o.Id,
                             SignalIndex = o.SignalIndex,
                             SignalPattern = o.SignalPattern,
                             SignalTime = o.SignalTime,
                             ProdLine = o.ProdLine,
                             Process = o.Process,
                             RefId = o.RefId,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<LgwPlcSignalDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }

        public async Task<FileDto> GetSignalToExcel(GetLgwPlcSignalExportInput input)
        {
            DateTime dateTime = DateTime.UtcNow.Date;
            var filtered = from o in _repo.GetAll()
                 .WhereIf(!input.SignalTimeFrom.HasValue && !input.SignalTimeTo.HasValue, t => t.SignalTime.Value.Date == dateTime)
                .WhereIf(input.SignalTimeFrom.HasValue, t => t.SignalTime.Value.Date >= input.SignalTimeFrom.Value.Date)
                .WhereIf(input.SignalTimeTo.HasValue, t => input.SignalTimeTo.Value.Date >= t.SignalTime.Value.Date)
                .WhereIf(!string.IsNullOrWhiteSpace(input.SignalPattern), e => e.SignalPattern.Contains(input.SignalPattern))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProdLine), e => e.ProdLine.Contains(input.ProdLine))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Process), e => e.Process.Contains(input.Process))
                .WhereIf(input.RefId.HasValue, e => e.RefId == input.RefId)
                .OrderBy(o => o.SignalTime)
                .OrderByDescending(o => o.SignalTime)
                           select new LgwPlcSignalDto
                         {
                             Id = o.Id,
                             SignalIndex = o.SignalIndex,
                             SignalPattern = o.SignalPattern,
                             SignalTime = o.SignalTime,
                             ProdLine = o.ProdLine,
                             Process = o.Process,
                             RefId = o.RefId,
                             IsActive = o.IsActive,
                         };
            var exportToExcel = await filtered.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }
    }
}
