using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using prod.Authorization;
using prod.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using prod.Inventory.SPP.Cost.Dto;
using prod.Inventory.SPP.Cost.Exporting;
using prod.Inventory.SPP.InvoiceDetails.Exporting;
using prod.Inventory.SPP.InvoiceDetails.Dto;

namespace prod.Inventory.SPP.InvoiceDetails
{
    [AbpAuthorize(AppPermissions.Pages_SPP_InvoiceDetails_View)]
    public class InvSppInvoiceDetailsAppService : prodAppServiceBase, IInvSppInvoiceDetailsAppService
    {
        private readonly IDapperRepository<InvSppInvoiceDetails, long> _dapperRepo;
        private readonly IRepository<InvSppInvoiceDetails, long> _repo;
        private readonly IInvSppInvoiceDetailsExcelExporter _invoiceDetailsExcelExporter;

        public InvSppInvoiceDetailsAppService(IRepository<InvSppInvoiceDetails, long> repo,
                                         IDapperRepository<InvSppInvoiceDetails, long> dapperRepo,
                                         IInvSppInvoiceDetailsExcelExporter invoiceDetailsExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _invoiceDetailsExcelExporter = invoiceDetailsExcelExporter;
        }

        public async Task<PagedResultDto<InvSppInvoiceDetailsDto>> GetAll(GetInvSppInvoiceDetailsInput input)
        {
            string _sql = "Exec INV_SPP_INVOICEDETAILS_SEARCH @p_partno, @p_invoiceno, @p_stock, @p_supplier, @p_frommonthyear, @p_tomonthyear";

            IEnumerable<InvSppInvoiceDetailsDto> result = await _dapperRepo.QueryAsync<InvSppInvoiceDetailsDto>(_sql, new
            {
                p_partno = input.PartNo,
                p_invoiceno = input.InvoiceNo,
                p_stock = input.Stock,
                p_supplier = input.Supplier,
                p_frommonthyear = input.FromMonthYear,
                p_tomonthyear = input.ToMonthYear
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvSppInvoiceDetailsDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<FileDto> GetInvoiceDetailsToExcel(GetInvSppInvoiceDetailsExportInput input)
        {
            string _sql = "Exec INV_SPP_INVOICEDETAILS_SEARCH @p_partno, @p_invoiceno, @p_stock, @p_supplier, @p_frommonthyear, @p_tomonthyear";

            IEnumerable<InvSppInvoiceDetailsDto> result = await _dapperRepo.QueryAsync<InvSppInvoiceDetailsDto>(_sql, new
            {
                p_partno = input.PartNo,
                p_invoiceno = input.InvoiceNo,
                p_stock = input.Stock,
                p_supplier = input.Supplier,
                p_frommonthyear = input.FromMonthYear,
                p_tomonthyear = input.ToMonthYear
            });

            var exportToExcel = result.ToList();
            return _invoiceDetailsExcelExporter.ExportToFile(exportToExcel);
        }
    }
}
