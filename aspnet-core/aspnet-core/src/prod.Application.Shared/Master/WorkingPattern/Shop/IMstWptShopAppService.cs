using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.WorkingPattern.Dto;

namespace prod.Master.WorkingPattern
{
	public interface IMstWptShopAppService : IApplicationService
	{
		Task<PagedResultDto<MstWptShopDto>> GetAll(GetMstWptShopInput input);

		Task CreateOrEdit(CreateOrEditMstWptShopDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetShopToExcel(MstWptShopExportInput input);

    }
}

