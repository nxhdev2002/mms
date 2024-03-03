using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.CKD
{

    [Table("InvCkdPhysicalStockPart")]
    [Index(nameof(PartNo), Name = "IX_InvCkdPhysicalStockPart_PartNo")]
    [Index(nameof(LotNo), Name = "IX_InvCkdPhysicalStockPart_LotNo")]
    [Index(nameof(PartListId), Name = "IX_InvCkdPhysicalStockPart_PartListId")]
    [Index(nameof(MaterialId), Name = "IX_InvCkdPhysicalStockPart_MaterialId")]
    [Index(nameof(PeriodId), Name = "IX_InvCkdPhysicalStockPart_PeriodId")]
    public class InvCkdPhysicalStockPart : FullAuditedEntity<long>, IEntity<long>
    {

  

        [StringLength(12)]
        public virtual string PartNo { get; set; }

        [StringLength(12)]
        public virtual string PartNoNormalized { get; set; }

        [StringLength(300)]
        public virtual string PartName { get; set; }

        [StringLength(10)]
        public virtual string PartNoNormalizedS4 { get; set; }

        [StringLength(2)]
        public virtual string ColorSfx { get; set; }

        [StringLength(20)]
        public virtual string LotNo { get; set; }

        public virtual long? PartListId { get; set; }

        public virtual long? MaterialId { get; set; }

        public virtual int? BeginQty { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? WorkingDate { get; set; }

        public virtual long? PeriodId { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? LastCalDatetime { get; set; }

        public virtual int? Transtype { get; set; }


        public virtual int? ReceiveQty { get; set; }

        public virtual int? IssueQty { get; set; }

        public virtual int? ActualQty { get; set; }

        public virtual decimal? CalculatorQty { get; set; }


        [StringLength(50)]
        public virtual string Remark { get; set; }

        [StringLength(1)]
        public virtual string IsActive { get; set; }
    }

}

