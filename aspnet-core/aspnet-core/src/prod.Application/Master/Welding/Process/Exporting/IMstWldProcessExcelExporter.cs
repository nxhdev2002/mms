using Abp.Application.Services;
using prod.Dto;
using prod.Master.Welding.Dto;
using System.Collections.Generic;

namespace prod.Master.Welding.Exporting
{

	public interface IMstWldProcessExcelExporter : IApplicationService
	{

		FileDto ExportToFile(List<MstWldProcessDto> mstwldprocess);

	}

}


