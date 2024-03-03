using Abp.Application.Services;
using prod.Dto;
using prod.Master.Painting.BmpPartType.Dto;
using System.Collections.Generic;

namespace prod.Master.Painting.Exporting
{

	public interface IMstPtsBmpPartTypeExcelExporter : IApplicationService
	{

		FileDto ExportToFile(List<MstPtsBmpPartTypeDto> mstptsbmpparttype);

	}

}


