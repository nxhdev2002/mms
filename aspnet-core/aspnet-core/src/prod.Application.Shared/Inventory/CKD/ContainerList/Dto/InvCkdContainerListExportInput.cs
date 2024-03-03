using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.CKD.Dto
{
    public class InvCkdContainerListExportInput
    {
        public virtual string ContainerNo { get; set; }

        public virtual string Renban { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string HaisenNo { get; set; }

        public virtual string BillOfLadingNo { get; set; }

        public virtual string InvoiceNo { get; set; }

        public virtual string OrderTypeCode { get; set; }

        public virtual DateTime? PortDateFrom { get; set; }

        public virtual DateTime? PortDateTo { get; set; }

        public virtual DateTime? ReceiveDateFrom { get; set; }

        public virtual DateTime? ReceiveDateTo { get; set; }

        public virtual string GoodsTypeCode { get; set; }

        public virtual string radio { get; set; }

        public virtual DateTime? BillDateFrom { get; set; }
        public virtual DateTime? BillDateTo { get; set; }

        public virtual string CkdPio { get; set; }

        public virtual string LotNo { get; set; }

    }
    public class ShipmentInfoDetailExportInput
    {
        public virtual string Id { get; set; }
  

    }

}


