using Abp.Application.Services.Dto;
using prod.Master.Inventory.MstInvCpsInventoryGroups;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Inventory.Dto
{

    public class MstInvCpsInventoryGroupDto : EntityDto<long?>
    {

        public virtual string Productgroupcode { get; set; }

        public virtual string Productgroupname { get; set; }

        public virtual string IsActive { get; set; }

    }


    public class GetMstInvCpsInventoryGroupInput : PagedAndSortedResultRequestDto
    {

        public virtual string Productgroupcode { get; set; }

        public virtual string Productgroupname { get; set; }

    }

}

