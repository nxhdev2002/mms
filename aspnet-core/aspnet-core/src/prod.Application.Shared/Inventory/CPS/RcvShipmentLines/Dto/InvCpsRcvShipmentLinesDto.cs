using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.CPS.Dto
{

    public class InvCpsRcvShipmentLinesDto : EntityDto<long?>
    {

        public virtual long? ShipmentHeaderId { get; set; }

        public virtual int? LineNum { get; set; }

        public virtual long? CategoryId { get; set; }

        public virtual decimal? QuantityShipped { get; set; }

        public virtual decimal? QuantityReceived { get; set; }

        public virtual string UnitOfMeasure { get; set; }

        public virtual string PartNo { get; set; }

        public virtual string ItemDescription { get; set; }

        public virtual long? ItemId { get; set; }

        public virtual string ShipmentLineStatusCode { get; set; }

        public virtual long? Poheaderid { get; set; }

        public virtual long? PoLineId { get; set; }

        public virtual long? PoLineShipmentId { get; set; }

        public virtual long? EmployeeId { get; set; }

        public virtual string IsActive { get; set; }

    }

    public class CreateOrEditInvCpsRcvShipmentLinesDto : EntityDto<long?>
    {

        public virtual long? ShipmentHeaderId { get; set; }

        public virtual int? LineNum { get; set; }

        public virtual long? CategoryId { get; set; }

        public virtual decimal? QuantityShipped { get; set; }

        public virtual decimal? QuantityReceived { get; set; }

        [StringLength(InvCpsRcvShipmentLinesConsts.MaxUnitOfMeasureLength)]
        public virtual string UnitOfMeasure { get; set; }

        [StringLength(InvCpsRcvShipmentLinesConsts.MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(InvCpsRcvShipmentLinesConsts.MaxItemDescriptionLength)]
        public virtual string ItemDescription { get; set; }

        public virtual long? ItemId { get; set; }

        [StringLength(InvCpsRcvShipmentLinesConsts.MaxShipmentLineStatusCodeLength)]
        public virtual string ShipmentLineStatusCode { get; set; }

        public virtual long? Poheaderid { get; set; }

        public virtual long? PoLineId { get; set; }

        public virtual long? PoLineShipmentId { get; set; }

        public virtual long? EmployeeId { get; set; }

        [StringLength(InvCpsRcvShipmentLinesConsts.MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

    public class GetInvCpsRcvShipmentLinesInput : PagedAndSortedResultRequestDto
    {

        public virtual string UnitOfMeasure { get; set; }

        public virtual string PartNo { get; set; }

        public virtual string ItemDescription { get; set; }

        public virtual string ShipmentLineStatusCode { get; set; }

        public virtual string IsActive { get; set; }

    }

}


