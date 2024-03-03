using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Painting.Andon
{

	[Table("PtsAdoLineEfficiencyDetails")]
	[Index(nameof(Line), Name = "IX_PtsAdoLineEfficiencyDetails_Line")]
	[Index(nameof(VolActual), Name = "IX_PtsAdoLineEfficiencyDetails_VolActual")]
	[Index(nameof(LineStopTime), Name = "IX_PtsAdoLineEfficiencyDetails_LineStopTime")]
	public class PtsAdoLineEfficiencyDetails : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxLineLength = 5;

		public const int MaxShiftLength = 40;

		public const int MaxStatusLength = 1;

		[StringLength(MaxLineLength)]
		public virtual string Line { get; set; }

		public virtual int? VolActual { get; set; }

		public virtual int? LineStopTime { get; set; }

		public virtual decimal? LineEfficiency { get; set; }

		public virtual DateTime? WorkingDate { get; set; }

		[StringLength(MaxShiftLength)]
		public virtual string Shift { get; set; }

		[StringLength(MaxStatusLength)]
		public virtual string Status { get; set; }
	}

}


