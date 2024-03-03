using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.LogW.Mwh
{

	[Table("LgwMwhContList")]
	[Index(nameof(ContainerNo), Name = "IX_LgwMwhContList_ContainerNo")]
	[Index(nameof(Renban), Name = "IX_LgwMwhContList_Renban")]
	[Index(nameof(SupplierNo), Name = "IX_LgwMwhContList_SupplierNo")]
	[Index(nameof(IsActive), Name = "IX_LgwMwhContList_IsActive")]
	public class LgwMwhContList : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxContainerNoLength = 50;

		public const int MaxRenbanLength = 50;

		public const int MaxSupplierNoLength = 50;

		public const int MaxStatusLength = 50;

		public const int MaxShopLength = 50;

		public const int MaxIsActiveLength = 50;

		[StringLength(MaxContainerNoLength)]
		public virtual string ContainerNo { get; set; }

		[StringLength(MaxRenbanLength)]
		public virtual string Renban { get; set; }

		[StringLength(MaxSupplierNoLength)]
		public virtual string SupplierNo { get; set; }

		[Column(TypeName = "date")]
		public virtual DateTime? DevanningDate { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? StartDevanningDate { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? FinishDevanningDate { get; set; }

		[StringLength(MaxStatusLength)]
		public virtual string Status { get; set; }

		public virtual int? ContScheduleId { get; set; }

		[StringLength(MaxShopLength)]
		public virtual string Shop { get; set; }

		[StringLength(MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

}


