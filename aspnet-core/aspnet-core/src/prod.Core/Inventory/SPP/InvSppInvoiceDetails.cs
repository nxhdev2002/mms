using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.SPP
{

    [Table("InvSppInvoiceDetails")]
    [Index(nameof(PartNo), Name = "IX_InvSppInvoiceDetails_PartNo")]
    [Index(nameof(InvoiceNo), Name = "IX_InvSppInvoiceDetails_InvoiceNo")]
    [Index(nameof(Stock), Name = "IX_InvSppInvoiceDetails_Stock")]
    [Index(nameof(Month), nameof(Year), Name = "IX_InvSppInvoiceDetails_Month_Year")]
    [Index(nameof(PeriodId), Name = "IX_InvSppInvoiceDetails_PeriodId")]
    [Index(nameof(Stock), nameof(PeriodId), Name = "IX_InvSppInvoiceDetails_Stock_PeriodId")]
    [Index(nameof(Stock), nameof(Month), nameof(Year), Name = "IX_InvSppInvoiceDetails_Stock_Month_Year")]
    [Index(nameof(PartNo), nameof(Stock), nameof(Month), nameof(Year), Name = "IX_InvSppInvoiceDetails_PartNo_Stock_Month_Year")]
    [Index(nameof(PartNo), nameof(Stock), nameof(PeriodId), Name = "IX_InvSppInvoiceDetails_PartNo_Stock_PeriodId")]
    [Index(nameof(InvoiceNo), nameof(PartNo), nameof(Stock), nameof(Month), nameof(Year), Name = "IX_InvSppInvoiceDetails_InvoiceNo_PartNo_Stock_Month_Year")]
    [Index(nameof(InvoiceNo), nameof(PartNo), nameof(Stock), nameof(PeriodId), Name = "IX_InvSppInvoiceDetails_InvoiceNo_PartNo_Stock_PeriodId")]
    [Index(nameof(Supplier), nameof(Stock), nameof(Month), nameof(Year), Name = "IX_InvSppInvoiceDetails_Supplier_Stock_Month_Year")]
    [Index(nameof(Supplier), nameof(Stock), nameof(PeriodId), Name = "IX_InvSppInvoiceDetails_Supplier_Stock_PeriodId")]

    public class InvSppInvoiceDetails : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxInvoiceLength = 50;

        public const int MaxPartNoLength = 255;

        public const int MaxSupplierLength = 255;

        public const int MaxTypeLength = 5;

        public const int MaxCaseNoLength = 50;

        public const int MaxStockLength = 1;

        public const int MaxPONoLength = 20;

        public const int MaxItemNoLength = 10;

        public const int MaxCustomerNoLength = 15;

        public virtual long? InvoiceId { get; set; }

        [StringLength(MaxInvoiceLength)]
        public virtual string InvoiceNo { get; set; }

        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(MaxSupplierLength)]
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

        [StringLength(MaxTypeLength)]
        public virtual string Type { get; set; }

        [StringLength(MaxCaseNoLength)]
        public virtual string CaseNo { get; set; }

        public virtual int? Month { get; set; }

        public virtual int? Year { get; set; }

        public virtual int? PeriodId { get; set; }

        [StringLength(MaxStockLength)]
        public virtual string Stock { get; set; }

        public virtual decimal? FobVn { get; set; }

        [StringLength(MaxPONoLength)]
        public virtual string PONo { get; set; }

        public virtual decimal? LcVn { get; set; }

        public virtual decimal? CifVn { get; set; }

        public virtual decimal? VatVn { get; set; }

        public virtual decimal? InlandVn { get; set; }

        public virtual decimal? TaxVn { get; set; }

        [StringLength(MaxItemNoLength)]
        public virtual string ItemNo { get; set; }

        public virtual decimal? FreightVn { get; set; }

        public virtual decimal? InsuranceVn { get; set; }

        public virtual DateTime? ReceiveDate { get; set; }

        public virtual DateTime? ProcessDate { get; set; }

        [StringLength(MaxCustomerNoLength)]
        public virtual string CustomerNo { get; set; }

        public virtual int? IsInternal { get; set; }
    }

}
