using System.Collections.Generic;
using prod.Editions;
using prod.Editions.Dto;
using prod.MultiTenancy.Payments;
using prod.MultiTenancy.Payments.Dto;

namespace prod.Web.Models.Payment
{
    public class BuyEditionViewModel
    {
        public SubscriptionStartType? SubscriptionStartType { get; set; }

        public EditionSelectDto Edition { get; set; }

        public decimal? AdditionalPrice { get; set; }

        public EditionPaymentType EditionPaymentType { get; set; }

        public List<PaymentGatewayModel> PaymentGateways { get; set; }
    }
}
