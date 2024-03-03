using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Microsoft.EntityFrameworkCore;
using prod.Authorization;
using prod.Dto;
using prod.EntityFrameworkCore;
using prod.LogA.Ekb.Dto;
using prod.LogA.Ekb.Ekanban.Dto;
using prod.LogA.Ekb.Exporting;
using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Linq.Extensions;
using System.Linq.Dynamic.Core;

namespace prod.LogA.Ekb.Ekanban
{
    //  [AbpAuthorize(AppPermissions.Pages_LogA_Ekb_Ekanban)]
    public class LgaEkbEkanbanAppService : prodAppServiceBase, ILgaEkbEkanbanAppService
    {
        private readonly IDapperRepository<LgaEkbEkanban, long> _dapperRepo;
        private readonly IRepository<LgaEkbEkanban, long> _repo;
        private readonly ILgaEkbEkanbanExcelExporter _calendarListExcelExporter;

        public LgaEkbEkanbanAppService(IRepository<LgaEkbEkanban, long> repo,
                                         IDapperRepository<LgaEkbEkanban, long> dapperRepo,
                                        ILgaEkbEkanbanExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        //  [AbpAuthorize(AppPermissions.Pages_LogA_Ekb_Ekanban_Edit)]
        public async Task CreateOrEdit(CreateOrEditLgaEkbEkanbanDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditLgaEkbEkanbanDto input)
        {
            var mainObj = ObjectMapper.Map<LgaEkbEkanban>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditLgaEkbEkanbanDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        //  [AbpAuthorize(AppPermissions.Pages_LogA_Ekb_Ekanban_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<LgaEkbEkanbanDto>> GetAll(GetLgaEkbEkanbanInput input)
        {
            DateTime dateTime = DateTime.UtcNow.Date;
            var filtered = _repo.GetAll()
                .WhereIf(!input.WorkingDateFrom.HasValue && !input.WorkingDateTo.HasValue, t => t.WorkingDate == dateTime)
                .WhereIf(input.WorkingDateFrom.HasValue, t => input.WorkingDateFrom.Value.Date <= t.WorkingDate)
                .WhereIf(input.WorkingDateTo.HasValue, t => input.WorkingDateTo.Value.Date.AddDays(1) > t.WorkingDate)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Shift), e => e.Shift.Contains(input.Shift))
                .WhereIf(!string.IsNullOrWhiteSpace(input.PartNo), e => e.PartNo.Contains(input.PartNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.BackNo), e => e.BackNo.Contains(input.BackNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.PcAddress), e => e.PcAddress.Contains(input.PcAddress))
                .WhereIf(!string.IsNullOrWhiteSpace(input.SpsAddress), e => e.SpsAddress.Contains(input.SpsAddress))
                ;
            var pageAndFiltered = filtered.OrderByDescending(s => s.WorkingDate);


            var system = from o in pageAndFiltered
                         select new LgaEkbEkanbanDto
                         {
                             Id = o.Id,
                             KanbanSeq=o.KanbanSeq,
                             KanbanNoInDate=o.KanbanNoInDate,
                             ProdLine = o.ProdLine,
                             WorkingDate = o.WorkingDate,
                             Shift = o.Shift,
                             ProgressId = o.ProgressId,
                             ProcessId = o.ProcessId,
                             PartListId = o.PartListId,
                             PartNo = o.PartNo,
                             PartNoNormalized = o.PartNoNormalized,
                             BackNo = o.BackNo,
                             PcAddress = o.PcAddress,
                             SpsAddress = o.SpsAddress,
                             Sorting = o.Sorting,
                             RequestQty = o.RequestQty,
                             ScanQty = o.ScanQty,
                             InputQty = o.InputQty,
                             IsZeroKb = o.IsZeroKb,
                             RequestDatetime = o.RequestDatetime,
                             PikStartDatetime = o.PikStartDatetime,
                             PikFinishDatetime = o.PikFinishDatetime,
                             DelStartDatetime = o.DelStartDatetime,
                             DelFinishDatetime = o.DelFinishDatetime,
                             Status = o.Status,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<LgaEkbEkanbanDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }

        public async Task<FileDto> GetEkanbanToExcel(GetLgaEkbEkanbanExportInput input)
        {
            DateTime dateTime = DateTime.UtcNow.Date;

            var filtered = _repo.GetAll()
                .WhereIf(!input.WorkingDateFrom.HasValue && !input.WorkingDateTo.HasValue, t => t.WorkingDate == dateTime)
                .WhereIf(input.WorkingDateFrom.HasValue, t => input.WorkingDateFrom.Value.Date <= t.WorkingDate)
                .WhereIf(input.WorkingDateTo.HasValue, t => input.WorkingDateTo.Value.Date.AddDays(1) > t.WorkingDate)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Shift), e => e.Shift.Contains(input.Shift))
                .WhereIf(!string.IsNullOrWhiteSpace(input.PartNo), e => e.PartNo.Contains(input.PartNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.BackNo), e => e.BackNo.Contains(input.BackNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.PcAddress), e => e.PcAddress.Contains(input.PcAddress))
                .WhereIf(!string.IsNullOrWhiteSpace(input.SpsAddress), e => e.SpsAddress.Contains(input.SpsAddress));

            var pageAndFiltered = filtered.OrderByDescending(s => s.WorkingDate);

            var query = from o in pageAndFiltered
                        select new LgaEkbEkanbanDto
                        {
                            Id = o.Id,
                            KanbanSeq = o.KanbanSeq,
                            KanbanNoInDate = o.KanbanNoInDate,
                            ProdLine = o.ProdLine,
                            WorkingDate = o.WorkingDate,
                            Shift = o.Shift,
                            ProgressId = o.ProgressId,
                            ProcessId = o.ProcessId,
                            PartListId = o.PartListId,
                            PartNo = o.PartNo,
                            PartNoNormalized = o.PartNoNormalized,
                            BackNo = o.BackNo,
                            PcAddress = o.PcAddress,
                            SpsAddress = o.SpsAddress,
                            Sorting = o.Sorting,
                            RequestQty = o.RequestQty,
                            ScanQty = o.ScanQty,
                            InputQty = o.InputQty,
                            IsZeroKb = o.IsZeroKb,
                            RequestDatetime = o.RequestDatetime,
                            PikStartDatetime = o.PikStartDatetime,
                            PikFinishDatetime = o.PikFinishDatetime,
                            DelStartDatetime = o.DelStartDatetime,
                            DelFinishDatetime = o.DelFinishDatetime,
                            Status = o.Status,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

    }
}
