using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.Painting.Dto;

namespace prod.Master.Painting
{

	public interface IMstPtsPaintingProcessAppService : IApplicationService
	{

		Task<PagedResultDto<MstPtsPaintingProcessDto>> GetAll(GetMstPtsPaintingProcessInput input);

		Task CreateOrEdit(CreateOrEditMstPtsPaintingProcessDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetPaintingProcessToExcel(MstPtsPaintingProcessExportInput input);


    }

}
