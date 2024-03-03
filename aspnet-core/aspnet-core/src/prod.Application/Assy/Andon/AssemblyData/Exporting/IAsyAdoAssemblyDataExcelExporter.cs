using Abp.Application.Services;
using System.Collections.Generic;
using prod.Assy.Andon.Dto;
using prod.Dto;

namespace prod.Assy.Andon.Exporting
{
	public interface IAsyAdoAssemblyDataExcelExporter : IApplicationService
	{
		FileDto ExportToFile(List<AsyAdoAssemblyDataDto> asyadoassemblydata);

	}

}


