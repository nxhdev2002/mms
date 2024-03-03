using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.CKD.Dto
{

    public class MstCkdCustomsLeadtimeExportInput
    {

        public virtual string SupplierNo { get; set; }

        public virtual int Leadtime { get; set; }

    }

}


