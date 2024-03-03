using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.CPS.Dto
{

    public class InvCpsInvoiceLinesExportInput
    {

        public virtual long? InvoiceId { get; set; }

    }

}


