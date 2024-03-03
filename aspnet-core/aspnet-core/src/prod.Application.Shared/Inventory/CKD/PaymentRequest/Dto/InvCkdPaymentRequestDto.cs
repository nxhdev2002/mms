using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace prod.Inventory.CKD.Dto
{
    public class InvCkdPaymentRequestDto : EntityDto<long?>
    {
        public virtual long? Id { get; set; }

        public virtual DateTime? ReqDate { get; set; }

        public virtual string ReqPerson { get; set; }

        public virtual string IsFromEcus5 { get; set; }

        public virtual string ReqDepartment { get; set; }

        public virtual string Status { get; set; }

        public virtual string CustomsPort { get; set; }

        public virtual TimeSpan? Time { get; set; }

    }

    public class GetInvCkdPaymentRequestInput : PagedAndSortedResultRequestDto
    {
        public virtual string RequestNo { get; set; }

        public virtual string CustomsNo { get; set; }

        public virtual string CkdPio { get; set; }

        public virtual string OrderTypeCode { get; set; }

    }

    public class GetPaymentRequestExportInput
    {
        public virtual string RequestNo { get; set; }

        public virtual string CustomsNo { get; set; }

        public virtual string CkdPio { get; set; }

        public virtual string OrderTypeCode { get; set; }

    }

    public class GetCustomsDeclarePmExportInput : PagedAndSortedResultRequestDto
    {
        public virtual long? pid { get; set; }

    }

    //payment request
    public class InvCkdCustomsDeclarePmDto 
    {
        public virtual string InvoiceNo { get; set; }

        public virtual string ISPAID { get; set; }

        public virtual string OrdertypeCode { get; set; }

        public virtual decimal? ImpTax { get; set; }

        public virtual decimal ImpVat { get; set; }

        public virtual string CustomsDeclareNo { get; set; }

        public virtual DateTime? DeclareDate { get; set; }

        public virtual string BillofladingNo { get; set; }

        public virtual DateTime? BillDate { get; set; }
        public virtual decimal? Tax { get; set; }
        public virtual decimal? Vat { get; set; }

        public virtual int IsFromEcus { get; set; }

        public virtual string Description { get; set; }

        public virtual string CustomsPort { get; set; }

        public virtual string BusinessType { get; set; }
        public virtual DateTime? InputDate { get; set; }
        public virtual decimal? ExchangeRate { get; set; }

        public virtual string Forwarder { get; set; }
        public virtual decimal? ActualTax { get; set; }
        public virtual decimal? ActualVat { get; set; }

        public virtual decimal? Sum { get; set; }

        public virtual decimal? CompleteTax { get; set; }
        public virtual decimal? CompleteVat { get; set; }

        public virtual string TaxNote { get; set; }
    }


}
