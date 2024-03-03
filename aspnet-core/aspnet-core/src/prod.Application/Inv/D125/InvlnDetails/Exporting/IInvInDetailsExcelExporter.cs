using Abp.Application.Services;
using System.Collections.Generic;using prod.Dto;
using prod.Inv.D125.Dto;

namespace prod.Inv.D125.Exporting
{
	public interface IInvInDetailsExcelExporter : IApplicationService
	{
		FileDto ExportToFile(List<InvInDetailsDto> invindetails);

	}

}


