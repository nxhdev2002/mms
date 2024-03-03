using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using prod.Master.WorkingPattern.Dto;

namespace prod.Master.WorkingPattern.Exporting
{

	public interface IMstWptSeasonMonthExcelExporter : IApplicationService
	{

		FileDto ExportToFile(List<MstWptSeasonMonthDto> mstwptseasonmonth);

	}

}


