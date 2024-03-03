using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Inventory.Dto
{

    public class MstInvShippingCompanyDto : EntityDto<long?>
    {

        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual string IsActive { get; set; }

    }


    public class GetMstInvShippingCompanyInput : PagedAndSortedResultRequestDto
    {

        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

    }

    public class MstInvShippingCompanyExportInput 
    {

        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

    }

}


