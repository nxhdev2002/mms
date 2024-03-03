using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Welding
{

	[Table("MstWldPunchIndicator")]
	[Index(nameof(Grade), Name = "IX_MstWldPunchIndicator_Grade")]
	public class MstWldPunchIndicator : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxGradeLength = 50;

		public const int MaxIndicatorLength = 500;

		public const int MaxIsActiveLength = 1;

		[StringLength(MaxGradeLength)]
		public virtual string Grade { get; set; }

		[Column(TypeName = "nvarchar(500)")]
		[StringLength(MaxIndicatorLength)]
		public virtual string Indicator { get; set; }

		[StringLength(MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

}
