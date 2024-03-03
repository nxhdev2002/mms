using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.LogA.Lds.Dto
{

	public class LgaLdsLotPlanDto : EntityDto<long?>
	{

		public virtual string ProdLine { get; set; }

		public virtual DateTime? WorkingDate { get; set; }

        public virtual DateTime? WorkingDateFrom { get; set; }
        public virtual DateTime? WorkingDateTo { get; set; }

        public virtual string Shift { get; set; }

		public virtual int? SeqLineIn { get; set; }

		public virtual DateTime? PlanStartDatetime { get; set; }

        public virtual string Model { get; set; }

        public virtual string LotNo { get; set; }

		public virtual string LotNo2 { get; set; }

		public virtual int? Trip { get; set; }

		public virtual string Dolly { get; set; }

		public virtual DateTime? StartDatetime { get; set; }

		public virtual DateTime? FinishDatetime { get; set; }

		public virtual int? DelaySecond { get; set; }

		public virtual string Status { get; set; }

		public virtual string IsActive { get; set; }

	}

	public class CreateOrEditLgaLdsLotPlanDto : EntityDto<long?>
	{

		[StringLength(LgaLdsLotPlanConsts.MaxProdLineLength)]
		public virtual string ProdLine { get; set; }

		public virtual DateTime? WorkingDate { get; set; }

		[StringLength(LgaLdsLotPlanConsts.MaxShiftLength)]
		public virtual string Shift { get; set; }

		public virtual int? SeqLineIn { get; set; }

		public virtual DateTime? PlanStartDatetime { get; set; }

        [StringLength(LgaLdsLotPlanConsts.MaxModelLength)]
        public virtual string Model { get; set; }

        [StringLength(LgaLdsLotPlanConsts.MaxLotNoLength)]
		public virtual string LotNo { get; set; }

		[StringLength(LgaLdsLotPlanConsts.MaxLotNo2Length)]
		public virtual string LotNo2 { get; set; }

		public virtual int? Trip { get; set; }

		[StringLength(LgaLdsLotPlanConsts.MaxDollyLength)]
		public virtual string Dolly { get; set; }

		public virtual DateTime? StartDatetime { get; set; }

		public virtual DateTime? FinishDatetime { get; set; }

		public virtual int? DelaySecond { get; set; }

		[StringLength(LgaLdsLotPlanConsts.MaxStatusLength)]
		public virtual string Status { get; set; }

		[StringLength(LgaLdsLotPlanConsts.MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

	public class GetLgaLdsLotPlanInput : PagedAndSortedResultRequestDto
	{
        public virtual DateTime? WorkingDateFrom { get; set; }
        public virtual DateTime? WorkingDateTo { get; set; }
        public virtual string ProdLine { get; set; }
		public virtual DateTime? WorkingDate { get; set; }
	}

    public class GetLgaLdsLotPlanExportInput 

    {
        public virtual DateTime? WorkingDateFrom { get; set; }
        public virtual DateTime? WorkingDateTo { get; set; }
        public virtual string ProdLine { get; set; }
        public virtual DateTime? WorkingDate { get; set; }
    }

}


