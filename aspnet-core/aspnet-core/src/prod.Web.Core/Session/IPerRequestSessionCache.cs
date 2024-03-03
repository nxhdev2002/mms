using System.Threading.Tasks;
using prod.Sessions.Dto;

namespace prod.Web.Session
{
    public interface IPerRequestSessionCache
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformationsAsync();
    }
}
