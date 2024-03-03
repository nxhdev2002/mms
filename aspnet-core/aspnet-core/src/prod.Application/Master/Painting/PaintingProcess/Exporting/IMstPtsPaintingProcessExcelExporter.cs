using Abp.Application.Services;
using prod.Dto;
using prod.Master.Painting.Dto;
using System.Collections.Generic;

namespace prod.Master.Painting.PaintingProcess.Exporting
{

	public interface IMstPtsPaintingProcessExcelExporter : IApplicationService
	{

		FileDto ExportToFile(List<MstPtsPaintingProcessDto> mstptspaintingprocess);
	}

}


