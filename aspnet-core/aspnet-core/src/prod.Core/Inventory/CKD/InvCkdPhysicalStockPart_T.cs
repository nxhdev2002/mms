using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.CKD
{

    [Table("InvCkdPhysicalStockPart_T")]
    public class InvCkdPhysicalStockPart_T : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxGuidLength = 128;

        public const int MaxPartNoLength = 12;

        public const int MaxCfcLength = 10;

        public const int MaxSupplierNoLength = 50;

        public const int MaxErrorDescriptionLength = 5000;

        

        [StringLength(MaxGuidLength)]
        public virtual string Guid { get; set; }

        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(MaxCfcLength)]
        public virtual string Cfc { get; set; }

        [StringLength(MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        public virtual decimal? Qty { get; set; }

        [StringLength(MaxErrorDescriptionLength)]
        public string ErrorDescription { get; set; }

    }

}

