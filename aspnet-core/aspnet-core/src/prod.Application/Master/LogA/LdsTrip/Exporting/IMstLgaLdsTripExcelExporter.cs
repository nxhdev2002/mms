using Abp.Application.Services;
using System.Collections.Generic;
using prod.Master.LogA.Dto;
using prod.Dto;

namespace prod.Master.LogA.Exporting
{

	public interface IMstLgaLdsTripExcelExporter : IApplicationService
	{

		FileDto ExportToFile(List<MstLgaLdsTripDto> mstlgaldstrip);

	}

}


