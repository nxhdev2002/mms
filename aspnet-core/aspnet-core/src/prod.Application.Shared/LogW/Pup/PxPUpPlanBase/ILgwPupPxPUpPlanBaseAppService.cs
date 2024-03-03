using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.LogW.Pup.Dto;
using prod.LogW.Pup.ImportDto;

namespace prod.LogW.Pup
{

	public interface ILgwPupPxPUpPlanBaseAppService : IApplicationService
	{

		Task<PagedResultDto<LgwPupPxPUpPlanBaseDto>> GetAll(GetLgwPupPxPUpPlanBaseInput input);

		Task CreateOrEdit(CreateOrEditLgwPupPxPUpPlanBaseDto input);

		Task Delete(EntityDto input);

        Task<List<ImportPxPUpPlanBaseDto>> ImportPxPUpPlanBaseFromExcel(List<ImportPxPUpPlanBaseDto> PxPUpPlanBases);

    }

}



