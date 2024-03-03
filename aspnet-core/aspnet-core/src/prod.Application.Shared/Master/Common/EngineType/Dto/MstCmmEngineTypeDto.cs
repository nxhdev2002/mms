using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Cmm.Dto
{

    public class MstCmmEngineTypeDto : EntityDto<long?>
    {

        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

    }

    public class CreateOrEditMstCmmEngineTypeDto : EntityDto<long?>
    {

        [StringLength(MstCmmEngineTypeConsts.MaxCodeLength)]
        public virtual string Code { get; set; }

        [StringLength(MstCmmEngineTypeConsts.MaxNameLength)]
        public virtual string Name { get; set; }
    }

    public class GetMstCmmEngineTypeInput : PagedAndSortedResultRequestDto
    {

        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

    }

}

