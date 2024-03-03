using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using prod.Inventory.CKD.Dto;
using Abp.ObjectComparators.DateTimeComparators;
using prod.Dto;

namespace prod.Inventory.CKD
{

    public interface IInvCkdContainerRentalWHPlanAppService : IApplicationService
    {

        Task<PagedResultDto<InvCkdContainerRentalWHPlanDto>> GetAll(GetInvCkdContainerRentalWHPlanInput input);



        Task<PagedResultDto<InvCkdContainerRentalWHPlanDto>> GetDataLateDate();

        Task CreateOrEdit(CreateOrEditInvCkdContainerRentalWHPlanDto input);

        Task Delete(EntityDto input);

        Task<List<InvCkdContainerRentalWHPlanTDto>> GetImportDataFromExcel(byte[] fileBytes, string fileName);


        Task<int> ConFirmStatus(string p_container_id, string p_status);

		Task<int> ConFirmStatusMultiCkb(string listIdStatus, string status);

        Task<List<InvCkdContainerRentalWHPlanDetailsImportDto>> ImportRepackTransferFromExcel(byte[] fileBytes, string fileName);

    }

}


