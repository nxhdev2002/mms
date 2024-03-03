using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.CKD
{

    [Table("InvCkdProductionMapping_T")]
    public class InvCkdProductionMapping_T : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxGuidLength = 128;

        public const int MaxShopLength = 11;

        public const int MaxModelLength = 1;

        public const int MaxLotNoLength = 50;

        public const int MaxNoInLotLength = 50;

        public const int MaxGradeLength = 2;

        public const int MaxBodyNoLength = 50;

        public const int MaxTimeInLength = 20;

        public const int MaxUseLotNoLength = 50;

        public const int MaxSupplierNoLength = 50;

        public const int MaxErrorDescriptionLength = 5000;


        [StringLength(MaxGuidLength)]
        public virtual string Guid { get; set; }
        public virtual long? PlanSequence { get; set; }

        [StringLength(MaxShopLength)]
        public virtual string Shop { get; set; }

        [StringLength(MaxModelLength)]
        public virtual string Model { get; set; }

        [StringLength(MaxLotNoLength)]
        public virtual string LotNo { get; set; }

        [StringLength(MaxNoInLotLength)]
        public virtual string NoInLot { get; set; }

        [StringLength(MaxGradeLength)]
        public virtual string Grade { get; set; }

        [StringLength(MaxBodyNoLength)]
        public virtual string BodyNo { get; set; }

        [Column(TypeName = "Date")]
        public virtual DateTime? DateIn { get; set; }

        [StringLength(MaxTimeInLength)]
        public virtual string TimeIn { get; set; }

        [StringLength(MaxUseLotNoLength)]
        public virtual string UseLotNo { get; set; }

        [StringLength(MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        public virtual long? PartId { get; set; }

        public virtual decimal? Quantity { get; set; }

        public virtual long? PeriodId { get; set; }

        public virtual long? WipId { get; set; }

        public virtual long? InStockId { get; set; }

        public virtual long? MappingId { get; set; }

        [StringLength(MaxErrorDescriptionLength)]
        public string ErrorDescription { get; set; }
    }

}


