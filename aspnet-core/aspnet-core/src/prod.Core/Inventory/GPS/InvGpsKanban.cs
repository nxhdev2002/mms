using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.GPS
{

    [Table("InvGpsKanban")]
    [Index(nameof(PartNo), Name = "IX_InvGpsKanban_PartNo")]
    [Index(nameof(Status), Name = "IX_InvGpsKanban_Status")]
    [Index(nameof(IsActive), Name = "IX_InvGpsKanban_IsActive")]
    public class InvGpsKanban : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxBackNoLength = 20;

        public const int MaxPartNoLength = 20;

        public const int MaxColorSfxLength = 2;

        public const int MaxPartNameLength = 400;

        public const int MaxPcAddressLength = 40;

        public const int MaxWhSpsPickingLength = 40;

        public const int MaxPackagingTypeLength = 20;

        public const int MaxGeneratedByLength = 50;

        public const int MaxStatusLength = 50;

        public const int MaxIsActiveLength = 1;

        public virtual long? ContentListId { get; set; }

        [StringLength(MaxBackNoLength)]
        public virtual string BackNo { get; set; }

        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(MaxColorSfxLength)]
        public virtual string ColorSfx { get; set; }

        [StringLength(MaxPartNameLength)]
        public virtual string PartName { get; set; }

        public virtual int? BoxSize { get; set; }

        public virtual int? BoxQty { get; set; }

        [StringLength(MaxPcAddressLength)]
        public virtual string PcAddress { get; set; }

        [StringLength(MaxWhSpsPickingLength)]
        public virtual string WhSpsPicking { get; set; }

        public virtual int? ActualBoxQty { get; set; }

        public virtual int? RenbanNo { get; set; }

        public virtual int? NoInRenban { get; set; }

        [StringLength(MaxPackagingTypeLength)]
        public virtual string PackagingType { get; set; }

        public virtual int? ActualBoxSize { get; set; }

        [StringLength(MaxGeneratedByLength)]
        public virtual string GeneratedBy { get; set; }

        [StringLength(MaxStatusLength)]
        public virtual string Status { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}

