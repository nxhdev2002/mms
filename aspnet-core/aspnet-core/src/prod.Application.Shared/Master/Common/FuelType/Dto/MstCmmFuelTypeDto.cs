using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Common.Dto
{

    public class MstCmmFuelTypeDto : EntityDto<long?>
    {

        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

        public virtual string IsActive { get; set; }

    }

    public class CreateOrEditMstCmmFuelTypeDto : EntityDto<long?>
    {

        [StringLength(MstCmmFuelTypeConsts.MaxCodeLength)]
        public virtual string Code { get; set; }

        [StringLength(MstCmmFuelTypeConsts.MaxNameLength)]
        public virtual string Name { get; set; }

        [StringLength(MstCmmFuelTypeConsts.MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

    public class GetMstCmmFuelTypeInput : PagedAndSortedResultRequestDto
    {

        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

        public virtual string IsActive { get; set; }

    }

}

