using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Cmm
{
    [Table("MstCmmMMValidationResult")]
    public class MstCmmMMValidationResult : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxMaterialCodeLength = 5000;

        public const int MaxMaterialNameLength = 50;

        public const int MaxMaterialGroupLength = 50;

        public const int MaxValuationClassLength = 50;

        public const int MaxRuleCodeLength = 50;

        public const int MaxRuleDescriptionLength = 50;

        public const int MaxRuleItemLength = 50;

        public const int MaxOptionLength = 50;

        public const int MaxResultFieldLength = 50;

        public const int MaxExpectedResultLength = 50;

        public const int MaxActualResultLength = 50;

        public const int MaxLastValidationDatetimeLength = 50;

        public const int MaxLastvalidationbyLength = 50;

        public const int MaxStatusLength = 50;

        public const int MaxErrorMessageLength = 5000;

        public const int MaxIsactiveLength = 1;

        public virtual long? MateriaId { get; set; }

        [StringLength(MaxMaterialCodeLength)]
        public virtual string MaterialCode { get; set; }

        [StringLength(MaxMaterialNameLength)]
        public virtual string MaterialName { get; set; }

        [StringLength(MaxMaterialGroupLength)]
        public virtual string MaterialGroup { get; set; }

        [StringLength(MaxValuationClassLength)]
        public virtual string ValuationClass { get; set; }

        public virtual long? RuleId { get; set; }

        [StringLength(MaxRuleCodeLength)]
        public virtual string RuleCode { get; set; }

        [StringLength(MaxRuleDescriptionLength)]
        public virtual string RuleDescription { get; set; }

        [StringLength(MaxRuleItemLength)]
        public virtual string RuleItem { get; set; }

        [StringLength(MaxOptionLength)]
        public virtual string Option { get; set; }

        [StringLength(MaxResultFieldLength)]
        public virtual string ResultField { get; set; }

        [StringLength(MaxExpectedResultLength)]
        public virtual string ExpectedResult { get; set; }

        [StringLength(MaxActualResultLength)]
        public virtual string ActualResult { get; set; }

        [StringLength(MaxLastValidationDatetimeLength)]
        public virtual string LastValidationDatetime { get; set; }

        [StringLength(MaxLastvalidationbyLength)]
        public virtual string Lastvalidationby { get; set; }

        public virtual long? LastValidationId { get; set; }

        [StringLength(MaxStatusLength)]
        public virtual string Status { get; set; }

        [StringLength(MaxErrorMessageLength)]
        public virtual string ErrorMessage { get; set; }

        [StringLength(MaxIsactiveLength)]
        public virtual string IsActive { get; set; }
    }

}
