using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.CKD
{

    [Table("InvCkdInvoice")]
    [Index(nameof(InvoiceNo), Name = "IX_InvCkdInvoice_InvoiceNo")]
    [Index(nameof(BillId), Name = "IX_InvCkdInvoice_BillId")]
    [Index(nameof(InvoiceDate), Name = "IX_InvCkdInvoice_InvoiceDate")]
    [Index(nameof(Insurance), Name = "IX_InvCkdInvoice_Insurance")]
    [Index(nameof(NetWeight), Name = "IX_InvCkdInvoice_NetWeight")]
    [Index(nameof(Remarks), Name = "IX_InvCkdInvoice_Remarks")]
    [Index(nameof(Freezed), Name = "IX_InvCkdInvoice_Freezed")]
    [Index(nameof(Status), Name = "IX_InvCkdInvoice_Status")]
    [Index(nameof(CifVn), Name = "IX_InvCkdInvoice_CifVn")]
    [Index(nameof(IsActive), Name = "IX_InvCkdInvoice_IsActive")]
    public class InvCkdInvoice : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxInvoiceNoLength = 20;

        public const int MaxOrdertypeCodeLength = 4;

        public const int MaxGoodstypeCodeLength = 4;

        public const int MaxCeptTypeLength = 10;

        public const int MaxCurrencyLength = 20;

        public const int MaxRemarksLength = 1000;

        public const int MaxSupplierNoLength = 10;

        public const int MaxSourceTypeLength = 2;

        public const int MaxStatusErrLength = 1000;

        public const int MaxLastOrdertypeLength = 4;

        public const int MaxStatusLength = 10;

        public const int MaxSpotaxLength = 1;

        public const int MaxIsActiveLength = 1;

        [StringLength(MaxInvoiceNoLength)]
        public virtual string InvoiceNo { get; set; }

        public virtual long? BillId { get; set; }

        [StringLength(MaxOrdertypeCodeLength)]
        public virtual string OrdertypeCode { get; set; }

        [StringLength(MaxGoodstypeCodeLength)]
        public virtual string GoodstypeCode { get; set; }

        public virtual long? InvoiceParentId { get; set; }

        public virtual DateTime? InvoiceDate { get; set; }

        public virtual decimal? Fob { get; set; }

        public virtual decimal? Freight { get; set; }

        public virtual decimal? FreightTotal { get; set; }

        public virtual decimal? Insurance { get; set; }

        public virtual decimal? InsuranceTotal { get; set; }

        public virtual decimal? Cif { get; set; }

        public virtual decimal? ThcTotal { get; set; }

        public virtual decimal? NetWeight { get; set; }

        public virtual decimal? GrossWeight { get; set; }

        [StringLength(MaxCeptTypeLength)]
        public virtual string CeptType { get; set; }

        [StringLength(MaxCurrencyLength)]
        public virtual string Currency { get; set; }

        [StringLength(MaxRemarksLength)]
        public virtual string Remarks { get; set; }

        [StringLength(MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        public virtual int? Quantity { get; set; }

        public virtual int? Flag { get; set; }

        public virtual int? Freezed { get; set; }

        [StringLength(MaxSourceTypeLength)]
        public virtual string SourceType { get; set; }

        [StringLength(MaxStatusErrLength)]
        public virtual string StatusErr { get; set; }

        [StringLength(MaxLastOrdertypeLength)]
        public virtual string LastOrdertype { get; set; }

        [StringLength(MaxStatusLength)]
        public virtual string Status { get; set; }

        public virtual decimal? FobVn { get; set; }

        public virtual decimal? FreightTotalVn { get; set; }

        public virtual decimal? InsuranceTotalVn { get; set; }

        public virtual decimal? CifVn { get; set; }

        public virtual decimal? ThcTotalVn { get; set; }

        [StringLength(MaxSpotaxLength)]
        public virtual string Spotax { get; set; }

        public virtual long? PeriodId { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}

