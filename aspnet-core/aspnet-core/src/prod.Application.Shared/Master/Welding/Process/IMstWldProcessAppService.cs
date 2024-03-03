using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.Welding.Dto;

namespace prod.Master.Welding
{

	public interface IMstWldProcessAppService : IApplicationService
	{

		Task<PagedResultDto<MstWldProcessDto>> GetAll(GetMstWldProcessInput input);

		Task CreateOrEdit(CreateOrEditMstWldProcessDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetProcessToExcel(MstWldProcessExportInput input);

    }

}


