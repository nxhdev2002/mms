using Abp.Application.Services;
using prod.Dto;
using prod.Master.Painting.Dto;
using System.Collections.Generic;

namespace prod.Master.Painting.Exporting
{

	public interface IMstPtsBmpPartListExcelExporter : IApplicationService
	{

		FileDto ExportToFile(List<MstPtsBmpPartListDto> mstptsbmppartlist);

	}

}


