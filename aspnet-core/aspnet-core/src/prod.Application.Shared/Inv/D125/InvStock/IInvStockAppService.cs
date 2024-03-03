using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;
using prod.Inv.D125.Dto;

namespace prod.Inv.D125
{
	public interface IInvStockAppService : IApplicationService
	{
		Task<PagedResultDto<InvStockDto>> GetAll(GetInvStockInput input);
	}

}


