using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using prod.Authorization;
using prod.Dto;
using prod.Master.Common.Dto;
using prod.Master.Common.Exporting;
using prod.Master.Common.Plant.Dto;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Master.Common
{
    [AbpAuthorize(AppPermissions.Pages_InvtSetup_Plant_View)]
    public class MstCmmPlantAppService : prodAppServiceBase, IMstCmmPlantAppService
    {
        private readonly IRepository<MstCmmPlant, long> _repo;
        private readonly IMstCmmPlantExcelExporter _calendarListExcelExporter;


        public MstCmmPlantAppService(IRepository<MstCmmPlant, long> repo,
                                        IMstCmmPlantExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        public async Task<PagedResultDto<MstCmmPlantDto>> GetAll(GetMstCmmPlantInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.PlantName), e => e.PlantName.Contains(input.PlantName))
                .WhereIf(!string.IsNullOrWhiteSpace(input.BranchNo), e => e.BranchNo.Contains(input.BranchNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.AddressLanguageEn), e => e.AddressLanguageEn.Contains(input.AddressLanguageEn));
            var pageAndFiltered = filtered.OrderBy(s => s.Id);

            var system = from o in pageAndFiltered
                         select new MstCmmPlantDto
                         {
                             Id = o.Id,
                             PlantCode = o.PlantCode,
                             PlantName = o.PlantName,
                             BranchNo = o.BranchNo,
                             AddressLanguageEn = o.AddressLanguageEn,
                             AddressLanguageVn = o.AddressLanguageVn,
                             IsActive = o.IsActive
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstCmmPlantDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }

        public async Task<FileDto> GetPlantToExcel(MstCmmPlantExportInput input)
        {
            var query = from o in _repo.GetAll()
                        select new MstCmmPlantDto
                        {
                            Id = o.Id,
                            PlantCode = o.PlantCode,
                            PlantName = o.PlantName,
                            BranchNo = o.BranchNo,
                            AddressLanguageEn = o.AddressLanguageEn,
                            AddressLanguageVn = o.AddressLanguageVn,
                            IsActive = o.IsActive
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }
    }
}
