using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Inventory
{

	[Table("MstInvDevanningCaseType")]
	[Index(nameof(Source), Name = "IX_MstInvDevanningCaseType_Source")]
	[Index(nameof(CaseNo), Name = "IX_MstInvDevanningCaseType_CaseNo")]
	public class MstInvDevanningCaseType : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxSourceLength = 10;

		public const int MaxCaseNoLength = 5;

		public const int MaxShoptypeCodeLength = 2;

		public const int MaxTypeLength = 10;

		public const int MaxCarFamilyCodeLength = 4;

		public const int MaxIsActiveLength = 1;

		[StringLength(MaxSourceLength)]
		public virtual string Source { get; set; }

		[StringLength(MaxCaseNoLength)]
		public virtual string CaseNo { get; set; }

		[StringLength(MaxShoptypeCodeLength)]
		public virtual string ShoptypeCode { get; set; }

		[StringLength(MaxTypeLength)]
		public virtual string Type { get; set; }

		[StringLength(MaxCarFamilyCodeLength)]
		public virtual string CarFamilyCode { get; set; }

		[StringLength(MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

}

