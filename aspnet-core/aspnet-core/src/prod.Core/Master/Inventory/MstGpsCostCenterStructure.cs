using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Master.Inventory
{
    [Table("MstGpsCostCenterStructure")]
    public class MstGpsCostCenterStructure : FullAuditedEntity<long>, IEntity<long>
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

            public virtual string SubGroup { get; set; }

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
}
