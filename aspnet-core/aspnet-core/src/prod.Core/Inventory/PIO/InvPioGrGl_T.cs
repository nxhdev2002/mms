using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;

namespace prod.Inventory.CKD
{
    public class InvPioGrGl_T : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxGuidLength = 128;

        public const int MaxGradeLength = 50;

        public const int MaxPartNoLength = 50;

        public const int MaxTypeItemLength = 10;

        public const int MaxTypeLength = 1;

        public const int MaxErrorDescriptionLength = 5000;



        [StringLength(MaxGuidLength)]
        public virtual string Guid { get; set; }

        [StringLength(MaxGradeLength)]
        public virtual string Grade { get; set; }

        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(MaxTypeItemLength)]
        public virtual string TypeItem { get; set; }

        public virtual int? LastMonthQty { get; set; }

        public virtual int? UsageQty { get; set; }

        [StringLength(MaxTypeLength)]
        public virtual string Type { get; set; }

        public virtual DateTime? WorkingDate { get; set; }

        [StringLength(MaxErrorDescriptionLength)]
        public string ErrorDescription { get; set; }
    }
}
