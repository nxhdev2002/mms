using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.LogW.Dto;

namespace prod.Master.LogW
{

	public interface IMstLgwScreenConfigAppService : IApplicationService
	{

		Task<PagedResultDto<MstLgwScreenConfigDto>> GetAll(GetMstLgwScreenConfigInput input);

		Task CreateOrEdit(CreateOrEditMstLgwScreenConfigDto input);

		Task Delete(EntityDto input);

	}

}


