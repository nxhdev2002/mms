using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Inventory
{
    [Table("MstInvHrGlCodeCombination")]
    [Index(nameof(ChartOfAccountsId), Name = "IX_MstInvHrGlCodeCombination_ChartOfAccountsId")]
    [Index(nameof(IsActive), Name = "IX_MstInvHrGlCodeCombination_IsActive")]
    public class MstInvHrGlCodeCombination : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxAccountTypeLength = 255;

        public const int MaxEnabledFlagLength = 50;

        public const int MaxSegment1Length = 50;

        public const int MaxSegment2Length = 50;

        public const int MaxSegment3Length = 50;

        public const int MaxSegment4Length = 50;

        public const int MaxSegment5Length = 50;

        public const int MaxSegment6Length = 50;

        public const int MaxConcatenatedsegmentsLength = 255;

        public const int MaxIsActiveLength = 1;

        public virtual long ChartOfAccountsId { get; set; }

        [StringLength(MaxAccountTypeLength)]
        public virtual string AccountType { get; set; }

        [StringLength(MaxEnabledFlagLength)]
        public virtual string EnabledFlag { get; set; }

        [StringLength(MaxSegment1Length)]
        public virtual string Segment1 { get; set; }

        [StringLength(MaxSegment2Length)]
        public virtual string Segment2 { get; set; }

        [StringLength(MaxSegment3Length)]
        public virtual string Segment3 { get; set; }

        [StringLength(MaxSegment4Length)]
        public virtual string Segment4 { get; set; }

        [StringLength(MaxSegment5Length)]
        public virtual string Segment5 { get; set; }

        [StringLength(MaxSegment6Length)]
        public virtual string Segment6 { get; set; }

        [StringLength(MaxConcatenatedsegmentsLength)]
        public virtual string Concatenatedsegments { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }
}
