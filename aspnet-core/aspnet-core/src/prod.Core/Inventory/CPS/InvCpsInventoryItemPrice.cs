using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.CPS
{
    [Table("InvCpsInventoryItemPrice")]
    public class InvCpsInventoryItemPrice : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxPartNoLength = 40;

        public const int MaxColorLength = 40;

        public const int MaxPartNameLength = 500;

        public const int MaxPartNameSupplierLength = 240;

        public const int MaxSupplierNameLength = 255;

        public const int MaxCurrencyCodeLength = 15;

        public const int MaxPartNoCPSLenght = 50;

        public const int MaxProductGroupNameLenght = 240;

        public const int MaxUnitMeasLookupCodeLenght = 30;


        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(MaxColorLength)]
        public virtual string Color { get; set; }

        [StringLength(MaxPartNameLength)]
        public virtual string PartName { get; set; }

        [StringLength(MaxPartNameSupplierLength)]
        public virtual string PartNameSupplier { get; set; }

        [StringLength(MaxSupplierNameLength)]
        public virtual string SupplierName { get; set; }

        [StringLength(MaxCurrencyCodeLength)]
        public virtual string CurrencyCode { get; set; }

        public virtual long? UnitPrice { get; set; }
        public virtual long? TaxPrice { get; set; }
        public virtual DateTime? EffectiveFrom { get; set; }
        public virtual DateTime? EffectiveTo { get; set; }

        [StringLength(MaxPartNoCPSLenght)]
        public virtual string PartNoCPS { get; set; }
        
        [StringLength(MaxProductGroupNameLenght)]
        public virtual string ProductGroupName { get; set; }
        
        [StringLength(MaxUnitMeasLookupCodeLenght)]
        public virtual string UnitMeasLookupCode { get; set; }
        public virtual DateTime? ApproveDate { get; set; }

    }

}
