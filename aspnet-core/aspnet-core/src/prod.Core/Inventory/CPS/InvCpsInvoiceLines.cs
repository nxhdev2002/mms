using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.CPS
{

    [Table("InvCpsInvoiceLines")]
    public class InvCpsInvoiceLines : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxPoNumberLength = 20;

        public const int MaxPartNoLength = 100;

        public const int MaxItemDescriptionLength = 255;

        public const int MaxNoteLength = 255;

        public const int MaxIsActiveLength = 1;

        public virtual long? InvoiceId { get; set; }

        public virtual int? LineNum { get; set; }

        public virtual long? PoHeaderId { get; set; }

        [StringLength(MaxPoNumberLength)]
        public virtual string PoNumber { get; set; }

        public virtual long? VendorId { get; set; }

        public virtual long? ItemId { get; set; }

        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(MaxItemDescriptionLength)]
        public virtual string ItemDescription { get; set; }

        public virtual decimal? Quantity { get; set; }

        public virtual decimal? QuantityOrder { get; set; }

        public virtual decimal? UnitPrice { get; set; }

        public virtual decimal? Amount { get; set; }

        public virtual decimal? AmountVat { get; set; }

        public virtual decimal? ForeignPrice { get; set; }

        public virtual decimal? TaxRate { get; set; }

        public virtual decimal? UnitpricePo { get; set; }

        [StringLength(MaxNoteLength)]
        public virtual string Note { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}
