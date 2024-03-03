using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.LogW.Lup.Dto;

namespace prod.LogW.Lup
{

	public interface ILgwLupLotUpPlanAppService : IApplicationService
	{

		Task<PagedResultDto<LgwLupLotUpPlanDto>> GetAll(GetLgwLupLotUpPlanInput input);

		Task CreateOrEdit(CreateOrEditLgwLupLotUpPlanDto input);

		Task Delete(EntityDto input);
		Task<List<ImportLotUpPlanDto>> ImportPxPUpPlanFromExcel(List<ImportLotUpPlanDto> LotUpPlans);

		Task MergeDataLotUpPlan(string v_Guid);
		

	}

}


