﻿using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.LogW.Pup.Dto;

namespace prod.LogW.Pup.Exporting
{

	public interface ILgwPupPxPUpPlanBaseExcelExporter : IApplicationService
	{

		FileDto ExportToFile(List<LgwPupPxPUpPlanBaseDto> lgwpuppxpupplanbase);

	}

}

