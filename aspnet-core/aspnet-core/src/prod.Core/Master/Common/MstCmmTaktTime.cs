using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Cmm
{

	[Table("MstCmmTaktTime")]
	[Index(nameof(ProdLine), Name = "IX_MstCmmTaktTime_ProdLine")]
	[Index(nameof(GroupCd), Name = "IX_MstCmmTaktTime_GroupCd")]
	[Index(nameof(IsActive), Name = "IX_MstCmmTaktTime_IsActive")]
	public class MstCmmTaktTime : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxProdLineLength = 50;

		public const int MaxGroupCdLength = 50;

		public const int MaxTaktTimeLength = 50;

		public const int MaxIsActiveLength = 1;

		[StringLength(MaxProdLineLength)]
		public virtual string ProdLine { get; set; }

		[StringLength(MaxGroupCdLength)]
		public virtual string GroupCd { get; set; }

		public virtual int? TaktTimeSecond { get; set; }

		[StringLength(MaxTaktTimeLength)]
		public virtual string TaktTime { get; set; }

		[StringLength(MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

}


