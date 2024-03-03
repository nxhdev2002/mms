using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.LogW.Pup
{

	[Table("LgwPupPxPUpPlanBase")]
	[Index(nameof(WorkingDate), Name = "IX_LgwPupPxPUpPlanBase_WorkingDate")]
	[Index(nameof(ProdLine), Name = "IX_LgwPupPxPUpPlanBase_ProdLine")]
	[Index(nameof(IsActive), Name = "IX_LgwPupPxPUpPlanBase_IsActive")]
	public class LgwPupPxPUpPlanBase : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxProdLineLength = 50;

		public const int MaxIsActiveLength = 1;

		[Column(TypeName = "date")]
		public virtual DateTime? WorkingDate { get; set; }

		[StringLength(MaxProdLineLength)]
		public virtual string ProdLine { get; set; }

		public virtual int? Shift1 { get; set; }

		public virtual int? Shift2 { get; set; }

		public virtual int? Shift3 { get; set; }

		[StringLength(MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

}


