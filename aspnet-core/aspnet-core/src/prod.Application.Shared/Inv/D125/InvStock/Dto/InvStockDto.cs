using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;

namespace prod.Inv.D125.Dto
{
    public class InvStockDto : EntityDto<long?>
    {
        public virtual decimal? PeriodId { get; set; }

        public virtual string PartNo { get; set; }

        public virtual string Source { get; set; }

        public virtual string CarFamilyCode { get; set; }

        public virtual string LotNo { get; set; }

        public virtual decimal? Quantity { get; set; }

        public virtual string CustomsDeclareNo { get; set; }

        public virtual DateTime? DeclareDate { get; set; }

        public virtual string DcType { get; set; }

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

    public class GetInvStockInput : PagedAndSortedResultRequestDto
    {

        public virtual decimal? PeriodId { get; set; }

        public virtual string PartNo { get; set; }

        public virtual string Source { get; set; }

        public virtual string CarFamilyCode { get; set; }

        public virtual string LotNo { get; set; }

        public virtual string InStockByLot { get; set; }

    }
    public class GetInvStockẼportInput
    {

        public virtual decimal? PeriodId { get; set; }

        public virtual string PartNo { get; set; }

        public virtual string Source { get; set; }

        public virtual string CarFamilyCode { get; set; }

        public virtual string LotNo { get; set; }

        public virtual string InStockByLot { get; set; }

    }

    public class InvPeriodDto : EntityDto<long?>
    {
        public virtual string Description { get; set; }

        public virtual DateTime? From_Date { get; set; }

        public virtual DateTime? To_Date { get; set; }

        public virtual string Status { get; set; }
    }
}


