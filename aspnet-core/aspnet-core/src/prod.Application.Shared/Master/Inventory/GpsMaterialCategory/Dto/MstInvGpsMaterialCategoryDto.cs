using Abp.Application.Services.Dto;
using prod.Master.Common;
using prod.Master.Inventory.GpsCategory;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace prod.Master.Inventory.GpsMaterialCategory.Dto
{

        public class MstInvGpsMaterialCategoryDto : EntityDto<long?>
        {

            public virtual string Name { get; set; }
             public virtual string IsActive { get; set; }
    }

        public class CreateOrEditMstInvGpsMaterialCategoryDto : EntityDto<long?>
        {


            [StringLength(MstInvGpsMaterialCategoryConsts.MaxNameLength)]
            public virtual string Name { get; set; }

            [StringLength(MstInvGpsMaterialCategoryConsts.MaxIsActiveLength)]
            public virtual string IsActive { get; set; }
    }

        public class GetMstInvGpsMaterialCategoryInput : PagedAndSortedResultRequestDto
        {


            public virtual string Name { get; set; }

    }
}
