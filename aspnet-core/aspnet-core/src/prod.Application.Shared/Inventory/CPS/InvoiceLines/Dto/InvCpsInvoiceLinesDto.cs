using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.CPS.Dto
{

    public class InvCpsInvoiceLinesDto : EntityDto<long?>
    {

        public virtual long? InvoiceId { get; set; }

        public virtual int? LineNum { get; set; }

        public virtual long? PoHeaderId { get; set; }

        public virtual string PoNumber { get; set; }

        public virtual long? VendorId { get; set; }

        public virtual long? ItemId { get; set; }

        public virtual string PartNo { get; set; }

        public virtual string ItemDescription { get; set; }

        public virtual decimal? Quantity { get; set; }

        public virtual decimal? QuantityOrder { get; set; }

        public virtual decimal? UnitPrice { get; set; }

        public virtual decimal? Amount { get; set; }

        public virtual decimal? AmountVat { get; set; }

        public virtual decimal? ForeignPrice { get; set; }

        public virtual decimal? TaxRate { get; set; }

        public virtual decimal? UnitpricePo { get; set; }

        public virtual string Note { get; set; }

        public virtual string IsActive { get; set; }

    }

    public class InvCpsInvoiceLinesDtoGrid : EntityDto<long?>
    {

        public virtual string PoNumber { get; set; }

        public virtual string PartNo { get; set; }

        public virtual string ItemDescription { get; set; }

        public virtual decimal? Quantity { get; set; }

        public virtual decimal? QuantityOrder { get; set; }

        public virtual decimal? UnitPrice { get; set; }

        public virtual decimal? Amount { get; set; }

        public virtual decimal? AmountVat { get; set; }

        public virtual decimal? TaxRate { get; set; }

        public virtual string Note { get; set; }

    }

    public class GetInvCpsInvoiceLinesInput : PagedAndSortedResultRequestDto
    {
        public virtual long? InvoiceId { get; set; }

    }

}


