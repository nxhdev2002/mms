using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Painting.Andon.Dto;

namespace prod.Painting.Andon
{

	public interface IPtsAdoScanInfoAppService : IApplicationService
	{

		Task<PagedResultDto<PtsAdoScanInfoDto>> GetAll(GetPtsAdoScanInfoInput input);



	}

}


