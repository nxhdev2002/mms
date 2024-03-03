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
using prod.Master.LogW.Dto;
using prod.Master.LogW.Exporting;

namespace prod.Master.LogW.EciPartModule
{
    //  [AbpAuthorize(AppPermissions.Pages_Master_LogW_EciPartModule)]
    public class MstLgwEciPartModuleAppService : prodAppServiceBase, IMstLgwEciPartModuleAppService
    {
        private readonly IDapperRepository<MstLgwEciPartModule, long> _dapperRepo;
        private readonly IRepository<MstLgwEciPartModule, long> _repo;
        private readonly IMstLgwEciPartModuleExcelExporter _calendarListExcelExporter;

        public MstLgwEciPartModuleAppService(IRepository<MstLgwEciPartModule, long> repo,
                                         IDapperRepository<MstLgwEciPartModule, long> dapperRepo,
                                        IMstLgwEciPartModuleExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }



        public async Task<PagedResultDto<MstLgwEciPartModuleDto>> GetAll(GetMstLgwEciPartModuleInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.PartNo), e => e.PartNo.Contains(input.PartNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.CaseNo), e => e.CaseNo.Contains(input.CaseNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.SupplierNo), e => e.SupplierNo.Contains(input.SupplierNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ContainerNo), e => e.ContainerNo.Contains(input.ContainerNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Renban), e => e.Renban.Contains(input.Renban))
                .WhereIf(!string.IsNullOrWhiteSpace(input.CasePrefix), e => e.CasePrefix.Contains(input.CasePrefix))
                .WhereIf(!string.IsNullOrWhiteSpace(input.EciType), e => e.EciType.Contains(input.EciType))
                .WhereIf(!string.IsNullOrWhiteSpace(input.IsActive), e => e.IsActive.Contains(input.IsActive))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstLgwEciPartModuleDto
                         {
                             Id = o.Id,
                             PartNo = o.PartNo,
                             CaseNo = o.CaseNo,
                             SupplierNo = o.SupplierNo,
                             ContainerNo = o.ContainerNo,
                             Renban = o.Renban,
                             CasePrefix = o.CasePrefix,
                             ChkEciPartId = o.ChkEciPartId,
                             EciType = o.EciType,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstLgwEciPartModuleDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetEciPartModuleToExcel(MstLgwEciPartModuleExportInput input)
        {
            var filtered = _repo.GetAll()
                 .WhereIf(!string.IsNullOrWhiteSpace(input.PartNo), e => e.PartNo.Contains(input.PartNo))
                 .WhereIf(!string.IsNullOrWhiteSpace(input.CaseNo), e => e.CaseNo.Contains(input.CaseNo))
                 .WhereIf(!string.IsNullOrWhiteSpace(input.SupplierNo), e => e.SupplierNo.Contains(input.SupplierNo))
                 .WhereIf(!string.IsNullOrWhiteSpace(input.ContainerNo), e => e.ContainerNo.Contains(input.ContainerNo))
                 .WhereIf(!string.IsNullOrWhiteSpace(input.Renban), e => e.Renban.Contains(input.Renban))
                 .WhereIf(!string.IsNullOrWhiteSpace(input.CasePrefix), e => e.CasePrefix.Contains(input.CasePrefix))
                 .WhereIf(!string.IsNullOrWhiteSpace(input.EciType), e => e.EciType.Contains(input.EciType))
                 .WhereIf(!string.IsNullOrWhiteSpace(input.IsActive), e => e.IsActive.Contains(input.IsActive))
                 ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var query = from o in pageAndFiltered
                         select new MstLgwEciPartModuleDto
                        {
                            Id = o.Id,
                            PartNo = o.PartNo,
                            CaseNo = o.CaseNo,
                            SupplierNo = o.SupplierNo,
                            ContainerNo = o.ContainerNo,
                            Renban = o.Renban,
                            CasePrefix = o.CasePrefix,
                            ChkEciPartId = o.ChkEciPartId,
                            EciType = o.EciType,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        // public async Task GenerateAsync()
        //  {
        //    await _dapperRepo.ExecuteAsync(MstLgwEciPartModuleConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}
