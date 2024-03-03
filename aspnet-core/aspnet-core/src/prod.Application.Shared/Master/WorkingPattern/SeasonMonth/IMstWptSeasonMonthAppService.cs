using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.WorkingPattern.Dto;

namespace prod.Master.WorkingPattern
{

	public interface IMstWptSeasonMonthAppService : IApplicationService
	{

		Task<PagedResultDto<MstWptSeasonMonthDto>> GetAll(GetMstWptSeasonMonthInput input);

		Task CreateOrEdit(CreateOrEditMstWptSeasonMonthDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetSeasonMonthToExcel(MstWptSeasonMonthExportInput input);


    }

}


