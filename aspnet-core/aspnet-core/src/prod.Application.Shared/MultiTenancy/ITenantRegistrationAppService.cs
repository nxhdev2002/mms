using System.Threading.Tasks;
using Abp.Application.Services;
using prod.Editions.Dto;
using prod.MultiTenancy.Dto;

namespace prod.MultiTenancy
{
    public interface ITenantRegistrationAppService: IApplicationService
    {
        Task<RegisterTenantOutput> RegisterTenant(RegisterTenantInput input);

        Task<EditionsSelectOutput> GetEditionsForSelect();

        Task<EditionSelectDto> GetEdition(int editionId);
    }
}