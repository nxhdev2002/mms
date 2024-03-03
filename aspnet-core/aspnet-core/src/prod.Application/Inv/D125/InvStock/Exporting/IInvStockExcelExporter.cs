using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;using prod.Dto;
using prod.Inv.D125.Dto;

namespace prod.Inv.D125.Exporting
{
	public interface IInvStockExcelExporter : IApplicationService
	{
		FileDto ExportToFile(List<InvStockDto> invstock);

	}

}

