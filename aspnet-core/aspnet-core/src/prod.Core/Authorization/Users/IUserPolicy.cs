using System.Threading.Tasks;
using Abp.Domain.Policies;

namespace prod.Authorization.Users
{
    public interface IUserPolicy : IPolicy
    {
        Task CheckMaxUserCountAsync(int tenantId);
    }
}
