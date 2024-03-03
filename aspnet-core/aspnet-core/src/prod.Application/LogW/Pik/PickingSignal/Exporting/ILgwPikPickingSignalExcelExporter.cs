using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;using prod.Dto;
using System.Threading.Tasks;
using prod.LogW.Pik.Dto;

namespace prod.LogW.Pik.Exporting
{

	public interface ILgwPikPickingSignalExcelExporter : IApplicationService
	{

		FileDto ExportToFile(List<LgwPikPickingSignalDto> lgwpikpickingsignal);

	}

}


