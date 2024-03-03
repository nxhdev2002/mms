using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Common.Dto
{

    public class MstCmmMaterialMasterDto : EntityDto<long?>
    {

        public virtual string MaterialType { get; set; }

        public virtual string MaterialCode { get; set; }

        public virtual string IndustrySector { get; set; }

        public virtual string Description { get; set; }

        public virtual string MaterialGroup { get; set; }

        public virtual string BaseUnitOfMeasure { get; set; }

        public virtual string DeletionFlag { get; set; }

        public virtual string Plant { get; set; }

        public virtual string StorageLocation { get; set; }

        public virtual string ProductionGroup { get; set; }

        public virtual string ProductionPurpose { get; set; }

        public virtual string ProductionType { get; set; }

        public virtual string ProfitCenter { get; set; }

        public virtual string BatchManagement { get; set; }

        public virtual string ReservedStock { get; set; }

        public virtual string LotCode { get; set; }

        public virtual string LotCodeM { get; set; }

        public virtual string MrpGroup { get; set; }

        public virtual string MrpType { get; set; }

        public virtual string ProcurementType { get; set; }

        public virtual string SpecialProcurement { get; set; }

        public virtual string ProductionStorageLocation { get; set; }

        public virtual string RepetManufacturing { get; set; }

        public virtual string RemProfile { get; set; }

        public virtual string DoNotCost { get; set; }

        public virtual string VarianceKey { get; set; }

        public virtual decimal? CostingLotSize { get; set; }

        public virtual string ProductionVersion { get; set; }

        public virtual string SpecialProcurementCtgView { get; set; }

        public virtual string ValuationCategory { get; set; }

        public virtual string ValuationType { get; set; }

        public virtual string ValuationClass { get; set; }

        public virtual string PriceDetermination { get; set; }

        public virtual string PriceControl { get; set; }

        public virtual decimal? StandardPrice { get; set; }

        public virtual decimal? MovingPrice { get; set; }

        public virtual string WithQtyStructure { get; set; }

        public virtual string MaterialOrigin { get; set; }

        public virtual string OriginGroup { get; set; }

        public virtual string AuthorizationGroup { get; set; }

        public virtual string MatSrc { get; set; }

        public virtual DateTime? EffectiveDateFrom { get; set; }

        public virtual DateTime? EffectiveDateTo { get; set; }

        public virtual string IsActive { get; set; }
        
       
        

    }

    public class CreateOrEditMstCmmMaterialMasterDto : EntityDto<long?>
    {

        [StringLength(MstCmmMaterialMasterConsts.MaxMaterialTypeLength)]
        public virtual string MaterialType { get; set; }

        [StringLength(MstCmmMaterialMasterConsts.MaxMaterialCodeLength)]
        public virtual string MaterialCode { get; set; }

        [StringLength(MstCmmMaterialMasterConsts.MaxIndustrySectorLength)]
        public virtual string IndustrySector { get; set; }

        [StringLength(MstCmmMaterialMasterConsts.MaxDescriptionLength)]
        public virtual string Description { get; set; }

        [StringLength(MstCmmMaterialMasterConsts.MaxMaterialGroupLength)]
        public virtual string MaterialGroup { get; set; }

        [StringLength(MstCmmMaterialMasterConsts.MaxBaseUnitOfMeasureLength)]
        public virtual string BaseUnitOfMeasure { get; set; }

        [StringLength(MstCmmMaterialMasterConsts.MaxDeletionFlagLength)]
        public virtual string DeletionFlag { get; set; }

        [StringLength(MstCmmMaterialMasterConsts.MaxPlantLength)]
        public virtual string Plant { get; set; }

        [StringLength(MstCmmMaterialMasterConsts.MaxStorageLocationLength)]
        public virtual string StorageLocation { get; set; }

        [StringLength(MstCmmMaterialMasterConsts.MaxProductionGroupLength)]
        public virtual string ProductionGroup { get; set; }

        [StringLength(MstCmmMaterialMasterConsts.MaxProductionPurposeLength)]
        public virtual string ProductionPurpose { get; set; }

        [StringLength(MstCmmMaterialMasterConsts.MaxProductionTypeLength)]
        public virtual string ProductionType { get; set; }

        [StringLength(MstCmmMaterialMasterConsts.MaxProfitCenterLength)]
        public virtual string ProfitCenter { get; set; }

        [StringLength(MstCmmMaterialMasterConsts.MaxBatchManagementLength)]
        public virtual string BatchManagement { get; set; }

        [StringLength(MstCmmMaterialMasterConsts.MaxReservedStockLength)]
        public virtual string ReservedStock { get; set; }

        [StringLength(MstCmmMaterialMasterConsts.MaxLotCodeLength)]
        public virtual string LotCode { get; set; }

        [StringLength(MstCmmMaterialMasterConsts.MaxMrpGroupLength)]
        public virtual string MrpGroup { get; set; }

        [StringLength(MstCmmMaterialMasterConsts.MaxMrpTypeLength)]
        public virtual string MrpType { get; set; }

        [StringLength(MstCmmMaterialMasterConsts.MaxProcurementTypeLength)]
        public virtual string ProcurementType { get; set; }

        [StringLength(MstCmmMaterialMasterConsts.MaxSpecialProcurementLength)]
        public virtual string SpecialProcurement { get; set; }

        [StringLength(MstCmmMaterialMasterConsts.MaxProductionStorageLocationLength)]
        public virtual string ProductionStorageLocation { get; set; }

        [StringLength(MstCmmMaterialMasterConsts.MaxRepetManufacturingLength)]
        public virtual string RepetManufacturing { get; set; }

        [StringLength(MstCmmMaterialMasterConsts.MaxRemProfileLength)]
        public virtual string RemProfile { get; set; }

        [StringLength(MstCmmMaterialMasterConsts.MaxDoNotCostLength)]
        public virtual string DoNotCost { get; set; }

        [StringLength(MstCmmMaterialMasterConsts.MaxVarianceKeyLength)]
        public virtual string VarianceKey { get; set; }

        public virtual decimal? CostingLotSize { get; set; }

        [StringLength(MstCmmMaterialMasterConsts.MaxProductionVersionLength)]
        public virtual string ProductionVersion { get; set; }

        [StringLength(MstCmmMaterialMasterConsts.MaxSpecialProcurementCtgViewLength)]
        public virtual string SpecialProcurementCtgView { get; set; }

        [StringLength(MstCmmMaterialMasterConsts.MaxValuationCategoryLength)]
        public virtual string ValuationCategory { get; set; }

        [StringLength(MstCmmMaterialMasterConsts.MaxValuationTypeLength)]
        public virtual string ValuationType { get; set; }

        [StringLength(MstCmmMaterialMasterConsts.MaxValuationClassLength)]
        public virtual string ValuationClass { get; set; }

        [StringLength(MstCmmMaterialMasterConsts.MaxPriceDeterminationLength)]
        public virtual string PriceDetermination { get; set; }

        [StringLength(MstCmmMaterialMasterConsts.MaxPriceControlLength)]
        public virtual string PriceControl { get; set; }

        public virtual decimal? StandardPrice { get; set; }

        public virtual decimal? MovingPrice { get; set; }

        [StringLength(MstCmmMaterialMasterConsts.MaxWithQtyStructureLength)]
        public virtual string WithQtyStructure { get; set; }

        [StringLength(MstCmmMaterialMasterConsts.MaxMaterialOriginLength)]
        public virtual string MaterialOrigin { get; set; }

        [StringLength(MstCmmMaterialMasterConsts.MaxOriginGroupLength)]
        public virtual string OriginGroup { get; set; }

        [StringLength(MstCmmMaterialMasterConsts.MaxAuthorizationGroupLength)]
        public virtual string AuthorizationGroup { get; set; }

        [StringLength(MstCmmMaterialMasterConsts.MaxMatSrcLength)]
        public virtual string MatSrc { get; set; }

        public virtual DateTime? EffectiveDateFrom { get; set; }

        public virtual DateTime? EffectiveDateTo { get; set; }

        [StringLength(MstCmmMaterialMasterConsts.MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

    public class GetMstCmmMaterialMasterInput : PagedAndSortedResultRequestDto
    {
        public virtual string MaterialCode { get; set; }
        public virtual string MaterialGroup { get; set; }
        public virtual string ValuationType { get; set; }
      
    }

 
    public class MstCmmMMValidationResultDto : EntityDto<long?> 
    {
        public virtual long? MateriaId { get; set; }
        public virtual string MaterialCode { get; set; }
        public virtual string MaterialName { get; set; }
        public virtual string MaterialGroup { get; set; }
        public virtual string ValuationClass { get; set; }
        public virtual long? RuleId { get; set; }
        public virtual string RuleCode { get; set; }
        public virtual string RuleDescription { get; set; }
        public virtual string RuleItem { get; set; }

        public virtual string Option { get; set; }
        public virtual string ResultField { get; set; }
        public virtual string ExpectedResult { get; set; }
        public virtual string ActualResult { get; set; }
        public virtual DateTime? LastValidationDatetime { get; set; }
        public virtual string Lastvalidationby { get; set; }
        public virtual long? LastValidationId { get; set; }
        public virtual string Status { get; set; }
        public virtual string ErrorMessage { get; set; }


    }

    public class GetMasterialValidationResultInput : PagedAndSortedResultRequestDto
    {
        public virtual string MaterialCode { get; set; }
        public virtual string MaterialGroup { get; set; }
        public virtual string RuleCode { get; set; }
        public virtual string RuleItem { get; set; }
        public virtual string Resultfield { get; set; }
    }

    public class GetMstCmmMaterialMasterHistoryInput : PagedAndSortedResultRequestDto
    {
        public virtual long Id { get; set; }
        public virtual string TableName { get; set; }
    }

    public class GetMstCmmMaterialMasterHistoryExcelInput
    {
        public virtual long Id { get; set; }

        public virtual string TableName { get; set; }
    }
}


