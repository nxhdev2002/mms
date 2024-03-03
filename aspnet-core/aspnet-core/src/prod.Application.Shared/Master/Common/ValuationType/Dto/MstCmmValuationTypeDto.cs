using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Common.Dto
{

    public class MstCmmValuationTypeDto : EntityDto<long?>
    {

        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

        //public virtual string Product { get; set; }

        //public virtual string MaterialTypeId { get; set; }

        //public virtual string MaterialType { get; set; }

        //public virtual string IsActive { get; set; }

    }
 
    public class GetMstCmmValuationTypeInput : PagedAndSortedResultRequestDto
    {

        public virtual string Code { get; set; }   
        //public virtual string MaterialType { get; set; }


    }

}

