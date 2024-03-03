using Abp.Application.Services.Dto;
using prod.Master.Inventory.GpsMaterialCategory;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace prod.Inventory.Gps.PartListByCategory.Dto
{
    public class InvGpsPartListByCategoryDto : EntityDto<long?>
    {
        public virtual string Item { get; set; }
        public virtual string Description { get; set; }
        public virtual string Uom { get; set; }
        public virtual string Category { get; set; }
        public virtual string Location { get; set; }
        public virtual string ExpenseAccount { get; set; }
        public virtual string Group { get; set; }
        public virtual string CurrentCategory { get; set; }
        public virtual string PartType { get; set; }
    }

    public class GetInvGpsPartListByCategoryInput : PagedAndSortedResultRequestDto
    {
        public virtual string Item { get; set; }
        public virtual string Category { get; set; }
        public virtual string Location { get; set; }
        public virtual string PartType { get; set; }
    }
    public class CreateOrEditInvGpsPartListByCategoryDto : EntityDto<long?>
    {
        public const int MaxItemLength = 50;

        public const int MaxDescriptionLength = 100;

        public const int MaxUomLength = 10;

        public const int MaxCatetoryLength = 50;

        public const int MaxLocationLength = 50;

        public const int MaxExpenseAccountLength = 10;

        public const int MaxGroupLength = 1;

        public const int MaxCurrentCategoryLength = 50;

        public const int MaxPartTypeLength = 10;

        [StringLength(MaxItemLength)]
        public virtual string Item { get; set; }

        [StringLength(MaxDescriptionLength)]
        public virtual string Description { get; set; }

        [StringLength(MaxUomLength)]
        public virtual string Uom { get; set; }

        [StringLength(MaxCatetoryLength)]
        public virtual string Category { get; set; }

        [StringLength(MaxLocationLength)]
        public virtual string Location { get; set; }

        [StringLength(MaxExpenseAccountLength)]
        public virtual string ExpenseAccount { get; set; }

        [StringLength(MaxGroupLength)]
        public virtual string Group { get; set; }

        [StringLength(MaxCurrentCategoryLength)]
        public virtual string CurrentCategory { get; set; }

        [StringLength(MaxPartTypeLength)]
        public virtual string PartType { get; set; }
    }

    public class InvGpsPartListByCategoryImportDto
    {
        public virtual string Item { get; set; }
        public virtual string Description { get; set; }
        public virtual string Uom { get; set; }
        public virtual string Category { get; set; }
        public virtual string Location { get; set; }
        public virtual string ExpenseAccount { get; set; }
        public virtual string Group { get; set; }
        public virtual string CurrentCategory { get; set; }
        public virtual string PartType { get; set; }
        public virtual string Guid { get; set; }
    }
}
