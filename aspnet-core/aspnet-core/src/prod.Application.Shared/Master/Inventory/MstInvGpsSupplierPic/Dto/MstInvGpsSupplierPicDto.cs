using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Inv.Dto
{

    public class MstInvGpsSupplierPicDto : EntityDto<long?>
    {

        public virtual long? SupplierId { get; set; }

        public virtual string PicName { get; set; }

        public virtual string PicTelephone { get; set; }

        public virtual string PicEmail { get; set; }

        public virtual string IsMainPic { get; set; }

        public virtual string IsSendEmail { get; set; }

        public virtual string IsActive { get; set; }

    }

    public class CreateOrEditMstInvGpsSupplierPicDto : EntityDto<long?>
    {

        public virtual long? SupplierId { get; set; }

        [StringLength(MstInvGpsSupplierPicConsts.MaxPicNameLength)]
        public virtual string PicName { get; set; }

        [StringLength(MstInvGpsSupplierPicConsts.MaxPicTelephoneLength)]
        public virtual string PicTelephone { get; set; }

        [StringLength(MstInvGpsSupplierPicConsts.MaxPicEmailLength)]
        public virtual string PicEmail { get; set; }

        [StringLength(MstInvGpsSupplierPicConsts.MaxIsMainPicLength)]
        public virtual string IsMainPic { get; set; }

        [StringLength(MstInvGpsSupplierPicConsts.MaxIsSendEmailLength)]
        public virtual string IsSendEmail { get; set; }

        [StringLength(MstInvGpsSupplierPicConsts.MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

    public class GetMstInvGpsSupplierPicInput : PagedAndSortedResultRequestDto
    {

        public virtual string PicName { get; set; }

        public virtual string PicTelephone { get; set; }

        public virtual string PicEmail { get; set; }
 

    }

    public class MstInvGpsSupplierPicExportInput
    {
        public virtual string PicName { get; set; }

        public virtual string PicTelephone { get; set; }

        public virtual string PicEmail { get; set; }

    }

}


