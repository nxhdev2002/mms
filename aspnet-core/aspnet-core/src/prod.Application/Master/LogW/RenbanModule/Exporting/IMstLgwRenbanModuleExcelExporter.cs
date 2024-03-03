using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.LogW.Dto;

namespace prod.Master.LogW.Exporting
{

	public interface IMstLgwRenbanModuleExcelExporter : IApplicationService
	{

		FileDto ExportToFile(List<MstLgwRenbanModuleDto> mstlgwrenbanmodule);

	}

}


