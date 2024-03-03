using Castle.MicroKernel.SubSystems.Conversion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PayPalCheckoutSdk.Orders;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace prod.Painting.Andon
{
    [Table("PtsBmpProgress")]
    [Index(nameof(PartId), Name = "IX_PtsBmpProgress_PartId")]
    [Index(nameof(BackNo), Name = "IX_PtsBmpProgress_BackNo")]
    [Index(nameof(PartTypeCode), Name = "IX_PtsBmpProgress_PartTypeCode")]
    [Index(nameof(SequenceNo), Name = "IX_PtsBmpProgress_SequenceNo")]
    [Index(nameof(ProdLine), Name = "IX_PtsBmpProgress_ProdLine")]
    [Index(nameof(WorkingDate), Name = "IX_PtsBmpProgress_WorkingDate")]
    [Index(nameof(Shift), Name = "IX_PtsBmpProgress_Shift")]
    [Index(nameof(SignalId), Name = "IX_PtsBmpProgress_SignalId")]
    [Index(nameof(Status), Name = "IX_PtsBmpProgress_Status")]
    [Index(nameof(IsActive), Name = "IX_PtsBmpProgress_IsActive")]

    public class PtsBmpProgress : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxBackNoLength = 50;

        public const int MaxModelLength = 50;

        public const int MaxCfcLength = 50;

        public const int MaxGradeLength = 50;

        public const int MaxPartTypeCodeLength = 50;

        public const int MaxIsPunchLength = 1;

        public const int MaxIsBumperLength = 1;

        public const int MaxLotNoLength = 50;

        public const int MaxSequenceNoLength = 50;

        public const int MaxBodyNoLength = 50;

        public const int MaxColorLength = 50;

        public const int MaxSpecialColorLength = 50;

        public const int MaxProcessLength = 50;

        public const int MaxPkProcessLength = 50;

        public const int MaxProdLineLength = 50;

        public const int MaxShiftLength = 50;

        public const int MaxSignalCodeLength = 50;

        public const int MaxStatusLength = 50;
        
        public const int MaxIsActiveLength = 1;




        public virtual long? PartId { get; set; }

        [StringLength(MaxBackNoLength)]
        public virtual string BackNo { get; set; }

        [StringLength(MaxModelLength)]
        public virtual string Model { get; set; }

        [StringLength(MaxCfcLength)]
        public virtual string Cfc { get; set; }

        [StringLength(MaxGradeLength)]
        public virtual string Grade { get; set; }

        [StringLength(MaxPartTypeCodeLength)]
        public virtual string PartTypeCode { get; set; }

        [StringLength(MaxIsPunchLength)]
        public virtual string IsPunch { get; set; }

        [StringLength(MaxIsBumperLength)]
        public virtual string IsBumper { get; set; }

        [StringLength(MaxLotNoLength)]
        public virtual string LotNo { get; set; }

        public virtual int? NoInLot { get; set; }

        [StringLength(MaxSequenceNoLength)]
        public virtual string SequenceNo { get; set; }

        [StringLength(MaxBodyNoLength)]
        public virtual string BodyNo { get; set; }

        [StringLength(MaxColorLength)]
        public virtual string Color { get; set; }

        [StringLength(MaxSpecialColorLength)]
        public virtual string SpecialColor { get; set; }

        [StringLength(MaxProcessLength)]
        public virtual string Process { get; set; }

        [StringLength(MaxPkProcessLength)]
        public virtual string PkProcess { get; set; }

        [StringLength(MaxProdLineLength)]
        public virtual string ProdLine { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? WorkingDate { get; set; }

        [StringLength(MaxShiftLength)]
        public virtual string Shift { get; set; }

        public virtual int? NoInDate { get; set; }

        public virtual int? NoInShift { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? TaktTimeStart { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? ToUpDate { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? UpDate { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? UpPunchDate { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? UpSkipDate { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? UpRecoatDate { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? SubAssyDate { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? SubAssyPunchDate { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? SubAssySkipDate { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? SubAssyRecoatDate { get; set; }

        public virtual int? SignalId { get; set; }

        [StringLength(MaxSignalCodeLength)]
        public virtual string SignalCode { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? UpSignalDate { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? SubAssySignalDate { get; set; }

        [StringLength(MaxStatusLength)]
        public virtual string Status { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }
}
