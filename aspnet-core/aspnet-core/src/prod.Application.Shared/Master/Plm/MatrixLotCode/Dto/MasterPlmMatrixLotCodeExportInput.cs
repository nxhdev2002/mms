using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Plm.Dto
{

    public class MasterPlmMatrixLotCodeExportInput
    {

        public virtual int? ScreenId { get; set; }

        public virtual int? LotcodeGradeId { get; set; }

        public virtual int? PartId { get; set; }

    }

}


