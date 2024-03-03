using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.CPS
{
    [Table("InvCpsInvoiceHeaders")]
    public class InvCpsInvoiceHeaders : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxInvoiceNumLength = 50;

        public const int MaxInvoiceSymbolLength = 10;

        public const int MaxDescriptionLength = 40;

        public const int MaxVendorNameLength = 255;

        public const int MaxCurrencyCodeLength = 15;

        public const int MaxIsActiveLength = 1;

        [StringLength(MaxInvoiceNumLength)]
        public virtual string InvoiceNum { get; set; }

        [StringLength(MaxInvoiceSymbolLength)]
        public virtual string InvoiceSymbol { get; set; }

        [StringLength(MaxDescriptionLength)]
        public virtual string Description { get; set; }

        public virtual DateTime? InvoiceDate { get; set; }

        public virtual long? VendorId { get; set; }

        [StringLength(MaxVendorNameLength)]
        public virtual string VendorName { get; set; }

        [StringLength(MaxCurrencyCodeLength)]
        public virtual string CurrencyCode { get; set; }

        public virtual decimal? InvoiceAmount { get; set; }

        public virtual decimal? AmountVat { get; set; }

        public virtual decimal? TaxRate { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}
