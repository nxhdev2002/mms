using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Assy.Andon.Dto;

namespace prod.Assy.Andon
{

	public interface IAsyAdoVehicleDetailsAppService : IApplicationService
	{

		Task<PagedResultDto<AsyAdoVehicleDetailsDto>> GetAll(GetAsyAdoVehicleDetailsInput input);

		Task<FileDto> GetVehicleDetailsToExcel(GetAsyAdoVehicleDetailsExportInput input);


    }

}
