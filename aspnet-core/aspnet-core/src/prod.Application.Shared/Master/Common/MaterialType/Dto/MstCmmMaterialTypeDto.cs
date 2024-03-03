using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace prod.Master.Common.Dto
{
    public class MstCmmMaterialTypeDto : EntityDto<long?>
    {
        public virtual string Code { get; set; }
        public virtual string Name { get; set; }
        public virtual string IsActive { get; set; }
    }

    public class CreateOrEditMstCmmMaterialTypeDto : EntityDto<long?>
    {

        [StringLength(MstCmmMaterialTypeConsts.MaxCodeLength)]
        public virtual string Code { get; set; }

        [StringLength(MstCmmMaterialTypeConsts.MaxNameLength)]
        public virtual string Name { get; set; }

        [StringLength(MstCmmMaterialTypeConsts.MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

    public class GetMstCmmMaterialTypeInput : PagedAndSortedResultRequestDto
    {
        public virtual string Code { get; set; }       
        public virtual string Name { get; set; }
        public virtual string IsActive { get; set; }

    }
}