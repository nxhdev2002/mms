using Abp.Application.Services.Dto;
using prod.Master.Common.EngineModel;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Cmm.Dto
{

    public class MstCmmEngineModelDto : EntityDto<long?>
    {

        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

    }

    public class CreateOrEditMstCmmEngineModelDto : EntityDto<long?>
    {

        [StringLength(MstCmmEngineModelConsts.MaxCodeLength)]
        public virtual string Code { get; set; }

        [StringLength(MstCmmEngineModelConsts.MaxNameLength)]
        public virtual string Name { get; set; }
    }

    public class GetMstCmmEngineModelInput : PagedAndSortedResultRequestDto
    {

        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

    }

}