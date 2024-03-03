using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;
using prod.Dto;
using prod.Inv.D125.Dto;

namespace prod.Inv.D125
{
	public interface IInvOutLineOffAppService : IApplicationService
	{
		Task<PagedResultDto<InvOutLineOffDto>> GetAll(GetInvOutLineOffInput input);

		Task<FileDto> GetInvOutLineOffToExcel(GetInvOutLineOffExportInput input);

    }

}


