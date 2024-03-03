using System.Threading.Tasks;
using prod.Authorization.Users;

namespace prod.WebHooks
{
    public interface IAppWebhookPublisher
    {
        Task PublishTestWebhook();
    }
}
