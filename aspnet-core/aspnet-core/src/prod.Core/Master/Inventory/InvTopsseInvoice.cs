using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Inventory
{

    [Table("InvTopsseInvoice")]
    public class InvTopsseInvoice : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxDistFdLength = 7;

        public const int MaxInvoiceNoLength = 15;

        public const int MaxBillOfLadingLength = 20;


        [StringLength(MaxDistFdLength)]
        public virtual string DistFd { get; set; }

        [StringLength(MaxInvoiceNoLength)]
        public virtual string InvoiceNo { get; set; }

        public virtual DateTime? InvoiceDate { get; set; }

        [StringLength(MaxBillOfLadingLength)]
        public virtual string BillOfLading { get; set; }

        public virtual DateTime? ProcessDate { get; set; }

        public virtual DateTime? Etd { get; set; }
    }

}
