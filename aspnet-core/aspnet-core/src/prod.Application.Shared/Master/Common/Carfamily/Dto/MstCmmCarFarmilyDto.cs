using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Common.Dto
{

    public class MstCmmCarfamilyDto : EntityDto<long?>
    {

        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

        public virtual string IsActive { get; set; }

    }

    public class CreateOrEditMstCmmCarfamilyDto : EntityDto<long?>
    {

        [StringLength(MstCmmCarfamilyConsts.MaxCodeLength)]
        public virtual string Code { get; set; }

        [StringLength(MstCmmCarfamilyConsts.MaxNameLength)]
        public virtual string Name { get; set; }

        [StringLength(MstCmmCarfamilyConsts.MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

    public class GetMstCmmCarfamilyInput : PagedAndSortedResultRequestDto
    {

        public virtual string Code { get; set; }


    }

}


