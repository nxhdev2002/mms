using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using prod.Assy.Andon.Dto;
using prod.Assy.Andon.Exporting;
using prod.Authorization;
using prod.Dto;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Assy.Andon
{
    [AbpAuthorize(AppPermissions.Pages_ProdPlan_AssemblyData_View)]
    public class AsyAdoAssemblyDataAppService : prodAppServiceBase, IAsyAdoAssemblyDataAppService
    {
        private readonly IDapperRepository<AsyAdoAssemblyData, long> _dapperRepo;
        private readonly IRepository<AsyAdoAssemblyData, long> _repo;
        private readonly IAsyAdoAssemblyDataExcelExporter _calendarListExcelExporter;

        public AsyAdoAssemblyDataAppService(IRepository<AsyAdoAssemblyData, long> repo,
                                         IDapperRepository<AsyAdoAssemblyData, long> dapperRepo,
                                        IAsyAdoAssemblyDataExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        public async Task<PagedResultDto<AsyAdoAssemblyDataDto>> GetAll(GetAsyAdoAssemblyDataInput input)
        {
            DateTime dateTime = DateTime.UtcNow.Date;

            var filtered = _repo.GetAll()
                .WhereIf(!input.WorkingDateFrom.HasValue && !input.WorkingDateTo.HasValue, t => t.WorkingDate == dateTime)
                .WhereIf(input.WorkingDateFrom.HasValue, t => input.WorkingDateFrom.Value.Date <= t.WorkingDate)
                .WhereIf(input.WorkingDateTo.HasValue, t => input.WorkingDateTo.Value.Date.AddDays(1) > t.WorkingDate)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Line), e => e.Line.Contains(input.Line))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Shift), e => e.Shift.Contains(input.Shift))
                .WhereIf(!string.IsNullOrWhiteSpace(input.SeqNo), e => e.SeqNo.Contains(input.SeqNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.LotNo), e => e.LotNo.Contains(input.LotNo))
                .WhereIf(input.NoInShift != null, e => e.NoInShift == input.NoInShift)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Body), e => e.Body.Contains(input.Body))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Process), e => e.Process.Contains(input.Process));

            var pageAndFiltered = filtered.OrderByDescending(s => s.WorkingDate).ThenBy(s => s.Line).ThenBy(t => t.Shift).ThenBy(t => t.NoInDate).ThenBy(t => t.CreationTime);

            var system = from o in pageAndFiltered
                         select new AsyAdoAssemblyDataDto
                         {
                             Id = o.Id,
                             WorkingDate = o.WorkingDate,
                             NoInDate = o.NoInDate,
                             Shift = o.Shift,
                             Line = o.Line,
                             Process = o.Process,
                             Model = o.Model,
                             Body = o.Body,
                             SeqNo = o.SeqNo,
                             Grade = o.Grade,
                             LotNo = o.LotNo,
                             NoInLot = o.NoInLot,
                             LotNoIndex = o.LotNoIndex,
                             NoInShift = o.NoInShift,
                             Color = o.Color,
                             CreateDate = o.CreateDate

                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<AsyAdoAssemblyDataDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetAssemblyDataToExcel(GetAsyAdoAssemblyDataInput input)
        {
            DateTime dateTime = DateTime.UtcNow.Date;

            var filtered = _repo.GetAll()
                .WhereIf(!input.WorkingDateFrom.HasValue && !input.WorkingDateTo.HasValue, t => t.WorkingDate == dateTime)
                .WhereIf(input.WorkingDateFrom.HasValue, t => input.WorkingDateFrom.Value.Date <= t.WorkingDate)
                .WhereIf(input.WorkingDateTo.HasValue, t => input.WorkingDateTo.Value.Date.AddDays(1) > t.WorkingDate)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Line), e => e.Line.Contains(input.Line))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Shift), e => e.Shift.Contains(input.Shift))
                .WhereIf(!string.IsNullOrWhiteSpace(input.SeqNo), e => e.SeqNo.Contains(input.SeqNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.LotNo), e => e.LotNo.Contains(input.LotNo))
                .WhereIf(input.NoInShift != null, e => e.NoInShift == input.NoInShift)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Body), e => e.Body.Contains(input.Body))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Process), e => e.Process.Contains(input.Process));

            var pageAndFiltered = filtered.OrderByDescending(s => s.WorkingDate).ThenBy(s => s.Line).ThenBy(t => t.Shift).ThenBy(t => t.NoInDate).ThenBy(t => t.CreationTime);

            var system = from o in pageAndFiltered
                         select new AsyAdoAssemblyDataDto
                         {
                             Id = o.Id,
                             WorkingDate = o.WorkingDate,
                             NoInDate = o.NoInDate,
                             Shift = o.Shift,
                             Line = o.Line,
                             Process = o.Process,
                             Model = o.Model,
                             Body = o.Body,
                             SeqNo = o.SeqNo,
                             Grade = o.Grade,
                             LotNo = o.LotNo,
                             NoInLot = o.NoInLot,
                             LotNoIndex = o.LotNoIndex,
                             NoInShift = o.NoInShift,
                             Color = o.Color,
                             CreateDate = o.CreateDate

                         };
            var exportToExcel = await system.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }


    }
}
