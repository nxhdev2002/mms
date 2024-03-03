using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Welding.Andon.Dto;
using prod.Welding.Andon.ImportDto;

namespace prod.Welding.Andon
{
	public interface IWldAdoEdInAppService : IApplicationService
	{
		Task<List<ImportWldAdoEdInDto>> ImportWldAdoEdInFromExcel(List<ImportWldAdoEdInDto> WeldingPlans);
    }
}


