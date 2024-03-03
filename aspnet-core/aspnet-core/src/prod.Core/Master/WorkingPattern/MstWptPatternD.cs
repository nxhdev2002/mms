using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.WorkingPattern
{

	[Table("MstWptPatternD")]
	[Index(nameof(PatternHId), Name = "IX_MstWptPatternD_PatternHId")]
	[Index(nameof(ShiftNo), Name = "IX_MstWptPatternD_ShiftNo")]
	public class MstWptPatternD : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxShiftNameLength = 10;

		public const int MaxDayOfWeekLength = 100;

		public const int MaxSeasonTypeLength = 10;

		public const int MaxIsActiveLength = 1;

		[Required]
		public virtual int? PatternHId { get; set; }

		[Required]
		public virtual int? ShiftNo { get; set; }

		[StringLength(MaxShiftNameLength)]
		public virtual string ShiftName { get; set; }

		public virtual TimeSpan? StartTime { get; set; }

		public virtual TimeSpan? EndTime { get; set; }

		[StringLength(MaxDayOfWeekLength)]
		public virtual string DayOfWeek { get; set; }

		[StringLength(MaxSeasonTypeLength)]
		public virtual string SeasonType { get; set; }

		[StringLength(MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

}

