using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.LogW.Pup.ImportDto

{

	public class ImportPxPUpPlanDto 
	{
		[StringLength(LgwPupPxPUpPlanConsts.MaxGuidLength)]
		public virtual string Guid { get; set; }

		[StringLength(LgwPupPxPUpPlanConsts.MaxProdLineLength)]
		public virtual string ProdLine { get; set; }

		public virtual DateTime? WorkingDate { get; set; }

		[StringLength(LgwPupPxPUpPlanConsts.MaxShiftLength)]
		public virtual string Shift { get; set; }

		public virtual int? NoInShift { get; set; }

		public virtual int? SeqLineIn { get; set; }

		[StringLength(LgwPupPxPUpPlanConsts.MaxCaseNoLength)]
		public virtual string CaseNo { get; set; }

		[StringLength(LgwPupPxPUpPlanConsts.MaxSupplierNoLength)]
		public virtual string SupplierNo { get; set; }

		[StringLength(LgwPupPxPUpPlanConsts.MaxUpTableLength)]
		public virtual string UpTable { get; set; }

		[StringLength(LgwPupPxPUpPlanConsts.MaxIsNoPxpDataLength)]
		public virtual string IsNoPxpData { get; set; }

		public virtual DateTime? UnpackingStartDatetime { get; set; }

		public virtual DateTime? UnpackingFinishDatetime { get; set; }

		[StringLength(LgwPupPxPUpPlanConsts.MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
		public string Exception { get; set; }
		public bool CanBeImported()
		{
			return string.IsNullOrEmpty(Exception);
		}
	}


}

