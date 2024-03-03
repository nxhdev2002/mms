using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
using prod.Master.LogA;

namespace prod.Welding.Andon.ImportDto
{

	public class ImportWldAdoWeldingPlanDto : EntityDto<long?>
    {
        [StringLength(MstLgaBp2PartListConsts.MaxGuidLength)]
        public virtual string Guid { get; set; }

        [Required]
		[StringLength(WldAdoWeldingPlanConsts.MaxModelLength)]
		public virtual string Model { get; set; }

		[Required]
		[StringLength(WldAdoWeldingPlanConsts.MaxLotNoLength)]
		public virtual string LotNo { get; set; }

		[Required]
		public virtual int? NoInLot { get; set; }

        [StringLength(WldAdoWeldingPlanConsts.MaxGradeLength)]
        public virtual string Grade { get; set; }

        [Required]
		[StringLength(WldAdoWeldingPlanConsts.MaxBodyNoLength)]
		public virtual string BodyNo { get; set; }

        [StringLength(WldAdoWeldingPlanConsts.MaxVinNoLength)]
        public virtual string VinNo { get; set; }

        [StringLength(WldAdoWeldingPlanConsts.MaxColorLength)]
		public virtual string Color { get; set; }

		[StringLength(WldAdoWeldingPlanConsts.MaxPlanTimeLength)]
		public virtual string PlanTime { get; set; }

		public virtual DateTime? RequestDate { get; set; }

		[StringLength(WldAdoWeldingPlanConsts.MaxShiftLength)]
		public virtual string Shift { get; set; }

		public virtual DateTime? WInDate { get; set; }

		public virtual DateTime? WOutDate { get; set; }

		public virtual DateTime? EdIn { get; set; }

        [StringLength(WldAdoWeldingPlanConsts.MaxWeldingLength)]
        public virtual string Welding { get; set; }

        public string Exception { get; set; }
        public bool CanBeImported()
        {
            return string.IsNullOrEmpty(Exception);
        }
    }

}

