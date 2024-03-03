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
using prod.LogW.Mwh.Dto;
using prod.LogW.Mwh.Exporting;

namespace prod.LogW.Mwh
{
    //  [AbpAuthorize(AppPermissions.Pages_LogW_Mwh_ModuleCalling)]
    public class LgwMwhModuleCallingAppService : prodAppServiceBase, ILgwMwhModuleCallingAppService
    {
        private readonly IDapperRepository<LgwMwhModuleCalling, long> _dapperRepo;
        private readonly IRepository<LgwMwhModuleCalling, long> _repo;
        private readonly ILgwMwhModuleCallingExcelExporter _calendarListExcelExporter;

        public LgwMwhModuleCallingAppService(IRepository<LgwMwhModuleCalling, long> repo,
                                         IDapperRepository<LgwMwhModuleCalling, long> dapperRepo,
                                        ILgwMwhModuleCallingExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

      

        
      

        public async Task<PagedResultDto<LgwMwhModuleCallingDto>> GetAll(GetLgwMwhModuleCallingInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Renban), e => e.Renban.Contains(input.Renban))
                .WhereIf(!string.IsNullOrWhiteSpace(input.CaseNo), e => e.CaseNo.Contains(input.CaseNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.SupplierNo), e => e.SupplierNo.Contains(input.SupplierNo))
           
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new LgwMwhModuleCallingDto
                         {
                             Id = o.Id,
                             Renban = o.Renban,
                             CaseNo = o.CaseNo,
                             SupplierNo = o.SupplierNo,
                             CallTime = o.CallTime,
                             CalledModuleNo = o.CalledModuleNo,
                             CaseType = o.CaseType,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<LgwMwhModuleCallingDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetModuleCallingToExcel()
        {
            var query = from o in _repo.GetAll()
                        select new LgwMwhModuleCallingDto
                        {
                            Id = o.Id,
                            Renban = o.Renban,
                            CaseNo = o.CaseNo,
                            SupplierNo = o.SupplierNo,
                            CallTime = o.CallTime,
                            CalledModuleNo = o.CalledModuleNo,
                            CaseType = o.CaseType,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        // public async Task GenerateAsync()
        //  {
        //    await _dapperRepo.ExecuteAsync(LgwMwhModuleCallingConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}
