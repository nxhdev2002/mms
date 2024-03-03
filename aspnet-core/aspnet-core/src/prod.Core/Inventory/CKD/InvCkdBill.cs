using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.CKD
{

    [Table("InvCkdBill")]
    [Index(nameof(BillofladingNo), Name = "IX_InvCkdBill_BillofladingNo")]
    [Index(nameof(ShipmentId), Name = "IX_InvCkdBill_ShipmentId")]
    [Index(nameof(IsActive), Name = "IX_InvCkdBill_IsActive")]
    public class InvCkdBill : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxBillofladingNoLength = 20;

        public const int MaxStatusCodeLength = 50;

        public const int MaxIsActiveLength = 1;

        [StringLength(MaxBillofladingNoLength)]
        public virtual string BillofladingNo { get; set; }

        public virtual long? ShipmentId { get; set; }

        [Column(TypeName = "date")]
        public virtual DateTime? BillDate { get; set; }

        public virtual int? Status { get; set; }

        [StringLength(MaxStatusCodeLength)]
        public virtual string StatusCode { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}

