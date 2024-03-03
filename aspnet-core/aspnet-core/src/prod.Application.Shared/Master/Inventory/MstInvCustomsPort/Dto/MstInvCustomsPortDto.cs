using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Inventory.Dto
{

    public class MstInvCustomsPortDto : EntityDto<long?>
    {

        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

        public virtual string AccountNumber { get; set; }

        public virtual string BankName { get; set; }

        public virtual string VendorNumber { get; set; }

        public virtual string IsActive { get; set; }

    }


    public class GetMstInvCustomsPortInput : PagedAndSortedResultRequestDto
    {

        public virtual string Code { get; set; }

        public virtual string Name { get; set; }


    }

    public class MstInvCustomsPortExportInput
    {

        public virtual string Code { get; set; }

        public virtual string Name { get; set; }


    }

}


