using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.CKD.Dto
{

    public class InvCkdContainerInvoiceDto : EntityDto<long?>
    {

        public virtual string ContainerNo { get; set; }

        public virtual string Renban { get; set; }

        public virtual long? InvoiceId { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string SealNo { get; set; }

        public virtual int? ContainerSize { get; set; }

        public virtual DateTime? PlandedvanningDate { get; set; }

        public virtual DateTime? ActualvanningDate { get; set; }

        public virtual decimal? Thc { get; set; }

        public virtual decimal? Inland { get; set; }

        public virtual DateTime? CdDate { get; set; }

        public virtual string Status { get; set; }

        public virtual decimal? ThcVn { get; set; }

        public virtual decimal? InlandVn { get; set; }

        public virtual DateTime? PeriodDate { get; set; }

        public virtual long? PeriodId { get; set; }

        public virtual string DateStatus { get; set; }

        public virtual decimal? Fob { get; set; }

        public virtual decimal? Freight { get; set; }

        public virtual decimal? Insurance { get; set; }

        public virtual decimal? Tax { get; set; }

        public virtual decimal? Amount { get; set; }

        public virtual decimal? TaxVnd { get; set; }

        public virtual decimal? VatVnd { get; set; }

        public virtual string InvoiceNo { get; set; }

        public virtual string IsActive { get; set; }

        public virtual decimal? GrandFob { get; set; }

        public virtual decimal? GrandFreight { get; set; }

        public virtual decimal? GrandInsurance { get; set; }

        public virtual decimal? GrandTax { get; set; }

        public virtual decimal? GrandAmount { get; set; }

        public virtual long? GrandTaxVn { get; set; }

        public virtual long? GrandVatVn { get; set; }

        public virtual string BillOfLadingNo { get; set; }

        public virtual DateTime? BillDate { get; set; }

    }

    public class CreateOrEditInvCkdContainerInvoiceDto : EntityDto<long?>
    {

        [StringLength(InvCkdContainerInvoiceConsts.MaxContainerNoLength)]
        public virtual string ContainerNo { get; set; }

        [StringLength(InvCkdContainerInvoiceConsts.MaxRenbanLength)]
        public virtual string Renban { get; set; }

        public virtual long? InvoiceId { get; set; }

        [StringLength(InvCkdContainerInvoiceConsts.MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        [StringLength(InvCkdContainerInvoiceConsts.MaxSealNoLength)]
        public virtual string SealNo { get; set; }

        public virtual int? ContainerSize { get; set; }

        public virtual DateTime? PlandedvanningDate { get; set; }

        public virtual DateTime? ActualvanningDate { get; set; }

        public virtual decimal? Thc { get; set; }

        public virtual decimal? Inland { get; set; }

        public virtual DateTime? CdDate { get; set; }

        [StringLength(InvCkdContainerInvoiceConsts.MaxStatusLength)]
        public virtual string Status { get; set; }

        public virtual decimal? ThcVn { get; set; }

        public virtual decimal? InlandVn { get; set; }

        public virtual DateTime? PeriodDate { get; set; }

        public virtual long? PeriodId { get; set; }

        [StringLength(InvCkdContainerInvoiceConsts.MaxDateStatusLength)]
        public virtual string DateStatus { get; set; }

        public virtual decimal? Fob { get; set; }

        public virtual decimal? Freight { get; set; }

        public virtual decimal? Insurance { get; set; }

        public virtual decimal? Amount { get; set; }

        public virtual string InvoiceNo { get; set; }

        [StringLength(InvCkdContainerInvoiceConsts.MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

    public class InvCkdContainerInvoiceViewCustomsDto : EntityDto<long?>
    {

        public virtual long? ContInvId { get; set; }

        public virtual long? InvoiceId { get; set; }

        public virtual long? BillId { get; set; }

        public virtual string ContainerNo { get; set; }

        public virtual string Renban { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string InvoiceNo { get; set; }

        public virtual DateTime? DeclareDate { get; set; }

        public virtual string CustomsDeclareNo { get; set; }

        public virtual float? Taxvnd { get; set; }

        public virtual string Currency { get; set; }

        public virtual string ExchangeRate { get; set; }

        public virtual string BillOfladingNo { get; set; }

        public virtual string Description { get; set; }
    }

    public class InvCkdContainerInvoiceCustomsDto : EntityDto<long?>
    {

        public virtual long? ContInvId { get; set; }

        public virtual long? InvoiceId { get; set; }

        public virtual long? BillId { get; set; }

        public virtual string ContainerNo { get; set; }

        public virtual string Renban { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string InvoiceNo { get; set; }

        public virtual DateTime? DeclareDate { get; set; }

        public virtual string CustomsDeclareNo { get; set; }

        public virtual float? Taxvnd { get; set; }

        public virtual string Currency { get; set; }

        public virtual string ExchangeRate { get; set; }

        public virtual string BillOfladingNo { get; set; }

        public virtual string Description { get; set; }
    }

    public class GetInvCkdContainerInvoiceInput : PagedAndSortedResultRequestDto
    {
        public virtual string BillofladingNo { get; set; }
        public virtual string ContainerNo { get; set; }

        public virtual string Renban { get; set; }

        public virtual string InvoiceNo { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string SealNo { get; set; }

        public virtual string Status { get; set; }

        public virtual DateTime? BillDateFrom { get; set; }
        public virtual DateTime? BillDateTo { get; set; }

        public virtual string CkdPio { get; set; }
        public virtual string OrderTypeCode { get; set; }


    }

    public class GetInvCkdContainerInvoiceInputExcel
    {
        public virtual string BillofladingNo { get; set; }
        public virtual string ContainerNo { get; set; }

        public virtual string Renban { get; set; }

        public virtual string InvoiceNo { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string SealNo { get; set; }

        public virtual string Status { get; set; }

        public virtual DateTime? BillDateFrom { get; set; }
        public virtual DateTime? BillDateTo { get; set; }

        public virtual string CkdPio { get; set; }

        public virtual string OrderTypeCode { get; set; }
    }

    public class GetViewCustomInput : PagedAndSortedResultRequestDto
    {
        public virtual long? Id { get; set; }
    }

    public class GetViewCustomExcelInput
    {
        public virtual long? Id { get; set; }
    }
}


