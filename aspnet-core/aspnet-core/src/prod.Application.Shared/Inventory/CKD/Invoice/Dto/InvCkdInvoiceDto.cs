using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using prod.Master.Cmm;
using prod.Master.Cmm.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace prod.Inventory.Invoice.Dto
{
    public class InvCkdInvoiceDto : EntityDto<long?>
    {


        [StringLength(InvCkdInvoiceConsts.MaxInvoiceNoLength)]
        public virtual string InvoiceNo { get; set; }

        [StringLength(InvCkdInvoiceConsts.MaxShipmentNoLength)]
        public virtual string ShipmentNo { get; set; }

        public virtual long? BillId { get; set; }

        [StringLength(InvCkdInvoiceConsts.MaxOrdertypeCodeLength)]
        public virtual string OrdertypeCode { get; set; }

        [StringLength(InvCkdInvoiceConsts.MaxBillNoLength)]
        public virtual string BillNo { get; set; }

        [StringLength(InvCkdInvoiceConsts.MaxGoodstypeCodeLength)]
        public virtual string GoodstypeCode { get; set; }

        public virtual string InvoiceParentNo { get; set; }

        public virtual DateTime? InvoiceDate { get; set; }

        public virtual decimal? Fob { get; set; }

        public virtual decimal? Freight { get; set; }

        public virtual decimal? FreightTotal { get; set; }

        public virtual decimal? Insurance { get; set; }

        public virtual decimal? InsuranceTotal { get; set; }

        public virtual decimal? Cif { get; set; }

        public virtual decimal? ThcTotal { get; set; }

        public virtual decimal? NetWeight { get; set; }

        public virtual decimal? GrossWeight { get; set; }

        [StringLength(InvCkdInvoiceConsts.MaxCeptTypeLength)]
        public virtual string CeptType { get; set; }

        [StringLength(InvCkdInvoiceConsts.MaxCurrencyLength)]
        public virtual string Currency { get; set; }

        [StringLength(InvCkdInvoiceConsts.MaxRemarksLength)]
        public virtual string Remarks { get; set; }

        [StringLength(InvCkdInvoiceConsts.MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        public virtual int? Quantity { get; set; }

        public virtual int? Flag { get; set; }

        public virtual int? Freezed { get; set; }

        [StringLength(InvCkdInvoiceConsts.MaxSourceTypeLength)]
        public virtual string SourceType { get; set; }

        [StringLength(InvCkdInvoiceConsts.MaxStatusErrLength)]
        public virtual string StatusErr { get; set; }

        [StringLength(InvCkdInvoiceConsts.MaxLastOrdertypeLength)]
        public virtual string LastOrdertype { get; set; }

        [StringLength(InvCkdInvoiceConsts.MaxStatusLength)]
        public virtual string Description { get; set; }

        public virtual decimal? FobVn { get; set; }

        public virtual decimal? FreightTotalVn { get; set; }

        public virtual decimal? InsuranceTotalVn { get; set; }

        public virtual decimal? CifVn { get; set; }

        public virtual decimal? ThcTotalVn { get; set; }

        [StringLength(InvCkdInvoiceConsts.MaxSpotaxLength)]
        public virtual string Spotax { get; set; }

        public virtual long? PeriodId { get; set; }

        [StringLength(InvCkdInvoiceConsts.MaxIsActiveLength)]
        public virtual string IsActive { get; set; }

        public virtual string Status { get; set; }

        public virtual DateTime? BillDate { get; set; }
    }



    public class InvCkdInvoiceOutputDto : EntityDto<long?>
    { 
        public virtual string InvoiceNo { get; set; }

        public virtual string ShipmentNo { get; set; }

        public virtual long? BillId { get; set; }

        public virtual string OrdertypeCode { get; set; }

        public virtual string GoodstypeCode { get; set; }

        public virtual long? InvoiceParentId { get; set; }

        public virtual DateTime? InvoiceDate { get; set; }

        public virtual decimal? Fob { get; set; }

        public virtual decimal? Freight { get; set; }

        public virtual decimal? FreightTotal { get; set; }

        public virtual decimal? Insurance { get; set; }

        public virtual decimal? InsuranceTotal { get; set; }

        public virtual decimal? Cif { get; set; }

        public virtual decimal? ThcTotal { get; set; }

        public virtual decimal? NetWeight { get; set; }

        public virtual decimal? GrossWeight { get; set; }

        public virtual string CeptType { get; set; }

        public virtual string Currency { get; set; }

        public virtual string Remarks { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual int? Quantity { get; set; }

        public virtual int? Flag { get; set; }

        public virtual int? Freezed { get; set; }

        public virtual string SourceType { get; set; }

        public virtual string StatusErr { get; set; }

        public virtual string LastOrdertype { get; set; }

        public virtual string Status { get; set; }

        public virtual decimal? FobVn { get; set; }

        public virtual decimal? FreightTotalVn { get; set; }

        public virtual decimal? InsuranceTotalVn { get; set; }

        public virtual decimal? CifVn { get; set; }

        public virtual decimal? ThcTotalVn { get; set; }

        public virtual string Spotax { get; set; }

        public virtual long? PeriodId { get; set; }

        public virtual string IsActive { get; set; }
    }

    public class GetInvCkdInvoiceDtolInput : PagedAndSortedResultRequestDto
    {
        public virtual string InvoiceNo { get; set; }
        public virtual DateTime? InvoiceDateFrom { get; set; }
        public virtual DateTime? InvoiceDateTo { get; set; }
        public virtual string BillNo { get; set; }
        public virtual string ShipmentNo { get; set; }
        public virtual string ContainerNo { get; set; }
        public virtual DateTime? BillDateFrom { get; set; }
        public virtual DateTime? BillDateTo { get; set; }
        public virtual string SupplierNo { get; set; }
        public virtual string OrderTypeCode { get; set; }
        public virtual string CkdPio { get; set; }

    }

    public class GetInvCkdInvoiceDetailDtolInput : PagedAndSortedResultRequestDto
    {
        public virtual long? InvoiceId { get; set; }

    }

    public class GetInvCkdInvoice
    {
        public virtual long? InvoiceId { get; set; }
    }

    public class GetInvCkdInvoiceHistoryInput : PagedAndSortedResultRequestDto
    {
        public virtual long Id { get; set; }

        public virtual string TableName { get; set; }
    }

    public class GetInvCkdInvoiceHistoryExcelInput
    {
        public virtual long Id { get; set; }

        public virtual string TableName { get; set; }
    }

    public class InvCkdInvoiceDetailsByLotDto : EntityDto<long?>
    {
        public virtual string PartNo { get; set; }

        public virtual string CarfamilyCode { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string LotNo { get; set; }

        public virtual string ModuleNo { get; set; }

        public virtual string ContainerNo { get; set; }

        public virtual string InvoiceNo { get; set; }

        public virtual string UsageQty { get; set; }

        public virtual string PartName { get; set; }

    }
    public class GetInvCkdInvoiceCustomsDtolInput : PagedAndSortedResultRequestDto
    {
        public virtual string InvoiceNo { get; set; }
        public virtual DateTime? InvoiceDateFrom { get; set; }
        public virtual DateTime? InvoiceDateTo { get; set; }
        public virtual string BillNo { get; set; }
        public virtual string ShipmentNo { get; set; }
        public virtual string ContainerNo { get; set; }
        public virtual DateTime? BillDateFrom { get; set; }
        public virtual DateTime? BillDateTo { get; set; }

        public virtual string OrderTypeCode { get; set; }
        public virtual string SupplierNo { get; set; }
        public virtual string CkdPio { get; set; }
    }
    

    public class InvCkdInvoiceDetailsByLotInput : PagedAndSortedResultRequestDto
    {
        public virtual string LotNo { get; set; }
    }

    public class InvCkdInvoiceDetailsByLotExportInput
    {
        public virtual string LotNo { get; set; }
    }

    public class ChangedRecordIdsInvoiceDto
    {
        public virtual List<long?> Invoice { get; set; }
        public virtual List<long?> InvoiceDetail { get; set; }

    }
}


