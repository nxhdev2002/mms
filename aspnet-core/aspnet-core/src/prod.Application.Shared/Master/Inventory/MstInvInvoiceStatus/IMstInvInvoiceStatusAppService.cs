using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Master.Inventory.Dto;
using System.Threading.Tasks;

namespace prod.Master.Inventory
{

    public interface IMstInvInvoiceStatusAppService : IApplicationService
    {

        Task<PagedResultDto<MstInvInvoiceStatusDto>> GetAll(GetMstInvInvoiceStatusInput input);

    }

}
