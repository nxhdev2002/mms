using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Common.Dto
{
    public class MstCmmStorageLocationDto : EntityDto<long?>
    {
        public virtual string PlantCode { get; set; }

        public virtual string PlantName { get; set; }

        public virtual string StorageLocation { get; set; }

        public virtual string StorageLocationName { get; set; }

        public virtual string AddressLanguageEn { get; set; }

        public virtual string AddressLanguageVn { get; set; }

        public virtual long CategoryId { get; set; }

        public virtual string Category { get; set; }

        public virtual string IsActive { get; set; }
    }

    public class GetMstCmmStorageLocationInput : PagedAndSortedResultRequestDto
    {
        public virtual string PlantName { get; set; }
        public virtual string StorageLocationName { get; set; }
        public virtual string AddressLanguageEn { get; set; }
        public virtual string Category { get; set; }

    }
}

