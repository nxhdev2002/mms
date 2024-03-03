using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.LogW.Dto;

namespace prod.Master.LogW
{

	public interface IMstLgwContDevanningLTAppService : IApplicationService
	{

		Task<PagedResultDto<MstLgwContDevanningLTDto>> GetAll(GetMstLgwContDevanningLTInput input);

		Task CreateOrEdit(CreateOrEditMstLgwContDevanningLTDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetContDevanningLTToExcel(MstLgwContDevanningLTExportInput input);



        Task<List<ImportContDevanningLTDto>> ImportContDevanningLTFromExcel(List<ImportContDevanningLTDto> LotUpPlans);


		Task MergeDataContDevanningLT(string v_Guid);

	}

}


