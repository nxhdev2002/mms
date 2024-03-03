using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Plm.Dto
{

    public class MasterPlmPartExportInput
    {

        public virtual string PartName { get; set; }

        public virtual string PartCd { get; set; }

        public virtual int? R { get; set; }

        public virtual int? G { get; set; }

        public virtual int? B { get; set; }

    }

}