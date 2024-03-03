using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.WorkingPattern.Dto;

namespace prod.Master.WorkingPattern
{

	public interface IMstWptWorkingTimeAppService : IApplicationService
	{

		Task<PagedResultDto<MstWptWorkingTimeDto_Dapper>> GetAll(GetMstWptWorkingTimeInput_Dapper input);

		Task CreateOrEdit(CreateOrEditMstWptWorkingTimeDto input);

		Task Delete(EntityDto input);

	}

}


