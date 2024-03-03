using System.Threading.Tasks;
using Abp.Webhooks;

namespace prod.WebHooks
{
    public interface IWebhookEventAppService
    {
        Task<WebhookEvent> Get(string id);
    }
}
