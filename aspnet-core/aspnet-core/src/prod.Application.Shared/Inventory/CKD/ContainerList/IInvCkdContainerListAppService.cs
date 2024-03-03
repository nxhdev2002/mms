using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace prod.Inventory.CKD
{
    public interface IInvCkdContainerListAppService : IApplicationService
    {
        Task<PagedResultDto<InvCkdContainerListDto>> GetAll(GetInvCkdContainerListInput input);
        Task<PagedResultDto<InvCkdContainerInvoiceDto>> GetDataContainerInvoiceByCont(GetInvCkdContIntransitbyContInput input);
    }

}


