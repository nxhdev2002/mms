using System;
namespace prod.Inventory.CPS.Dto
{

    public class InvCpsPoHeadersExportInput
    {

        public virtual string TypeLookupCode { get; set; }

        public virtual string PoNumber { get; set; }

        public virtual long? VendorId { get; set; }

        public virtual string CurrencyCode { get; set; }

        public virtual string AuthorizationStatus { get; set; }

        public virtual decimal? TotalPrice { get; set; }

        public virtual decimal? TotalPriceUsd { get; set; }

        public virtual DateTime? ApprovedDate { get; set; }

        public virtual string Comments { get; set; }

        public virtual DateTime? SubmitDate { get; set; }

        public virtual string IsActive { get; set; }

    }

}


