using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Inventory.CKD.Dto;

namespace prod.Inventory.CKD
{
    public interface IInvCkdProductionMappingAppService : IApplicationService
    {
        Task<PagedResultDto<InvCkdProductionMappingDto>> GetAll(GetInvCkdProductionMappingInput input);

        Task<List<InvCkdProductionMappingDto>> ImportInvCkdProductionMappingFromExcel(byte[] fileBytes, string fileName);
    }
}


