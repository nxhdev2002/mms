using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Inventory
{
    [Table("MstGpsMaterialCategoryMapping")]
    public class MstGpsMaterialCategoryMapping : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxYVCategoryLength = 50;

        public const int MaxGLExpenseAccountLength = 10;

        public const int MaxGLLevel5InWBSLength = 8;

        public const int MaxGLAccountDescriptionLength = 255;

        public const int MaxDefinitionLength = 1000;

        public const int MaxFixedVariableCostLength = 1;

        public const int MaxExampleLength = 1000;

        public const int MaxAccountTypeLength = 2;

        public const int MaxPostingKeyLength = 2;

        public const int MaxPartTypeLength = 3;

        public const int MaxDocumentTypeLength = 2;

        public const int MaxIsAssetLength = 1;

        public const int MaxIsActiveLength = 1;

        public const int MaxRevertCancelLength = 1;

        [StringLength(MaxYVCategoryLength)]
        public virtual string YVCategory { get; set; }

        [StringLength(MaxGLExpenseAccountLength)]
        public virtual string GLExpenseAccount { get; set; }

        [StringLength(MaxGLLevel5InWBSLength)]
        public virtual string GLLevel5InWBS { get; set; }

        [StringLength(MaxGLAccountDescriptionLength)]
        public virtual string GLAccountDescription { get; set; }

        [StringLength(MaxDefinitionLength)]
        public virtual string Definition { get; set; }

        [StringLength(MaxFixedVariableCostLength)]
        public virtual string FixedVariableCost { get; set; }

        [StringLength(MaxExampleLength)]
        public virtual string Example { get; set; }

        [StringLength(MaxAccountTypeLength)]
        public virtual string AccountType { get; set; }

        [StringLength(MaxPostingKeyLength)]
        public virtual string PostingKey { get; set; }

        [StringLength(MaxPartTypeLength)]
        public virtual string PartType { get; set; }

        [StringLength(MaxDocumentTypeLength)]
        public virtual string DocumentType { get; set; }

        [StringLength(MaxIsAssetLength)]
        public virtual string IsAsset { get; set; }

        [StringLength(MaxRevertCancelLength)]
        public virtual string RevertCancel { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }

    }
}
