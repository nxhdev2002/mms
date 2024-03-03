using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.LogA
{

    [Table("MstLgaEkbUser")]
    [Index(nameof(UserCode), Name = "IX_MstLgaEkbUser_UserCode")]
    [Index(nameof(ProdLine), Name = "IX_MstLgaEkbUser_ProdLine")]
    [Index(nameof(Sortingg), Name = "IX_MstLgaEkbUser_Sortingg")]
    [Index(nameof(IsActive), Name = "IX_MstLgaEkbUser_IsActive")]
    public class MstLgaEkbUser : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxUserCodeLength = 50;

        public const int MaxUserNameLength = 50;

        public const int MaxProcessCodeLength = 50;

        public const int MaxProcessGroupLength = 50;

        public const int MaxProcessSubgroupLength = 50;

        public const int MaxProdLineLength = 50;

        public const int MaxUserTypeLength = 50;

        public const int MaxIsActiveLength = 1;

        [StringLength(MaxUserCodeLength)]
        public virtual string UserCode { get; set; }

        [StringLength(MaxUserNameLength)]
        public virtual string UserName { get; set; }

        public virtual int? ProcessId { get; set; }

        [StringLength(MaxProcessCodeLength)]
        public virtual string ProcessCode { get; set; }

        [StringLength(MaxProcessGroupLength)]
        public virtual string ProcessGroup { get; set; }

        [StringLength(MaxProcessSubgroupLength)]
        public virtual string ProcessSubgroup { get; set; }

        [StringLength(MaxProdLineLength)]
        public virtual string ProdLine { get; set; }

        [StringLength(MaxUserTypeLength)]
        public virtual string UserType { get; set; }

        public virtual int? LeadTime { get; set; }

        public virtual int? Sortingg { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}