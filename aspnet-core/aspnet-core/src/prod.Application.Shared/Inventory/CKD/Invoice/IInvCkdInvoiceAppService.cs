using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Dto;
using prod.Inventory.CKD.Invoice.Dto;
using prod.Inventory.Invoice.Dto;
using prod.Master.Cmm.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.CKD.Invoice
{
    public interface IInvCkdInvoiceAppService : IApplicationService
    {

        Task<PagedResultDto<InvCkdInvoiceDetailsDto>> GetInvoiceDetailsGetbyinvoiceid(GetInvCkdInvoiceDetailDtolInput input);
        Task<PagedResultDto<InvCkdInvoiceDto>> GetInvoiceSearch(GetInvCkdInvoiceDtolInput input);

    }
}
