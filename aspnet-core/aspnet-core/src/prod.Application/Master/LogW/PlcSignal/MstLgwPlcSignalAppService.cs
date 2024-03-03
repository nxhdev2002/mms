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
using prod.Master.LogW.Dto;
using prod.Master.LogW.Exporting;

namespace prod.Master.LogW
{
    //  [AbpAuthorize(AppPermissions.Pages_Master_LogW_PlcSignal)]
    public class MstLgwPlcSignalAppService : prodAppServiceBase, IMstLgwPlcSignalAppService
    {
        private readonly IDapperRepository<MstLgwPlcSignal, long> _dapperRepo;
        private readonly IRepository<MstLgwPlcSignal, long> _repo;
        private readonly IMstLgwPlcSignalExcelExporter _calendarListExcelExporter;

        public MstLgwPlcSignalAppService(IRepository<MstLgwPlcSignal, long> repo,
                                         IDapperRepository<MstLgwPlcSignal, long> dapperRepo,
                                        IMstLgwPlcSignalExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        ////  [AbpAuthorize(AppPermissions.Pages_Master_LogW_PlcSignal_Edit)]
        //public async Task CreateOrEdit(CreateOrEditMstLgwPlcSignalDto input)
        //{
        //    if (input.Id == null) await Create(input);
        //    else await Update(input);
        //}

        ////CREATE
        //private async Task Create(CreateOrEditMstLgwPlcSignalDto input)
        //{
        //    var mainObj = ObjectMapper.Map<MstLgwPlcSignal>(input);

        //    await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        //}

        //// EDIT
        //private async Task Update(CreateOrEditMstLgwPlcSignalDto input)
        //{
        //    using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
        //    {
        //        var mainObj = await _repo.GetAll()
        //        .FirstOrDefaultAsync(e => e.Id == input.Id);

        //        var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
        //    }
        //}

        ////  [AbpAuthorize(AppPermissions.Pages_Master_LogW_PlcSignal_Delete)]
        //public async Task Delete(EntityDto input)
        //{
        //    var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
        //    CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        //}

        public async Task<PagedResultDto<MstLgwPlcSignalDto>> GetAll(GetMstLgwPlcSignalInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProdLine), e => e.ProdLine.Contains(input.ProdLine))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Process), e => e.Process.Contains(input.Process))
                .WhereIf(!string.IsNullOrWhiteSpace(input.SignalCode), e => e.SignalCode.Contains(input.SignalCode))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstLgwPlcSignalDto
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

            return new PagedResultDto<MstLgwPlcSignalDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetPlcSignalToExcel(MstLgwPlcSignalExportInput input)
        {
            var filtered = _repo.GetAll()
               .WhereIf(!string.IsNullOrWhiteSpace(input.ProdLine), e => e.ProdLine.Contains(input.ProdLine))
               .WhereIf(!string.IsNullOrWhiteSpace(input.Process), e => e.Process.Contains(input.Process))
               .WhereIf(!string.IsNullOrWhiteSpace(input.SignalCode), e => e.SignalCode.Contains(input.SignalCode))
               ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var query = from o in pageAndFiltered
                       //Where(s => s.IsDeleted == false)

                        select new MstLgwPlcSignalDto
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
        //    await _dapperRepo.ExecuteAsync(MstLgwPlcSignalConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}

