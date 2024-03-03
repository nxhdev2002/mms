using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.CKD
{

    [Table("InvCkdInvoiceLot")]
    public class InvCkdInvoiceLot : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxFixlotLength = 4;

        public const int MaxEdNoLength = 6;

        public const int MaxPaymentTermsLength = 100;

        public const int MaxRemarksLength = 1000;

        public const int MaxSupplierNoLength = 50;

        public const int MaxF1Length = 255;

        public const int MaxF2Length = 255;

        public const int MaxF3Length = 255;

        public const int MaxConstraintLength = 20;

        public const int MaxIsActiveLength = 1;

        [StringLength(MaxFixlotLength)]
        public virtual string Fixlot { get; set; }

        public virtual long? PreCustomsId { get; set; }

        public virtual long? CustomsDeclareId { get; set; }

        public virtual long? InvoiceId { get; set; }

        [StringLength(MaxEdNoLength)]
        public virtual string EdNo { get; set; }

        public virtual int? UnitPerLot { get; set; }

        public virtual int? Islot { get; set; }

        [StringLength(MaxPaymentTermsLength)]
        public virtual string PaymentTerms { get; set; }

        [StringLength(MaxRemarksLength)]
        public virtual string Remarks { get; set; }

        [StringLength(MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        [StringLength(MaxF1Length)]
        public virtual string F1 { get; set; }

        [StringLength(MaxF2Length)]
        public virtual string F2 { get; set; }

        [StringLength(MaxF3Length)]
        public virtual string F3 { get; set; }

        public virtual DateTime? InvCopyDate { get; set; }

        public virtual DateTime? InvOrginDate { get; set; }

        public virtual DateTime? FormdbDate { get; set; }

        [StringLength(MaxConstraintLength)]
        public virtual string Constraint { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}

