using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.LogW.Pik.Dto;

namespace prod.LogW.Pik
{
	public interface ILgwPikPickingProgressAppService : IApplicationService
	{
		Task<PagedResultDto<LgwPikPickingProgressDto>> GetAll(GetLgwPikPickingProgressInput input);
	}
}


