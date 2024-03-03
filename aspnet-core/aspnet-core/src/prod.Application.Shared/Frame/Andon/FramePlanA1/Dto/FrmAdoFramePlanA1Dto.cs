using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Frame.Andon.Dto
{

	public class FrmAdoFramePlanA1Dto : EntityDto<long?>
	{

		public virtual int? No { get; set; }

		public virtual string Model { get; set; }

		public virtual string LotNo { get; set; }

		public virtual int? NoInLot { get; set; }

		public virtual string BodyNo { get; set; }

		public virtual string Color { get; set; }

		public virtual string VinNo { get; set; }

		public virtual string FrameId { get; set; }

		public virtual string Status { get; set; }

		public virtual string PlanMonth { get; set; }

		public virtual DateTime? PlanDate { get; set; }

		public virtual string Grade { get; set; }

		public virtual string Version { get; set; }

		public virtual string IsActive { get; set; }

        public virtual string MessagesError { get; set; }

    }

	public class CreateOrEditFrmAdoFramePlanA1Dto : EntityDto<long?>
	{

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
	}

	public class GetFrmAdoFramePlanA1Input : PagedAndSortedResultRequestDto
	{
		public virtual string Model { get; set; }
		public virtual string LotNo { get; set; }
		public virtual string BodyNo { get; set; }
		public virtual string VinNo { get; set; }	
		public virtual string PlanMonth { get; set; }

	}

    public class GetFrmAdoFramePlanA1ExportInput
	{ 
        public virtual string Model { get; set; }
        public virtual string LotNo { get; set; }
        public virtual string BodyNo { get; set; }
        public virtual string VinNo { get; set; }
        public virtual string PlanMonth { get; set; }

    }

}

