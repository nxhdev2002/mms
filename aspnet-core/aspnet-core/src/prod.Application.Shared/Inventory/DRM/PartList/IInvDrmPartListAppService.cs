using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Inventory.DRM.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace prod.Inventory.DRM
{
    public interface IInvDrmPartListAppService : IApplicationService
    {
        Task<PagedResultDto<InvDrmPartListDto>> GetAll(GetInvDrmPartListInput input);

        Task<List<InvDrmIhpPartImportDto>> ImportInvDRMIHPPartFromExcel(byte[] fileBytes, string fileName);
    }
}
