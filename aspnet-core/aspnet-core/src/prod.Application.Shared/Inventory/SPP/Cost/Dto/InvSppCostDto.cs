using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace prod.Inventory.SPP.Cost.Dto
{
    public class InvSppCostDto : EntityDto<long?>
    {

        public virtual string PartNo { get; set; }

        public virtual string InvoiceNo { get; set; }

        public virtual decimal? ReciveQty { get; set; }

        public virtual decimal? Price { get; set; }

        public virtual decimal? Amount { get; set; }

        public virtual decimal? PriceVn { get; set; }

        public virtual decimal? AmountVn { get; set; }

        public virtual string Stock { get; set; }

        public virtual int? Month { get; set; }

        public virtual int? Year { get; set; }

        public virtual int? PeriodId { get; set; }
    }

    public class GetInvSppCostInput : PagedAndSortedResultRequestDto
    {
        public virtual string PartNo { get; set; }
        public virtual string InvoiceNo { get; set; }
        public virtual string Stock { get; set; }
        public virtual DateTime? FromMonthYear { get; set; }
        public virtual DateTime? ToMonthYear { get; set; }
    }
    public class GetInvSppCostExportInput
    {
        public virtual string PartNo { get; set; }
        public virtual string InvoiceNo { get; set; }
        public virtual string Stock { get; set; }
        public virtual DateTime? FromMonthYear { get; set; }
        public virtual DateTime? ToMonthYear { get; set; }
    }
}
