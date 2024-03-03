using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.CKD.Dto
{

    public class InvCkdContainerListDto : EntityDto<long?>
    {
        public virtual string RequestStatus { get; set; }

        public virtual string ContainerNo { get; set; }

        public virtual string Renban { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string HaisenNo { get; set; }

        public virtual string BillOfLadingNo { get; set; }

        public virtual string SealNo { get; set; }

        public virtual string Carrier { get; set; }
        public virtual DateTime? CdDate { get; set; }

        public virtual string CdStatus { get; set; }

        public virtual int? ContainerSize { get; set; }

        public virtual long? ShipmentId { get; set; }

        public virtual DateTime? ShippingDate { get; set; }

        public virtual DateTime? PortDate { get; set; }

        public virtual DateTime? PortDateActual { get; set; }

        public virtual string FormatPortDateActual
        {
            get
            {
                return PortDateActual == null ? "" : string.Format("{0:dd/MM/yyyy}", PortDateActual);
            }
            set { }
        }

        public virtual DateTime? PortTransitDate { get; set; }

        public virtual DateTime? ReceiveDate { get; set; }

        public virtual long? RequestId { get; set; }

        public virtual string RequestLotNo { get; set; }

        public virtual string InvoiceNo { get; set; }

        public virtual string ListLotNo { get; set; }
        public virtual string LotNo { get; set; }

        public virtual string ListCaseNo { get; set; }

        public virtual string Transport { get; set; }

        public virtual string Shop { get; set; }

        public virtual string Dock { get; set; }

        public virtual DateTime? DevanningDate { get; set; }

        public virtual string DevanningTime { get; set; }

        public virtual string Remark { get; set; }

        public virtual string WhLocation { get; set; }

        public virtual DateTime? GateinDate { get; set; }

        public virtual string GateinTime { get; set; }

        public virtual long? TransitPortReqId { get; set; }

        public virtual DateTime? TransitPortReqDate { get; set; }

        public virtual string TransitPortReqTime { get; set; }

        public virtual string FormatTransitPortReqTime
        {
            get
            {
                return TransitPortReqTime == null ? "" : TransitPortReqTime.ToString().Substring(0, 8);
            }
            set { }
        }

        public virtual string TransitPortRemark { get; set; }

        public virtual decimal? Fob { get; set; }

        public virtual decimal? Freight { get; set; }

        public virtual decimal? Insurance { get; set; }

        public virtual decimal? Cif { get; set; }

        public virtual decimal? Tax { get; set; }

        public virtual decimal? Amount { get; set; }

        public virtual string Description { get; set; }
        public virtual string CdStatusDes { get; set; }

        public virtual string LocationCode { get; set; }

        public virtual DateTime? LocationDate { get; set; }

        public virtual long? ReceivingPeriodId { get; set; }

        public virtual string IsActive { get; set; }

        public virtual string OrdertypeCode { get; set; }

        public virtual string GoodstypeCode { get; set; }

        public virtual decimal? GrandFob { get; set; }

        public virtual decimal? GrandFreight { get; set; }

        public virtual decimal? GrandInsurance { get; set; }

        public virtual decimal? GrandCif { get; set; }

        public virtual decimal? GrandTax { get; set; }

        public virtual decimal? GrandAmount { get; set; }
        public virtual int? GrandOverDEM { get; set; }

        public virtual int? GrandOverDET { get; set; }

        public virtual int? GrandOverDEMDET { get; set; }
        public virtual decimal? GrandOverFeeDEM { get; set; }

        public virtual decimal? GrandOverFeeDET { get; set; }

        public virtual decimal? GrandOverFeeDEMDET { get; set; }

        public virtual DateTime? BillDate { get; set; }

        public virtual decimal? OverFeeDEM { get; set; }
        public virtual decimal? OverFeeDET { get; set; }
        public virtual decimal? OverFeeDEMDET { get; set; }
        public virtual int? OverDEM { get; set; }
        public virtual int? OverDET { get; set; }
        public virtual int? OverDEMDET { get; set; }

        public virtual DateTime? EstOverDEM { get; set; }
        public virtual DateTime? EstOverDET { get; set; }
        public virtual DateTime? EstOverCombine { get; set; }

        public virtual DateTime? DevaningDateTime { get; set; }

    }

    public class CreateOrEditInvCkdContainerListDto : EntityDto<long?>
    {
        [StringLength(InvCkdContainerListConsts.MaxRequestStatusLength)]
        public virtual string RequestStatus { get; set; }
        [StringLength(InvCkdContainerListConsts.MaxContainerNoLength)]
        public virtual string ContainerNo { get; set; }

        [StringLength(InvCkdContainerListConsts.MaxRenbanLength)]
        public virtual string Renban { get; set; }

        [StringLength(InvCkdContainerListConsts.MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        [StringLength(InvCkdContainerListConsts.MaxHaisenNoLength)]
        public virtual string HaisenNo { get; set; }

        [StringLength(InvCkdContainerListConsts.MaxBillOfLadingNoLength)]
        public virtual string BillOfLadingNo { get; set; }

        [StringLength(InvCkdContainerListConsts.MaxSealNoLength)]
        public virtual string SealNo { get; set; }

        public virtual DateTime? CdDate { get; set; }

        [StringLength(InvCkdContainerListConsts.MaxCdStatusLength)]
        public virtual string CdStatus { get; set; }

        public virtual int? ContainerSize { get; set; }

        public virtual long? ShipmentId { get; set; }

        public virtual DateTime? ShippingDate { get; set; }

        public virtual DateTime? PortDate { get; set; }

        public virtual DateTime? PortDateActual { get; set; }

        public virtual DateTime? PortTransitDate { get; set; }

        public virtual DateTime? ReceiveDate { get; set; }

        public virtual long? RequestId { get; set; }

        [StringLength(InvCkdContainerListConsts.MaxRequestLotNoLength)]
        public virtual string RequestLotNo { get; set; }

        [StringLength(InvCkdContainerListConsts.MaxInvoiceNoLength)]
        public virtual string InvoiceNo { get; set; }

        [StringLength(InvCkdContainerListConsts.MaxListLotNoLength)]
        public virtual string ListLotNo { get; set; }

        [StringLength(InvCkdContainerListConsts.MaxListCaseNoLength)]
        public virtual string ListCaseNo { get; set; }

        [StringLength(InvCkdContainerListConsts.MaxTransportLength)]
        public virtual string Transport { get; set; }

        [StringLength(InvCkdContainerListConsts.MaxShopLength)]
        public virtual string Shop { get; set; }

        [StringLength(InvCkdContainerListConsts.MaxDockLength)]
        public virtual string Dock { get; set; }

        public virtual DateTime? DevanningDate { get; set; }

        [StringLength(InvCkdContainerListConsts.MaxDevanningTimeLength)]
        public virtual string DevanningTime { get; set; }

        [StringLength(InvCkdContainerListConsts.MaxRemarkLength)]
        public virtual string Remark { get; set; }

        [StringLength(InvCkdContainerListConsts.MaxWhLocationLength)]
        public virtual string WhLocation { get; set; }

        public virtual DateTime? GateinDate { get; set; }


        [StringLength(InvCkdContainerListConsts.MaxGateinTimeLength)]
        public virtual string GateinTime { get; set; }

        public virtual long? TransitPortReqId { get; set; }

        public virtual DateTime? TransitPortReqDate { get; set; }

        [StringLength(InvCkdContainerListConsts.MaxTransitPortReqTimeLength)]
        public virtual string TransitPortReqTime { get; set; }

        [StringLength(InvCkdContainerListConsts.MaxTransitPortRemarkLength)]
        public virtual string TransitPortRemark { get; set; }

        public virtual decimal? Fob { get; set; }

        public virtual decimal? Freight { get; set; }

        public virtual decimal? Insurance { get; set; }

        public virtual decimal? Cif { get; set; }

        public virtual decimal? Tax { get; set; }

        public virtual decimal? Amount { get; set; }

        [StringLength(InvCkdContainerListConsts.MaxStatusLength)]
        public virtual string Status { get; set; }

        [StringLength(InvCkdContainerListConsts.MaxLocationCodeLength)]
        public virtual string LocationCode { get; set; }

        public virtual DateTime? LocationDate { get; set; }

        public virtual long? ReceivingPeriodId { get; set; }

        [StringLength(InvCkdContainerListConsts.MaxIsActiveLength)]
        public virtual string IsActive { get; set; }

        [StringLength(InvCkdContainerListConsts.MaxOrderTypeCodeLength)]
        public virtual string OrdertypeCode { get; set; }

        [StringLength(InvCkdContainerListConsts.MaxGoodsTypeCodeLength)]
        public virtual string GoodstypeCode { get; set; }
    }

    public class GetInvCkdContainerListInput : PagedAndSortedResultRequestDto
    {

        public virtual string ContainerNo { get; set; }

        public virtual string Renban { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string HaisenNo { get; set; }

        public virtual string BillOfLadingNo { get; set; }

        public virtual string InvoiceNo { get; set; }

        public virtual string OrderTypeCode { get; set; }

        public virtual DateTime? PortDateFrom { get; set; }

        public virtual DateTime? PortDateTo { get; set; }

        public virtual DateTime? ReceiveDateFrom { get; set; }

        public virtual DateTime? ReceiveDateTo { get; set; }

        public virtual string GoodsTypeCode { get; set; }

        public virtual string radio { get; set; }

        public virtual DateTime? BillDateFrom { get; set; }
        public virtual DateTime? BillDateTo { get; set; }
        public virtual string CkdPio { get; set; }
        public virtual string LotNo { get; set; }

    }
    public class ShipmentInfoDetailListDto : EntityDto<long?>
    {
        public virtual string ToPort { get; set; }
        public virtual string SupplierNo { get; set; }

        public virtual DateTime? Eta { get; set; }

        public virtual DateTime? Ata { get; set; }

        public virtual string ShippingcompanyCode { get; set; }

        public virtual string BillOfLadingNo { get; set; }

        public virtual string InvoiceNo { get; set; }

        public virtual string ContainerNo { get; set; }


        public virtual string Renban { get; set; }

        public virtual string LotNo { get; set; }

        public virtual string ModuleCase { get; set; }

        public virtual string SealNo { get; set; }

        public virtual int? ContainerSize { get; set; }

        public virtual string ExpRenban { get; set; }

        public virtual string PartNo { get; set; }

        public virtual string PartName { get; set; }

        public virtual int? UsageQty { get; set; }

        public virtual DateTime Firmpackingmonth { get; set; }

        public virtual string ReexportCode { get; set; }

    }

    public class GetInvCkdContainerListExportInvoiceList
    {
        public virtual string ContainerNo { get; set; }
        public virtual string Renban { get; set; }
        public virtual string PartNo { get; set; }
        public virtual string InvoiceNo { get; set; }
        public virtual string SupplierNo { get; set; }
        public virtual string PartName { get; set; }
        public virtual string Fixlot { get; set; }
        public virtual string LotNo { get; set; }

        public virtual string ModuleNo { get; set; }
        public virtual string CaseNo { get; set; }
        public virtual decimal? Fob { get; set; }
        public virtual decimal? Freight { get; set; }
        public virtual decimal? Insurance { get; set; }
        public virtual decimal? Cif { get; set; }
        public virtual decimal? Tax { get; set; }
        public virtual decimal? TaxRate { get; set; }
        public virtual decimal? Vat { get; set; }
        public virtual decimal? VatRate { get; set; }
        public virtual string CeptType { get; set; }
        public virtual string CarfamilyCode { get; set; }
        public virtual decimal? PartNetWeight { get; set; }
        public virtual string OrderNo { get; set; }
        public virtual DateTime? Firmpackingmonth { get; set; }
        public virtual decimal? VatVn { get; set; }
        public virtual string DeclareType { get; set; }
    }

    public class GetInvCkdContainerListExportNoDeclareCustomsList
    {
        public virtual string ContainerNo { get; set; }
        public virtual string Renban { get; set; }
        public virtual string InvoiceNo { get; set; }
        public virtual DateTime? InvoiceDate { get; set; }
    }

    public class GetGetImPortDeVanList
    {
        public virtual string ContainerNo { get; set; }

        public virtual string Renban { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string HaisenNo { get; set; }

        public virtual string BillOfLadingNo { get; set; }

        public virtual string InvoiceNo { get; set; }

        public virtual string OrderTypeCode { get; set; }

        public virtual DateTime? PortDateFrom { get; set; }

        public virtual DateTime? PortDateTo { get; set; }

        public virtual DateTime? ReceiveDateFrom { get; set; }

        public virtual DateTime? ReceiveDateTo { get; set; }

        public virtual string GoodsTypeCode { get; set; }

        public virtual string radio { get; set; }
        public virtual string CkdPio { get; set; }
    }
    public class ImportDevanDto
    {
        public virtual int NoNumber { get; set; }

        public virtual string ModuleCaseNo { get; set; }

        public virtual string ContainerNo { get; set; }

        public virtual string InvoiceNo { get; set; }

        public virtual string Renban { get; set; }

        public virtual string DevanningDateS4 { get; set; }

        public virtual string CancelFlag { get; set; }

        public virtual string Plant { get; set; }


    }

}


