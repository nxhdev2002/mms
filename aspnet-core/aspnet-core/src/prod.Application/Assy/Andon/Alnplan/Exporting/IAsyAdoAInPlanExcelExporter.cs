using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Assy.Andon.Dto;

namespace prod.Assy.Andon.Exporting
{

	public interface IAsyAdoAInPlanExcelExporter : IApplicationService
	{

		FileDto ExportToFile(List<AsyAdoAInPlanDto> asyadoainplan);
        FileDto ExportToHistoricalFile(List<string> data);

    }

}


