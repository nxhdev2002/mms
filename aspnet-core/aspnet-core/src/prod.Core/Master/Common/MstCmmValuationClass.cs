using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Common
{

    [Table("MstCmmValuationClass")]
    [Index(nameof(Code), Name = "IX_MstCmmValuationClass_Code")]
    [Index(nameof(BsAccount), Name = "IX_MstCmmValuationClass_BsAccount")]
    [Index(nameof(BsAccountDescription), Name = "IX_MstCmmValuationClass_BsAccountDescription")]
    [Index(nameof(IsActive), Name = "IX_MstCmmValuationClass_IsActive")]
    public class MstCmmValuationClass : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxCodeLength = 4;

        public const int MaxNameLength = 25;

        public const int MaxBsAccountLength = 25;

        public const int MaxBsAccountDescriptionLength = 200;

        public const int MaxIsActiveLength = 1;

        [StringLength(MaxCodeLength)]
        public virtual string Code { get; set; }

        [StringLength(MaxNameLength)]
        public virtual string Name { get; set; }

        [StringLength(MaxBsAccountLength)]
        public virtual string BsAccount { get; set; }

        [StringLength(MaxBsAccountDescriptionLength)]
        public virtual string BsAccountDescription { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}


