using Abp.Application.Services;
using System.Collections.Generic;
using prod.Dto;
using prod.Master.WorkingPattern.Dto;

namespace prod.Master.WorkingPattern.Exporting
{

	public interface IMstWptPatternHExcelExporter : IApplicationService
	{

		FileDto ExportToFile(List<MstWptPatternHDto> mstwptpatternh);

	}

}


