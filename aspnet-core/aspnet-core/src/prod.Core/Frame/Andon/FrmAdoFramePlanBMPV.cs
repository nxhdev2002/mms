using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Frame.Andon
{

    [Table("FrmAdoFramePlanBMPV")]
    [Index(nameof(No), Name = "IX_FrmAdoFramePlanBMPV_No")]
    [Index(nameof(LotNo), Name = "IX_FrmAdoFramePlanBMPV_LotNo")]
    [Index(nameof(NoInLot), Name = "IX_FrmAdoFramePlanBMPV_NoInLot")]
    [Index(nameof(FrameId), Name = "IX_FrmAdoFramePlanBMPV_FrameId")]
    [Index(nameof(PlanMonth), Name = "IX_FrmAdoFramePlanBMPV_PlanMonth")]
    [Index(nameof(PlanDate), Name = "IX_FrmAdoFramePlanBMPV1_PlanDate")]
    public class FrmAdoFramePlanBMPV : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxModelLength = 50;

        public const int MaxLotNoLength = 50;

        public const int MaxBodyNoLength = 50;

        public const int MaxColorLength = 50;

        public const int MaxVinNoLength = 50;

        public const int MaxFrameIdLength = 50;

        public const int MaxStatusLength = 50;

        public const int MaxPlanMonthLength = 50;

        public const int MaxGradeLength = 50;

        public const int MaxVersionLength = 50;

        public const int MaxIsActiveLength = 50;

        public virtual int? No { get; set; }

        [StringLength(MaxModelLength)]
        public virtual string Model { get; set; }

        [StringLength(MaxLotNoLength)]
        public virtual string LotNo { get; set; }

        public virtual int? NoInLot { get; set; }

        [StringLength(MaxBodyNoLength)]
        public virtual string BodyNo { get; set; }

        [StringLength(MaxColorLength)]
        public virtual string Color { get; set; }

        [StringLength(MaxVinNoLength)]
        public virtual string VinNo { get; set; }

        [StringLength(MaxFrameIdLength)]
        public virtual string FrameId { get; set; }

        [StringLength(MaxStatusLength)]
        public virtual string Status { get; set; }

        [StringLength(MaxPlanMonthLength)]
        public virtual string PlanMonth { get; set; }

        [Column(TypeName = "date")]
        public virtual DateTime? PlanDate { get; set; }

        [StringLength(MaxGradeLength)]
        public virtual string Grade { get; set; }

        [StringLength(MaxVersionLength)]
        public virtual string Version { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}

