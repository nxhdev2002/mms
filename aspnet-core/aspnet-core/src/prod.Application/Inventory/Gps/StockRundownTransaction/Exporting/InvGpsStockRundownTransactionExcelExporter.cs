using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.GPS.Exporting;
using prod.Inventory.GPS.Dto;
using prod.Storage;
using prod.Inventory.GPS.Dto;
namespace prod.Inventory.GPS.Exporting
{
    public class InvGpsStockRundownTransactionExcelExporter : NpoiExcelExporterBase, IInvGpsStockRundownTransactionExcelExporter
    {
        public InvGpsStockRundownTransactionExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<InvGpsStockRundownTransactionDto> gpsstockrundowntransaction)
        {
            return CreateExcelPackage(
                "InventoryGPSGpsStockRundownTransaction.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("GpsStockRundownTransaction");
                    AddHeader(
                                sheet,
                                ("PartNo"),
                                ("PartName"),
                                ("PartId"),
                                ("MaterialId"),
                                ("Qty"),
                                ("WorkingDate"),
                                ("TransactionDate"),
                                ("TransactionId")
                               );
                    AddObjects(
                         sheet, gpsstockrundowntransaction,
                                _ => _.PartNo,
                                _ => _.PartName,
                                _ => _.PartId,
                                _ => _.MaterialId,
                                _ => _.Qty,
                                _ => _.WorkingDate,
                                _ => _.TransactionDate,
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
