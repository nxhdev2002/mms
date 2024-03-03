using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Assy.Andon
{

	[Table("AsyAdoTotalDelayFinalAsakai")]
	[Index(nameof(BodyNo), Name = "IX_AsyAdoTotalDelayFinalAsakai_BodyNo")]
	[Index(nameof(LotNo), Name = "IX_AsyAdoTotalDelayFinalAsakai_LotNo")]
	[Index(nameof(DelayFlag), Name = "IX_AsyAdoTotalDelayFinalAsakai_DelayFlag")]
	public class AsyAdoTotalDelayFinalAsakai : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxBodyNoLength = 10;

		public const int MaxLotNoLength = 20;

		public const int MaxColorLength = 10;

		public const int MaxTotalDelayLeadTimeLength = 10;

		public const int MaxCurrLocationLength = 255;

		public const int MaxLocationLength = 255;

		public const int MaxWDelayWithLeadTimeLength = 255;

		public const int MaxTDelayWithLeadTimeLength = 255;

		public const int MaxADelayWithLeadTimeLength = 255;

		public const int MaxIDelayWithLeadTimeLength = 255;

		public const int MaxFOutDelayLength = 255;

		public const int MaxDelayFlagLength = 1;

		[Required]
		[StringLength(MaxBodyNoLength)]
		public virtual string BodyNo { get; set; }

		[Required]
		[StringLength(MaxLotNoLength)]
		public virtual string LotNo { get; set; }

		[StringLength(MaxColorLength)]
		public virtual string Color { get; set; }

		[StringLength(MaxTotalDelayLeadTimeLength)]
		public virtual string TotalDelayLeadTime { get; set; }

		public virtual DateTime? DispatchPlanDatetime { get; set; }

		[StringLength(MaxCurrLocationLength)]
		public virtual string CurrLocation { get; set; }

		[StringLength(MaxLocationLength)]
		public virtual string Location { get; set; }

		[StringLength(MaxWDelayWithLeadTimeLength)]
		public virtual string WDelayWithLeadTime { get; set; }

		[StringLength(MaxTDelayWithLeadTimeLength)]
		public virtual string TDelayWithLeadTime { get; set; }

		[StringLength(MaxADelayWithLeadTimeLength)]
		public virtual string ADelayWithLeadTime { get; set; }

		[StringLength(MaxIDelayWithLeadTimeLength)]
		public virtual string IDelayWithLeadTime { get; set; }

		[StringLength(MaxFOutDelayLength)]
		public virtual string FOutDelay { get; set; }

		[StringLength(MaxDelayFlagLength)]
		public virtual string DelayFlag { get; set; }
	}

}

