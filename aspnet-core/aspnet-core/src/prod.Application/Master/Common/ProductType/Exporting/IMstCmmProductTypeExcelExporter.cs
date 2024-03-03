using Abp.Application.Services;
using prod.Dto;
using prod.Master.Common.Dto;
using System.Collections.Generic;

namespace prod.Master.Common.Exporting
{

	public interface IMstCmmProductTypeExcelExporter : IApplicationService
	{

		FileDto ExportToFile(List<MstCmmProductTypeDto> mstcmmproducttype);

	}

}