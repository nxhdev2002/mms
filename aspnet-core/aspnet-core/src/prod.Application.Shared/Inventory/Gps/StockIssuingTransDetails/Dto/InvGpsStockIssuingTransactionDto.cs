using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace prod.Inventory.Gps.StockIssuingTransDetails.Dto
{
    public class InvGpsStockIssuingTransactionDto : EntityDto<long?>
    {
        public virtual string Guid { get; set; }
        public virtual string PoNo { get; set; }

        public virtual string PartNo { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string Uom { get; set; }

        public virtual string LotNo { get; set; }

        public virtual int? NoInLot { get; set; }

        public virtual Decimal? Qty { get; set; }

        public virtual string Vin { get; set; }

        public virtual string BodyNo { get; set; }

        public virtual string Color { get; set; }

        public virtual string PartName { get; set; }

        public virtual DateTime? WorkingDate { get; set; }

        public virtual string Puom { get; set; }

        public virtual int? CostCenter { get; set; }

        public virtual Decimal? GrandTotal { get; set; }

        public virtual string ErrorDescription { get; set; }

        public virtual long? CreatorUserId { get; set; }

        public virtual DateTime? TransactionDate { get; set; }

    }

    public class GetStockIssuingTransactionInput : PagedAndSortedResultRequestDto
    {
        public virtual string PartNo { get; set; }

        public virtual DateTime? WorkingDateFrom { get; set; }

        public virtual DateTime? WorkingDateTo { get; set; }

        //public virtual string SupplierNo { get; set; }

        //public virtual string Vin { get; set; }

        //public virtual string Cfc { get; set; }

        //public virtual string LotNo { get; set; }

        //public virtual int? NoInLot { get; set; }
               
    }

    public class GetStockIssuingTransactionExportInput
    {
        public virtual string PartNo { get; set; }

        public virtual DateTime? WorkingDateFrom { get; set; }

        public virtual DateTime? WorkingDateTo { get; set; }

        //public virtual string SupplierNo { get; set; }

        //public virtual string Vin { get; set; }

        //public virtual string Cfc { get; set; }

        //public virtual string LotNo { get; set; }

        //public virtual int? NoInLot { get; set; }

    }
}
