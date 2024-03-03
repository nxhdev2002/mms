using Abp.Application.Services.Dto;
using Abp.Application.Services;
using System.Threading.Tasks;
using prod.Inventory.CKD.Dto;

namespace prod.Inventory.CKD
{
    public interface IInvCkdShipmentAppService : IApplicationService
    {

        Task<PagedResultDto<InvCkdShipmentDto>> GetAll(GetInvCkdShipmentInput input);

    }
}
