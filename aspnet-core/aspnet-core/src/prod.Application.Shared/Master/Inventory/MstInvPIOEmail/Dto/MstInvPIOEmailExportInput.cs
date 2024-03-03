using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Inventory.Dto
{

    public class MstInvPIOEmailExportInput
    {

        public virtual string Subject { get; set; }

        public virtual string To { get; set; }

        public virtual string Cc { get; set; }

        public virtual string BodyMess { get; set; }

        public virtual string SupplierCd { get; set; }

        public virtual string IsActive { get; set; }

    }

}


