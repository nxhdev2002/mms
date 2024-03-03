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
using prod.Welding.Andon.Dto;
using prod.Welding.Andon.Exporting;

namespace prod.Welding.Andon
{
    //  [AbpAuthorize(AppPermissions.Pages_Welding_Andon_PunchQueue)]
    public class WldAdoPunchQueueAppService : prodAppServiceBase, IWldAdoPunchQueueAppService
    {
        private readonly IDapperRepository<WldAdoPunchQueue, long> _dapperRepo;
        private readonly IRepository<WldAdoPunchQueue, long> _repo;
        private readonly IWldAdoPunchQueueExcelExporter _calendarListExcelExporter;

        public WldAdoPunchQueueAppService(IRepository<WldAdoPunchQueue, long> repo,
                                         IDapperRepository<WldAdoPunchQueue, long> dapperRepo,
                                        IWldAdoPunchQueueExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        //  [AbpAuthorize(AppPermissions.Pages_Welding_Andon_PunchQueue_Edit)]
        public async Task CreateOrEdit(CreateOrEditWldAdoPunchQueueDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditWldAdoPunchQueueDto input)
        {
            var mainObj = ObjectMapper.Map<WldAdoPunchQueue>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditWldAdoPunchQueueDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        //  [AbpAuthorize(AppPermissions.Pages_Welding_Andon_PunchQueue_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<WldAdoPunchQueueDto>> GetAll(GetWldAdoPunchQueueInput input)
        {
            DateTime dateTime = DateTime.Now.Date;
            var filtered = _repo.GetAll()
                .WhereIf(!input.ScanTimeFrom.HasValue && !input.ScanTimeTo.HasValue, t => t.ScanTime.Value.Date == dateTime)
                .WhereIf(input.ScanTimeFrom.HasValue, t => t.ScanTime.Value.Date >= input.ScanTimeFrom.Value.Date)
                .WhereIf(input.ScanTimeTo.HasValue, t => input.ScanTimeTo.Value.Date >= t.ScanTime.Value.Date)
                .WhereIf(!string.IsNullOrWhiteSpace(input.BodyNo), e => e.BodyNo.Contains(input.BodyNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Model), e => e.Model.Contains(input.Model))
                .WhereIf(!string.IsNullOrWhiteSpace(input.LotNo), e => e.LotNo.Contains(input.LotNo));

            var pageAndFiltered = filtered.OrderByDescending(s => s.ScanTime);

            var system = from o in pageAndFiltered
                         select new WldAdoPunchQueueDto
                         {
                             Id = o.Id,
                             BodyNo = o.BodyNo,
                             Model = o.Model,
                             Line = o.Line,
                             LotNo = o.LotNo,
                             Color = o.Color,
                             ProcessGroup = o.ProcessGroup,
                             ScanTime = o.ScanTime,
                             PunchFlag = o.PunchFlag,
                             PunchIndicator = o.PunchIndicator,
                             IsCall = o.IsCall,
                             IsCf = o.IsCf
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<WldAdoPunchQueueDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetPunchQueueToExcel(GetWldAdoPunchQueueExportInput input)
        {
            DateTime dateTime = DateTime.Now.Date;
            var filtered = _repo.GetAll()
                .WhereIf(!input.ScanTimeFrom.HasValue && !input.ScanTimeTo.HasValue, t => t.ScanTime.Value.Date == dateTime)
                .WhereIf(input.ScanTimeFrom.HasValue, t => t.ScanTime.Value.Date >= input.ScanTimeFrom.Value.Date)
                .WhereIf(input.ScanTimeTo.HasValue, t => input.ScanTimeTo.Value.Date >= t.ScanTime.Value.Date)
                .WhereIf(!string.IsNullOrWhiteSpace(input.BodyNo), e => e.BodyNo.Contains(input.BodyNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Model), e => e.Model.Contains(input.Model))
                .WhereIf(!string.IsNullOrWhiteSpace(input.LotNo), e => e.LotNo.Contains(input.LotNo));

            var pageAndFiltered = filtered.OrderByDescending(s => s.ScanTime);

            var query = from o in pageAndFiltered
                        select new WldAdoPunchQueueDto
                        {
                            Id = o.Id,
                            BodyNo = o.BodyNo,
                            Model = o.Model,
                            Line = o.Line,
                            LotNo = o.LotNo,
                            Color = o.Color,
                            ProcessGroup = o.ProcessGroup,
                            ScanTime = o.ScanTime,
                            PunchFlag = o.PunchFlag,
                            PunchIndicator = o.PunchIndicator,
                            IsCall = o.IsCall,
                            IsCf = o.IsCf
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }
    }
}
