using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Painting.Andon.Dto;

namespace prod.Painting.Andon
{

	public interface IPtsAdoLineEfficiencyDetailsAppService : IApplicationService
	{

		Task<PagedResultDto<PtsAdoLineEfficiencyDetailsDto>> GetAll(GetPtsAdoLineEfficiencyDetailsInput input);

		Task CreateOrEdit(CreateOrEditPtsAdoLineEfficiencyDetailsDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetLineEfficiencyDetailsToExcel(GetPtsAdoLineEfficiencyDetailsExportInput input);


    }

}


