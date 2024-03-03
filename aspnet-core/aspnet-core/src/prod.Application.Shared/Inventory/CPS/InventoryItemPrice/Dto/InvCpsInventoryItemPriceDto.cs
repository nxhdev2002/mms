using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.CPS.Dto
{

    public class InvCpsInventoryItemPriceDto : EntityDto<long?>
    {

        public virtual string PartNo { get; set; }

        public virtual string Color { get; set; }

        public virtual string PartName { get; set; }

        public virtual string PartNameSupplier { get; set; }

        public virtual string SupplierName { get; set; }

        public virtual string CurrencyCode { get; set; }

        public virtual long? UnitPrice { get; set; }
        public virtual long? TaxPrice { get; set; }
        public virtual DateTime? EffectiveFrom { get; set; }
        public virtual DateTime? EffectiveTo { get; set; }

        public virtual string PartNoCPS { get; set; }

        public virtual string ProductGroupName { get; set; }

        public virtual string UnitMeasLookupCode { get; set; }
        public virtual DateTime? ApproveDate { get; set; }

    }

    public class GetInvCpsInventoryItemPriceInput : PagedAndSortedResultRequestDto
    {

        public virtual string PartNo { get; set; }

        public virtual string PartName { get; set; }
        public virtual DateTime? EffectiveFrom { get; set; }
        public virtual DateTime? EffectiveTo { get; set; }

    }

    public class GetInvCpsInventoryItemPriceExportInput 
    {

        public virtual string PartNo { get; set; }

        public virtual string Color { get; set; }

        public virtual string PartName { get; set; }

        public virtual string PartNameSupplier { get; set; }

        public virtual string SupplierName { get; set; }

        public virtual string CurrencyCode { get; set; }

        public virtual long? UnitPrice { get; set; }
        public virtual long? TaxPrice { get; set; }
        public virtual DateTime? EffectiveFrom { get; set; }
        public virtual DateTime? EffectiveTo { get; set; }

        public virtual string PartNoCPS { get; set; }

        public virtual string ProductGroupName { get; set; }

        public virtual string UnitMeasLookupCode { get; set; }
        public virtual DateTime? ApproveDate { get; set; }

    }
}