using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Painting.Andon
{

	[Table("PtsAdoTotalDelay")]
	[Index(nameof(WipId), Name = "IX_PtsAdoTotalDelay_WipId")]
	[Index(nameof(ProgressId), Name = "IX_PtsAdoTotalDelay_ProgressId")]
	[Index(nameof(BodyNo), Name = "IX_PtsAdoTotalDelay_BodyNo")]
	[Index(nameof(LotNo), Name = "IX_PtsAdoTotalDelay_LotNo")]
	[Index(nameof(IsActive), Name = "IX_PtsAdoTotalDelay_IsActive")]
	public class PtsAdoTotalDelay : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxBodyNoLength = 50;

		public const int MaxLotNoLength = 50;

		public const int MaxColorLength = 50;

		public const int MaxModeLength = 50;

		public const int MaxTargetRepairLength = 100;

		public const int MaxLocationLength = 100;

		public const int MaxIsActiveLength = 1;

		public virtual long? WipId { get; set; }

		public virtual long? ProgressId { get; set; }

		[StringLength(MaxBodyNoLength)]
		public virtual string BodyNo { get; set; }

		[StringLength(MaxLotNoLength)]
		public virtual string LotNo { get; set; }

		[StringLength(MaxColorLength)]
		public virtual string Color { get; set; }

		[StringLength(MaxModeLength)]
		public virtual string Mode { get; set; }

		[StringLength(MaxTargetRepairLength)]
		public virtual string TargetRepair { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? StartRepair { get; set; }

		[StringLength(MaxLocationLength)]
		public virtual string Location { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? AInPlanDate { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? EdInAct { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? RepairIn { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? Leadtime { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? LeadtimePlus { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? Etd { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? RecoatIn { get; set; }

		[StringLength(MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

}


