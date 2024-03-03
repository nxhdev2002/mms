using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inv.D125
{
	[Table("InvOutWipStock")]
	[Index(nameof(LotNo), Name = "IX_InvOutWipStock_LotNo")]
	[Index(nameof(PartNo), Name = "IX_InvOutWipStock_PartNo")]
	[Index(nameof(SumInland), Name = "IX_InvOutWipStock_SumInland")]
	[Index(nameof(Amount), Name = "IX_InvOutWipStock_Amount")]
	[Index(nameof(SumInlandVn), Name = "IX_InvOutWipStock_SumInlandVn")]
	[Index(nameof(AmountVn), Name = "IX_InvOutWipStock_AmountVn")]
	public class InvOutWipStock : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxLotNoLength = 50;

		public const int MaxPartNoLength = 50;

		public const int MaxCarfamilyCodeLength = 50;

		public const int MaxDcTypeLength = 50;

		public const int MaxInStockByLotLength = 50;

        public const int MaxCustomsDeclareNoLength = 50;
        

        public virtual decimal? PeriodId { get; set; }

		[StringLength(MaxLotNoLength)]
		public virtual string LotNo { get; set; }

		[StringLength(MaxPartNoLength)]
		public virtual string PartNo { get; set; }

		[StringLength(MaxCarfamilyCodeLength)]
		public virtual string CarfamilyCode { get; set; }

		public virtual decimal? UsageQty { get; set; }

		public virtual decimal? SumCif { get; set; }

		public virtual decimal? SumTax { get; set; }

		public virtual decimal? SumInland { get; set; }

		public virtual decimal? Amount { get; set; }

		public virtual decimal? SumCifVn { get; set; }

		public virtual decimal? SumTaxVn { get; set; }

		public virtual decimal? SumInlandVn { get; set; }

		public virtual decimal? AmountVn { get; set; }

        [StringLength(MaxCustomsDeclareNoLength)]
        public virtual string CustomsDeclareNo { get; set; }

        public virtual DateTime? DeclareDate { get; set; }

		[StringLength(MaxDcTypeLength)]
		public virtual string DcType { get; set; }

		[StringLength(MaxInStockByLotLength)]
		public virtual string InStockByLot { get; set; }
	}

}


