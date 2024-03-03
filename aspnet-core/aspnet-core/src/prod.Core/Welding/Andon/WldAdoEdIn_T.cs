using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Welding.Andon
{

	[Table("WldAdoEdIn_T")]
	[Index(nameof(Model), Name = "IX_WldAdoEdIn_T_Model")]
	[Index(nameof(RequestDate), Name = "IX_WldAdoEdIn_T_RequestDate")]
	public class WldAdoEdIn_T : FullAuditedEntity<long>, IEntity<long>
	{
        public const int MaxGuidLength = 128;

        public const int MaxModelLength = 10;

		public const int MaxLotNoLength = 10;

		public const int MaxBodyNoLength = 10;

		public const int MaxColorLength = 10;

		public const int MaxPlanTimeLength = 10;

		public const int MaxShiftLength = 20;

        public const int MaxRemarksLength = 255;

        public const int MaxProdLineLength = 50;

        public const int MaxVinLength = 100;


        [StringLength(MaxGuidLength)]
        public virtual string Guid { get; set; }

        [Required]
        public virtual int? No { get; set; }

        [Required]
        public virtual int? Seq { get; set; }

		[StringLength(MaxModelLength)]
		public virtual string Model { get; set; }

		[Required]
		[StringLength(MaxLotNoLength)]
		public virtual string LotNo { get; set; }

		[Required]
		public virtual int? NoInLot { get; set; }

        [Required]
        [StringLength(MaxBodyNoLength)]
        public virtual string BodyNo { get; set; }

        [StringLength(MaxColorLength)]
        public virtual string Color { get; set; }

        [StringLength(MaxVinLength)]
        public virtual string Vin { get; set; }

        [Column(TypeName = "date")]
        public virtual DateTime? RequestDate { get; set; }

        [StringLength(MaxProdLineLength)]
        public virtual string ProdLine { get; set; }

        [StringLength(MaxPlanTimeLength)]
        public virtual string PlanTime { get; set; }

        [StringLength(MaxShiftLength)]
        public virtual string Shift { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? EdIn { get; set; }

        [StringLength(MaxRemarksLength)]
		public virtual string Remarks { get; set; }
		
	}

}

