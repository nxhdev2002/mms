using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Plm.Dto
{

    public class MstPlmLotCodeGradeDto : EntityDto<long?>
    {

        public virtual int? ModelId { get; set; }

        public virtual string LotCode { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string Grade { get; set; }

        public virtual int? Odering { get; set; }

        public virtual string GradeName { get; set; }

        public virtual string ModeCode { get; set; }

        public virtual string ModelVin { get; set; }

        public virtual int? VisStart { get; set; }

        public virtual int? VisEnd { get; set; }

        public virtual string MaLotCode { get; set; }

        public virtual string VehicleId { get; set; }

        public virtual string TestNo { get; set; }

        public virtual string Dab { get; set; }

        public virtual string Pab { get; set; }

        public virtual string EngCode { get; set; }

        public virtual string Lab { get; set; }

        public virtual string Rab { get; set; }

        public virtual string Kab { get; set; }

        public virtual int? IsFcLabel { get; set; }

        public virtual int? IsActive { get; set; }

        public virtual int? R { get; set; }

        public virtual int? G { get; set; }

        public virtual int? B { get; set; }

        public virtual string Clab { get; set; }

        public virtual string CharStr { get; set; }

    }

    public class CreateOrEditMstPlmLotCodeGradeDto : EntityDto<long?>
    {

        public virtual int? ModelId { get; set; }

        [StringLength(MstPlmLotCodeGradeConsts.MaxLotCodeLength)]
        public virtual string LotCode { get; set; }

        [StringLength(MstPlmLotCodeGradeConsts.MaxCfcLength)]
        public virtual string Cfc { get; set; }

        [StringLength(MstPlmLotCodeGradeConsts.MaxGradeLength)]
        public virtual string Grade { get; set; }

        public virtual int? Odering { get; set; }

        [StringLength(MstPlmLotCodeGradeConsts.MaxGradeNameLength)]
        public virtual string GradeName { get; set; }

        [StringLength(MstPlmLotCodeGradeConsts.MaxModeCodeLength)]
        public virtual string ModeCode { get; set; }

        [StringLength(MstPlmLotCodeGradeConsts.MaxModelVinLength)]
        public virtual string ModelVin { get; set; }

        public virtual int? VisStart { get; set; }

        public virtual int? VisEnd { get; set; }

        [StringLength(MstPlmLotCodeGradeConsts.MaxMaLotCodeLength)]
        public virtual string MaLotCode { get; set; }

        [StringLength(MstPlmLotCodeGradeConsts.MaxVehicleIdLength)]
        public virtual string VehicleId { get; set; }

        [StringLength(MstPlmLotCodeGradeConsts.MaxTestNoLength)]
        public virtual string TestNo { get; set; }

        [StringLength(MstPlmLotCodeGradeConsts.MaxDabLength)]
        public virtual string Dab { get; set; }

        [StringLength(MstPlmLotCodeGradeConsts.MaxPabLength)]
        public virtual string Pab { get; set; }

        [StringLength(MstPlmLotCodeGradeConsts.MaxEngCodeLength)]
        public virtual string EngCode { get; set; }

        [StringLength(MstPlmLotCodeGradeConsts.MaxLabLength)]
        public virtual string Lab { get; set; }

        [StringLength(MstPlmLotCodeGradeConsts.MaxRabLength)]
        public virtual string Rab { get; set; }

        [StringLength(MstPlmLotCodeGradeConsts.MaxKabLength)]
        public virtual string Kab { get; set; }

        public virtual int? IsFcLabel { get; set; }

        public virtual int? IsActive { get; set; }

        public virtual int? R { get; set; }

        public virtual int? G { get; set; }

        public virtual int? B { get; set; }

        [StringLength(MstPlmLotCodeGradeConsts.MaxClabLength)]
        public virtual string Clab { get; set; }

        [StringLength(MstPlmLotCodeGradeConsts.MaxCharStrLength)]
        public virtual string CharStr { get; set; }
    }

    public class GetMstPlmLotCodeGradeInput : PagedAndSortedResultRequestDto
    {

        public virtual string LotCode { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string Grade { get; set; }

        public virtual string ModeCode { get; set; }

        public virtual string ModelVin { get; set; }

        public virtual string MaLotCode { get; set; }

    }

}


