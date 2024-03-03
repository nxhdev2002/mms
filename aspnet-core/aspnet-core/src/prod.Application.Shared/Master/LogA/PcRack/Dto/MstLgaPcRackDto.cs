
using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.LogA.Dto
{

    public class MstLgaPcRackDto : EntityDto<long?>
    {

        public virtual string Code { get; set; }

        public virtual string Address { get; set; }

        public virtual int? Ordering { get; set; }

        public virtual string IsActive { get; set; }

    }

    public class CreateOrEditMstLgaPcRackDto : EntityDto<long?>
    {

        [StringLength(MstLgaPcRackConsts.MaxCodeLength)]
        public virtual string Code { get; set; }

        [StringLength(MstLgaPcRackConsts.MaxAddressLength)]
        public virtual string Address { get; set; }

        public virtual int? Ordering { get; set; }

        [StringLength(MstLgaPcRackConsts.MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

    public class GetMstLgaPcRackInput : PagedAndSortedResultRequestDto
    {

        public virtual string Code { get; set; }

        public virtual string Address { get; set; }

    }

}


