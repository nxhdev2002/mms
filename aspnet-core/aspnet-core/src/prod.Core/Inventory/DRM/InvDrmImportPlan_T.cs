using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.DRM
{
    [Table("InvDrmImportPlan_T")]
    public class InvDrmImportPlan_T : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxGuidLength = 128;

        public const int MaxSupplierNoLength = 10;

        public const int MaxShipmentNoLength = 10;

        public const int MaxCfcLength = 4;

        public const int MaxPartCodeLength = 60;

        public const int MaxPartNoLength = 12;

        public const int MaxPartNameLength = 300;

        public const int MaxRemarkLength = 1000;

        [StringLength(MaxGuidLength)]
        public virtual string Guid { get; set; }

        [StringLength(MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        public virtual DateTime? Etd { get; set; }

        public virtual DateTime? Eta { get; set; }

        [StringLength(MaxShipmentNoLength)]
        public virtual string ShipmentNo { get; set; }

        [StringLength(MaxCfcLength)]
        public virtual string Cfc { get; set; }

        [StringLength(MaxPartCodeLength)]
        public virtual string PartCode { get; set; }

        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(MaxPartNameLength)]
        public virtual string PartName { get; set; }

        public virtual int? Qty { get; set; }

        public virtual DateTime? PackingMonth { get; set; }

        public virtual DateTime? DelayEtd { get; set; }

        public virtual DateTime? DelayEta { get; set; }

        [StringLength(MaxRemarkLength)]
        public virtual string Remark { get; set; }

        public virtual DateTime? Ata { get; set; }
    }

}

