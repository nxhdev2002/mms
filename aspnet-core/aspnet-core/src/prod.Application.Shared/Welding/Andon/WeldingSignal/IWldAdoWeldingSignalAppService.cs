using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Welding.Andon.Dto;

namespace prod.Welding.Andon
{

	public interface IWldAdoWeldingSignalAppService : IApplicationService
	{

		Task<PagedResultDto<WldAdoWeldingSignalDto>> GetAll(GetWldAdoWeldingSignalInput input);

		Task CreateOrEdit(CreateOrEditWldAdoWeldingSignalDto input);

		Task Delete(EntityDto input);

	}

}


