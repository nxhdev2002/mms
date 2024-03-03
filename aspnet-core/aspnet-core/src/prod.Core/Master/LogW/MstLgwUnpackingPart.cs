using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.LogW
{

	[Table("MstLgwUnpackingPart")]
	[Index(nameof(BackNo), Name = "IX_MstLgwUnpackingPart_BackNo")]
	[Index(nameof(PartNo), Name = "IX_MstLgwUnpackingPart_PartNo")]
	[Index(nameof(IsActive), Name = "IX_MstLgwUnpackingPart_IsActive")]
	public class MstLgwUnpackingPart : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxCfcLength = 50;

		public const int MaxModelLength = 50;

		public const int MaxBackNoLength = 50;

		public const int MaxPartNoLength = 50;

		public const int MaxPartNameLength = 255;

		public const int MaxModuleCodeLength = 50;

		public const int MaxIsActiveLength = 1;

		[StringLength(MaxCfcLength)]
		public virtual string Cfc { get; set; }

		[StringLength(MaxModelLength)]
		public virtual string Model { get; set; }

		[StringLength(MaxBackNoLength)]
		public virtual string BackNo { get; set; }

		[StringLength(MaxPartNoLength)]
		public virtual string PartNo { get; set; }

		[StringLength(MaxPartNameLength)]
		public virtual string PartName { get; set; }

		[StringLength(MaxModuleCodeLength)]
		public virtual string ModuleCode { get; set; }

		[StringLength(MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

}

