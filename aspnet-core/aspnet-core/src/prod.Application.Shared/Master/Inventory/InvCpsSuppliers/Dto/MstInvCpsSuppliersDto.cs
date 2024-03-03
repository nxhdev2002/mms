using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Inventory.Dto
{

    public class MstInvCpsSuppliersDto : EntityDto<long?>
    {

        public virtual string SupplierName { get; set; }

        public virtual string SupplierNumber { get; set; }

        public virtual string VatregistrationNum { get; set; }

        public virtual string VatregistrationInvoice { get; set; }

        public virtual string TaxPayerId { get; set; }

        public virtual long? RegistryId { get; set; }

        public virtual DateTime? StartDateActive { get; set; }

        public virtual DateTime? EndDateActive { get; set; }

        public virtual string IsActive { get; set; }

    }


    public class GetMstInvCpsSuppliersInput : PagedAndSortedResultRequestDto
    {
        public virtual string SupplierName { get; set; }
        public virtual string VatregistrationInvoice { get; set; }

    }

}


