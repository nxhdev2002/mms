using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.LogW.Mwh
{

	[Table("LgwMwhModuleCalling")]
	[Index(nameof(Renban), Name = "IX_LgwMwhModuleCalling_Renban")]
	[Index(nameof(CaseNo), Name = "IX_LgwMwhModuleCalling_CaseNo")]
	[Index(nameof(SupplierNo), Name = "IX_LgwMwhModuleCalling_SupplierNo")]
	[Index(nameof(IsActive), Name = "IX_LgwMwhModuleCalling_IsActive")]
	public class LgwMwhModuleCalling : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxRenbanLength = 50;

		public const int MaxCaseNoLength = 50;

		public const int MaxSupplierNoLength = 50;

		public const int MaxCalledModuleNoLength = 50;

		public const int MaxCaseTypeLength = 50;

		public const int MaxIsActiveLength = 1;

		[StringLength(MaxRenbanLength)]
		public virtual string Renban { get; set; }

		[StringLength(MaxCaseNoLength)]
		public virtual string CaseNo { get; set; }

		[StringLength(MaxSupplierNoLength)]
		public virtual string SupplierNo { get; set; }

		public virtual DateTime? CallTime { get; set; }

		[StringLength(MaxCalledModuleNoLength)]
		public virtual string CalledModuleNo { get; set; }

		[StringLength(MaxCaseTypeLength)]
		public virtual string CaseType { get; set; }

		[StringLength(MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

}

