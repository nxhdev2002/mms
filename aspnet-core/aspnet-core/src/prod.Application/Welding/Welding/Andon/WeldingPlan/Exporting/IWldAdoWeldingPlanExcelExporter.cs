using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Welding.Andon.Dto;

namespace prod.Welding.Andon.Exporting
{

	public interface IWldAdoWeldingPlanExcelExporter : IApplicationService
	{

		FileDto ExportToFile(List<WldAdoWeldingPlanDto> wldadoweldingplan);
		FileDto ExportToFileErr(List<WldAdoWeldingPlanDto> weldingplanerr);
        FileDto ExportToHistoricalFile(List<string> data);


    }

}


