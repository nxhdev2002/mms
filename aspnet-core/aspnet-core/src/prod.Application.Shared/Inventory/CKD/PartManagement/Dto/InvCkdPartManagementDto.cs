using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.CKD.Dto
{

    public class InvCkdPartManagementDto : EntityDto<long?>
    {

        public virtual string PartNo { get; set; }

        public virtual string Cfc { get; set; }
        public virtual string CarfamilyCode { get; set; }     
        public virtual string SupplierNo { get; set; }

        public virtual int? UsageQty { get; set; }

        public virtual string Fixlot { get; set; }

        public virtual string ModuleCaseNo { get; set; }

        public virtual DateTime? Firmpackingmonth { get; set; }

        public virtual string CarName { get; set; }

        public virtual string InvoiceNo { get; set; }

        public virtual DateTime? InvoiceDate { get; set; }

        public virtual string ContainerNo { get; set; }

        public virtual string Renban { get; set; }

        public virtual string BillofladingNo { get; set; }

        public virtual string ShipmentNo { get; set; }

        public virtual string SealNo { get; set; }

        public virtual DateTime? CdDate { get; set; }

        public virtual int? CdStatus { get; set; }

        public virtual int? ContainerSize { get; set; }

        public virtual DateTime? ShippingDate { get; set; }

        public virtual DateTime? PortDate { get; set; }

        public virtual DateTime? ReceiveDate { get; set; }

        public virtual DateTime? PortDateActual { get; set; }

        public virtual DateTime? PortTransitDate { get; set; }

        public virtual DateTime? UnpackingDate { get; set; }

        public virtual string StorageLocationCode { get; set; }

        public virtual string OrdertypeCode { get; set; }

        public virtual string GoodstypeCode { get; set; }

        public virtual decimal? Fob { get; set; } 
        public virtual decimal? Freight { get; set; }  
        public virtual decimal? Insurance { get; set; }
        public virtual decimal? Cif { get; set; }
        public virtual decimal? Tax { get; set; }
        public virtual decimal? Amount { get; set; }

        public virtual decimal? TotalQty { get; set; }
        public virtual decimal? TotalFob { get; set; }
        public virtual decimal? TotalCif { get; set; }
        public virtual decimal? TotalFreight { get; set; }
        public virtual decimal? TotalInsurance { get; set; }
        public virtual decimal? TotalTax { get; set; }
        public virtual decimal? TotalAmount { get; set; }
        public virtual decimal? ThcTotal { get; set; } 
        public virtual decimal? NetWeight { get; set; } 
        public virtual decimal? GrossWeight { get; set; }
        public virtual DateTime? BillDate { get; set; }
        public virtual string OrderNo { get; set; }
        public virtual string LotNo { get; set; }

        public virtual string PartName { get; set; }

    }


    public class GetInvCkdPartManagementInput : PagedAndSortedResultRequestDto
    {

        public virtual string PartNo { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string InvoiceNo { get; set; }

        public virtual DateTime? InvoiceDateFrom { get; set; }

        public virtual DateTime? InvoiceDateTo { get; set; }

        public virtual string BillofladingNo { get; set; }

        public virtual string ShipmentNo { get; set; }

        public virtual string ContainerNo { get; set; }

        public virtual string Renban { get; set; }

        public virtual DateTime? PortDateFrom { get; set; }

        public virtual DateTime? PortDateTo { get; set; }

        public virtual DateTime? ReceiveDateFrom { get; set; }

        public virtual DateTime? ReceiveDateTo { get; set; }

        public virtual string radio { get; set; }

        public virtual string ModuleCaseNo { get; set; }

        public virtual DateTime? UnpackingDateFrom { get; set; }

        public virtual DateTime? UnpackingDateTo { get; set; }

        public virtual string StorageLocationCode { get; set; }
        public virtual DateTime? BillDateFrom { get; set; }
        public virtual DateTime? BillDateTo { get; set; }

        public virtual string OrderNo { get; set; }

        public virtual string OrderTypeCode { get; set; }

        public virtual string GoodsTypeCode { get; set; }

        public virtual string CkdPio { get; set; }
        public virtual DateTime? FirmPackingDateFrom { get; set; }
        public virtual DateTime? FirmPackingDateTo { get; set; }
        public virtual string LotNo { get; set; }

    }

    public class InvCkdPartManagementExportInput 
    {

        public virtual string PartNo { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string InvoiceNo { get; set; }

        public virtual DateTime? InvoiceDateFrom { get; set; }

        public virtual DateTime? InvoiceDateTo { get; set; }

        public virtual string BillofladingNo { get; set; }

        public virtual string ShipmentNo { get; set; }

        public virtual string ContainerNo { get; set; }

        public virtual string Renban { get; set; }

        public virtual DateTime? PortDateFrom { get; set; }

        public virtual DateTime? PortDateTo { get; set; }

        public virtual DateTime? ReceiveDateFrom { get; set; }

        public virtual DateTime? ReceiveDateTo { get; set; }

        public virtual string radio { get; set; }

        public virtual string ModuleCaseNo { get; set; }

        public virtual DateTime? UnpackingDateFrom { get; set; }

        public virtual DateTime? UnpackingDateTo { get; set; }

        public virtual string StorageLocationCode { get; set; }
        public virtual DateTime? BillDateFrom { get; set; }
        public virtual DateTime? BillDateTo { get; set; }

        public virtual string OrderNo { get; set; }

        public virtual string OrderTypeCode { get; set; }

        public virtual string GoodsTypeCode { get; set; }

        public virtual string CkdPio { get; set; }
        public virtual DateTime? FirmPackingDateFrom { get; set; }
        public virtual DateTime? FirmPackingDateTo { get; set; }
        public virtual string LotNo { get; set; }
    }

    public class InvCkdPartManagementReportDto
    {

        public virtual string SupplierNo { get; set; }

        public virtual string PartNo { get; set; }
        public virtual string PartName { get; set; }
        public virtual string fixlot { get; set; }

        public virtual string GoodstypeCode { get; set; }

        public virtual string ContainerNo { get; set; }

        public virtual int? sum_usageqty { get; set; }

        public virtual decimal? fob { get; set; }

        public virtual decimal? cif { get; set; }

        public virtual decimal? sum_fob { get; set; }

        public virtual decimal? sum_cif { get; set; }

        public virtual decimal? tax { get; set; }

        public virtual decimal? vat { get; set; }

        public virtual decimal? sum_tax { get; set; }

        public virtual decimal? sum_vat { get; set; }

        public virtual string InvoiceNo { get; set; }

        public virtual DateTime? InvoiceDate { get; set; }

        public virtual string OrdertypeCode { get; set; }

        public virtual string CustomsDeclareNo { get; set; }

        public virtual DateTime? DeclareDate { get; set; }

        public virtual DateTime? firmpackingmonth { get; set; }

    }

    public class GetInvCkdPartManagementHistoryInput : PagedAndSortedResultRequestDto
    {
        public virtual long Id { get; set; }

        public virtual string TableName { get; set; }
    }

    public class GetInvCkdPartManagementHistoryExcelInput
    {
        public virtual long Id { get; set; }

        public virtual string TableName { get; set; }
    }
}


