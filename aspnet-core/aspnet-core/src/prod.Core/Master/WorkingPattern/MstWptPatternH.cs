using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.WorkingPattern
{

	[Table("MstWptPatternH")]
	[Index(nameof(Type), Name = "IX_MstWptPatternH_Type")]
	public class MstWptPatternH : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxDescriptionLength = 250;

		public const int MaxIsActiveLength = 1;

		[Required]
		public virtual int? Type { get; set; }

		[Column(TypeName = "date")]
		public virtual DateTime? StartDate { get; set; }

		[Column(TypeName = "date")]
		public virtual DateTime? EndDate { get; set; }

		[StringLength(MaxDescriptionLength)]
		public virtual string Description { get; set; }

		[StringLength(MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

}


