using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Welding.Andon.Dto;

namespace prod.Welding.Andon
{

	public interface IWldAdoPunchQueueAppService : IApplicationService
	{

		Task<PagedResultDto<WldAdoPunchQueueDto>> GetAll(GetWldAdoPunchQueueInput input);

		Task CreateOrEdit(CreateOrEditWldAdoPunchQueueDto input);

		Task Delete(EntityDto input);

	}

}


