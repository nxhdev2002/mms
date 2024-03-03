using Abp.Application.Services.Dto;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using prod.Inventory.CKD.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using prod.Dto;
using prod.Inventory.CKD.PaymentRequest.Exporting;
using Abp.Authorization;
using prod.Authorization;

namespace prod.Inventory.CKD.PaymentRequest
{
      [AbpAuthorize(AppPermissions.Pages_Ckd_Intransit_PaymentRequest_View)]

    public class InvCkdPaymentRequestAppService : prodAppServiceBase, IInvCkdPaymentRequestAppService
    {
        private readonly IDapperRepository<InvCkdPaymentRequest, long> _dapperRepoPayment;
        private readonly IDapperRepository<InvCkdCustomsDeclare, long> _dapperRepoCustoms;
        private readonly IRepository<InvCkdPaymentRequest, long> _repoPayment;
        private readonly IInvCkdPaymentRequestExcelExporter _calendarListExcelExporter;

        public InvCkdPaymentRequestAppService(IRepository<InvCkdPaymentRequest, long> repoPayment,
                                         IDapperRepository<InvCkdPaymentRequest, long> dapperRepoPayment,
                                         IDapperRepository<InvCkdCustomsDeclare, long> dapperRepoCustoms,
                                         IInvCkdPaymentRequestExcelExporter calendarListExcelExporter
            )
        {
            _repoPayment = repoPayment;
            _dapperRepoPayment = dapperRepoPayment;
            _dapperRepoCustoms = dapperRepoCustoms;
            _calendarListExcelExporter = calendarListExcelExporter;
        }
        public async Task<PagedResultDto<InvCkdCustomsDeclarePmDto>> GetCustomsDeclareByPayment(GetCustomsDeclarePmExportInput input)
        {
            string _sql = "Exec INV_CKD_CUSTOMS_DECLARE_GETBYPAYMENT @p_id";

            IEnumerable<InvCkdCustomsDeclarePmDto> result = await _dapperRepoCustoms.QueryAsync<InvCkdCustomsDeclarePmDto>(_sql, new
            {
                p_id = input.pid
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvCkdCustomsDeclarePmDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<PagedResultDto<InvCkdPaymentRequestDto>> GetPaymentRequestSearch(GetInvCkdPaymentRequestInput input)
        {
            string _sql = "Exec INV_CKD_PAYMENT_REQUEST_SEARCH @p_payment_request_no, @p_customs_no, @p_CkdPio, @p_OrderTypeCode";

            IEnumerable<InvCkdPaymentRequestDto> result = await _dapperRepoPayment.QueryAsync<InvCkdPaymentRequestDto>(_sql, new
            {
                p_payment_request_no = input.RequestNo,
                p_customs_no = input.CustomsNo,
                p_CkdPio = input.CkdPio,
                p_OrderTypeCode = input.OrderTypeCode
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvCkdPaymentRequestDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<FileDto> GetPaymentRequestToExcel(GetPaymentRequestExportInput input)
        {
            string _sql = "Exec INV_CKD_PAYMENT_REQUEST_SEARCH @p_payment_request_no, @p_customs_no, @p_CkdPio, @p_OrderTypeCode";

            IEnumerable<InvCkdPaymentRequestDto> result = await _dapperRepoPayment.QueryAsync<InvCkdPaymentRequestDto>(_sql, new
            {
                p_payment_request_no = input.RequestNo,
                p_customs_no = input.CustomsNo,
                p_CkdPio = input.CkdPio,
                p_OrderTypeCode = input.OrderTypeCode
            });
            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportToFilePaymentRequest(exportToExcel);
        }

        public async Task<FileDto> GetCustomsDeclareToExcel(GetCustomsDeclarePmExportInput input)
        {
            string _sql = "Exec INV_CKD_CUSTOMS_DECLARE_GETBYPAYMENT @p_id";

            IEnumerable<InvCkdCustomsDeclarePmDto> result = await _dapperRepoCustoms.QueryAsync<InvCkdCustomsDeclarePmDto>(_sql, new
            {
                p_id = input.pid
            });
            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportToFileCustomsDeclare(exportToExcel);
        }
    }
}
