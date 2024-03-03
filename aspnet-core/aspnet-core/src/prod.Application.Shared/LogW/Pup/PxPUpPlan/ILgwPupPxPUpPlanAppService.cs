using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;using prod.Dto;
using System.Threading.Tasks;
using prod.LogW.Pup.Dto;
using prod.LogW.Pup.ImportDto;

namespace prod.LogW.Pup
{

	public interface ILgwPupPxPUpPlanAppService : IApplicationService
	{

		Task<PagedResultDto<LgwPupPxPUpPlanDto>> GetAll(GetLgwPupPxPUpPlanInput input);

		Task CreateOrEdit(CreateOrEditLgwPupPxPUpPlanDto input);

		Task Delete(EntityDto input);

		Task<PagedResultDto<LgwPupPxPUpPlanOutputDto>> GetPxpUnpackPlan();

		Task<List<ImportPxPUpPlanDto>> ImportPxPUpPlanFromExcel(List<ImportPxPUpPlanDto> pxpUpPlans);

        

    }

}


