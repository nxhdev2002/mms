using Abp.Application.Services.Dto;
using System;

namespace prod.Inventory.Gps.StockReceivingTransDetails.Dto
{
    public class InvGpsStockReceivingTransactionDto : EntityDto<long?>
    {
        public virtual string Guid { get; set; }
        public virtual string PoNo { get; set; }

        public virtual string PartNo { get; set; }

        public virtual string PartName { get; set; }

        public virtual string Puom { get; set; }

        public virtual Decimal? Qty { get; set; }

        public virtual Decimal? GrandQty { get; set; }

        public virtual DateTime? WorkingDate { get; set; }

        public virtual DateTime? TransactionDate { get; set; }

        public virtual string ErrorDescription { get; set; }

        public virtual long? CreatorUserId { get; set; }

    }

    public class GetStockReceivingTransactionInput : PagedAndSortedResultRequestDto
    {
        public virtual string PartNo { get; set; }

        public virtual DateTime? WorkingDateFrom { get; set; }

        public virtual DateTime? WorkingDateTo { get; set; }

    }

    public class GetStockReceivingTransactionExportInput
    {
        public virtual string PartNo { get; set; }

        public virtual DateTime? WorkingDateFrom { get; set; }

        public virtual DateTime? WorkingDateTo { get; set; }

    }
}
