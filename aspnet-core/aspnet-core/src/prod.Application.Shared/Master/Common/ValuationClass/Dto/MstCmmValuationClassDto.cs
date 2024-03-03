using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Common.Dto
{

    public class MstCmmValuationClassDto : EntityDto<long?>
    {

        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

        public virtual string BsAccount { get; set; }

        public virtual string BsAccountDescription { get; set; }


    }

    public class GetMstCmmValuationClassInput : PagedAndSortedResultRequestDto
    {

        public virtual string Name { get; set; }

        public virtual string BsAccount { get; set; }

    }

}

