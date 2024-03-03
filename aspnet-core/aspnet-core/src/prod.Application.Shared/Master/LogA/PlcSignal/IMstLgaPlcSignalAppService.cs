using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.LogA.Dto;

namespace prod.Master.LogA
{

	public interface IMstLgaPlcSignalAppService : IApplicationService
	{

		Task<PagedResultDto<MstLgaPlcSignalDto>> GetAll(GetMstLgaPlcSignalInput input);

		Task CreateOrEdit(CreateOrEditMstLgaPlcSignalDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetPlcSignalToExcel(GetMstLgaPlcSignalExcelInput input);


    }

}


