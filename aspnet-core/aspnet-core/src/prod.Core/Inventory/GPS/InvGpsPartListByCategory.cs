using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace prod.Inventory.GPS
{
    [Table("InvGpsPartListByCategory_T")]
    public class InvGpsPartListByCategory : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxItemLength = 50;

        public const int MaxDescriptionLength = 100;

        public const int MaxUomLength = 10;

        public const int MaxCatetoryLength = 50;

        public const int MaxLocationLength = 50;

        public const int MaxExpenseAccountLength = 10;

        public const int MaxGroupLength = 1;

        public const int MaxCurrentCategoryLength = 50;

        public const int MaxPartTypeLength = 10;

        [StringLength(MaxItemLength)]
        public virtual string Item { get; set; }

        [StringLength(MaxDescriptionLength)]
        public virtual string Description { get; set; }

        [StringLength(MaxUomLength)]
        public virtual string Uom { get; set; }

        [StringLength(MaxCatetoryLength)]
        public virtual string Category { get; set; }

        [StringLength(MaxLocationLength)]
        public virtual string Location { get; set; }

        [StringLength(MaxExpenseAccountLength)]
        public virtual string ExpenseAccount { get; set; }

        [StringLength(MaxGroupLength)]
        public virtual string Group { get; set; }

        [StringLength(MaxCurrentCategoryLength)]
        public virtual string CurrentCategory { get; set; }

        [StringLength(MaxPartTypeLength)]
        public virtual string PartType { get; set; }
    }
}
