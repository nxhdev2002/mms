using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Inventory
{

    [Table("MstInvHrTitles")]
    [Index(nameof(Code), Name = "IX_MstInvHrTitles_Code")]
    [Index(nameof(IsActive), Name = "IX_MstInvHrTitles_IsActive")]
    public class MstInvHrTitles : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxCodeLength = 50;

        public const int MaxNameLength = 500;

        public const int MaxDescriptionLength = 100;

        public const int MaxIsActiveLength = 1;


        [StringLength(MaxCodeLength)]
        public virtual string Code { get; set; }

        [StringLength(MaxNameLength)]
        public virtual string Name { get; set; }

        [StringLength(MaxDescriptionLength)]
        public virtual string Description { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}

