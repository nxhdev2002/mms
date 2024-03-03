using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;using prod.Dto;
using System.Threading.Tasks;
using prod.Master.Cmm.Dto;

namespace prod.Master.Cmm
{

	public interface IMstCmmTaktTimeAppService : IApplicationService
	{

		Task<PagedResultDto<MstCmmTaktTimeDto>> GetAll(GetMstCmmTaktTimeInput input);

		Task CreateOrEdit(CreateOrEditMstCmmTaktTimeDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetTaktTimeToExcel(MstCmmTaktTimeExportInput input);


    }

}

