using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.LogW.Mwh
{

	[Table("LgwMwhRobbingLane")]
	[Index(nameof(LaneNo), Name = "IX_LgwMwhRobbingLane_LaneNo")]
	[Index(nameof(ContNo), Name = "IX_LgwMwhRobbingLane_ContNo")]
	[Index(nameof(Renban), Name = "IX_LgwMwhRobbingLane_Renban")]
	[Index(nameof(SupplierNo), Name = "IX_LgwMwhRobbingLane_SupplierNo")]
	public class LgwMwhRobbingLane : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxLaneNoLength = 10;

		public const int MaxLaneNameLength = 50;

		public const int MaxContNoLength = 50;

		public const int MaxRenbanLength = 10;

		public const int MaxSupplierNoLength = 10;

		public const int MaxShowOnlyLength = 1;

		public const int MaxIsActiveLength = 1;

		[StringLength(MaxLaneNoLength)]
		public virtual string LaneNo { get; set; }

		[StringLength(MaxLaneNameLength)]
		public virtual string LaneName { get; set; }

		[StringLength(MaxContNoLength)]
		public virtual string ContNo { get; set; }

		[StringLength(MaxRenbanLength)]
		public virtual string Renban { get; set; }

		[StringLength(MaxSupplierNoLength)]
		public virtual string SupplierNo { get; set; }

		[StringLength(MaxShowOnlyLength)]
		public virtual string ShowOnly { get; set; }

		[StringLength(MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

}

