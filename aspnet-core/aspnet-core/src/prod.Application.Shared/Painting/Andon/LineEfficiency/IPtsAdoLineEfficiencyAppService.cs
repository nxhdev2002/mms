using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Painting.Andon.Dto;

namespace prod.Painting.Andon
{

	public interface IPtsAdoLineEfficiencyAppService : IApplicationService
	{

		Task<PagedResultDto<PtsAdoLineEfficiencyDto>> GetAll(GetPtsAdoLineEfficiencyInput input);

		Task CreateOrEdit(CreateOrEditPtsAdoLineEfficiencyDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetLineEfficiencyToExcel(GetPtsAdoLineEfficiencyExportInput input);


    }

}


