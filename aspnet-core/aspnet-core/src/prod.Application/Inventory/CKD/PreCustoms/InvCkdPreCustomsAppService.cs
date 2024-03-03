using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using prod.Authorization;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using prod.Inventory.CKD.Exporting;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Inventory.CKD
{
    [AbpAuthorize(AppPermissions.Pages_Ckd_Intransit_PreCustoms_View)]
    public class InvCkdPreCustomsAppService : prodAppServiceBase, IInvCkdPreCustomsAppService
    {
        private readonly IDapperRepository<InvCkdPreCustoms, long> _dapperRepo;
        private readonly IDapperRepository<InvCkdInvoice, long> _invoicedapperRepo;
        private readonly IInvCkdPreCustomsExcelExporter _calendarListExcelExporter;

        public InvCkdPreCustomsAppService(IDapperRepository<InvCkdInvoice, long> invoicedapperRepo,
                                          IDapperRepository<InvCkdPreCustoms, long> dapperRepo,
                                         IInvCkdPreCustomsExcelExporter calendarListExcelExporter
            )
        {
            _invoicedapperRepo = invoicedapperRepo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        public async Task<PagedResultDto<InvCkdPreCustomsDto>> GetAllPreCustoms(GetInvCkdPreCustomsInput input)
        {
            string _sql = "Exec INV_CKD_PRE_CUSTOMS_SEARCH @p_preId, @p_invoice_id , @p_bill_Date, @p_pre_Custom_No, @p_CkdPio, @p_OrderTypeCode";

            IEnumerable<InvCkdPreCustomsDto> result = await _dapperRepo.QueryAsync<InvCkdPreCustomsDto>(_sql,
                  new
                  {
                      p_preId = input.BillNo,
                      p_invoice_id = input.InvoiceNo,
                      p_bill_Date = input.BillDate,
                      p_pre_Custom_No = input.PreCustomsId,
                      p_CkdPio = input.CkdPio,
                      p_OrderTypeCode = input.OrderTypeCode
                  });
            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvCkdPreCustomsDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<PagedResultDto<InvoiceListDto>> GetInvoice(GetInvCkdinvoiceInput input)
        {
            string _sql = "Exec INV_CKD_INVOICE_SEARCHBYPRE @p_pre_id";

            IEnumerable<InvoiceListDto> result = await _invoicedapperRepo.QueryAsync<InvoiceListDto>(_sql,
                  new
                  {
                      p_pre_id = input.PreCustomsId,
                  });
            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvoiceListDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<PagedResultDto<InvoiceDetailListDto>> GetInvoiceDetail(GetInvCkdinvoiceDetailInput input)
        {
            string _sql = "Exec INV_CKD_INVOICE_DETAILS_GETBYPREINVOICEID @p_preId,@p_invoice_id";

            IEnumerable<InvoiceDetailListDto> result = await _invoicedapperRepo.QueryAsync<InvoiceDetailListDto>(_sql,
                  new
                  {
                      p_preId = input.PreCustomsId,
                      p_invoice_id = input.Id,
                  });
            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvoiceDetailListDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<FileDto> GetCkdPreCustomsToExcel(GetInvCkdPreCustomsInput input)
        {
            string _sql = "Exec INV_CKD_PRE_CUSTOMS_SEARCH @p_preId, @p_invoice_id, @p_bill_Date, @p_pre_Custom_No,  @p_CkdPio, @p_OrderTypeCode";
            IEnumerable<InvCkdPreCustomsDto> result = await _dapperRepo.QueryAsync<InvCkdPreCustomsDto>(_sql,
              new
              {
                  p_preId = input.BillNo,
                  p_invoice_id = input.InvoiceNo,
                  p_bill_Date = input.BillDate,
                  p_pre_Custom_No = input.PreCustomsId,
                  p_CkdPio = input.CkdPio,
                  p_OrderTypeCode = input.OrderTypeCode
              });
            var listPreResult = result.ToList();
            return _calendarListExcelExporter.PreCustomsExportToFile(listPreResult);
        }
        public async Task<FileDto> GetCkdInvoiceToExcel(GetInvCkdinvoiceInput input)
        {
            string _sql = "Exec INV_CKD_INVOICE_SEARCHBYPRE @p_pre_id";
            IEnumerable<InvoiceListDto> result = await _invoicedapperRepo.QueryAsync<InvoiceListDto>(_sql,
              new
              {
                  p_pre_id = input.PreCustomsId,
              });
            var listInvoiceResult = result.ToList();
            return _calendarListExcelExporter.InvoiceExportToFile(listInvoiceResult);
        }
        public async Task<FileDto> GetCkdInvoiceDdtailToExcel(GetInvCkdinvoiceDetailInput input)
        {
            string _sql = "Exec INV_CKD_INVOICE_DETAILS_GETBYPREINVOICEID @p_preId,@p_invoice_id";
            IEnumerable<InvoiceDetailListDto> result = await _invoicedapperRepo.QueryAsync<InvoiceDetailListDto>(_sql,
              new
              {
                  p_preId = input.PreCustomsId,
                  p_invoice_id = input.Id,
              });
            var listInvoiceDetailResult = result.ToList();
            return _calendarListExcelExporter.InvoiceDetailExportToFile(listInvoiceDetailResult);
        }
    }
}
