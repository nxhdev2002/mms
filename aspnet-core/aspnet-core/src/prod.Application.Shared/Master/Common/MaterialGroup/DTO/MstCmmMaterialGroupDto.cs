using Abp.Application.Services.Dto;
namespace prod.Master.Common.Dto
{
    public class MstCmmMaterialGroupDto : EntityDto<long?>
    {
        public virtual string Code { get; set; }
        public virtual string Name { get; set; }
        public virtual string IsActive { get; set; }
    }

    public class GetMstCmmMaterialGroupInput : PagedAndSortedResultRequestDto
    {
        public virtual string Code { get; set; }
    }
}

