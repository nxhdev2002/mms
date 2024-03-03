using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.LogA.Bp2.Dto;

namespace prod.LogA.Bp2
{

	public interface ILgaBp2ProgressAppService : IApplicationService
	{

		Task<PagedResultDto<LgaBp2ProgressDto>> GetAll(GetLgaBp2ProgressInput input);


	}

}


