using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Master.WorkingPattern.Dto;
using prod.Dto;

namespace prod.Master.WorkingPattern.Exporting
{

	public interface IMstWptDailyWorkingTimeExcelExporter : IApplicationService
	{

		FileDto ExportToFile(List<MstWptDailyWorkingTimeDto> mstwptdailyworkingtime);

	}

}

