using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Assy.Andon.Dto
{

	public class AsyAdoTotalDelayFinalAsakaiDto : EntityDto<long?>
	{

		public virtual string BodyNo { get; set; }

		public virtual string LotNo { get; set; }

		public virtual string Color { get; set; }

		public virtual string TotalDelayLeadTime { get; set; }

		public virtual DateTime? DispatchPlanDatetime { get; set; }

		public virtual string CurrLocation { get; set; }

		public virtual string Location { get; set; }

		public virtual string WDelayWithLeadTime { get; set; }

		public virtual string TDelayWithLeadTime { get; set; }

		public virtual string ADelayWithLeadTime { get; set; }

		public virtual string IDelayWithLeadTime { get; set; }

		public virtual string FOutDelay { get; set; }

		public virtual string DelayFlag { get; set; }

	}

	public class CreateOrEditAsyAdoTotalDelayFinalAsakaiDto : EntityDto<long?>
	{

		[Required]
		[StringLength(AsyAdoTotalDelayFinalAsakaiConsts.MaxBodyNoLength)]
		public virtual string BodyNo { get; set; }

		[Required]
		[StringLength(AsyAdoTotalDelayFinalAsakaiConsts.MaxLotNoLength)]
		public virtual string LotNo { get; set; }

		[StringLength(AsyAdoTotalDelayFinalAsakaiConsts.MaxColorLength)]
		public virtual string Color { get; set; }

		[StringLength(AsyAdoTotalDelayFinalAsakaiConsts.MaxTotalDelayLeadTimeLength)]
		public virtual string TotalDelayLeadTime { get; set; }

		public virtual DateTime? DispatchPlanDatetime { get; set; }

		[StringLength(AsyAdoTotalDelayFinalAsakaiConsts.MaxCurrLocationLength)]
		public virtual string CurrLocation { get; set; }

		[StringLength(AsyAdoTotalDelayFinalAsakaiConsts.MaxLocationLength)]
		public virtual string Location { get; set; }

		[StringLength(AsyAdoTotalDelayFinalAsakaiConsts.MaxWDelayWithLeadTimeLength)]
		public virtual string WDelayWithLeadTime { get; set; }

		[StringLength(AsyAdoTotalDelayFinalAsakaiConsts.MaxTDelayWithLeadTimeLength)]
		public virtual string TDelayWithLeadTime { get; set; }

		[StringLength(AsyAdoTotalDelayFinalAsakaiConsts.MaxADelayWithLeadTimeLength)]
		public virtual string ADelayWithLeadTime { get; set; }

		[StringLength(AsyAdoTotalDelayFinalAsakaiConsts.MaxIDelayWithLeadTimeLength)]
		public virtual string IDelayWithLeadTime { get; set; }

		[StringLength(AsyAdoTotalDelayFinalAsakaiConsts.MaxFOutDelayLength)]
		public virtual string FOutDelay { get; set; }

		[StringLength(AsyAdoTotalDelayFinalAsakaiConsts.MaxDelayFlagLength)]
		public virtual string DelayFlag { get; set; }
	}

	public class GetAsyAdoTotalDelayFinalAsakaiInput : PagedAndSortedResultRequestDto
	{
		public virtual string BodyNo { get; set; }
		public virtual string LotNo { get; set; }	

	}

    public class GetAsyAdoTotalDelayFinalAsakaiExportInput
    {
        public virtual string BodyNo { get; set; }
        public virtual string LotNo { get; set; }

    }

}


