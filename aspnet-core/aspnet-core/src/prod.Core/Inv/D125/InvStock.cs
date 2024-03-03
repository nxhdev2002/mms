using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inv.D125
{
	[Table("InvStock")]
	[Index(nameof(PartNo), Name = "IX_InvStock_PartNo")]
	[Index(nameof(Source), Name = "IX_InvStock_Source")]
	[Index(nameof(DeclareDate), Name = "IX_InvStock_DeclareDate")]
	[Index(nameof(DcType), Name = "IX_InvStock_DcType")]
	[Index(nameof(Tax), Name = "IX_InvStock_Tax")]
	[Index(nameof(Inland), Name = "IX_InvStock_Inland")]
	[Index(nameof(InlandVn), Name = "IX_InvStock_InlandVn")]
	[Index(nameof(CostVn), Name = "IX_InvStock_CostVn")]
	public class InvStock : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxPartNoLength = 50;

		public const int MaxSourceLength = 50;

		public const int MaxCarFamilyCodeLength = 50;

		public const int MaxLotNoLength = 50;

		public const int MaxDcTypeLength = 50;

		public const int MaxInStockByLotLength = 50;

        public const int MaxCustomsDeclareNoLength = 50;

        


        public virtual decimal PeriodId { get; set; }

		[StringLength(MaxPartNoLength)]
		public virtual string PartNo { get; set; }

		[StringLength(MaxSourceLength)]
		public virtual string Source { get; set; }

		[StringLength(MaxCarFamilyCodeLength)]
		public virtual string CarFamilyCode { get; set; }

		[StringLength(MaxLotNoLength)]
		public virtual string LotNo { get; set; }

		public virtual decimal? Quantity { get; set; }

        [StringLength(MaxCustomsDeclareNoLength)]
        public virtual string CustomsDeclareNo { get; set; }

        public virtual DateTime? DeclareDate { get; set; }

		[StringLength(MaxDcTypeLength)]
		public virtual string DcType { get; set; }

		[StringLength(MaxInStockByLotLength)]
		public virtual string InStockByLot { get; set; }

		public virtual decimal? Cif { get; set; }

		public virtual decimal? Tax { get; set; }

		public virtual decimal? Inland { get; set; }

		public virtual decimal? Cost { get; set; }

		public virtual decimal? Amount { get; set; }

		public virtual decimal? CifVn { get; set; }

		public virtual decimal? TaxVn { get; set; }

		public virtual decimal? InlandVn { get; set; }

		public virtual decimal? CostVn { get; set; }

		public virtual decimal? AmountVn { get; set; }
	}

}

