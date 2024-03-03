using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Master.Inventory.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace prod.Master.Inventory
{
	public interface IMstInvDemDetDaysAppService : IApplicationService
	{
		Task<PagedResultDto<MstInvDemDetDaysDto>> GetAll(GetMstInvDemDetDaysInput input);

		Task CreateOrEdit(CreateOrEditMstInvDemDetDaysDto input);
		Task Delete(EntityDto input);
		Task<List<MstInvDemDetDaysImportDto>> ImportMstInvDemDetDaysFromExcel(byte[] fileBytes, string fileName);
	}
}
