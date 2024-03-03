using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Painting.Andon
{

	[Table("PtsAdoPaintingData")]
	[Index(nameof(LifetimeNo), Name = "IX_PtsAdoPaintingData_LifetimeNo")]
	[Index(nameof(Model), Name = "IX_PtsAdoPaintingData_Model")]
	[Index(nameof(Cfc), Name = "IX_PtsAdoPaintingData_Cfc")]
	[Index(nameof(ProcessSeq), Name = "IX_PtsAdoPaintingData_ProcessSeq")]
	[Index(nameof(WorkingDate), Name = "IX_PtsAdoPaintingData_WorkingDate")]
	[Index(nameof(Shift), Name = "IX_PtsAdoPaintingData_Shift")]
	[Index(nameof(NoInDate), Name = "IX_PtsAdoPaintingData_NoInDate")]
	[Index(nameof(ProcessCode), Name = "IX_PtsAdoPaintingData_ProcessCode")]
	[Index(nameof(IsActive), Name = "IX_PtsAdoPaintingData_IsActive")]
	public class PtsAdoPaintingData : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxModelLength = 10;

		public const int MaxCfcLength = 10;

		public const int MaxGradeLength = 10;

		public const int MaxLotNoLength = 10;

		public const int MaxBodyNoLength = 10;

		public const int MaxColorLength = 10;

		public const int MaxProdLineLength = 10;

		public const int MaxShiftLength = 10;

		public const int MaxProcessCodeLength = 40;

		public const int MaxInfoProcessLength = 20;

		public const int MaxIsProjectLength = 1;

		public const int MaxIsActiveLength = 1;

		public virtual long? LifetimeNo { get; set; }

		[StringLength(MaxModelLength)]
		public virtual string Model { get; set; }

		[StringLength(MaxCfcLength)]
		public virtual string Cfc { get; set; }

		[StringLength(MaxGradeLength)]
		public virtual string Grade { get; set; }

		[StringLength(MaxLotNoLength)]
		public virtual string LotNo { get; set; }

		public virtual int? NoInLot { get; set; }

		[StringLength(MaxBodyNoLength)]
		public virtual string BodyNo { get; set; }

		[StringLength(MaxColorLength)]
		public virtual string Color { get; set; }

		[StringLength(MaxProdLineLength)]
		public virtual string ProdLine { get; set; }

		public virtual int? ProcessGroup { get; set; }

		public virtual int? SubGroup { get; set; }

		public virtual int? ProcessSeq { get; set; }

		public virtual int? Filler { get; set; }

		[Column(TypeName = "date")]
		public virtual DateTime? WorkingDate { get; set; }

		[StringLength(MaxShiftLength)]
		public virtual string Shift { get; set; }

		public virtual int? NoInDate { get; set; }

		[StringLength(MaxProcessCodeLength)]
		public virtual string ProcessCode { get; set; }

		[StringLength(MaxInfoProcessLength)]
		public virtual string InfoProcess { get; set; }

		public virtual int? InfoProcessNo { get; set; }

		[StringLength(MaxIsProjectLength)]
		public virtual string IsProject { get; set; }

		[StringLength(MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

}