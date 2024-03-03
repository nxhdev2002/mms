using Abp.Application.Services;
using Abp.Application.Services.Dto;

using System.Threading.Tasks;

using prod.Master.Inv.Dto;

namespace prod.Master.Inv
{

    public interface IMstInvGpsTruckSupplierAppService : IApplicationService
    {

        Task<PagedResultDto<MstInvGpsTruckSupplierDto>> GetAll(GetMstInvGpsTruckSupplierInput input);

        Task CreateOrEdit(CreateOrEditMstInvGpsTruckSupplierDto input);

        Task Delete(EntityDto input);

    }

}


