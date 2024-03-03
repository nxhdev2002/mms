using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using prod.Authorization;
using prod.Dto;
using prod.Master.Inventory.Dto;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Master.Inventory
{
    [AbpAuthorize(AppPermissions.Pages_Master_Ckd_CustomsStatus_View)]
    public class MstInvCustomsStatusAppService : prodAppServiceBase, IMstInvCustomsStatusAppService
    {
        private readonly IDapperRepository<MstInvCustomsStatus, long> _dapperRepo;
        private readonly IRepository<MstInvCustomsStatus, long> _repo;
        private readonly IMstInvCustomsStatusExcelExporter _calendarListExcelExporter;

        public MstInvCustomsStatusAppService(IRepository<MstInvCustomsStatus, long> repo,
                                         IDapperRepository<MstInvCustomsStatus, long> dapperRepo,
                                        IMstInvCustomsStatusExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        public async Task<PagedResultDto<MstInvCustomsStatusDto>> GetAll(GetMstInvCustomsStatusInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code));

            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstInvCustomsStatusDto
                         {
                             Id = o.Id,
                             Code = o.Code,
                             Description = o.Description,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstInvCustomsStatusDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetCustomsStatusToExcel(GetMstInvCustomsStatusInput input)
        {
            var filtered = _repo.GetAll()
               .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code));
            ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);

            var query = from o in pageAndFiltered
                        select new MstInvCustomsStatusDto
                        {
                            Id = o.Id,
                            Code = o.Code,
                            Description = o.Description,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        // public async Task GenerateAsync()
        //  {
        //    await _dapperRepo.ExecuteAsync(MstInvCustomsStatusConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}
