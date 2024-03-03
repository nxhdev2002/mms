using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.CKD
{
    [Table("InvCkdPartList")]
    public class InvCkdPartList : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxPartNoLength = 12;

        public const int MaxPartNoNormalizedLength = 10;

        public const int MaxPartNameLength = 40;

        public const int MaxSupplierNoLength = 50;

        public const int MaxSupplierCdLength = 50;

        public const int MaxColorSfxLength = 10;

        public const int MaxIsActiveLength = 1;

        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(MaxPartNoNormalizedLength)]
        public virtual string PartNoNormalized { get; set; }

        [StringLength(MaxPartNameLength)]
        public virtual string PartName { get; set; }

        [StringLength(MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        [StringLength(MaxSupplierCdLength)]
        public virtual string SupplierCd { get; set; }

        [StringLength(MaxColorSfxLength)]
        public virtual string ColorSfx { get; set; }

        public virtual long? SupplierId { get; set; }

        public virtual long? MaterialId { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }
}


