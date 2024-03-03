using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Plan.Ccr.Dto;

namespace prod.Plan.Ccr.Exporting
{

	public interface IPlnCcrProductionPlanExcelExporter : IApplicationService
	{

		FileDto ExportToFile(List<PlnCcrProductionPlanDto> plnccrproductionplan);

	}

}


