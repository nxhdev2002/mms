﻿using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Inventory.Dto
{

    public class MstInvInvoiceStatusDto : EntityDto<long?>
    {

        public virtual string Code { get; set; }

        public virtual string Description { get; set; }

        public virtual string IsActive { get; set; }

    }
}