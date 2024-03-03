using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.LogW.Dto;

namespace prod.Master.LogW
{

	public interface IMstLgwLayoutAppService : IApplicationService
	{

		Task<PagedResultDto<MstLgwLayoutDto>> GetAll(GetMstLgwLayoutInput input);

		Task<FileDto> GetLayoutToExcel(MstLgwLayoutExportInput input);


    }

}


