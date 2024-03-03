using Abp.Application.Services.Dto;
namespace prod.Master.Common.Dto
{
    public class MstCmmStorageLocationCategoryDto : EntityDto<long?>
    {
        public virtual string Code { get; set; }
        public virtual string Name { get; set; }
        public virtual string AreaType { get; set; }
        public virtual string IsActive { get; set; }
    }

    public class GetMstCmmStorageLocationCategoryInput : PagedAndSortedResultRequestDto
    {
        public virtual string Name { get; set; }
        public virtual string AreaType { get; set; }

    }
}

