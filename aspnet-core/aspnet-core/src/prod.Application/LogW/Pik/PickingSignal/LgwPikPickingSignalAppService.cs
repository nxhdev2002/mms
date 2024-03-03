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
using prod.LogW.Pik.Dto;
using prod.LogW.Pik.Exporting;

namespace prod.LogW.Pik
{
    //  [AbpAuthorize(AppPermissions.Pages_LogW_Pik_PickingSignal)]
    public class LgwPikPickingSignalAppService : prodAppServiceBase, ILgwPikPickingSignalAppService
    {
        private readonly IDapperRepository<LgwPikPickingSignal, long> _dapperRepo;
        private readonly IRepository<LgwPikPickingSignal, long> _repo;
        private readonly ILgwPikPickingSignalExcelExporter _calendarListExcelExporter;

        public LgwPikPickingSignalAppService(IRepository<LgwPikPickingSignal, long> repo,
                                         IDapperRepository<LgwPikPickingSignal, long> dapperRepo,
                                        ILgwPikPickingSignalExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }
      
        public async Task<PagedResultDto<LgwPikPickingSignalDto>> GetAll(GetLgwPikPickingSignalInput input)
        {
            DateTime dateTime = DateTime.UtcNow.Date;
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.PickingTabletId), e => e.PickingTabletId.Contains(input.PickingTabletId))
                .WhereIf(input.FirstSignalTime.HasValue, t => t.FirstSignalTime <= input.FirstSignalTime.Value.Date)
                .WhereIf(input.LastSignalTime.HasValue, t => t.LastSignalTime <= input.LastSignalTime.Value.Date)

                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);

                
            var system = from o in pageAndFiltered
                         select new LgwPikPickingSignalDto
                         {
                             Id = o.Id,
                             PickingTabletId = o.PickingTabletId,
                             TabletProcessId = o.TabletProcessId,
                             PickingProgressId = o.PickingProgressId,
                             FirstSignalTime = o.FirstSignalTime,
                             LastSignalTime = o.LastSignalTime,
                             IsCompleted = o.IsCompleted,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<LgwPikPickingSignalDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetPickingSignalToExcel(GetLgwPikPickingSignalExportInput input)
        {
            DateTime dateTime = DateTime.UtcNow.Date;

            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.PickingTabletId), e => e.PickingTabletId.Contains(input.PickingTabletId))
                .WhereIf(input.FirstSignalTime.HasValue, t => t.FirstSignalTime <= input.FirstSignalTime.Value.Date)
                .WhereIf(input.LastSignalTime.HasValue, t => t.LastSignalTime <= input.LastSignalTime.Value.Date)
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);

            var query = from o in pageAndFiltered
                        select new LgwPikPickingSignalDto
                        {
                            Id = o.Id,
                            PickingTabletId = o.PickingTabletId,
                            TabletProcessId = o.TabletProcessId,
                            PickingProgressId = o.PickingProgressId,
                            FirstSignalTime = o.FirstSignalTime,
                            LastSignalTime = o.LastSignalTime,
                            IsCompleted = o.IsCompleted,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

    }
}

