
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
using prod.Master.Common.Dto;
using prod.Master.Common.Exporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Master.Common.MaterialType
{
    [AbpAuthorize(AppPermissions.Pages_InvtSetup_MaterialType_View)]
    public class MstCmmMaterialTypeAppService : prodAppServiceBase, IMstCmmMaterialTypeAppService
    {
        private readonly IDapperRepository<MstCmmMaterialType, long> _dapperRepository;
        private readonly IRepository<MstCmmMaterialType, long> _repo;
        private readonly IMstCmmMaterialTypeExcelExporter _calendarListExcelExporter;
        public MstCmmMaterialTypeAppService(IRepository<MstCmmMaterialType, long> repo,
            IDapperRepository<MstCmmMaterialType, long> dapperRepository,
                                        IMstCmmMaterialTypeExcelExporter calendarListExcelExporter)
        {
            _repo = repo;
            _dapperRepository = dapperRepository;
            _calendarListExcelExporter = calendarListExcelExporter;
        }
        public async Task<PagedResultDto<MstCmmMaterialTypeDto>> GetAll(GetMstCmmMaterialTypeInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.Name.Contains(input.Name))
                .WhereIf(!string.IsNullOrWhiteSpace(input.IsActive), e => e.IsActive.Contains(input.IsActive))
            ;

            var pageAndFiltered = filtered.OrderByDescending(x => x.Id);

            var system = from o in pageAndFiltered
                         select new MstCmmMaterialTypeDto

                         {
                             Id = o.Id,
                             Name = o.Name,
                             Code = o.Code,
                             IsActive = o.IsActive
                         };

            var totalCount = await filtered.CountAsync();
			var paged = system.PageBy(input);

            return new PagedResultDto<MstCmmMaterialTypeDto>(
                 totalCount,
                 await paged.ToListAsync()
             );
        } 

        public async Task<FileDto> GetMaterialTypeToExcel(MstCmmMaterialTypeExportInput input)
        {
            var filtered = _repo.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.Name.Contains(input.Name))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code));

            var pageAndFiltered = filtered.OrderBy(s => s.Id);
            var system = from o in pageAndFiltered
                         select new MstCmmMaterialTypeDto
                        {
                            Id = o.Id,
                            Code = o.Code,
                            Name = o.Name,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await system.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }
    }
}
