using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace prod.Inventory.IF
{

    [Table("IF_FQF3MM06")]
    public class IF_FQF3MM06 : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxAuthorizationGroupLength = 4;

        public const int MaxMaterialTypeLength = 4;

        public const int MaxMaterialCodeLength = 40;

        public const int MaxIndustrySectorLength = 1;

        public const int MaxMaterialDescriptionLength = 40;

        public const int MaxMaterialGroupLength = 9;

        public const int MaxBaseUnitOfMeasureLength = 3;

        public const int MaxFlagDeletionPlantLevelLength = 1;

        public const int MaxPlantLength = 4;

        public const int MaxStorageLocationLength = 4;

        public const int MaxProductGroupLength = 3;

        public const int MaxProductPurposeLength = 2;

        public const int MaxProductTypeLength = 20;

        public const int MaxProfitCenterLength = 10;

        public const int MaxBatchManagementLength = 1;

        public const int MaxReservedStockLength = 1;

        public const int MaxResidueLength = 1;

        public const int MaxLotCodeLength = 5;

        public const int MaxMrpGroupLength = 4;

        public const int MaxMrpTypeLength = 2;

        public const int MaxProcurementTypeLength = 1;

        public const int MaxSpecialProcurementLength = 2;

        public const int MaxProdStorLocationLength = 4;

        public const int MaxRepetManufacturingLength = 1;

        public const int MaxRemProfileLength = 4;

        public const int MaxDoNotCostLength = 1;

        public const int MaxVarianceKeyLength = 6;

        public const int MaxCostingLotSizeLength = 14;

        public const int MaxProductionVersionLength = 4;

        public const int MaxSpecialProcurementTypeLength = 2;

        public const int MaxValuationCategoryLength = 1;

        public const int MaxValuationTypeLength = 10;

        public const int MaxValuationClassLength = 4;

        public const int MaxPriceDeterminationLength = 1;

        public const int MaxPriceControlLength = 1;

        public const int MaxStandardPriceLength = 12;

        public const int MaxMovingPriceLength = 12;

        public const int MaxWithQtyStructureLength = 1;

        public const int MaxMaterialOriginLength = 1;

        public const int MaxOriginGroupLength = 4;

        public const int MaxBasicDataTextLength = 255;

        public const int MaxKatashikiLength = 20;

        public const int MaxVehicleControlKatashikiLength = 20;

        public const int MaxToyotaOrNonToyotaLength = 2;

        public const int MaxCategoryOfGearLength = 2;

        public const int MaxGoshiCarLength = 1;

        public const int MaxSeriesOfVehiclesLength = 18;

        public const int MaxDeliverPowerOfDrivingWheelsLength = 3;

        public const int MaxFuelTypeLength = 2;

        public const int MaxVehicleNameLength = 6;

        public const int MaxPriceUnitLength = 1;

        public const int MaxMaruCodeLength = 1;

        public const int MaxEndingOfRecordLength = 1;

        public virtual int? RecordId { get; set; }

        [StringLength(MaxAuthorizationGroupLength)]
        public virtual string AuthorizationGroup { get; set; }

        [StringLength(MaxMaterialTypeLength)]
        public virtual string MaterialType { get; set; }

        [StringLength(MaxMaterialCodeLength)]
        public virtual string MaterialCode { get; set; }

        [StringLength(MaxIndustrySectorLength)]
        public virtual string IndustrySector { get; set; }

        [StringLength(MaxMaterialDescriptionLength)]
        public virtual string MaterialDescription { get; set; }

        [StringLength(MaxMaterialGroupLength)]
        public virtual string MaterialGroup { get; set; }

        [StringLength(MaxBaseUnitOfMeasureLength)]
        public virtual string BaseUnitOfMeasure { get; set; }

        [StringLength(MaxFlagDeletionPlantLevelLength)]
        public virtual string FlagDeletionPlantLevel { get; set; }

        [StringLength(MaxPlantLength)]
        public virtual string Plant { get; set; }

        [StringLength(MaxStorageLocationLength)]
        public virtual string StorageLocation { get; set; }

        [StringLength(MaxProductGroupLength)]
        public virtual string ProductGroup { get; set; }

        [StringLength(MaxProductPurposeLength)]
        public virtual string ProductPurpose { get; set; }

        [StringLength(MaxProductTypeLength)]
        public virtual string ProductType { get; set; }

        [StringLength(MaxProfitCenterLength)]
        public virtual string ProfitCenter { get; set; }

        [StringLength(MaxBatchManagementLength)]
        public virtual string BatchManagement { get; set; }

        [StringLength(MaxReservedStockLength)]
        public virtual string ReservedStock { get; set; }

        [StringLength(MaxResidueLength)]
        public virtual string Residue { get; set; }

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

        [StringLength(MaxProdStorLocationLength)]
        public virtual string ProdStorLocation { get; set; }

        [StringLength(MaxRepetManufacturingLength)]
        public virtual string RepetManufacturing { get; set; }

        [StringLength(MaxRemProfileLength)]
        public virtual string RemProfile { get; set; }

        [StringLength(MaxDoNotCostLength)]
        public virtual string DoNotCost { get; set; }

        [StringLength(MaxVarianceKeyLength)]
        public virtual string VarianceKey { get; set; }

        [StringLength(MaxCostingLotSizeLength)]
        public virtual string CostingLotSize { get; set; }

        [StringLength(MaxProductionVersionLength)]
        public virtual string ProductionVersion { get; set; }

        [StringLength(MaxSpecialProcurementTypeLength)]
        public virtual string SpecialProcurementType { get; set; }

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

        [StringLength(MaxStandardPriceLength)]
        public virtual string StandardPrice { get; set; }

        [StringLength(MaxMovingPriceLength)]
        public virtual string MovingPrice { get; set; }

        [StringLength(MaxWithQtyStructureLength)]
        public virtual string WithQtyStructure { get; set; }

        [StringLength(MaxMaterialOriginLength)]
        public virtual string MaterialOrigin { get; set; }

        [StringLength(MaxOriginGroupLength)]
        public virtual string OriginGroup { get; set; }

        [StringLength(MaxBasicDataTextLength)]
        public virtual string BasicDataText { get; set; }

        [StringLength(MaxKatashikiLength)]
        public virtual string Katashiki { get; set; }

        [StringLength(MaxVehicleControlKatashikiLength)]
        public virtual string VehicleControlKatashiki { get; set; }

        [StringLength(MaxToyotaOrNonToyotaLength)]
        public virtual string ToyotaOrNonToyota { get; set; }

        [StringLength(MaxCategoryOfGearLength)]
        public virtual string CategoryOfGear { get; set; }

        [StringLength(MaxGoshiCarLength)]
        public virtual string GoshiCar { get; set; }

        [StringLength(MaxSeriesOfVehiclesLength)]
        public virtual string SeriesOfVehicles { get; set; }

        [StringLength(MaxDeliverPowerOfDrivingWheelsLength)]
        public virtual string DeliverPowerOfDrivingWheels { get; set; }

        [StringLength(MaxFuelTypeLength)]
        public virtual string FuelType { get; set; }

        [StringLength(MaxVehicleNameLength)]
        public virtual string VehicleName { get; set; }

        [StringLength(MaxPriceUnitLength)]
        public virtual string PriceUnit { get; set; }

        [StringLength(MaxMaruCodeLength)]
        public virtual string MaruCode { get; set; }

        [StringLength(MaxEndingOfRecordLength)]
        public virtual string EndingOfRecord { get; set; }
    }

}
