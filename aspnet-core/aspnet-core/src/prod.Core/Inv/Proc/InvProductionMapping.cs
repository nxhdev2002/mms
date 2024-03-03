using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inv.Proc
{ 
    
        [Table("InvProductionMapping")]
        [Index(nameof(Shop), Name = "IX_InvProductionMapping_PlanSequence")]
        [Index(nameof(Model), Name = "IX_InvProductionMapping_InvoiceNo")]
        [Index(nameof(LotNo), Name = "IX_InvProductionMapping_Fob")]
        [Index(nameof(NoInLot), Name = "IX_InvProductionMapping_InlandCharge")]
        [Index(nameof(Grade), Name = "IX_InvProductionMapping_Amount")]
        [Index(nameof(CifVn), Name = "IX_InvProductionMapping_CifVn")]
        [Index(nameof(AmountVn), Name = "IX_InvProductionMapping_AmountVn")]
        [Index(nameof(TimeIn), Name = "IX_InvProductionMapping_PriceVn")]
        [Index(nameof(UseLotNo), Name = "IX_InvProductionMapping_ContSize")]
        public class InvProductionMapping : FullAuditedEntity<long>, IEntity<long>
        {

            public const int MaxShopLength = 11;

            public const int MaxModelLength = 1;

            public const int MaxLotNoLength = 50;

            public const int MaxNoInLotLength = 50;

            public const int MaxGradeLength = 2;

            public const int MaxBodyNoLength = 50;

            public const int MaxTimeInLength = 20;

            public const int MaxUseLotNoLength = 50;

            public const int MaxSupplierNoLength = 50;
            public virtual decimal PlanSequence { get; set; }

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

            [StringLength(MaxTimeInLength)]
             public virtual string TimeIn { get; set; }

             [StringLength(MaxUseLotNoLength)]
             public virtual string UseLotNo { get; set; }

            [StringLength(MaxSupplierNoLength)]
             public virtual string SupplierNo { get; set; }       

            public virtual decimal? PartId { get; set; }

            public virtual decimal? Quantity { get; set; }

            public virtual decimal? Cost { get; set; }

            public virtual decimal? Cif { get; set; }

            public virtual decimal? Fob { get; set; }         

            public virtual decimal? Freight { get; set; }

            public virtual decimal? Insurance { get; set; }

            public virtual decimal? Thc { get; set; }

            public virtual decimal? Tax { get; set; }

            public virtual decimal? InLand { get; set; }

            public virtual decimal   Amount { get; set; }

            public virtual decimal? PeriodId { get; set; }
  
            public virtual decimal? CostVn { get; set; }

            public virtual decimal? CifVn { get; set; }

            public virtual decimal? FobVn { get; set; }

            public virtual decimal? FreightVn { get; set; }

            public virtual decimal? InsuranceVn { get; set; }

            public virtual decimal? ThcVn { get; set; }

            public virtual decimal? TaxVn { get; set; }

            public virtual decimal? InlandVn { get; set; }

            public virtual decimal? AmountVn { get; set; }

            public virtual decimal? WipId { get; set; }

            public virtual decimal? InStockId { get; set; }

            public virtual decimal? MappingId { get; set; }
             public DateTime? DateIn { get; set; }
    }
}
