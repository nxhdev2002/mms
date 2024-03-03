using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.LogA.Dto;

namespace prod.Master.LogA
{

	public interface IMstLgaSpsRackAppService : IApplicationService
	{

		Task<PagedResultDto<MstLgaSpsRackDto>> GetAll(GetMstLgaSpsRackInput input);

		Task CreateOrEdit(CreateOrEditMstLgaSpsRackDto input);

		Task Delete(EntityDto input);

	}

}

