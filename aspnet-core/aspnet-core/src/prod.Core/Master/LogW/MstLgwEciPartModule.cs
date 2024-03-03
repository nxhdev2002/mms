using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.LogW
{

	[Table("MstLgwEciPartModule")]
	[Index(nameof(ChkEciPartId), Name = "IX_MstLgwEciPartModule_ChkEciPartId")]
	[Index(nameof(IsActive), Name = "IX_MstLgwEciPartModule_IsActive")]
	public class MstLgwEciPartModule : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxPartNoLength = 12;

		public const int MaxCaseNoLength = 30;

		public const int MaxSupplierNoLength = 10;

		public const int MaxContainerNoLength = 15;

		public const int MaxRenbanLength = 6;

		public const int MaxCasePrefixLength = 8;

		public const int MaxEciTypeLength = 1;

		public const int MaxIsActiveLength = 1;

		[StringLength(MaxPartNoLength)]
		public virtual string PartNo { get; set; }

		[StringLength(MaxCaseNoLength)]
		public virtual string CaseNo { get; set; }

		[StringLength(MaxSupplierNoLength)]
		public virtual string SupplierNo { get; set; }

		[StringLength(MaxContainerNoLength)]
		public virtual string ContainerNo { get; set; }

		[StringLength(MaxRenbanLength)]
		public virtual string Renban { get; set; }

		[StringLength(MaxCasePrefixLength)]
		public virtual string CasePrefix { get; set; }

		[Required]
		public virtual long? ChkEciPartId { get; set; }

		[StringLength(MaxEciTypeLength)]
		public virtual string EciType { get; set; }

		[StringLength(MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

}


