using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.LogA.Lds.Dto;

namespace prod.LogA.Lds.Exporting
{

	public interface ILgaLdsLotPlanExcelExporter : IApplicationService
	{

		FileDto ExportToFile(List<LgaLdsLotPlanDto> lgaldslotplan);

	}

}


