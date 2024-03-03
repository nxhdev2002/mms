using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.CKD.Dto
{

    public class InvCkdContainerIntransitExportInput
    {

        public virtual string ContainerNo { get; set; }

        public virtual string Renban { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual DateTime? ShippingDate { get; set; }

        public virtual DateTime? PortDate { get; set; }

        public virtual DateTime? TransDate { get; set; }

        public virtual DateTime? TmvDate { get; set; }

        public virtual decimal? Cif { get; set; }

        public virtual decimal? Tax { get; set; }

        public virtual decimal? Inland { get; set; }

        public virtual decimal? Thc { get; set; }

        public virtual decimal? Fob { get; set; }

        public virtual decimal? Freight { get; set; }

        public virtual decimal? Insurance { get; set; }

        public virtual decimal? Amount { get; set; }

        public virtual string Status { get; set; }

        public virtual DateTime? PeriodDate { get; set; }

        public virtual long? PeriodId { get; set; }

        public virtual decimal? CifVn { get; set; }

        public virtual decimal? TaxVn { get; set; }

        public virtual decimal? InlandVn { get; set; }

        public virtual decimal? ThcVn { get; set; }

        public virtual decimal? FobVn { get; set; }

        public virtual decimal? FreightVn { get; set; }

        public virtual decimal? InsuranceVn { get; set; }

        public virtual decimal? AmountVn { get; set; }

        public virtual string Forwarder { get; set; }

        public virtual string Generated { get; set; }

        public virtual string Shop { get; set; }

        public virtual string IsActive { get; set; }

    }

}


