using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Common.Dto
{

    public class MstCmmUomDto : EntityDto<long?>
    {

        public virtual string Code { get; set; }

        public virtual string Name { get; set; }


    }

    public class CreateOrEditMstCmmUomDto : EntityDto<long?>
    {

        [StringLength(MstCmmUomConsts.MaxCodeLength)]
        public virtual string Code { get; set; }

        [StringLength(MstCmmUomConsts.MaxNameLength)]
        public virtual string Name { get; set; }

    }

    public class GetMstCmmUomInput : PagedAndSortedResultRequestDto
    {

        public virtual string Code { get; set; }

        public virtual string Name { get; set; }


    }

}


