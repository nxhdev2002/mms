using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Master.Pio.DTO;
using System.Threading.Tasks;

namespace prod.Master.Pio
{
	public interface IMstLspSupplierInforAppService : IApplicationService
	{
		Task<PagedResultDto<MstLspSupplierInforDto>> GetAll(GetMstLspSupplierInforInput input);


	}
}
