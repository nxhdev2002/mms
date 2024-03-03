using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Inv.Dto
{

    public class MstInvGpsTmvPicDto : EntityDto<long?>
    {

        public virtual string PicUserAccount { get; set; }

        public virtual string PicName { get; set; }

        public virtual string PicTelephone { get; set; }

        public virtual string PicEmail { get; set; }

        public virtual string IsMainPic { get; set; }

        public virtual string PicTelephone2 { get; set; }

        public virtual string Suppliers { get; set; }

        public virtual string IsActive { get; set; }

    }

    public class CreateOrEditMstInvGpsTmvPicDto : EntityDto<long?>
    {

        [StringLength(MstInvGpsTmvPicConsts.MaxPicUserAccountLength)]
        public virtual string PicUserAccount { get; set; }

        [StringLength(MstInvGpsTmvPicConsts.MaxPicNameLength)]
        public virtual string PicName { get; set; }

        [StringLength(MstInvGpsTmvPicConsts.MaxPicTelephoneLength)]
        public virtual string PicTelephone { get; set; }

        [StringLength(MstInvGpsTmvPicConsts.MaxPicEmailLength)]
        public virtual string PicEmail { get; set; }

        [StringLength(MstInvGpsTmvPicConsts.MaxIsMainPicLength)]
        public virtual string IsMainPic { get; set; }

        [StringLength(MstInvGpsTmvPicConsts.MaxPicTelephone2Length)]
        public virtual string PicTelephone2 { get; set; }

        [StringLength(MstInvGpsTmvPicConsts.MaxSuppliersLength)]
        public virtual string Suppliers { get; set; }

        [StringLength(MstInvGpsTmvPicConsts.MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

    public class GetMstInvGpsTmvPicInput : PagedAndSortedResultRequestDto
    {

        public virtual string PicUserAccount { get; set; }

        public virtual string PicName { get; set; }

        public virtual string PicTelephone { get; set; }

        public virtual string PicEmail { get; set; }

        public virtual string IsMainPic { get; set; }

        public virtual string PicTelephone2 { get; set; }

        public virtual string Suppliers { get; set; }

        public virtual string IsActive { get; set; }

    }

}


