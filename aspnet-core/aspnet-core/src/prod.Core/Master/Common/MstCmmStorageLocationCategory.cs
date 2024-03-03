using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Common
{
    [Table("MstCmmStorageLocationCategory")]
    [Index(nameof(Code), Name = "IX_MstCmmStorageLocationCategory_Code")]
    [Index(nameof(IsActive), Name = "IX_MstCmmStorageLocationCategory_IsActive")]
    public class MstCmmStorageLocationCategory : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxCodeLength = 4;

        public const int MaxNameLength = 50;

        public const int MaxAreaTypeLength = 50;

        public const int MaxIsActiveLength = 1;

        [StringLength(MaxCodeLength)]
        public virtual string Code { get; set; }

        [StringLength(MaxNameLength)]
        public virtual string Name { get; set; }

        [StringLength(MaxAreaTypeLength)]
        public virtual string AreaType { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }

    }
}

