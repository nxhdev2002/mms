using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Frame.Andon.Dto
{

	public class ImportFrmAdoFramePlanA1Dto 
	{
		[StringLength(FrmAdoFramePlanA1Consts.MaxGuidLength)]
		public virtual string Guid { get; set; }
		public virtual int? No { get; set; }

		[StringLength(FrmAdoFramePlanA1Consts.MaxModelLength)]
		public virtual string Model { get; set; }

		[StringLength(FrmAdoFramePlanA1Consts.MaxLotNoLength)]
		public virtual string LotNo { get; set; }

		public virtual int? NoInLot { get; set; }

		[StringLength(FrmAdoFramePlanA1Consts.MaxBodyNoLength)]
		public virtual string BodyNo { get; set; }

		[StringLength(FrmAdoFramePlanA1Consts.MaxColorLength)]
		public virtual string Color { get; set; }

		[StringLength(FrmAdoFramePlanA1Consts.MaxVinNoLength)]
		public virtual string VinNo { get; set; }

		[StringLength(FrmAdoFramePlanA1Consts.MaxFrameIdLength)]
		public virtual string FrameId { get; set; }

		[StringLength(FrmAdoFramePlanA1Consts.MaxStatusLength)]
		public virtual string Status { get; set; }

		[StringLength(FrmAdoFramePlanA1Consts.MaxPlanMonthLength)]
		public virtual string PlanMonth { get; set; }

		public virtual DateTime? PlanDate { get; set; }

		[StringLength(FrmAdoFramePlanA1Consts.MaxGradeLength)]
		public virtual string Grade { get; set; }

		[StringLength(FrmAdoFramePlanA1Consts.MaxVersionLength)]
		public virtual string Version { get; set; }

		[StringLength(FrmAdoFramePlanA1Consts.MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
		public string Exception { get; set; }
		public bool CanBeImported()
		{
			return string.IsNullOrEmpty(Exception);
		}
	}


}

