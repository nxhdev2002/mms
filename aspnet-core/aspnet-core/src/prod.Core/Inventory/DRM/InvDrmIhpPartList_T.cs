using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.DRM
{
    [Table("InvDrmIhpPartList_T")]
  
    public class InvDrmIhpPartList_T : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxGuidLength = 128;

        public const int MaxSupplierTypeLength = 20;

        public const int MaxSupplierIhpLength = 20;

        public const int MaxSupplierDrmLength = 20;  

        public const int MaxCfcLength = 4;

        public const int MaxCfcIhpLength = 4;

        public const int MaxMaterialCodeLength = 40;

        public const int MaxMaterialSpecLength = 200;

        public const int MaxSpecLength = 200;

        public const int MaxPartCodeLength = 60;

        public const int MaxPartNoLength = 60;

        public const int MaxPartNameLength = 300;

        public const int MaxSizeLength = 40;

        public const int MaxGradeLength = 3;

        public const int MaxErrorDescriptionLength = 5000;



        [StringLength(MaxGuidLength)]
        public virtual string Guid { get; set; }

        [StringLength(MaxSupplierTypeLength)]
        public virtual string SupplierType { get; set; }

        [StringLength(MaxSupplierDrmLength)]
        public virtual string SupplierDrm { get; set; }

        [StringLength(MaxCfcLength)]
        public virtual string Cfc { get; set; }

        [StringLength(MaxMaterialCodeLength)]
        public virtual string MaterialCode { get; set; }

        [StringLength(MaxMaterialSpecLength)]
        public virtual string MaterialSpec { get; set; }

        public virtual int? BoxQty { get; set; }

        //
        [StringLength(MaxSupplierIhpLength)]
        public virtual string SupplierIhp { get; set; }

        [StringLength(MaxCfcIhpLength)]
        public virtual string CfcIhp { get; set; }

        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(MaxPartNameLength)]
        public virtual string PartName { get; set; }

        //
        [StringLength(MaxPartCodeLength)]
        public virtual string PartCode { get; set; }

        [StringLength(MaxSpecLength)]
        public virtual string Spec { get; set; }

        [StringLength(MaxSizeLength)]
        public virtual string Size { get; set; }

        public virtual DateTime? FirstDayProduct { get; set; }
        public virtual DateTime? LastDayProduct { get; set; }

        [StringLength(MaxGradeLength)]
        public virtual string Grade { get; set; }

        public virtual int? UsageQty { get; set; }

        [StringLength(MaxErrorDescriptionLength)]
        public virtual string ErrorDescription { get; set; }

    }

}
