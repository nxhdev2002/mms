﻿using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inv.D125.Dto
{
	public class InvOutWipStockDto : EntityDto<long?>
	{
		public virtual decimal? PeriodId { get; set; }

		public virtual string LotNo { get; set; }

		public virtual string PartNo { get; set; }

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

		public virtual string CustomsDeclareNo { get; set; }

		public virtual DateTime? DeclareDate { get; set; }

		public virtual string DcType { get; set; }

		public virtual string InStockByLot { get; set; }

	}
	
	public class GetInvOutWipStockInput : PagedAndSortedResultRequestDto
	{
		public virtual decimal? PeriodId { get; set; }

		public virtual string LotNo { get; set; }

		public virtual string PartNo { get; set; }

		public virtual string CarfamilyCode { get; set; }

		public virtual string InStockByLot { get; set; }

	}

    public class GetInvOutWipStockExportInput
    {
        public virtual decimal? PeriodId { get; set; }

        public virtual string LotNo { get; set; }

        public virtual string PartNo { get; set; }

        public virtual string CarfamilyCode { get; set; }

        public virtual string InStockByLot { get; set; }

    }

}


