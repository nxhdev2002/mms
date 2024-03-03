using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.CKD
{

    [Table("InvCkdPhysicalStockPartPeriod")]
    [Index(nameof(PartNo), Name = "IX_InvCkdPhysicalStockPartPeriod_PartNo")]
    [Index(nameof(LotNo), Name = "IX_InvCkdPhysicalStockPartPeriod_LotNo")]
    [Index(nameof(PartListId), Name = "IX_InvCkdPhysicalStockPartPeriod_PartListId")]
    [Index(nameof(MaterialId), Name = "IX_InvCkdPhysicalStockPartPeriod_MaterialId")]
    [Index(nameof(PeriodId), Name = "IX_InvCkdPhysicalStockPartPeriod_PeriodId")]
    public class InvCkdPhysicalStockPartPeriod : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxPartNoLength = 12;

        public const int MaxPartNoNormalizedLength = 12;

        public const int MaxPartNameLength = 300;

        public const int MaxPartNoNormalizedS4Length = 10;

        public const int MaxColorSfxLength = 2;

        public const int MaxLotNoLength = 20;

        public const int MaxRemarkLength = 1000;

        public const int MaxIsActiveLength = 1;

        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(MaxPartNoNormalizedLength)]
        public virtual string PartNoNormalized { get; set; }

        [StringLength(MaxPartNameLength)]
        public virtual string PartName { get; set; }

        [StringLength(MaxPartNoNormalizedS4Length)]
        public virtual string PartNoNormalizedS4 { get; set; }

        [StringLength(MaxColorSfxLength)]
        public virtual string ColorSfx { get; set; }

        [StringLength(MaxLotNoLength)]
        public virtual string LotNo { get; set; }

        public virtual long? PartListId { get; set; }

        public virtual long? MaterialId { get; set; }

        public virtual int? BeginQty { get; set; }

        public virtual int? ReceiveQty { get; set; }

        public virtual int? IssueQty { get; set; }

        public virtual int? CalculatorQty { get; set; }

        public virtual int? ActualQty { get; set; }

        public virtual long? PeriodId { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? LastCalDatetime { get; set; }

        public virtual int? Transtype { get; set; }

        [StringLength(MaxRemarkLength)]
        public virtual string Remark { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}
