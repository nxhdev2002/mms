using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.CKD.Dto
{
    public class InvCkdStockReceivingExportInput
    {
        public virtual string PartNo { get; set; }
        public virtual string Cfc { get; set; }
        public virtual string SupplierNo { get; set; }
        public virtual DateTime? WorkingDateFrom { get; set; }
        public virtual DateTime? WorkingDateTo { get; set; }

    }

}


