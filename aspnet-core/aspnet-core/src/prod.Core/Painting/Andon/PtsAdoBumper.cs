using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Painting.Andon
{

	[Table("PtsAdoBumper")]
	[Index(nameof(WipId), Name = "IX_PtsAdoBumper_WipId")]
	[Index(nameof(BodyNo), Name = "IX_PtsAdoBumper_BodyNo")]
	[Index(nameof(LotNo), Name = "IX_PtsAdoBumper_LotNo")]
	[Index(nameof(NoInLot), Name = "IX_PtsAdoBumper_NoInLot")]
	[Index(nameof(BumperStatus), Name = "IX_PtsAdoBumper_BumperStatus")]
	public class PtsAdoBumper : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxModelLength = 50;

		public const int MaxGradeLength = 50;

		public const int MaxBodyNoLength = 50;

		public const int MaxLotNoLength = 50;

		public const int MaxColorLength = 50;

		public const int MaxIsActiveLength = 1;

		public virtual long? WipId { get; set; }

		[StringLength(MaxModelLength)]
		public virtual string Model { get; set; }

		[StringLength(MaxGradeLength)]
		public virtual string Grade { get; set; }

		[StringLength(MaxBodyNoLength)]
		public virtual string BodyNo { get; set; }

		[StringLength(MaxLotNoLength)]
		public virtual string LotNo { get; set; }

		public virtual int? NoInLot { get; set; }

		[StringLength(MaxColorLength)]
		public virtual string Color { get; set; }

		public virtual int? BumperStatus { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? InitialDate { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? I1Date { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? I2Date { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? InlInDate { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? InlOutDate { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? PreparationDate { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? UnpackingDate { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? JigSettingDate { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? BoothDate { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? FinishedDate { get; set; }

		public virtual int? ExtSeq { get; set; }

		public virtual int? UnpSeq { get; set; }

		[StringLength(MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

}
