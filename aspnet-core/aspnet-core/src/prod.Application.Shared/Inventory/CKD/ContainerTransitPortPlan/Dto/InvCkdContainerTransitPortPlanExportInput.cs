using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.CKD.Dto
{
    public class InvCkdContainerTransitPortPlanExportInput
    {
        public virtual string ContainerNo { get; set; }

        public virtual string Renban { get; set; }

        public virtual string InvoiceNo { get; set; }

        public virtual string BillOfLadingNo { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string SealNo { get; set; }
    }
}


