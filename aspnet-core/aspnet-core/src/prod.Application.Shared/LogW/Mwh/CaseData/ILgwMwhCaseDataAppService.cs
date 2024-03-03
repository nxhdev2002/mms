using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;using prod.Dto;
using System.Threading.Tasks;
using prod.LogW.Mwh.Dto;

namespace prod.LogW.Mwh
{

	public interface ILgwMwhCaseDataAppService : IApplicationService
	{

		Task<PagedResultDto<LgwMwhCaseDataDto>> GetAll(GetLgwMwhCaseDataInput input);

		Task<PagedResultDto<LgwMwhCaseDataDto>> GetCaseDataHisByCaseNo(GetLgwMwhCaseDataHisByCaseNoInput input);




    }

}


