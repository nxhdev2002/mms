using Abp.Application.Services.Dto;
namespace prod.Master.Common.Dto
{
    public class MstCmmPlantDto : EntityDto<long?>
    {
        public virtual string PlantCode { get; set; }

        public virtual string PlantName { get; set; }

        public virtual string BranchNo { get; set; }

        public virtual string AddressLanguageEn { get; set; }

        public virtual string AddressLanguageVn { get; set; }

        public virtual string IsActive { get; set; }

    }

    public class GetMstCmmPlantInput : PagedAndSortedResultRequestDto
    {
        public virtual string PlantName { get; set; }

        public virtual string BranchNo { get; set; }

        public virtual string AddressLanguageEn { get; set; }
    }
}


