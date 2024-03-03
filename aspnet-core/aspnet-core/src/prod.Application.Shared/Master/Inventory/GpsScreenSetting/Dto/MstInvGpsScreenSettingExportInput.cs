using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Inv.Dto
{

    public class MstInvGpsScreenSettingExportInput
    {

        public virtual string ScreenName { get; set; }

        public virtual string ScreenType { get; set; }

        public virtual string ScreenValue { get; set; }

        public virtual string Description { get; set; }

        public virtual int? BarcodeId { get; set; }

        public virtual string IsActive { get; set; }

    }

}


