using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inv.D125
{
	[Table("InvOutLineOff")]
	[Index(nameof(PartNo), Name = "IX_InvOutLineOff_PartNo")]
	[Index(nameof(CarFamilyCode), Name = "IX_InvOutLineOff_CarFamilyCode")]
	[Index(nameof(SumInland), Name = "IX_InvOutLineOff_SumInland")]
	[Index(nameof(Amount), Name = "IX_InvOutLineOff_Amount")]
	[Index(nameof(SumTaxVn), Name = "IX_InvOutLineOff_SumTaxVn")]
	[Index(nameof(SumInlandVn), Name = "IX_InvOutLineOff_SumInlandVn")]
	public class InvOutLineOff : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxPartNoLength = 50;

		public const int MaxCarFamilyCodeLength = 50;

		public const int MaxDcTypeLength = 50;

		public const int MaxInStockByLotLength = 50;

        public const int MaxCustomsDeclareNoLength = 50;

        

        public virtual decimal? PeriodId { get; set; }

		[StringLength(MaxPartNoLength)]
		public virtual string PartNo { get; set; }

		[StringLength(MaxCarFamilyCodeLength)]
		public virtual string? CarFamilyCode { get; set; }

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


