using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.DRM
{

    [Table("InvDrmStockPart")]
    public class InvDrmStockPart : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxMaterialCodeLength = 40;

        public const int MaxMaterialSpecLength = 200;

        public const int MaxPartNoLength = 20;

        public const int MaxPartNameLength = 200;

        public const int MaxSupplierNoLength = 10;

        public const int MaxCfcLength = 4;

        public const int MaxPartCodeLength = 50;

        public const int MaxModelLength = 20;

        public const int MaxGradeNameLength = 20;

        public const int MaxShiftLength = 50;


        [StringLength(MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        [StringLength(MaxCfcLength)]
        public virtual string Cfc { get; set; }

        [StringLength(MaxMaterialCodeLength)]
        public virtual string MaterialCode { get; set; }

        [StringLength(MaxMaterialSpecLength)]
        public virtual string MaterialSpec { get; set; }

        [StringLength(MaxPartCodeLength)]
        public virtual string PartCode { get; set; }

        public virtual long? DrmMaterialId { get; set; }

        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(MaxPartNameLength)]
        public virtual string PartName { get; set; }

        public virtual long? PartId { get; set; }

        public virtual long? MaterialId { get; set; }

        public virtual int? Qty { get; set; }

        public virtual DateTime? WorkingDate { get; set; }

        [StringLength(MaxModelLength)]
        public virtual string Model { get; set; }

        [StringLength(MaxGradeNameLength)]
        public virtual string GradeName { get; set; }

        public virtual int? Use { get; set; }

        public virtual int? Press { get; set; }

        public virtual int? IhpOh { get; set; }

        public virtual int? PressBroken { get; set; }

        public virtual int? Hand { get; set; }

        public virtual int? HandOh { get; set; }

        public virtual int? HandBroken { get; set; }

        public virtual int? MaterialIn { get; set; }

        public virtual int? MaterialInAddition { get; set; }

        [StringLength(MaxShiftLength)]
        public virtual string Shift { get; set; }
    }

}

