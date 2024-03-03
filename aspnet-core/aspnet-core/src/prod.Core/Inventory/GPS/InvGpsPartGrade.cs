using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.GPS
{
    [Table("InvGpsPartGrade")]
    public class InvGpsPartGrade : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxGradeLength = 3;

        public const int MaxBodyColorLength = 3;

        public const int MaxProcessUseLength = 50;

        public const int MaxShopLength = 50;


        public virtual int? PartListId { get; set; }

        [StringLength(MaxGradeLength)]
        public virtual string Grade { get; set; }

        [StringLength(MaxBodyColorLength)]
        public virtual string BodyColor { get; set; }

        public virtual decimal? UsageQty { get; set; }

        [StringLength(MaxProcessUseLength)]
        public virtual string ProcessUse { get; set; }

        [StringLength(MaxShopLength)]
        public virtual string Shop { get; set; }
        public virtual int? VehicleId { get; set; }
    }
}

