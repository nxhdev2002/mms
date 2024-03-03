using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.LogA
{
    [Table("MstLgaSpsRack")]
    [Index(nameof(Code), Name = "IX_MstLgaSpsRack_Code")]
    [Index(nameof(IsActive), Name = "IX_MstLgaSpsRack_IsActive")]
    public class MstLgaSpsRack : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxCodeLength = 50;

        public const int MaxAddressLength = 200;

        public const int MaxIsActiveLength = 1;


        [StringLength(MaxCodeLength)]
        public virtual string Code { get; set; }

        [StringLength(MaxAddressLength)]
        public virtual string Address { get; set; }

        public virtual int? Ordering { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}