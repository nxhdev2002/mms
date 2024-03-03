using Abp.Application.Services.Dto;

namespace prod.Master.Inventory.ContainerDeliveryType.Dto
{

    public class MstInvContainerDeliveryTypeDto : EntityDto<long?>
    {

        public virtual string Code { get; set; }

        public virtual string Description { get; set; }

        public virtual string IsActive { get; set; }

    }

    public class GetMstInvContainerDeliveryTypeInput : PagedAndSortedResultRequestDto
    {

        public virtual string Code { get; set; }

        public virtual string Description { get; set; }

    }

    public class MstInvContainerDeliveryTypeExportInput
    {

        public virtual string Code { get; set; }

        public virtual string Description { get; set; }

    }
}