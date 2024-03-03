using Abp.Application.Services.Dto;
using prod.Inventory.GPS.InvGpsContentList;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.GPS.Dto
{

    public class InvGpsContentListDto : EntityDto<long?>
    {

        public virtual DateTime? WorkingDate { get; set; }

        public virtual string Shift { get; set; }

        public virtual string SupplierName { get; set; }

        public virtual string SupplierCode { get; set; }

        public virtual int? RenbanNo { get; set; }

        public virtual string PcAddress { get; set; }

        public virtual string DockNo { get; set; }

        public virtual string OrderNo { get; set; }

        public virtual DateTime? OrderDatetime { get; set; }

        public virtual int? TripNo { get; set; }

        public virtual int? PalletBoxQty { get; set; }

        public virtual DateTime? EstPackingDatetime { get; set; }

        public virtual DateTime? EstArrivalDatetime { get; set; }

        public virtual string ContentNo { get; set; }

        public virtual long? OrderId { get; set; }

        public virtual int? PalletSize { get; set; }

        public virtual string IsPalletOnly { get; set; }

        public virtual string PackagingType { get; set; }

        public virtual string IsAdhocReceiving { get; set; }

        public virtual string GeneratedBy { get; set; }

        public virtual string UnpackStatus { get; set; }

        public virtual string ModuleCd { get; set; }

        public virtual int? ModuleRunNo { get; set; }

        public virtual DateTime? UpStartAct { get; set; }

        public virtual DateTime? UpFinishAct { get; set; }

        public virtual int? UpScanUserId { get; set; }

        public virtual string Status { get; set; }

        public virtual string IsActive { get; set; }

    }

    public class CreateOrEditInvGpsContentListDto : EntityDto<long?>
    {

        public virtual DateTime? WorkingDate { get; set; }

        [StringLength(InvGpsContentListConsts.MaxShiftLength)]
        public virtual string Shift { get; set; }

        [StringLength(InvGpsContentListConsts.MaxSupplierNameLength)]
        public virtual string SupplierName { get; set; }

        [StringLength(InvGpsContentListConsts.MaxSupplierCodeLength)]
        public virtual string SupplierCode { get; set; }

        public virtual int? RenbanNo { get; set; }

        [StringLength(InvGpsContentListConsts.MaxPcAddressLength)]
        public virtual string PcAddress { get; set; }

        [StringLength(InvGpsContentListConsts.MaxDockNoLength)]
        public virtual string DockNo { get; set; }

        [StringLength(InvGpsContentListConsts.MaxOrderNoLength)]
        public virtual string OrderNo { get; set; }

        public virtual DateTime? OrderDatetime { get; set; }

        public virtual int? TripNo { get; set; }

        public virtual int? PalletBoxQty { get; set; }

        public virtual DateTime? EstPackingDatetime { get; set; }

        public virtual DateTime? EstArrivalDatetime { get; set; }

        [StringLength(InvGpsContentListConsts.MaxContentNoLength)]
        public virtual string ContentNo { get; set; }

        public virtual long? OrderId { get; set; }

        public virtual int? PalletSize { get; set; }

        [StringLength(InvGpsContentListConsts.MaxIsPalletOnlyLength)]
        public virtual string IsPalletOnly { get; set; }

        [StringLength(InvGpsContentListConsts.MaxPackagingTypeLength)]
        public virtual string PackagingType { get; set; }

        [StringLength(InvGpsContentListConsts.MaxIsAdhocReceivingLength)]
        public virtual string IsAdhocReceiving { get; set; }

        [StringLength(InvGpsContentListConsts.MaxGeneratedByLength)]
        public virtual string GeneratedBy { get; set; }

        [StringLength(InvGpsContentListConsts.MaxUnpackStatusLength)]
        public virtual string UnpackStatus { get; set; }

        [StringLength(InvGpsContentListConsts.MaxModuleCdLength)]
        public virtual string ModuleCd { get; set; }

        public virtual int? ModuleRunNo { get; set; }

        public virtual DateTime? UpStartAct { get; set; }

        public virtual DateTime? UpFinishAct { get; set; }

        public virtual int? UpScanUserId { get; set; }

        [StringLength(InvGpsContentListConsts.MaxStatusLength)]
        public virtual string Status { get; set; }

        [StringLength(InvGpsContentListConsts.MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

    public class GetInvGpsContentListInput : PagedAndSortedResultRequestDto
    {
        public virtual DateTime? WorkingDateFrom { get; set; }
        public virtual DateTime? WorkingDateTo { get; set; }
        public virtual string Shift { get; set; }
        public virtual string SupplierCode { get; set; }
        public virtual string DockNo { get; set; }
    }

}


