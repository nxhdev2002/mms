using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Inv
{

    [Table("MstInvGpsSupplierOrderTime")]
    [Index(nameof(SupplierId), Name = "IX_MstInvGpsSupplierOrderTime_SupplierId")]
    [Index(nameof(OrderType), Name = "IX_MstInvGpsSupplierOrderTime_OrderType")]
    [Index(nameof(IsActive), Name = "IX_MstInvGpsSupplierOrderTime_IsActive")]
    public class MstInvGpsSupplierOrderTime : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxOrderTypeLength = 1;

        public const int MaxIsActiveLength = 1;

        public virtual long? SupplierId { get; set; }

        public virtual int? OrderSeq { get; set; }

        [StringLength(MaxOrderTypeLength)]
        public virtual string OrderType { get; set; }

        [Column(TypeName = "time(7)")]
        public virtual TimeSpan? OrderTime { get; set; }

        public virtual int? ReceivingDay { get; set; }

        [Column(TypeName = "time(7)")]
        public virtual TimeSpan? ReceiveTime { get; set; }

        [Column(TypeName = "time(7)")]
        public virtual TimeSpan? KeihenTime { get; set; }

        public virtual int? KeihenDay { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }
}

