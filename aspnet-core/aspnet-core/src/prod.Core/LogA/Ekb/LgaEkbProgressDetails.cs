using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.LogA.Ekb
{

    [Table("LgaEkbProgressDetails")]
    [Index(nameof(ProdLine), Name = "IX_LgaEkbProgressDetails_ProdLine")]
    [Index(nameof(WorkingDate), Name = "IX_LgaEkbProgressDetails_WorkingDate")]
    [Index(nameof(Shift), Name = "IX_LgaEkbProgressDetails_Shift")]
    [Index(nameof(NoInShift), Name = "IX_LgaEkbProgressDetails_NoInShift")]
    [Index(nameof(NoInDate), Name = "IX_LgaEkbProgressDetails_NoInDate")]
    [Index(nameof(ProgressId), Name = "IX_LgaEkbProgressDetails_ProgressId")]
    [Index(nameof(ProcessId), Name = "IX_LgaEkbProgressDetails_ProcessId")]
    [Index(nameof(PartListId), Name = "IX_LgaEkbProgressDetails_PartListId")]
    [Index(nameof(PartNo), Name = "IX_LgaEkbProgressDetails_PartNo")]
    [Index(nameof(BackNo), Name = "IX_LgaEkbProgressDetails_BackNo")]
    [Index(nameof(Sorting), Name = "IX_LgaEkbProgressDetails_Sorting")]
    [Index(nameof(IsZeroKb), Name = "IX_LgaEkbProgressDetails_IsZeroKb")]
    [Index(nameof(IsActive), Name = "IX_LgaEkbProgressDetails_IsActive")]
    public class LgaEkbProgressDetails : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxProdLineLength = 50;

        public const int MaxShiftLength = 50;

        public const int MaxPartNoLength = 50;

        public const int MaxPartNoNormalizedLength = 50;

        public const int MaxBackNoLength = 50;
        
        public const int MaxSequenceNoLength = 10;

        public const int MaxBodyColorLength = 50;

        public const int MaxPcAddressLength = 200;

        public const int MaxSpsAddressLength = 200;

        public const int MaxBodyNoLength = 50;

        public const int MaxLotNoLength = 50;

        public const int MaxGradeLength = 50;

        public const int MaxModelLength = 50;

        public const int MaxIsZeroKbLength = 1;

        public const int MaxStatusLength = 50;

        public const int MaxIsActiveLength = 1;

        public const int MaxKanbanSeqLength = 50;

        


        [StringLength(MaxProdLineLength)]
        public virtual string ProdLine { get; set; }

        public virtual DateTime? WorkingDate { get; set; }

        [StringLength(MaxShiftLength)]
        public virtual string Shift { get; set; }
        public virtual long? KanbanId { get; set; }

        [StringLength(MaxKanbanSeqLength)]
        public virtual string KanbanSeq { get; set; }

        public virtual int? NoInShift { get; set; }

        public virtual int? NoInDate { get; set; }

        public virtual long? ProgressId { get; set; }

        public virtual long? ProcessId { get; set; }

        public virtual long? PartListId { get; set; }

        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(MaxPartNoNormalizedLength)]
        public virtual string PartNoNormalized { get; set; }

        [StringLength(MaxBackNoLength)]
        public virtual string BackNo { get; set; }

        [StringLength(MaxSequenceNoLength)]
        public virtual string SequenceNo { get; set; }

        [StringLength(MaxBodyColorLength)]
        public virtual string BodyColor { get; set; }

        [StringLength(MaxPcAddressLength)]
        public virtual string PcAddress { get; set; }

        [StringLength(MaxSpsAddressLength)]
        public virtual string SpsAddress { get; set; }

        public virtual int? Sorting { get; set; }

        public virtual int? UsageQty { get; set; }

        [StringLength(MaxBodyNoLength)]
        public virtual string BodyNo { get; set; }

        [StringLength(MaxLotNoLength)]
        public virtual string LotNo { get; set; }

        public virtual int? NoInLot { get; set; }

        [StringLength(MaxGradeLength)]
        public virtual string Grade { get; set; }

        [StringLength(MaxModelLength)]
        public virtual string Model { get; set; }

        public virtual int? EkbQty { get; set; }

        public virtual int? RemainQty { get; set; }

        [StringLength(MaxIsZeroKbLength)]
        public virtual string IsZeroKb { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? NewtaktDatetime { get; set; }

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