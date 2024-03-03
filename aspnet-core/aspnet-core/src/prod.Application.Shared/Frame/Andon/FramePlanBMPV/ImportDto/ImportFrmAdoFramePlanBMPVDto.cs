using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Frame.Andon.Dto
{

    public class ImportFrmAdoFramePlanBMPVDto
    {
        [StringLength(FrmAdoFramePlanBMPVConsts.MaxGuidLength)]
        public virtual string Guid { get; set; }
        public virtual int? No { get; set; }

        [StringLength(FrmAdoFramePlanBMPVConsts.MaxModelLength)]
        public virtual string Model { get; set; }

        [StringLength(FrmAdoFramePlanBMPVConsts.MaxLotNoLength)]
        public virtual string LotNo { get; set; }

        public virtual int? NoInLot { get; set; }

        [StringLength(FrmAdoFramePlanBMPVConsts.MaxBodyNoLength)]
        public virtual string BodyNo { get; set; }

        [StringLength(FrmAdoFramePlanBMPVConsts.MaxColorLength)]
        public virtual string Color { get; set; }

        [StringLength(FrmAdoFramePlanBMPVConsts.MaxVinNoLength)]
        public virtual string VinNo { get; set; }

        [StringLength(FrmAdoFramePlanBMPVConsts.MaxFrameIdLength)]
        public virtual string FrameId { get; set; }

        [StringLength(FrmAdoFramePlanBMPVConsts.MaxStatusLength)]
        public virtual string Status { get; set; }

        [StringLength(FrmAdoFramePlanBMPVConsts.MaxPlanMonthLength)]
        public virtual string PlanMonth { get; set; }

        public virtual DateTime? PlanDate { get; set; }

        [StringLength(FrmAdoFramePlanBMPVConsts.MaxGradeLength)]
        public virtual string Grade { get; set; }

        [StringLength(FrmAdoFramePlanBMPVConsts.MaxVersionLength)]
        public virtual string Version { get; set; }

        [StringLength(FrmAdoFramePlanBMPVConsts.MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
        public string Exception { get; set; }
        public bool CanBeImported()
        {
            return string.IsNullOrEmpty(Exception);
        }
    }


}

