using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.LogW.Pup.Dto
{

	public class LgwPupPxPUpPlanDto : EntityDto<long?>
	{

		public virtual string ProdLine { get; set; }

		public virtual DateTime? WorkingDate { get; set; }
        public virtual DateTime? WorkingDateFrom { get; set; }
        public virtual DateTime? WorkingDateTo { get; set; }

        public virtual string Shift { get; set; }

		public virtual int? NoInShift { get; set; }

		public virtual int? SeqLineIn { get; set; }

		public virtual string CaseNo { get; set; }

		public virtual string SupplierNo { get; set; }

		public virtual string UpTable { get; set; }

		public virtual string IsNoPxpData { get; set; }

		public virtual DateTime? UnpackingStartDatetime { get; set; }

		public virtual DateTime? UnpackingFinishDatetime { get; set; }

		public virtual string UnpackingTime { get; set; }

		public virtual DateTime? UnpackingDate { get; set; }

		public virtual DateTime? UnpackingDatetime { get; set; }

		public virtual int? UpLt { get; set; }

		public virtual string Status { get; set; }

		public virtual int? DelaySecond { get; set; }

		public virtual DateTime? DelayConfirmFlag { get; set; }

		public virtual string WhLocation { get; set; }

		public virtual DateTime? InvoiceDate { get; set; }

		public virtual string Remarks { get; set; }

		public virtual string IsActive { get; set; }


	}

	public class CreateOrEditLgwPupPxPUpPlanDto : EntityDto<long?>
	{

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

        public virtual DateTime? DelayConfirmFlag { get; set; }

        public virtual int? DelaySecond { get; set; }

        public virtual DateTime? InvoiceDate { get; set; }

        [StringLength(LgwPupPxPUpPlanConsts.MaxRemarksLength)]
        public virtual string Remarks { get; set; }

        [StringLength(LgwPupPxPUpPlanConsts.MaxStatusLength)]
        public virtual string Status { get; set; }

        public virtual DateTime? UnpackingDate { get; set; }

        public virtual DateTime? UnpackingDatetime { get; set; }

        [StringLength(LgwPupPxPUpPlanConsts.MaxUnpackingTimeLength)]
        public virtual string UnpackingTime { get; set; }

        public virtual int? UpLt { get; set; }

        [StringLength(LgwPupPxPUpPlanConsts.MaxWhLocationLength)]
        public virtual string WhLocation { get; set; }



    }

    public class GetLgwPupPxPUpPlanInput : PagedAndSortedResultRequestDto
	{

		public virtual string ProdLine { get; set; }
		public virtual DateTime? WorkingDate { get; set; }
        public virtual DateTime? WorkingDateFrom { get; set; }
        public virtual DateTime? WorkingDateTo { get; set; }
        public virtual string Shift { get; set; }
	}

    public class GetLgwPupPxPUpPlanExportInput
	{ 
        public virtual string ProdLine { get; set; }
        public virtual DateTime? WorkingDate { get; set; }
        public virtual DateTime? WorkingDateFrom { get; set; }
        public virtual DateTime? WorkingDateTo { get; set; }
        public virtual string Shift { get; set; }
    }

}


