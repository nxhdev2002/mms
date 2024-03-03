using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.LogW
{

	[Table("MstLgwPickingTabletProcess")]
	[Index(nameof(PickingTabletId), Name = "IX_MstLgwPickingTabletProcess_PickingTabletId")]
	[Index(nameof(PickingPosition), Name = "IX_MstLgwPickingTabletProcess_PickingPosition")]
	[Index(nameof(Process), Name = "IX_MstLgwPickingTabletProcess_Process")]
	[Index(nameof(IsActive), Name = "IX_MstLgwPickingTabletProcess_IsActive")]
	public class MstLgwPickingTabletProcess : FullAuditedEntity<long>, IEntity<long>
	{
		public const int MaxPickingTabletIdLength = 50;

		public const int MaxPickingPositionLength = 50;

		public const int MaxProcessLength = 50;

		public const int MaxLogicSequenceLength = 50;

		public const int MaxHasModelLength = 1;

		public const int MaxIsLotSupplyLength = 1;

		public const int MaxIsActiveLength = 1;

		[StringLength(MaxPickingTabletIdLength)]
		public virtual string PickingTabletId { get; set; }

		[StringLength(MaxPickingPositionLength)]
		public virtual string PickingPosition { get; set; }

		[StringLength(MaxProcessLength)]
		public virtual string Process { get; set; }

		public virtual int? PickingCycle { get; set; }

		public virtual int? LogicSequenceNo { get; set; }

		[StringLength(MaxLogicSequenceLength)]
		public virtual string LogicSequence { get; set; }

		[StringLength(MaxHasModelLength)]
		public virtual string HasModel { get; set; }

		[StringLength(MaxIsLotSupplyLength)]
		public virtual string IsLotSupply { get; set; }

		[StringLength(MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

}

