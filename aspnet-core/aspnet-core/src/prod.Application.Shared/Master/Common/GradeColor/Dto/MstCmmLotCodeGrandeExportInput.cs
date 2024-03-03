using System;
using System.Collections.Generic;
using System.Text;

namespace prod.Master.Common.GradeColor.Dto
{
    public class MstCmmLotCodeGrandeExportInput
    {
        public virtual string Model { get; set; }

        public virtual string LotCode { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string Grade { get; set; }

        public virtual string GradeName { get; set; }

        public virtual string ModelCode { get; set; }

        public virtual string ModelVin { get; set; }

        public virtual string IdLine { get; set; }

        public virtual string Spec200 { get; set; }

        public virtual string SsNo { get; set; }

        public virtual string KatashikiCtl { get; set; }

        public virtual string Katashiki { get; set; }

        public virtual string VehNameCd { get; set; }

        public virtual string MarLotCode { get; set; }

        public virtual string TestNo { get; set; }

        public virtual string VehicleId { get; set; }

        public virtual string ProdSfx { get; set; }

        public virtual string SalesSfx { get; set; }

        public virtual string Brand { get; set; }

        public virtual string CarSeries { get; set; }

        public virtual string TransmissionType { get; set; }

        public virtual string EngineType { get; set; }

        public virtual string FuelType { get; set; }

        public virtual string GoshiCar { get; set; }

        public virtual DateTime? CreationTime { get; set; }
        public virtual long CreatorUserId { get; set; }
        public virtual DateTime? LastModificationTime { get; set; }
        public virtual long? LastModifierUserId { get; set; }
        public virtual Boolean IsDeleted { get; set; }
        public virtual string DeleterUserId { get; set; }
        public virtual DateTime? DeletionTime { get; set; }
        public virtual string MaterialType { get; set; }
        public virtual string IndustrySector { get; set; }
        public virtual string Description { get; set; }
        public virtual string MaterialGroup { get; set; }
        public virtual string BaseUnitOfMeasure { get; set; }
        public virtual string DeletionFlag { get; set; }
        public virtual string Plant { get; set; }
        public virtual string StorageLocation { get; set; }
        public virtual string ProductionPurpose { get; set; }
        public virtual string ProductionType { get; set; }
        public virtual string ProfitCenter { get; set; }
        public virtual string BatchManagement { get; set; }
        public virtual string ReservedStock { get; set; }
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
        public virtual string MaterialCode { get; set; }
        public virtual string ProductionGroup { get; set; }
        public virtual string ValuationType { get; set; }
        public virtual string IsActive { get; set; }
    }
}

