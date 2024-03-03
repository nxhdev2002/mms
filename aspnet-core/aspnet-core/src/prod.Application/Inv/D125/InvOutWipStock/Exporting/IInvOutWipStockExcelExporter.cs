using Abp.Application.Services;
using System.Collections.Generic;using prod.Dto;
using prod.Inv.D125.Dto;

namespace prod.Inv.D125.Exporting
{
	public interface IInvOutWipStockExcelExporter : IApplicationService
	{
		FileDto ExportToFile(List<InvOutWipStockDto> invoutwipstock);
	}

}


