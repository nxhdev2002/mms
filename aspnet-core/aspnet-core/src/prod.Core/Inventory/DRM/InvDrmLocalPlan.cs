using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.DRM
{

    [Table("InvDrmLocalPlan")]
    public class InvDrmLocalPlan : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxSupplierNoLength = 10;

        public const int MaxShipmentNoLength = 10;

        public const int MaxCfcLength = 4;

        public const int MaxPartCodeLength = 60;

        public const int MaxMaterialCodeLength = 60;

        public const int MaxMaterialSpecLength = 500;

        public const int MaxRemarkLength = 1000;

        [StringLength(MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        public virtual DateTime? DeliveryDate { get; set; }

        [StringLength(MaxShipmentNoLength)]
        public virtual string ShipmentNo { get; set; }

        [StringLength(MaxCfcLength)]
        public virtual string Cfc { get; set; }

        [StringLength(MaxPartCodeLength)]
        public virtual string PartCode { get; set; }

        [StringLength(MaxMaterialCodeLength)]
        public virtual string MaterialCode { get; set; }

        [StringLength(MaxMaterialSpecLength)]
        public virtual string MaterialSpec { get; set; }

        public virtual int? Qty { get; set; }

        public virtual DateTime? DeliveryMonth { get; set; }

        public virtual DateTime? DelayDelivery { get; set; }

        [StringLength(MaxRemarkLength)]
        public virtual string Remark { get; set; }
    }

}