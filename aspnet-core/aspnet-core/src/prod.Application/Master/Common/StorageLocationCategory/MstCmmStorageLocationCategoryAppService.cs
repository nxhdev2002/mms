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
using prod.Master.Common.Dto;
using prod.Master.Common.Exporting;
using prod.Master.Common.Plant.Dto;
using prod.Master.Common.StorageLocationCategory.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;


namespace prod.Master.Common
{
      [AbpAuthorize(AppPermissions.Pages_InvtSetup_StorageLocationCategory_View)]
    public class MstCmmStorageLocationCategoryAppService : prodAppServiceBase, IMstCmmStorageLocationCategoryAppService
    {
        private readonly IRepository<MstCmmStorageLocationCategory, long> _repo;
        private readonly IMstCmmStorageLocationCategoryExcelExporter _calendarListExcelExporter;


        public MstCmmStorageLocationCategoryAppService(IRepository<MstCmmStorageLocationCategory, long> repo
            , IMstCmmStorageLocationCategoryExcelExporter calendarListExcelExporter)
        {
            _repo = repo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        public async Task<PagedResultDto<MstCmmStorageLocationCategoryDto>> GetAll(GetMstCmmStorageLocationCategoryInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.Name.Contains(input.Name))
                .WhereIf(!string.IsNullOrWhiteSpace(input.AreaType), e => e.AreaType.Contains(input.AreaType));
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstCmmStorageLocationCategoryDto
                         {
                             Id = o.Id,
                             Code = o.Code,
                             Name = o.Name,
                             AreaType = o.AreaType,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstCmmStorageLocationCategoryDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }

        public async Task<FileDto> GetStorageToExcel(MstCmmStorageLocationCategoryExportInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.Name.Contains(input.Name))
                .WhereIf(!string.IsNullOrWhiteSpace(input.AreaType), e => e.AreaType.Contains(input.AreaType));
            var pageAndFiltered = filtered.OrderBy(s => s.Id);

            var query = from o in pageAndFiltered
                         select new MstCmmStorageLocationCategoryDto
                        {
                            Id = o.Id,
                            Code = o.Code,
                            Name = o.Name,
                            AreaType = o.AreaType,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

    }
}
