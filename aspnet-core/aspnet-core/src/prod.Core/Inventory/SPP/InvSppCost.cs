using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.SPP
{

    [Table("InvSppCost")]
    [Index(nameof(PartNo), Name = "IX_InvSppCost_PartNo")]
    [Index(nameof(InvoiceNo), Name = "IX_InvSppCost_InvoiceNo")]
    [Index(nameof(Stock), Name = "IX_InvSppCost_Stock")]
    [Index(nameof(Month), nameof(Year), Name = "IX_InvSppCost_Month_Year")]
    [Index(nameof(PeriodId), Name = "IX_InvSppCost_PeriodId")]
    [Index(nameof(Stock), nameof(PeriodId), Name = "IX_InvSppCost_Stock_PeriodId")]
    [Index(nameof(Stock), nameof(Month), nameof(Year), Name = "IX_InvSppCost_Stock_Month_Year")]
    [Index(nameof(PartNo), nameof(Stock), nameof(Month), nameof(Year), Name = "IX_InvSppCost_PartNo_Stock_Month_Year")]
    [Index(nameof(PartNo), nameof(Stock), nameof(PeriodId), Name = "IX_InvSppCost_PartNo_Stock_PeriodId")]
    [Index(nameof(InvoiceNo), nameof(PartNo), nameof(Stock), nameof(Month), nameof(Year), Name = "IX_InvSppCost_InvoiceNo_PartNo_Stock_Month_Year")]
    [Index(nameof(InvoiceNo), nameof(PartNo), nameof(Stock), nameof(PeriodId), Name = "IX_InvSppCost_InvoiceNo_PartNo_Stock_PeriodId")]


    public class InvSppCost : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxPartNoLength = 255;

        public const int MaxInvoiceLength = 50;

        public const int MaxStockLength = 50;

        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(MaxInvoiceLength)]
        public virtual string InvoiceNo { get; set; }

        public virtual decimal? ReciveQty { get; set; }

        public virtual decimal? Price { get; set; }

        public virtual decimal? Amount { get; set; }

        public virtual decimal? PriceVn { get; set; }

        public virtual decimal? AmountVn { get; set; }

        [StringLength(MaxStockLength)]
        public virtual string Stock { get; set; }

        public virtual int? Month { get; set; }

        public virtual int? Year { get; set; }

        public virtual int? PeriodId { get; set; }

    }

}
