using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Inventory.DRM.Dto;
using prod.Inventory.CKD.Dto;

namespace prod.Inventory.DRM
{

    public interface IInvDrmImportPlanAppService : IApplicationService
    {

        Task<PagedResultDto<InvDrmImportPlanDto>> GetAll(GetInvDrmImportPlanInput input);

        //Task CreateOrEdit(CreateOrEditInvDrmImportPlanDto input);


        Task<List<InvDrmImportPlanImportDto>> ImportDataInvDrmImportPlanFromExcel(byte[] fileBytes, string fileName);

        Task<int> ConFirmStatus(string p_container_id, string p_status);

        Task<int> ConFirmStatusMultiCkb(string listIdStatus, string status);


    }

}


