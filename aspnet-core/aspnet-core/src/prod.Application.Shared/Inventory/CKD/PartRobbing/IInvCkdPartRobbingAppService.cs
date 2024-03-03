using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Inventory.CKD.PartRobbing.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace prod.Inventory.CKD
{
    public interface IInvCkdPartRobbingAppService : IApplicationService
    {
        Task<PagedResultDto<InvCkdPartRobbingDto>> GetPartRobbingSearch(GetPartRobbingInput input);

        Task<List<InvCkdPartRobbingImportDto>> ImportPartRobbingFromExcel(byte[] fileBytes, string fileName);
    }
}
