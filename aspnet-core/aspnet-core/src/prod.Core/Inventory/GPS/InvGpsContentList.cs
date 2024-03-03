using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.GPS
{

    [Table("InvGpsContentList")]
    [Index(nameof(WorkingDate), Name = "IX_InvGpsContentList_WorkingDate")]
    [Index(nameof(Shift), Name = "IX_InvGpsContentList_Shift")]
    [Index(nameof(SupplierCode), Name = "IX_InvGpsContentList_SupplierCode")]
    [Index(nameof(Status), Name = "IX_InvGpsContentList_Status")]
    [Index(nameof(IsActive), Name = "IX_InvGpsContentList_IsActive")]
    public class InvGpsContentList : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxShiftLength = 40;

        public const int MaxSupplierNameLength = 40;

        public const int MaxSupplierCodeLength = 10;

        public const int MaxPcAddressLength = 40;

        public const int MaxDockNoLength = 20;

        public const int MaxOrderNoLength = 50;

        public const int MaxContentNoLength = 60;

        public const int MaxIsPalletOnlyLength = 1;

        public const int MaxPackagingTypeLength = 20;

        public const int MaxIsAdhocReceivingLength = 1;

        public const int MaxGeneratedByLength = 50;

        public const int MaxUnpackStatusLength = 1;

        public const int MaxModuleCdLength = 10;

        public const int MaxStatusLength = 50;

        public const int MaxIsActiveLength = 1;

        [Column(TypeName = "date")]
        public virtual DateTime? WorkingDate { get; set; }

        [StringLength(MaxShiftLength)]
        public virtual string Shift { get; set; }

        [StringLength(MaxSupplierNameLength)]
        public virtual string SupplierName { get; set; }

        [StringLength(MaxSupplierCodeLength)]
        public virtual string SupplierCode { get; set; }

        public virtual int? RenbanNo { get; set; }

        [StringLength(MaxPcAddressLength)]
        public virtual string PcAddress { get; set; }

        [StringLength(MaxDockNoLength)]
        public virtual string DockNo { get; set; }

        [StringLength(MaxOrderNoLength)]
        public virtual string OrderNo { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? OrderDatetime { get; set; }

        public virtual int? TripNo { get; set; }

        public virtual int? PalletBoxQty { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? EstPackingDatetime { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? EstArrivalDatetime { get; set; }

        [StringLength(MaxContentNoLength)]
        public virtual string ContentNo { get; set; }

        public virtual long? OrderId { get; set; }

        public virtual int? PalletSize { get; set; }

        [StringLength(MaxIsPalletOnlyLength)]
        public virtual string IsPalletOnly { get; set; }

        [StringLength(MaxPackagingTypeLength)]
        public virtual string PackagingType { get; set; }

        [StringLength(MaxIsAdhocReceivingLength)]
        public virtual string IsAdhocReceiving { get; set; }

        [StringLength(MaxGeneratedByLength)]
        public virtual string GeneratedBy { get; set; }

        [StringLength(MaxUnpackStatusLength)]
        public virtual string UnpackStatus { get; set; }

        [StringLength(MaxModuleCdLength)]
        public virtual string ModuleCd { get; set; }

        public virtual int? ModuleRunNo { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? UpStartAct { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? UpFinishAct { get; set; }

        public virtual int? UpScanUserId { get; set; }

        [StringLength(MaxStatusLength)]
        public virtual string Status { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}

