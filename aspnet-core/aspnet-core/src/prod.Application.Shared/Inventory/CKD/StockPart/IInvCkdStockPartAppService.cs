using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Inventory.CKD.Dto;
using System.Threading.Tasks;

namespace prod.Inventory.CKD
{
	public interface IInvCkdStockPartAppService : IApplicationService
	{
        Task<PagedResultDto<InvCkdStockPartDto>> GetAll(GetInvCkdStockPartInput input);

        Task<PagedResultDto<InvCkdStockReceivingDto>> GetCheckStockPart(GetCheckStockPart input);

    }
}


