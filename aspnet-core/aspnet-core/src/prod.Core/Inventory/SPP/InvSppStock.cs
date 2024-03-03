using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.SPP
{
    [Table("InvSppStock")]
    [Index(nameof(PartNo), Name = "IX_InvSppStock_PartNo")]
    [Index(nameof(Warehouse), Name = "IX_InvSppStock_Warehouse")]
    [Index(nameof(Month), nameof(Year), Name = "IX_InvSppStock_Month_Year")]
    [Index(nameof(PeriodId), Name = "IX_InvSppStock_PeriodId")]
    [Index(nameof(Warehouse), nameof(PeriodId), Name = "IX_InvSppStock_Warehouse_PeriodId")]
    [Index(nameof(Warehouse), nameof(Month), nameof(Year), Name = "IX_InvSppStock_Warehouse_Month_Year")]
    [Index(nameof(PartNo), nameof(Warehouse), nameof(Month), nameof(Year), Name = "IX_InvSppStock_PartNo_Warehouse_Month_Year")]
    [Index(nameof(PartNo), nameof(Warehouse), nameof(PeriodId), Name = "IX_InvSppStock_PartNo_Warehouse_PeriodId")]

    public class InvSppStock : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxPartNoLength = 255;
        public const int MaxWarehouseLength = 5;


        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        public virtual int? Month { get; set; }

        public virtual int? Year { get; set; }

        public virtual decimal? Amount { get; set; }

        public virtual int? Qty { get; set; }

        public virtual decimal? PreAmount { get; set; }

        public virtual int? PreQty { get; set; }

        public virtual decimal? Price { get; set; }

        public virtual decimal? PrePrice { get; set; }

        [StringLength(MaxWarehouseLength)]
        public virtual string Warehouse { get; set; }

        public virtual int? PeriodId { get; set; }

        public virtual decimal? PriceVn { get; set; }

        public virtual decimal? PrePriceVn { get; set; }

        public virtual decimal? AmountVn { get; set; }

        public virtual decimal? PreAmountVn { get; set; }

        public virtual int? InQty { get; set; }

        public virtual decimal? InAmount { get; set; }

        public virtual decimal? InPrice { get; set; }

        public virtual int? OutQty { get; set; }

        public virtual decimal? OutAmount { get; set; }

        public virtual decimal? OutPrice { get; set; }

        public virtual decimal? InAmountVn { get; set; }

        public virtual decimal? InPriceVn { get; set; }

        public virtual decimal? OutAmountVn { get; set; }

        public virtual decimal? OutPriceVn { get; set; }
    }
}
