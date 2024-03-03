using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.LogA.Ekb
{

    [Table("LgaEkbProgress")]
    [Index(nameof(ProdLine), Name = "IX_LgaEkbProgress_ProdLine")]
    [Index(nameof(WorkingDate), Name = "IX_LgaEkbProgress_WorkingDate")]
    [Index(nameof(Shift), Name = "IX_LgaEkbProgress_Shift")]
    [Index(nameof(ProcessId), Name = "IX_LgaEkbProgress_ProcessId")]
    [Index(nameof(IsActive), Name = "IX_LgaEkbProgress_IsActive")]
    public class LgaEkbProgress : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxProdLineLength = 50;

        public const int MaxShiftLength = 50;

        public const int MaxProcessCodeLength = 50;

        public const int MaxStatusLength = 50;

        public const int MaxIsActiveLength = 1;

        [StringLength(MaxProdLineLength)]
        public virtual string ProdLine { get; set; }

        public virtual DateTime? WorkingDate { get; set; }

        [StringLength(MaxShiftLength)]
        public virtual string Shift { get; set; }

        public virtual int? NoInShift { get; set; }

        public virtual int? NoInDate { get; set; }

        public virtual long? ProcessId { get; set; }

        [StringLength(MaxProcessCodeLength)]
        public virtual string ProcessCode { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? NewtaktDatetime { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? StartDatetime { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? FinishDatetime { get; set; }

        [StringLength(MaxStatusLength)]
        public virtual string Status { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}