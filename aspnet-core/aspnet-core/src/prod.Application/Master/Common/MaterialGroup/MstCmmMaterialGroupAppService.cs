using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using prod.Authorization;
using prod.Dto;
using prod.Master.Common.Dto;
using prod.Master.Common.Exporting;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Master.Common
{
    [AbpAuthorize(AppPermissions.Pages_InvtSetup_MaterialGroup_View)]
    public class MstCmmMaterialGroupAppService : prodAppServiceBase, IMstCmmMaterialGroupAppService
    {
        private readonly IDapperRepository<MstCmmMaterialGroup, long> _dapperRepo;
        private readonly IRepository<MstCmmMaterialGroup, long> _repo;
        private readonly IMstCmmMaterialGroupExcelExporter _calendarListExcelExporter;

        public MstCmmMaterialGroupAppService(IRepository<MstCmmMaterialGroup, long> repo,
                                         IDapperRepository<MstCmmMaterialGroup, long> dapperRepo,
                                        IMstCmmMaterialGroupExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        public async Task<PagedResultDto<MstCmmMaterialGroupDto>> GetAll(GetMstCmmMaterialGroupInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code));
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstCmmMaterialGroupDto
                         {
                             Id = o.Id,
                             Code = o.Code,
                             Name = o.Name,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstCmmMaterialGroupDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetMaterialGroupToExcel(MstCmmMaterialGroupExportInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code));
            var pageAndFiltered = filtered.OrderBy(s => s.Id);

            var query = from o in pageAndFiltered
                        select new MstCmmMaterialGroupDto
                        {
                            Id = o.Id,
                            Code = o.Code,
                            Name = o.Name,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

    }
}
