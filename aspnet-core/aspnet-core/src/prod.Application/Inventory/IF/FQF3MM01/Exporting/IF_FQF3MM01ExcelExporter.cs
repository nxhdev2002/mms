using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.CKD.Exporting;
using prod.Inventory.CKD.Dto;
using prod.Storage;
using prod.Inventory.CKD.Dto;
using prod.Inventory.IF.FQF3MM01.Dto;
using prod.Inventory.IF.FQF3MM01.Exporting;

namespace prod.Inventory.CKD.Exporting
{
    public class IF_FQF3MM01ExcelExporter : NpoiExcelExporterBase, IIF_FQF3MM01ExcelExporter
    {
        public IF_FQF3MM01ExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<IF_FQF3MM01Dto> fqf3mm01)
        {
            return CreateExcelPackage(
                "IF_FQF3MM01.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("IF_FQF3MM01");
                    AddHeader(
                                sheet,
                                ("RecordId(M)"),
                                ("Vin(M)"),
                                ("Urn(O)"),
                                ("SpecSheetNo(M)"),
                                ("IdLine(M)"),
                                ("Katashiki(M)"),
                                ("SaleKatashiki(M)"),
                                ("SaleSuffix(M)"),
                                ("Spec200Digits(M)"),
                                ("ProductionSuffix(M)"),
                                ("LotCode(O)"),
                                ("EnginePrefix(M)"),
                                ("EngineNo(M)"),
                                ("PlantCode(M)"),
                                ("CurrentStatus(M)"),
                                ("LineOffDatetime(M)"),
                                ("InteriorColor(M)"),
                                ("ExteriorColor(M)"),
                                ("DestinationCode(M)"),
                                ("EdOdno(M)"),
                                ("CancelFlag(M)"),
                                ("SmsCarFamilyCode(M)"),
                                ("OrderType(M)"),
                                ("KatashikiCode(O)"),
                                ("EndOfRecord(M)")
                               );
                    AddObjects(
                         sheet, fqf3mm01,
                                _ => _.RecordId,
                                _ => _.Vin,
                                _ => _.Urn,
                                _ => _.SpecSheetNo,
                                _ => _.IdLine,
                                _ => _.Katashiki,
                                _ => _.SaleKatashiki,
                                _ => _.SaleSuffix,
                                _ => _.Spec200Digits,
                                _ => _.ProductionSuffix,
                                _ => _.LotCode,
                                _ => _.EnginePrefix,
                                _ => _.EngineNo,
                                _ => _.PlantCode,
                                _ => _.CurrentStatus,
                                _ => _.LineOffDatetime,
                                _ => _.InteriorColor,
                                _ => _.ExteriorColor,
                                _ => _.DestinationCode,
                                _ => _.EdOdno,
                                _ => _.CancelFlag,
                                _ => _.SmsCarFamilyCode,
                                _ => _.OrderType,
                                _ => _.KatashikiCode,
                                _ => _.EndOfRecord
                                );


                });

        }



        public FileDto ExportValidateToFile(List<GetIF_FQF3MM01_VALIDATE> fqf3mm01)
        {
            return CreateExcelPackage(
                "VALIDATE_IF_FQF3MM01.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("VALIDATE_FQF3MM01");
                    AddHeader(
                                sheet,
                                ("ErrorDescription"),
                                ("RecordId(M)"),
                                ("Vin(M)"),
                                ("SpecSheetNo(M)"),
                                ("IdLine(M)"),
                                ("Katashiki(M)"),
                                ("SaleKatashiki(M)"),
                                ("SaleSuffix(M)"),
                                ("Spec200Digits(M)"),
                                ("ProductionSuffix(M)"),
                                ("EnginePrefix(M)"),
                                ("EngineNo(M)"),
                                ("PlantCode(M)"),
                                ("CurrentStatus(M)"),
                                ("LineOffDatetime(M)"),
                                ("InteriorColor(M)"),
                                ("ExteriorColor(M)"),
                                ("DestinationCode(M)"),
                                ("EdOdno(M)"),
                                ("CancelFlag(M)"),
                                ("SmsCarFamilyCode(M)"),
                                ("OrderType(M)"),
                                ("EndOfRecord(M)"),
                                 ("HeaderFwgId"),
                                ("HeaderId"),
                                ("TrailerId")
                    
                               );
                    AddObjects(
                         sheet, fqf3mm01,
                                _ => _.ErrorDescription,
                                _ => _.RecordId,
                                _ => _.Vin,
                                _ => _.SpecSheetNo,
                                _ => _.IdLine,
                                _ => _.Katashiki,
                                _ => _.SaleKatashiki,
                                _ => _.SaleSuffix,
                                _ => _.Spec200Digits,
                                _ => _.ProductionSuffix,
                                _ => _.EnginePrefix,
                                _ => _.EngineNo,
                                _ => _.PlantCode,
                                _ => _.CurrentStatus,
                                _ => _.LineOffDatetime,
                                _ => _.InteriorColor,
                                _ => _.ExteriorColor,
                                _ => _.DestinationCode,
                                _ => _.EdOdno,
                                _ => _.CancelFlag,
                                _ => _.SmsCarFamilyCode,
                                _ => _.OrderType,
                                _ => _.EndOfRecord,
                                _ => _.HeaderFwgId,
                                _ => _.HeaderId,
                                _ => _.TrailerId
                              
                                );

                });
        }
    }
}
