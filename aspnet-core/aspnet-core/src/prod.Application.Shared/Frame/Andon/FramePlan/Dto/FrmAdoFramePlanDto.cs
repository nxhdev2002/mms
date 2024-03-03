using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Frame.Andon.Dto
{

	public class FrmAdoFramePlanDto : EntityDto<long?>
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

	}

	

	public class GetFrmAdoFramePlanInput : PagedAndSortedResultRequestDto
	{
		public virtual string Model { get; set; }
		public virtual string LotNo { get; set; }
		public virtual string BodyNo { get; set; }
		public virtual string VinNo { get; set; }
		public virtual string PlanMonth { get; set; }

	}


    public class GetFrmAdoFramePlanExportInput
    {

        public virtual string Model { get; set; }
        public virtual string LotNo { get; set; }
        public virtual string BodyNo { get; set; }
        public virtual string VinNo { get; set; }
        public virtual string PlanMonth { get; set; }

    }

}


