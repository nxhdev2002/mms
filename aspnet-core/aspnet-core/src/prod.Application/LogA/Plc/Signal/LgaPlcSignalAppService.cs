using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using prod.Authorization;
using prod.Dto;
using prod.EntityFrameworkCore;
using prod.LogA.Plc.Signal.Dto;
using prod.LogA.Plc.Exporting;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Abp.Domain.Repositories;
using System;

namespace prod.LogA.Plc.Signal
{
    //  [AbpAuthorize(AppPermissions.Pages_LogA_Plc_Signal)]
    public class LgaPlcSignalAppService : prodAppServiceBase, ILgaPlcSignalAppService
    {
        private readonly IRepository<LgaPlcSignal, long> _repo;
        private readonly ILgaPlcSignalExcelExporter _calendarListExcelExporter;

        public LgaPlcSignalAppService(IRepository<LgaPlcSignal, long> repo,
                                      ILgaPlcSignalExcelExporter calendarListExcelExporter)
        {
            _repo = repo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }
       
       
        public async Task<PagedResultDto<LgaPlcSignalDto>> GetAll(GetLgaPlcSignalInput input)
        {
            DateTime dateTime = DateTime.Now.Date;

            var filtered = _repo.GetAll()
                .WhereIf(!input.SignalTimeFrom.HasValue && !input.SignalTimeTo.HasValue, t => t.SignalTime.Value.Date == dateTime)
                .WhereIf(input.SignalTimeFrom.HasValue, t => t.SignalTime.Value.Date >= input.SignalTimeFrom.Value.Date)
                .WhereIf(input.SignalTimeTo.HasValue, t => input.SignalTimeTo.Value.Date >= t.SignalTime.Value.Date)
                .WhereIf(!string.IsNullOrWhiteSpace(input.SignalPattern), e => e.SignalPattern.Contains(input.SignalPattern))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProdLine), e => e.ProdLine.Contains(input.ProdLine))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Process), e => e.Process.Contains(input.Process));

            var pageAndFiltered = filtered.OrderByDescending(s => s.SignalTime);


            var system = from o in pageAndFiltered
                         select new LgaPlcSignalDto
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

            return new PagedResultDto<LgaPlcSignalDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetSignalToExcel(GetLgaPlcSignalExportInput input)
        {
            DateTime dateTime = DateTime.UtcNow.Date;
            var filtered = from o in _repo.GetAll().AsNoTracking()
                .WhereIf(!input.SignalTimeFrom.HasValue && !input.SignalTimeTo.HasValue, t => t.SignalTime.Value.Date == dateTime)
                .WhereIf(input.SignalTimeFrom.HasValue, t => t.SignalTime.Value.Date >= input.SignalTimeFrom.Value.Date)
                .WhereIf(input.SignalTimeTo.HasValue, t => input.SignalTimeTo.Value.Date >= t.SignalTime.Value.Date)
                .WhereIf(!string.IsNullOrWhiteSpace(input.SignalPattern), e => e.SignalPattern.Contains(input.SignalPattern))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProdLine), e => e.ProdLine.Contains(input.ProdLine))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Process), e => e.Process.Contains(input.Process))
                .OrderByDescending(o => o.SignalTime)
                           select new LgaPlcSignalDto
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
