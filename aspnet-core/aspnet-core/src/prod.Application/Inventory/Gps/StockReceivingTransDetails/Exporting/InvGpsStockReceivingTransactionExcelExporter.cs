using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.Gps.StockReceivingTransDetails.Dto;
using prod.Storage;
using System.Collections.Generic;

namespace prod.Inventory.Gps.StockReceivingTransDetails.Exporting
{
    public class InvGpsStockReceivingTransactionExcelExporter : NpoiExcelExporterBase, IInvGpsStockReceivingTransactionExcelExporter
    {
        public InvGpsStockReceivingTransactionExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }

        public FileDto ExportToFile(List<InvGpsStockReceivingTransactionDto> stockReceivingtrans)
        {
            return CreateExcelPackage(
                "InventoryGpsStockReceivingTrans.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("GpsStockReceivingTransaction");
                    AddHeader(
                                sheet,
                                ("Po No"),
                                ("Part No"),
                                ("Part Name"),
                                ("Puom"),
                                ("Qty"),
                                ("Working Date")
                               );
                    AddObjects(
                         sheet, stockReceivingtrans,
                                _ => _.PoNo,
                                _ => _.PartNo,
                                _ => _.PartName,
                                _ => _.Puom,
                                _ => _.Qty,
                                _ => _.WorkingDate
                                );
                });
        }

        public FileDto ExportErrToFile(List<InvGpsStockReceivingTransactionDto> stockReceivingtransErr)
        {
            return CreateExcelPackage(
                "InventoryGpsStockReceivingTrans.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("GpsStockReceivingTransaction");
                    AddHeader(
                                sheet,
                                ("Po No"),
                                ("Part No"),
                                ("Part Name"),
                                ("Puom"),
                                ("Qty"),
                                ("Working Date"),
                                ("Error Description")
                               );
                    AddObjects(
                         sheet, stockReceivingtransErr,
                                _ => _.PoNo,
                                _ => _.PartNo,
                                _ => _.PartName,
                                _ => _.Puom,
                                _ => _.Qty,
                                _ => _.WorkingDate,
                                 _ => _.ErrorDescription
                                );
                });
        }
    }
}
