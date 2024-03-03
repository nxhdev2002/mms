using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Assy
{

	[Table("MstAsyProcess")]
	[Index(nameof(ProcessSeq), Name = "IX_MstAsyProcess_ProcessSeq")]
	[Index(nameof(ProcessCode), Name = "IX_MstAsyProcess_ProcessCode")]
    [Index(nameof(ProdLine), Name = "IX_MstAsyProcess_ProdLine")]
    [Index(nameof(IsActive), Name = "IX_MstAsyProcess_IsActive")]
    public class MstAsyProcess : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxProcessCodeLength = 50;

		public const int MaxProcessNameLength = 1000;

		public const int MaxProcessDescLength = 1000;

		public const int MaxGroupNameLength = 100;

		public const int MaxSubgroupNameLength = 100;

        public const int MaxProdLineLength = 50;

        public const int MaxIsActiveLength = 1;

        public virtual int? ProcessSeq { get; set; }

		[StringLength(MaxProcessCodeLength)]
		public virtual string ProcessCode { get; set; }

		[StringLength(MaxProcessNameLength)]
		public virtual string ProcessName { get; set; }

        [StringLength(MaxProcessDescLength)]
        public virtual string ProcessDesc { get; set; }

		public virtual int? ProcessGroup { get; set; }

		[StringLength(MaxGroupNameLength)]
		public virtual string GroupName { get; set; }

        public virtual int? ProcessSubgroup { get; set; }

        [StringLength(MaxSubgroupNameLength)]
		public virtual string SubgroupName { get; set; }

		[StringLength(MaxProdLineLength)]
		public virtual string ProdLine { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}
