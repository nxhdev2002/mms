using Abp.Application.Services;
using System.Collections.Generic;
using prod.Master.WorkingPattern.Dto;
using prod.Dto;

namespace prod.Master.WorkingPattern.Exporting
{

	public interface IMstWptWorkingTypeExcelExporter : IApplicationService
	{

		FileDto ExportToFile(List<MstWptWorkingTypeDto> mstwptworkingtype);

	}

}


