using Abp.Application.Services.Dto;
using System;

namespace prod.LogW.Lup
{

	public class GetLotUnPackingAndonOutput : EntityDto<long?>
	{

		public virtual string ProdLine { get; set; }
		public virtual DateTime? WorkingDate { get; set; }
		public virtual string Shift { get; set; }
		public virtual string LotNo { get; set; }
		public virtual DateTime? PlanUnpackingStartDatetime { get; set; }
		public virtual DateTime? PlanUnpackingFinishDatetime { get; set; }
		public virtual string Tpm { get; set; }
		public virtual string Remarks { get; set; }
		public virtual int? UnpackingLeadTime { get; set; }
		public virtual int? ActualTime { get; set; }

		public virtual DateTime? UpCalltime { get; set; }
		public virtual DateTime? UnpackingActualStartDatetime { get; set; }
		public virtual DateTime? UnpackingActualFinishDatetime { get; set; }
		public virtual string UpStatus { get; set; }
		public virtual int? FinishedMkModuleCount { get; set; }
		public virtual int? MkModuleCount { get; set; }
		public virtual string IsDelay { get; set; }
		public virtual int? OtTime { get; set; }
		public virtual string ScreenStatus { get; set; }



	}
	  

}

