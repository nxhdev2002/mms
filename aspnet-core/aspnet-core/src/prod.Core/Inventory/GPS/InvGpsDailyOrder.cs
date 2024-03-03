using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.GPS
{

    [Table("InvGpsDailyOrder")]
    [Index(nameof(WorkingDate), Name = "IX_InvGpsDailyOrder_WorkingDate")]
    [Index(nameof(SupplierCode), Name = "IX_InvGpsDailyOrder_SupplierCode")]
    [Index(nameof(IsActive), Name = "IX_InvGpsDailyOrder_IsActive")]
    public class InvGpsDailyOrder : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxShiftLength = 40;

        public const int MaxSupplierNameLength = 40;

        public const int MaxSupplierCodeLength = 10;

        public const int MaxOrderNoLength = 50;

        public const int MaxTruckNoLength = 40;

        public const int MaxStatusLength = 50;

        public const int MaxGeneratedByLength = 10;

        public const int MaxIsActiveLength = 1;

        [Column(TypeName = "date")]
        public virtual DateTime? WorkingDate { get; set; }

        [StringLength(MaxShiftLength)]
        public virtual string Shift { get; set; }

        [StringLength(MaxSupplierNameLength)]
        public virtual string SupplierName { get; set; }

        [StringLength(MaxSupplierCodeLength)]
        public virtual string SupplierCode { get; set; }

        [StringLength(MaxOrderNoLength)]
        public virtual string OrderNo { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? OrderDatetime { get; set; }

        public virtual int? TripNo { get; set; }

        [StringLength(MaxTruckNoLength)]
        public virtual string TruckNo { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? EstArrivalDatetime { get; set; }

        [StringLength(MaxStatusLength)]
        public virtual string Status { get; set; }

        [StringLength(MaxGeneratedByLength)]
        public virtual string GeneratedBy { get; set; }

        public virtual long? TruckUnloadingId { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}


