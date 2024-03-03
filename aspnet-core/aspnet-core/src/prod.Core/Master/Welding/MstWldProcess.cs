using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Welding
{

	[Table("MstWldProcess")]
	[Index(nameof(ProcessSeq), Name = "IX_MstWldProcess_ProcessSeq")]
	[Index(nameof(ProcessCode), Name = "IX_MstWldProcess_ProcessCode")]
	[Index(nameof(ProcessSubgroup), Name = "IX_MstWldProcess_ProcessSubgroup")]
	[Index(nameof(SubgroupName), Name = "IX_MstWldProcess_SubgroupName")]
	public class MstWldProcess : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxProcessCodeLength = 50;

		public const int MaxProcessNameLength = 1000;

		public const int MaxProcessDescLength = 1000;

		public const int MaxGroupNameLength = 100;

		public const int MaxSubgroupNameLength = 100;

		public const int MaxIsActiveLength = 1;

		[Required]
		public virtual int? ProcessSeq { get; set; }

		[Required]
		[StringLength(MaxProcessCodeLength)]
		public virtual string ProcessCode { get; set; }

		[Column(TypeName = "nvarchar")]
		[StringLength(MaxProcessNameLength)]
		public virtual string ProcessName { get; set; }

		[Column(TypeName = "nvarchar")]
		[StringLength(MaxProcessDescLength)]
		public virtual string ProcessDesc { get; set; }

		[Required]
		public virtual int? ProcessGroup { get; set; }

		[StringLength(MaxGroupNameLength)]
		public virtual string GroupName { get; set; }

		public virtual int? ProcessSubgroup { get; set; }

		[StringLength(MaxSubgroupNameLength)]
		public virtual string SubgroupName { get; set; }

		[StringLength(MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

}

