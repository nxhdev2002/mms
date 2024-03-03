using Abp.Application.Services.Dto;
using prod.Master.Inv;
using System.ComponentModel.DataAnnotations;

namespace prod.Master.Inventory.Dto
{
    public class MstGpsMaterialCategoryMappingDto : EntityDto<long?>
    {
        public virtual string YVCategory { get; set; }
        public virtual string GLExpenseAccount { get; set; }
        public virtual string GLLevel5InWBS { get; set; }
        public virtual string GLAccountDescription { get; set; }
        public virtual string Definition { get; set; }
        public virtual string FixedVariableCost { get; set; }
        public virtual string Example { get; set; }
        public virtual string AccountType { get; set; }
        public virtual string PostingKey { get; set; }
        public virtual string PartType { get; set; }
        public virtual string DocumentType { get; set; }
        public virtual string IsAsset { get; set; }
        public virtual string RevertCancel { get; set; }
        public virtual string IsActive { get; set; }
    }
    public class CreateOrEditMstGpsMaterialCategoryMappingDto : EntityDto<long?>
    {

        [StringLength(MstGpsMaterialCategoryMappingConsts.MaxYVCategoryLength)]
        public virtual string YVCategory { get; set; }

        [StringLength(MstGpsMaterialCategoryMappingConsts.MaxGLExpenseAccountLength)]
        public virtual string GLExpenseAccount { get; set; }

        [StringLength(MstGpsMaterialCategoryMappingConsts.MaxGLLevel5InWBSLength)]
        public virtual string GLLevel5InWBS { get; set; }

        [StringLength(MstGpsMaterialCategoryMappingConsts.MaxGLAccountDescriptionLength)]
        public virtual string GLAccountDescription { get; set; }

        [StringLength(MstGpsMaterialCategoryMappingConsts.MaxDefinitionLength)]
        public virtual string Definition { get; set; }

        [StringLength(MstGpsMaterialCategoryMappingConsts.MaxFixedVariableCostLength)]
        public virtual string FixedVariableCost { get; set; }

        [StringLength(MstGpsMaterialCategoryMappingConsts.MaxExampleLength)]
        public virtual string Example { get; set; }

        [StringLength(MstGpsMaterialCategoryMappingConsts.MaxAccountTypeLength)]
        public virtual string AccountType { get; set; }

        [StringLength(MstGpsMaterialCategoryMappingConsts.MaxPostingKeyLength)]
        public virtual string PostingKey { get; set; }

        [StringLength(MstGpsMaterialCategoryMappingConsts.MaxPartTypeLength)]
        public virtual string PartType { get; set; }

        [StringLength(MstGpsMaterialCategoryMappingConsts.MaxDocumentTypeLength)]
        public virtual string DocumentType { get; set; }

        [StringLength(MstGpsMaterialCategoryMappingConsts.MaxIsAssetLength)]
        public virtual string IsAsset { get; set; }

        [StringLength(MstGpsMaterialCategoryMappingConsts.MaxRevertCancelLength)]
        public virtual string RevertCancel { get; set; }

        [StringLength(MstGpsMaterialCategoryMappingConsts.MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

    public class GetMstGpsMaterialCategoryMappingInput : PagedAndSortedResultRequestDto
    {

        public virtual string YVCategory { get; set; }

        public virtual string GLExpenseAccount { get; set; }


    }
    public class GetMstGpsMaterialCategoryMappingExcelInput
    {

        public virtual string YVCategory { get; set; }

        public virtual string GLExpenseAccount { get; set; }


    }
    public class MstGpsMaterialCategoryMappingImportDto : EntityDto<long?>
    {
        public virtual string YVCategory { get; set; }

        public virtual string GLExpenseAccount { get; set; }

        public virtual string GLLevel5InWBS { get; set; }

        public virtual string GLAccountDescription { get; set; }

        public virtual string Definition { get; set; }

        public virtual string FixedVariableCost { get; set; }

        public virtual string Example { get; set; }

        public virtual string IsActive { get; set; }
        public virtual string Guid { get; set; }

        public virtual string PostingKey { get; set; }

        public virtual string PartType { get; set; }

        public virtual string RevertCancel { get; set; }

        public virtual string AccountType { get; set; }

        public virtual string DocumentType { get; set; }

        public virtual string IsAsset { get; set; }

        public virtual long? CreatorUserId { get; set; }
    }


    public class ListCategoryDto
    {
        public virtual string YVCategory { get; set; }
    }

    public class GetMaterialCategoryMappingHistoryInput : PagedAndSortedResultRequestDto
    {
        public virtual long Id { get; set;}
        public virtual string TableName { get; set; }
    }
     public class GetMaterialCategoryMappingHistoryExcelInput 
    {
        public virtual long Id { get; set;}
        public virtual string TableName { get; set; }
    }

}
