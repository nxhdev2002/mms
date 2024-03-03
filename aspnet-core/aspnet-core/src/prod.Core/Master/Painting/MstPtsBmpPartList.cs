using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Painting
{
    [Table("MstPtsBmpPartList")]
    [Index(nameof(Model), Name = "IX_MstPtsBmpPartList_Model")]
    [Index(nameof(BackNo), Name = "IX_MstPtsBmpPartList_BackNo")]
    [Index(nameof(ProdLine), Name = "IX_MstPtsBmpPartList_ProdLine")]
    [Index(nameof(PartTypeCode), Name = "IX_MstPtsBmpPartList_PartTypeCode")]
    [Index(nameof(IsActive), Name = "IX_MstPtsBmpPartList_IsActive")]
    public class MstPtsBmpPartList : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxModelLength = 50;

        public const int MaxCfcLength = 50;

        public const int MaxGradeLength = 50;

        public const int MaxBackNoLength = 50;

        public const int MaxProdLineLength = 50;

        public const int MaxPartTypeCodeLength = 50;

        public const int MaxProcessLength = 50;

        public const int MaxPkProcessLength = 50;

        public const int MaxIsPunchLength = 50;


        public const int MaSpecialColorLength = 50;

        public const int MaxSignalCodeLength = 50;

        public const int MaxRemarkLength = 500;

        public const int MaxIsActiveLength = 1;


        [StringLength(MaxModelLength)]
        public virtual string Model { get; set; }

        [StringLength(MaxCfcLength)]
        public virtual string Cfc { get; set; }

        [StringLength(MaxGradeLength)]
        public virtual string Grade { get; set; }

        [StringLength(MaxBackNoLength)]
        public virtual string BackNo { get; set; }

        [StringLength(MaxProdLineLength)]
        public virtual string ProdLine { get; set; }

        [StringLength(MaxPartTypeCodeLength)]
        public virtual string PartTypeCode { get; set; }

        public virtual long? PartTypeId { get; set; }

        [StringLength(MaxProcessLength)]
        public virtual string Process { get; set; }

        [StringLength(MaxPkProcessLength)]
        public virtual string PkProcess { get; set; }

        [StringLength(MaxIsPunchLength)]
        public virtual string IsPunch { get; set; }

        [StringLength(MaSpecialColorLength)]
        public virtual string SpecialColor { get; set; }

        public virtual int? SignalId { get; set; }

        [StringLength(MaxSignalCodeLength)]
        public virtual string SignalCode { get; set; }

        [StringLength(MaxRemarkLength)]
        public virtual string Remark { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }

    }

}




