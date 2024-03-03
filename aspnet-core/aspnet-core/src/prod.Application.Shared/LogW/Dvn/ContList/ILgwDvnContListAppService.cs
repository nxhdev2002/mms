using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.LogW.Dvn.Dto;

namespace prod.LogW.Dvn
{

	public interface ILgwDvnContListAppService : IApplicationService
	{

		Task<PagedResultDto<LgwDvnContListDto>> GetAll(GetLgwDvnContListInput input);

		Task CreateOrEdit(CreateOrEditLgwDvnContListDto input);

		Task Delete(EntityDto input);

	}

}


