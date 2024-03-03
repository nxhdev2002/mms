using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.LogW.Pik
{

	[Table("LgwPikPickingSignal")]
	[Index(nameof(PickingTabletId), Name = "IX_LgwPikPickingSignal_PickingTabletId")]
	[Index(nameof(TabletProcessId), Name = "IX_LgwPikPickingSignal_TabletProcessId")]
	[Index(nameof(PickingProgressId), Name = "IX_LgwPikPickingSignal_PickingProgressId")]
	[Index(nameof(IsActive), Name = "IX_LgwPikPickingSignal_IsActive")]
	public class LgwPikPickingSignal : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxPickingTabletIdLength = 50;

		public const int MaxIsCompletedLength = 1;

		public const int MaxIsActiveLength = 1;

		[StringLength(MaxPickingTabletIdLength)]
		public virtual string PickingTabletId { get; set; }

		public virtual int? TabletProcessId { get; set; }

		public virtual int? PickingProgressId { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? FirstSignalTime { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? LastSignalTime { get; set; }

		[StringLength(MaxIsCompletedLength)]
		public virtual string IsCompleted { get; set; }

		[StringLength(MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

}


