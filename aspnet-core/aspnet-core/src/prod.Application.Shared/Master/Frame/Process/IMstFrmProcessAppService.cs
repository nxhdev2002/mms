using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.Frame.Dto;

namespace prod.Master.Frame
{

	public interface IMstFrmProcessAppService : IApplicationService
	{

		Task<PagedResultDto<MstFrmProcessDto>> GetAll(GetMstFrmProcessInput input);

		Task CreateOrEdit(CreateOrEditMstFrmProcessDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetProcessToExcel(MstFrmProcessExportInput input);


    }

}


