using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Inventory
{

    [Table("MstInvCpsSuppliers")]
    public class MstInvCpsSuppliers : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxSupplierNameLength = 255;

        public const int MaxSupplierNumberLength = 50;

        public const int MaxVatregistrationNumLength = 255;

        public const int MaxVatregistrationInvoiceLength = 255;

        public const int MaxTaxPayerIdLength = 255;

        public const int MaxIsActiveLength = 1;

        [StringLength(MaxSupplierNameLength)]
        public virtual string SupplierName { get; set; }

        [StringLength(MaxSupplierNumberLength)]
        public virtual string SupplierNumber { get; set; }

        [StringLength(MaxVatregistrationNumLength)]
        public virtual string VatregistrationNum { get; set; }

        [StringLength(MaxVatregistrationInvoiceLength)]
        public virtual string VatregistrationInvoice { get; set; }

        [StringLength(MaxTaxPayerIdLength)]
        public virtual string TaxPayerId { get; set; }

        public virtual long? RegistryId { get; set; }

        [Column(TypeName = "Date")]
        public virtual DateTime? StartDateActive { get; set; }

        [Column(TypeName = "Date")]
        public virtual DateTime? EndDateActive { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}

