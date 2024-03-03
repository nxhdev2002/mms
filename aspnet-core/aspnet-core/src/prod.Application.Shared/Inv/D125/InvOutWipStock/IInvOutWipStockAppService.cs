using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;
using prod.Dto;
using prod.Inv.D125.Dto;

namespace prod.Inv.D125
{
	public interface IInvOutWipStockAppService : IApplicationService
	{
		Task<PagedResultDto<InvOutWipStockDto>> GetAll(GetInvOutWipStockInput input);
		Task<FileDto> GetOutWipStockToExcel(GetInvOutWipStockExportInput input);

    }

}


