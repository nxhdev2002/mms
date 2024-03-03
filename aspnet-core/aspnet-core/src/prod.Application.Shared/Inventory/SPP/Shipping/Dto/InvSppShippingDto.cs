using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace prod.Inventory.SPP.Shipping.Dto
{
    public class InvSppShippingDto : EntityDto<long?>
    {
        public virtual string PartNo { get; set; }

        public virtual string PartName { get; set; }

        public virtual string CustomerOrderNo { get; set; }

        public virtual string CustomerNo { get; set; }

        public virtual string InvoiceNo { get; set; }

        public virtual float? CustomerType { get; set; }

        public virtual string Exporter { get; set; }

        public virtual int? OrderQty { get; set; }

        public virtual int? AllcocatedQty { get; set; }

        public virtual decimal? SalePrice { get; set; }

        public virtual string Remark { get; set; }

        public virtual decimal? SaleAmount { get; set; }

        public virtual int? Month { get; set; }
        public virtual int? Year { get; set; }

        public virtual string Stock { get; set; }

        public virtual string CorrectionCd { get; set; }

        public virtual string RouteNo { get; set; }

        public virtual decimal? Price { get; set; }

        public virtual decimal? PriceVn { get; set; }

        public virtual decimal? SalePriceUsd { get; set; }

        public virtual decimal? SaleAmountUsd { get; set; }

        public virtual string ItemNo { get; set; }

        public virtual DateTime? ProcessDate { get; set; }

        public virtual string SalesPriceCd { get; set; }

        public virtual string CorrectionReason { get; set; }

        public virtual string FrCd { get; set; }
    }

    public class GetInvSppShippingInput : PagedAndSortedResultRequestDto
    {
        public virtual string PartNo { get; set; }
        public virtual string CustomerNo { get; set; }
        public virtual string CustomerOrderNo { get; set; }
        public virtual string InvoiceNo { get; set; }
        public virtual string Stock { get; set; }
        public virtual DateTime? FromMonthYear { get; set; }
        public virtual DateTime? ToMonthYear { get; set; }
        public virtual List<string> repoType { get; set; }
    }
    public class GetInvSppShippingExportInput
    {
        public virtual string PartNo { get; set; }
        public virtual string CustomerNo { get; set; }
        public virtual string CustomerOrderNo { get; set; }
        public virtual string InvoiceNo { get; set; }
        public virtual string Stock { get; set; }
        public virtual DateTime? FromMonthYear { get; set; }
        public virtual DateTime? ToMonthYear { get; set; }

        public virtual List<string> repoType { get; set; }
    }
    // SPP Shipping Cost of Sale
    public class InvSppShippingCostOfSaleSummaryDto : EntityDto<long?>
    {
        public virtual string ReportType { get; set; }

        public virtual int Qty { get; set; }

        public virtual decimal? Cost { get; set; }

        public virtual decimal? SaleAmount { get; set; }

    }

    public class InvSppShippingCostOfSaleDetailDto : EntityDto<long?>
    {
        public virtual string CustomerNo { get; set; }

        public virtual string ReportType { get; set; }

        public virtual string SaleVoucher { get; set; }

        public virtual string PartNo { get; set; }

        public virtual int Qty { get; set; }

        public virtual decimal? Cost { get; set; }

        public virtual decimal? CostVn { get; set; }

        public virtual decimal? SalePrice { get; set; }
        
        public virtual decimal? SaleAmount { get; set; }

    }

    public class InvSppShippingGLTransactionDto : EntityDto<long?>
    {
        public virtual string TaiKhoan { get; set; }

        public virtual decimal? GhiNo { get; set; }

        public virtual decimal? GhiCo { get; set; }

        public virtual string MoTa { get; set; }

    }

    public class GetInvSppShippingCostOfSaleExportInput
    {
        public virtual string Stock { get; set; }
        public virtual int CurrencyType { get; set; }
        public virtual List<string> repoType { get; set; }
        public virtual DateTime FromMonthYear { get; set; }
        public virtual DateTime? ToMonthYear { get; set; }
    }




}
