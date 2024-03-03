using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Plm.Dto
{

    public class MasterPlmMatrixLotCodeDto : EntityDto<long?>
    {

        public virtual int? ScreenId { get; set; }

        public virtual int? LotcodeGradeId { get; set; }

        public virtual int? PartId { get; set; }

    }

    public class CreateOrEditMasterPlmMatrixLotCodeDto : EntityDto<long?>
    {

        public virtual int? ScreenId { get; set; }

        public virtual int? LotcodeGradeId { get; set; }

        public virtual int? PartId { get; set; }
    }

    public class GetMasterPlmMatrixLotCodeInput : PagedAndSortedResultRequestDto
    {


    }

}


