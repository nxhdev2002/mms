using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.LogW.Pik
{

	[Table("LgwPikPickingProgress")]
	[Index(nameof(PickingTabletId), Name = "IX_LgwPikPickingProgress_PickingTabletId")]
	[Index(nameof(ProdLine), Name = "IX_LgwPikPickingProgress_ProdLine")]
	[Index(nameof(ProcessCode), Name = "IX_LgwPikPickingProgress_ProcessCode")]
	[Index(nameof(WorkingDate), Name = "IX_LgwPikPickingProgress_WorkingDate")]
	[Index(nameof(Shift), Name = "IX_LgwPikPickingProgress_Shift")]
	[Index(nameof(IsActive), Name = "IX_LgwPikPickingProgress_IsActive")]
	public class LgwPikPickingProgress : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxPickingTabletIdLength = 50;

		public const int MaxProdLineLength = 50;

		public const int MaxProcessCodeLength = 50;

		public const int MaxProcessGroupLength = 50;

		public const int MaxShiftLength = 50;

		public const int MaxLotNoLength = 50;

		public const int MaxBodyNoLength = 50;

		public const int MaxIsActiveLength = 50;

		[StringLength(MaxPickingTabletIdLength)]
		public virtual string PickingTabletId { get; set; }

		[StringLength(MaxProdLineLength)]
		public virtual string ProdLine { get; set; }

		[StringLength(MaxProcessCodeLength)]
		public virtual string ProcessCode { get; set; }

		[StringLength(MaxProcessGroupLength)]
		public virtual string ProcessGroup { get; set; }

		public virtual int? SeqNo { get; set; }

		[Column(TypeName = "date")]
		public virtual DateTime? WorkingDate { get; set; }

		[StringLength(MaxShiftLength)]
		public virtual string Shift { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? TaktStartTime { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? StartTime { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? FinishTime { get; set; }

		[StringLength(MaxLotNoLength)]
		public virtual string LotNo { get; set; }

		[StringLength(MaxBodyNoLength)]
		public virtual string BodyNo { get; set; }

		[StringLength(MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

}

