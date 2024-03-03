using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Inventory.GPS.Dto;
using System.Threading.Tasks;

namespace prod.Inventory.GPS
{

    public interface IInvGpsContentListAppService : IApplicationService
    {

        Task<PagedResultDto<InvGpsContentListDto>> GetAll(GetInvGpsContentListInput input);

        Task CreateOrEdit(CreateOrEditInvGpsContentListDto input);

        Task Delete(EntityDto input);

    }

}


