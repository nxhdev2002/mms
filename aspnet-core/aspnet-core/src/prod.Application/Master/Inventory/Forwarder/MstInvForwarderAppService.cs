using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using prod.Authorization;
using prod.Dto;
using prod.Master.Inventory.Forwarder;
using prod.Master.Inventory.Forwarder.Dto;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Master.Inventory
{
    [AbpAuthorize(AppPermissions.Pages_Master_Ckd_Forwarder_View)]
    public class MstInvForwarderAppService : prodAppServiceBase, IMstInvForwarderAppService
    {
        private readonly IDapperRepository<MstInvForwarder, long> _dapperRepo;
        private readonly IRepository<MstInvForwarder, long> _repo;
        private readonly IMstInvForwarderExcelExporter _calendarListExcelExporter;

        public MstInvForwarderAppService(IRepository<MstInvForwarder, long> repo,
                                         IDapperRepository<MstInvForwarder, long> dapperRepo,
                                        IMstInvForwarderExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        public async Task<PagedResultDto<MstInvForwarderDto>> GetAll(GetMstInvForwarderInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.Name.Contains(input.Name))
                .WhereIf(!string.IsNullOrWhiteSpace(input.SupplierNo), e => e.SupplierNo.Contains(input.SupplierNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ShippingNo), e => e.ShippingNo.Contains(input.ShippingNo))
                .WhereIf(input.EfDatefrom.HasValue, t => t.EfDatefrom == input.EfDatefrom.Value)
                .WhereIf(input.EfDateto.HasValue, t => t.EfDateto == input.EfDateto.Value);

            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstInvForwarderDto
                         {
                             Id = o.Id,
                             Code = o.Code,
                             Name = o.Name,
                             SupplierNo = o.SupplierNo,
                             ShippingNo = o.ShippingNo,
                             EfDatefrom = o.EfDatefrom,
                             EfDateto = o.EfDateto,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstInvForwarderDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetForwarderToExcel(MstInvForwarderExportInput input)
        {
            var filtered = _repo.GetAll()
              .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
              .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.Name.Contains(input.Name))
              .WhereIf(!string.IsNullOrWhiteSpace(input.SupplierNo), e => e.SupplierNo.Contains(input.SupplierNo))
              .WhereIf(!string.IsNullOrWhiteSpace(input.ShippingNo), e => e.ShippingNo.Contains(input.ShippingNo))
              .WhereIf(input.EfDatefrom.HasValue, t => t.EfDatefrom == input.EfDatefrom.Value)
              .WhereIf(input.EfDateto.HasValue, t => t.EfDateto == input.EfDateto.Value);

            var pageAndFiltered = filtered.OrderBy(s => s.Id);

            var query = from o in pageAndFiltered
                        select new MstInvForwarderDto
                        {
                            Id = o.Id,
                            Code = o.Code,
                            Name = o.Name,
                            SupplierNo = o.SupplierNo,
                            ShippingNo = o.ShippingNo,
                            EfDatefrom = o.EfDatefrom,
                            EfDateto = o.EfDateto,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

    }
}
