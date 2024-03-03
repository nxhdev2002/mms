using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Inventory.BOM.Dto;
using System.Threading.Tasks;

namespace prod.Inventory.BOM.GenBOMData
{
    public interface IInvGenBOMDataAppService : IApplicationService
    {
        Task<PagedResultDto<InvGenBOMDataDto>> GetAll(GetInvGenBOMDataInput input);
    }
}
