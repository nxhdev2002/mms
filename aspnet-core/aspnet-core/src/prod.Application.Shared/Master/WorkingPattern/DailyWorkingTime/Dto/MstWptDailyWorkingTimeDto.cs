using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.WorkingPattern.Dto
{

	public class MstWptDailyWorkingTimeDto : EntityDto<long?>
	{

		public virtual int? ShiftNo { get; set; }

		public virtual string ShiftName { get; set; }

		public virtual int? ShopId { get; set; }

		public virtual string ShopName { get; set; }

		public virtual DateTime? WorkingDate { get; set; }

		public virtual TimeSpan? StartTime { get; set; }

		public virtual TimeSpan? EndTime { get; set; }

		public virtual int? WorkingType { get; set; }

		public virtual string WorkingTypeDesc { get; set; }	

		public virtual string Description { get; set; }

		public virtual DateTime? FromTime { get; set; }

		public virtual DateTime? ToTime { get; set; }

		public virtual string IsManual { get; set; }

		public virtual string IsActive { get; set; }

	}

	public class CreateOrEditMstWptDailyWorkingTimeDto : EntityDto<long?>
	{

		public virtual int? ShiftNo { get; set; }

		[StringLength(MstWptDailyWorkingTimeConsts.MaxShiftNameLength)]
		public virtual string ShiftName { get; set; }

		public virtual int? ShopId { get; set; }

		public virtual DateTime? WorkingDate { get; set; }

		public virtual TimeSpan? StartTime { get; set; }

		public virtual TimeSpan? EndTime { get; set; }

		public virtual int? WorkingType { get; set; }

		[StringLength(MstWptDailyWorkingTimeConsts.MaxDescriptionLength)]
		public virtual string Description { get; set; }

		public virtual DateTime? FromTime { get; set; }

		public virtual DateTime? ToTime { get; set; }

		[StringLength(MstWptDailyWorkingTimeConsts.MaxIsManualLength)]
		public virtual string IsManual { get; set; }

		[StringLength(MstWptDailyWorkingTimeConsts.MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

	public class GetMstWptDailyWorkingTimeInput : PagedAndSortedResultRequestDto
	{

		public virtual int? ShiftNo { get; set; }

		public virtual string ShopName { get; set; }

		public virtual int? WorkingType { get; set; }

		public virtual DateTime? WorkingDateFrom { get; set; }

		public virtual DateTime? WorkingDateTo { get; set; }



	}

}

