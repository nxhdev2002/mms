using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace prod.Master.Inventory
{
	[Table("MstGpsWbsCCMapping")]
	public class MstGpsWbsCCMapping : FullAuditedEntity<long>, IEntity<long>
	{
		public const int MaxCostCenterFromLength = 15;
		public const int MaxWbsFromLength = 25;
		public const int MaxCostCenterToLength = 15;
		public const int MaxWbsToLength = 25;
		public const int MaxIsActiveLength = 50;

		[StringLength(MaxCostCenterFromLength)]
		public virtual string CostCenterFrom { get; set; }

		[StringLength(MaxWbsFromLength)]
		public virtual string WbsFrom { get; set; }

		[StringLength(MaxCostCenterToLength)]
		public virtual string CostCenterTo { get; set; }

		[StringLength(MaxWbsToLength)]
		public virtual string WbsTo { get; set; }

		public virtual DateTime? EffectiveFromDate { get; set; }
		public virtual DateTime? EffectiveFromTo { get; set; }

		[StringLength(MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}
}
