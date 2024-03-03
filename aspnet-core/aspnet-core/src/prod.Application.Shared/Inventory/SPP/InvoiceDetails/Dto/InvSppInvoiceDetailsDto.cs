using Abp.Application.Services.Dto;
using System;

namespace prod.Inventory.SPP.InvoiceDetails.Dto
{
    public class InvSppInvoiceDetailsDto : EntityDto<long?>
    {

        public virtual long? InvoiceId { get; set; }

        public virtual string InvoiceNo { get; set; }

        public virtual string PartNo { get; set; }

        public virtual string Supplier { get; set; }

        public virtual decimal? Fob { get; set; }

        public virtual decimal? Freight { get; set; }

        public virtual decimal? Insurance { get; set; }

        public virtual decimal? Lc { get; set; }

        public virtual decimal? Cif { get; set; }

        public virtual decimal? Vat { get; set; }

        public virtual decimal? InLand { get; set; }

        public virtual decimal? Tax { get; set; }

        public virtual int? InvoiceQty { get; set; }

        public virtual int? ReceicedQty { get; set; }

        public virtual int? RejectQty { get; set; }

        public virtual long? PartId { get; set; }

        public virtual string Type { get; set; }

        public virtual string CaseNo { get; set; }

        public virtual int? Month { get; set; }

        public virtual int? Year { get; set; }

        public virtual string Stock { get; set; }

        public virtual decimal? FobVn { get; set; }

        public virtual string PONo { get; set; }

        public virtual decimal? LcVn { get; set; }

        public virtual decimal? CifVn { get; set; }

        public virtual decimal? VatVn { get; set; }

        public virtual decimal? InlandVn { get; set; }

        public virtual decimal? TaxVn { get; set; }

        public virtual string ItemNo { get; set; }

        public virtual decimal? FreightVn { get; set; }

        public virtual decimal? InsuranceVn { get; set; }

        public virtual DateTime? ReceiveDate { get; set; }

        public virtual DateTime? ProcessDate { get; set; }

        public virtual string CustomerNo { get; set; }

        public virtual int? IsInternal { get; set; }
    }

    public class GetInvSppInvoiceDetailsInput : PagedAndSortedResultRequestDto
    {
        public virtual string PartNo { get; set; }
        public virtual string InvoiceNo { get; set; }
        public virtual string Stock { get; set; }
        public virtual string Supplier { get; set; }
        public virtual DateTime? FromMonthYear { get; set; }
        public virtual DateTime? ToMonthYear { get; set; }
    }
    public class GetInvSppInvoiceDetailsExportInput
    {
        public virtual string PartNo { get; set; }
        public virtual string InvoiceNo { get; set; }
        public virtual string Stock { get; set; }
        public virtual string Supplier { get; set; }
        public virtual DateTime? FromMonthYear { get; set; }
        public virtual DateTime? ToMonthYear { get; set; }
    }
}
