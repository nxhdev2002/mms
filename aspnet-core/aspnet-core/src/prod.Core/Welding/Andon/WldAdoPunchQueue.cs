using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Welding.Andon
{

	[Table("WldAdoPunchQueue")]
	[Index(nameof(BodyNo), Name = "IX_WldAdoPunchQueue_BodyNo")]
	[Index(nameof(LotNo), Name = "IX_WldAdoPunchQueue_LotNo")]
	public class WldAdoPunchQueue : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxBodyNoLength = 50;

		public const int MaxModelLength = 50;

		public const int MaxLineLength = 100;

		public const int MaxLotNoLength = 50;

		public const int MaxColorLength = 50;

		public const int MaxPunchFlagLength = 1;

		public const int MaxPunchIndicatorLength = 500;

		public const int MaxIsCallLength = 1;

		public const int MaxIsCfLength = 1;

		[StringLength(MaxBodyNoLength)]
		public virtual string BodyNo { get; set; }

		[StringLength(MaxModelLength)]
		public virtual string Model { get; set; }

		[StringLength(MaxLineLength)]
		public virtual string Line { get; set; }

		[StringLength(MaxLotNoLength)]
		public virtual string LotNo { get; set; }

		[StringLength(MaxColorLength)]
		public virtual string Color { get; set; }

		public virtual int? ProcessGroup { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? ScanTime { get; set; }

		[StringLength(MaxPunchFlagLength)]
		public virtual string PunchFlag { get; set; }

		[StringLength(MaxPunchIndicatorLength)]
		public virtual string PunchIndicator { get; set; }

		[StringLength(MaxIsCallLength)]
		public virtual string IsCall { get; set; }

		[StringLength(MaxIsCfLength)]
		public virtual string IsCf { get; set; }
	}

}
