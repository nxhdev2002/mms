using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Storage;
using prod.Inventory.PIO.Stock.Dto;
using Stripe;

namespace prod.Inventory.PIO.Stock.Exporting
{
    public class InvPIOStockExcelExporter : NpoiExcelExporterBase, IInvPIOStockExcelExporter
    {
        public InvPIOStockExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<InvPIOStockDto> stock)
        {
            return CreateExcelPackage(
                "InventoryPIOStock.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Stock");
                    AddHeader(
                                sheet,
                                ("Part No"),
                                ("Part Name"),
                                ("Marketing Code"),
                                ("Working Date"),
                                ("Qty"),
                                ("Scan Date"),
                                ("Vin No"),
                                ("Part Type"),
                                ("Shop"),
                                ("Car Type"),
                                ("Interior Color")
                               );
                    AddObjects(
                         sheet, stock,
                                _ => _.PartNo,
                                _ => _.PartName,
                                _ => _.MktCode,
                                _ => _.WorkingDate,
                                _ => _.Qty,
                                _ => _.ScanDate_DDMMYYYY,
                                _ => _.VinNo,
                                _ => _.PartType,
                                _ => _.Shop,
                                _ => _.CarType,
                                _ => _.InteriorColor
                                );
                });

        }
    }
}
