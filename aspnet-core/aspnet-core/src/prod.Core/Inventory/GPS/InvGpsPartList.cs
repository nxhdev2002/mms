using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.GPS
{
    [Table("InvGpsPartList")]
    public class InvGpsPartList : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxPartNoLength = 50;

        public const int MaxPartNoNormalizedLength = 100;

        public const int MaxPartNameLength = 100;

        public const int MaxSupplierNoLength = 50;

        public const int MaxUomLength = 50;

        public const int MaxTypeLength = 50;

        public const int MaxColorLength = 3;

        public const int MaxSummerRadioLength = 50;

        public const int MaxWinterRatioLength = 50;

        public const int MaxDiffRatioLength = 50;

        public const int MaxSeasonTypeLength = 50;

        public const int MaxRemarkLength = 5000;

        public const int MaxRemark1Length = 5000;

        public const int MaxCategoryLength = 200;

        public const int MaxIsPartColorLength = 1;



        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(MaxPartNoNormalizedLength)]
        public virtual string PartNoNormalized { get; set; }

        [StringLength(MaxPartNameLength)]
        public virtual string PartName { get; set; }

        [StringLength(MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        [StringLength(MaxUomLength)]
        public virtual string Uom { get; set; }

        [StringLength(MaxTypeLength)]
        public virtual string Type { get; set; }

        public virtual int? BoxQty { get; set; }

        [StringLength(MaxColorLength)]
        public virtual string Color { get; set; }

        [StringLength(MaxSummerRadioLength)]
        public virtual decimal? SummerRadio { get; set; }

        [StringLength(MaxWinterRatioLength)]
        public virtual decimal? WinterRatio { get; set; }

        [StringLength(MaxDiffRatioLength)]
        public virtual decimal? DiffRatio { get; set; }

        [StringLength(MaxSeasonTypeLength)]
        public virtual string SeasonType { get; set; }

        [StringLength(MaxRemarkLength)]
        public virtual string Remark { get; set; }

        [StringLength(MaxRemark1Length)]
        public virtual string Remark1 { get; set; }

        public virtual int MinLot { get; set; }

        [StringLength(MaxCategoryLength)]
        public virtual string Category { get; set; }

        public  virtual DateTime? StartDate { get; set; }

        public virtual DateTime? EndDate { get; set; }

        [StringLength(MaxIsPartColorLength)]
        public virtual string IsPartColor { get; set; }
    }
}

