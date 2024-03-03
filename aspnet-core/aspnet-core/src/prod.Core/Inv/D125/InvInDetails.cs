using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inv.D125
{

    [Table("InvInDetails")]
    [Index(nameof(InvoiceNo), Name = "IX_InvInDetails_InvoiceNo")]
    [Index(nameof(PartNo), Name = "IX_InvInDetails_PartNo")]
    [Index(nameof(FixLot), Name = "IX_InvInDetails_FixLot")]
    [Index(nameof(CarfamilyCode), Name = "IX_InvInDetails_CarfamilyCode")]
    public class InvInDetails : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxPartNoLength = 50;

        public const int MaxSupplierNoLength = 50;

        public const int MaxFixLotLength = 50;

        public const int MaxCarfamilyCodeLength = 50;

        public const int MaxInvoiceNoLength = 50;

        public const int MaxCustomsDeclareNoLength = 50;



        public virtual decimal PeriodId { get; set; }

        [StringLength(MaxInvoiceNoLength)]
        public virtual string InvoiceNo { get; set; }

        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        public virtual decimal? UsageQty { get; set; }

        public virtual DateTime? InvoiceDate { get; set; }

        public virtual DateTime? ReceiveDate { get; set; }

        [StringLength(MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        [StringLength(MaxFixLotLength)]
        public virtual string FixLot { get; set; }

        [StringLength(MaxCarfamilyCodeLength)]
        public virtual string CarfamilyCode { get; set; }

        [StringLength(MaxCustomsDeclareNoLength)]
        public virtual string CustomsDeclareNo { get; set; }

        public virtual DateTime? DeclareDate { get; set; }
    }

}


