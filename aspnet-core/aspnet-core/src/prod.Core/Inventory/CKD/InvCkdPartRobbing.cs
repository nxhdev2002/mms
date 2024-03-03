using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.CKD
{
    [Table("InvCkdPartRobbing")]
    public class InvCkdPartRobbing : FullAuditedEntity<long>, IEntity<long>
    {

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

        public const int MaxTpmEciLength = 50;

        public const int MaxImplementTimmingLength = 50;

        public const int MaxUniqueLotKitLength = 50;

        public const int MaxBorrowServPartLength = 50;



        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(MaxPartNoNormalizedLength)]
        public virtual string PartNoNormalized { get; set; }

        [StringLength(MaxPartNameLength)]
        public virtual string PartName { get; set; }

        [StringLength(MaxCfcLength)]
        public virtual string Cfc { get; set; }

        [StringLength(MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        [StringLength(MaxCaseLength)]
        public virtual string Case { get; set; }

        [StringLength(MaxBoxLength)]
        public virtual string Box { get; set; }

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