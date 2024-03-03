using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;using prod.Dto;
using System.Threading.Tasks;
using prod.Inv.D125.Dto;

namespace prod.Inv.D125
{
	public interface IInvInDetailsAppService : IApplicationService
	{
		Task<PagedResultDto<InvInDetailsDto>> GetAll(GetInvInDetailsInput input);
		Task<FileDto> GetInDetailsToExcel(GetInvInDetailsExportInput input);

    }

}


