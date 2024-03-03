using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.CPS
{

    [Table("InvCpsPoLines")]
    public class InvCpsPoLines : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxPartNoLength = 40;

        public const int MaxColorLength = 40;

        public const int MaxItemDescriptionLength = 500;

        public const int MaxUnitMeasLookupCodeLength = 50;

        public const int MaxClosedReasonLength = 240;

        public const int MaxIsActiveLength = 1;

        public virtual long? PoHeaderId { get; set; }

        public virtual long? ItemId { get; set; }

        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(MaxColorLength)]
        public virtual string Color { get; set; }

        [StringLength(MaxItemDescriptionLength)]
        public virtual string ItemDescription { get; set; }

        public virtual long? CategoryId { get; set; }

        [StringLength(MaxUnitMeasLookupCodeLength)]
        public virtual string UnitMeasLookupCode { get; set; }

        public virtual decimal? ListPricePerUnit { get; set; }

        public virtual decimal? UnitPrice { get; set; }

        public virtual decimal? ForeignPrice { get; set; }

        public virtual decimal? Quantity { get; set; }

        public virtual decimal? QtyRcvToLerance { get; set; }

        public virtual decimal? MarketPrice { get; set; }

        public virtual DateTime? ClosedDate { get; set; }

        [StringLength(MaxClosedReasonLength)]
        public virtual string ClosedReason { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}

