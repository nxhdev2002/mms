using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.LogW
{

	[Table("MstLgwEciPart")]
	[Index(nameof(ModuleNo), Name = "IX_MstLgwEciPart_ModuleNo")]
	[Index(nameof(PartNo), Name = "IX_MstLgwEciPart_PartNo")]
	[Index(nameof(SupplierNo), Name = "IX_MstLgwEciPart_SupplierNo")]
	[Index(nameof(IsActive), Name = "IX_MstLgwEciPart_IsActive")]
	public class MstLgwEciPart : FullAuditedEntity<long>, IEntity<long>
	{
		public const int MaxModuleNoLength = 10;

		public const int MaxPartNoLength = 20;

		public const int MaxSupplierNoLength = 10;

		public const int MaxModuleNoEciLength = 10;

		public const int MaxPartNoEciLength = 20;

		public const int MaxSupplierNoEciLength = 10;

		public const int MaxStartEciSeqLength = 20;

		public const int MaxStartEciRenbanLength = 20;

		public const int MaxStartEciModuleLength = 20;

		public const int MaxIsActiveLength = 1;

		[StringLength(MaxModuleNoLength)]
		public virtual string ModuleNo { get; set; }

		[StringLength(MaxPartNoLength)]
		public virtual string PartNo { get; set; }

		[StringLength(MaxSupplierNoLength)]
		public virtual string SupplierNo { get; set; }

		[StringLength(MaxModuleNoEciLength)]
		public virtual string ModuleNoEci { get; set; }

		[StringLength(MaxPartNoEciLength)]
		public virtual string PartNoEci { get; set; }

		[StringLength(MaxSupplierNoEciLength)]
		public virtual string SupplierNoEci { get; set; }

		[StringLength(MaxStartEciSeqLength)]
		public virtual string StartEciSeq { get; set; }

		[StringLength(MaxStartEciRenbanLength)]
		public virtual string StartEciRenban { get; set; }

		[StringLength(MaxStartEciModuleLength)]
		public virtual string StartEciModule { get; set; }

		[StringLength(MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

}

