using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace prod.Inventory.GPS
{
    [Table("InvGpsUser")]
    public class InvGpsUser : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxNameLength = 200;

        public const int MaxShopLenght = 50;

        public const int MaxTeamLength = 50;

        public const int MaxCostCenterLength = 50;

        public const int MaxGroupLength = 200;

        public const int MaxSubGroupLength = 200;

        public const int MaxDivisionLength = 50;

        public const int MaxDeptLength = 200;


        public virtual long? EmployeeId { get; set; }

        [StringLength(MaxNameLength)]
        public virtual string Name { get; set; }

        [StringLength(MaxShopLenght)]
        public virtual string Shop { get; set; }

        [StringLength(MaxTeamLength)]
        public virtual string Team { get; set; }

        [StringLength(MaxCostCenterLength)]
        public virtual string CostCenter { get; set; }

        [StringLength(MaxGroupLength)]
        public virtual string Group { get; set; }

        [StringLength(MaxSubGroupLength)]
        public virtual string SubGroup { get; set; }

        [StringLength(MaxDivisionLength)]
        public virtual string Division { get; set; }

        [StringLength(MaxDeptLength)]
        public virtual string Dept { get; set; }

    }
}

