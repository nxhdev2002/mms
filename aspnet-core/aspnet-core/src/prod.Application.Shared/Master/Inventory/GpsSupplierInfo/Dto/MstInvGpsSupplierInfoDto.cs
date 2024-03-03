using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Inv.Dto
{

    public class MstInvGpsSupplierInfoDto : EntityDto<long?>
    {

        public virtual string SupplierCode { get; set; }

        public virtual string SupplierPlantCode { get; set; }

        public virtual string SupplierName { get; set; }

        public virtual string Address { get; set; }

        public virtual string DockX { get; set; }

        public virtual string DockXAddress { get; set; }

        public virtual string DeliveryMethod { get; set; }

        public virtual string DeliveryFrequency { get; set; }

        public virtual string Cd { get; set; }

        public virtual string OrderDateType { get; set; }

        public virtual string KeihenType { get; set; }

        public virtual decimal? StkConceptTmvMin { get; set; }

        public virtual decimal? StkConceptTmvMax { get; set; }

        public virtual decimal? StkConceptSupMMin { get; set; }

        public virtual decimal? StkConceptSupMMax { get; set; }

        public virtual decimal? StkConceptSupPMin { get; set; }

        public virtual decimal? StkConceptSupPMax { get; set; }

        public virtual int? TmvProductPercentage { get; set; }

        public virtual int? PicMainId { get; set; }

        public virtual decimal? DeliveryLt { get; set; }

        public virtual string ProductionShift { get; set; }

        public virtual DateTime? TcFrom { get; set; }

        public virtual DateTime? TcTo { get; set; }

        public virtual int? OrderTrip { get; set; }

        public virtual string SupplierNameEn { get; set; }

        public virtual string IsActive { get; set; }

    }

    public class CreateOrEditMstInvGpsSupplierInfoDto : EntityDto<long?>
    {

        [StringLength(MstInvGpsSupplierInfoConsts.MaxSupplierCodeLength)]
        public virtual string SupplierCode { get; set; }

        [StringLength(MstInvGpsSupplierInfoConsts.MaxSupplierPlantCodeLength)]
        public virtual string SupplierPlantCode { get; set; }

        [StringLength(MstInvGpsSupplierInfoConsts.MaxSupplierNameLength)]
        public virtual string SupplierName { get; set; }

        [StringLength(MstInvGpsSupplierInfoConsts.MaxAddressLength)]
        public virtual string Address { get; set; }

        [StringLength(MstInvGpsSupplierInfoConsts.MaxDockXLength)]
        public virtual string DockX { get; set; }

        [StringLength(MstInvGpsSupplierInfoConsts.MaxDockXAddressLength)]
        public virtual string DockXAddress { get; set; }

        [StringLength(MstInvGpsSupplierInfoConsts.MaxDeliveryMethodLength)]
        public virtual string DeliveryMethod { get; set; }

        [StringLength(MstInvGpsSupplierInfoConsts.MaxDeliveryFrequencyLength)]
        public virtual string DeliveryFrequency { get; set; }

        [StringLength(MstInvGpsSupplierInfoConsts.MaxCdLength)]
        public virtual string Cd { get; set; }

        [StringLength(MstInvGpsSupplierInfoConsts.MaxOrderDateTypeLength)]
        public virtual string OrderDateType { get; set; }

        [StringLength(MstInvGpsSupplierInfoConsts.MaxKeihenTypeLength)]
        public virtual string KeihenType { get; set; }

        public virtual decimal? StkConceptTmvMin { get; set; }

        public virtual decimal? StkConceptTmvMax { get; set; }

        public virtual decimal? StkConceptSupMMin { get; set; }

        public virtual decimal? StkConceptSupMMax { get; set; }

        public virtual decimal? StkConceptSupPMin { get; set; }

        public virtual decimal? StkConceptSupPMax { get; set; }

        public virtual int? TmvProductPercentage { get; set; }

        public virtual int? PicMainId { get; set; }

        public virtual decimal? DeliveryLt { get; set; }

        [StringLength(MstInvGpsSupplierInfoConsts.MaxProductionShiftLength)]
        public virtual string ProductionShift { get; set; }

        public virtual DateTime? TcFrom { get; set; }

        public virtual DateTime? TcTo { get; set; }

        public virtual int? OrderTrip { get; set; }

        [StringLength(MstInvGpsSupplierInfoConsts.MaxSupplierNameEnLength)]
        public virtual string SupplierNameEn { get; set; }

        [StringLength(MstInvGpsSupplierInfoConsts.MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

    public class GetMstInvGpsSupplierInfoInput : PagedAndSortedResultRequestDto
    {

        public virtual string SupplierCode { get; set; }

        public virtual string DeliveryMethod { get; set; }

        public virtual string DeliveryFrequency { get; set; }

        public virtual string OrderDateType { get; set; }

        public virtual string KeihenType { get; set; }

    }
    public class MstInvGpsSupplierInfoExportInput
    {

        public virtual string SupplierCode { get; set; }

        public virtual string SupplierPlantCode { get; set; }

        public virtual string SupplierName { get; set; }

        public virtual string Address { get; set; }

        public virtual string DockX { get; set; }

        public virtual string DockXAddress { get; set; }

        public virtual string DeliveryMethod { get; set; }

        public virtual string DeliveryFrequency { get; set; }

        public virtual string Cd { get; set; }

        public virtual string OrderDateType { get; set; }

        public virtual string KeihenType { get; set; }

        public virtual decimal? StkConceptTmvMin { get; set; }

        public virtual decimal? StkConceptTmvMax { get; set; }

        public virtual decimal? StkConceptSupMMin { get; set; }

        public virtual decimal? StkConceptSupMMax { get; set; }

        public virtual decimal? StkConceptSupPMin { get; set; }

        public virtual decimal? StkConceptSupPMax { get; set; }

        public virtual int? TmvProductPercentage { get; set; }

        public virtual int? PicMainId { get; set; }

        public virtual decimal? DeliveryLt { get; set; }

        public virtual string ProductionShift { get; set; }

        public virtual DateTime? TcFrom { get; set; }

        public virtual DateTime? TcTo { get; set; }

        public virtual int? OrderTrip { get; set; }

        public virtual string SupplierNameEn { get; set; }

        public virtual string IsActive { get; set; }

    }

}


