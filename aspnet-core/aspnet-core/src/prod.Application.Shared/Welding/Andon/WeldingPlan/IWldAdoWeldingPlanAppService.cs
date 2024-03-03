using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Welding.Andon.Dto;
using prod.Welding.Andon.ImportDto;

namespace prod.Welding.Andon
{

	public interface IWldAdoWeldingPlanAppService : IApplicationService
	{

		Task<PagedResultDto<WldAdoWeldingPlanDto>> GetAll(GetWldAdoWeldingPlanInput input);

		Task CreateOrEdit(CreateOrEditWldAdoWeldingPlanDto input);

		Task Delete(EntityDto input);

		Task<List<ImportWldAdoWeldingPlanDto>> ImportWldAdoWeldingPlanFromExcel(List<ImportWldAdoWeldingPlanDto> WeldingPlans);

        Task<List<ImportWldAdoEdInDto>> ImportWldAdoEdInFromExcel(List<ImportWldAdoEdInDto> EdIns);


    }

}


