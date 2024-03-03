using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.LogA.Lds
{

	[Table("LgaLdsLotPlan_T")]
	[Index(nameof(WorkingDate), Name = "IX_LgaLdsLotPlan_T_WorkingDate")]
	[Index(nameof(Shift), Name = "IX_LgaLdsLotPlan_T_Shift")]
	[Index(nameof(Status), Name = "IX_LgaLdsLotPlan_T_Status")]
	[Index(nameof(IsActive), Name = "IX_LgaLdsLotPlan_T_IsActive")]
	public class LgaLdsLotPlan_T : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxGuidLength = 128;

		public const int MaxProdLineLength = 50;

		public const int MaxShiftLength = 50;

		public const int MaxLotNoLength = 50;

		public const int MaxLotNo2Length = 50;

		public const int MaxDollyLength = 50;

		public const int MaxStatusLength = 50;

		public const int MaxIsActiveLength = 1;

		public const int MaxModelLength = 1;




        [StringLength(MaxGuidLength)]
		public virtual string Guid { get; set; }

		[StringLength(MaxProdLineLength)]
		public virtual string ProdLine { get; set; }

		[Column(TypeName = "date")]
		public virtual DateTime? WorkingDate { get; set; }

		[StringLength(MaxShiftLength)]
		public virtual string Shift { get; set; }

		public virtual int? SeqLineIn { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? PlanStartDatetime { get; set; }

        [StringLength(MaxModelLength)]
        public virtual string Model { get; set; }

        [StringLength(MaxLotNoLength)]
		public virtual string LotNo { get; set; }

		[StringLength(MaxLotNo2Length)]
		public virtual string LotNo2 { get; set; }

		public virtual int? Trip { get; set; }

		[StringLength(MaxDollyLength)]
		public virtual string Dolly { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? StartDatetime { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? FinishDatetime { get; set; }

		public virtual int? DelaySecond { get; set; }

		[StringLength(MaxStatusLength)]
		public virtual string Status { get; set; }

		[StringLength(MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

}

