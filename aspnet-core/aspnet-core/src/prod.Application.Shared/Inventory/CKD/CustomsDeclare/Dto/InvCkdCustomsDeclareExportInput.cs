using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.CKD.Dto
{

    public class InvCkdCustomsDeclareExportInput
    {

        public virtual string CustomsDeclareNo { get; set; }

        public virtual string BillofladingNo { get; set; }

        public virtual decimal? TotalcifCept { get; set; }

        public virtual decimal? TotalcifNoncept { get; set; }

        public virtual decimal? TotaltaxCept { get; set; }

        public virtual decimal? TotaltaxNoncept { get; set; }

        public virtual decimal? TotalvatCept { get; set; }

        public virtual decimal? TotalvatNoncept { get; set; }

        public virtual decimal? ExchangeRate { get; set; }

        public virtual DateTime? DeclareDate { get; set; }

        public virtual decimal? ActualTax { get; set; }

        public virtual decimal? ActualVat { get; set; }

        public virtual string CustomsPort { get; set; }

        public virtual string TaxNote { get; set; }

        public virtual string Status { get; set; }

        public virtual DateTime? InputDate { get; set; }

        public virtual DateTime? ActualPaymentDate { get; set; }

        public virtual string OrdertypeCode { get; set; }

        public virtual decimal? CompleteTax { get; set; }

        public virtual decimal? CompleteVat { get; set; }

        public virtual string Forwarder { get; set; }

        public virtual string BusinessType { get; set; }

        public virtual int? NoPerGroup { get; set; }

        public virtual int? NumOfGroup { get; set; }

        public virtual int? Isvnaccs { get; set; }

        public virtual int? StatusCo { get; set; }

        public virtual int? IsFromEcus { get; set; }

        public virtual string DeclareType { get; set; }

        public virtual DateTime? CreatedDate { get; set; }

        public virtual string IsActive { get; set; }

    }

}


