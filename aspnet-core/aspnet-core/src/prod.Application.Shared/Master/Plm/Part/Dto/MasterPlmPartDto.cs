using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Plm.Dto
{

    public class MasterPlmPartDto : EntityDto<long?>
    {

        public virtual string PartName { get; set; }

        public virtual string PartCd { get; set; }

        public virtual int? R { get; set; }

        public virtual int? G { get; set; }

        public virtual int? B { get; set; }

    }

    public class CreateOrEditMasterPlmPartDto : EntityDto<long?>
    {

        [StringLength(MasterPlmPartConsts.MaxPartNameLength)]
        public virtual string PartName { get; set; }

        [StringLength(MasterPlmPartConsts.MaxPartCdLength)]
        public virtual string PartCd { get; set; }

        public virtual int? R { get; set; }

        public virtual int? G { get; set; }

        public virtual int? B { get; set; }
    }

    public class GetMasterPlmPartInput : PagedAndSortedResultRequestDto
    {

        public virtual string PartName { get; set; }

        public virtual string PartCd { get; set; }

        
    }

}


