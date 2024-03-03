using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.CKD
{
    [Table("InvCkdStockReceiving")]
    [Index(nameof(PartNo), Name = "IX_InvCkdStockReceiving_PartNo")]
    [Index(nameof(PartListId), Name = "IX_InvCkdStockReceiving_PartListId")]
    [Index(nameof(PartListGradeId), Name = "IX_InvCkdStockReceiving_PartListGradeId")]
    [Index(nameof(MaterialId), Name = "IX_InvCkdStockReceiving_MaterialId")]
    [Index(nameof(ReferenceId), Name = "IX_InvCkdStockReceiving_ReferenceId")]
    [Index(nameof(WorkingDate), Name = "IX_InvCkdStockReceiving_WorkingDate")]
    [Index(nameof(PeriodId), Name = "IX_InvCkdStockReceiving_PeriodId")]

    public class InvCkdStockReceiving : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxPartNoLength = 12;

        public const int MaxPartNoNormalizedLength = 12;

        public const int MaxPartNameLength = 300;

        public const int MaxPartNoNormalizedS4Length = 10;

        public const int MaxColorSfxLength = 2;

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

        public virtual long? ReferenceId { get; set; }

        public virtual DateTime? WorkingDate { get; set; }

        public virtual long? PeriodId { get; set; }
        public virtual DateTime? CreationTime { get; set; }
        public virtual long? CreatorUserId { get; set; }

        public virtual DateTime? LastModificationTime { get; set; }

        public virtual long? LastModifierUserId { get; set; }

        public virtual long? DeleterUserId { get; set; }

        public virtual DateTime? DeletionTime { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}

