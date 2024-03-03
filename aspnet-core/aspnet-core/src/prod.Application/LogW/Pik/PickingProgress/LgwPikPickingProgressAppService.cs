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
    //  [AbpAuthorize(AppPermissions.Pages_LogW_Pik_PickingProgress)]
    public class LgwPikPickingProgressAppService : prodAppServiceBase, ILgwPikPickingProgressAppService
    {
        private readonly IDapperRepository<LgwPikPickingProgress, long> _dapperRepo;
        private readonly IRepository<LgwPikPickingProgress, long> _repo;
        private readonly ILgwPikPickingProgressExcelExporter _calendarListExcelExporter;

        public LgwPikPickingProgressAppService(IRepository<LgwPikPickingProgress, long> repo,
                                         IDapperRepository<LgwPikPickingProgress, long> dapperRepo,
                                        ILgwPikPickingProgressExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        public async Task<PagedResultDto<LgwPikPickingProgressDto>> GetAll(GetLgwPikPickingProgressInput input)
        {
            DateTime dateTime = DateTime.Now.Date;

            var filtered = _repo.GetAll()
                .WhereIf(!input.WorkingDateFrom.HasValue && !input.WorkingDateTo.HasValue, t => t.WorkingDate == dateTime)
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProdLine), e => e.ProdLine.Contains(input.ProdLine))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProcessCode), e => e.ProcessCode.Contains(input.ProcessCode))
                .WhereIf(input.WorkingDateFrom.HasValue, t => input.WorkingDateFrom.Value.Date <= t.WorkingDate)
                .WhereIf(input.WorkingDateTo.HasValue, t => input.WorkingDateTo.Value.Date.AddDays(1) > t.WorkingDate);

            var pageAndFiltered = filtered.OrderByDescending(s => s.WorkingDate)
                                        .ThenBy(s => s.ProdLine)
                                        .ThenBy(t => t.SeqNo);



            var system = from o in pageAndFiltered
                         select new LgwPikPickingProgressDto
                         {
                             Id = o.Id,
                             PickingTabletId = o.PickingTabletId,
                             ProdLine = o.ProdLine,
                             BodyNo = o.BodyNo,
                             LotNo = o.LotNo,
                             ProcessCode = o.ProcessCode,
                             ProcessGroup = o.ProcessGroup,
                             SeqNo = o.SeqNo,
                             WorkingDate = o.WorkingDate,
                             Shift = o.Shift,
                             TaktStartTime = o.TaktStartTime,
                             StartTime = o.StartTime,
                             FinishTime = o.FinishTime,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<LgwPikPickingProgressDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetPickingProgressToExcel(GetLgwPikPickingProgressExportInput input)
        {

            DateTime dateTime = DateTime.Now.Date;
            var filtered = _repo.GetAll()
              .WhereIf(!input.WorkingDateFrom.HasValue && !input.WorkingDateTo.HasValue, t => t.WorkingDate == dateTime)
              .WhereIf(!string.IsNullOrWhiteSpace(input.ProdLine), e => e.ProdLine.Contains(input.ProdLine))
              .WhereIf(!string.IsNullOrWhiteSpace(input.ProcessCode), e => e.ProcessCode.Contains(input.ProcessCode))
              .WhereIf(input.WorkingDateFrom.HasValue, t => input.WorkingDateFrom.Value.Date <= t.WorkingDate)
              .WhereIf(input.WorkingDateTo.HasValue, t => input.WorkingDateTo.Value.Date.AddDays(1) > t.WorkingDate);

            var pageAndFiltered = filtered.OrderByDescending(s => s.WorkingDate)
                                        .ThenBy(s => s.ProdLine)
                                        .ThenBy(t => t.SeqNo);

            var query = from o in pageAndFiltered
                        select new LgwPikPickingProgressDto
                        {
                            Id = o.Id,
                            PickingTabletId = o.PickingTabletId,
                            ProdLine = o.ProdLine,
                            BodyNo = o.BodyNo,
                            LotNo = o.LotNo,
                            ProcessCode = o.ProcessCode,
                            ProcessGroup = o.ProcessGroup,
                            SeqNo = o.SeqNo,
                            WorkingDate = o.WorkingDate,
                            Shift = o.Shift,
                            TaktStartTime = o.TaktStartTime,
                            StartTime = o.StartTime,
                            FinishTime = o.FinishTime,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

    }
}
