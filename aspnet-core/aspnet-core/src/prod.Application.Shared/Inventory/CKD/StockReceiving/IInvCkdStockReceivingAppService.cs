        using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Inventory.CKD.Dto;
using System.Threading.Tasks;

namespace prod.Inventory.CKD
{
    public interface IInvCkdStockReceivingAppService : IApplicationService
    {
        Task<PagedResultDto<InvCkdStockReceivingDto>> GetAll(GetInvCkdStockReceivingInput input);

    }

}


