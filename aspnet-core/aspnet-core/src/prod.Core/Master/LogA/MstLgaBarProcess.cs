using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.LogA
{
    [Table("MstLgaBarProcess")]
    [Index(nameof(Code), Name = "IX_MstLgaBarProcess_Code")]
    [Index(nameof(ProdLine), Name = "IX_MstLgaBarProcess_ProdLine")]
    [Index(nameof(IsActive), Name = "IX_MstLgaBarProcess_IsActive")]
    public class MstLgaBarProcess : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxCodeLength = 50;

        public const int MaxProcessNameLength = 50;

        public const int MaxProcessGroupLength = 50;

        public const int MaxProcessSubgroupLength = 50;

        public const int MaxProdLineLength = 50;

        public const int MaxProcessTypeLength = 50;

        public const int MaxIsActiveLength = 1;

        [StringLength(MaxCodeLength)]
        public virtual string Code { get; set; }

        [StringLength(MaxProcessNameLength)]
        public virtual string ProcessName { get; set; }

        [StringLength(MaxProcessGroupLength)]
        public virtual string ProcessGroup { get; set; }

        [StringLength(MaxProcessSubgroupLength)]
        public virtual string ProcessSubgroup { get; set; }

        [StringLength(MaxProdLineLength)]
        public virtual string ProdLine { get; set; }

        [StringLength(MaxProcessTypeLength)]
        public virtual string ProcessType { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}

