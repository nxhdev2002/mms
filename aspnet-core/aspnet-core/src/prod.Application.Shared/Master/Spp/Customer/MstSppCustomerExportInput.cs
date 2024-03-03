using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Spp.Dto
{

    public class MstSppCustomerExportInput
    {

        public virtual string Code { get; set; }

        public virtual DateTime? FromMonthYear { get; set; }
        public virtual DateTime? ToMonthYear { get; set; }

    }

}


