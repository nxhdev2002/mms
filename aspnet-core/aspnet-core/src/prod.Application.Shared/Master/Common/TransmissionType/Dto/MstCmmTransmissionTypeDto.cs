using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Common.Dto
{

    public class MstCmmTransmissionTypeDto : EntityDto<long?>
    {

        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

        public virtual string IsActive { get; set; }

    }

    public class CreateOrEditMstCmmTransmissionTypeDto : EntityDto<long?>
    {

        [StringLength(MstCmmTransmissionTypeConsts.MaxCodeLength)]
        public virtual string Code { get; set; }

        [StringLength(MstCmmTransmissionTypeConsts.MaxNameLength)]
        public virtual string Name { get; set; }

        [StringLength(MstCmmTransmissionTypeConsts.MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

    public class GetMstCmmTransmissionTypeInput : PagedAndSortedResultRequestDto
    {

        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

       

    }

}


