using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.LogA.Sps
{
    [Table("LgaSpsStock_T")]
    [Index(nameof(PartNo), Name = "IX_LgaSpsStock_T_PartNo")]
    [Index(nameof(SupplierNo), Name = "IX_LgaSpsStock_T_SupplierNo")]
    [Index(nameof(SpsRackAddress), Name = "IX_LgaSpsStock_T_SpsRackAddress")]
    [Index(nameof(IsActive), Name = "IX_LgaSpsStock_T_IsActive")]

    public class LgaSpsStock_T : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxGuidLength = 128;

        public const int MaxPartNoLength = 50;

        public const int MaxPartNameLength = 500;

        public const int MaxSupplierNoLength = 50;

        public const int MaxBackNoLength = 50;

        public const int MaxSpsRackAddressLength = 500;

        public const int MaxPcRackAddressLength = 500;

        public const int MaxPcPicKingMemberLength = 50;

        public const int MaxProcessLength = 50;

        public const int MaxIsActiveLength = 1;


        [StringLength(MaxGuidLength)]
        public virtual string Guid { get; set; }

        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(MaxPartNameLength)]
        public virtual string PartName { get; set; }

        [StringLength(MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        [StringLength(MaxBackNoLength)]
        public virtual string BackNo { get; set; }

        [StringLength(MaxSpsRackAddressLength)]
        public virtual string SpsRackAddress { get; set; }

        [StringLength(MaxPcRackAddressLength)]
        public virtual string PcRackAddress { get; set; }

        public virtual int? RackCapBox { get; set; }

        [StringLength(MaxPcPicKingMemberLength)]
        public virtual string PcPicKingMember { get; set; }

        public virtual int? EkbQty { get; set; }

        public virtual int? StockQty { get; set; }

        public virtual int? BoxQty { get; set; }


        [StringLength(MaxProcessLength)]
        public virtual string Process { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}