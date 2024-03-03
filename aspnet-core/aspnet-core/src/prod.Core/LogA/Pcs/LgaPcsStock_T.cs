using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.LogA.Pcs
{
    [Table("LgaPcsStock_T")]
    [Index(nameof(PartNo), Name = "IX_LgaPcsStock_T_PartNo")]
    [Index(nameof(SupplierNo), Name = "IX_LgaPcsStock_T_SupplierNo")]
    [Index(nameof(PcRackAddress), Name = "IX_LgaPcsStock_T_PcRackAddress")]
    [Index(nameof(IsActive), Name = "IX_LgaPcsStock_T_IsActive")]

    public class LgaPcsStock_T : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxGuidLength = 128;

        public const int MaxPartNoLength = 50;

        public const int MaxPartNameLength = 500;

        public const int MaxSupplierNoLength = 50;

        public const int MaxBackNoLength = 50;

        public const int MaxPcRackAddressLength = 200;

        public const int MaxOutTypeLength = 50;

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

        [StringLength(MaxPcRackAddressLength)]
        public virtual string PcRackAddress { get; set; }

        public virtual int? UsagePerHour { get; set; }

        public virtual int? RackCapBox { get; set; }

        [StringLength(MaxOutTypeLength)]
        public virtual string OutType { get; set; }

        public virtual int? StockQty { get; set; }

        public virtual int? BoxQty { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}