using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.WorkingPattern.Dto;

namespace prod.Master.WorkingPattern
{

	public interface IMstWptPatternHAppService : IApplicationService
	{

		Task<PagedResultDto<MstWptPatternHDto>> GetAll(GetMstWptPatternHInput input);

		Task CreateOrEdit(CreateOrEditMstWptPatternHDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetPatternHToExcel(MstWptPatternHExportInput input);


    }

}


