using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.LogW
{

	[Table("MstLgwContDevanningLT_T")]
	[Index(nameof(RenbanCode), Name = "IX_MstLgwContDevanningLT_T_RenbanCode")]
	[Index(nameof(IsActive), Name = "IX_MstLgwContDevanningLT_T_IsActive")]
	public class MstLgwContDevanningLT_T : FullAuditedEntity<long>, IEntity<long>
	{
		public const int MaxGuidLength = 128;

		public const int MaxRenbanCodeLength = 10;

		public const int MaxSourceLength = 10;

		public const int MaxIsActiveLength = 50;

		[StringLength(MaxGuidLength)]
		public virtual string Guid { get; set; }

		[StringLength(MaxRenbanCodeLength)]
		public virtual string RenbanCode { get; set; }

		[StringLength(MaxSourceLength)]
		public virtual string Source { get; set; }

		public virtual int? DevLeadtime { get; set; }

		[StringLength(MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

}

