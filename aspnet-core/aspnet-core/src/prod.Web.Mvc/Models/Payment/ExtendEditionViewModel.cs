using System.Collections.Generic;
using prod.Editions.Dto;
using prod.MultiTenancy.Payments;

namespace prod.Web.Models.Payment
{
    public class ExtendEditionViewModel
    {
        public EditionSelectDto Edition { get; set; }

        public List<PaymentGatewayModel> PaymentGateways { get; set; }
    }
}