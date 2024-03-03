using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.CKD.Dto
{

    public class InvCkdModuleCaseDto : EntityDto<long?>
    {

        public virtual string ModuleNo { get; set; }

        public virtual string CaseNo { get; set; }

        public virtual string ContainerNo { get; set; }

        public virtual string Renban { get; set; }

        public virtual long? InvoiceId { get; set; }

        public virtual string LotNo { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual long? ContainerListId { get; set; }

        public virtual string ModuleCaseNo { get; set; }

        public virtual string InvoiceNo { get; set; }

        public virtual DateTime? CdDate { get; set; }

        public virtual string StorageLocationCode { get; set; }

        public virtual long? LocationId { get; set; }

        public virtual DateTime? LocationDate { get; set; }

        public virtual string LocationBy { get; set; }

        public virtual DateTime? UnpackingDate { get; set; }

        public virtual string UnpackingTime { get; set; }

        public virtual DateTime? UnpackingDatetime { get; set; }

        public virtual DateTime? DevanningDate { get; set; }

        public virtual string DevanningTime { get; set; }

        public virtual decimal? NetWeight { get; set; }

        public virtual decimal? GrossWeight { get; set; }

        public virtual int? MeasuarementM3 { get; set; }

        public virtual int? Lengthofcasemodule { get; set; }

        public virtual int? Widthofcasemodule { get; set; }

        public virtual int? Heightofcasemodule { get; set; }

        public virtual string Dummy { get; set; }

        public virtual DateTime? Plannedpackingdate { get; set; }

        public virtual string Dgflag { get; set; }

        public virtual string PackingType { get; set; }

        public virtual string RrType { get; set; }

        public virtual decimal? FreightPerInvoice { get; set; }

        public virtual decimal? InsurancePerInvoice { get; set; }

        public virtual string InnerMaterialType1 { get; set; }

        public virtual string InnerMaterialType2 { get; set; }

        public virtual string InnerMaterialType3 { get; set; }

        public virtual string InnerMaterialType4 { get; set; }

        public virtual string InnerMaterialType5 { get; set; }

        public virtual string InnerMaterialType6 { get; set; }

        public virtual string InnerMaterialType7 { get; set; }

        public virtual string InnerMaterialType8 { get; set; }

        public virtual string InnerMaterialType9 { get; set; }

        public virtual string InnerMaterialType10 { get; set; }

        public virtual string InnerMaterialType11 { get; set; }

        public virtual string InnerMaterialType12 { get; set; }

        public virtual int? InnerMaterialQuantity1 { get; set; }

        public virtual int? InnerMaterialQuantity2 { get; set; }

        public virtual int? InnerMaterialQuantity3 { get; set; }

        public virtual int? InnerMaterialQuantity4 { get; set; }

        public virtual int? InnerMaterialQuantity5 { get; set; }

        public virtual int? InnerMaterialQuantity6 { get; set; }

        public virtual int? InnerMaterialQuantity7 { get; set; }

        public virtual int? InnerMaterialQuantity8 { get; set; }

        public virtual int? InnerMaterialQuantity9 { get; set; }

        public virtual int? InnerMaterialQuantity10 { get; set; }

        public virtual int? InnerMaterialQuantity11 { get; set; }

        public virtual int? InnerMaterialQuantity12 { get; set; }

        public virtual int? Status { get; set; }

        public virtual string IsActive { get; set; }

        public virtual DateTime? BillDate { get; set; }

    }

    public class CreateOrEditInvCkdModuleCaseDto : EntityDto<long?>
    {

        [StringLength(InvCkdModuleCaseConsts.MaxModuleNoLength)]
        public virtual string ModuleNo { get; set; }

        [StringLength(InvCkdModuleCaseConsts.MaxCaseNoLength)]
        public virtual string CaseNo { get; set; }

        [StringLength(InvCkdModuleCaseConsts.MaxContainerNoLength)]
        public virtual string ContainerNo { get; set; }

        [StringLength(InvCkdModuleCaseConsts.MaxRenbanLength)]
        public virtual string Renban { get; set; }

        public virtual long? InvoiceId { get; set; }

        [StringLength(InvCkdModuleCaseConsts.MaxLotNoLength)]
        public virtual string LotNo { get; set; }

        [StringLength(InvCkdModuleCaseConsts.MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        public virtual long? ContainerListId { get; set; }

        [StringLength(InvCkdModuleCaseConsts.MaxModuleCaseNoLength)]
        public virtual string ModuleCaseNo { get; set; }

        [StringLength(InvCkdModuleCaseConsts.MaxInvoiceNoLength)]
        public virtual string InvoiceNo { get; set; }

        public virtual DateTime? CdDate { get; set; }

        [StringLength(InvCkdModuleCaseConsts.MaxStorageLocationCodeLength)]
        public virtual string StorageLocationCode { get; set; }

        public virtual long? LocationId { get; set; }

        public virtual DateTime? LocationDate { get; set; }

        [StringLength(InvCkdModuleCaseConsts.MaxLocationByLength)]
        public virtual string LocationBy { get; set; }

        public virtual DateTime? UnpackingDate { get; set; }

        public virtual string UnpackingTime { get; set; }

        public virtual DateTime? UnpackingDatetime { get; set; }

        public virtual decimal? NetWeight { get; set; }

        public virtual decimal? GrossWeight { get; set; }

        public virtual int? MeasuarementM3 { get; set; }

        public virtual int? Lengthofcasemodule { get; set; }

        public virtual int? Widthofcasemodule { get; set; }

        public virtual int? Heightofcasemodule { get; set; }

        [StringLength(InvCkdModuleCaseConsts.MaxDummyLength)]
        public virtual string Dummy { get; set; }

        public virtual DateTime? Plannedpackingdate { get; set; }

        [StringLength(InvCkdModuleCaseConsts.MaxDgflagLength)]
        public virtual string Dgflag { get; set; }

        [StringLength(InvCkdModuleCaseConsts.MaxPackingTypeLength)]
        public virtual string PackingType { get; set; }

        [StringLength(InvCkdModuleCaseConsts.MaxRrTypeLength)]
        public virtual string RrType { get; set; }

        public virtual decimal? FreightPerInvoice { get; set; }

        public virtual decimal? InsurancePerInvoice { get; set; }

        [StringLength(InvCkdModuleCaseConsts.MaxInnerMaterialType1Length)]
        public virtual string InnerMaterialType1 { get; set; }

        [StringLength(InvCkdModuleCaseConsts.MaxInnerMaterialType2Length)]
        public virtual string InnerMaterialType2 { get; set; }

        [StringLength(InvCkdModuleCaseConsts.MaxInnerMaterialType3Length)]
        public virtual string InnerMaterialType3 { get; set; }

        [StringLength(InvCkdModuleCaseConsts.MaxInnerMaterialType4Length)]
        public virtual string InnerMaterialType4 { get; set; }

        [StringLength(InvCkdModuleCaseConsts.MaxInnerMaterialType5Length)]
        public virtual string InnerMaterialType5 { get; set; }

        [StringLength(InvCkdModuleCaseConsts.MaxInnerMaterialType6Length)]
        public virtual string InnerMaterialType6 { get; set; }

        [StringLength(InvCkdModuleCaseConsts.MaxInnerMaterialType7Length)]
        public virtual string InnerMaterialType7 { get; set; }

        [StringLength(InvCkdModuleCaseConsts.MaxInnerMaterialType8Length)]
        public virtual string InnerMaterialType8 { get; set; }

        [StringLength(InvCkdModuleCaseConsts.MaxInnerMaterialType9Length)]
        public virtual string InnerMaterialType9 { get; set; }

        [StringLength(InvCkdModuleCaseConsts.MaxInnerMaterialType10Length)]
        public virtual string InnerMaterialType10 { get; set; }

        [StringLength(InvCkdModuleCaseConsts.MaxInnerMaterialType11Length)]
        public virtual string InnerMaterialType11 { get; set; }

        [StringLength(InvCkdModuleCaseConsts.MaxInnerMaterialType12Length)]
        public virtual string InnerMaterialType12 { get; set; }

        public virtual int? InnerMaterialQuantity1 { get; set; }

        public virtual int? InnerMaterialQuantity2 { get; set; }

        public virtual int? InnerMaterialQuantity3 { get; set; }

        public virtual int? InnerMaterialQuantity4 { get; set; }

        public virtual int? InnerMaterialQuantity5 { get; set; }

        public virtual int? InnerMaterialQuantity6 { get; set; }

        public virtual int? InnerMaterialQuantity7 { get; set; }

        public virtual int? InnerMaterialQuantity8 { get; set; }

        public virtual int? InnerMaterialQuantity9 { get; set; }

        public virtual int? InnerMaterialQuantity10 { get; set; }

        public virtual int? InnerMaterialQuantity11 { get; set; }

        public virtual int? InnerMaterialQuantity12 { get; set; }

        public virtual int? Status { get; set; }

        [StringLength(InvCkdModuleCaseConsts.MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

    public class GetInvCkdModuleCaseInput : PagedAndSortedResultRequestDto
    {
        public virtual string ModuleCaseNo { get; set; }
        public virtual string ContainerNo { get; set; }
        public virtual string Renban { get; set;}

        public virtual string SupplierNo { get; set; }

        public virtual string InvoiceNo { get; set; }

        public virtual string CaseNo { get; set; }
        public virtual DateTime? DevanningFromDate { get; set; }
        public virtual DateTime? DevanningToDate { get; set; }
        public virtual DateTime? UnpackingFromDate { get; set; }
        public virtual DateTime? UnpackingToDate { get; set; }
        public virtual string StorageLocationCode { get; set; }
        public virtual string radio { get; set; }
        public virtual DateTime? BillDateFrom { get; set; }
        public virtual DateTime? BillDateTo { get; set; }

        public virtual string CkdPio { get; set; }
        public virtual string OrderTypeCode { get; set; }
        public virtual string BillNo { get; set; }
        public virtual string LotNo { get; set; }

    }

    public class GetInvCkdContIntransitbyContInput : PagedAndSortedResultRequestDto
    {
        public virtual string containerNo { get; set; }
        public virtual string Renban { get; set; }


    }

}
