using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Inventory.Dto
{

    public class MstInvPIOPartTypeDto : EntityDto<long?>
    {

        public virtual string Code { get; set; }

        public virtual string Description { get; set; }

    }

    public class CreateOrEditMstInvPIOPartTypeDto : EntityDto<long?>
    {

        [StringLength(MstInvPIOPartTypeConsts.MaxCodeLength)]
        public virtual string Code { get; set; }

        [StringLength(MstInvPIOPartTypeConsts.MaxDescriptionLength)]
        public virtual string Description { get; set; }
    }

    public class GetMstInvPIOPartTypeInput : PagedAndSortedResultRequestDto
    {

        public virtual string Code { get; set; }

        public virtual string Description { get; set; }

    }

}


