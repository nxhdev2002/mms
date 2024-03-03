using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.CKD.PartRobbing.Dto;
using prod.Storage;
using System.Collections.Generic;

namespace prod.Inventory.CKD.PartRobbing.Exporting
{
    public class InvCkdPartRobbingExcelExporter : NpoiExcelExporterBase, IInvCkdPartRobbingExcelExporter
    {
        public InvCkdPartRobbingExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager)
        {
        }
        public FileDto ExportToFile(List<InvCkdPartRobbingDto> invckdpartrobbing)
        {
            return CreateExcelPackage(
                "InventoryCKDPartRobbing.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("PartRobbing");
                    AddHeader(
                                sheet,
                                    ("Part No"),
                                    ("Part No Normalized"),
                                    ("Part Name"),
                                    ("Cfc"),
                                    ("SupplierNo"),
                                    ("Robbing Qty"),
                                    ("Unit Qty"),
                                    ("EffectVeh Qty"),
                                    ("Detail Model"),
                                    ("Shop"),
                                    ("Case"),
                                    ("Box")
                                   );
                    AddObjects(
                         sheet, invckdpartrobbing,
                                _ => _.PartNo,
                                _ => _.PartNoNormalized,
                                _ => _.PartName,
                                _ => _.Cfc,
                                _ => _.SupplierNo,
                                _ => _.RobbingQty,
                                _ => _.UnitQty,
                                _ => _.EffectVehQty,
                                _ => _.DetailModel,
                                _ => _.Shop,
                                _ => _.Case,
                                _ => _.Box
                                );
                });
        }

        public FileDto ExportToFileErr(List<InvCkdPartRobbingImportDto> invckdpartrobbing)
        {
            return CreateExcelPackage(
                "InventoryCKDPartRobbingErr.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("PartRobbing");
                    AddHeader(
                                sheet,
                                    ("Guid"),
                                    ("Part No"),
                                    ("Part NoNormalized"),
                                    ("PartName"),
                                    ("Cfc"),
                                    ("SupplierNo"),
                                    ("Robbing Qty"),
                                    ("Unit Qty"),
                                    ("EffectVeh Qty"),
                                    ("Shop"),
                                    ("Case"),
                                    ("Box"),
                                    ("ErrorDescription")

                                   );
                    AddObjects(
                         sheet, invckdpartrobbing,
                                _ => _.Guid,
                                _ => _.PartNo,
                                _ => _.PartNoNormalized,
                                _ => _.PartName,
                                _ => _.Cfc,
                                _ => _.SupplierNo,
                                _ => _.RobbingQty,
                                _ => _.UnitQty,
                                _ => _.EffectVehQty,
                                _ => _.Shop,
                                _ => _.Case,
                                _ => _.Box,
                                _ => _.ErrorDescription

                                );

                });
        }
    }
}
