using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.LogW.Mwh
{

	[Table("LgwMwhCaseData")]
	[Index(nameof(CaseNo), Name = "IX_LgwMwhCaseData_CaseNo")]
	[Index(nameof(ContainerNo), Name = "IX_LgwMwhCaseData_ContainerNo")]
	[Index(nameof(Renban), Name = "IX_LgwMwhCaseData_Renban")]
	[Index(nameof(SupplierNo), Name = "IX_LgwMwhCaseData_SupplierNo")]
	[Index(nameof(CasePrefix), Name = "IX_LgwMwhCaseData_CasePrefix")]
	[Index(nameof(ContScheduleId), Name = "IX_LgwMwhCaseData_ContScheduleId")]
	[Index(nameof(Status), Name = "IX_LgwMwhCaseData_Status")]
	[Index(nameof(LocId), Name = "IX_LgwMwhCaseData_LocId")]
	[Index(nameof(IsBigpart), Name = "IX_LgwMwhCaseData_IsBigpart")]
	[Index(nameof(IsActive), Name = "IX_LgwMwhCaseData_IsActive")]
	public class LgwMwhCaseData : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxCaseNoLength = 50;

		public const int MaxLotNoLength = 50;

		public const int MaxGradeLength = 50;

		public const int MaxModelLength = 50;

		public const int MaxContainerNoLength = 50;

		public const int MaxRenbanLength = 50;

		public const int MaxSupplierNoLength = 50;

		public const int MaxOrderTypeLength = 50;

		public const int MaxCasePrefixLength = 50;

		public const int MaxProdLineLength = 50;

		public const int MaxStatusLength = 50;

		public const int MaxZoneCdLength = 50;

		public const int MaxAreaCdLength = 50;

		public const int MaxLocCdLength = 50;

		public const int MaxLocByLength = 50;

		public const int MaxShopLength = 50;

		public const int MaxIsBigpartLength = 50;

		public const int MaxIsActiveLength = 1;

		[StringLength(MaxCaseNoLength)]
		public virtual string CaseNo { get; set; }

		[StringLength(MaxLotNoLength)]
		public virtual string LotNo { get; set; }

		[StringLength(MaxGradeLength)]
		public virtual string Grade { get; set; }

		[StringLength(MaxModelLength)]
		public virtual string Model { get; set; }

		public virtual int? CaseQty { get; set; }

		[StringLength(MaxContainerNoLength)]
		public virtual string ContainerNo { get; set; }

		[StringLength(MaxRenbanLength)]
		public virtual string Renban { get; set; }

		[StringLength(MaxSupplierNoLength)]
		public virtual string SupplierNo { get; set; }

		[StringLength(MaxOrderTypeLength)]
		public virtual string OrderType { get; set; }

		[StringLength(MaxCasePrefixLength)]
		public virtual string CasePrefix { get; set; }

		[StringLength(MaxProdLineLength)]
		public virtual string ProdLine { get; set; }

		public virtual long? PxpCaseId { get; set; }

		public virtual long? ContScheduleId { get; set; }

		[StringLength(MaxStatusLength)]
		public virtual string Status { get; set; }

		[Column(TypeName = "date")]
		public virtual DateTime? DevanningDate { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? StartDevanningDate { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? FinishDevanningDate { get; set; }

		[StringLength(MaxZoneCdLength)]
		public virtual string ZoneCd { get; set; }

		[StringLength(MaxAreaCdLength)]
		public virtual string AreaCd { get; set; }

		public virtual long? LocId { get; set; }

		[StringLength(MaxLocCdLength)]
		public virtual string LocCd { get; set; }

		public virtual DateTime? LocDate { get; set; }

		[StringLength(MaxLocByLength)]
		public virtual string LocBy { get; set; }

		[StringLength(MaxShopLength)]
		public virtual string Shop { get; set; }

		[StringLength(MaxIsBigpartLength)]
		public virtual string IsBigpart { get; set; }

		[StringLength(MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

}

