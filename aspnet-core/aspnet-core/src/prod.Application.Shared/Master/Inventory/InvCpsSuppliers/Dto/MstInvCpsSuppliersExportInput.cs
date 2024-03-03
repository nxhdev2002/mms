using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Inventory.Dto
{

    public class MstInvCpsSuppliersExportInput
    {
        public virtual string SupplierName { get; set; }
        public virtual string VatregistrationInvoice { get; set; }

    }

}


