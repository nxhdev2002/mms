using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.LogW
{

	[Table("MstLgwContDevanningLT")]
	[Index(nameof(RenbanCode), Name = "IX_MstLgwContDevanningLT_RenbanCode")]
	[Index(nameof(IsActive), Name = "IX_MstLgwContDevanningLT_IsActive")]
	public class MstLgwContDevanningLT : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxRenbanCodeLength = 10;

		public const int MaxSourceLength = 10;

		public const int MaxIsActiveLength = 50;

		[StringLength(MaxRenbanCodeLength)]
		public virtual string RenbanCode { get; set; }

		[StringLength(MaxSourceLength)]
		public virtual string Source { get; set; }

		public virtual int? DevLeadtime { get; set; }

		[StringLength(MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

}

