using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Inventory.IHP.Dto;
using System.Threading.Tasks;

namespace prod.Inventory.IHP
{
    public interface IInvIhpMatCustomDeclareAppService : IApplicationService
    {
        Task<PagedResultDto<InvIphMatCustomDeclareDto>> GetAllCustomDeclare(GetInvIphMatCustomDeclareInput input);

        Task<PagedResultDto<InvIphMatCustomDeclareDetailsDto>> GetCustomDeclareDetails(GetInvIphMatCustomDeclareDetailsInput input);
    }
}
