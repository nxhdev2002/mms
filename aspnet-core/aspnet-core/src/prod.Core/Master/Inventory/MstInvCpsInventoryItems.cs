using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Inventory
{
    [Table("MstInvCpsInventoryItems")]
    public class MstInvCpsInventoryItems : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxPartNoLength = 40;

        public const int MaxPartNameLength = 500;

        public const int MaxColorLength = 40;

        public const int MaxPuomLength = 50;

        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(MaxPartNameLength)]
        public virtual string PartName { get; set; }

        [StringLength(MaxColorLength)]
        public virtual string Color { get; set; }

        [StringLength(MaxPuomLength)]
        public virtual string Puom { get; set; }
    }

}
