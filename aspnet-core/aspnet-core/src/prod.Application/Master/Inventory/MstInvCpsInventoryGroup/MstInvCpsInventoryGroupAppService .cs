using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using prod;
using prod.Authorization;
using prod.Dto;
using prod.Master.Inventory;
using prod.Master.Inventory.Dto;
using prod.Master.Inventory.Exporting;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Master.Inventory
{
    [AbpAuthorize(AppPermissions.Pages_Master_Cps_CpsInventoryGroup_View)]
    public class MstInvCpsInventoryGroupAppService : prodAppServiceBase, IMstInvCpsInventoryGroupAppService
    {
        private readonly IDapperRepository<MstInvCpsInventoryGroup, long> _dapperRepo;
        private readonly IRepository<MstInvCpsInventoryGroup, long> _repo;
        private readonly IMstInvCpsInventoryGroupExcelExporter _calendarListExcelExporter;

        public MstInvCpsInventoryGroupAppService(IRepository<MstInvCpsInventoryGroup, long> repo,
                                         IDapperRepository<MstInvCpsInventoryGroup, long> dapperRepo,
                                        IMstInvCpsInventoryGroupExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }


        public async Task<PagedResultDto<MstInvCpsInventoryGroupDto>> GetAll(GetMstInvCpsInventoryGroupInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Productgroupcode), e => e.Productgroupcode.Contains(input.Productgroupcode))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Productgroupname), e => e.Productgroupname.Contains(input.Productgroupname))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstInvCpsInventoryGroupDto
                         {
                             Id = o.Id,
                             Productgroupcode = o.Productgroupcode,
                             Productgroupname = o.Productgroupname,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstInvCpsInventoryGroupDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetCpsInventoryGroupToExcel(MstInvCpsInventoryGroupExportInput input)
        {
            var query = from o in _repo.GetAll()
                 .WhereIf(!string.IsNullOrWhiteSpace(input.Productgroupcode), e => e.Productgroupcode.Contains(input.Productgroupcode))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Productgroupname), e => e.Productgroupname.Contains(input.Productgroupname))

                        select new MstInvCpsInventoryGroupDto
                        {
                            Id = o.Id,
                            Productgroupcode = o.Productgroupcode,
                            Productgroupname = o.Productgroupname,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

    }
}
