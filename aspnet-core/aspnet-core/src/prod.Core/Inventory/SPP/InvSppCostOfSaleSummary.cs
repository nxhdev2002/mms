using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.SPP
{

    [Table("InvSppCostOfSaleSummary")]
    public class InvSppCostOfSaleSummary : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxCustomerNoLength = 50;

        public const int MaxPartNoLength = 255;

        public const int MaxGoodstypeLength = 50;


        [StringLength(MaxCustomerNoLength)]
        public virtual string CustomerNo { get; set; }

        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }
        public virtual int? Qty { get; set; }

        public virtual decimal? Cost { get; set; }

        public virtual decimal? CostVn { get; set; }

        public virtual decimal? SaleAmount { get; set; }

        public virtual int Month { get; set; }

        public virtual int Year { get; set; }

        public virtual long PeriodId { get; set; }
            
        [StringLength(MaxGoodstypeLength)]
        public virtual string Goodstype { get; set; }

    }

}
