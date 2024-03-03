using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using prod.Master.WorkingPattern.Dto;

namespace prod.Master.WorkingPattern
{

	public interface IMstWptCalendarAppService : IApplicationService
	{

		Task<PagedResultDto<MstWptCalendarDto>> GetAll(GetMstWptCalendarInput input);

		Task CreateOrEdit(CreateOrEditMstWptCalendarDto input);

		Task Delete(EntityDto input);

		Task GenerateAsync();

	}

}

