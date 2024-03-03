using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.LogW.Lup
{
 

	[Table("LgwLupLotUpPlan_T")]
	[Index(nameof(WorkingDate), Name = "IX_LgwLupLotUpPlan_T_WorkingDate")]
	[Index(nameof(Shift), Name = "IX_LgwLupLotUpPlan_T_Shift")]
	[Index(nameof(IsActive), Name = "IX_LgwLupLotUpPlan_T_IsActive")]
	public class LgwLupLotUpPlan_T : FullAuditedEntity<long>, IEntity<long>
	{
		public const int MaxGuidLength = 128;

		public const int MaxProdLineLength = 50;

		public const int MaxShiftLength = 50;

		public const int MaxLotNoLength = 20;

		public const int MaxTpmLength = 200;

		public const int MaxRemarksLength = 200;

		public const int MaxIsActiveLength = 1;

		[StringLength(MaxGuidLength)]
		public virtual string Guid { get; set; }


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

		[StringLength(MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}
}
