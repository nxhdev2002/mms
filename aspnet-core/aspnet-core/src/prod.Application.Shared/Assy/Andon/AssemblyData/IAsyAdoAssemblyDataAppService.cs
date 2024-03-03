using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using prod.Assy.Andon.Dto;
using prod.Dto;

namespace prod.Assy.Andon
{
	public interface IAsyAdoAssemblyDataAppService : IApplicationService
	{
		Task<PagedResultDto<AsyAdoAssemblyDataDto>> GetAll(GetAsyAdoAssemblyDataInput input);
		 
		Task<FileDto> GetAssemblyDataToExcel(GetAsyAdoAssemblyDataInput input);

    }

}


