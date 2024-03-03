using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.WorkingPattern
{

	[Table("MstWptDailyWorkingTime")]
	[Index(nameof(ShiftNo), Name = "IX_MstWptDailyWorkingTime_ShiftNo")]
	[Index(nameof(ShiftName), Name = "IX_MstWptDailyWorkingTime_ShiftName")]
	[Index(nameof(ShopId), Name = "IX_MstWptDailyWorkingTime_ShopId")]
	[Index(nameof(WorkingDate), Name = "IX_MstWptDailyWorkingTime_WorkingDate")]
	[Index(nameof(WorkingType), Name = "IX_MstWptDailyWorkingTime_WorkingType")]
	[Index(nameof(IsActive), Name = "IX_MstWptDailyWorkingTime_IsActive")]
	public class MstWptDailyWorkingTime : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxShiftNameLength = 40;

		public const int MaxDescriptionLength = 250;

		public const int MaxIsManualLength = 1;

		public const int MaxIsActiveLength = 1;

		public virtual int? ShiftNo { get; set; }

		[StringLength(MaxShiftNameLength)]
		public virtual string ShiftName { get; set; }

		public virtual int? ShopId { get; set; }

		[Column(TypeName = "date")]
		public virtual DateTime? WorkingDate { get; set; }

		public virtual TimeSpan? StartTime { get; set; }

		public virtual TimeSpan? EndTime { get; set; }

		public virtual int? WorkingType { get; set; }

		[StringLength(MaxDescriptionLength)]
		public virtual string Description { get; set; }

		public virtual DateTime? FromTime { get; set; }

		public virtual DateTime? ToTime { get; set; }

		[StringLength(MaxIsManualLength)]
		public virtual string IsManual { get; set; }

		[StringLength(MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

}

