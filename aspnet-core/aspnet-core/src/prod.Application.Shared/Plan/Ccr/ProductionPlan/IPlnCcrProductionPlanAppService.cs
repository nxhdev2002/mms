using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Plan.Ccr.Dto;
using prod.Plan.Ccr.ProductionPlan.ImportDto;
using prod.LogW.Pup.ImportDto;

namespace prod.Plan.Ccr
{

	public interface IPlnCcrProductionPlanAppService : IApplicationService
	{

		Task<PagedResultDto<PlnCcrProductionPlanDto>> GetAll(GetPlnCcrProductionPlanInput input);

		Task CreateOrEdit(CreateOrEditPlnCcrProductionPlanDto input);

		Task Delete(EntityDto input);

		void ImportPlnCcrProductionPlanFMKFromExcel(List<ImportPlnCcrProductionPlanDto> plnCcrProductionPlanFmks);
		void ImportPlnCcrProductionPlanFPFromExcel(List<ImportPlnCcrProductionPlanDto> plnCcrProductionPlanFps);
		void ImportPlnCcrProductionPlanW1FromExcel(List<ImportPlnCcrProductionPlanDto> plnCcrProductionPlanW1s);
		void ImportPlnCcrProductionPlanW2FromExcel(List<ImportPlnCcrProductionPlanDto> plnCcrProductionPlanW2s);
        void ImportPlnCcrProductionPlanW3FromExcel(List<ImportPlnCcrProductionPlanDto> plnCcrProductionPlanW3s);
        void ImportPlnCcrProductionPlanA1FromExcel(List<ImportPlnCcrProductionPlanDto> plnCcrProductionPlanA1s);
		void ImportPlnCcrProductionPlanA2FromExcel(List<ImportPlnCcrProductionPlanDto> plnCcrProductionPlanA2s);

		//Task<List<ImportPlnCcrProductionPlanDto>> ImportPlnCcrProductionPlanFMKFromExcel(List<ImportPlnCcrProductionPlanDto> plnCcrProductionPlanFmks);
		//Task<List<ImportPlnCcrProductionPlanDto>> ImportPlnCcrProductionPlanFPFromExcel(List<ImportPlnCcrProductionPlanDto> plnCcrProductionPlanFps);
		//Task<List<ImportPlnCcrProductionPlanDto>> ImportPlnCcrProductionPlanW1FromExcel(List<ImportPlnCcrProductionPlanDto> plnCcrProductionPlanW1s);
		//Task<List<ImportPlnCcrProductionPlanDto>> ImportPlnCcrProductionPlanW2FromExcel(List<ImportPlnCcrProductionPlanDto> plnCcrProductionPlanW2s);
		//Task<List<ImportPlnCcrProductionPlanDto>> ImportPlnCcrProductionPlanA1FromExcel(List<ImportPlnCcrProductionPlanDto> plnCcrProductionPlanA1s);
		//Task<List<ImportPlnCcrProductionPlanDto>> ImportPlnCcrProductionPlanA2FromExcel(List<ImportPlnCcrProductionPlanDto> plnCcrProductionPlanA2s);

	}

}


