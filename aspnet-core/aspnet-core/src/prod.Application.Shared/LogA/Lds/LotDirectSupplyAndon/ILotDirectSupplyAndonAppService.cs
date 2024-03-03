using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;
using prod.LogA.Lds.Dto;

namespace prod.LogA.Lds
{
	public interface ILotDirectSupplyAndonAppService : IApplicationService
	{
		Task<PagedResultDto<LotDirectSupplyAndonDto>> GetDataLotDirectSupplyAndon(string prod_line);

	}

}
