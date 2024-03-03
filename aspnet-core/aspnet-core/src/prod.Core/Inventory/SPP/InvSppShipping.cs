using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using PayPalCheckoutSdk.Orders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.SPP
{

    [Table("InvSppShipping")]
    [Index(nameof(PartNo), Name = "IX_InvSppShipping_PartNo")]
    [Index(nameof(CustomerNo), Name = "IX_InvSppShipping_CustomerNo")]
    [Index(nameof(InvoiceNo), Name = "IX_InvSppShipping_InvoiceNo")]
    [Index(nameof(CustomerOrderNo), Name = "IX_InvSppShipping_CustomerOrderNo")]
    [Index(nameof(Stock), Name = "IX_InvSppShipping_Stock")]
    [Index(nameof(Month), nameof(Year), Name = "IX_InvSppShipping_Month_Year")]
    [Index(nameof(PeriodId), Name = "IX_InvSppShipping_PeriodId")]
    [Index(nameof(Stock), nameof(PeriodId), Name = "IX_InvSppShipping_Stock_PeriodId")]
    [Index(nameof(Stock), nameof(Month), nameof(Year), Name = "IX_InvSppShipping_Stock_Month_Year")]
    [Index(nameof(PartNo), nameof(Stock), nameof(Month), nameof(Year), Name = "IX_InvSppShipping_PartNo_Stock_Month_Year")]
    [Index(nameof(PartNo), nameof(Stock), nameof(PeriodId), Name = "IX_InvSppShipping_PartNo_Stock_PeriodId")]
    [Index(nameof(InvoiceNo), nameof(PartNo), nameof(Stock), nameof(Month), nameof(Year), Name = "IX_InvSppShipping_InvoiceNo_PartNo_Stock_Month_Year")]
    [Index(nameof(InvoiceNo), nameof(PartNo), nameof(Stock), nameof(PeriodId), Name = "IX_InvSppShipping_InvoiceNo_PartNo_Stock_PeriodId")]
    [Index(nameof(CustomerOrderNo), nameof(Stock), nameof(Month), nameof(Year), Name = "IX_InvSppShipping_CustomerOrderNo_Stock_Month_Year")]
    [Index(nameof(CustomerOrderNo), nameof(Stock), nameof(PeriodId), Name = "IX_InvSppShipping_CustomerOrderNo_Stock_PeriodId")]


    public class InvSppShipping : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxPartNoLength = 255;

        public const int MaxPartNameLength = 255;

        public const int MaxCustomerOrderNoLength = 50;

        public const int MaxCustomerNoLength = 50;

        public const int MaxInvoiceNoLength = 50;

        public const int MaxExporterLength = 50;

        public const int MaxRemarkLength = 1000;

        public const int MaxStockLength = 5;

        public const int MaxCorrectionCdLength = 50;

        public const int MaxRouteNoLength = 50;

        public const int MaxItemNoLength = 15;

        public const int MaxSalesPriceCdLength = 1;

        public const int MaxCorrectionReasonLength = 1;

        public const int MaxFrCdLength = 50;


        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(MaxPartNameLength)]
        public virtual string PartName { get; set; }

        [StringLength(MaxCustomerOrderNoLength)]
        public virtual string CustomerOrderNo { get; set; }

        [StringLength(MaxCustomerNoLength)]
        public virtual string CustomerNo { get; set; }

        [StringLength(MaxInvoiceNoLength)]
        public virtual string InvoiceNo { get; set; }

        public virtual float? CustomerType { get; set; }

        [StringLength(MaxExporterLength)]
        public virtual string Exporter { get; set; }

        public virtual int? OrderQty { get; set; }

        public virtual int? AllcocatedQty { get; set; }

        public virtual decimal? SalePrice { get; set; }

        [StringLength(MaxRemarkLength)]
        public virtual string Remark { get; set; }

        public virtual decimal? SaleAmount { get; set; }

        public virtual int? Month { get; set; }
        public virtual int? Year { get; set; }

        [StringLength(MaxStockLength)]
        public virtual string Stock { get; set; }

        [StringLength(MaxCorrectionCdLength)]
        public virtual string CorrectionCd { get; set; }

        [StringLength(MaxRouteNoLength)]
        public virtual string RouteNo { get; set; }

        public virtual int? PeriodId { get; set; }

        public virtual decimal? Price { get; set; }

        public virtual decimal? PriceVn { get; set; }

        public virtual decimal? SalePriceUsd { get; set; }

        public virtual decimal? SaleAmountUsd { get; set; }

        [StringLength(MaxItemNoLength)]
        public virtual string ItemNo { get; set; }

        public virtual DateTime? ProcessDate { get; set; }

        [StringLength(MaxSalesPriceCdLength)]
        public virtual string SalesPriceCd { get; set; }

        [StringLength(MaxCorrectionReasonLength)]
        public virtual string CorrectionReason { get; set; }

        [StringLength(MaxFrCdLength)]
        public virtual string FrCd { get; set; }
    }

}


