using Abp.Application.Services.Dto;
using System;

namespace prod.Inventory.PIO.StockTransaction.Dto
{
    public class InvPIOStockTransactionDto : EntityDto<long?>
    {
        public virtual string PartNo { get; set; }

        public virtual string PartName { get; set; }

        public virtual long? PartId { get; set; }

        public virtual string MktCode { get; set; }

        public virtual DateTime? WorkingDate { get; set; }

        public virtual int? Qty { get; set; }

        public virtual long? TransId { get; set; }

        public virtual DateTime? TransDatetime { get; set; }

        public virtual DateTime? ScanDate { get; set; }

        public virtual long? VehicleId { get; set; }

        public virtual string VinNo { get; set; }

        public virtual string PartType { get; set; }

        public virtual string Shop { get; set; }

        public virtual string CarType { get; set; }

        public virtual string InteriorColor { get; set; }

        public virtual string ExtColor { get; set; }

        public virtual string IsActive { get; set; }
        public virtual string Route { get; set; }

        public virtual int? GrandQty { get; set; }

    }

    public class GetInvPIOStockTransactionInput : PagedAndSortedResultRequestDto
    {

        public virtual string PartNo { get; set; }

        public virtual string MktCode { get; set; }

        public virtual string VinNo { get; set; }

        public virtual DateTime? WorkingDateFrom { get; set; }

        public virtual DateTime? WorkingDateTo { get; set; }

    }

    public class GetInvPIOStockTransactionExportInput
    {
        public virtual string PartNo { get; set; }

        public virtual string MktCode { get; set; }

        public virtual string VinNo { get; set; }

        public virtual DateTime? WorkingDateFrom { get; set; }

        public virtual DateTime? WorkingDateTo { get; set; }

    }
}
