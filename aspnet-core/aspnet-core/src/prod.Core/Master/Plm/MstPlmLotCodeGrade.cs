using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Plm
{
    [Table("MstPlmLotCodeGrade")]
    [Index(nameof(LotCode), Name = "IX_MstPlmLotCodeGrade_LotCode")]
    [Index(nameof(Cfc), Name = "IX_MstPlmLotCodeGrade_Cfc")]
    [Index(nameof(ModeCode), Name = "IX_MstPlmLotCodeGrade_ModeCode")]
    [Index(nameof(ModelVin), Name = "IX_MstPlmLotCodeGrade_ModelVin")]
    [Index(nameof(MaLotCode), Name = "IX_MstPlmLotCodeGrade_MaLotCode")]
    public class MstPlmLotCodeGrade : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxLotCodeLength = 50;

        public const int MaxCfcLength = 50;

        public const int MaxGradeLength = 50;

        public const int MaxGradeNameLength = 50;

        public const int MaxModeCodeLength = 20;

        public const int MaxModelVinLength = 20;

        public const int MaxMaLotCodeLength = 50;

        public const int MaxVehicleIdLength = 10;

        public const int MaxTestNoLength = 10;

        public const int MaxDabLength = 3;

        public const int MaxPabLength = 3;

        public const int MaxEngCodeLength = 3;

        public const int MaxLabLength = 255;

        public const int MaxRabLength = 255;

        public const int MaxKabLength = 255;

        public const int MaxClabLength = 255;

        public const int MaxCharStrLength = 255;

        public virtual int? ModelId { get; set; }

        [StringLength(MaxLotCodeLength)]
        public virtual string LotCode { get; set; }

        [StringLength(MaxCfcLength)]
        public virtual string Cfc { get; set; }

        [StringLength(MaxGradeLength)]
        public virtual string Grade { get; set; }

        public virtual int? Odering { get; set; }

        [StringLength(MaxGradeNameLength)]
        public virtual string GradeName { get; set; }

        [StringLength(MaxModeCodeLength)]
        public virtual string ModeCode { get; set; }

        [StringLength(MaxModelVinLength)]
        public virtual string ModelVin { get; set; }

        public virtual int? VisStart { get; set; }

        public virtual int? VisEnd { get; set; }

        [StringLength(MaxMaLotCodeLength)]
        public virtual string MaLotCode { get; set; }

        [StringLength(MaxVehicleIdLength)]
        public virtual string VehicleId { get; set; }

        [StringLength(MaxTestNoLength)]
        public virtual string TestNo { get; set; }

        [StringLength(MaxDabLength)]
        public virtual string Dab { get; set; }

        [StringLength(MaxPabLength)]
        public virtual string Pab { get; set; }

        [StringLength(MaxEngCodeLength)]
        public virtual string EngCode { get; set; }

        [StringLength(MaxLabLength)]
        public virtual string Lab { get; set; }

        [StringLength(MaxRabLength)]
        public virtual string Rab { get; set; }

        [StringLength(MaxKabLength)]
        public virtual string Kab { get; set; }

        public virtual int? IsFcLabel { get; set; }

        public virtual int? IsActive { get; set; }

        public virtual int? R { get; set; }

        public virtual int? G { get; set; }

        public virtual int? B { get; set; }

        [StringLength(MaxClabLength)]
        public virtual string Clab { get; set; }

        [StringLength(MaxCharStrLength)]
        public virtual string CharStr { get; set; }
    }

}
