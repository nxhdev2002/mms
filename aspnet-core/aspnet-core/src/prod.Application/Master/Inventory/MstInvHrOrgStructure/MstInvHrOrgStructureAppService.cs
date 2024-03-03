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
using prod.Master.Inventory.Dto;
using prod.Master.Inventory.Exporting;

namespace prod.Master.Inventory
{
    [AbpAuthorize(AppPermissions.Pages_Master_Hr_HrOrgStructure_View)]
    public class MstInvHrOrgStructureAppService : prodAppServiceBase, IMstInvHrOrgStructureAppService
    {
        private readonly IDapperRepository<MstInvHrOrgStructure, System.Guid> _dapperRepo;
        private readonly IRepository<MstInvHrOrgStructure, System.Guid> _repo;
        private readonly IMstInvHrOrgStructureExcelExporter _calendarListExcelExporter;

        public MstInvHrOrgStructureAppService(IRepository<MstInvHrOrgStructure, System.Guid> repo,
                                         IDapperRepository<MstInvHrOrgStructure, System.Guid> dapperRepo,
                                        IMstInvHrOrgStructureExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        public async Task<PagedResultDto<MstInvHrOrgStructureDto>> GetAll(GetMstInvHrOrgStructureInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.Name.Contains(input.Name));
            var pageAndFiltered = filtered.OrderBy(s =>s.Code).ThenBy(s => s.Name);


            var system = from o in pageAndFiltered
                         select new MstInvHrOrgStructureDto
                         {
                             Id =  o.Id.ToString(),
                             Code = o.Code,
                             Name = o.Name,
                             Description = o.Description,
                             Published = o.Published,
                             Orgstructuretypename = o.Orgstructuretypename,
                             Orgstructuretypecode = o.Orgstructuretypecode,
                             Parentid = o.Parentid,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstInvHrOrgStructureDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetHrOrgStructureToExcel(MstInvHrOrgStructureExportInput input)
        {
            var filtered = _repo.GetAll()
              /*     .WhereIf(!string.IsNullOrWhiteSpace(input.Hrid), e => e.Hrid.Contains(input.Hrid))*/
                     .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                     .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.Name.Contains(input.Name))
              /*  .WhereIf(!string.IsNullOrWhiteSpace(input.Description), e => e.Description.Contains(input.Description))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Orgstructuretypename), e => e.Orgstructuretypename.Contains(input.Orgstructuretypename))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Orgstructuretypecode), e => e.Orgstructuretypecode.Contains(input.Orgstructuretypecode))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Parentid), e => e.Parentid.Contains(input.Parentid))
                .WhereIf(!string.IsNullOrWhiteSpace(input.IsActive), e => e.IsActive.Contains(input.IsActive))*/
              ;
            var pageAndFiltered = filtered.OrderBy(s => s.Code).ThenBy(s => s.Name);

            var query = from o in pageAndFiltered
                        select new MstInvHrOrgStructureDto
                        {
                            Id = o.Id.ToString(),
                            Code = o.Code,
                            Name = o.Name,
                            Description = o.Description,
                            Published = o.Published,
                            Orgstructuretypename = o.Orgstructuretypename,
                            Orgstructuretypecode = o.Orgstructuretypecode,
                            Parentid = o.Parentid,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        // public async Task GenerateAsync()
        //  {
        //    await _dapperRepo.ExecuteAsync(MstInvHrOrgStructureConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}
