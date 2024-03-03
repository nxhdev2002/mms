using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.LogW.Dvn
{

	[Table("LgwDvnContList")]
	[Index(nameof(ContainerNo), Name = "IX_LgwDvnContList_ContainerNo")]
	[Index(nameof(Renban), Name = "IX_LgwDvnContList_Renban")]
	[Index(nameof(SupplierNo), Name = "IX_LgwDvnContList_SupplierNo")]
	[Index(nameof(IsActive), Name = "IX_LgwDvnContList_IsActive")]
	public class LgwDvnContList : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxContainerNoLength = 50;

		public const int MaxRenbanLength = 50;

		public const int MaxSupplierNoLength = 50;

		public const int MaxLotNoLength = 20;

		public const int MaxShiftNoLength = 10;

		public const int MaxDevanningDockLength = 10;

		public const int MaxDevanningTypeLength = 10;

		public const int MaxStatusLength = 50;

		public const int MaxSortingStatusLength = 10;

		public const int MaxIsActiveLength = 1;

		[StringLength(MaxContainerNoLength)]
		public virtual string ContainerNo { get; set; }

		[StringLength(MaxRenbanLength)]
		public virtual string Renban { get; set; }

		[StringLength(MaxSupplierNoLength)]
		public virtual string SupplierNo { get; set; }

		[StringLength(MaxLotNoLength)]
		public virtual string LotNo { get; set; }

		[Column(TypeName = "date")]
		public virtual DateTime? WorkingDate { get; set; }

		[StringLength(MaxShiftNoLength)]
		public virtual string ShiftNo { get; set; }

		[StringLength(MaxDevanningDockLength)]
		public virtual string DevanningDock { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? PlanDevanningDate { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? ActDevanningDate { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? ActDevanningDateFinished { get; set; }

		[StringLength(MaxDevanningTypeLength)]
		public virtual string DevanningType { get; set; }

		[StringLength(MaxStatusLength)]
		public virtual string Status { get; set; }

		public virtual int? DevLeadtime { get; set; }

		public virtual int? PlanDevanningLineOff { get; set; }

		[StringLength(MaxSortingStatusLength)]
		public virtual string SortingStatus { get; set; }

		[StringLength(MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

}

