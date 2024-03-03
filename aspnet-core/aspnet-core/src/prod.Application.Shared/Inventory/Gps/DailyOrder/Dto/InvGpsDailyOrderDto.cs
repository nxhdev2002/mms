using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.GPS.Dto
{

    public class InvGpsDailyOrderDto : EntityDto<long?>
    {

        public virtual DateTime? WorkingDate { get; set; }

        public virtual string Shift { get; set; }

        public virtual string SupplierName { get; set; }

        public virtual string SupplierCode { get; set; }

        public virtual string OrderNo { get; set; }

        public virtual DateTime? OrderDatetime { get; set; }

        public virtual int? TripNo { get; set; }

        public virtual string TruckNo { get; set; }

        public virtual DateTime? EstArrivalDatetime { get; set; }      

        public virtual long? TruckUnloadingId { get; set; }

        public virtual string IsActive { get; set; }

    }

    public class CreateOrEditInvGpsDailyOrderDto : EntityDto<long?>
    {

        public virtual DateTime? WorkingDate { get; set; }

        [StringLength(InvGpsDailyOrderConsts.MaxShiftLength)]
        public virtual string Shift { get; set; }

        [StringLength(InvGpsDailyOrderConsts.MaxSupplierNameLength)]
        public virtual string SupplierName { get; set; }

        [StringLength(InvGpsDailyOrderConsts.MaxSupplierCodeLength)]
        public virtual string SupplierCode { get; set; }

        [StringLength(InvGpsDailyOrderConsts.MaxOrderNoLength)]
        public virtual string OrderNo { get; set; }

        public virtual DateTime? OrderDatetime { get; set; }

        public virtual int? TripNo { get; set; }

        [StringLength(InvGpsDailyOrderConsts.MaxTruckNoLength)]
        public virtual string TruckNo { get; set; }

        public virtual DateTime? EstArrivalDatetime { get; set; }    

        public virtual long? TruckUnloadingId { get; set; }

        [StringLength(InvGpsDailyOrderConsts.MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

    public class GetInvGpsDailyOrderInput : PagedAndSortedResultRequestDto
    {
        public virtual DateTime? WorkingDateFrom { get; set; }

        public virtual DateTime? WorkingDateTo { get; set; }

        public virtual string Shift { get; set; }

        public virtual string SupplierCode { get; set; }

        public virtual string OrderNo { get; set; }

        public virtual string TruckNo { get; set; } 

    }

}


