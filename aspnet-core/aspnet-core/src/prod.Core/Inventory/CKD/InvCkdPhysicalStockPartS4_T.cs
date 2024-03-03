using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inv.CKD
{
    [Table("InvCkdPhysicalStockPartS4_T")]
    public class InvCkdPhysicalStockPartS4_T : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxGuidLength = 128;

        public const int MaxMaterialCodeLength = 40;

        public const int MaxErrorDescriptionLength = 5000;




        [StringLength(MaxGuidLength)]
        public virtual string Guid { get; set; }

        [StringLength(MaxMaterialCodeLength)]
        public virtual string MaterialCode { get; set; }

        public virtual long? PeriodId { get; set; }

        public virtual int? Qty { get; set; }

        [StringLength(MaxErrorDescriptionLength)]
        public string ErrorDescription { get; set; }
    }

}