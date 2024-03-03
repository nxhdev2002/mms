using Microsoft.DotNet.PlatformAbstractions;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.DRM.StockPart.Exporting;
using prod.Inventory.IF.FQF3MM05.Dto;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.IF.FQF3MM05.Exporting
{
    public class IF_FQF3MM05ExcelExporter : NpoiExcelExporterBase, IIF_FQF3MM05ExcelExporter
    {
        public IF_FQF3MM05ExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<IF_FQF3MM05Dto> fqf3mm05)
        {
            return CreateExcelPackage(
                "IFIFFQF3MM05.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("FQF3MM05");
                    AddHeader(
                            sheet,
                            ("RunningNo (M)"),
								("DocumentDate (M)"),
								("PostingDate (M)"),
								("DocumentHeaderText (M)"),
								("MovementType (M)"),
								("MaterialCodeFrom (M)"),
								("PlantFrom (M)"),
								("ValuationTypeFrom (O)"),
								("StorageLocationFrom (M)"),
								("ProductionVersion (O)"),
								("Quantity (M)"),
								("UnitOfEntry (M)"),
								("ItemText (O)"),
								("GlAccount (O)"),
								("CostCenter (C)"),
								("Wbs (C)"),
								("MaterialCodeTo (C)"),
								("PlantTo (C)"),
								("ValuationTypeTo (C)"),
								("StorageLocationTo (C)"),
								("BfPc (O)"),
								("CancelFlag (O)"),
								("ReffMatDocNo (C)"),
								("VendorNo (C)"),
								("ProfitCenter (C)"),
								("ShipemntCat (C)"),
								("Reference (O)"),
								("AssetNo (C)"),
								("SubAssetNo (C)"),
								("EndOfRecord (M)")
				        );
                    AddObjects(
                        sheet, fqf3mm05,
                        _ => _.RunningNo,
                        _ => _.DocumentDate,
                        _ => _.PostingDate,
                        _ => _.DocumentHeaderText,
                        _ => _.MovementType,
                        _ => _.MaterialCodeFrom,
                        _ => _.PlantFrom,
                        _ => _.ValuationTypeFrom,
                        _ => _.StorageLocationFrom,
                        _ => _.ProductionVersion,
                        _ => _.Quantity,
                        _ => _.UnitOfEntry,
                        _ => _.ItemText,
                        _ => _.GlAccount,
                        _ => _.CostCenter,
                        _ => _.Wbs,
                        _ => _.MaterialCodeTo,
                        _ => _.PlantTo,
                        _ => _.ValuationTypeTo,
                        _ => _.StorageLocationTo,
                        _ => _.BfPc,
                        _ => _.CancelFlag,
                        _ => _.ReffMatDocNo,
                        _ => _.VendorNo,
                        _ => _.ProfitCenter,
                        _ => _.ShipemntCat,
                        _ => _.Reference,
                        _ => _.AssetNo,
                        _ => _.SubAssetNo,
                        _ => _.EndOfRecord
                    );
                }
            );
        }


        public FileDto ExportValidateToFile(List<GetIF_FQF3MM05_VALIDATE> fqf3mm05)
        {
            return CreateExcelPackage(
                "IFIFFQF3MM05_VALIDATE.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("FQF3MM05_VALIDATE");
                    AddHeader(
                            sheet,
                                ("ErrorDescription"),
                                ("RunningNo (M)"),
                                ("DocumentDate (M)"),
                                ("PostingDate (M)"),
                                ("DocumentHeaderText (M)"),
                                ("MovementType (M)"),
                                ("MaterialCodeFrom (M)"),
                                ("PlantFrom (M)"),
                                ("StorageLocationFrom (M)"),
                                ("Quantity (M)"),
                                ("UnitOfEntry (M)"),
                                ("CostCenter (C)"),
                                ("Wbs (C)"),
                                ("MaterialCodeTo (C)"),
                                ("PlantTo (C)"),
                                ("ValuationTypeTo (C)"),
                                ("StorageLocationTo (C)"),
                                ("ReffMatDocNo (C)"),
                                ("VendorNo (C)"),
                                ("ProfitCenter (C)"),
                                ("ShipemntCat (C)"),
                                ("AssetNo (C)"),
                                ("SubAssetNo (C)"),
                                ("EndOfRecord (M)")
                        );
                    AddObjects(
                        sheet, fqf3mm05,
                        _ => _.ErrorDescription,
                        _ => _.RunningNo,
                        _ => _.DocumentDate,
                        _ => _.PostingDate,
                        _ => _.DocumentHeaderText,
                        _ => _.MovementType,
                        _ => _.MaterialCodeFrom,
                        _ => _.PlantFrom,
                        _ => _.StorageLocationFrom,
                        _ => _.Quantity,
                        _ => _.UnitOfEntry,
                        _ => _.CostCenter,
                        _ => _.Wbs,
                        _ => _.MaterialCodeTo,
                        _ => _.PlantTo,
                        _ => _.ValuationTypeTo,
                        _ => _.StorageLocationTo,
                        _ => _.ReffMatDocNo,
                        _ => _.VendorNo,
                        _ => _.ProfitCenter,
                        _ => _.ShipemntCat,
                        _ => _.AssetNo,
                        _ => _.SubAssetNo,
                        _ => _.EndOfRecord
                    );
                }
            );
        }
    }
}
