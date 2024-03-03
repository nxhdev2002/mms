using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.WorkingPattern.Dto;

namespace prod.Master.WorkingPattern
{

	public interface IMstWptWeekAppService : IApplicationService
	{

		Task<PagedResultDto<MstWptWeekDto>> GetAll(GetMstWptWeekInput input);

		Task CreateOrEdit(CreateOrEditMstWptWeekDto input);

		Task Delete(EntityDto input);
		Task<FileDto> GetWeekToExcel(MstWptWeekExportInput input);



    }

}


