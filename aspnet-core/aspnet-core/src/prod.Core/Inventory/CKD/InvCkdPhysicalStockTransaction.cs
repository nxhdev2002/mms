using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.CKD
{
    [Table("InvCkdPhysicalStockTransaction")]
    [Index(nameof(PartNo), Name = "IX_InvCkdPhysicalStockTransaction_PartNo")]
    [Index(nameof(PartListId), Name = "IX_InvCkdPhysicalStockTransaction_PartListId")]
    [Index(nameof(PartListGradeId), Name = "IX_InvCkdPhysicalStockTransaction_PartListGradeId")]
    [Index(nameof(MaterialId), Name = "IX_InvCkdPhysicalStockTransaction_MaterialId")]
    [Index(nameof(LotNo), Name = "IX_InvCkdPhysicalStockTransaction_LotNo")]
    [Index(nameof(ReferenceId), Name = "IX_InvCkdPhysicalStockTransaction_ReferenceId")]
    [Index(nameof(WorkingDate), Name = "IX_InvCkdPhysicalStockTransaction_WorkingDate")]
    [Index(nameof(PeriodId), Name = "IX_InvCkdPhysicalStockTransaction_PeriodId")]
    public class InvCkdPhysicalStockTransaction : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxPartNoLength = 12;

        public const int MaxPartNoNormalizedLength = 12;

        public const int MaxPartNameLength = 300;

        public const int MaxPartNoNormalizedS4Length = 10;

        public const int MaxColorSfxLength = 2;

        public const int MaxLotNoLength = 20;

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

        public virtual long? PartListId { get; set; }

        public virtual long? PartListGradeId { get; set; }

        public virtual long? MaterialId { get; set; }

        public virtual decimal? Qty { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? TransactionDatetime { get; set; }

        [StringLength(MaxLotNoLength)]
        public virtual string LotNo { get; set; }

        public virtual long? ReferenceId { get; set; }

        public virtual DateTime? WorkingDate { get; set; }

        public virtual long? PeriodId { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}
