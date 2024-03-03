using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.LogW.Pup
{

	[Table("LgwPupPxPUpPlan_T")]
	[Index(nameof(WorkingDate), Name = "IX_LgwPupPxPUpPlan_T_WorkingDate")]
	[Index(nameof(Shift), Name = "IX_LgwPupPxPUpPlan_T_Shift")]
	[Index(nameof(CaseNo), Name = "IX_LgwPupPxPUpPlan_T_CaseNo")]
	[Index(nameof(IsActive), Name = "IX_LgwPupPxPUpPlan_T_IsActive")]
	public class LgwPupPxPUpPlan_T : FullAuditedEntity<long>, IEntity<long>
	{
		public const int MaxGuidLength = 128;

		public const int MaxProdLineLength = 50;

		public const int MaxShiftLength = 50;

		public const int MaxCaseNoLength = 20;

		public const int MaxSupplierNoLength = 50;

		public const int MaxUpTableLength = 50;

		public const int MaxIsNoPxpDataLength = 20;

		public const int MaxUnpackingTimeLength = 50;

		public const int MaxWhLocationLength = 20;

		public const int MaxRemarksLength = 40;

		public const int MaxIsActiveLength = 1;

        public const int MaxStatusLength = 50;



        [StringLength(MaxGuidLength)]
		public virtual string Guid { get; set; }

		[StringLength(MaxProdLineLength)]
		public virtual string ProdLine { get; set; }

		[Column(TypeName = "date")]
		public virtual DateTime? WorkingDate { get; set; }

		[StringLength(MaxShiftLength)]
		public virtual string Shift { get; set; }

		public virtual int? NoInShift { get; set; }

		public virtual int? SeqLineIn { get; set; }

		[StringLength(MaxCaseNoLength)]
		public virtual string CaseNo { get; set; }

		[StringLength(MaxSupplierNoLength)]
		public virtual string SupplierNo { get; set; }

		[StringLength(MaxUpTableLength)]
		public virtual string UpTable { get; set; }

		[StringLength(MaxIsNoPxpDataLength)]
		public virtual string IsNoPxpData { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? UnpackingStartDatetime { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? UnpackingFinishDatetime { get; set; }

		[StringLength(MaxUnpackingTimeLength)]
		public virtual string UnpackingTime { get; set; }

		[Column(TypeName = "date")]
		public virtual DateTime? UnpackingDate { get; set; }

		public virtual DateTime? UnpackingDatetime { get; set; }

		public virtual int? UpLt { get; set; }

        [StringLength(MaxStatusLength)]
        public virtual string Status { get; set; }

		public virtual int? DelaySecond { get; set; }

		public virtual DateTime? DelayConfirmFlag { get; set; }

		[StringLength(MaxWhLocationLength)]
		public virtual string WhLocation { get; set; }

		public virtual DateTime? InvoiceDate { get; set; }

		[StringLength(MaxRemarksLength)]
		public virtual string Remarks { get; set; }

		[StringLength(MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

}

