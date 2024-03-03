using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.WorkingPattern.Dto;

namespace prod.Master.WorkingPattern
{

	public interface IMstWptDailyWorkingTimeAppService : IApplicationService
	{

		Task<PagedResultDto<MstWptDailyWorkingTimeDto>> GetAll(GetMstWptDailyWorkingTimeInput input);

		Task CreateOrEdit(CreateOrEditMstWptDailyWorkingTimeDto input);

		Task Delete(EntityDto input);

		Task<PagedResultDto<MstWptPatternDDto>> MstWptPatternD_GetsActive();

		Task<PagedResultDto<MstWptShopDto>> MstWptShop_GetsActive();

		Task<PagedResultDto<MstWptWorkingTypeDto>> MstWptWorkingType_GetsForDaily();

	}

}
