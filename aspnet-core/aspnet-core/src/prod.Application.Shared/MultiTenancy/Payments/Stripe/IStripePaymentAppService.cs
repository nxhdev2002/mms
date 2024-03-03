using System.Threading.Tasks;
using Abp.Application.Services;
using prod.MultiTenancy.Payments.Dto;
using prod.MultiTenancy.Payments.Stripe.Dto;

namespace prod.MultiTenancy.Payments.Stripe
{
    public interface IStripePaymentAppService : IApplicationService
    {
        Task ConfirmPayment(StripeConfirmPaymentInput input);

        StripeConfigurationDto GetConfiguration();

        Task<SubscriptionPaymentDto> GetPaymentAsync(StripeGetPaymentInput input);

        Task<string> CreatePaymentSession(StripeCreatePaymentSessionInput input);
    }
}