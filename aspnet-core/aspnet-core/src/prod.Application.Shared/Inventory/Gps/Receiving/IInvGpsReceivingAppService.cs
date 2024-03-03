using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Inventory.GPS.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace prod.Inventory.GPS
{

    public interface IInvGpsReceivingAppService : IApplicationService
    {

        Task<PagedResultDto<InvGpsReceivingDto>> GetAll(GetInvGpsReceivingInput input);

        Task CreateOrEdit(CreateOrEditInvGpsReceivingDto input);
        Task<CheckDto> spCheckPartNoMaterial(string PartNo);
        Task<List<InvGpsReceivingImportDto>> ImportDataInvGpsReceiveFromExcel(byte[] fileBytes, string fileName);

    }

}


