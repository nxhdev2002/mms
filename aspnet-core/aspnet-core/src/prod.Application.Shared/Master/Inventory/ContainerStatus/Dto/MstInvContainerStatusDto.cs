using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Inventory.ContainerStatus.Dto
{

    public class MstInvContainerStatusDto : EntityDto<long?>
    {

        public virtual string Code { get; set; }

        public virtual string Description { get; set; }

        public virtual string DescriptionVn { get; set; }

        public virtual string IsActive { get; set; }

    }


    public class GetMstInvContainerStatusInput : PagedAndSortedResultRequestDto
    {

        public virtual string Code { get; set; }

        public virtual string Description { get; set; }


    }

     public class MstInvContainerStatusExportInput
    {
        public virtual string Code { get; set; }

        public virtual string Description { get; set; }

    }
}


