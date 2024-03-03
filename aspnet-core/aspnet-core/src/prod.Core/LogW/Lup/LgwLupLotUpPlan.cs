using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.LogW.Lup
{

	[Table("LgwLupLotUpPlan")]
	[Index(nameof(ProdLine), Name = "IX_LgwLupLotUpPlan_ProdLine")]
	[Index(nameof(WorkingDate), Name = "IX_LgwLupLotUpPlan_WorkingDate")]
	[Index(nameof(Shift), Name = "IX_LgwLupLotUpPlan_Shift")]
	[Index(nameof(MkStatus), Name = "IX_LgwLupLotUpPlan_MkStatus")]
	[Index(nameof(UpStatus), Name = "IX_LgwLupLotUpPlan_UpStatus")]
	[Index(nameof(IsActive), Name = "IX_LgwLupLotUpPlan_IsActive")]
	public class LgwLupLotUpPlan : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxProdLineLength = 50;

		public const int MaxShiftLength = 50;

		public const int MaxLotNoLength = 20;

		public const int MaxTpmLength = 200;

		public const int MaxRemarksLength = 200;

		public const int MaxMkStatusLength = 50;

		public const int MaxUpStatusLength = 50;

		public const int MaxIsActiveLength = 1;

		[StringLength(MaxProdLineLength)]
		public virtual string ProdLine { get; set; }

		[Column(TypeName = "date")]
		public virtual DateTime? WorkingDate { get; set; }

		[StringLength(MaxShiftLength)]
		public virtual string Shift { get; set; }

		public virtual int? NoInShift { get; set; }

		public virtual int? NoInDay { get; set; }

		[StringLength(MaxLotNoLength)]
		public virtual string LotNo { get; set; }

		public virtual int? LotPartialNo { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? UnpackingStartDatetime { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? UnpackingFinishDatetime { get; set; }

		[Column(TypeName = "nvarchar(200)")]
		[StringLength(MaxTpmLength)]
		public virtual string Tpm { get; set; }

		[Column(TypeName = "nvarchar(200)")]
		[StringLength(MaxRemarksLength)]
		public virtual string Remarks { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? UpCalltime { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? UnpackingActualStartDatetime { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? UnpackingActualFinishDatetime { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? MakingFinishDatetime { get; set; }

		[Column(TypeName = "nvarchar(200)")]
		[StringLength(MaxMkStatusLength)]
		public virtual string MkStatus { get; set; }

		[Column(TypeName = "nvarchar(200)")]
		[StringLength(MaxUpStatusLength)]
		public virtual string UpStatus { get; set; }

		[StringLength(MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

}

