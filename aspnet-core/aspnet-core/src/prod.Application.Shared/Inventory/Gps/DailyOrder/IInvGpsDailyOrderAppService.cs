using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;
using prod.Inventory.GPS.Dto;

namespace prod.Inventory.GPS
{

    public interface IInvGpsDailyOrderAppService : IApplicationService
    {

        Task<PagedResultDto<InvGpsDailyOrderDto>> GetAll(GetInvGpsDailyOrderInput input);

        Task CreateOrEdit(CreateOrEditInvGpsDailyOrderDto input);

        Task Delete(EntityDto input);

    }

}


