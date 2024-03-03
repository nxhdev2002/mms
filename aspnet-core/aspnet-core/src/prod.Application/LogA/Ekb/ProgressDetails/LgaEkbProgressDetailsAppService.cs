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
    //  [AbpAuthorize(AppPermissions.Pages_LogA_Ekb_ProgressDetails)]
    public class LgaEkbProgressDetailsAppService : prodAppServiceBase, ILgaEkbProgressDetailsAppService
    {
        private readonly IDapperRepository<LgaEkbProgressDetails, long> _dapperRepo;
        private readonly IRepository<LgaEkbProgressDetails, long> _repo;
        private readonly ILgaEkbProgressDetailsExcelExporter _calendarListExcelExporter;

        public LgaEkbProgressDetailsAppService(IRepository<LgaEkbProgressDetails, long> repo,
                                         IDapperRepository<LgaEkbProgressDetails, long> dapperRepo,
                                        ILgaEkbProgressDetailsExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        //  [AbpAuthorize(AppPermissions.Pages_LogA_Ekb_ProgressDetails_Edit)]
        public async Task CreateOrEdit(CreateOrEditLgaEkbProgressDetailsDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditLgaEkbProgressDetailsDto input)
        {
            var mainObj = ObjectMapper.Map<LgaEkbProgressDetails>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditLgaEkbProgressDetailsDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        //  [AbpAuthorize(AppPermissions.Pages_LogA_Ekb_ProgressDetails_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<LgaEkbProgressDetailsDto>> GetAll(GetLgaEkbProgressDetailsInput input)
        {
            DateTime dateTime = DateTime.Now.Date;

            var filtered = _repo.GetAll()
                .WhereIf(!input.WorkingDateFrom.HasValue && !input.WorkingDateTo.HasValue, t => t.WorkingDate == dateTime)           
                .WhereIf(input.WorkingDateFrom.HasValue, t => input.WorkingDateFrom.Value.Date <= t.WorkingDate)
                .WhereIf(input.WorkingDateTo.HasValue, t => input.WorkingDateTo.Value.Date.AddDays(1) > t.WorkingDate)
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProdLine), e => e.ProdLine.Contains(input.ProdLine))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Shift), e => e.Shift.Contains(input.Shift))
                .WhereIf(!string.IsNullOrWhiteSpace(input.PartNo), e => e.PartNo.Contains(input.PartNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.KanbanSeq), e => e.KanbanSeq.Contains(input.KanbanSeq))
                .WhereIf(!string.IsNullOrWhiteSpace(input.SequenceNo), e => e.SequenceNo.Contains(input.SequenceNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.BodyNo), e => e.BodyNo.Contains(input.BodyNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.LotNo), e => e.LotNo.Contains(input.LotNo))
                .WhereIf(input.NoInLot > 0, e => e.NoInLot == input.NoInLot)
                ;

            var pageAndFiltered = filtered.OrderByDescending(s => s.WorkingDate).ThenBy(s => s.ProdLine).ThenBy(t => t.Shift).ThenBy(a => a.NoInShift);

            var system = from o in pageAndFiltered
                         select new LgaEkbProgressDetailsDto
                         {
                             Id = o.Id,
                             ProdLine = o.ProdLine,
                             WorkingDate = o.WorkingDate,
                             Shift = o.Shift,
                             NoInShift = o.NoInShift,
                             NoInDate = o.NoInDate,
                             ProgressId = o.ProgressId,
                             ProcessId = o.ProcessId,
                             PartListId = o.PartListId,
                             PartNo = o.PartNo,
                             PartNoNormalized = o.PartNoNormalized,
                             BackNo = o.BackNo,
                             PcAddress = o.PcAddress,
                             SpsAddress = o.SpsAddress,
                             //Sorting = o.Sorting,
                             UsageQty = o.UsageQty,
                             SequenceNo = o.SequenceNo,
                             BodyNo = o.BodyNo,
                             LotNo = o.LotNo,
                             NoInLot = o.NoInLot,
                             Grade = o.Grade,
                             Model = o.Model,
                             BodyColor = o.BodyColor,
                             EkbQty = o.EkbQty,
                             RemainQty = o.RemainQty,
                             IsZeroKb = o.IsZeroKb,
                             NewtaktDatetime = o.NewtaktDatetime,
                             PikStartDatetime = o.PikStartDatetime,
                             PikFinishDatetime = o.PikFinishDatetime,
                             DelStartDatetime = o.DelStartDatetime,
                             DelFinishDatetime = o.DelFinishDatetime,
                             KanbanId = o.KanbanId,
                             KanbanSeq = o.KanbanSeq,
                             Status = o.Status,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<LgaEkbProgressDetailsDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }

        public async Task<FileDto> GetProgressDetailsToExcel(GetLgaEkbProgressDetailsInput input)
        {
            DateTime dateTime = DateTime.Now.Date;
            var filtered = _repo.GetAll()
                          .WhereIf(!input.WorkingDateFrom.HasValue && !input.WorkingDateTo.HasValue, t => t.WorkingDate == dateTime)
                          .WhereIf(input.WorkingDateFrom.HasValue, t => input.WorkingDateFrom.Value.Date <= t.WorkingDate)
                          .WhereIf(input.WorkingDateTo.HasValue, t => input.WorkingDateTo.Value.Date.AddDays(1) > t.WorkingDate)
                          .WhereIf(!string.IsNullOrWhiteSpace(input.ProdLine), e => e.ProdLine.Contains(input.ProdLine))
                          .WhereIf(!string.IsNullOrWhiteSpace(input.Shift), e => e.Shift.Contains(input.Shift))
                          .WhereIf(!string.IsNullOrWhiteSpace(input.PartNo), e => e.PartNo.Contains(input.PartNo))
                          .WhereIf(!string.IsNullOrWhiteSpace(input.KanbanSeq), e => e.KanbanSeq.Contains(input.KanbanSeq))
                          .WhereIf(!string.IsNullOrWhiteSpace(input.SequenceNo), e => e.SequenceNo.Contains(input.SequenceNo))
                          .WhereIf(!string.IsNullOrWhiteSpace(input.BodyNo), e => e.BodyNo.Contains(input.BodyNo))
                          .WhereIf(!string.IsNullOrWhiteSpace(input.LotNo), e => e.LotNo.Contains(input.LotNo))
                          .WhereIf(input.NoInLot > 0, e => e.NoInLot == input.NoInLot)
                          ;

            var pageAndFiltered = filtered.OrderByDescending(s => s.WorkingDate).ThenBy(s => s.ProdLine).ThenBy(t => t.Shift).ThenBy(a => a.NoInShift);


            var query = from o in pageAndFiltered
                        select new LgaEkbProgressDetailsDto
                        {
                            Id = o.Id,
                            ProdLine = o.ProdLine,
                            WorkingDate = o.WorkingDate,
                            Shift = o.Shift,
                            NoInShift = o.NoInShift,
                            NoInDate = o.NoInDate,
                            ProgressId = o.ProgressId,
                            ProcessId = o.ProcessId,
                            PartListId = o.PartListId,
                            PartNo = o.PartNo,
                            PartNoNormalized = o.PartNoNormalized,
                            BackNo = o.BackNo,
                            PcAddress = o.PcAddress,
                            SpsAddress = o.SpsAddress,
                            //Sorting = o.Sorting,
                            UsageQty = o.UsageQty,
                            SequenceNo = o.SequenceNo,
                            BodyNo = o.BodyNo,
                            LotNo = o.LotNo,
                            NoInLot = o.NoInLot,
                            Grade = o.Grade,
                            Model = o.Model,
                            BodyColor = o.BodyColor,
                            EkbQty = o.EkbQty,
                            RemainQty = o.RemainQty,
                            IsZeroKb = o.IsZeroKb,
                            NewtaktDatetime = o.NewtaktDatetime,
                            PikStartDatetime = o.PikStartDatetime,
                            PikFinishDatetime = o.PikFinishDatetime,
                            DelStartDatetime = o.DelStartDatetime,
                            DelFinishDatetime = o.DelFinishDatetime,
                            KanbanId = o.KanbanId,
                            KanbanSeq = o.KanbanSeq,
                            Status = o.Status,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }
    }
}