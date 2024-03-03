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
    [AbpAuthorize(AppPermissions.Pages_Master_Cps_CpsSuppliers_View)]
    public class MstInvCpsSuppliersAppService : prodAppServiceBase, IMstInvCpsSuppliersAppService
    {
        private readonly IDapperRepository<MstInvCpsSuppliers, long> _dapperRepo;
        private readonly IRepository<MstInvCpsSuppliers, long> _repo;
        private readonly IMstInvCpsSuppliersExcelExporter _calendarListExcelExporter;

        public MstInvCpsSuppliersAppService(IRepository<MstInvCpsSuppliers, long> repo,
                                         IDapperRepository<MstInvCpsSuppliers, long> dapperRepo,
                                        IMstInvCpsSuppliersExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        public async Task<PagedResultDto<MstInvCpsSuppliersDto>> GetAll(GetMstInvCpsSuppliersInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.SupplierName), e => e.SupplierName.Contains(input.SupplierName))
                .WhereIf(!string.IsNullOrWhiteSpace(input.VatregistrationInvoice), e => e.VatregistrationInvoice.Contains(input.VatregistrationInvoice))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);

            var system = from o in pageAndFiltered
                         select new MstInvCpsSuppliersDto
                         {
                             Id = o.Id,
                             SupplierName = o.SupplierName,
                             SupplierNumber = o.SupplierNumber,
                             VatregistrationNum = o.VatregistrationNum,
                             VatregistrationInvoice = o.VatregistrationInvoice,
                             TaxPayerId = o.TaxPayerId,
                             RegistryId = o.RegistryId,
                             StartDateActive = o.StartDateActive,
                             EndDateActive = o.EndDateActive,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstInvCpsSuppliersDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetCpsSuppliersToExcel(MstInvCpsSuppliersExportInput input)
        {
            var filtered = _repo.GetAll()
            .WhereIf(!string.IsNullOrWhiteSpace(input.SupplierName), e => e.SupplierName.Contains(input.SupplierName))
            .WhereIf(!string.IsNullOrWhiteSpace(input.VatregistrationInvoice), e => e.VatregistrationInvoice.Contains(input.VatregistrationInvoice))
            ;

            var pageAndFiltered = filtered.OrderBy(s => s.Id);

            var query = from o in pageAndFiltered
                        select new MstInvCpsSuppliersDto
                        {
                            Id = o.Id,
                            SupplierName = o.SupplierName,
                            SupplierNumber = o.SupplierNumber,
                            VatregistrationNum = o.VatregistrationNum,
                            VatregistrationInvoice = o.VatregistrationInvoice,
                            TaxPayerId = o.TaxPayerId,
                            RegistryId = o.RegistryId,
                            StartDateActive = o.StartDateActive,
                            EndDateActive = o.EndDateActive,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

    }
}
