using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.CKD
{

    [Table("InvCkdProdPlanDaily")]
    public class InvCkdProdPlanDaily : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxProdlineLength = 2;

        public const int MaxCfcLength = 4;

        public const int MaxModelLength = 1;

        public const int MaxGradeLength = 2;

        public const int MaxColorLength = 20;

        public const int MaxBodynoLength = 10;

        public const int MaxLotnoLength = 20;

        public const int MaxVinnoLength = 17;

        public const int MaxIsprojectLength = 1;

        public const int MaxVehicleidLength = 10;

        public const int MaxIndentlineLength = 3;

        public const int MaxColortypeLength = 4;

        public const int MaxEngineidLength = 12;

        public const int MaxGoshicarLength = 1;

        public const int MaxSalessfxLength = 2;

        public const int MaxSsnoLength = 2;

        public const int MaxTransidLength = 18;

        public virtual long? No { get; set; }

        [StringLength(MaxProdlineLength)]
        public virtual string Prodline { get; set; }

        [StringLength(MaxCfcLength)]
        public virtual string Cfc { get; set; }

        [StringLength(MaxModelLength)]
        public virtual string Model { get; set; }

        [StringLength(MaxGradeLength)]
        public virtual string Grade { get; set; }

        [StringLength(MaxColorLength)]
        public virtual string Color { get; set; }

        [StringLength(MaxBodynoLength)]
        public virtual string Bodyno { get; set; }

        [StringLength(MaxLotnoLength)]
        public virtual string Lotno { get; set; }

        public virtual int? Noinlot { get; set; }

        [StringLength(MaxVinnoLength)]
        public virtual string Vinno { get; set; }

        public virtual DateTime? Winplandate { get; set; }

        public virtual TimeSpan? Winplantime { get; set; }

        public virtual DateTime? Winplandatetime { get; set; }

        public virtual DateTime? Woutplandate { get; set; }

        public virtual TimeSpan? Woutplantime { get; set; }

        public virtual DateTime? Woutplandatetime { get; set; }

        public virtual DateTime? Tinplandate { get; set; }

        public virtual TimeSpan? Tinplantime { get; set; }

        public virtual DateTime? Tinplandatetime { get; set; }

        public virtual DateTime? Toutplandate { get; set; }

        public virtual TimeSpan? Toutplantime { get; set; }

        public virtual DateTime? Toutplandatetime { get; set; }

        public virtual DateTime? Ainplandate { get; set; }

        public virtual TimeSpan? Ainplantime { get; set; }

        public virtual DateTime? Ainplandatetime { get; set; }

        public virtual DateTime? Aoutplandate { get; set; }

        public virtual TimeSpan? Aoutplantime { get; set; }

        public virtual DateTime? Aoutplandatetime { get; set; }

        public virtual DateTime? Lineoffdate { get; set; }

        public virtual TimeSpan? Lineofftime { get; set; }

        public virtual DateTime? Lineoffdatetime { get; set; }

        public virtual DateTime? Pdidate { get; set; }

        public virtual TimeSpan? Pditime { get; set; }

        public virtual DateTime? Pdidatetime { get; set; }

        [StringLength(MaxIsprojectLength)]
        public virtual string Isproject { get; set; }

        [StringLength(MaxVehicleidLength)]
        public virtual string Vehicleid { get; set; }

        [StringLength(MaxIndentlineLength)]
        public virtual string Indentline { get; set; }

        [StringLength(MaxColortypeLength)]
        public virtual string Colortype { get; set; }

        [StringLength(MaxEngineidLength)]
        public virtual string Engineid { get; set; }

        [StringLength(MaxGoshicarLength)]
        public virtual string Goshicar { get; set; }

        [StringLength(MaxSalessfxLength)]
        public virtual string Salessfx { get; set; }

        [StringLength(MaxSsnoLength)]
        public virtual string Ssno { get; set; }

        [StringLength(MaxTransidLength)]
        public virtual string Transid { get; set; }
    }

}


