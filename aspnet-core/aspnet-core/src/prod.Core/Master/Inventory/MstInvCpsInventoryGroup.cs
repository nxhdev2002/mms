using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Inventory
{

    [Table("MstInvCpsInventoryGroup")]
    public class MstInvCpsInventoryGroup : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxProductgroupcodeLength = 255;

        public const int MaxProductgroupnameLength = 50;

        public const int MaxIsActiveLength = 1;

        [StringLength(MaxProductgroupcodeLength)]
        public virtual string Productgroupcode { get; set; }

        [StringLength(MaxProductgroupnameLength)]
        public virtual string Productgroupname { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}