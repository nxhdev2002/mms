using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using prod.Inventory.CKD.Exporting;
using prod.Inventory.Gps.PartList.Dto;
using prod.Inventory.Gps.PartList.Exporting;
using prod.Storage;

namespace prod.Inventory.CKD.Exporting
{
    public class InvGpsPartListExcelExporter : NpoiExcelExporterBase, IInvGpsPartListExcelExporter
    {
        public InvGpsPartListExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }

        public FileDto ExportToFileErr(List<InvGpsPartListImportDto> partlistcolor_err)
        {
            return CreateExcelPackage(
                "ListErrorImportGpsPartCoLor.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("PartCoLor");
                    AddHeader(
                                sheet,
                                ("No"),
                                ("IsPartColor"),
                                ("PartNo"),
                                ("PartNoNormalized"),
                                ("PartName"),
                                ("SupplierNo"),
                                ("Uom"),
                                ("BoxQty"),
                                ("Remark1"),
                                ("ProcessUse"),
                                ("Type"),
                                ("SeasonType"),
                                ("WinterRadio"),
                                ("SummerRadio"),
                                ("DiffRatio"),
                                ("Remark"),
                                ("Grade"),
                                ("BodyColor"),
                                ("ErrorDescription")
                               );
                    AddObjects(
                         sheet, partlistcolor_err,
                                _ => _.ROW_NO,
                                _ => _.IsPartColor,
                                _ => _.PartNo,
                                _ => _.PartNoNormalized,
                                _ => _.PartName,
                                _ => _.SupplierNo,
                                _ => _.Uom,
                                _ => _.BoxQty,
                                _ => _.Remark1,
                                _ => _.ProcessUse,
                                _ => _.Type,
                                _ => _.SeasonType,
                                _ => _.WinterRatio,
                                _ => _.SummerRadio,
                                _ => _.DiffRatio,
                                _ => _.Remark,
                                _ => _.Grade,
                                _ => _.BodyColor,
                                _ => _.ErrorDescription
                                );


                });

        }
        public FileDto ExportToFileNoColorErr(List<InvGpsPartListImportDto> partlistnocolor_err)
        {
            return CreateExcelPackage(
                "ListErrorImportGpsPartNoCoLor.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("PartNoCoLor");
                    AddHeader(
                                sheet,
                                ("No"),
                                ("IsPartColor"),
                                ("PartNo"),
                                ("PartName"),
                                ("Uom"),
                                ("MinLot"),
                                ("Type"),
                                ("Category"),
                                ("StartDate"),
                                ("EndDate"),
                                ("ErrorDescription")
                               );
                    AddObjects(
                         sheet, partlistnocolor_err,
                                _ => _.ROW_NO,
                                _ => _.IsPartColor,
                                _ => _.PartNo,
                                _ => _.PartName,
                                _ => _.Uom,
                                _ => _.MinLot,
                                _ => _.Type,
                                _ => _.Category,
                                _ => _.StartDate,
                                _ => _.EndDate,
                                _ => _.ErrorDescription
                                );


                });

        }

        public FileDto ExportValidateToFile(List<ValidateGpsPartListDto> invgpspartlistvalidate)
        {
            return CreateExcelPackage(
                "InventoryGpsPartListValidate.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("PartList");
                    AddHeader(
                                sheet,
                                ("PartListId"),
                                ("ErrorDescription"),
                                ("PartNo"),
                                ("PartNoNormalized"),
                                ("PartName"),
                                ("Grade"),
                                ("Shop"),
                                ("BodyColor"),
                               // ("UsageQty"),
                                ("CreationTime"),
                                ("CreatorUserId")
                               );
                    AddObjects(
                         sheet, invgpspartlistvalidate,
                             _ => _.PartListId,
                             _ => _.ErrorDescription,
                             _ => _.PartNo,
                             _ => _.PartNoNormalized,
                             _ => _.PartName,
                             _ => _.Grade,
                             _ => _.Shop,
                             _ => _.BodyColor,
                            // _ => _.UsageQty,
                             _ => _.CreationTime,
                             _ => _.CreatorUserId
                                );


                });

        }
    }
}
