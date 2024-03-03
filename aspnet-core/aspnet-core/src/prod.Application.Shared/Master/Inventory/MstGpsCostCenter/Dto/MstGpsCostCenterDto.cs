using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace prod.Master.Inventory.Dto
{
    public class MstGpsCostCenterDto : EntityDto<long?>
    {
        public virtual string Group { get; set; }
        public virtual string SubGroup { get; set; }
        public virtual string Division { get; set; }
        public virtual string Dept { get; set; }
        public virtual string Shop { get; set; }
        public virtual string Team { get; set; }
        public virtual string CostCenterCategory { get; set; }
        public virtual string GroupSubGroup { get; set; }
        public virtual string DivDept { get; set; }
        public virtual string Process { get; set; }
        public virtual string BudgetOwner { get; set; }
        public virtual string CostCenterGroup { get; set; }
        public virtual string CostCenterCurrent { get; set; }
        public virtual string CostCenter { get; set; }
		public virtual string IsDirectCostCenter { get; set; }
	}
    public class CreateOrEditMstGpsCostCenterDto : EntityDto<long?>
    {

        public const int MaxGroupLength = 50;

        public const int MaxDivisionLength = 50;

        public const int MaxShopLength = 50;

        public const int MaxTeamLength = 50;

        public const int MaxCostCenterCategoryLength = 10;

        public const int MaxGroupSubGroupLength = 10;

        public const int MaxDivDeptLength = 10;

        public const int MaxProcessLength = 50;

        public const int MaxBudgetOwnerLength = 50;

        public const int MaxCostCenterGroupLength = 50;

        public const int MaxCostCenterCurrentLength = 10;

        public const int MaxCostCenterLength = 10;

		public const int MaxIsDirectCostCenterLength = 1;

		[StringLength(MaxGroupLength)]
        public virtual string Group { get; set; }

        public virtual string SubGroup { get; set;}

        [StringLength(MaxDivisionLength)]
        public virtual string Division { get; set; }

        public virtual string Dept { get; set; }

        [StringLength(MaxShopLength)]
        public virtual string Shop { get; set; }

        [StringLength(MaxTeamLength)]
        public virtual string Team { get; set; }

        [StringLength(MaxCostCenterCategoryLength)]
        public virtual string CostCenterCategory { get; set; }

        [StringLength(MaxGroupSubGroupLength)]
        public virtual string GroupSubGroup { get; set; }

        [StringLength(MaxDivDeptLength)]
        public virtual string DivDept { get; set; }

        [StringLength(MaxProcessLength)]
        public virtual string Process { get; set; }

        [StringLength(MaxBudgetOwnerLength)]
        public virtual string BudgetOwner { get; set; }

        [StringLength(MaxCostCenterGroupLength)]
        public virtual string CostCenterGroup { get; set; }

        [StringLength(MaxCostCenterCurrentLength)]
        public virtual string CostCenterCurrent { get; set; }

        [StringLength(MaxCostCenterLength)]
        public virtual string CostCenter { get; set; }

		[StringLength(MaxIsDirectCostCenterLength)]
		public virtual string IsDirectCostCenter { get; set; }

	}
    public class GetMstGpsCostCenterInput : PagedAndSortedResultRequestDto
    {
        public virtual string Group { get; set; }
        public virtual string SubGroup { get; set; }
        public virtual string Division { get; set; }
        public virtual string Dept { get; set; }
        public virtual string Shop { get; set; }
    }
    public class GetMstGpsCostCenterExcelInput
    {
        public virtual string Group { get; set; }
        public virtual string SubGroup { get; set; }
        public virtual string Division { get; set; }
        public virtual string Dept { get; set; }
        public virtual string Shop { get; set; }
    }

    public class MstGpsCostCenterImportDto
    {
        public virtual string Group { get; set; }
        public virtual string SubGroup { get; set; }
        public virtual string Division { get; set; }
        public virtual string Dept { get; set; }
        public virtual string Shop { get; set; }
        public virtual string Team { get; set; }
        public virtual string CostCenterCategory { get; set; }
        public virtual string GroupSubGroup { get; set; }
        public virtual string DivDept { get; set; }
        public virtual string Process { get; set; }
        public virtual string BudgetOwner { get; set; }
        public virtual string CostCenterGroup { get; set; }
        public virtual string CostCenterCurrent { get; set; }
        public virtual string CostCenter { get; set; }
        public virtual string Guid { get; set; }
    }
}
