using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Inventory.CKD.Dto;

namespace prod.Inventory.CKD
{

    public interface IInvCkdContainerIntransitAppService : IApplicationService
    {

        Task<PagedResultDto<InvCkdContainerIntransitDto>> GetAll(GetInvCkdContainerIntransitInput input);


    }

}


