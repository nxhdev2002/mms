using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.CPS.Dto
{
    public class InvCpsInvoiceHeadersExportInput
    {

        public virtual string PoNumber { get; set; }

        public virtual long? InventoryGroup { get; set; }


        public virtual long? Supplier { get; set; }

        public virtual DateTime? CreationTimeFrom { get; set; }


        public virtual DateTime? CreationTimeTo { get; set; }

        public virtual string InvoiceNo { get; set; }

        public virtual string InvoiceSymbol { get; set; }

        public virtual string PartNo { get; set; }


    }

}