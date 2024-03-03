using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.LogA.Bp2.Dto;

namespace prod.LogA.Bp2.Exporting
{

	public interface ILgaBp2ProgressExcelExporter : IApplicationService
	{

		FileDto ExportToFile(List<LgaBp2ProgressDto> lgabp2progress);

	}

}


