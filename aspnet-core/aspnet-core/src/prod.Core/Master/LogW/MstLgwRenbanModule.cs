using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.LogW
{

	[Table("MstLgwRenbanModule")]
	[Index(nameof(Renban), Name = "IX_MstLgwRenbanModule_Renban")]
	[Index(nameof(CaseNo), Name = "IX_MstLgwRenbanModule_CaseNo")]
	[Index(nameof(SupplierNo), Name = "IX_MstLgwRenbanModule_SupplierNo")]
	[Index(nameof(CaseOrder), Name = "IX_MstLgwRenbanModule_CaseOrder")]
	[Index(nameof(IsActive), Name = "IX_MstLgwRenbanModule_IsActive")]
	public class MstLgwRenbanModule : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxRenbanLength = 50;

		public const int MaxCaseNoLength = 50;

		public const int MaxSupplierNoLength = 50;

		public const int MaxModuleTypeLength = 50;

		public const int MaxModuleSizeLength = 50;

		public const int MaxSortingTypeLength = 50;

		public const int MaxCaseTypeLength = 50;

		public const int MaxProdLineLength = 50;

		public const int MaxModelLength = 50;

		public const int MaxCfcLength = 50;

		public const int MaxWhLocLength = 50;

		public const int MaxIsUsePxpDataLength = 1;

		public const int MaxRemarkLength = 50;

		public const int MaxIsActiveLength = 1;

		[StringLength(MaxRenbanLength)]
		public virtual string Renban { get; set; }

		[StringLength(MaxCaseNoLength)]
		public virtual string CaseNo { get; set; }

		[StringLength(MaxSupplierNoLength)]
		public virtual string SupplierNo { get; set; }

		public virtual int? MinModule { get; set; }

		public virtual int? MaxModule { get; set; }

		public virtual int? ModuleCapacity { get; set; }

		[StringLength(MaxModuleTypeLength)]
		public virtual string ModuleType { get; set; }

		[StringLength(MaxModuleSizeLength)]
		public virtual string ModuleSize { get; set; }

		[StringLength(MaxSortingTypeLength)]
		public virtual string SortingType { get; set; }

		public virtual int? MinMod { get; set; }

		public virtual int? MaxMod { get; set; }

		public virtual int? MonitorVisualize { get; set; }

		public virtual int? CaseOrder { get; set; }

		[StringLength(MaxCaseTypeLength)]
		public virtual string CaseType { get; set; }

		[StringLength(MaxProdLineLength)]
		public virtual string ProdLine { get; set; }

		[StringLength(MaxModelLength)]
		public virtual string Model { get; set; }

		[StringLength(MaxCfcLength)]
		public virtual string Cfc { get; set; }

		[StringLength(MaxWhLocLength)]
		public virtual string WhLoc { get; set; }

		[StringLength(MaxIsUsePxpDataLength)]
		public virtual string IsUsePxpData { get; set; }

		public virtual int? UpLeadtime { get; set; }

		[StringLength(MaxRemarkLength)]
		public virtual string Remark { get; set; }

		[StringLength(MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

}

