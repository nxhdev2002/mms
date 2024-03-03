using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Assy.Andon
{
	[Table("AsyAdoAInPlan")]
	[Index(nameof(LotNo), Name = "IX_AsyAdoAInPlan_LotNo")]
	[Index(nameof(NoInLot), Name = "IX_AsyAdoAInPlan_NoInLot")]
	[Index(nameof(BodyNo), Name = "IX_AsyAdoAInPlan_BodyNo")]
	[Index(nameof(SequenceNo), Name = "IX_AsyAdoAInPlan_SequenceNo")]
	public class AsyAdoAInPlan : FullAuditedEntity<long>, IEntity<long>
	{
        public const int MaxModelLength = 20;

        public const int MaxLotNoLength = 20;

        public const int MaxGradeLength = 255;

        public const int MaxBodyNoLength = 10;

        public const int MaxColorLength = 10;

        public const int MaxSequenceNoLength = 50;

        public const int MaxCfcLength = 4;

        public const int MaxVinNoLength = 18;

        public const int MaxEngineIdLength = 12;

        public const int MaxTransIdLength = 18;

        public const int MaxSalesSfxLength = 2;

        public const int MaxColorTypeLength = 4;

        public const int MaxIndentLineLength = 3;

        public const int MaxSsNoLength = 2;

        public const int MaxEdNumberLength = 5;

        public const int MaxGoshiCarLength = 1;


        [Required]
		[StringLength(MaxModelLength)]
		public virtual string Model { get; set; }

		[Required]
		[StringLength(MaxLotNoLength)]
		public virtual string LotNo { get; set; }

		[Required]
		public virtual int? NoInLot { get; set; }

		[StringLength(MaxGradeLength)]
		public virtual string Grade { get; set; }

		[Required]
		[StringLength(MaxBodyNoLength)]
		public virtual string BodyNo { get; set; }

		[StringLength(MaxColorLength)]
		public virtual string Color { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? AInDateActual { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? AOutDateActual { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? AInPlanDatetime { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? AOutPlanDatetime { get; set; }

		[StringLength(MaxSequenceNoLength)]
		public virtual string SequenceNo { get; set; }

		public virtual int? IsStart { get; set; }

        public virtual long? VehicleId { get; set; }

        [StringLength(MaxCfcLength)]
        public virtual string Cfc { get; set; }

        [StringLength(MaxVinNoLength)]
        public virtual string VinNo { get; set; }

        [StringLength(MaxEngineIdLength)]
        public virtual string EngineId { get; set; }

        [StringLength(MaxTransIdLength)]
        public virtual string TransId { get; set; }

        [StringLength(MaxSalesSfxLength)]
        public virtual string SalesSfx { get; set; }

        [StringLength(MaxColorTypeLength)]
        public virtual string ColorType { get; set; }

        [StringLength(MaxIndentLineLength)]
        public virtual string IndentLine { get; set; }

        [StringLength(MaxSsNoLength)]
        public virtual string SsNo { get; set; }

        [StringLength(MaxEdNumberLength)]
        public virtual string EdNumber { get; set; }

        [StringLength(MaxGoshiCarLength)]
        public virtual string GoshiCar { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? LineoffDatetime { get; set; }

        [Column(TypeName = "date")]
        public virtual DateTime? LineoffDate { get; set; }

        [Column(TypeName = "time(7)")]
        public virtual TimeSpan? LineoffTime { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? PdiDatetime { get; set; }

        [Column(TypeName = "date")]
        public virtual DateTime? PdiDate { get; set; }

        [Column(TypeName = "time(7)")]
        public virtual TimeSpan? PdiTime { get; set; }

        public virtual long? AssemblyId { get; set; }
    }

}

