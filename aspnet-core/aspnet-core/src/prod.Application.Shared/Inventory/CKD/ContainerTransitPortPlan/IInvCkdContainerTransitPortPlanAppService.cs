using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;

using System.Threading.Tasks;

using prod.Inventory.CKD.Dto;

namespace prod.Inventory.CKD
{

    public interface IInvCkdContainerTransitPortPlanAppService : IApplicationService
    {

        Task<PagedResultDto<InvCkdContainerTransitPortPlanDto>> GetAll(GetInvCkdContainerTransitPortPlanInput input);

        Task CreateOrEdit(CreateOrEditInvCkdContainerTransitPortPlanDto input);

        Task Delete(EntityDto input);

        Task<PagedResultDto<InvCkdContainerTransitPortPlanDto>> GetDataLateDate();

        Task<List<InvCkdContainerTransitPortPlanDto>> GetImportDataFromExcel(byte[] fileBytes, string fileName);

		Task<int> ConFirmStatus(string p_container_id, string p_status);

		Task<int> ConFirmStatusMultiCkb(string listIdStatus, string status);
	}

}


