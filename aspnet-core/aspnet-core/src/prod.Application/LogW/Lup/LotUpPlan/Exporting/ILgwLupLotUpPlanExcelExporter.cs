using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.LogW.Lup.Dto;

namespace prod.LogW.Lup.Exporting
{

	public interface ILgwLupLotUpPlanExcelExporter : IApplicationService
	{

		FileDto ExportToFile(List<LgwLupLotUpPlanDto> lgwluplotupplan);

	}

}


