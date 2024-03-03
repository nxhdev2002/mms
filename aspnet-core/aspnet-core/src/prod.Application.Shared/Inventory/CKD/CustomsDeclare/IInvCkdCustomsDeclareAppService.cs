using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Inventory.CKD.Dto;
using prod.Inventory.CKD.Dto;

namespace prod.Inventory.CKD
{

    public interface IInvCkdCustomsDeclareAppService : IApplicationService
    {

        Task<PagedResultDto<InvCkdCustomsDeclareDto>> GetAllCustomsDeclare(GetInvCkdCustomerDeclareInput input);

        Task<PagedResultDto<PreCustomerDto>> GetInvCkdPreCustomsList(GetInvCkdPreCustomsListInput input);

        Task<PagedResultDto<InVoiceListDto>> GetInvoice(GetInvCkdInvoiceInput input);

    }

}


