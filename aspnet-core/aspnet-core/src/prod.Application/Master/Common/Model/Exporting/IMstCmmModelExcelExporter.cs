using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.Cmm.Dto;

namespace prod.Master.Cmm.Exporting
{

	public interface IMstCmmModelExcelExporter : IApplicationService
	{

		FileDto ExportToFile(List<MstCmmModelDto> mstcmmmodel);
		FileDto ExportLotCodeToFile(List<MstCmmLotCodeGradeDto> lotcodegrades);

	}

}


