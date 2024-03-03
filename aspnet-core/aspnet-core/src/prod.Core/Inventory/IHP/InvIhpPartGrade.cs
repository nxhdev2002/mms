using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.IHP
{
    [Table("InvIhpPartGrade")]
    public class InvIhpPartGrade : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxGradeLength = 3;

        [StringLength(MaxGradeLength)]
        public virtual string Grade { get; set; }

        public virtual long? IhpPartId { get; set; }

        public virtual int? UsageQty { get; set; }

        public virtual DateTime? FirstDayProduct { get; set; }

        public virtual DateTime? LastDayProduct { get; set; }
    }
}

