using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Cmm.Dto
{

    public class MstCmmModelDto : EntityDto<long?>
    {

        public virtual string Code { get; set; }


        public virtual string Name { get; set; }

        public virtual string IsActive { get; set; }

        public List<MstCmmLotCodeGradeDto> ListLotCodeGrade { get; set; }

    }

    public class CreateOrEditMstCmmModelDto : EntityDto<long?>
    {

        [StringLength(MstCmmModelConsts.MaxCodeLength)]
        public virtual string Code { get; set; }

        [StringLength(MstCmmModelConsts.MaxNameLength)]
        public virtual string Name { get; set; }

        [StringLength(MstCmmModelConsts.MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

    public class GetMstCmmModelInput : PagedAndSortedResultRequestDto
    {
        public virtual string Cfc { get; set; }

        public virtual string ModelVin { get; set; }

        public virtual string ModelCode { get; set; }

    }

}