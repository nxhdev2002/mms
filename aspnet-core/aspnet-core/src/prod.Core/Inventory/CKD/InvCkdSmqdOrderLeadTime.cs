using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.CKD
{

    [Table("InvCkdSmqdOrderLeadTime")]
    public class InvCkdSmqdOrderLeadTime : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxSupplierNoLength = 50;

        public const int MaxCfcLength = 4;

        public const int MaxExpCodeLength = 50;


        [StringLength(MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        [StringLength(MaxCfcLength)]
        public virtual string Cfc { get; set; }

        [StringLength(MaxExpCodeLength)]
        public virtual string ExpCode { get; set; }

        public virtual int? Sea { get; set; }

        public virtual int? Air { get; set; }
    }

}

