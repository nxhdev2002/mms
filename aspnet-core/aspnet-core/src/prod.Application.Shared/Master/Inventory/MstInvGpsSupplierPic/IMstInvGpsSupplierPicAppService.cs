using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;

using System.Threading.Tasks;

using prod.Master.Inv.Dto;

namespace prod.Master.Inv
{

    public interface IMstInvGpsSupplierPicAppService : IApplicationService
    {

        Task<PagedResultDto<MstInvGpsSupplierPicDto>> GetAll(GetMstInvGpsSupplierPicInput input);

        Task CreateOrEdit(CreateOrEditMstInvGpsSupplierPicDto input);

        Task Delete(EntityDto input);

    }

}


