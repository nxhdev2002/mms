using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace prod.Master.Inventory.HrTitles.Dto
{
    public class MstInvHrTitlesDto : EntityDto<long?>
    {
        public virtual string Id_Code { get; set; }

        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual string IsActive { get; set; }
    }

    public class MstInvHrTitlesInput: PagedAndSortedResultRequestDto
    {
        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

    }

    public class MstInvHrTitlesExportInput
    {
        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

    }
}
