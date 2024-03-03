using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;
using prod.Inventory.CKD.Dto;

namespace prod.Inventory.CKD
{

    public interface IInvCkdPhysicalStockPartPeriodAppService : IApplicationService
    {

        Task<PagedResultDto<InvCkdPhysicalStockPartPeriodDto>> GetAll(GetInvCkdPhysicalStockPartPeriodInput input);

        Task CreateOrEdit(CreateOrEditInvCkdPhysicalStockPartPeriodDto input);

        Task Delete(EntityDto input);

    }

}