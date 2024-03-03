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
    //  [AbpAuthorize(AppPermissions.Pages_Welding_Andon_WeldingSignal)]
    public class WldAdoWeldingSignalAppService : prodAppServiceBase, IWldAdoWeldingSignalAppService
    {
        private readonly IDapperRepository<WldAdoWeldingSignal, long> _dapperRepo;
        private readonly IRepository<WldAdoWeldingSignal, long> _repo;
        private readonly IWldAdoWeldingSignalExcelExporter _calendarListExcelExporter;

        public WldAdoWeldingSignalAppService(IRepository<WldAdoWeldingSignal, long> repo,
                                         IDapperRepository<WldAdoWeldingSignal, long> dapperRepo,
                                        IWldAdoWeldingSignalExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        //  [AbpAuthorize(AppPermissions.Pages_Welding_Andon_WeldingSignal_Edit)]
        public async Task CreateOrEdit(CreateOrEditWldAdoWeldingSignalDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditWldAdoWeldingSignalDto input)
        {
            var mainObj = ObjectMapper.Map<WldAdoWeldingSignal>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditWldAdoWeldingSignalDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        //  [AbpAuthorize(AppPermissions.Pages_Welding_Andon_WeldingSignal_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<WldAdoWeldingSignalDto>> GetAll(GetWldAdoWeldingSignalInput input)
        {
            DateTime dateTime = DateTime.Now.Date;
            var filtered = _repo.GetAll()
                .WhereIf(!input.SignalDateFrom.HasValue && !input.SignalDateTo.HasValue, t => t.SignalDate.Value.Date == dateTime)
                .WhereIf(input.SignalDateFrom.HasValue, t => t.SignalDate.Value.Date >= input.SignalDateFrom.Value.Date)
                .WhereIf(input.SignalDateTo.HasValue, t => input.SignalDateTo.Value.Date >= t.SignalDate.Value.Date)
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProdLine), e => e.ProdLine.Contains(input.ProdLine))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Process), e => e.Process.Contains(input.Process))
                .WhereIf(!string.IsNullOrWhiteSpace(input.SignalType), e => e.SignalType.Contains(input.SignalType));

            var pageAndFiltered = filtered.OrderByDescending(s => s.SignalDate);

            var system = from o in pageAndFiltered
                         select new WldAdoWeldingSignalDto
                         {
                             Id = o.Id,
                             ProdLine = o.ProdLine,
                             Process = o.Process,
                             SignalType = o.SignalType,
                             SignalBy = o.SignalBy,
                             SignalDate = o.SignalDate,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<WldAdoWeldingSignalDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetWeldingSignalToExcel(GetWldAdoWeldingSignalExportInput input)
        {
            DateTime dateTime = DateTime.Now.Date;
            var filtered = _repo.GetAll()
                .WhereIf(!input.SignalDateFrom.HasValue && !input.SignalDateTo.HasValue, t => t.SignalDate.Value.Date == dateTime)
                .WhereIf(input.SignalDateFrom.HasValue, t => t.SignalDate.Value.Date >= input.SignalDateFrom.Value.Date)
                .WhereIf(input.SignalDateTo.HasValue, t => input.SignalDateTo.Value.Date >= t.SignalDate.Value.Date)
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProdLine), e => e.ProdLine.Contains(input.ProdLine))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Process), e => e.Process.Contains(input.Process))
                .WhereIf(!string.IsNullOrWhiteSpace(input.SignalType), e => e.SignalType.Contains(input.SignalType));

            var pageAndFiltered = filtered.OrderByDescending(s => s.SignalDate);

            var query = from o in pageAndFiltered
                        select new WldAdoWeldingSignalDto
                        {
                            Id = o.Id,
                            ProdLine = o.ProdLine,
                            Process = o.Process,
                            SignalType = o.SignalType,
                            SignalBy = o.SignalBy,
                            SignalDate = o.SignalDate,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }
    }
}
