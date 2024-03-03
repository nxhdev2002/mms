using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.CKD
{

    [Table("InvCkdContainerInvoice")]
    public class InvCkdContainerInvoice : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxContainerNoLength = 15;

        public const int MaxRenbanLength = 6;

        public const int MaxSupplierNoLength = 10;

        public const int MaxSealNoLength = 20;

        public const int MaxStatusLength = 10;

        public const int MaxDateStatusLength = 200;

        public const int MaxIsActiveLength = 1;

        [StringLength(MaxContainerNoLength)]
        public virtual string ContainerNo { get; set; }

        [StringLength(MaxRenbanLength)]
        public virtual string Renban { get; set; }

        public virtual long? InvoiceId { get; set; }

        [StringLength(MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        [StringLength(MaxSealNoLength)]
        public virtual string SealNo { get; set; }

        public virtual int? ContainerSize { get; set; }

        public virtual DateTime? PlandedvanningDate { get; set; }

        public virtual DateTime? ActualvanningDate { get; set; }

        public virtual decimal? Thc { get; set; }

        public virtual decimal? Inland { get; set; }

        public virtual DateTime? CdDate { get; set; }

        [StringLength(MaxStatusLength)]
        public virtual string Status { get; set; }

        public virtual decimal? ThcVn { get; set; }

        public virtual decimal? InlandVn { get; set; }

        public virtual DateTime? PeriodDate { get; set; }

        public virtual long? PeriodId { get; set; }

        [StringLength(MaxDateStatusLength)]
        public virtual string DateStatus { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}


