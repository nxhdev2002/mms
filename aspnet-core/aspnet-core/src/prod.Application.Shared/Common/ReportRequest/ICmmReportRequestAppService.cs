using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Common.Dto;

namespace prod.Common
{
	public interface ICmmReportRequestAppService : IApplicationService
	{
		Task<PagedResultDto<CmmReportRequestDto>> GetDatabyUser(GetCmmReportRequestInput input);
	}
}


