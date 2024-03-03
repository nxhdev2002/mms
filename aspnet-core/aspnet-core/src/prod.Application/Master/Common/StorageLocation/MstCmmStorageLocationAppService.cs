using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using prod;
using prod.Authorization;
using prod.Dto;
using prod.Master.Common;
using prod.Master.Common.Dto;
using prod.Master.Common.Exporting;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace vovina.Master.Common
{
    [AbpAuthorize(AppPermissions.Pages_InvtSetup_StorageLocation_View)]
    public class MstCmmStorageLocationAppService : prodAppServiceBase, IMstCmmStorageLocationAppService
    {
        private readonly IRepository<MstCmmStorageLocation, long> _repo;
        private readonly IMstCmmStorageLocationExcelExporter _storagelocationListExcelExporter;
        public MstCmmStorageLocationAppService(IRepository<MstCmmStorageLocation, long> repo,
            IMstCmmStorageLocationExcelExporter storagelocationListExcelExporter)

        {
            _storagelocationListExcelExporter = storagelocationListExcelExporter;
            _repo = repo;
        }

        public async Task<PagedResultDto<MstCmmStorageLocationDto>> GetAll(GetMstCmmStorageLocationInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.PlantName), e => e.PlantName.Contains(input.PlantName))
                .WhereIf(!string.IsNullOrWhiteSpace(input.StorageLocationName), e => e.StorageLocationName.Contains(input.StorageLocationName))
                .WhereIf(!string.IsNullOrWhiteSpace(input.AddressLanguageEn), e => e.AddressLanguageEn.Contains(input.AddressLanguageEn))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Category), e => e.Category.Contains(input.Category));

            var pageAndFiltered = filtered.OrderBy(s => s.PlantName);

            var system = from o in pageAndFiltered
                         select new MstCmmStorageLocationDto
                         {
                             Id = o.Id,
                             PlantCode = o.PlantCode,
                             PlantName = o.PlantName,
                             StorageLocation = o.StorageLocation,
                             StorageLocationName = o.StorageLocationName,
                             AddressLanguageEn = o.AddressLanguageEn,
                             AddressLanguageVn = o.AddressLanguageVn,
                             CategoryId = o.CategoryId,
                             Category = o.Category,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstCmmStorageLocationDto>(
                totalCount,
                 await paged.ToListAsync()
            );

        }
        public async Task<FileDto> GetStorageLocationToExcel(GetMstCmmStorageLocationInput input)
        {
            var query = from o in _repo.GetAll()
                        select new MstCmmStorageLocationDto
                        {

                            Id = o.Id,
                            PlantCode = o.PlantCode,
                            PlantName = o.PlantName,
                            StorageLocation = o.StorageLocation,
                            StorageLocationName = o.StorageLocationName,
                            AddressLanguageEn = o.AddressLanguageEn,
                            AddressLanguageVn = o.AddressLanguageVn,
                            CategoryId = o.CategoryId,
                            Category = o.Category,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _storagelocationListExcelExporter.ExportToFile(exportToExcel);
        }

    }
}

