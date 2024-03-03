using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Common
{

	[Table("MstCmmLookup")]
	[Index(nameof(DomainCode), Name = "IX_MstCmmLookup_DomainCode")]
	[Index(nameof(ItemOrder), Name = "IX_MstCmmLookup_ItemOrder")]
	public class MstCmmLookup : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxDomainCodeLength = 50;

		public const int MaxItemCodeLength = 50;

		public const int MaxItemValueLength = 1024;

		public const int MaxDescriptionLength = 1024;

		public const int MaxIsUseLength = 1;

		public const int MaxIsRestrictLength = 1;

		[StringLength(MaxDomainCodeLength)]
		public virtual string DomainCode { get; set; }

		[StringLength(MaxItemCodeLength)]
		public virtual string ItemCode { get; set; }

		[StringLength(MaxItemValueLength)]
		public virtual string ItemValue { get; set; }

		[Required]
		public virtual int? ItemOrder { get; set; }

		[StringLength(MaxDescriptionLength)]
		public virtual string Description { get; set; }

		[StringLength(MaxIsUseLength)]
		public virtual string IsUse { get; set; }

		[StringLength(MaxIsRestrictLength)]
		public virtual string IsRestrict { get; set; }
	}

}
