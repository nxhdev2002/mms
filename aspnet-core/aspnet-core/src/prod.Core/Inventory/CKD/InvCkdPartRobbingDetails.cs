using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.CKD
{
    [Table("InvCkdPartRobbingDetails")]
    public class InvCkdPartRobbingDetails : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxPartNoLength = 50;

        public const int MaxGradeLength = 3;

        public const int MaxIsActiveLength = 1;

        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        public virtual long? PartId { get; set; }

        [StringLength(MaxGradeLength)]
        public virtual string Grade { get; set; }

        public virtual int? RobbingQty { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }
}

