using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace prod.Master.Inventory.Dto
{
    public class MstGpsMaterialRegisterByShopDto : EntityDto<long?>
    {
        public virtual string PartNo { get; set; }
        public virtual string Description { get; set; }
        public virtual string Uom { get; set; }
        public virtual string Category { get; set; }
        public virtual string ExpenseAccount { get; set; }
        public virtual string Shop { get; set; }
        public virtual string CostCenter { get; set; }
    }

    public class CreateOrEditMstGpsMaterialRegisterByShopDto : EntityDto<long?>
    {

        public const int MaxPartNoLength = 50;

        public const int MaxDescriptionLength = 500;

        public const int MaxUomLength = 10;

        public const int MaxCategoryLength = 50;

        public const int MaxExpenseAccountLength = 50;

        public const int MaxShopLength = 100;

        public const int MaxCostCenterLength = 20;

        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(MaxDescriptionLength)]
        public virtual string Description { get; set; }

        [StringLength(MaxUomLength)]
        public virtual string Uom { get; set; }

        [StringLength(MaxCategoryLength)]
        public virtual string Category { get; set; }

        [StringLength(MaxExpenseAccountLength)]
        public virtual string ExpenseAccount { get; set; }

        [StringLength(MaxShopLength)]
        public virtual string Shop { get; set; }

        [StringLength(MaxCostCenterLength)]
        public virtual string CostCenter { get; set; }

    }

    public class GetMstGpsMaterialRegisterByShopInput : PagedAndSortedResultRequestDto
    {
        public virtual string PartNo { get; set; }
        public virtual string Uom { get; set; }
        public virtual string Category { get; set; }
        public virtual string ExpenseAccount { get; set; }
        public virtual string Shop { get; set; }
        public virtual string CostCenter { get; set; }
    }

    public class GetMstGpsMaterialRegisterByShopExcelInput
    {
        public virtual string PartNo { get; set; }
        public virtual string Uom { get; set; }
        public virtual string Category { get; set; }
        public virtual string ExpenseAccount { get; set; }
        public virtual string Shop { get; set; }
        public virtual string CostCenter { get; set; }
    }

    public class MstGpsMaterialRegisterByShopImportDto : EntityDto<long?>
    {
        public virtual string PartNo { get; set; }
        public virtual string Description { get; set; }
        public virtual string Uom { get; set; }
        public virtual string Category { get; set; }
        public virtual string ExpenseAccount { get; set; }
        public virtual string Shop { get; set; }
        public virtual string CostCenter { get; set; }
        public virtual string Guid { get; set; }
    }

    public class CbxCategory : EntityDto<long?>
    {
        public virtual long? Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string IsActive { get; set; }
    }
}
