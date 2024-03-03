using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Painting.Andon.Dto;
using prod.Master.Common.Dto;

namespace prod.Painting.Andon
{
	public interface IPtsAdoDelayControlScreenAppService : IApplicationService
	{
		Task<PagedResultDto<GetDataOutput>> GetData();
		Task<PagedResultDto<MstCmmLookupDto>> GetLookupDelayTaget(string ItemCode);

	}

}


