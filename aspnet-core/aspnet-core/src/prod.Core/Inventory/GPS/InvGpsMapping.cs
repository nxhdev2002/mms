using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.GPS
{
    [Table("InvGpsMapping")]
    public class InvGpsMapping : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxPartNoLength = 50;

        public const int MaxPartTypeLength = 50;

        public const int MaxPartCatetoryLength = 50;

        public const int MaxShopRegisterLength = 50;

        public const int MaxLocationLength = 50;

        public const int MaxCostCenterLength = 10;

        public const int MaxWbsLength = 24;

        public const int MaxGlAccountLength = 10;

        public const int MaxExpenseAccountLength = 50;

        public const int MaxLastRenewLength = 50;

        public const int MaxRenewByLength = 50;

        public const int MaxStatusLength = 50;

        public const int MaxIsAciveLength = 50;

        public virtual long? PartId { get; set; }

        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(MaxPartTypeLength)]
        public virtual string PartType { get; set; }

        [StringLength(MaxPartCatetoryLength)]
        public virtual string PartCatetory { get; set; }

        [StringLength(MaxShopRegisterLength)]
        public virtual string ShopRegister { get; set; }

        [StringLength(MaxLocationLength)]
        public virtual string Location { get; set; }

        [StringLength(MaxCostCenterLength)]
        public virtual string CostCenter { get; set; }

        [StringLength(MaxWbsLength)]
        public virtual string Wbs { get; set; }

        [StringLength(MaxGlAccountLength)]
        public virtual string GlAccount { get; set; }

        [StringLength(MaxExpenseAccountLength)]
        public virtual string ExpenseAccount { get; set; }

        public virtual DateTime? EffectiveDateTo { get; set; }
        public virtual DateTime? EffectiveDateFrom { get; set; }

        [StringLength(MaxLastRenewLength)]
        public virtual string LastRenew { get; set; }

        [StringLength(MaxRenewByLength)]
        public virtual string RenewBy { get; set; }

        [StringLength(MaxStatusLength)]
        public virtual string Status { get; set; }

        [StringLength(MaxIsAciveLength)]
        public virtual string IsAcive { get; set; }

    }
}
