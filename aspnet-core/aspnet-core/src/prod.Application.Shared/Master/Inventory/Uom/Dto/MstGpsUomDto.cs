using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Inventory.Dto
{

    public class MstGpsUomDto : EntityDto<long?>
    {

        public virtual string Code { get; set; }

        public virtual string Name { get; set; }


    }

    public class CreateOrEditMstGpsUomDto : EntityDto<long?>
    {

        [StringLength(MstGpsUomConsts.MaxCodeLength)]
        public virtual string Code { get; set; }

        [StringLength(MstGpsUomConsts.MaxNameLength)]
        public virtual string Name { get; set; }

    }

    public class GetMstGpsUomInput : PagedAndSortedResultRequestDto
    {

        public virtual string Code { get; set; }

        public virtual string Name { get; set; }


    }

}


