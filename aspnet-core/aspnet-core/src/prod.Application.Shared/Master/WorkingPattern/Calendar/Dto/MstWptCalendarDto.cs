using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.WorkingPattern.Dto
{

	public class MstWptCalendarDto : EntityDto<long?>
	{
		public virtual DateTime? WorkingDate { get; set; }
        public virtual DateTime? RequestDateFrom { get; set; }
        public virtual DateTime? RequestDateTo { get; set; }

        public virtual string WorkingType { get; set; }

		public virtual string WorkingStatus { get; set; }

		public virtual string SeasonType { get; set; }

		public virtual string DayOfWeek { get; set; }

		public virtual int? WeekNumber { get; set; }

		public virtual int? WeekWorkingDays { get; set; }

		public virtual string IsActive { get; set; }

	}

	public class CreateOrEditMstWptCalendarDto : EntityDto<long?>
	{
		[Required]
		public virtual DateTime? WorkingDate { get; set; }

		[Required]
		[StringLength(MstWptCalendarConsts.MaxWorkingTypeLength)]
		public virtual string WorkingType { get; set; }

		[StringLength(MstWptCalendarConsts.MaxWorkingStatusLength)]
		public virtual string WorkingStatus { get; set; }

		[StringLength(MstWptCalendarConsts.MaxSeasonTypeLength)]
		public virtual string SeasonType { get; set; }

		[StringLength(MstWptCalendarConsts.MaxDayOfWeekLength)]
		public virtual string DayOfWeek { get; set; }

		public virtual int? WeekNumber { get; set; }

		public virtual int? WeekWorkingDays { get; set; }

		[StringLength(MstWptCalendarConsts.MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

	public class GetMstWptCalendarInput : PagedAndSortedResultRequestDto
	{

		public virtual DateTime? WorkingDate { get; set; }

        public virtual DateTime? WorkingDateFrom { get; set; }

        public virtual DateTime? WorkingDateTo { get; set; }

        public virtual string WorkingType { get; set; }

		public virtual string WorkingStatus { get; set; }

		public virtual string SeasonType { get; set; }

		public virtual string DayOfWeek { get; set; }

		public virtual string IsActive { get; set; }

	}

	public class GetMstWptCalendarDetailInput : PagedAndSortedResultRequestDto
	{
		public virtual DateTime? monthSearch { get; set; }

		public virtual DateTime? yearSearch { get; set; }

	}

	public class MstWptCalendarDetailDto 
	{
		public virtual DateTime? WorkingMonth { get; set; }

		public virtual string WorkingType { get; set; }

		public virtual string Day_No { get; set; }

		public virtual string IsActive { get; set; }

		// DAY 
		public virtual string DAY_1 { get; set; }
		public virtual string DAY_2 { get; set; }
		public virtual string DAY_3 { get; set; }
		public virtual string DAY_4 { get; set; }
		public virtual string DAY_5 { get; set; }
		public virtual string DAY_6 { get; set; }
		public virtual string DAY_7 { get; set; }
		public virtual string DAY_8 { get; set; }
		public virtual string DAY_9 { get; set; }
		public virtual string DAY_10 { get; set; }
		public virtual string DAY_11 { get; set; }
		public virtual string DAY_12 { get; set; }
		public virtual string DAY_13 { get; set; }
		public virtual string DAY_14 { get; set; }
		public virtual string DAY_15 { get; set; }
		public virtual string DAY_16 { get; set; }
		public virtual string DAY_17 { get; set; }
		public virtual string DAY_18 { get; set; }
		public virtual string DAY_19 { get; set; }
		public virtual string DAY_20 { get; set; }
		public virtual string DAY_21 { get; set; }
		public virtual string DAY_22 { get; set; }
		public virtual string DAY_23 { get; set; }
		public virtual string DAY_24 { get; set; }
		public virtual string DAY_25 { get; set; }
		public virtual string DAY_26 { get; set; }
		public virtual string DAY_27 { get; set; }
		public virtual string DAY_28 { get; set; }
		public virtual string DAY_29 { get; set; }
		public virtual string DAY_30 { get; set; }
		public virtual string DAY_31 { get; set; }

	}

}

