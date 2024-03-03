using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.LogA.Lds.Dto;
using prod.LogA.Lds.LotPlan.ImportDto;

namespace prod.LogA.Lds
{

	public interface ILgaLdsLotPlanAppService : IApplicationService
	{

		Task<PagedResultDto<LgaLdsLotPlanDto>> GetAll(GetLgaLdsLotPlanInput input);

		Task CreateOrEdit(CreateOrEditLgaLdsLotPlanDto input);

		Task Delete(EntityDto input);

		Task<List<ImportLgaLdsLotPlanDto>> ImportLgaLdsLotPlanFromExcel(List<ImportLgaLdsLotPlanDto> LgaLdsLotPlans);
		Task MergeDataLgaLdsLotPlan(string v_Guid);

	}

}


