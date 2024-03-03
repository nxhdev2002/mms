using Abp.Application.Services;
using prod.Dto;
using prod.Master.Welding.Dto;
using System.Collections.Generic;

namespace prod.Master.Welding.Exporting
{

	public interface IMstWldPunchIndicatorExcelExporter : IApplicationService
	{

		FileDto ExportToFile(List<MstWldPunchIndicatorDto> mstwldpunchindicator);

	}

}


