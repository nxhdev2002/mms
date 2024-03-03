using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.CPS.Dto
{

    public class InvCpsPoLinesDto : EntityDto<long?>
    {

        public virtual long? PoHeaderId { get; set; }

        public virtual long? ItemId { get; set; }

        public virtual string PartNo { get; set; }

        public virtual string Color { get; set; }

        public virtual string ItemDescription { get; set; }

        public virtual long? CategoryId { get; set; }

        public virtual string UnitMeasLookupCode { get; set; }

        public virtual decimal? ListPricePerUnit { get; set; }

        public virtual decimal? UnitPrice { get; set; }

        public virtual decimal? ForeignPrice { get; set; }

        public virtual decimal? Quantity { get; set; }

        public virtual decimal? QtyRcvToLerance { get; set; }

        public virtual decimal? MarketPrice { get; set; }

        public virtual DateTime? ClosedDate { get; set; }

        public virtual string ClosedReason { get; set; }

        public virtual string IsActive { get; set; }

    }

    public class CreateOrEditInvCpsPoLinesDto : EntityDto<long?>
    {

        public virtual long? PoHeaderId { get; set; }

        public virtual long? ItemId { get; set; }

        [StringLength(InvCpsPoLinesConsts.MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(InvCpsPoLinesConsts.MaxColorLength)]
        public virtual string Color { get; set; }

        [StringLength(InvCpsPoLinesConsts.MaxItemDescriptionLength)]
        public virtual string ItemDescription { get; set; }

        public virtual long? CategoryId { get; set; }

        [StringLength(InvCpsPoLinesConsts.MaxUnitMeasLookupCodeLength)]
        public virtual string UnitMeasLookupCode { get; set; }

        public virtual decimal? ListPricePerUnit { get; set; }

        public virtual decimal? UnitPrice { get; set; }

        public virtual decimal? ForeignPrice { get; set; }

        public virtual decimal? Quantity { get; set; }

        public virtual decimal? QtyRcvToLerance { get; set; }

        public virtual decimal? MarketPrice { get; set; }

        public virtual DateTime? ClosedDate { get; set; }

        [StringLength(InvCpsPoLinesConsts.MaxClosedReasonLength)]
        public virtual string ClosedReason { get; set; }

        [StringLength(InvCpsPoLinesConsts.MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

    public class GetInvCpsPoLinesInput : PagedAndSortedResultRequestDto
    {

        public virtual string PartNo { get; set; }

        public virtual string Color { get; set; }


    }

}


