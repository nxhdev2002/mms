using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Cmm
{

    [Table("MstCmmProductGroup")]
    public class MstCmmProductGroup : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxCodeLength = 50;

        public const int MaxDescriptionLength = 200;

        [StringLength(MaxCodeLength)]
        public virtual string Code { get; set; }

        [StringLength(MaxDescriptionLength)]
        public virtual string Description { get; set; }
    }

}

