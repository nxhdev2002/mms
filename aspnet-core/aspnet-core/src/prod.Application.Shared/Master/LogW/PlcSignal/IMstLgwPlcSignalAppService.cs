using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;using prod.Dto;
using System.Threading.Tasks;
using prod.Master.LogW.Dto;

namespace prod.Master.LogW
{

	public interface IMstLgwPlcSignalAppService : IApplicationService
	{

		Task<PagedResultDto<MstLgwPlcSignalDto>> GetAll(GetMstLgwPlcSignalInput input);

        //Task CreateOrEdit(CreateOrEditMstLgwPlcSignalDto input);

        //Task Delete(EntityDto input);

        Task<FileDto> GetPlcSignalToExcel(MstLgwPlcSignalExportInput input);


    }

}


