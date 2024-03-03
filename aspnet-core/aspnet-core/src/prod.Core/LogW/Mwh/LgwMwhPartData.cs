using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.LogW.Mwh
{

	[Table("LgwMwhPartData")]
	[Index(nameof(PxppartId), Name = "IX_LgwMwhPartData_PxppartId")]
	[Index(nameof(PartNo), Name = "IX_LgwMwhPartData_PartNo")]
	[Index(nameof(CaseNo), Name = "IX_LgwMwhPartData_CaseNo")]
	[Index(nameof(ModuleNo), Name = "IX_LgwMwhPartData_ModuleNo")]
	[Index(nameof(ContainerNo), Name = "IX_LgwMwhPartData_ContainerNo")]
	[Index(nameof(SupplierNo), Name = "IX_LgwMwhPartData_SupplierNo")]
	[Index(nameof(PxpcaseId), Name = "IX_LgwMwhPartData_PxpcaseId")]
	[Index(nameof(OrderType), Name = "IX_LgwMwhPartData_OrderType")]
	[Index(nameof(IsActive), Name = "IX_LgwMwhPartData_IsActive")]
	public class LgwMwhPartData : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxPartNoLength = 12;

		public const int MaxLotNoLength = 10;

		public const int MaxFixlotLength = 4;

		public const int MaxCaseNoLength = 30;

		public const int MaxModuleNoLength = 30;

		public const int MaxContainerNoLength = 15;

		public const int MaxSupplierNoLength = 10;

		public const int MaxPartNameLength = 300;

		public const int MaxCarfamilyCodeLength = 4;

		public const int MaxOrderTypeLength = 50;

		public const int MaxIsActiveLength = 1;

		[Required]
		public virtual long? PxppartId { get; set; }

		[StringLength(MaxPartNoLength)]
		public virtual string PartNo { get; set; }

		[StringLength(MaxLotNoLength)]
		public virtual string LotNo { get; set; }

		[StringLength(MaxFixlotLength)]
		public virtual string Fixlot { get; set; }

		[StringLength(MaxCaseNoLength)]
		public virtual string CaseNo { get; set; }

		[StringLength(MaxModuleNoLength)]
		public virtual string ModuleNo { get; set; }

		[StringLength(MaxContainerNoLength)]
		public virtual string ContainerNo { get; set; }

		[StringLength(MaxSupplierNoLength)]
		public virtual string SupplierNo { get; set; }

		public virtual int? UsageQty { get; set; }

		[StringLength(MaxPartNameLength)]
		public virtual string PartName { get; set; }

		[StringLength(MaxCarfamilyCodeLength)]
		public virtual string CarfamilyCode { get; set; }

		public virtual long? InvoiceParentId { get; set; }

		[Required]
		public virtual long? PxpcaseId { get; set; }

		[StringLength(MaxOrderTypeLength)]
		public virtual string OrderType { get; set; }

		[StringLength(MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

}

