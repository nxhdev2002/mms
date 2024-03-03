using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Inventory.CKD.Dto;

namespace prod.Inventory.CKD
{

    public interface IInvCkdPhysicalConfirmLotAppService : IApplicationService
    {
        Task<PagedResultDto<InvCkdPhysicalConfirmLotDto>> GetAll(GetInvCkdPhysicalConfirmLotInput input);
        Task<List<InvCkdPhysicalConfirmLot_TDto>> ImportDataInvCkdPhysicalConfirmFromExcel(byte[] fileBytes, string fileName);
    }
}


