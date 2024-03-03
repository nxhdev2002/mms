using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Frame.Andon.Dto
{

    public class FrmAdoFramePlanBMPVDto : EntityDto<long?>
    {

        public virtual int? No { get; set; }

        public virtual string Model { get; set; }

        public virtual string LotNo { get; set; }

        public virtual int? NoInLot { get; set; }

        public virtual string BodyNo { get; set; }

        public virtual string Color { get; set; }

        public virtual string VinNo { get; set; }

        public virtual string FrameId { get; set; }

        public virtual string Status { get; set; }

        public virtual string PlanMonth { get; set; }

        public virtual DateTime? PlanDate { get; set; }

        public virtual string Grade { get; set; }

        public virtual string Version { get; set; }

        public virtual string IsActive { get; set; }

        public virtual string MessagesError { get; set; }

    }

    public class CreateOrEditFrmAdoFramePlanBMPVDto : EntityDto<long?>
    {

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
    }

    public class GetFrmAdoFramePlanBMPVInput : PagedAndSortedResultRequestDto
    {
        public virtual string Model { get; set; }
        public virtual string LotNo { get; set; }
        public virtual string BodyNo { get; set; }
        public virtual string VinNo { get; set; }  
        public virtual string PlanMonth { get; set; }

    }

    public class GetFrmAdoFramePlanBMPVExportInput
    {
        public virtual string Model { get; set; }
        public virtual string LotNo { get; set; }
        public virtual string BodyNo { get; set; }
        public virtual string VinNo { get; set; }
        public virtual string PlanMonth { get; set; }

    }

}

