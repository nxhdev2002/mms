using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Spp.Dto
{

    public class MstSppGlAccountExportInput
    {

        public virtual string GlAccountNo { get; set; }

        public virtual string GlType { get; set; }

        public virtual DateTime? StartDate { get; set; }

        public virtual DateTime? EndDate { get; set; }

    }

}


