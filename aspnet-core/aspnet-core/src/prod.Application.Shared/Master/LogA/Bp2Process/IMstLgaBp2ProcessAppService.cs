using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.LogA.Dto;
using prod.Master.LogA.Bp2Process.ImportDto;

namespace prod.Master.LogA
{

	public interface IMstLgaBp2ProcessAppService : IApplicationService
	{

		Task<PagedResultDto<MstLgaBp2ProcessDto>> GetAll(GetMstLgaBp2ProcessInput input);

		Task CreateOrEdit(CreateOrEditMstLgaBp2ProcessDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetBp2ProcessToExcel(GetMstLgaBp2ProcessExcelInput input);

        Task<List<ImportMstLgaBp2ProcessDto>> ImportMstLgaBp2ProcessFromExcel(List<ImportMstLgaBp2ProcessDto> MstLgaBp2ProcessS);

		Task MergeDataMstLgaBp2Process(string v_Guid);

	}

}


