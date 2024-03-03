using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.LogW.Lup
{

	[Table("LgwLupContModule")]
	[Index(nameof(InvoiceNo), Name = "IX_LgwLupContModule_InvoiceNo")]
	[Index(nameof(SupplierNo), Name = "IX_LgwLupContModule_SupplierNo")]
	[Index(nameof(ContainerNo), Name = "IX_LgwLupContModule_ContainerNo")]
	[Index(nameof(Renban), Name = "IX_LgwLupContModule_Renban")]
	[Index(nameof(IsActive), Name = "IX_LgwLupContModule_IsActive")]
	public class LgwLupContModule : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxInvoiceNoLength = 50;

		public const int MaxSupplierNoLength = 50;

		public const int MaxContainerNoLength = 50;

		public const int MaxRenbanLength = 50;

		public const int MaxLotNoLength = 20;

		public const int MaxModuleNoLength = 20;

		public const int MaxStatusLength = 10;

		public const int MaxSortingTypeLength = 20;

		public const int MaxSortingStatusLength = 10;

		public const int MaxIsActiveLength = 1;

		[StringLength(MaxInvoiceNoLength)]
		public virtual string InvoiceNo { get; set; }

		[StringLength(MaxSupplierNoLength)]
		public virtual string SupplierNo { get; set; }

		[StringLength(MaxContainerNoLength)]
		public virtual string ContainerNo { get; set; }

		[StringLength(MaxRenbanLength)]
		public virtual string Renban { get; set; }

		[StringLength(MaxLotNoLength)]
		public virtual string LotNo { get; set; }

		[StringLength(MaxModuleNoLength)]
		public virtual string ModuleNo { get; set; }

		[StringLength(MaxStatusLength)]
		public virtual string Status { get; set; }

		[StringLength(MaxSortingTypeLength)]
		public virtual string SortingType { get; set; }

		[StringLength(MaxSortingStatusLength)]
		public virtual string SortingStatus { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? UpdatedSortingStatus { get; set; }

		[StringLength(MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

}

