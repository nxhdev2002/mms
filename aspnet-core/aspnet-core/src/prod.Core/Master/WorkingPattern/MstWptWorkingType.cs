using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.WorkingPattern
{

	[Table("MstWptWorkingType")]
	[Index(nameof(WorkingType), Name = "IX_MstWptWorkingType_WorkingType")]
	public class MstWptWorkingType : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxDescriptionLength = 250;

		public const int MaxIsActiveLength = 1;

		public virtual int? WorkingType { get; set; }

		[StringLength(MaxDescriptionLength)]
		public virtual string Description { get; set; }

		[StringLength(MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

}
