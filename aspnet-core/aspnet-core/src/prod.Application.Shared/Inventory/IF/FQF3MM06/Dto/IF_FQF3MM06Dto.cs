using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace prod.Inventory.IF.FQF3MM06.Dto
{
    public class IF_FQF3MM06Dto : EntityDto<long?>
    {
        public virtual int? RecordId { get; set; }

        public virtual string AuthorizationGroup { get; set; }

        public virtual string MaterialType { get; set; }

        public virtual string MaterialCode { get; set; }

        public virtual string IndustrySector { get; set; }

        public virtual string MaterialDescription { get; set; }

        public virtual string MaterialGroup { get; set; }

        public virtual string BaseUnitOfMeasure { get; set; }

        public virtual string FlagDeletionPlantLevel { get; set; }

        public virtual string Plant { get; set; }

        public virtual string StorageLocation { get; set; }

        public virtual string ProductGroup { get; set; }

        public virtual string ProductPurpose { get; set; }

        public virtual string ProductType { get; set; }

        public virtual string ProfitCenter { get; set; }

        public virtual string BatchManagement { get; set; }

        public virtual string ReservedStock { get; set; }

        public virtual string Residue { get; set; }

        public virtual string LotCode { get; set; }

        public virtual string MrpGroup { get; set; }

        public virtual string MrpType { get; set; }

        public virtual string ProcurementType { get; set; }

        public virtual string SpecialProcurement { get; set; }

        public virtual string ProdStorLocation { get; set; }

        public virtual string RepetManufacturing { get; set; }

        public virtual string RemProfile { get; set; }

        public virtual string DoNotCost { get; set; }

        public virtual string VarianceKey { get; set; }

        public virtual string CostingLotSize { get; set; }

        public virtual string ProductionVersion { get; set; }

        public virtual string SpecialProcurementType { get; set; }

        public virtual string ValuationCategory { get; set; }

        public virtual string ValuationType { get; set; }

        public virtual string ValuationClass { get; set; }

        public virtual string PriceDetermination { get; set; }

        public virtual string PriceControl { get; set; }

        public virtual string StandardPrice { get; set; }

        public virtual string MovingPrice { get; set; }

        public virtual string WithQtyStructure { get; set; }

        public virtual string MaterialOrigin { get; set; }

        public virtual string OriginGroup { get; set; }

        public virtual string BasicDataText { get; set; }

        public virtual string Katashiki { get; set; }

        public virtual string VehicleControlKatashiki { get; set; }

        public virtual string ToyotaOrNonToyota { get; set; }

        public virtual string CategoryOfGear { get; set; }

        public virtual string GoshiCar { get; set; }

        public virtual string SeriesOfVehicles { get; set; }

        public virtual string DeliverPowerOfDrivingWheels { get; set; }

        public virtual string FuelType { get; set; }

        public virtual string VehicleName { get; set; }

        public virtual string PriceUnit { get; set; }

        public virtual string MaruCode { get; set; }

        public virtual string EndingOfRecord { get; set; }

    }

    public class GetIF_FQF3MM06Input : PagedAndSortedResultRequestDto
    {
        public virtual string MaterialCode { get; set; }

        public virtual string MaterialDescription { get; set; }

        public virtual DateTime? CreateDateFrom { get; set; }

        public virtual DateTime? CreateDateTo { get; set; }

    }
    public class GetIF_FQF3MM06ExportInput
    {
        public virtual string MaterialCode { get; set; }

        public virtual string MaterialDescription { get; set; }

        public virtual DateTime? CreateDateFrom { get; set; }

        public virtual DateTime? CreateDateTo { get; set; }

    }
    public class GetIF_FQF3MM06_VALIDATE : EntityDto<long?>
    {
        public virtual int? RecordId { get; set; }

        public virtual string AuthorizationGroup { get; set; }

        public virtual string MaterialType { get; set; }

        public virtual string MaterialCode { get; set; }

        public virtual string IndustrySector { get; set; }

        public virtual string MaterialDescription { get; set; }

        public virtual string MaterialGroup { get; set; }

        public virtual string BaseUnitOfMeasure { get; set; }

        public virtual string FlagDeletionPlantLevel { get; set; }

        public virtual string Plant { get; set; }

        public virtual string StorageLocation { get; set; }

        public virtual string ProductGroup { get; set; }

        public virtual string ProductPurpose { get; set; }

        public virtual string ProductType { get; set; }

        public virtual string ProfitCenter { get; set; }

        public virtual string BatchManagement { get; set; }

        public virtual string ReservedStock { get; set; }

        public virtual string Residue { get; set; }

        public virtual string LotCode { get; set; }

        public virtual string MrpGroup { get; set; }

        public virtual string MrpType { get; set; }

        public virtual string ProcurementType { get; set; }

        public virtual string SpecialProcurement { get; set; }

        public virtual string ProdStorLocation { get; set; }

        public virtual string RepetManufacturing { get; set; }

        public virtual string RemProfile { get; set; }

        public virtual string DoNotCost { get; set; }

        public virtual string VarianceKey { get; set; }

        public virtual string CostingLotSize { get; set; }

        public virtual string ProductionVersion { get; set; }

        public virtual string SpecialProcurementType { get; set; }

        public virtual string ValuationCategory { get; set; }

        public virtual string ValuationType { get; set; }

        public virtual string ValuationClass { get; set; }

        public virtual string PriceDetermination { get; set; }

        public virtual string PriceControl { get; set; }

        public virtual string StandardPrice { get; set; }

        public virtual string MovingPrice { get; set; }

        public virtual string WithQtyStructure { get; set; }

        public virtual string MaterialOrigin { get; set; }

        public virtual string OriginGroup { get; set; }

        public virtual string BasicDataText { get; set; }

        public virtual string Katashiki { get; set; }

        public virtual string VehicleControlKatashiki { get; set; }

        public virtual string ToyotaOrNonToyota { get; set; }

        public virtual string CategoryOfGear { get; set; }

        public virtual string GoshiCar { get; set; }

        public virtual string SeriesOfVehicles { get; set; }

        public virtual string DeliverPowerOfDrivingWheels { get; set; }

        public virtual string FuelType { get; set; }

        public virtual string VehicleName { get; set; }

        public virtual string PriceUnit { get; set; }

        public virtual string MaruCode { get; set; }

        public virtual string EndingOfRecord { get; set; }

        public virtual string ErrorDescription { get; set; }
    }

    public class GetIF_FQF3MM06_VALIDATEInput : PagedAndSortedResultRequestDto
    {
        public DateTime? CreateDateFrom { get; set; }
        public DateTime? CreateDateTo { get; set; }
    }

    public class GetIF_FQF3MM06VALIDATEExportInput
    {
        public virtual DateTime? CreateDateFrom { get; set; }

        public virtual DateTime? CreateDateTo { get; set; }

    }
}
