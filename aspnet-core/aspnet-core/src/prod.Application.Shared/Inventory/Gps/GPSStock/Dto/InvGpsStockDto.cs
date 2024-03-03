using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.GPS.Dto
{

    public class InvGpsStockDto : EntityDto<long?>
    {

        public virtual string PartNo { get; set; }

        public virtual string PartName { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string Uom { get; set; }

        public virtual string LotNo { get; set; }

        public virtual int? NoInLot { get; set; }

        public virtual string VinNo { get; set; }

        public virtual string BodyNo { get; set; }

        public virtual string Color { get; set; }

        public virtual decimal? Qty { get; set; }

        public virtual DateTime? WorkingDate { get; set; }

        public virtual long? TransactionId { get; set; }

    }

    public class GetInvGpsStockInput : PagedAndSortedResultRequestDto
    {

        public virtual string PartNo { get; set; }

        public virtual DateTime? WorkingDateFrom { get; set; }
        public virtual DateTime? WorkingDateTo { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string VinNo { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string LotNo { get; set; }

        public virtual int? NoInLot { get; set; }

    }

}


