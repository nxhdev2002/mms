using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Painting.Andon
{

	[Table("PtsAdoLineEfficiency")]
	[Index(nameof(Line), Name = "IX_PtsAdoLineEfficiency_Line")]
	[Index(nameof(Shift), Name = "IX_PtsAdoLineEfficiency_Shift")]
	[Index(nameof(WorkingDate), Name = "IX_PtsAdoLineEfficiency_WorkingDate")]
	[Index(nameof(IsActive), Name = "IX_PtsAdoLineEfficiency_IsActive")]
	public class PtsAdoLineEfficiency : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxLineLength = 5;

		public const int MaxShiftLength = 1;

		public const int MaxStopTimeLength = 10;

		public const int MaxEfficiencyLength = 10;

		public const int MaxTaktTimeLength = 10;

		public const int MaxOvertimeLength = 10;

		public const int MaxNonProdActLength = 10;

		public const int MaxIsActiveLength = 1;

		[Required]
		[StringLength(MaxLineLength)]
		public virtual string Line { get; set; }

		[StringLength(MaxShiftLength)]
		public virtual string Shift { get; set; }

		[Required]
		public virtual DateTime? WorkingDate { get; set; }

		public virtual int? VolTarget { get; set; }

		public virtual int? VolActual { get; set; }

		public virtual int? VolBalance { get; set; }

		[StringLength(MaxStopTimeLength)]
		public virtual string StopTime { get; set; }

		[StringLength(MaxEfficiencyLength)]
		public virtual string Efficiency { get; set; }

		[StringLength(MaxTaktTimeLength)]
		public virtual string TaktTime { get; set; }

		[StringLength(MaxOvertimeLength)]
		public virtual string Overtime { get; set; }

		[StringLength(MaxNonProdActLength)]
		public virtual string NonProdAct { get; set; }

		public virtual int? OffLine1 { get; set; }

		public virtual int? OffLine2 { get; set; }

		public virtual int? OffLine3 { get; set; }

		public virtual int? ShiftVolPlan { get; set; }

		[StringLength(MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

}

