using System.Threading.Tasks;
using Abp.Application.Services;
using prod.Sessions.Dto;

namespace prod.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();

        Task<UpdateUserSignInTokenOutput> UpdateUserSignInToken();
    }
}
