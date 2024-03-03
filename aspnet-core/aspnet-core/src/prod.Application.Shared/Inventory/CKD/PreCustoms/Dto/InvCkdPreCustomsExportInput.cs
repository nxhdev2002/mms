using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.CKD.Dto
{

    public class InvCkdPreCustomsExportInput
    {

        public virtual string BillofladingNo { get; set; }

        public virtual decimal? BillDate { get; set; }

        public virtual decimal? TAX { get; set; }

        public virtual decimal? VAT { get; set; }

        public virtual string Description { get; set; }

    }
    public class InvCkdInvoiceExportInput
    {

        public virtual string SupplierNo { get; set; }

        public virtual string InvoiceNo { get; set; }

        public virtual decimal? Fob { get; set; }

        public virtual decimal? FreightTotal { get; set; }

        public virtual decimal? InsuranceTotal { get; set; }

        public virtual decimal? Cif { get; set; }

        public virtual string Currency { get; set; }

        public virtual string Description { get; set; }

    }

    public class InvCkdInvoiceDetailListExportInput : EntityDto<long?>
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

        public virtual string DeclareType { get; set; }


    }
}


