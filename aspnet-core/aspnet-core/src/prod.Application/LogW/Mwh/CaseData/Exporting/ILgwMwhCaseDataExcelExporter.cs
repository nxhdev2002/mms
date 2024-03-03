using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.LogW.Mwh.Dto;

namespace prod.LogW.Mwh.Exporting
{

	public interface ILgwMwhCaseDataExcelExporter : IApplicationService
	{

		FileDto ExportToFile(List<LgwMwhCaseDataDto> lgwmwhcasedata);

		FileDto ExportWhBigCaseDataToFile(List<LgwMwhCaseDataDto> lgwmwhcasedata);

		FileDto ExportRobbingDataToFile(List<LgwMwhCaseDataDto> lgwmwhcasedata);

	}

}


