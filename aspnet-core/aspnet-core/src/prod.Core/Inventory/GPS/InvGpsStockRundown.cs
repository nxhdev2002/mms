using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.GPS
{
    [Table("InvGpsStockRundown")]
    public class InvGpsStockRundown : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxPartNoLength = 20;

        public const int MaxPartNameLength = 200;


        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(MaxPartNameLength)]
        public virtual string PartName { get; set; }

        public virtual long? PartId { get; set; }

        public virtual long? MaterialId { get; set; }

        public virtual int? Qty { get; set; }

        public virtual DateTime? WorkingDate { get; set; }

        public virtual long? TransactionId { get; set; }
    }
}

