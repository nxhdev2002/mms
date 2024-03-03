using Abp.Application.Services;
using prod.Dto;
using prod.Master.Inventory.Dto;
using System.Collections.Generic;

namespace prod.Master.Inventory.Exporting
{
	public interface IMstGpsWbsCCMappingExcelExporter : IApplicationService
	{
		FileDto ExportToFile(List<MstGpsWbsCCMappingDto> mstgpswbsccmapping);

		FileDto ExportToHistoricalFile(List<string> data);
	}
}
