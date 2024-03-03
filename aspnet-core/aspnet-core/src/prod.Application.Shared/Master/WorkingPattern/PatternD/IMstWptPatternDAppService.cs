using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.WorkingPattern.Dto;

namespace prod.Master.WorkingPattern
{

	public interface IMstWptPatternDAppService : IApplicationService
	{

		Task<PagedResultDto<MstWptPatternDDto>> GetAll(GetMstWptPatternDInput input);

		Task CreateOrEdit(CreateOrEditMstWptPatternDDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetPatternDToExcel(MstWptPatternDExportInput input);


    }

}


