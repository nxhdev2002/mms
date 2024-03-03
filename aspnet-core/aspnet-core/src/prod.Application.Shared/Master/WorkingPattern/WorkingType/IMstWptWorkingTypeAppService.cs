using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.WorkingPattern.Dto;

namespace prod.Master.WorkingPattern
{

	public interface IMstWptWorkingTypeAppService : IApplicationService
	{

		Task<PagedResultDto<MstWptWorkingTypeDto>> GetAll(GetMstWptWorkingTypeInput input);

		Task CreateOrEdit(CreateOrEditMstWptWorkingTypeDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetWorkingTypeToExcel(MstWptWorkingTypeExportInput input);


    }

}


