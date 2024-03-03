using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;using prod.Dto;
using System.Threading.Tasks;
using prod.LogW.Ado.Dto;

namespace prod.LogW.Ado
{

	public interface ILgwAdoCallingLightStatusAppService : IApplicationService
	{

		Task<PagedResultDto<LgwAdoCallingLightStatusDto>> GetAll(GetLgwAdoCallingLightStatusInput input);

		Task CreateOrEdit(CreateOrEditLgwAdoCallingLightStatusDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetCallingLightStatusToExcel(GetLgwAdoCallingLightStatusInput input);

		Task<List<LgwAdoCallingLightStatusDto>> GetCallingLightData(string prod_line);
	}

}


