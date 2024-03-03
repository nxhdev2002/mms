using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace prod.Inventory.SPP.Stock.Dto
{
    public class InvSppStockDto : EntityDto<long?>
    {
        public virtual string PartNo { get; set; }

        public virtual int? Month { get; set; }

        public virtual int? Year { get; set; }

        public virtual decimal? Amount { get; set; }

        public virtual int? Qty { get; set; }

        public virtual decimal? PreAmount { get; set; }

        public virtual int? PreQty { get; set; }

        public virtual decimal? Price { get; set; }

        public virtual decimal? PrePrice { get; set; }

        public virtual string Warehouse { get; set; }

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

    public class GetInvSppStockInput : PagedAndSortedResultRequestDto
    {
        public virtual string PartNo { get; set; }
        public virtual string Warehouse { get; set; }
        public virtual DateTime? FromMonthYear { get; set; }
        public virtual DateTime? ToMonthYear { get; set; }
        public virtual List<string> repoType { get; set; }
    }
    public class GetInvSppStockExportInput
    {
        public virtual string PartNo { get; set; }
        public virtual string Warehouse { get; set; }
        public virtual DateTime? FromMonthYear { get; set; }
        public virtual DateTime? ToMonthYear { get; set; }
        public virtual List<string> repoType { get; set; }
    }

    public class GetInvSppStockBalanceReportInput
    {
        public virtual string Stock { get; set; }
        public virtual List<string> repoType { get; set; }
        public virtual int CurrencyType { get; set; }
        public virtual DateTime FromMonthYear { get; set; }
        public virtual DateTime? ToMonthYear { get; set; }
    }

    public class InvSppStockBalanceReporttDto : EntityDto<long?>
    {
        public virtual string PartNo { get; set; }

        public virtual string ReportType { get; set; }

        public virtual int PreQty { get; set; }

        public virtual decimal? PreAmount { get; set; }

        public virtual int InQty { get; set; }

        public virtual decimal? InAmount { get; set; }

        public virtual int OutQty { get; set; }

        public virtual decimal? OutAmount { get; set; }

        public virtual int Qty { get; set; }

        public virtual decimal? Amount { get; set; }

    }

}
