using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Plm.Dto
{

    public class MasterPlmMatrixExportInput
    {

        public virtual int? ScreenId { get; set; }

        public virtual int? PartId { get; set; }

        public virtual int? Ordering { get; set; }

        public virtual int? SideId { get; set; }

    }

}


