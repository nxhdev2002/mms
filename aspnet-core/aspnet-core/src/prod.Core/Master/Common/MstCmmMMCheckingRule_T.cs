using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Cmm
{

    [Table("MstCmmMMCheckingRule_T")]
    public class MstCmmMMCheckingRule_T : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxRuleCodeLength = 50;

        public const int MaxRuleDescriptionLength = 5000;

        public const int MaxRuleItemLength = 50;

        public const int MaxField1NameLength = 50;

        public const int MaxField1ValueLength = 50;

        public const int MaxField2NameLength = 50;

        public const int MaxField2ValueLength = 50;

        public const int MaxField3NameLength = 50;

        public const int MaxField3ValueLength = 50;

        public const int MaxField4NameLength = 50;

        public const int MaxField4ValueLength = 50;

        public const int MaxField5NameLength = 50;

        public const int MaxField5ValueLength = 50;

        public const int MaxOptionLength = 50;

        public const int MaxResultfieldLength = 50;

        public const int MaxExpectedresultLength = 50;

        public const int MaxCheckoptionLength = 50;

        public const int MaxErrormessageLength = 5000;

        public const int MaxIsActiveLength = 1;

        [StringLength(128)]
        public virtual string Guid { get; set; }

        [StringLength(MaxRuleCodeLength)]
        public virtual string RuleCode { get; set; }

        [StringLength(MaxRuleDescriptionLength)]
        public virtual string RuleDescription { get; set; }

        [StringLength(MaxRuleItemLength)]
        public virtual string RuleItem { get; set; }

        [StringLength(MaxField1NameLength)]
        public virtual string Field1Name { get; set; }

        [StringLength(MaxField1ValueLength)]
        public virtual string Field1Value { get; set; }

        [StringLength(MaxField2NameLength)]
        public virtual string Field2Name { get; set; }

        [StringLength(MaxField2ValueLength)]
        public virtual string Field2Value { get; set; }

        [StringLength(MaxField3NameLength)]
        public virtual string Field3Name { get; set; }

        [StringLength(MaxField3ValueLength)]
        public virtual string Field3Value { get; set; }

        [StringLength(MaxField4NameLength)]
        public virtual string Field4Name { get; set; }

        [StringLength(MaxField4ValueLength)]
        public virtual string Field4Value { get; set; }

        [StringLength(MaxField5NameLength)]
        public virtual string Field5Name { get; set; }

        [StringLength(MaxField5ValueLength)]
        public virtual string Field5Value { get; set; }

        [StringLength(MaxOptionLength)]
        public virtual string Option { get; set; }

        [StringLength(MaxResultfieldLength)]
        public virtual string Resultfield { get; set; }

        [StringLength(MaxExpectedresultLength)]
        public virtual string Expectedresult { get; set; }

        [StringLength(MaxCheckoptionLength)]
        public virtual string Checkoption { get; set; }

        [StringLength(MaxErrormessageLength)]
        public virtual string Errormessage { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}

