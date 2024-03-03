using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Frame.Andon.Dto
{

	public class ImportFrmAdoFramePlanDto 
	{
		[StringLength(FrmAdoFramePlanConsts.MaxGuidLength)]
		public virtual string Guid { get; set; }
		public virtual int? No { get; set; }

		[StringLength(FrmAdoFramePlanConsts.MaxModelLength)]
		public virtual string Model { get; set; }

		[StringLength(FrmAdoFramePlanConsts.MaxLotNoLength)]
		public virtual string LotNo { get; set; }

		public virtual int? NoInLot { get; set; }

		[StringLength(FrmAdoFramePlanConsts.MaxBodyNoLength)]
		public virtual string BodyNo { get; set; }

		[StringLength(FrmAdoFramePlanConsts.MaxColorLength)]
		public virtual string Color { get; set; }

		[StringLength(FrmAdoFramePlanConsts.MaxVinNoLength)]
		public virtual string VinNo { get; set; }

		[StringLength(FrmAdoFramePlanConsts.MaxFrameIdLength)]
		public virtual string FrameId { get; set; }

		[StringLength(FrmAdoFramePlanConsts.MaxStatusLength)]
		public virtual string Status { get; set; }

		[StringLength(FrmAdoFramePlanConsts.MaxPlanMonthLength)]
		public virtual string PlanMonth { get; set; }

		public virtual DateTime? PlanDate { get; set; }

		[StringLength(FrmAdoFramePlanConsts.MaxGradeLength)]
		public virtual string Grade { get; set; }

		[StringLength(FrmAdoFramePlanConsts.MaxVersionLength)]
		public virtual string Version { get; set; }

		[StringLength(FrmAdoFramePlanConsts.MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
		public string Exception { get; set; }
        public bool CanBeImported()
        {
            return string.IsNullOrEmpty(Exception);
        }
    }


}

