using Abp.Application.Services;
using Abp.Application.Services.Dto;
﻿using Abp.Application.Services;
using System.Collections.Generic;
using prod.Dto;
using prod.Inv.D125.Dto;

namespace prod.Inv.D125.Exporting
{
	public interface IInvOutLineOffExcelExporter : IApplicationService
	{
		FileDto ExportToFile(List<InvOutLineOffDto> invoutlineoff);

	}

}


