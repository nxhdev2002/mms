using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Assy.Andon
{

    [Table("AsyAdoAssemblyData")]
    [Index(nameof(WorkingDate), Name = "IX_AsyAdoAssemblyData_WorkingDate")]
    [Index(nameof(Line), Name = "IX_AsyAdoAPlanShift_Line")]
    [Index(nameof(Process), Name = "IX_AsyAdoAPlanShift_Process")]
    [Index(nameof(Model), Name = "IX_AsyAdoAPlanShift_Model")]
    [Index(nameof(Body), Name = "IX_AsyAdoAPlanShift_Body")]
    public class AsyAdoAssemblyData : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxLineLength = 2;

        public const int MaxProcessLength = 5;

        public const int MaxModelLength = 2;

        public const int MaxBodyLength = 5;

        public const int MaxSeqNoLength = 10;

        public const int MaxGradeLength = 2;

        public const int MaxLotNoLength = 10;

        public const int MaxColorLength = 5;

        public const int MaxShiftLength = 10;


        [Column(TypeName = "date")]
        public virtual DateTime? WorkingDate { get; set; }

        [StringLength(MaxShiftLength)]
        public virtual string Shift { get; set; }

        public virtual int? NoInDate { get; set; }

        [StringLength(MaxLineLength)]
        public virtual string Line { get; set; }

        [StringLength(MaxProcessLength)]
        public virtual string Process { get; set; }

        [StringLength(MaxModelLength)]
        public virtual string Model { get; set; }

        [StringLength(MaxBodyLength)]
        public virtual string Body { get; set; }

        [StringLength(MaxSeqNoLength)]
        public virtual string SeqNo { get; set; }

        [StringLength(MaxGradeLength)]
        public virtual string Grade { get; set; }

        [StringLength(MaxLotNoLength)]
        public virtual string LotNo { get; set; }
        public virtual int? NoInLot { get; set; }
        public virtual int? LotNoIndex { get; set; }
        public virtual int? NoInShift { get; set; }

        [StringLength(MaxColorLength)]
        public virtual string Color { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? CreateDate { get; set; }

    }

}