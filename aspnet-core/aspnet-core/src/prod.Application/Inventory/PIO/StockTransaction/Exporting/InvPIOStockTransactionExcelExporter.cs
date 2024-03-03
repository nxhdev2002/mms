using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.PIO.StockTransaction.Dto;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.PIO.StockTransaction.Exporting
{

    public class InvPIOStockTransactionExcelExporter : NpoiExcelExporterBase, IInvPIOStockTransactionExcelExporter
    {
        public InvPIOStockTransactionExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<InvPIOStockTransactionDto> stocktransaction)
        {
            return CreateExcelPackage(
                "InventoryPIOStockTransaction.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("StockTransaction");
                    AddHeader(
                                sheet,
                                ("Part No"),
                                ("Part Name"),
                                ("Part Id"),
                                ("Mkt Code"),
                                ("Working Date"),
                                ("Qty"),
                                ("Trans Id"),
                                ("Scan Date"),
                                ("Vehicle Id"),
                                ("Vin No"),
                                ("Part Type"),
                                ("Shop"),
                                ("Car Type"),
                                ("Interior Color"),
                                ("Is Active")
                               );
                    AddObjects(
                         sheet, stocktransaction,
                                _ => _.PartNo,
                                _ => _.PartName,
                                _ => _.PartId,
                                _ => _.MktCode,
                                _ => _.WorkingDate,
                                _ => _.Qty,
                                _ => _.TransId,
                                _ => _.TransDatetime,
                                _ => _.VehicleId,
                                _ => _.VinNo,
                                _ => _.PartType,
                                _ => _.Shop,
                                _ => _.CarType,
                                _ => _.InteriorColor,
                                _ => _.IsActive
                                );
                });
        }
    }
}
