using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Inv.Dto
{

    public class MstInvGpsTmvPicExportInput
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


