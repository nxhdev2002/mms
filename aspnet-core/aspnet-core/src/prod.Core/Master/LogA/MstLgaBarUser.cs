using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.LogA
{
    [Table("MstLgaBarUser")]
    [Index(nameof(UserId), Name = "IX_MstLgaBarUser_UserId")]
    [Index(nameof(ProcessId), Name = "IX_MstLgaBarUser_ProcessId")]
    [Index(nameof(ProcessCode), Name = "IX_MstLgaBarUser_ProcessCode")]
    [Index(nameof(IsActive), Name = "IX_MstLgaBarUser_IsActive")]
    public class MstLgaBarUser : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxUserIdLength = 50;

        public const int MaxUserNameLength = 50;

        public const int MaxUserDescriptionLength = 200;

        public const int MaxIsNeedPassLength = 1;

        public const int MaxPwdLength = 50;

        public const int MaxProcessCodeLength = 50;

        public const int MaxProcessGroupLength = 50;

        public const int MaxProcessSubgroupLength = 50;

        public const int MaxIsActiveLength = 1;



        [StringLength(MaxUserIdLength)]
        public virtual string UserId { get; set; }

        [StringLength(MaxUserNameLength)]
        public virtual string UserName { get; set; }

        [StringLength(MaxUserDescriptionLength)]
        public virtual string UserDescription { get; set; }

        [StringLength(MaxIsNeedPassLength)]
        public virtual string IsNeedPass { get; set; }

        [StringLength(MaxPwdLength)]
        public virtual string Pwd { get; set; }

        public virtual long? ProcessId { get; set; }

        [StringLength(MaxProcessCodeLength)]
        public virtual string ProcessCode { get; set; }

        [StringLength(MaxProcessGroupLength)]
        public virtual string ProcessGroup { get; set; }

        [StringLength(MaxProcessSubgroupLength)]
        public virtual string ProcessSubgroup { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}

