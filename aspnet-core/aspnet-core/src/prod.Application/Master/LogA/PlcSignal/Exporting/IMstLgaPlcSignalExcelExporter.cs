using Abp.Application.Services;
using System.Collections.Generic;
using prod.Dto;
using prod.Master.LogA.Dto;

namespace prod.Master.LogA.Exporting
{

	public interface IMstLgaPlcSignalExcelExporter : IApplicationService
	{

		FileDto ExportToFile(List<MstLgaPlcSignalDto> mstlgaplcsignal);

	}

}


