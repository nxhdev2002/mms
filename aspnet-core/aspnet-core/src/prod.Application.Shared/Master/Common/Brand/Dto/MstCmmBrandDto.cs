
using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Common.Dto
{

    public class MstCmmBrandDto : EntityDto<long?>
    {

        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

        public virtual string IsActive { get; set; }
         

    }   

    public class CreateOrEditMstCmmBrandDto : EntityDto<long?>
    {

        [StringLength(MstCmmBrandConsts.MaxCodeLength)]
        public virtual string Code { get; set; }

        [StringLength(MstCmmBrandConsts.MaxNameLength)]
        public virtual string Name { get; set; }

        [StringLength(MstCmmBrandConsts.MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

    public class GetMstCmmBrandInput : PagedAndSortedResultRequestDto
    {
        public virtual string Code { get; set; }
        public virtual string Name { get; set; }

    }

}
