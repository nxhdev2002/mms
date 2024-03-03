using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Inventory.CKD.Dto;
using prod.Master.Inventory.ContainerStatus.Dto;

namespace prod.Inventory.CKD
{

    public interface IInvCkdContainerInvoiceAppService : IApplicationService
    {

        Task<PagedResultDto<InvCkdContainerInvoiceDto>> GetAll(GetInvCkdContainerInvoiceInput input);

        //Task CreateOrEdit(CreateOrEditInvCkdContainerInvoiceDto input);

        //Task Delete(EntityDto input);

        Task<List<MstInvContainerStatusDto>> GetContStatusList();

    }

}


