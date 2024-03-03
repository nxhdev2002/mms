using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Inventory
{

    [Table("MstGpsMaterialCategory")]
    public class MstInvGpsMaterialCategory : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxNameLength = 50;

        public const int MaxIsActiveLength = 1;

        [StringLength(MaxNameLength)]
        public virtual string Name { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}
