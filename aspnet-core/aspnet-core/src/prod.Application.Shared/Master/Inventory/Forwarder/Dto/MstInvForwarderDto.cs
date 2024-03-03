using Abp.Application.Services.Dto;
using System;
namespace prod.Master.Inventory.Forwarder.Dto
{

    public class MstInvForwarderDto : EntityDto<long?>
    {

        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string ShippingNo { get; set; }

        public virtual DateTime? EfDatefrom { get; set; }

        public virtual DateTime? EfDateto { get; set; }

        public virtual string IsActive { get; set; }

    }


    public class GetMstInvForwarderInput : PagedAndSortedResultRequestDto
    {

        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string ShippingNo { get; set; }

        public virtual DateTime? EfDatefrom { get; set; }

        public virtual DateTime? EfDateto { get; set; }


    }

    public class MstInvForwarderExportInput
    {
        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string ShippingNo { get; set; }

        public virtual DateTime? EfDatefrom { get; set; }

        public virtual DateTime? EfDateto { get; set; }


    }

}


