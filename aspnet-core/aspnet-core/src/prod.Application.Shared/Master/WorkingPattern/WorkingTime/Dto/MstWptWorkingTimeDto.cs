using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.WorkingPattern.Dto
{

	public class MstWptWorkingTimeDto : EntityDto<long?>
	{

		public virtual int? ShiftNo { get; set; }

		public virtual int? ShopId { get; set; }

		public virtual int? WorkingType { get; set; }

		public virtual TimeSpan? StartTime { get; set; }

		public virtual TimeSpan? EndTime { get; set; }

		public virtual string Description { get; set; }

		public virtual int? PatternHId { get; set; }

		public virtual string SeasonType { get; set; }

		public virtual string DayOfWeek { get; set; }

		public virtual int? WeekWorkingDays { get; set; }

		public virtual string IsActive { get; set; }

	}

	public class CreateOrEditMstWptWorkingTimeDto : EntityDto<long?>
	{

		public virtual int? ShiftNo { get; set; }

		public virtual int? ShopId { get; set; }

		public virtual int? WorkingType { get; set; }

		public virtual TimeSpan? StartTime { get; set; }

		public virtual TimeSpan? EndTime { get; set; }

		[StringLength(MstWptWorkingTimeConsts.MaxDescriptionLength)]
		public virtual string Description { get; set; }

		public virtual int? PatternHId { get; set; }

		[StringLength(MstWptWorkingTimeConsts.MaxSeasonTypeLength)]
		public virtual string SeasonType { get; set; }

		[StringLength(MstWptWorkingTimeConsts.MaxDayOfWeekLength)]
		public virtual string DayOfWeek { get; set; }

		public virtual int? WeekWorkingDays { get; set; }

		[StringLength(MstWptWorkingTimeConsts.MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

	public class GetMstWptWorkingTimeInput : PagedAndSortedResultRequestDto
	{

		public virtual int? ShiftNo { get; set; }

		public virtual int? ShopId { get; set; }

		public virtual int? WorkingType { get; set; }

		public virtual string Description { get; set; }

		public virtual int? PatternHId { get; set; }

		public virtual TimeSpan? StartTime { get; set; }

		public virtual TimeSpan? EndTime { get; set; }

		public virtual string SeasonType { get; set; }

		public virtual string DayOfWeek { get; set; }

		public virtual int? WeekWorkingDays { get; set; }

		public virtual string IsActive { get; set; }

	}

	public class MstWptWorkingTimeDto_Dapper : EntityDto<long?>
	{
		 
		public virtual int? RowNo { get; set; }

		public virtual int? ShiftNo { get; set; }

		public virtual int? ShopId { get; set; }

		public virtual string ShopName { get; set; }

		public virtual int? WorkingType { get; set; }

		public virtual string WorkingTypeDesc { get; set; }

		public virtual TimeSpan? StartTime { get; set; }

		public virtual TimeSpan? EndTime { get; set; }

		public virtual string Description { get; set; }

		public virtual int? PatternHId { get; set; }

		public virtual string SeasonType { get; set; }

		public virtual string DayOfWeek { get; set; }

		public virtual int? WeekWorkingDays { get; set; }

		public virtual string SeasonDayOfWeek { get; set; }

		public virtual string IsActive { get; set; }

		public virtual DateTime? StartDate { get; set; }

		public virtual DateTime? EndDate { get; set; }

		public virtual string PatternDescription { get; set; }

	}

	public class GetMstWptWorkingTimeInput_Dapper : PagedAndSortedResultRequestDto
	{
		public virtual int? ShiftNo { get; set; }

		public virtual string ShopName { get; set; }

	}

}


