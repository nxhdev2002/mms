using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Common
{
    [Table("MstCmmMaterialMaster")]
    public class MstCmmMaterialMaster : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxMaterialTypeLength = 4;

        public const int MaxMaterialCodeLength = 40;

        public const int MaxIndustrySectorLength = 1;

        public const int MaxDescriptionLength = 40;

        public const int MaxMaterialGroupLength = 9;

        public const int MaxBaseUnitOfMeasureLength = 3;

        public const int MaxDeletionFlagLength = 1;

        public const int MaxPlantLength = 4;

        public const int MaxStorageLocationLength = 4;

        public const int MaxProductionGroupLength = 3;

        public const int MaxProductionPurposeLength = 2;

        public const int MaxProductionTypeLength = 10;

        public const int MaxProfitCenterLength = 10;

        public const int MaxBatchManagementLength = 3;

        public const int MaxReservedStockLength = 2;

        public const int MaxLotCodeLength = 10;

        public const int MaxMrpGroupLength = 4;

        public const int MaxMrpTypeLength = 2;

        public const int MaxProcurementTypeLength = 1;

        public const int MaxSpecialProcurementLength = 2;

        public const int MaxProductionStorageLocationLength = 4;

        public const int MaxRepetManufacturingLength = 1;

        public const int MaxRemProfileLength = 4;

        public const int MaxDoNotCostLength = 1;

        public const int MaxVarianceKeyLength = 25;

        public const int MaxProductionVersionLength = 4;

        public const int MaxSpecialProcurementCtgViewLength = 2;

        public const int MaxValuationCategoryLength = 1;

        public const int MaxValuationTypeLength = 10;

        public const int MaxValuationClassLength = 4;

        public const int MaxPriceDeterminationLength = 1;

        public const int MaxPriceControlLength = 1;

        public const int MaxWithQtyStructureLength = 1;

        public const int MaxMaterialOriginLength = 1;

        public const int MaxOriginGroupLength = 4;

        public const int MaxAuthorizationGroupLength = 4;

        public const int MaxMatSrcLength = 4;

        public const int MaxIsActiveLength = 1;

        [StringLength(MaxMaterialTypeLength)]
        public virtual string MaterialType { get; set; }

        [StringLength(MaxMaterialCodeLength)]
        public virtual string MaterialCode { get; set; }

        [StringLength(MaxIndustrySectorLength)]
        public virtual string IndustrySector { get; set; }

        [StringLength(MaxDescriptionLength)]
        public virtual string Description { get; set; }

        [StringLength(MaxMaterialGroupLength)]
        public virtual string MaterialGroup { get; set; }

        [StringLength(MaxBaseUnitOfMeasureLength)]
        public virtual string BaseUnitOfMeasure { get; set; }

        [StringLength(MaxDeletionFlagLength)]
        public virtual string DeletionFlag { get; set; }

        [StringLength(MaxPlantLength)]
        public virtual string Plant { get; set; }

        [StringLength(MaxStorageLocationLength)]
        public virtual string StorageLocation { get; set; }

        [StringLength(MaxProductionGroupLength)]
        public virtual string ProductionGroup { get; set; }

        [StringLength(MaxProductionPurposeLength)]
        public virtual string ProductionPurpose { get; set; }

        [StringLength(MaxProductionTypeLength)]
        public virtual string ProductionType { get; set; }

        [StringLength(MaxProfitCenterLength)]
        public virtual string ProfitCenter { get; set; }

        [StringLength(MaxBatchManagementLength)]
        public virtual string BatchManagement { get; set; }

        [StringLength(MaxReservedStockLength)]
        public virtual string ReservedStock { get; set; }

        [StringLength(MaxLotCodeLength)]
        public virtual string LotCode { get; set; }

        [StringLength(MaxMrpGroupLength)]
        public virtual string MrpGroup { get; set; }

        [StringLength(MaxMrpTypeLength)]
        public virtual string MrpType { get; set; }

        [StringLength(MaxProcurementTypeLength)]
        public virtual string ProcurementType { get; set; }

        [StringLength(MaxSpecialProcurementLength)]
        public virtual string SpecialProcurement { get; set; }

        [StringLength(MaxProductionStorageLocationLength)]
        public virtual string ProductionStorageLocation { get; set; }

        [StringLength(MaxRepetManufacturingLength)]
        public virtual string RepetManufacturing { get; set; }

        [StringLength(MaxRemProfileLength)]
        public virtual string RemProfile { get; set; }

        [StringLength(MaxDoNotCostLength)]
        public virtual string DoNotCost { get; set; }

        [StringLength(MaxVarianceKeyLength)]
        public virtual string VarianceKey { get; set; }

        public virtual decimal? CostingLotSize { get; set; }

        [StringLength(MaxProductionVersionLength)]
        public virtual string ProductionVersion { get; set; }

        [StringLength(MaxSpecialProcurementCtgViewLength)]
        public virtual string SpecialProcurementCtgView { get; set; }

        [StringLength(MaxValuationCategoryLength)]
        public virtual string ValuationCategory { get; set; }

        [StringLength(MaxValuationTypeLength)]
        public virtual string ValuationType { get; set; }

        [StringLength(MaxValuationClassLength)]
        public virtual string ValuationClass { get; set; }

        [StringLength(MaxPriceDeterminationLength)]
        public virtual string PriceDetermination { get; set; }

        [StringLength(MaxPriceControlLength)]
        public virtual string PriceControl { get; set; }

        public virtual decimal? StandardPrice { get; set; }

        public virtual decimal? MovingPrice { get; set; }

        [StringLength(MaxWithQtyStructureLength)]
        public virtual string WithQtyStructure { get; set; }

        [StringLength(MaxMaterialOriginLength)]
        public virtual string MaterialOrigin { get; set; }

        [StringLength(MaxOriginGroupLength)]
        public virtual string OriginGroup { get; set; }

        [StringLength(MaxAuthorizationGroupLength)]
        public virtual string AuthorizationGroup { get; set; }

        [StringLength(MaxMatSrcLength)]
        public virtual string MatSrc { get; set; }

        public virtual DateTime? EffectiveDateFrom { get; set; }

        public virtual DateTime? EffectiveDateTo { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}

