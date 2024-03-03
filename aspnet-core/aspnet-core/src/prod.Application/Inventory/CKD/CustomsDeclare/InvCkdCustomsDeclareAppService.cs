using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Dapper;
using Microsoft.EntityFrameworkCore;
using NUglify.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using prod.Authorization;
using prod.Dto;
using prod.EntityFrameworkCore;
using prod.Inventory.CKD.Dto;
using prod.Inventory.CKD.Exporting;
using prod.Inventory.CKD.CustomsDeclare.Exporting;
using prod.MultiTenancy.Accounting.Dto;

namespace prod.Inventory.CKD
{
      [AbpAuthorize(AppPermissions.Pages_Ckd_Intransit_CustomsDeclare_View)]
    public class InvCkdCustomsDeclareAppService : prodAppServiceBase, IInvCkdCustomsDeclareAppService
    {
        private readonly IDapperRepository<InvCkdCustomsDeclare, long> _dapperRepo;
        private readonly IDapperRepository<InvCkdInvoice, long> _invoicedapperRepo;
        private readonly IDapperRepository<InvCkdPreCustoms, long> _predapperRepo;
        private readonly IRepository<InvCkdCustomsDeclare, long> _repo;
        private readonly IInvCkdCustomsDeclareExcelExporter _calendarListExcelExporter;

        public InvCkdCustomsDeclareAppService(IDapperRepository<InvCkdCustomsDeclare, long> dapperRepo,
                                        IDapperRepository<InvCkdInvoice, long> invoicedapperRepo,
                                       IDapperRepository<InvCkdPreCustoms, long> predapperRepo,
                                       IRepository<InvCkdCustomsDeclare, long> repo,
                                       IInvCkdCustomsDeclareExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
            _invoicedapperRepo = invoicedapperRepo;
            _predapperRepo =  predapperRepo;
        }

        public async Task<PagedResultDto<InvCkdCustomsDeclareDto>> GetAllCustomsDeclare(GetInvCkdCustomerDeclareInput input)
        {
            string _sql = "Exec INV_CKD_CUSTOMS_DECLARE_SEARCH @p_customer_declare_no,@p_customer_declare_date,@p_bill_no, @p_invoice_no, @p_CkdPio, @p_OrderTypeCode";

            IEnumerable<InvCkdCustomsDeclareDto> result = await _dapperRepo.QueryAsync<InvCkdCustomsDeclareDto>(_sql,
                  new
                  {
                      p_customer_declare_no = input.CustomsDeclareNo,
                      p_customer_declare_date = input.DeclareDate,
                      p_bill_no = input.BillofladingNo,
                      p_invoice_no = input.InvoiceNo,
                      p_CkdPio = input.CkdPio,
                      p_OrderTypeCode = input.OrderTypeCode
                  });
            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvCkdCustomsDeclareDto>(
                totalCount,
                pagedAndFiltered);
        }
                
        public async Task<PagedResultDto<PreCustomerDto>> GetInvCkdPreCustomsList(GetInvCkdPreCustomsListInput input)
        {
            string _sql = "Exec INV_CKD_PRE_CUSTOMS_SEARCH_BY_BILL_NO @p_cus_id, @p_bill_no";

            IEnumerable<PreCustomerDto> result = await _predapperRepo.QueryAsync<PreCustomerDto>(_sql,
                  new
                  {
                      p_cus_id = input.CustomsDeclareId,
                      p_bill_no = input.BillNo
                  });
            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<PreCustomerDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<PagedResultDto<InVoiceListDto>> GetInvoice(GetInvCkdInvoiceInput input)
        {
            string _sql = "Exec INV_CKD_INVOICE_DETAILS_GETBYPRECUSID @p_preId";

            IEnumerable<InVoiceListDto> result = await _invoicedapperRepo.QueryAsync<InVoiceListDto>(_sql,
                  new
                  {
                      p_preId = input.PreCustomsId,
                  });
            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InVoiceListDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<FileDto> GetCkdCustomerDeclareToExcel(GetInvCkdCustomerDeclareInput input)
        {
            string _sql = "Exec INV_CKD_CUSTOMS_DECLARE_SEARCH @p_customer_declare_no,@p_customer_declare_date,@p_bill_no, @p_invoice_no, @p_CkdPio, @p_OrderTypeCode";
            IEnumerable<InvCkdCustomsDeclareDto> result = await _dapperRepo.QueryAsync<InvCkdCustomsDeclareDto>(_sql,
              new
              {
                  p_customer_declare_no = input.CustomsDeclareNo,
                  p_customer_declare_date = input.DeclareDate,
                  p_bill_no = input.BillofladingNo,
                  p_invoice_no = input.InvoiceNo,
                  p_CkdPio = input.CkdPio,
                  p_OrderTypeCode = input.OrderTypeCode
              });
            var listPoHeaderResult = result.ToList();
            return _calendarListExcelExporter.CustomerDeclareExportToFile(listPoHeaderResult);

           }
        public async Task<FileDto> GetCkdPreCustomsToExcel(GetInvCkdPreCustomsListInput input)
        {
            string _sql = "Exec INV_CKD_PRE_CUSTOMS_SEARCH_BY_BILL_NO @p_cus_id, @p_bill_no ";
            IEnumerable<PreCustomerDto> result = await _dapperRepo.QueryAsync<PreCustomerDto>(_sql,
              new
              {
                  p_cus_id = input.CustomsDeclareId,
                  p_bill_no = input.BillNo
              });
            var listPoHeaderResult = result.ToList();
            return _calendarListExcelExporter.PreCustomsExportToFile(listPoHeaderResult);

        }
        public async Task<FileDto> GetCkdInvoiceToExcel(GetInvCkdInvoiceInput input)
        {
            string _sql = "Exec INV_CKD_INVOICE_DETAILS_GETBYPRECUSID @p_preId";
            IEnumerable<InVoiceListDto> result = await _dapperRepo.QueryAsync<InVoiceListDto>(_sql,
              new
              {
                  p_preId = input.PreCustomsId,
              });
            var listPoHeaderResult = result.ToList();
            return _calendarListExcelExporter.InvoiceExportToFile(listPoHeaderResult);

        }

    }
}
