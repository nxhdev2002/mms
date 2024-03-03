using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using prod.Authorization;
using prod.Dto;
using prod.EntityFrameworkCore;
using prod.LogW.Ado;
using prod.LogW.Ado.Dto;
using prod.LogW.Ado.Exporting;
using System;

namespace prod.Master.Ado
{
   //  [AbpAuthorize(AppPermissions.Pages_LogW_Ado_CallingLightStatus)]
    public class LgwAdoCallingLightStatusAppService : prodAppServiceBase, ILgwAdoCallingLightStatusAppService
    {
        private readonly IDapperRepository<LgwAdoCallingLightStatus, long> _dapperRepo;
        private readonly IRepository<LgwAdoCallingLightStatus, long> _repo;
        private readonly ILgwAdoCallingLightStatusExcelExporter _calendarListExcelExporter;

        public LgwAdoCallingLightStatusAppService(IRepository<LgwAdoCallingLightStatus, long> repo,
                                         IDapperRepository<LgwAdoCallingLightStatus, long> dapperRepo,
                                        ILgwAdoCallingLightStatusExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        //  [AbpAuthorize(AppPermissions.Pages_LogW_Ado_CallingLightStatus_Edit)]
        public async Task CreateOrEdit(CreateOrEditLgwAdoCallingLightStatusDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditLgwAdoCallingLightStatusDto input)
        {
            var mainObj = ObjectMapper.Map<LgwAdoCallingLightStatus>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditLgwAdoCallingLightStatusDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        //  [AbpAuthorize(AppPermissions.Pages_LogW_Ado_CallingLightStatus_Edit)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<LgwAdoCallingLightStatusDto>> GetAll(GetLgwAdoCallingLightStatusInput input)
        {
            DateTime dateTime = DateTime.UtcNow.Date;

            var filtered = _repo.GetAll()
                .WhereIf(!input.WorkingDateFrom.HasValue && !input.WorkingDateTo.HasValue, t => t.WorkingDate == dateTime)
                .WhereIf(input.WorkingDateFrom.HasValue, t => input.WorkingDateFrom.Value.Date <= t.WorkingDate)
                .WhereIf(input.WorkingDateTo.HasValue, t => input.WorkingDateTo.Value.Date.AddDays(1) > t.WorkingDate)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProdLine), e => e.ProdLine.Contains(input.ProdLine))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Shift), e => e.Shift.Contains(input.Shift));

            var pageAndFiltered = filtered.OrderByDescending(s => s.WorkingDate).ThenBy(n => n.ProdLine).ThenBy(n => n.Sorting);

            var system = from o in pageAndFiltered
                         select new LgwAdoCallingLightStatusDto
                         {
                             Id = o.Id,
                             Code = o.Code,
                             LightName = o.LightName,
                             ProdLine = o.ProdLine,
                             Process = o.Process,
                             BlockCode = o.BlockCode,
                             BlockDescription = o.BlockDescription,
                             Sorting = o.Sorting,
                             SignalId = o.SignalId,
                             SignalCode = o.SignalCode,
                             StartDate = o.StartDate,
                             FinshDate = o.FinshDate,
                             Status = o.Status,
                             WorkingDate = o.WorkingDate,
                             Shift = o.Shift,
                             NoInDate = o.NoInDate,
                             NoInShift = o.NoInShift,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<LgwAdoCallingLightStatusDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetCallingLightStatusToExcel(GetLgwAdoCallingLightStatusInput input)
        {
            DateTime dateTime = DateTime.UtcNow.Date;

            var filtered = _repo.GetAll()
                  .WhereIf(!input.WorkingDateFrom.HasValue && !input.WorkingDateTo.HasValue, t => t.WorkingDate == dateTime)
                  .WhereIf(input.WorkingDateFrom.HasValue, t => input.WorkingDateFrom.Value.Date <= t.WorkingDate)
                  .WhereIf(input.WorkingDateTo.HasValue, t => input.WorkingDateTo.Value.Date.AddDays(1) > t.WorkingDate)
                  .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                  .WhereIf(!string.IsNullOrWhiteSpace(input.ProdLine), e => e.ProdLine.Contains(input.ProdLine))
                  .WhereIf(!string.IsNullOrWhiteSpace(input.Shift), e => e.Shift.Contains(input.Shift));

            var pageAndFiltered = filtered.OrderByDescending(s => s.WorkingDate).ThenBy(n => n.ProdLine).ThenBy(n => n.Sorting);

            var query = from o in pageAndFiltered
                        select new LgwAdoCallingLightStatusDto
                        {
                            Id = o.Id,
                            Code = o.Code,
                            LightName = o.LightName,
                            ProdLine = o.ProdLine,
                            Process = o.Process,
                            BlockCode = o.BlockCode,
                            BlockDescription = o.BlockDescription,
                            Sorting = o.Sorting,
                            SignalId = o.SignalId,
                            SignalCode = o.SignalCode,
                            StartDate = o.StartDate,
                            FinshDate = o.FinshDate,
                            Status = o.Status,
                            WorkingDate = o.WorkingDate,
                            Shift = o.Shift,
                            NoInDate = o.NoInDate,
                            NoInShift = o.NoInShift,
                };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }
        public async Task<List<LgwAdoCallingLightStatusDto>> GetCallingLightData(string prod_line)
        {
            string _sql = "Exec LGW_ADO_CALLING_LIGHT_STATUS_GET_DATA @prod_line";

            var _sqldata = await _dapperRepo.QueryAsync<LgwAdoCallingLightStatusDto>(_sql, new
            {
                prod_line = prod_line
            });

            return _sqldata.ToList();
        }
        public async Task<List<LgwAdoCallingLightStatusDto>> UpdateCallingLightData(string progress_id)
        {

            string _sql = "Exec LGW_ADO_CALLING_LIGHT_STATUS_START_FINISH @progress_id";

            var filtered = await _dapperRepo.QueryAsync<LgwAdoCallingLightStatusDto>(_sql, new
            {
                progress_id = progress_id
            });

            return filtered.ToList();
        }
    }
}

