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
using prod.Authorization;
using prod.Dto;
using prod.EntityFrameworkCore;
using prod.Master.LogA.Dto;
using prod.Master.LogA.Exporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;


namespace prod.Master.LogA
{
    //  [AbpAuthorize(AppPermissions.Pages_Master_LogA_PlcSignal)]
    public class MstLgaPlcSignalAppService : prodAppServiceBase, IMstLgaPlcSignalAppService
    {
        private readonly IDapperRepository<MstLgaPlcSignal, long> _dapperRepo;
        private readonly IRepository<MstLgaPlcSignal, long> _repo;
        private readonly IMstLgaPlcSignalExcelExporter _calendarListExcelExporter;

        public MstLgaPlcSignalAppService(IRepository<MstLgaPlcSignal, long> repo,
                                         IDapperRepository<MstLgaPlcSignal, long> dapperRepo,
                                        IMstLgaPlcSignalExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_LogA_PlcSignal_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstLgaPlcSignalDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstLgaPlcSignalDto input)
        {
            var mainObj = ObjectMapper.Map<MstLgaPlcSignal>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstLgaPlcSignalDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_LogA_PlcSignal_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstLgaPlcSignalDto>> GetAll(GetMstLgaPlcSignalInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.SignalPattern), e => e.SignalPattern.Contains(input.SignalPattern))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProdLine), e => e.ProdLine.Contains(input.ProdLine))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Process), e => e.Process.Contains(input.Process))               
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstLgaPlcSignalDto
                         {
                             Id = o.Id,
                             SignalIndex = o.SignalIndex,
                             SignalPattern = o.SignalPattern,
                             ProdLine = o.ProdLine,
                             Process = o.Process,
                             SubProcess = o.SubProcess,
                             SignalCode = o.SignalCode,
                             SignalDescription = o.SignalDescription,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstLgaPlcSignalDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetPlcSignalToExcel(GetMstLgaPlcSignalExcelInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.SignalPattern), e => e.SignalPattern.Contains(input.SignalPattern))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProdLine), e => e.ProdLine.Contains(input.ProdLine))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Process), e => e.Process.Contains(input.Process))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var query = from o in pageAndFiltered
                         select new MstLgaPlcSignalDto
                        {
                            Id = o.Id,
                            SignalIndex = o.SignalIndex,
                            SignalPattern = o.SignalPattern,
                            ProdLine = o.ProdLine,
                            Process = o.Process,
                            SubProcess = o.SubProcess,
                            SignalCode = o.SignalCode,
                            SignalDescription = o.SignalDescription,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        // public async Task GenerateAsync()
        //  {
        //    await _dapperRepo.ExecuteAsync(MstLgaPlcSignalConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}
