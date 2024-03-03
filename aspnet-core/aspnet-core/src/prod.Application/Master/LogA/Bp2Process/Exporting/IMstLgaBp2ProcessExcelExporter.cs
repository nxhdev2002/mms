using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.LogA.Dto;

namespace prod.Master.LogA.Exporting
{

	public interface IMstLgaBp2ProcessExcelExporter : IApplicationService
	{

		FileDto ExportToFile(List<MstLgaBp2ProcessDto> mstlgabp2process);

	}

}


