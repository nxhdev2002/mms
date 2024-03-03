using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Inventory.Dto;
using prod.Master.Inventory.Exporting;
using prod.Storage;

namespace vovina.Master.Inventory.Exporting
{
    public class MstInvCpsInventoryItemsExcelExporter : NpoiExcelExporterBase, IMstInvCpsInventoryItemsExcelExporter
    {
        public MstInvCpsInventoryItemsExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstInvCpsInventoryItemsDto> cpsinventoryitems)
        {
            return CreateExcelPackage(
                "MasterInventoryCpsInventoryItems.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("CpsInventoryItems");
                    AddHeader(
                                sheet,
                                ("PartNo"),
                                ("PartName"),
                                ("Color"),
                                ("Puom")

                               );
                    AddObjects(
                         sheet, cpsinventoryitems,
                                _ => _.PartNo,
                                _ => _.PartName,
                                _ => _.Color,
                                _ => _.Puom

                                );

                    for (var i = 0; i < 8; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });

        }
    }
}