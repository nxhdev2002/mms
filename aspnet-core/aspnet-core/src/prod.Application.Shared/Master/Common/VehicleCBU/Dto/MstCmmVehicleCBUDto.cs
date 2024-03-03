using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace prod.Master.Common.VehicleCBU.Dto
{
    //Vehicle CBU
    public class MstCmmVehicleCBUDto : EntityDto<long?>
    {
        public virtual long? IdVehicle { get; set; }
        public virtual string VehicleType { get; set; }
        public virtual string Model { get; set; }
        public virtual string MarketingCode { get; set; }
        public virtual string ProductionCode { get; set; }
        public virtual string Katashiki { get; set; }

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

        public virtual string ProdSfx { get; set; }
        public virtual string CreateMaterial { get; set; }
    }

    public class GetMstCmmVehicleCBUInput : PagedAndSortedResultRequestDto
    {

        public virtual string VehicleType { get; set; }

        public virtual string Model { get; set; }

        public virtual string MarketingCode { get; set; }

        public virtual string Katashiki { get; set; }


    }

    public class GetMstCmmVehicleCBUExportInput
    {

        public virtual string VehicleType { get; set; }

        public virtual string Model { get; set; }

        public virtual string MarketingCode { get; set; }

        public virtual string Katashiki { get; set; }


    }

    //Vehicle CBU Color
    public class MstCmmVehicleCBUColorDto : EntityDto<long?>
    {

        public virtual string ExtColor { get; set; }

        public virtual string IntColor { get; set; }

    }

    public class GetMstCmmVehicleCBUColorInput : PagedAndSortedResultRequestDto
    {

        public virtual long? VehicleCBUId { get; set; }


    }

    public class GetMstCmmVehicleCBUColorExportInput
    {

        public virtual long? VehicleCBUId { get; set; }

    }

    public class UpdateCmmVehicleCBUCreateMaterial
    {

        public virtual long? Id { get; set; }

    }


    public class MstCmmVehicleCBUColorValidationResultDto
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

    public class GetVehicleCBUValidationResultInput : PagedAndSortedResultRequestDto
    {
        public virtual string MaterialCode { get; set; }
        public virtual string MaterialGroup { get; set; }
        public virtual string RuleCode { get; set; }
        public virtual string RuleItem { get; set; }
        public virtual string Resultfield { get; set; }
    }

    public class GetMstCmmVehicleCBUHistoryInput : PagedAndSortedResultRequestDto
    {
        public virtual long Id { get; set; }
        public virtual string TableName { get; set; }
    }

    public class GetMstCmmVehicleCBUHistoryExcelInput
    {
        public virtual long Id { get; set; }

        public virtual string TableName { get; set; }
    }
}
