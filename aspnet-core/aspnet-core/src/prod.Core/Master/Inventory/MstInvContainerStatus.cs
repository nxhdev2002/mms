using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Inventory
{

    [Table("MstInvContainerStatus")]
    [Index(nameof(Code), Name = "IX_MstInvContainerStatus_Code")]
    [Index(nameof(Description), Name = "IX_MstInvContainerStatus_Description")]
    public class MstInvContainerStatus : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxCodeLength = 12;

        public const int MaxDescriptionLength = 50;

        public const int MaxDescriptionVnLength = 30;

        public const int MaxIsActiveLength = 100;

        [StringLength(MaxCodeLength)]
        public virtual string Code { get; set; }

        [StringLength(MaxDescriptionLength)]
        public virtual string Description { get; set; }

        [StringLength(MaxDescriptionVnLength)]
        public virtual string DescriptionVn { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}

