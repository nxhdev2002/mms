using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Welding.Andon.Dto;

namespace prod.Welding.Andon.Exporting
{

	public interface IWldAdoPunchQueueExcelExporter : IApplicationService
	{

		FileDto ExportToFile(List<WldAdoPunchQueueDto> wldadopunchqueue);

	}

}


