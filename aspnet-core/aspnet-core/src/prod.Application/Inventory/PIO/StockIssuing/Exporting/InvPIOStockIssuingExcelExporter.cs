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

    public class InvPIOStockIssuingExcelExporter : NpoiExcelExporterBase, IInvPIOStockIssuingExcelExporter
    {
        public InvPIOStockIssuingExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<InvPIOStockTransactionDto> stocktransaction)
        {
            return CreateExcelPackage(
                "InventoryPIOStockIssuing.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("StockIssuing");
                    AddHeader(
                                sheet,
                                ("Part No"),
                                ("Part Name"),
                                ("Mkt Code"),
                                ("Working Date"),
                                ("Qty"),
                                ("Trans Datetime"),
                                ("Vin No"),
                                ("Part Type"),
                                ("Shop"),
                                ("Car Type"),
                                ("Interior Color"),
                                ("Ext Color"),
                                ("Route")
                               );
                    AddObjects(
                         sheet, stocktransaction,
                                _ => _.PartNo,
                                _ => _.PartName,
                                _ => _.MktCode,
                                _ => _.WorkingDate,
                                _ => _.Qty,
                                _ => _.TransDatetime,
                                _ => _.VinNo,
                                _ => _.PartType,
                                _ => _.Shop,
                                _ => _.CarType,
                                _ => _.InteriorColor,
                                _ => _.ExtColor,
                                _ => _.Route
                                );
                });
        }
    }
}
