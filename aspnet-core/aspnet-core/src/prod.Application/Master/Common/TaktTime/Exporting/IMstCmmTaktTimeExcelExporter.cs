using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;using prod.Dto;
using System.Threading.Tasks;
using prod.Master.Cmm.Dto;
using prod.Dto;

namespace prod.Master.Cmm.Exporting
{

	public interface IMstCmmTaktTimeExcelExporter : IApplicationService
	{
		FileDto ExportToFile(List<MstCmmTaktTimeDto> mstcmmtakttime);

	}

}


