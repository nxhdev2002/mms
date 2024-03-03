using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using prod.Authorization;
using prod.Dto;
using prod.Inventory.PIO.TopsseInvoice.Dto;
using prod.Inventory.PIO.TopsseInvoice.Exporting;
using prod.Master.Inventory;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prod.Inventory.PIO.TopsseInvoice
{
    [AbpAuthorize(AppPermissions.Pages_PIO_Master_TopsseInvoice_View)]
    public class InvTopsseInvoiceAppService : prodAppServiceBase, IInvTopsseInvoiceAppService
    {
        private readonly IDapperRepository<InvTopsseInvoice, long> _dapperRepo;
        private readonly IDapperRepository<InvTopsseInvoiceDetails, long> _dapperRepoDetails;
        private readonly IRepository<InvTopsseInvoice, long> _repo;
        private readonly IInvTopsseInvoiceExcelExporter _calendarListExcelExporter;

        public InvTopsseInvoiceAppService(IRepository<InvTopsseInvoice, long> repo,
                                         IDapperRepository<InvTopsseInvoice, long> dapperRepo,
                                         IDapperRepository<InvTopsseInvoiceDetails, long> dapperRepoDetails,
                                        IInvTopsseInvoiceExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _dapperRepoDetails = dapperRepoDetails;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        public async Task<PagedResultDto<InvTopsseInvoiceDetailsDto>> GetTopsseInvoiceDetailsById(GetInvTopsseInvoiceDetailsInput input)
        {
            string _sql = "Exec INV_PIO_TOPSSE_INVOICE_DETAILS_BY_ID @p_id";

            IEnumerable<InvTopsseInvoiceDetailsDto> result = await _dapperRepoDetails.QueryAsync<InvTopsseInvoiceDetailsDto>(_sql, new
            {
                p_id = input.TopsseInvoiceId
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvTopsseInvoiceDetailsDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<PagedResultDto<InvTopsseInvoiceDto>> GetTopsseInvoiceSearch(GetInvTopsseInvoiceInput input)
        {
            string _sql = "Exec INV_PIO_TOPSSE_INVOICE_SEARCH @p_invoiceno, @p_invoicedate, @p_orderno, @p_partno, @p_status";

            IEnumerable<InvTopsseInvoiceDto> result = await _dapperRepo.QueryAsync<InvTopsseInvoiceDto>(_sql, new
            {
                p_invoiceno = input.InvoiceNo,
                p_invoicedate = input.InvoiceDate,
                p_orderno = input.OrderNo,
                p_partno = input.PartNo,
                p_status = input.Status
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvTopsseInvoiceDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<FileDto> GetTopsseInvoiceToExcel(GetInvTopsseInvoiceExportInput input)
        {
            string _sql = "Exec INV_PIO_TOPSSE_INVOICE_SEARCH @p_invoiceno, @p_invoicedate, @p_orderno, @p_partno, @p_status";

            IEnumerable<InvTopsseInvoiceDto> result = await _dapperRepo.QueryAsync<InvTopsseInvoiceDto>(_sql, new
            {
                p_invoiceno = input.InvoiceNo,
                p_invoicedate = input.InvoiceDate,
                p_orderno = input.OrderNo,
                p_partno = input.PartNo,
                p_status = input.Status
            });

            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        public async Task<FileDto> GetTopsseInvoiceDetailsToExcel(GetInvTopsseInvoiceDetailsExportInput input)
        {
            string _sql = "Exec INV_PIO_TOPSSE_INVOICE_DETAILS_BY_ID @p_id";

            IEnumerable<InvTopsseInvoiceDetailsDto> result = await _dapperRepoDetails.QueryAsync<InvTopsseInvoiceDetailsDto>(_sql, new
            {
                p_id = input.TopsseInvoiceId
            });

            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportToFileDetails(exportToExcel);
        }

        public async Task<PagedResultDto<InvTopsseInvoiceDetailsDto>> GetTopsseInvoiceDetailsToReceive(GetInvTopsseInvoiceDetailsInput input)
        {
            string _sql = "Exec INV_PIO_TOPSSE_INVOICE_DETAILS_TO_RECEIVE @p_id";

            IEnumerable<InvTopsseInvoiceDetailsDto> result = await _dapperRepoDetails.QueryAsync<InvTopsseInvoiceDetailsDto>(_sql, new
            {
                p_id = input.TopsseInvoiceId
            });

            var listResult = result.ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvTopsseInvoiceDetailsDto>(
                totalCount,
                listResult);
        }

        public async Task GetDataReceiveTopsseInvoice(GetTopsseInvoiceDetailsAfterReceiveInput input)
        {
            string _sql = "Exec INV_PIO_TOPSSE_INVOICE_DETAILS_UPDATE_RECEIVE @p_value, @p_receive_date, @p_UserId";

            await _dapperRepoDetails.QueryAsync<InvTopsseInvoiceDetailsDto>(_sql, new
            {
                p_value = input.StringValue,
                p_receive_date = input.ReceiveDate,
                p_UserId = AbpSession.UserId
            });
        }
    }
}
