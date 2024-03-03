using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;

namespace prod.Inv.Proc.Dto
{
    public class InvProductionMappingDto : EntityDto<long?>
    {
        public virtual decimal? PlanSequence { get; set; }

        public virtual string Shop { get; set; }

        public virtual string Model { get; set; }

        public virtual string LotNo { get; set; }

        public virtual string NoInLot { get; set; }

        public virtual string Grade { get; set; }

        public virtual string BodyNo { get; set; }

        public virtual DateTime? DateIn { get; set; }


        public virtual string TimeIn { get; set; }

        public virtual string UseLotNo { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual decimal? PartId { get; set; }


        public virtual decimal? Quantity { get; set; }

        public virtual decimal? Cost { get; set; }

        public virtual decimal? Cif { get; set; }

        public virtual decimal? Fob { get; set; }

        public virtual decimal? Freight { get; set; }

        public virtual decimal? Insurance { get; set; }

        public virtual decimal? Thc { get; set; }

        public virtual decimal? Tax { get; set; }

        public virtual decimal? InLand { get; set; }

        public virtual decimal? Amount { get; set; }

        public virtual decimal? PeriodId { get; set; }

        public virtual decimal? CostVn { get; set; }

        public virtual decimal? CifVn { get; set; }

        public virtual decimal? FobVn { get; set; }

        public virtual decimal? FreightVn { get; set; }

        public virtual decimal? InsuranceVn { get; set; }

        public virtual decimal? ThcVn { get; set; }

        public virtual decimal? TaxVn { get; set; }

        public virtual decimal? InlandVn { get; set; }

        public virtual decimal? AmountVn { get; set; }

        public virtual decimal? WipId { get; set; }

        public virtual decimal? InStockId { get; set; }

        public virtual decimal? MappingId { get; set; }

    }
    public class InvProductionMappingInput : PagedAndSortedResultRequestDto
    {
        public virtual decimal? PeriodId { get; set; }

        public virtual string Shop { get; set; }

        public virtual string BodyNo { get; set; }

        public virtual string LotNo { get; set; }

        public virtual DateTime? DateInFrom { get; set; }

        public virtual DateTime? DateInTo { get; set; }

    }

    public class GetInvProductionMappingHistoryInput : PagedAndSortedResultRequestDto
    {
        public virtual long Id { get; set; }
        public virtual string TableName { get; set; }
    }
    public class GetInvProductionMappingHistoryExcelInput
    {
        public virtual long Id { get; set; }
        public virtual string TableName { get; set; }
    }
}
