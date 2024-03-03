using Abp.Application.Services.Dto;
using prod.Master.Common.DriveTrain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace prod.Master.Inventory.GpsCategory.Dto
{
    public class MstInvGpsCategoryDto : EntityDto<long?>
    {
        public virtual string Code { get; set; }

        public virtual string Name { get; set; }
    }

    public class CreateOrEditMstInvGpsCategoryDto : EntityDto<long?>
    {

        [StringLength(MstInvGpsCategoryConsts.MaxCodeLength)]
        public virtual string Code { get; set; }

        [StringLength(MstInvGpsCategoryConsts.MaxNameLength)]
        public virtual string Name { get; set; }
    }

    public class GetMstInvGpsCategoryInput : PagedAndSortedResultRequestDto
    {

        public virtual string Code { get; set; }

        public virtual string Name { get; set; }


    }
}
