using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Frame.Andon
{

	[Table("FrmAdoFrameProgress")]
	[Index(nameof(ScanningId), Name = "IX_FrmAdoFrameProgress_ScanningId")]
	[Index(nameof(BodyNo), Name = "IX_FrmAdoFrameProgress_BodyNo")]
	[Index(nameof(ProcessGroup), Name = "IX_FrmAdoFrameProgress_ProcessGroup")]
	[Index(nameof(NoInLot), Name = "IX_FrmAdoFrameProgress_NoInLot")]
	[Index(nameof(SequenceNo), Name = "IX_FrmAdoFrameProgress_SequenceNo")]
	[Index(nameof(Location), Name = "IX_FrmAdoFrameProgress_Location")]
	public class FrmAdoFrameProgress : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxBodyNoLength = 50;

		public const int MaxColorLength = 50;

		public const int MaxScanTypeCdLength = 255;

		public const int MaxScanLocationLength = 50;

		public const int MaxScanValueLength = 255;

		public const int MaxModeLength = 50;

		public const int MaxVinNoLength = 50;

		public const int MaxFrameNoLength = 50;

		public const int MaxModelLength = 50;

		public const int MaxGradeLength = 50;

		public const int MaxLotNoLength = 50;

		public const int MaxSequenceNoLength = 50;

		public const int MaxLocationLength = 50;

		public const int MaxAndonTransferLength = 1;

		public const int MaxRescanBodyNoLength = 50;

		public const int MaxRescanLotNoLength = 50;

		public const int MaxRescanModeLength = 50;

		public const int MaxErrorCdLength = 50;

		public const int MaxIsRescanLength = 1;

		public const int MaxIsActiveLength = 1;

		public virtual long? ScanningId { get; set; }

		[StringLength(MaxBodyNoLength)]
		public virtual string BodyNo { get; set; }

		[StringLength(MaxColorLength)]
		public virtual string Color { get; set; }

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

		[StringLength(MaxVinNoLength)]
		public virtual string VinNo { get; set; }

		[StringLength(MaxFrameNoLength)]
		public virtual string FrameNo { get; set; }

		[StringLength(MaxModelLength)]
		public virtual string Model { get; set; }

		[StringLength(MaxGradeLength)]
		public virtual string Grade { get; set; }

		[StringLength(MaxLotNoLength)]
		public virtual string LotNo { get; set; }

		public virtual int? NoInLot { get; set; }

		[StringLength(MaxSequenceNoLength)]
		public virtual string SequenceNo { get; set; }

		[StringLength(MaxLocationLength)]
		public virtual string Location { get; set; }

		[StringLength(MaxAndonTransferLength)]
		public virtual string AndonTransfer { get; set; }

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

		[StringLength(MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

}
