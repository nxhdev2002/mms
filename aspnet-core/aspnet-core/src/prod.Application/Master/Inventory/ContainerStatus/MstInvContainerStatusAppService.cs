using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using prod.Authorization;
using prod.Dto;
using prod.Master.Inventory.ContainerStatus;
using prod.Master.Inventory.ContainerStatus.Dto;
using prod.Master.Inventory.Exporting;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Master.Inventory
{
    [AbpAuthorize(AppPermissions.Pages_Master_Ckd_ContainerStatus_View)]
    public class MstInvContainerStatusAppService : prodAppServiceBase, IMstInvContainerStatusAppService
    {
        private readonly IDapperRepository<MstInvContainerStatus, long> _dapperRepo;
        private readonly IRepository<MstInvContainerStatus, long> _repo;
        private readonly IMstInvContainerStatusExcelExporter _calendarListExcelExporter;

        public MstInvContainerStatusAppService(IRepository<MstInvContainerStatus, long> repo,
                                         IDapperRepository<MstInvContainerStatus, long> dapperRepo,
                                        IMstInvContainerStatusExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        public async Task<PagedResultDto<MstInvContainerStatusDto>> GetAll(GetMstInvContainerStatusInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Description), e => e.Description.Contains(input.Description));
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstInvContainerStatusDto
                         {
                             Id = o.Id,
                             Code = o.Code,
                             Description = o.Description,
                             DescriptionVn = o.DescriptionVn,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstInvContainerStatusDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetContainerStatusToExcel(MstInvContainerStatusExportInput input)
        {
            var filtered = _repo.GetAll()
             .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
             .WhereIf(!string.IsNullOrWhiteSpace(input.Description), e => e.Description.Contains(input.Description));
            var pageAndFiltered = filtered.OrderBy(s => s.Id);

            var query = from o in pageAndFiltered
                        select new MstInvContainerStatusDto
                        {
                            Id = o.Id,
                            Code = o.Code,
                            Description = o.Description,
                            DescriptionVn = o.DescriptionVn,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

    }
}
