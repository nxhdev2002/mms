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

namespace prod.Master.Inventory
{

    [Table("MstGpsMaterialRegisterByShop")]
    public class MstGpsMaterialRegisterByShop : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxPartNoLength = 50;

        public const int MaxDescriptionLength = 500;

        public const int MaxUomLength = 10;

        public const int MaxCategoryLength = 50;

        public const int MaxExpenseAccountLength = 50;

        public const int MaxShopLength = 100;

        public const int MaxCostCenterLength = 20;

        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(MaxDescriptionLength)]
        public virtual string Description { get; set; }

        [StringLength(MaxUomLength)]
        public virtual string Uom { get; set; }

        [StringLength(MaxCategoryLength)]
        public virtual string Category { get; set; }

        [StringLength(MaxExpenseAccountLength)]
        public virtual string ExpenseAccount { get; set; }

        [StringLength(MaxShopLength)]
        public virtual string Shop { get; set; }

        [StringLength(MaxCostCenterLength)]
        public virtual string CostCenter { get; set; }
    }
}
