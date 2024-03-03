using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.CKD
{
    public class InvCkdPartRobbing_T : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxGuidLength = 128;

        public const int MaxPartNoLength = 50;

        public const int MaxPartNoNormalizedLength = 50;

        public const int MaxPartNameLength = 500;

        public const int MaxOrderPatternLength = 10;

        public const int MaxCfcLength = 4;

        public const int MaxShopLength = 10;

        public const int MaxCaseLength = 50;

        public const int MaxBoxLength = 50;

        public const int MaxSupplierNoLength = 50;

        public const int MaxIsActiveLength = 1;

        public const int MaxDetailModelLength = 5000;

        public const int MaxTpmEciLength = 50;

        public const int MaxImplementTimmingLength = 50;

        public const int MaxUniqueLotKitLength = 50;

        public const int MaxBorrowServPartLength = 50;



        [StringLength(MaxGuidLength)]
        public virtual string Guid { get; set; }
        [StringLength(MaxCfcLength)]
        public virtual string Cfc { get; set; }

        [StringLength(MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(MaxPartNameLength)]
        public virtual string PartName { get; set; }

        public virtual int? RobbingQty { get; set; }

        public virtual int? UnitQty { get; set; }

        public virtual int? EffectVehQty { get; set; }

        [StringLength(MaxCaseLength)]
        public virtual string Case { get; set; }

        [StringLength(MaxBoxLength)]
        public virtual string Box { get; set; }

        [StringLength(MaxDetailModelLength)]
        public virtual string DetailModel { get; set; }

        [StringLength(MaxShopLength)]
        public virtual string Shop { get; set; }

        [StringLength(MaxTpmEciLength)]
        public virtual string TpmEci { get; set; }

        [StringLength(MaxImplementTimmingLength)]
        public virtual string ImplementTimming { get; set; }

        [StringLength(MaxUniqueLotKitLength)]
        public virtual string UniqueLotKit { get; set; }

        [StringLength(MaxBorrowServPartLength)]
        public virtual string BorrowServPart { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }
}
