using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Welding.Andon.Dto;

namespace prod.Welding.Andon
{

	public interface IWldAdoWeldingProgressAppService : IApplicationService
	{

		Task<PagedResultDto<WldAdoWeldingProgressDto>> GetAll(GetWldAdoWeldingProgressInput input);

		Task CreateOrEdit(CreateOrEditWldAdoWeldingProgressDto input);

		Task Delete(EntityDto input);

	}

}


