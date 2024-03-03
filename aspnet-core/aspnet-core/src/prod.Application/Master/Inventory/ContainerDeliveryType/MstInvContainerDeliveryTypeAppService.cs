using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using prod.Authorization;
using prod.Dto;
using prod.EntityFrameworkCore;
using prod.Master.Inventory.ContainerDeliveryType;
using prod.Master.Inventory.ContainerDeliveryType.Dto;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Master.Inventory
{
    [AbpAuthorize(AppPermissions.Pages_Master_Ckd_ContainerDeliveryType_View)]
    public class MstInvContainerDeliveryTypeAppService : prodAppServiceBase, IMstInvContainerDeliveryTypeAppService
    {
        private readonly IDapperRepository<MstInvContainerDeliveryType, long> _dapperRepo;
        private readonly IRepository<MstInvContainerDeliveryType, long> _repo;
        private readonly IMstInvContainerDeliveryTypeExcelExporter _calendarListExcelExporter;

        public MstInvContainerDeliveryTypeAppService(IRepository<MstInvContainerDeliveryType, long> repo,
                                         IDapperRepository<MstInvContainerDeliveryType, long> dapperRepo,
                                        IMstInvContainerDeliveryTypeExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        public async Task<PagedResultDto<MstInvContainerDeliveryTypeDto>> GetAll(GetMstInvContainerDeliveryTypeInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Description), e => e.Description.Contains(input.Description))
             ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstInvContainerDeliveryTypeDto
                         {
                             Id = o.Id,
                             Code = o.Code,
                             Description = o.Description,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstInvContainerDeliveryTypeDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetContainerDeliveryTypeToExcel(MstInvContainerDeliveryTypeExportInput input)
        {
            var filtered = _repo.GetAll()
            .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
            .WhereIf(!string.IsNullOrWhiteSpace(input.Description), e => e.Description.Contains(input.Description));

            var pageAndFiltered = filtered.OrderBy(s => s.Id);

            var query = from o in pageAndFiltered
                        select new MstInvContainerDeliveryTypeDto
                        {
                            Id = o.Id,
                            Code = o.Code,
                            Description = o.Description,
                            IsActive = o.IsActive
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

    }
}
