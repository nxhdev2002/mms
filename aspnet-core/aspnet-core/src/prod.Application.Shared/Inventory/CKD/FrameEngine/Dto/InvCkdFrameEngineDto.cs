using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.CKD.Dto
{
    public class InvCkdFrameEngineDto : EntityDto<long?>
    {
        public virtual string SupplierNo { get; set; }
        public virtual string BillofladingNo { get; set; }

        public virtual string InvoiceNo { get; set; }

        public virtual DateTime? InvoiceDate { get; set; }

        public virtual string LotCode { get; set; }

        public virtual string LotNo { get; set; }

        public virtual long? InvoiceId { get; set; }

        public virtual string FrameNo { get; set; }

        public virtual string EngineNo { get; set; }

        public virtual string PartNo { get; set; }

        public virtual string Module { get; set; }

    }


    public class GetInvCkdFrameEngineInput : PagedAndSortedResultRequestDto
    {
        public virtual long? InvoiceId { get; set; }
        public virtual string FrameEngineNo { get; set; }

        public virtual string Type { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string BillofladingNo { get; set; }

        public virtual string InvoiceNo { get; set; }

        public virtual DateTime? InvoiceDateFrom { get; set; }
        public virtual DateTime? InvoiceDateTo { get; set; }

        public virtual string OrderTypeCode { get; set; }

    }

}


