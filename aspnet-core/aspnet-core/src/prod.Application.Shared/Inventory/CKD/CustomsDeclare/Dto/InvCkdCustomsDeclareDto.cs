using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.CKD.Dto
{

    public class InvCkdCustomsDeclareDto : EntityDto<long?>
    {
        public virtual string CustomsDeclareNo { get; set; }

        public virtual DateTime? DeclareDate { get; set; }

        public virtual string BillofladingNo { get; set; }

        public virtual DateTime? BillDate { get; set; }
        public virtual decimal? Tax { get; set; }
        public virtual decimal? Vat { get; set; }

        public virtual string IsFromEcus { get; set; }

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

    public class CreateOrEditInvCkdCustomsDeclareDto : EntityDto<long?>
    {
        [StringLength(InvCkdCustomsDeclareConsts.MaxCustomsDeclareNoLength)]
        public virtual string CustomsDeclareNo { get; set; }

        public virtual DateTime? DeclareDate { get; set; }

        [StringLength(InvCkdCustomsDeclareConsts.MaxBillofladingNoLength)]
        public virtual string BillofladingNo { get; set; }

        public virtual DateTime? BillDate { get; set; }
        public virtual decimal? Tax { get; set; }
        public virtual decimal? Vat { get; set; }

        public virtual int IsFromEcus { get; set; }

        [StringLength(InvCkdCustomsDeclareConsts.MaxStatusLength)]
        public virtual string Description { get; set; }

        [StringLength(InvCkdCustomsDeclareConsts.MaxCustomsPortLength)]
        public virtual string CustomsPort { get; set; }

        [StringLength(InvCkdCustomsDeclareConsts.MaxBusinessTypeLength)]
        public virtual string BusinessType { get; set; }
        public virtual DateTime? InputDate { get; set; }
        public virtual decimal? ExchangeRate { get; set; }

        [StringLength(InvCkdCustomsDeclareConsts.MaxForwarderLength)]
        public virtual string Forwarder { get; set; }
        public virtual decimal? ActualTax { get; set; }
        public virtual decimal? ActualVat { get; set; }

        public virtual decimal? Sum { get; set; }

        public virtual decimal? CompleteTax { get; set; }
        public virtual decimal? CompleteVat { get; set; }

        [StringLength(InvCkdCustomsDeclareConsts.MaxTaxNoteLength)]
        public virtual string TaxNote { get; set; }


    }

    public class GetInvCkdCustomerDeclareInput : PagedAndSortedResultRequestDto
    {

        public virtual string CustomsDeclareNo { get; set; }

        public virtual DateTime? DeclareDate { get; set; }

        public virtual string BillofladingNo { get; set; }

        public virtual string InvoiceNo { get; set; }

        public virtual string CkdPio { get; set; }

        public virtual string OrderTypeCode { get; set; }

    }

    public class GetInvCkdPreCustomsListInput : PagedAndSortedResultRequestDto
    {
         
        public virtual long? CustomsDeclareId { get; set; }
        public virtual string BillNo { get; set; }


    }

    public class GetInvCkdInvoiceInput : PagedAndSortedResultRequestDto
    {

        public virtual long? PreCustomsId { get; set; }

    }
    public class PreCustomerDto : EntityDto<long?>
    {
        public virtual long? Id { get; set; }

        public virtual string PreNoGroup { get; set; }

        public virtual string InvoiceList { get; set; }

        public virtual string BillofladingNo { get; set; }

        public virtual decimal? Tax { get; set; }

        public virtual decimal? Vat { get; set; }

        public virtual string Description { get; set; }

    }
    public class InVoiceListDto :EntityDto<long?>  
    {
        public virtual string InvoiceNo { get; set; }
        public virtual string Fixlot { get; set; }
        public virtual string SupplierNo { get; set; }
        public virtual string PartNo { get; set; }
        public virtual string PartName { get; set; }
        public virtual int? Quantity { get; set; }
        public virtual decimal? Fob { get; set; }
        public virtual decimal? Freight { get; set; }
        public virtual decimal? Insurance { get; set; }
        public virtual decimal? Thc { get; set; }
        public virtual decimal? Cif { get; set; }
        public virtual string CeptType { get; set; }
        public virtual decimal? Tax { get; set; }
        public virtual decimal? Vat { get; set; }
        public virtual decimal? TaxRate { get; set; }
        public virtual decimal? VatRate { get; set; }

    }

}


