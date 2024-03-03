using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.CKD
{

    [Table("InvCkdContainerIntransit")]
    public class InvCkdContainerIntransit : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxContainerNoLength = 15;

        public const int MaxRenbanLength = 6;

        public const int MaxSupplierNoLength = 10;

        public const int MaxStatusLength = 10;

        public const int MaxForwarderLength = 10;

        public const int MaxGeneratedLength = 1;

        public const int MaxShopLength = 10;

        public const int MaxIsActiveLength = 1;

        [StringLength(MaxContainerNoLength)]
        public virtual string ContainerNo { get; set; }

        [StringLength(MaxRenbanLength)]
        public virtual string Renban { get; set; }

        [StringLength(MaxSupplierNoLength)]
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

        [StringLength(MaxStatusLength)]
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

        [StringLength(MaxForwarderLength)]
        public virtual string Forwarder { get; set; }

        [StringLength(MaxGeneratedLength)]
        public virtual string Generated { get; set; }

        [StringLength(MaxShopLength)]
        public virtual string Shop { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }
}

