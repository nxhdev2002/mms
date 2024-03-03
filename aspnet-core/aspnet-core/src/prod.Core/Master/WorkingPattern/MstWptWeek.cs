using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.WorkingPattern
{

	[Table("MstWptWeek")]
	[Index(nameof(WorkingYear), Name = "IX_MstWptWeek_WorkingYear")]
	public class MstWptWeek : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxIsActiveLength = 1;

		public virtual int? WorkingYear { get; set; }

		public virtual int? WeekNumber { get; set; }

		public virtual int? WorkingDays { get; set; }

		[StringLength(MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

}

