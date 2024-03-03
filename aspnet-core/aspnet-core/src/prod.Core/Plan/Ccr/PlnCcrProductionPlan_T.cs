using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Plan.Ccr
{

	[Table("PlnCcrProductionPlan_T")]
	[Index(nameof(Shop), Name = "IX_PlnCcrProductionPlan_T_Shop")]
	[Index(nameof(LotNo), Name = "IX_PlnCcrProductionPlan_T_LotNo")]
	[Index(nameof(NoInLot), Name = "IX_PlnCcrProductionPlan_T_NoInLot")]
	public class PlnCcrProductionPlan_T : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxGuidLength = 128;

		public const int MaxShopLength = 10;

		public const int MaxModelLength = 1;

		public const int MaxLotNoLength = 50;

		public const int MaxNoInLotLength = 50;

		public const int MaxGradeLength = 2;

		public const int MaxBodyLength = 50;

		public const int MaxSupplierNoLength = 50;

		public const int MaxUseLotNoLength = 50;

		public const int MaxSupplierNo2Length = 50;

		public const int MaxUseLotNo2Length = 50;

		public const int MaxUseNoInLotLength = 50;


		[StringLength(MaxGuidLength)]
		public virtual string Guid { get; set; }

		public virtual long? PlanSequence { get; set; }

		[StringLength(MaxShopLength)]
		public virtual string Shop { get; set; }

		[StringLength(MaxModelLength)]
		public virtual string Model { get; set; }

		[StringLength(MaxLotNoLength)]
		public virtual string LotNo { get; set; }

		[StringLength(MaxNoInLotLength)]
		public virtual string NoInLot { get; set; }

		[StringLength(MaxGradeLength)]
		public virtual string Grade { get; set; }

		[StringLength(MaxBodyLength)]
		public virtual string Body { get; set; }

		[Column(TypeName = "date")]
		public virtual DateTime? DateIn { get; set; }

		[Column(TypeName = "time(7)")]
		public virtual TimeSpan? TimeIn { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? DateTimeIn { get; set; }

		[StringLength(MaxSupplierNoLength)]
		public virtual string SupplierNo { get; set; }

		[StringLength(MaxUseLotNoLength)]
		public virtual string UseLotNo { get; set; }

		[StringLength(MaxSupplierNo2Length)]
		public virtual string SupplierNo2 { get; set; }

		[StringLength(MaxUseLotNo2Length)]
		public virtual string UseLotNo2 { get; set; }

		[StringLength(MaxUseNoInLotLength)]
		public virtual string UseNoInLot { get; set; }
	}

}

