using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Painting.Andon
{

	[Table("PtsAdoPaintingProgress")]
	[Index(nameof(ScanningId), Name = "IX_PtsAdoPaintingProgress_ScanningId")]
	[Index(nameof(BodyNo), Name = "IX_PtsAdoPaintingProgress_BodyNo")]
	[Index(nameof(Color), Name = "IX_PtsAdoPaintingProgress_Color")]
	[Index(nameof(ProcessGroup), Name = "IX_PtsAdoPaintingProgress_ProcessGroup")]
	[Index(nameof(Model), Name = "IX_PtsAdoPaintingProgress_Model")]
	[Index(nameof(LotNo), Name = "IX_PtsAdoPaintingProgress_LotNo")]
	[Index(nameof(NoInLot), Name = "IX_PtsAdoPaintingProgress_NoInLot")]
	[Index(nameof(SequenceNo), Name = "IX_PtsAdoPaintingProgress_SequenceNo")]
	[Index(nameof(IsPaintOut), Name = "IX_PtsAdoPaintingProgress_IsPaintOut")]
	[Index(nameof(IsActive), Name = "IX_PtsAdoPaintingProgress_IsActive")]
	public class PtsAdoPaintingProgress : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxBodyNoLength = 50;

		public const int MaxColorLength = 50;

		public const int MaxColorOrgLength = 50;

		public const int MaxScanTypeCdLength = 255;

		public const int MaxScanLocationLength = 50;

		public const int MaxScanValueLength = 255;

		public const int MaxModeLength = 50;

		public const int MaxModelLength = 50;

		public const int MaxLotNoLength = 50;

		public const int MaxSequenceNoLength = 50;

		public const int MaxDefectDescLength = 200;

		public const int MaxTargetRepairLength = 100;

		public const int MaxLocationLength = 50;

		public const int MaxDuplicateLotLength = 100;

		public const int MaxWeldTransferLength = 1;

		public const int MaxRescanBodyNoLength = 50;

		public const int MaxRescanLotNoLength = 50;

		public const int MaxRescanModeLength = 50;

		public const int MaxErrorCdLength = 50;

		public const int MaxIsRescanLength = 1;

		public const int MaxIsPaintOutLength = 1;

		public const int MaxIsActiveLength = 1;

		public virtual long? ScanningId { get; set; }

		[StringLength(MaxBodyNoLength)]
		public virtual string BodyNo { get; set; }

		[StringLength(MaxColorLength)]
		public virtual string Color { get; set; }

		[StringLength(MaxColorOrgLength)]
		public virtual string ColorOrg { get; set; }

		[StringLength(MaxScanTypeCdLength)]
		public virtual string ScanTypeCd { get; set; }

		[StringLength(MaxScanLocationLength)]
		public virtual string ScanLocation { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? ScanTime { get; set; }

		[StringLength(MaxScanValueLength)]
		public virtual string ScanValue { get; set; }

		[StringLength(MaxModeLength)]
		public virtual string Mode { get; set; }

		public virtual int? ProcessGroup { get; set; }

		public virtual int? ProcessSubgroup { get; set; }

		public virtual int? ProcessSeq { get; set; }

		public virtual int? ConveyerStatus { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? LastConveyerRun { get; set; }

		public virtual int? TcStatus { get; set; }

		[StringLength(MaxModelLength)]
		public virtual string Model { get; set; }

		[StringLength(MaxLotNoLength)]
		public virtual string LotNo { get; set; }

		public virtual int? NoInLot { get; set; }

		[StringLength(MaxSequenceNoLength)]
		public virtual string SequenceNo { get; set; }

		[StringLength(MaxDefectDescLength)]
		public virtual string DefectDesc { get; set; }

		[StringLength(MaxTargetRepairLength)]
		public virtual string TargetRepair { get; set; }

		[StringLength(MaxLocationLength)]
		public virtual string Location { get; set; }

		[StringLength(MaxDuplicateLotLength)]
		public virtual string DuplicateLot { get; set; }

		[StringLength(MaxWeldTransferLength)]
		public virtual string WeldTransfer { get; set; }

		[StringLength(MaxRescanBodyNoLength)]
		public virtual string RescanBodyNo { get; set; }

		[StringLength(MaxRescanLotNoLength)]
		public virtual string RescanLotNo { get; set; }

		[StringLength(MaxRescanModeLength)]
		public virtual string RescanMode { get; set; }

		[StringLength(MaxErrorCdLength)]
		public virtual string ErrorCd { get; set; }

		[StringLength(MaxIsRescanLength)]
		public virtual string IsRescan { get; set; }

		[StringLength(MaxIsPaintOutLength)]
		public virtual string IsPaintOut { get; set; }

		[StringLength(MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

}


