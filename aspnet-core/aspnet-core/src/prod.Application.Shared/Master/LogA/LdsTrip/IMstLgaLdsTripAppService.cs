using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.LogA.Dto;

namespace prod.Master.LogA
{

	public interface IMstLgaLdsTripAppService : IApplicationService
	{

		Task<PagedResultDto<MstLgaLdsTripDto>> GetAll(GetMstLgaLdsTripInput input);

		Task CreateOrEdit(CreateOrEditMstLgaLdsTripDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetTripToExcel(GetMstLgaLdsTripExcelInput input);


    }

}


