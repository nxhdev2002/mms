using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.LogW.Dvn.Dto;

namespace prod.LogW.Dvn.Exporting
{

	public interface ILgwDvnContListExcelExporter : IApplicationService
	{

		FileDto ExportToFile(List<LgwDvnContListDto> lgwdvncontlist);

	}

}


