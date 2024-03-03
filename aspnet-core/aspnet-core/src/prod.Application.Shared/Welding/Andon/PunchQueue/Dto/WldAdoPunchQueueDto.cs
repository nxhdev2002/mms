using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Welding.Andon.Dto
{

	public class WldAdoPunchQueueDto : EntityDto<long?>
	{

		public virtual string BodyNo { get; set; }

		public virtual string Model { get; set; }

		public virtual string Line { get; set; }

		public virtual string LotNo { get; set; }

		public virtual string Color { get; set; }

		public virtual int? ProcessGroup { get; set; }

		public virtual DateTime? ScanTime { get; set; }

		public virtual string PunchFlag { get; set; }

		public virtual string PunchIndicator { get; set; }

		public virtual string IsCall { get; set; }

		public virtual string IsCf { get; set; }

	}

	public class CreateOrEditWldAdoPunchQueueDto : EntityDto<long?>
	{

		[StringLength(WldAdoPunchQueueConsts.MaxBodyNoLength)]
		public virtual string BodyNo { get; set; }

		[StringLength(WldAdoPunchQueueConsts.MaxModelLength)]
		public virtual string Model { get; set; }

		[StringLength(WldAdoPunchQueueConsts.MaxLineLength)]
		public virtual string Line { get; set; }

		[StringLength(WldAdoPunchQueueConsts.MaxLotNoLength)]
		public virtual string LotNo { get; set; }

		[StringLength(WldAdoPunchQueueConsts.MaxColorLength)]
		public virtual string Color { get; set; }

		public virtual int? ProcessGroup { get; set; }

		public virtual DateTime? ScanTime { get; set; }

		[StringLength(WldAdoPunchQueueConsts.MaxPunchFlagLength)]
		public virtual string PunchFlag { get; set; }

		[StringLength(WldAdoPunchQueueConsts.MaxPunchIndicatorLength)]
		public virtual string PunchIndicator { get; set; }

		[StringLength(WldAdoPunchQueueConsts.MaxIsCallLength)]
		public virtual string IsCall { get; set; }

		[StringLength(WldAdoPunchQueueConsts.MaxIsCfLength)]
		public virtual string IsCf { get; set; }
	}

	public class GetWldAdoPunchQueueInput : PagedAndSortedResultRequestDto
	{
		public virtual string BodyNo { get; set; }
		public virtual string Model { get; set; }
		public virtual string LotNo { get; set; }
        public virtual DateTime? ScanTimeFrom { get; set; }

        public virtual DateTime? ScanTimeTo { get; set; }


    }

    public class GetWldAdoPunchQueueExportInput
    {
        public virtual string BodyNo { get; set; }
        public virtual string Model { get; set; }
        public virtual string LotNo { get; set; }

        public virtual DateTime? ScanTimeFrom { get; set; }

        public virtual DateTime? ScanTimeTo { get; set; }
    }
}


