using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Welding.Andon
{

	[Table("WldAdoWeldingPlan")]
	[Index(nameof(Model), Name = "IX_WldAdoWeldingPlan_Model")]
	[Index(nameof(RequestDate), Name = "IX_WldAdoWeldingPlan_RequestDate")]
	public class WldAdoWeldingPlan : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxModelLength = 20;

		public const int MaxLotNoLength = 10;

		public const int MaxBodyNoLength = 10;

		public const int MaxColorLength = 20;

		public const int MaxPlanTimeLength = 10;

		public const int MaxShiftLength = 20;

        public const int MaxGradeLength = 10;

        public const int MaxProdLineLength = 50;

        public const int MaxVinNoLength = 100;

        public const int MaxCfcLength = 4;


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

        [StringLength(MaxProdLineLength)]
        public virtual string ProdLine { get; set; }

        [Required]
        [StringLength(MaxBodyNoLength)]
        public virtual string BodyNo { get; set; }

        [StringLength(MaxVinNoLength)]
        public virtual string VinNo { get; set; }

        [StringLength(MaxColorLength)]
		public virtual string Color { get; set; }

		[StringLength(MaxPlanTimeLength)]
		public virtual string PlanTime { get; set; }

		[Column(TypeName = "date")]
		public virtual DateTime? RequestDate { get; set; }

		[StringLength(MaxShiftLength)]
		public virtual string Shift { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? WInDate { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? WOutDate { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? EdIn { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? TInPlanDatetime { get; set; }

        public virtual long? VehicleId { get; set; }

        [StringLength(MaxCfcLength)]
        public virtual string Cfc { get; set; }

        public virtual long? WeldingId { get; set; }

        public virtual long? AssemblyId { get; set; }
    }

}

