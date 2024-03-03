using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Frame.Andon.Dto;

namespace prod.Frame.Andon
{
	public interface IFrmAdoFrameProgressAppService : IApplicationService
	{
		Task<PagedResultDto<FrmAdoFrameProgressDto>> GetAll(GetFrmAdoFrameProgressInput input);
	}
}


