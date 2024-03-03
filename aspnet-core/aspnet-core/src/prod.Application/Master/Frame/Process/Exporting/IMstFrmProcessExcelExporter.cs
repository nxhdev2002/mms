using Abp.Application.Services;
using prod.Dto;
using prod.Master.Frame.Dto;
using System.Collections.Generic;

namespace prod.Master.Frame.Exporting
{

	public interface IMstFrmProcessExcelExporter : IApplicationService
	{

		FileDto ExportToFile(List<MstFrmProcessDto> mstfrmprocess);

	}

}


