using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.WorkingPattern.Dto
{

	public class MstWptPatternDDto : EntityDto<long?>
	{

		public virtual int? PatternHId { get; set; }

		public virtual int? ShiftNo { get; set; }

		public virtual string ShiftName { get; set; }

		public virtual TimeSpan? StartTime { get; set; }

		public virtual TimeSpan? EndTime { get; set; }

		public virtual string DayOfWeek { get; set; }

		public virtual string SeasonType { get; set; }

		public virtual string IsActive { get; set; }

	}

	public class CreateOrEditMstWptPatternDDto : EntityDto<long?>
	{

		[Required]
		public virtual int? PatternHId { get; set; }

		[Required]
		public virtual int? ShiftNo { get; set; }

		[StringLength(MstWptPatternDConsts.MaxShiftNameLength)]
		public virtual string ShiftName { get; set; }

		public virtual TimeSpan? StartTime { get; set; }

		public virtual TimeSpan? EndTime { get; set; }

		[StringLength(MstWptPatternDConsts.MaxDayOfWeekLength)]
		public virtual string DayOfWeek { get; set; }

		[StringLength(MstWptPatternDConsts.MaxSeasonTypeLength)]
		public virtual string SeasonType { get; set; }

		[StringLength(MstWptPatternDConsts.MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

	public class GetMstWptPatternDInput : PagedAndSortedResultRequestDto
	{

		public virtual int? PatternHId { get; set; }

		public virtual int? ShiftNo { get; set; }

		public virtual string ShiftName { get; set; }
		public virtual TimeSpan? StartTime { get; set; }

		public virtual TimeSpan? EndTime { get; set; }

		public virtual string DayOfWeek { get; set; }

		public virtual string SeasonType { get; set; }

		public virtual string IsActive { get; set; }

	}

}


