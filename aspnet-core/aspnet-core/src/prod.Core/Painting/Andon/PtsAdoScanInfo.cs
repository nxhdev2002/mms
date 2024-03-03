using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Painting.Andon
{

	[Table("PtsAdoScanInfo")]
	[Index(nameof(ScanType), Name = "IX_PtsAdoScanInfo_ScanType")]
	[Index(nameof(ScanTime), Name = "IX_PtsAdoScanInfo_ScanTime")]
	public class PtsAdoScanInfo : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxScanTypeLength = 50;

		public const int MaxScanValueLength = 50;

		public const int MaxScanLocationLength = 50;

		public const int MaxScanByLength = 50;

		public const int MaxIsProcessedLength = 1;

		[Required]
		[StringLength(MaxScanTypeLength)]
		public virtual string ScanType { get; set; }

		[StringLength(MaxScanValueLength)]
		public virtual string ScanValue { get; set; }

		[StringLength(MaxScanLocationLength)]
		public virtual string ScanLocation { get; set; }

		[Required]
		public virtual DateTime? ScanTime { get; set; }

		[StringLength(MaxScanByLength)]
		public virtual string ScanBy { get; set; }

		[StringLength(MaxIsProcessedLength)]
		public virtual string IsProcessed { get; set; }
	}

}


