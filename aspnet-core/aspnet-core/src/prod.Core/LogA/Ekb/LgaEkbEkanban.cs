using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.LogA.Ekb
{

    [Table("LgaEkbEkanban")]
    [Index(nameof(KanbanSeq), Name = "IX_LgaEkbEkanban_KanbanSeq")]
    [Index(nameof(ProdLine), Name = "IX_LgaEkbEkanban_ProdLine")]
    [Index(nameof(WorkingDate), Name = "IX_LgaEkbEkanban_WorkingDate")]
    [Index(nameof(Shift), Name = "IX_LgaEkbEkanban_Shift")]
    [Index(nameof(ProgressId), Name = "IX_LgaEkbEkanban_ProgressId")]
    [Index(nameof(ProcessId), Name = "IX_LgaEkbEkanban_ProcessId")]
    [Index(nameof(PartListId), Name = "IX_LgaEkbEkanban_PartListId")]
    [Index(nameof(PartNo), Name = "IX_LgaEkbEkanban_PartNo")]
    [Index(nameof(BackNo), Name = "IX_LgaEkbEkanban_BackNo")]
    [Index(nameof(Sorting), Name = "IX_LgaEkbEkanban_Sorting")]
    [Index(nameof(IsActive), Name = "IX_LgaEkbEkanban_IsActive")]
    public class LgaEkbEkanban : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxKanbanSeqLength = 50;

        public const int MaxProdLineLength = 50;

        public const int MaxShiftLength = 50;

        public const int MaxPartNoLength = 50;

        public const int MaxPartNoNormalizedLength = 50;

        public const int MaxBackNoLength = 50;

        public const int MaxPcAddressLength = 200;

        public const int MaxSpsAddressLength = 200;

        public const int MaxIsZeroKbLength = 1;

        public const int MaxStatusLength = 50;

        public const int MaxIsActiveLength = 1;

        [StringLength(MaxKanbanSeqLength)]
        public virtual string KanbanSeq { get; set; }

        public virtual int? KanbanNoInDate { get; set; }

        [StringLength(MaxProdLineLength)]
        public virtual string ProdLine { get; set; }

        [Column(TypeName = "date")]
        public virtual DateTime? WorkingDate { get; set; }

        [StringLength(MaxShiftLength)]
        public virtual string Shift { get; set; }

        public virtual long? ProgressId { get; set; }

        public virtual long? ProcessId { get; set; }

        public virtual long? PartListId { get; set; }

        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(MaxPartNoNormalizedLength)]
        public virtual string PartNoNormalized { get; set; }

        [StringLength(MaxBackNoLength)]
        public virtual string BackNo { get; set; }

        [StringLength(MaxPcAddressLength)]
        public virtual string PcAddress { get; set; }

        [StringLength(MaxSpsAddressLength)]
        public virtual string SpsAddress { get; set; }

        public virtual int? Sorting { get; set; }

        public virtual int? RequestQty { get; set; }

        public virtual int? ScanQty { get; set; }

        public virtual int? InputQty { get; set; }

        [StringLength(MaxIsZeroKbLength)]
        public virtual string IsZeroKb { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? RequestDatetime { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? PikStartDatetime { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? PikFinishDatetime { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? DelStartDatetime { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? DelFinishDatetime { get; set; }

        [StringLength(MaxStatusLength)]
        public virtual string Status { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}