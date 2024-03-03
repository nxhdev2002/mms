using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inv.CKD
{
    [Table("InvCkdPhysicalStockPartS4")]
    public class InvCkdPhysicalStockPartS4 : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxMaterialCodeLength = 40;

        [StringLength(MaxMaterialCodeLength)]
        public virtual string MaterialCode { get; set; }

        public virtual long? PeriodId { get; set; }

        public virtual int? Qty { get; set; }
    }

}