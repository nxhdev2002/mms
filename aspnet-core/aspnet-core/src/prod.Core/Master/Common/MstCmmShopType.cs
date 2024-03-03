using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Common
{

    [Table("MstCmmShopType")]
    [Index(nameof(Code), Name = "IX_MstCmmShopType_Code")]
    [Index(nameof(IsActive), Name = "IX_MstCmmShopType_IsActive")]
    public class MstCmmShopType : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxCodeLength = 2;

        public const int MaxNameLength = 20;

        public const int MaxIsActiveLength = 1;

        [StringLength(MaxCodeLength)]
        public virtual string Code { get; set; }

        [StringLength(MaxNameLength)]
        public virtual string Name { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}
