using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;using prod.Dto;
using System.Threading.Tasks;
using prod.LogW.Ado.Dto;

namespace prod.LogW.Ado.Exporting
{

	public interface ILgwAdoCallingLightStatusExcelExporter : IApplicationService
	{

		FileDto ExportToFile(List<LgwAdoCallingLightStatusDto> lgwadocallinglightstatus);

	}

}

