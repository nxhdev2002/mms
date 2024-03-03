using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.LogW.Lup.Dto
{

	public class LgwLupLotUpPlanDto : EntityDto<long?>
	{

		public virtual string ProdLine { get; set; }

		public virtual DateTime? WorkingDate { get; set; }
        public virtual DateTime? WorkingDateFrom { get; set; }
        public virtual DateTime? WorkingDateTo { get; set; }

        public virtual string Shift { get; set; }

		public virtual int? NoInShift { get; set; }

		public virtual int? NoInDay { get; set; }

		public virtual string LotNo { get; set; }

		public virtual int? LotPartialNo { get; set; }

		public virtual DateTime? UnpackingStartDatetime { get; set; }

		public virtual DateTime? UnpackingFinishDatetime { get; set; }

		public virtual string Tpm { get; set; }

		public virtual string Remarks { get; set; }


        public virtual DateTime? UpCalltime { get; set; }

        public virtual DateTime? UnpackingActualFinishDatetime { get; set; }

        public virtual DateTime? UnpackingActualStartDatetime { get; set; }

        public virtual string UpStatus { get; set; }

        public virtual DateTime? MakingFinishDatetime { get; set; }

        public virtual string MkStatus { get; set; }

        public virtual string IsActive { get; set; }

	}

	public class CreateOrEditLgwLupLotUpPlanDto : EntityDto<long?>
	{

		[StringLength(LgwLupLotUpPlanConsts.MaxProdLineLength)]
		public virtual string ProdLine { get; set; }

		public virtual DateTime? WorkingDate { get; set; }

		[StringLength(LgwLupLotUpPlanConsts.MaxShiftLength)]
		public virtual string Shift { get; set; }

		public virtual int? NoInShift { get; set; }

		public virtual int? NoInDay { get; set; }

		[StringLength(LgwLupLotUpPlanConsts.MaxLotNoLength)]
		public virtual string LotNo { get; set; }

		public virtual int? LotPartialNo { get; set; }

		public virtual DateTime? UnpackingStartDatetime { get; set; }

		public virtual DateTime? UnpackingFinishDatetime { get; set; }

		[StringLength(LgwLupLotUpPlanConsts.MaxTpmLength)]
		public virtual string Tpm { get; set; }

		[StringLength(LgwLupLotUpPlanConsts.MaxRemarksLength)]
		public virtual string Remarks { get; set; }

        public virtual DateTime? UpCalltime { get; set; }

        public virtual DateTime? UnpackingActualFinishDatetime { get; set; }

        public virtual DateTime? UnpackingActualStartDatetime { get; set; }

        [StringLength(LgwLupLotUpPlanConsts.MaxUpStatusLength)]
        public virtual string UpStatus { get; set; }

        public virtual DateTime? MakingFinishDatetime { get; set; }

        [StringLength(LgwLupLotUpPlanConsts.MaxMkStatusLength)]
        public virtual string MkStatus { get; set; }

        [StringLength(LgwLupLotUpPlanConsts.MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

	public class GetLgwLupLotUpPlanInput : PagedAndSortedResultRequestDto
	{
		public virtual DateTime? WorkingDate { get; set; }
        public virtual DateTime? WorkingDateFrom { get; set; }
        public virtual DateTime? WorkingDateTo { get; set; }
        public virtual string LotNo { get; set; }

	}

    public class GetLgwLupLotUpPlanExportInput
    {
        public virtual DateTime? WorkingDate { get; set; }
        public virtual DateTime? WorkingDateFrom { get; set; }
        public virtual DateTime? WorkingDateTo { get; set; }

        public virtual string LotNo { get; set; }

    }



}


