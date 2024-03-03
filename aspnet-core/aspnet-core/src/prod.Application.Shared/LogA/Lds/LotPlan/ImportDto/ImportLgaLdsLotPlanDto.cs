using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace prod.LogA.Lds.LotPlan.ImportDto
{

	public class ImportLgaLdsLotPlanDto : EntityDto<long?>
	{
		[StringLength(LgaLdsLotPlanConsts.MaxGuidLength)]
		public virtual string Guid { get; set; }

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

		public string Exception { get; set; }
		public bool CanBeImported()
		{
			return string.IsNullOrEmpty(Exception);
		}
	}

}
