using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.LogW.Dto;

namespace prod.Master.LogW
{

	public interface IMstLgwRobbingLaneAppService : IApplicationService
	{

		Task<PagedResultDto<MstLgwRobbingLaneDto>> GetAll(GetMstLgwRobbingLaneInput input);

		Task CreateOrEdit(CreateOrEditMstLgwRobbingLaneDto input);

		Task Delete(EntityDto input);
		Task<FileDto> GetRobbingLaneToExcel(MstLgwRobbingLaneExportInput input);



    }

}


