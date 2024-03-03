using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Inventory
{

    [Table("MstInvContainerDeliveryType")]
    [Index(nameof(Code), Name = "IX_MstInvContainerDeliveryType_Code")]
    [Index(nameof(Description), Name = "IX_MstInvContainerDeliveryType_Description")]
    [Index(nameof(IsActive), Name = "IX_MstInvContainerDeliveryType_IsActive")]
    public class MstInvContainerDeliveryType : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxCodeLength = 10;

        public const int MaxDescriptionLength = 100;

        public const int MaxIsActiveLength = 1;

        [StringLength(MaxCodeLength)]
        public virtual string Code { get; set; }

        [StringLength(MaxDescriptionLength)]
        public virtual string Description { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }
}
