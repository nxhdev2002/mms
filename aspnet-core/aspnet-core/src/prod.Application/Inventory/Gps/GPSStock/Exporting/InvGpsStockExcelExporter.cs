using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.GPS.Exporting;
using prod.Inventory.GPS.Dto;
using prod.Storage;
using prod.Inventory.GPS.Dto;
namespace prod.Inventory.GPS.Exporting
{
    public class InvGpsStockExcelExporter : NpoiExcelExporterBase, IInvGpsStockExcelExporter
    {
        public InvGpsStockExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<InvGpsStockDto> gpsstock)
        {
            return CreateExcelPackage(
                "InventoryGPSGpsStock.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("GpsStock");
                    AddHeader(
                                sheet,
                                ("PartNo"),
                                ("PartName"),
                                ("WorkingDate"),
                                ("Qty"),
                                ("Cfc"),
                                ("SupplierNo"),
                                ("Uom"),
                                ("LotNo"),
                                ("NoInLot"),
                                ("VinNo"),
                                ("BodyNo"),
                                ("Color"),
                                ("TransactionId")

                               );
                    AddObjects(
                         sheet, gpsstock,
                                _ => _.PartNo,
                                _ => _.PartName,
                                _ => _.WorkingDate,
                                _ => _.Qty,
                                _ => _.Cfc,
                                _ => _.SupplierNo,
                                _ => _.Uom,
                                _ => _.LotNo,
                                _ => _.NoInLot,
                                _ => _.VinNo,
                                _ => _.BodyNo,
                                _ => _.Color,
                                _ => _.TransactionId
                                );

                    for (var i = 0; i < 8; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });

        }
    }
}
