using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Frame.Andon.Dto;

namespace prod.Frame.Andon
{

	public interface IFrmAdoFramePlanAppService : IApplicationService
	{

		Task<PagedResultDto<FrmAdoFramePlanDto>> GetAll(GetFrmAdoFramePlanInput input);

		Task<List<ImportFrmAdoFramePlanDto>> ImportFramePlan_V2(List<ImportFrmAdoFramePlanDto> framePlans);

		Task MergeDataFramePlan(string v_Guid);

		Task<FileDto> GetFramePlanToExcel(GetFrmAdoFramePlanExportInput input);


    }

}


