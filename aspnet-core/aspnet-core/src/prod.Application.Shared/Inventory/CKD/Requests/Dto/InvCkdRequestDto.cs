using Abp.Application.Services.Dto;
using System;

namespace prod.Inventory.CKD.Dto
{
    public class InvCkdRequestDto : EntityDto<long?>
    {
        public virtual DateTime? ReqDate { get; set; }
        public virtual string CkdReqNo { get; set; }
        public virtual DateTime? IssueDate { get; set; }
        public virtual string Version { get; set; }
        public virtual string Status { get; set; }

    }

    public class InvCkdRequestContentByLotDto : EntityDto<long?>
    {
        public virtual string Dock { get; set; }
        public virtual string TimeRequest { get; set; }
        public virtual string Grade { get; set; }
        public virtual string LotNo { get; set; }
        public virtual string Robbing { get; set; }
        public virtual string Remarks { get; set; }

    }


    public class InvCkdRequestContentByPxPDto : EntityDto<long?>
    {
        public virtual string PartNo { get; set; }
        public virtual string PartName { get; set; }
        public virtual string Source { get; set; }
        public virtual string Model { get; set; }
        public virtual  string StockendofReqDate { get; set; }
        public virtual int? MinstockAtTmv { get; set; }
        public virtual int? Balance { get; set; }
        public virtual int? ReqQuantity { get; set; }
        public virtual string TimeRequest { get; set; }
        public virtual string Remarks { get; set; }
    }


    public class GetInvCkdDeliveryScheGetByReqByMakeDto : EntityDto<long?>
    {
        public virtual string Shop { get; set; }
        public virtual string Status { get; set; }
        public virtual string TimeRequest { get; set; }
        public virtual string  PcdTimeRequest { get; set; }
        public virtual string ContainerNo { get; set; }
        public virtual string Renban { get; set; }
        public virtual string LotNo { get; set; }
        public virtual string LotFollow { get; set; }
        public virtual int? ContainerSize { get; set; }
        public virtual string ShippingcompanyCode { get; set; }
        public virtual string ReturnType { get; set; }
        public virtual string BillofladingNo { get; set; }
        public virtual string InvoiceNo { get; set; }
        public virtual string ListLotNo { get; set; }
        public virtual string ListcaseNo { get; set; }
        public virtual string Source { get; set; }
        public virtual string Transporter { get; set; }
        public virtual DateTime? CdDate { get; set; }
        public virtual DateTime? DevanningDate { get; set; }
        public virtual DateTime? ActualDevanningDate { get; set; }
        public virtual string ActualDevanningTime { get; set; }
        public virtual string ContStatus { get; set; }
        public virtual string PartNo { get; set; }
        public virtual string PartName { get; set; }
        public virtual string DevanningTime { get; set; }
    }



    public class GetInvCkdDeliveryScheGetByReqByCallDto : EntityDto<long?>
    {
        public virtual string Shop { get; set; }
        public virtual string Status { get; set; }
        public virtual string TimeRequest { get; set; }
        public virtual string ContainerNo { get; set; }
        public virtual string Renban { get; set; }
        public virtual string LotNo { get; set; }
        public virtual string LotFollow { get; set; }
        public virtual string BillofladingNo { get; set; }
        public virtual string InvoiceNo { get; set; }
        public virtual string ListLotNo { get; set; }
        public virtual string ListcaseNo { get; set; }
        public virtual string Source { get; set; }
        public virtual string Transport { get; set; }
        public virtual DateTime? CdDate { get; set; }
        public virtual string ReturnType { get; set; }
        public virtual DateTime? DevanningDate { get; set; }
        public virtual string DevanningTime { get; set; }
        public virtual DateTime? ActualDevanningDate { get; set; }
        public virtual string ActualDevanningTime { get; set; }
        public virtual string ContStatus { get; set; }
    }




    public class GetInvCkdRequestInput : PagedAndSortedResultRequestDto
    {

        public virtual string p_request_no { get; set; }
        public virtual DateTime? request_date_from { get; set; }
        public virtual DateTime? request_date_to { get; set; }
        public virtual string p_status { get; set;} 
        public virtual string p_container_no { get; set; }
        public virtual string p_reban { get; set; }
        public virtual string p_invoice_no { get; set; }



    }
    public class GetInvCkdRequestInputExcel
    {

        public virtual string p_request_no { get; set; }
        public virtual DateTime? request_date_from { get; set; }
        public virtual DateTime? request_date_to { get; set; }
        public virtual string p_status { get; set; }
        public virtual string p_container_no { get; set; }
        public virtual string p_reban { get; set; }
        public virtual string p_invoice_no { get; set; }

    }

    public class GetInvCkdRequestDetailInput : PagedAndSortedResultRequestDto
    {

        public virtual int? p_ckd_req_id { get; set; }
    }

    public class GetCkdRequestDetailInput : PagedAndSortedResultRequestDto
    {

        public virtual int? p_ckd_req_id { get; set; }
        public virtual string p_containerno { get; set; }
        public virtual string p_renban { get; set; }
        public virtual string p_invoiceno { get; set; }
    }



    public class GetInvCkdRequestDetailInputExcel
    {

        public virtual int? p_ckd_req_id { get; set; }
        public virtual string p_containerno { get; set; }
        public virtual string p_renban { get; set; }
        public virtual string p_invoiceno { get; set; }

    }
    public class GetInvCkdRequestDetailByInputExcel
    {
        public virtual int? p_ckd_req_id { get; set; }

    }
}
