using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.Tmss
{

    [Table("InvTmssDispatchPlan")]
    public class InvTmssDispatchPlan : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxVehicleTypeLength = 10;

        public const int MaxModelLength = 50;

        public const int MaxMarketingCodeLength = 50;

        public const int MaxProductionCodeLength = 50;

        public const int MaxDealerLength = 50;

        public const int MaxVinLength = 100;

        public const int MaxExtColorLength = 50;

        public const int MaxIntColorLength = 50;


        [StringLength(MaxVehicleTypeLength)]
        public virtual string VehicleType { get; set; }

        [StringLength(MaxModelLength)]
        public virtual string Model { get; set; }

        [StringLength(MaxMarketingCodeLength)]
        public virtual string MarketingCode { get; set; }

        [StringLength(MaxProductionCodeLength)]
        public virtual string ProductionCode { get; set; }

        [StringLength(MaxDealerLength)]
        public virtual string Dealer { get; set; }

        [StringLength(MaxVinLength)]
        public virtual string Vin { get; set; }

        [StringLength(MaxExtColorLength)]
        public virtual string ExtColor { get; set; }

        [StringLength(MaxIntColorLength)]
        public virtual string IntColor { get; set; }

        [Column(TypeName = "Date")]
        public virtual DateTime? DlrDispatchPlan { get; set; }

        [Column(TypeName = "Date")]
        public virtual DateTime? DlrDispatchDate { get; set; }
    }

}
