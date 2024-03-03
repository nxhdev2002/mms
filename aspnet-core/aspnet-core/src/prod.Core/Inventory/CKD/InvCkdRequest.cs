using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.CKD
{

    [Table("InvCkdRequest")]
    public class InvCkdRequest : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxCkdReqNoLength = 50;

        public const int MaxStatusLength = 10;

        public const int MaxIsActiveLength = 1;

        [StringLength(MaxCkdReqNoLength)]
        public virtual string CkdReqNo { get; set; }

        public virtual DateTime? IssueDate { get; set; }

        public virtual DateTime? ReqDate { get; set; }

        public virtual DateTime? PeriodDate { get; set; }

        public virtual long? PeriodId { get; set; }

        [StringLength(MaxStatusLength)]
        public virtual string Status { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}

