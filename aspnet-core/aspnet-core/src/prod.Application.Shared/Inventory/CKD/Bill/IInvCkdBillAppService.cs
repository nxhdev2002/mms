using Abp.Application.Services.Dto;
using Abp.Application.Services;
using System.Threading.Tasks;
using prod.Inventory.CKD.Dto;

namespace prod.Inventory.CKD
{

    public interface IInvCkdBillAppService : IApplicationService
    {

        Task<PagedResultDto<InvCkdBillDto>> GetAll(GetInvCkdBillInput input);

    }

}


