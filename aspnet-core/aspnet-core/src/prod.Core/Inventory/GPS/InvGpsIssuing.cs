using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace prod.Inventory.GPS
{
    public class InvGpsIssuing : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxPartNoLength = 12;

        public const int MaxPartNameLength = 300;

        public const int MaxUomLength = 50;

        public const int MaxLotNoLength = 50;


        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(MaxPartNameLength)]
        public virtual string PartName { get; set; }

        [StringLength(MaxUomLength)]
        public virtual string Uom { get; set; }

        public virtual int? BoxQty { get; set; }

        public virtual int? Box { get; set; }

        public virtual int? QtyRequest { get; set; }

        [StringLength(MaxLotNoLength)]
        public virtual string LotNo { get; set; }

        public virtual DateTime? ProdDate { get; set; }

        public virtual DateTime? ExpDate { get; set; }

        public virtual string CostCenter { get; set; }

        public virtual int? QtyIssue { get; set; }

        public virtual string IsIssue { get; set; }

        public virtual DateTime? IssueDate { get; set; }

        public virtual string Status { get; set; }

        public virtual string IsGentani { get; set; }

    }
}
