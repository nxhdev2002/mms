using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Common.Dto
{

    public class MstCmmVehicleNameDto : EntityDto<long?>
    {

        public virtual string Code { get; set; }

        public virtual string Name { get; set; }


    }

    public class GetMstCmmVehicleNameInput : PagedAndSortedResultRequestDto
    {

        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

    }

}