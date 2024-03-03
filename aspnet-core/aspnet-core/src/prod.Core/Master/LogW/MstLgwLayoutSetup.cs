using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.LogW
{

	[Table("MstLgwLayoutSetup")]
	[Index(nameof(Zone), Name = "IX_MstLgwLayoutSetup_Zone")]
	[Index(nameof(SubScreenNo), Name = "IX_MstLgwLayoutSetup_SubScreenNo")]
	[Index(nameof(ScreenArea), Name = "IX_MstLgwLayoutSetup_ScreenArea")]
	[Index(nameof(IsDisabled), Name = "IX_MstLgwLayoutSetup_IsDisabled")]
	[Index(nameof(IsActive), Name = "IX_MstLgwLayoutSetup_IsActive")]
	public class MstLgwLayoutSetup : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxZoneLength = 50;

		public const int MaxScreenAreaLength = 50;

		public const int MaxCellNameLength = 50;

		public const int MaxCellTypeLength = 50;

		public const int MaxColumnNameLength = 50;

		public const int MaxIsDisabledLength = 1;

		public const int MaxIsActiveLength = 1;

		[StringLength(MaxZoneLength)]
		public virtual string Zone { get; set; }

		public virtual int? SubScreenNo { get; set; }

		[StringLength(MaxScreenAreaLength)]
		public virtual string ScreenArea { get; set; }

		[StringLength(MaxCellNameLength)]
		public virtual string CellName { get; set; }

		[StringLength(MaxCellTypeLength)]
		public virtual string CellType { get; set; }

		public virtual int? NumRow { get; set; }

		[StringLength(MaxColumnNameLength)]
		public virtual string ColumnName { get; set; }

		[StringLength(MaxIsDisabledLength)]
		public virtual string IsDisabled { get; set; }

		[StringLength(MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

}

