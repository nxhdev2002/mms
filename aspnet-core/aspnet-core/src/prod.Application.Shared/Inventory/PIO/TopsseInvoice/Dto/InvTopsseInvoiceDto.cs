using Abp.Application.Services.Dto;
using System;
using System.Diagnostics;

namespace prod.Inventory.PIO.TopsseInvoice.Dto
{
    //Topsse Invoice
    public class InvTopsseInvoiceDto : EntityDto<long?>
    {
        public virtual string DistFd { get; set; }

        public virtual string InvoiceNo { get; set; }

        public virtual DateTime? InvoiceDate { get; set; }

        public virtual string BillOfLading { get; set; }

        public virtual DateTime? ProcessDate { get; set; }

        public virtual DateTime? Etd { get; set; }

        public virtual string Status { get; set; }
    }

    public class GetInvTopsseInvoiceInput : PagedAndSortedResultRequestDto
    {
        public virtual string InvoiceNo { get; set; }

        public virtual DateTime? InvoiceDate { get; set; }

        public virtual string OrderNo { get; set; }

        public virtual string PartNo { get; set; }

        public virtual string Status { get; set; }
    }

    public class GetInvTopsseInvoiceExportInput
    {
        public virtual string InvoiceNo { get; set; }

        public virtual DateTime? InvoiceDate { get; set; }

        public virtual string OrderNo { get; set; }

        public virtual string PartNo { get; set; }

        public virtual string Status { get; set; }
    }

    //Topsse Invoice Details
    public class InvTopsseInvoiceDetailsDto : EntityDto<long?>
    {
        public virtual string CaseNo { get; set; }

        public virtual string OrderNo { get; set; }

        public virtual string ItemNo { get; set; }

        public virtual string PartNo { get; set; }

        public virtual string PartName { get; set; }

        public virtual int? PartQty { get; set; }

        public virtual Decimal? UnitFob { get; set; }

        public virtual Decimal? Amount { get; set; }

        public virtual string TariffCd { get; set; }

        public virtual string HsCd { get; set; }

        public virtual int? ReceiveQty { get; set; }

        public virtual DateTime? ReceiveDate { get; set; }


        public virtual string KeyRow
        {
            get { return string.Format("{0}_{1}_{2}_{3}_{4}", CaseNo, OrderNo, ItemNo, PartNo, PartName); }
            set { }
        }

        public virtual int? RemainQty 
        {
            get { return ReceiveQty == null ? PartQty : (PartQty - ReceiveQty); } 
            set { }
        }

        public virtual int? ReceivedQuantity 
        {
            get { return ReceiveQty == null ? 0 : ReceiveQty; }
            set { }
        }
        public virtual int? ReceiveQuantity
        {
            get { return ReceiveQty == null ? PartQty : (PartQty - ReceiveQty); }
            set { }
        }

        public virtual Decimal? ReceiveAmount
        {
            get { return ReceiveQty == null ? Amount : (PartQty - ReceiveQty) * UnitFob; }
            set { }
        }

    }

    public class GetInvTopsseInvoiceDetailsInput : PagedAndSortedResultRequestDto
    {

        public virtual long? TopsseInvoiceId { get; set; }


    }

    public class GetInvTopsseInvoiceDetailsExportInput
    {

        public virtual long? TopsseInvoiceId { get; set; }

    }

    public class GetTopsseInvoiceDetailsAfterReceiveInput
    {

        public virtual string StringValue { get; set; }

        public virtual DateTime? ReceiveDate { get; set; }

    }
}
