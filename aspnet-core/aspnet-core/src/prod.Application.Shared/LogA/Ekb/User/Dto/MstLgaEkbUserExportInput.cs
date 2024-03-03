using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.LogA.Dto
{

    public class MstLgaEkbUserExportInput
    {

        public virtual string UserCode { get; set; }

        public virtual string UserName { get; set; }

        public virtual int? ProcessId { get; set; }

        public virtual string ProcessCode { get; set; }

        public virtual string ProcessGroup { get; set; }

        public virtual string ProcessSubgroup { get; set; }

        public virtual string ProdLine { get; set; }

        public virtual string UserType { get; set; }

        public virtual int? LeadTime { get; set; }

        public virtual int? Sortingg { get; set; }

        public virtual string IsActive { get; set; }

    }

}

