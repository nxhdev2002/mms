using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.CKD.Dto
{

    public class InvCkdContainerRentalWHPlanDto : EntityDto<long?>
    {
        public virtual long? ROW_NO { get; set; }
        public virtual string ContainerNo { get; set; }

        public virtual string Renban { get; set; }

        public virtual DateTime? RequestDate { get; set; }

        public virtual TimeSpan? RequestTime { get; set; }

        public virtual string InvoiceNo { get; set; }

        public virtual string BillofladingNo { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string SealNo { get; set; }

        public virtual string ListcaseNo { get; set; }

        public virtual string ListLotNo { get; set; }

        public virtual DateTime? CdDate { get; set; }

        public virtual DateTime? DevanningDate { get; set; }

        public virtual TimeSpan? DevanningTime { get; set; }

        public virtual DateTime? ActualDevanningDate { get; set; }

        public virtual DateTime? GateInPlanTime { get; set; }

        public virtual DateTime? GateInActualDateTime { get; set; }

        public virtual string Transport { get; set; }

        public virtual string PlateId { get; set; }

        public virtual string Status { get; set; }

        public virtual string IsActive { get; set; }
        public virtual string ErrorDescription { get; set; }
        public virtual string LotNo { get; set; }
        public virtual string ModuleCaseNo { get; set; }
        public virtual string PartNo { get; set; }

		public virtual string WHCode { get; set; }

	}

    public class InvCkdContainerRentalWHPlanDetails : EntityDto<long?>
    {

        public virtual string PartNo { get; set; }

        public virtual string Cfc { get; set; }
        public virtual string WhLocation { get; set; }
        public virtual string CarfamilyCode { get; set; }
        public virtual string SupplierNo { get; set; }
        public virtual int? RemainQty { get; set; }
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
    public class GetInvCkdContainerRentalWHPlanDetailsInput : PagedAndSortedResultRequestDto
    {
        public virtual string ContainerNo { get; set; }
    }

    public class InvCkdContainerPartRepackInput
    {
        public virtual long? Id { get; set; }
        public virtual string Exp { get; set; }
        public virtual string LotNo { get; set; }
        public virtual string Module { get; set; }
        public virtual string PartNo { get; set; }
        public virtual int? Qty { get; set; }
        public virtual int? Source { get; set; }
    }

     public class CreateOrEditInvCkdContainerRentalWHPlanDto : EntityDto<long?>
    {

        [StringLength(InvCkdContainerRentalWHPlanConsts.MaxContainerNoLength)]
        public virtual string ContainerNo { get; set; }

        [StringLength(InvCkdContainerRentalWHPlanConsts.MaxRenbanLength)]
        public virtual string Renban { get; set; }

        public virtual DateTime? RequestDate { get; set; }

        public virtual TimeSpan? RequestTime { get; set; }

        [StringLength(InvCkdContainerRentalWHPlanConsts.MaxInvoiceNoLength)]
        public virtual string InvoiceNo { get; set; }

        [StringLength(InvCkdContainerRentalWHPlanConsts.MaxBillofladingNoLength)]
        public virtual string BillofladingNo { get; set; }

        [StringLength(InvCkdContainerRentalWHPlanConsts.MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        [StringLength(InvCkdContainerRentalWHPlanConsts.MaxSealNoLength)]
        public virtual string SealNo { get; set; }

        [StringLength(InvCkdContainerRentalWHPlanConsts.MaxListcaseNoLength)]
        public virtual string ListcaseNo { get; set; }

        [StringLength(InvCkdContainerRentalWHPlanConsts.MaxListLotNoLength)]
        public virtual string ListLotNo { get; set; }

        public virtual DateTime? CdDate { get; set; }

        public virtual DateTime? DevanningDate { get; set; }

        public virtual TimeSpan? DevanningTime { get; set; }

        public virtual DateTime? ActualDevanningDate { get; set; }

        public virtual DateTime? GateInPlanTime { get; set; }

        public virtual DateTime? GateInActualDateTime { get; set; }

        [StringLength(InvCkdContainerRentalWHPlanConsts.MaxTransportLength)]
        public virtual string Transport { get; set; }

        [StringLength(InvCkdContainerRentalWHPlanConsts.MaxPlateIdLength)]
        public virtual string PlateId { get; set; }

        [StringLength(InvCkdContainerRentalWHPlanConsts.MaxStatusLength)]
        public virtual string Status { get; set; }

        [StringLength(InvCkdContainerRentalWHPlanConsts.MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
		[StringLength(InvCkdContainerRentalWHPlanConsts.MaxWHCodeLength)]
		public virtual string WHCode { get; set; }
	}

    public class GetInvCkdContainerRentalWHPlanInput : PagedAndSortedResultRequestDto
    {

        public virtual string ContainerNo { get; set; }

        public virtual string Renban { get; set; }

        public virtual string InvoiceNo { get; set; }

        public virtual string BillofladingNo { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string SealNo { get; set; }

		public virtual DateTime? RequestDateFrom { get; set; }

		public virtual DateTime? RequestDateTo { get; set; }
        public virtual string LotNo { get; set; }
        public virtual string ModuleCaseNo { get; set; }
        public virtual string PartNo { get; set; }

    }

    public class GetInvCkdContainerRentalRepackInput : PagedAndSortedResultRequestDto
    {
    }

    public class InvCkdContainerRentalWHPlanDetailsImportDto
    {
        public virtual string Exp { get; set; }
        public virtual string Module { get; set; }
        public virtual string RepackModule { get; set; }
        public virtual string Container { get; set; }
        public virtual string WHCurrrent { get; set; }
        public virtual string WHNew { get; set; }
        public virtual int? Shift { get; set; }
        public virtual DateTime? ReceiveDateTime { get; set; }
        public virtual string Guid { get; set; }
        public virtual string ErrorDescription { get; set; }
    }   

    public class InvCkdContainerRentalWhRepackDto : EntityDto<long?>
    {
        public virtual string Exp { get; set; }

        public virtual string Module { get; set; }

        public virtual string PartNo { get; set; }
        public virtual int? Qty { get; set; }
        public virtual DateTime? PackingDate { get; set; }
        public virtual string RepackModuleNo { get; set; }

        public virtual string WHCurrent { get; set; }

        public virtual string WHNew { get; set; }

        public virtual int? Shift { get; set; }
        public virtual DateTime? ReceiveDateTime { get; set; }

        public virtual string Container { get; set; }

        public virtual bool? Status { get; set; }
    }


    public class InvCkdContainerRentalWHPlErrDto
    {
        public virtual long? ROW_NO { get; set; }
        public virtual string ContainerNo { get; set; }

        public virtual string Renban { get; set; }

        public virtual DateTime? RequestDate { get; set; }

        public virtual TimeSpan? RequestTime { get; set; }

        public virtual string InvoiceNo { get; set; }

        public virtual string BillofladingNo { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string SealNo { get; set; }

        public virtual string ListcaseNo { get; set; }

        public virtual string ListLotNo { get; set; }

        public virtual DateTime? CdDate { get; set; }

        public virtual DateTime? DevanningDate { get; set; }

        public virtual TimeSpan? DevanningTime { get; set; }

        public virtual DateTime? ActualDevanningDate { get; set; }

        public virtual DateTime? GateInPlanTime { get; set; }

        public virtual DateTime? GateInActualDateTime { get; set; }

        public virtual string Transport { get; set; }

        public virtual string PlateId { get; set; }

        public virtual string Status { get; set; }

        public virtual string IsActive { get; set; }
        public virtual string ErrorDescription { get; set; }

    }

}


