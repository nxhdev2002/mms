﻿using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;using prod.Dto;
using System.Threading.Tasks;
using prod.LogW.Mwh.Dto;

namespace prod.LogW.Mwh
{

	public interface ILgwMwhPartDataAppService : IApplicationService
	{

		Task<PagedResultDto<LgwMwhPartDataDto>> GetAll(GetLgwMwhPartDataInput input);

		//Task CreateOrEdit(CreateOrEditLgwMwhPartDataDto input);

		//Task Delete(EntityDto input);

	}

}

