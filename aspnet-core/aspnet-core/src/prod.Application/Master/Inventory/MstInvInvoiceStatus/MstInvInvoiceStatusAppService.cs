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
    [AbpAuthorize(AppPermissions.Pages_Master_Ckd_InvoiceStatus_View)]
    public class MstInvInvoiceStatusAppService : prodAppServiceBase, IMstInvInvoiceStatusAppService
    {
        private readonly IDapperRepository<MstInvInvoiceStatus, long> _dapperRepo;
        private readonly IRepository<MstInvInvoiceStatus, long> _repo;
        private readonly IMstInvInvoiceStatusExcelExporter _calendarListExcelExporter;

        public MstInvInvoiceStatusAppService(IRepository<MstInvInvoiceStatus, long> repo,
                                         IDapperRepository<MstInvInvoiceStatus, long> dapperRepo,
                                        IMstInvInvoiceStatusExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        public async Task<PagedResultDto<MstInvInvoiceStatusDto>> GetAll(GetMstInvInvoiceStatusInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Description), e => e.Description.Contains(input.Description))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstInvInvoiceStatusDto
                         {
                             Id = o.Id,
                             Code = o.Code,
                             Description = o.Description,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstInvInvoiceStatusDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetInvoiceStatusToExcel(MstInvInvoiceStatusExportInput input)
        {
            var filtered = _repo.GetAll()
              .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
              .WhereIf(!string.IsNullOrWhiteSpace(input.Description), e => e.Description.Contains(input.Description))
              ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);

            var query = from o in pageAndFiltered
                        select new MstInvInvoiceStatusDto
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
        //    await _dapperRepo.ExecuteAsync(MstInvInvoiceStatusConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}
