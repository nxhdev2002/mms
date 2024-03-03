using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
using prod.Plan.Ccr.Production;

namespace prod.Plan.Ccr.Dto
{

    public class PlnCcrProductionPlanDto : EntityDto<long?>
	{

		public virtual long? PlanSequence { get; set; }

		public virtual string Shop { get; set; }

		public virtual string Model { get; set; }

		public virtual string LotNo { get; set; }

		public virtual string NoInLot { get; set; }

		public virtual string Grade { get; set; }

		public virtual string Body { get; set; }

		public virtual DateTime? DateIn { get; set; }

		public virtual TimeSpan? TimeIn { get; set; }

		public virtual DateTime? DateTimeIn { get; set; }

		public virtual string SupplierNo { get; set; }

		public virtual string UseLotNo { get; set; }

		public virtual string SupplierNo2 { get; set; }

		public virtual string UseLotNo2 { get; set; }

		public virtual string UseNoInLot { get; set; }

	}

	public class CreateOrEditPlnCcrProductionPlanDto : EntityDto<long?>
	{

		public virtual long? PlanSequence { get; set; }

		[StringLength(PlnCcrProductionPlanConsts.MaxShopLength)]
		public virtual string Shop { get; set; }

		[StringLength(PlnCcrProductionPlanConsts.MaxModelLength)]
		public virtual string Model { get; set; }

		[StringLength(PlnCcrProductionPlanConsts.MaxLotNoLength)]
		public virtual string LotNo { get; set; }

		[StringLength(PlnCcrProductionPlanConsts.MaxNoInLotLength)]
		public virtual string NoInLot { get; set; }

		[StringLength(PlnCcrProductionPlanConsts.MaxGradeLength)]
		public virtual string Grade { get; set; }

		[StringLength(PlnCcrProductionPlanConsts.MaxBodyLength)]
		public virtual string Body { get; set; }

		public virtual DateTime? DateIn { get; set; }

		public virtual TimeSpan? TimeIn { get; set; }

		public virtual DateTime? DateTimeIn { get; set; }

		[StringLength(PlnCcrProductionPlanConsts.MaxSupplierNoLength)]
		public virtual string SupplierNo { get; set; }

		[StringLength(PlnCcrProductionPlanConsts.MaxUseLotNoLength)]
		public virtual string UseLotNo { get; set; }

		[StringLength(PlnCcrProductionPlanConsts.MaxSupplierNo2Length)]
		public virtual string SupplierNo2 { get; set; }

		[StringLength(PlnCcrProductionPlanConsts.MaxUseLotNo2Length)]
		public virtual string UseLotNo2 { get; set; }

		[StringLength(PlnCcrProductionPlanConsts.MaxUseNoInLotLength)]
		public virtual string UseNoInLot { get; set; }
	}

	public class GetPlnCcrProductionPlanInput : PagedAndSortedResultRequestDto
	{
		public virtual string Shop { get; set; }
		public virtual string Model { get; set; }
		public virtual string LotNo { get; set; }
		public virtual DateTime? DateIn { get; set; }
	}

    public class GetPlnCcrProductionPlanExportInput
    {
        public virtual string Shop { get; set; }
        public virtual string Model { get; set; }
        public virtual string LotNo { get; set; }
        public virtual DateTime? DateIn { get; set; }
    }
}


