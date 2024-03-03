using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.LogW
{

	[Table("MstLgwRobbingLane")]
	[Index(nameof(ContNo), Name = "IX_MstLgwRobbingLane_ContNo")]
	[Index(nameof(Renban), Name = "IX_MstLgwRobbingLane_Renban")]
	[Index(nameof(SupplierNo), Name = "IX_MstLgwRobbingLane_SupplierNo")]
	[Index(nameof(IsDisabled), Name = "IX_MstLgwRobbingLane_IsDisabled")]
	[Index(nameof(IsActive), Name = "IX_MstLgwRobbingLane_IsActive")]
	public class MstLgwRobbingLane : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxLaneNoLength = 50;

		public const int MaxLaneNameLength = 50;

		public const int MaxContNoLength = 50;

		public const int MaxRenbanLength = 50;

		public const int MaxSupplierNoLength = 50;

		public const int MaxShowOnlyLength = 1;

		public const int MaxIsDisabledLength = 1;

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

		[StringLength(MaxIsDisabledLength)]
		public virtual string IsDisabled { get; set; }

		[StringLength(MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

}

