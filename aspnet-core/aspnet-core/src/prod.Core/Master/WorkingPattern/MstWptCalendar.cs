using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.WorkingPattern
{

	[Table("MstWptCalendar")]
	[Index(nameof(WorkingDate), Name="IX_MstWptCalendar_WorkingDate")]
	[Index(nameof(WorkingType), Name="IX_MstWptCalendar_WorkingType")]
	[Index(nameof(IsActive), Name="IX_MstWptCalendar_IsActive")]
	public class MstWptCalendar: FullAuditedEntity<long>, IEntity<long>	
	{
		public const int MaxWorkingTypeLength = 10;

		public const int MaxWorkingStatusLength = 10;

		public const int MaxSeasonTypeLength = 10;

		public const int MaxDayOfWeekLength = 100;

		public const int MaxIsActiveLength = 1;

		[Column(TypeName="date")]
		[Required]
		public virtual DateTime? WorkingDate { get; set; }

		[Required]
		[StringLength(MaxWorkingTypeLength)]
		public virtual string WorkingType { get; set; }

		[StringLength(MaxWorkingStatusLength)]
		public virtual string WorkingStatus { get; set; }

		[StringLength(MaxSeasonTypeLength)]
		public virtual string SeasonType { get; set; }

		[StringLength(MaxDayOfWeekLength)]
		public virtual string DayOfWeek { get; set; }

		public virtual int? WeekNumber { get; set; }

		public virtual int? WeekWorkingDays { get; set; }

		[StringLength(MaxIsActiveLength)]
		public virtual string IsActive { get; set; }

	}

}

