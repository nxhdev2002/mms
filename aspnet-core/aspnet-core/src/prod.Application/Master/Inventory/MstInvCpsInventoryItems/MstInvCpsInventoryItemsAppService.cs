using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using prod.Authorization;
using prod.Dto;
using prod.Master.Inventory.Dto;
using prod.Master.Inventory.Exporting;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Master.Inventory
{
    [AbpAuthorize(AppPermissions.Pages_Master_Inventory_CpsInventoryItems_View)]
    public class MstInvCpsInventoryItemsAppService : prodAppServiceBase, IMstInvCpsInventoryItemsAppService
    {
        private readonly IDapperRepository<MstInvCpsInventoryItems, long> _dapperRepo;
        private readonly IRepository<MstInvCpsInventoryItems, long> _repo;
        private readonly IMstInvCpsInventoryItemsExcelExporter _calendarListExcelExporter;

        public MstInvCpsInventoryItemsAppService(IRepository<MstInvCpsInventoryItems, long> repo,
                                         IDapperRepository<MstInvCpsInventoryItems, long> dapperRepo,
                                        IMstInvCpsInventoryItemsExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        public async Task<PagedResultDto<MstInvCpsInventoryItemsDto>> GetAll(GetMstInvCpsInventoryItemsInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.PartNo), e => e.PartNo.Contains(input.PartNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.PartName), e => e.PartName.Contains(input.PartName))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Color), e => e.Color.Contains(input.Color))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Puom), e => e.Puom.Contains(input.Puom))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstInvCpsInventoryItemsDto
                         {
                             Id = o.Id,
                             PartNo = o.PartNo,
                             PartName = o.PartName,
                             Color = o.Color,
                             Puom = o.Puom,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstInvCpsInventoryItemsDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }

        public async Task<FileDto> GetCpsInventoryItemsToExcel(MstInvCpsInventoryItemsExportInput input)
        {
            var query = from o in _repo.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PartNo),e => e.PartNo.Contains(input.PartNo))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PartName), e => e.PartName.Contains(input.PartName))
                        select new MstInvCpsInventoryItemsDto
                        {
                            Id = o.Id,
                            PartNo = o.PartNo,
                            PartName = o.PartName,
                            Color = o.Color,
                            Puom = o.Puom,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

    }
}
