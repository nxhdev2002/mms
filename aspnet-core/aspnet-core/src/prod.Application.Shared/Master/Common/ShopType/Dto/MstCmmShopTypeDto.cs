using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Common.Dto
{

    public class MstCmmShopTypeDto : EntityDto<long?>
    {

        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

        public virtual string IsActive { get; set; }

    }

    public class CreateOrEditMstCmmShopTypeDto : EntityDto<long?>
    {

        [StringLength(MstCmmShopTypeConsts.MaxCodeLength)]
        public virtual string Code { get; set; }

        [StringLength(MstCmmShopTypeConsts.MaxNameLength)]
        public virtual string Name { get; set; }

        [StringLength(MstCmmShopTypeConsts.MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

    public class GetMstCmmShopTypeInput : PagedAndSortedResultRequestDto
    {

        public virtual string Code { get; set; }

        public virtual string Name { get; set; }


    }

}


