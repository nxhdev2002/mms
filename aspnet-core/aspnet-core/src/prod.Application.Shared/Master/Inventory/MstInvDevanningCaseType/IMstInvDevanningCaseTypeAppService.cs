using Abp.Application.Services.Dto;
using prod.Master.Common.Dto;
using prod.Master.Inventory.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace prod.Master.Inventory
{
	public interface IMstInvDevanningCaseTypeAppService
	{
		Task<PagedResultDto<MstInvDevanningCaseTypeDto>> GetAll(GetMstInvDevanningCaseTypeInput input);

		Task CreateOrEdit(CreateOrEditMstInvDevanningCaseTypeDto input);

		Task Delete(EntityDto input);
	}
}
