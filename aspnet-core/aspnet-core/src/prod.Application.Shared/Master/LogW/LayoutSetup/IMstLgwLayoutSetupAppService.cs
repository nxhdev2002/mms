﻿using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.LogW.Dto;

namespace prod.Master.LogW
{

	public interface IMstLgwLayoutSetupAppService : IApplicationService
	{

		Task<PagedResultDto<MstLgwLayoutSetupDto>> GetAll(GetMstLgwLayoutSetupInput input);

		Task CreateOrEdit(CreateOrEditMstLgwLayoutSetupDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetLayoutSetupToExcel(MstLgwLayoutSetupExportInput input);


    }

}

