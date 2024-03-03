using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Common.Dto
{
    public class MstCmmMaterialMasterExportInput
    {
        public virtual string MaterialCode { get; set; }
        public virtual string MaterialGroup { get; set; }
        public virtual string ValuationType { get; set; }

    }
}


