using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.Welding.Dto;

namespace prod.Master.Welding
{

	public interface IMstWldPunchIndicatorAppService : IApplicationService
	{

		Task<PagedResultDto<MstWldPunchIndicatorDto>> GetAll(GetMstWldPunchIndicatorInput input);

		Task CreateOrEdit(CreateOrEditMstWldPunchIndicatorDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetPunchIndicatorToExcel(MstWldPunchIndicatorExportInput input);


    }

}


