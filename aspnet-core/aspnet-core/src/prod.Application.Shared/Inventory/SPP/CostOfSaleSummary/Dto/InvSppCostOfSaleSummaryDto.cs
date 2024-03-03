using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace prod.Inventory.SPP.CostOfSaleSummary.Dto
{
    public class InvSppCostOfSaleSummaryDto : EntityDto<long?>
    {

        public virtual string CustomerNo { get; set; }

        public virtual int PartsQty { get; set; }
            
        public virtual decimal? PartsCost { get; set; }

        public virtual decimal? PartsSaleAmount { get; set; }

        public virtual int ExportQty { get; set; }

        public virtual decimal? ExportCost { get; set; }

        public virtual decimal? ExportSaleAmount { get; set; }
        public virtual int OnhandAdjustmentQty { get; set; }

        public virtual decimal? OnhandAdjustmentCost { get; set; }

        public virtual decimal? OnhandAdjustmentSaleAmount { get; set; }
        public virtual int InternalQty { get; set; }

        public virtual decimal? InternalCost { get; set; }

        public virtual decimal? InternalSaleAmount { get; set; }
        public virtual int OthersQty { get; set; }

        public virtual decimal? OthersCost { get; set; }

        public virtual decimal? OthersSaleAmount { get; set; }
    }

    public class GetInvSppCostOfSaleSummaryInput : PagedAndSortedResultRequestDto
    {
        public virtual string CustomerNo { get; set; }
        public virtual DateTime? FromMonthYear { get; set; }
        public virtual DateTime? ToMonthYear { get; set; }
        public virtual string Goodstype { get;}
    }
    public class GetInvSppCostOfSaleSummaryExportInput
    {
        public virtual string CustomerNo { get; set; }
        public virtual DateTime? FromMonthYear { get; set; }
        public virtual DateTime? ToMonthYear { get; set; }
        public virtual string Goodstype { get; }
    }
}
