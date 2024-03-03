using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Inventory
{

    [Table("MstInvShippingCompany")]
    [Index(nameof(Code), Name = "IX_MstInvShippingCompany_Code")]
    [Index(nameof(Name), Name = "IX_MstInvShippingCompany_Name")]
    [Index(nameof(Description), Name = "IX_MstInvShippingCompany_Description")]
    [Index(nameof(IsActive), Name = "IX_MstInvShippingCompany_IsActive")]
    public class MstInvShippingCompany : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxCodeLength = 10;

        public const int MaxNameLength = 50;

        public const int MaxDescriptionLength = 200;

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

