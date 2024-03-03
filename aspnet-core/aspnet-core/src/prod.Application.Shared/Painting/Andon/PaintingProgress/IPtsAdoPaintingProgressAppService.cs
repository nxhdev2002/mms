using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Painting.Andon.Dto;

namespace prod.Painting.Andon
{

	public interface IPtsAdoPaintingProgressAppService : IApplicationService
	{

		Task<PagedResultDto<PtsAdoPaintingProgressDto>> GetAll(GetPtsAdoPaintingProgressInput input);

		Task CreateOrEdit(CreateOrEditPtsAdoPaintingProgressDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetPaintingProgressToExcel(GetPtsAdoPaintingProgressExportInput input);

    }

}


