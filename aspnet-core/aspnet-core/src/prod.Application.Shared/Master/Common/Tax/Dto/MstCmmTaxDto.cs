using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Cmm.Dto
{

    public class MstCmmTaxDto : EntityDto<long?>
    {

        public virtual string Code { get; set; }

        public virtual string Description { get; set; }

        public virtual string Rate { get; set; }

    }

    public class CreateOrEditMstCmmTaxDto : EntityDto<long?>
    {

        [StringLength(MstCmmTaxConsts.MaxCodeLength)]
        public virtual string Code { get; set; }

        [StringLength(MstCmmTaxConsts.MaxDescriptionLength)]
        public virtual string Description { get; set; }

        [StringLength(MstCmmTaxConsts.MaxRateLength)]
        public virtual string Rate { get; set; }
    }

    public class GetMstCmmTaxInput : PagedAndSortedResultRequestDto
    {

        public virtual string Code { get; set; }

        public virtual string Description { get; set; }

        public virtual string Rate { get; set; }

    }

}
