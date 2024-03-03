using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Master.Common.Dto;
using System.Threading.Tasks;

namespace prod.Master.Common
{

	public interface IMstCmmProductGroupAppService : IApplicationService
	{
		Task<PagedResultDto<MstCmmProductGroupDto>> GetAll(GetMstCmmProductGroupInput input);
	}

}

