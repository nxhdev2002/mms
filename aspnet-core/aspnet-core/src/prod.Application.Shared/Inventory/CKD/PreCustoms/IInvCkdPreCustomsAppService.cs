using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using prod.Inventory.CKD.Dto;

namespace prod.Inventory.CKD
{

    public interface IInvCkdPreCustomsAppService : IApplicationService
    {

        Task<PagedResultDto<InvCkdPreCustomsDto>> GetAllPreCustoms(GetInvCkdPreCustomsInput input);

        Task<PagedResultDto<InvoiceListDto>> GetInvoice(GetInvCkdinvoiceInput input);

        Task<PagedResultDto<InvoiceDetailListDto>> GetInvoiceDetail(GetInvCkdinvoiceDetailInput input);

    }

}


