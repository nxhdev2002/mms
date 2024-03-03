using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Painting.Andon.Dto;

namespace prod.Painting.Andon
{

	public interface IPtsAdoTotalDelayAppService : IApplicationService
	{

		Task<PagedResultDto<PtsAdoTotalDelayDto>> GetAll(GetPtsAdoTotalDelayInput input);

		Task CreateOrEdit(CreateOrEditPtsAdoTotalDelayDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetTotalDelayToExcel(PtsAdoTotalDelayExportInput input);


    }

}


