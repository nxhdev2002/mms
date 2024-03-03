using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Inventory.CKD.PhysicalStockIssuing.Dto;
using System.Threading.Tasks;

namespace prod.Inventory.CKD
{
    public interface IInvCkdPhysicalStockIssuingAppService : IApplicationService
    {
        Task<PagedResultDto<InvCkdPhysicalStockIssuingDto>> GetDataPhysicalStockIssuing(InvCkdPhysicalStockIssuingInputDto input);

    }
}
