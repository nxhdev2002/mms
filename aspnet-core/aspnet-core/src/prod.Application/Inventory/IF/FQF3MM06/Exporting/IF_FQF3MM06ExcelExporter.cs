using Microsoft.DotNet.PlatformAbstractions;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.DRM.StockPart.Exporting;
using prod.Inventory.IF.FQF3MM05.Exporting;
using prod.Inventory.IF.FQF3MM06.Dto;
using prod.Migrations;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.IF.FQF3MM06.Exporting
{
    public class IF_FQF3MM06ExcelExporter : NpoiExcelExporterBase, IIF_FQF3MM06ExcelExporter
    {
        public IF_FQF3MM06ExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<IF_FQF3MM06Dto> FQF3MM06)
        {
            return CreateExcelPackage(
                "IFIFFQF3MM06.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("FQF3MM06");
                    AddHeader(
                            sheet,
                                ("RecordId (M)"),
                                ("AuthorizationGroup (M)"),
                                ("MaterialType (O)"),
                                ("MaterialCode (M)"),
                                ("IndustrySector (M)"),
                                ("MaterialDescription (M)"),
                                ("MaterialGroup (O)"),
                                ("BaseUnitOfMeasure (M)"),
                                ("FlagDeletionPlantLevel (O)"),
                                ("Plant (M)"),
                                ("StorageLocation (M)"),
                                ("ProductGroup (O)"),
                                ("ProductPurpose (O)"),
                                ("ProductType (O)"),
                                ("ProfitCenter (O)"),
                                ("BatchManagement (O)"),
                                ("ReservedStock (O)"),
                                ("Residue (O)"),
                                ("LotCode (O)"),
                                ("MrpGroup (O)"),
                                ("MrpType (O)"),
                                ("ProcurementType (M)"),
                                ("SpecialProcurement (O)"),
                                ("ProdStorLocation (O)"),
                                ("RepetManufacturing (M)"),
                                ("RemProfile (M)"),
                                ("DoNotCost (O)"),
                                ("VarianceKey (O)"),
                                ("CostingLotSize (O)"),
                                ("ProductionVersion (O)"),
                                ("SpecialProcurementType (O)"),
                                ("ValuationCategory (O)"),
                                ("ValuationType (O)"),
                                ("ValuationClass (O)"),
                                ("PriceDetermination (O)"),
                                ("PriceControl (O)"),
                                ("StandardPrice (O)"),
                                ("MovingPrice (O)"),
                                ("WithQtyStructure (O)"),
                                ("MaterialOrigin (O)"),
                                ("OriginGroup (O)"),
                                ("BasicDataText (R)"),
                                ("Katashiki (O)"),
                                ("VehicleControlKatashiki (O)"),
                                ("ToyotaOrNonToyota (O)"),
                                ("CategoryOfGear (O)"),
                                ("GoshiCar (O)"),
                                ("SeriesOfVehicles (O)"),
                                ("DeliverPowerOfDrivingWheels (O)"),
                                ("FuelType (O)"),
                                ("VehicleName (O)"),
                                ("PriceUnit (M)"),
                                ("MaruCode (O)"),
                                ("EndingOfRecord (M)")

                               );
                    AddObjects(
                         sheet, FQF3MM06,
                                _ => _.RecordId,
                                _ => _.AuthorizationGroup,
                                _ => _.MaterialType,
                                _ => _.MaterialCode,
                                _ => _.IndustrySector,
                                _ => _.MaterialDescription,
                                _ => _.MaterialGroup,
                                _ => _.BaseUnitOfMeasure,
                                _ => _.FlagDeletionPlantLevel,
                                _ => _.Plant,
                                _ => _.StorageLocation,
                                _ => _.ProductGroup,
                                _ => _.ProductPurpose,
                                _ => _.ProductType,
                                _ => _.ProfitCenter,
                                _ => _.BatchManagement,
                                _ => _.ReservedStock,
                                _ => _.Residue,
                                _ => _.LotCode,
                                _ => _.MrpGroup,
                                _ => _.MrpType,
                                _ => _.ProcurementType,
                                _ => _.SpecialProcurement,
                                _ => _.ProdStorLocation,
                                _ => _.RepetManufacturing,
                                _ => _.RemProfile,
                                _ => _.DoNotCost,
                                _ => _.VarianceKey,
                                _ => _.CostingLotSize,
                                _ => _.ProductionVersion,
                                _ => _.SpecialProcurementType,
                                _ => _.ValuationCategory,
                                _ => _.ValuationType,
                                _ => _.ValuationClass,
                                _ => _.PriceDetermination,
                                _ => _.PriceControl,
                                _ => _.StandardPrice,
                                _ => _.MovingPrice,
                                _ => _.WithQtyStructure,
                                _ => _.MaterialOrigin,
                                _ => _.OriginGroup,
                                _ => _.BasicDataText,
                                _ => _.Katashiki,
                                _ => _.VehicleControlKatashiki,
                                _ => _.ToyotaOrNonToyota,
                                _ => _.CategoryOfGear,
                                _ => _.GoshiCar,
                                _ => _.SeriesOfVehicles,
                                _ => _.DeliverPowerOfDrivingWheels,
                                _ => _.FuelType,
                                _ => _.VehicleName,
                                _ => _.PriceUnit,
                                _ => _.MaruCode,
                                _ => _.EndingOfRecord

                                );
                }
            );
        }
        public FileDto ExportValidateToFile(List<GetIF_FQF3MM06_VALIDATE> FQF3MM06)
        {
            return CreateExcelPackage(
                "IFIFFQF3MM06_VALIDATE.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("FQF3MM06_VALIDATE");
                    AddHeader(
                            sheet,
                                ("ErrorDescription"),
                                ("RecordId (M)"),
                                ("AuthorizationGroup (M)"),
                                ("MaterialCode (M)"),
                                ("IndustrySector (M)"),
                                ("MaterialDescription (M)"),
                                ("BaseUnitOfMeasure (M)"),
                                ("Plant (M)"),
                                ("StorageLocation (M)"),
                                ("ProcurementType (M)"),
                                ("RepetManufacturing (M)"),
                                ("RemProfile (M)"),
                                ("BasicDataText (R)"),
                                ("PriceUnit (M)"),
                                ("EndingOfRecord (M)")

                               );
                    AddObjects(
                         sheet, FQF3MM06,
                                _ => _.ErrorDescription,
                                _ => _.RecordId,
                                _ => _.AuthorizationGroup,
                                _ => _.MaterialCode,
                                _ => _.IndustrySector,
                                _ => _.MaterialDescription,
                                _ => _.BaseUnitOfMeasure,
                                _ => _.Plant,
                                _ => _.StorageLocation,
                                _ => _.ProcurementType,
                                _ => _.RepetManufacturing,
                                _ => _.RemProfile,
                                _ => _.BasicDataText,
                                _ => _.PriceUnit,
                                _ => _.EndingOfRecord
                                );
                }
            );
        }
    }
}
