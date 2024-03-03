using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.PIO.StockRundownTransaction.Dto;
using prod.Storage;
using System.Collections.Generic;

namespace prod.Inventory.PIO.StockRundownTransaction.Exporting
{

    public class InvPIOStockRundownTransactionExcelExporter : NpoiExcelExporterBase, IInvPIOStockRundownTransactionExcelExporter
    {
        public InvPIOStockRundownTransactionExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<InvPIOStockRundownTransactionDto> stockrundowntransaction)
        {
            return CreateExcelPackage(
                "InvPIOStockRundownTrans.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("StockRundownTransaction");
                    AddHeader(
                                sheet,
                                ("Part No"),
                                ("Part Name"),
                                //("Part Id"),
                                ("Mkt Code"),
                                ("Working Date"),
                                ("Qty"),
                                //("Trans Id"),
                                ("Trans Datetime"),
                                //("Vehicle Id"),
                                ("Vin No"),
                                ("Part Type"),
                                ("Car Type"),
                                ("Interior Color"),
                                ("ExtColor"),
                                ("Is Active"),
                                ("Route")
                               );
                    AddObjects(
                         sheet, stockrundowntransaction,
                                _ => _.PartNo,
                                _ => _.PartName,
                                //_ => _.PartId,
                                _ => _.MktCode,
                                _ => _.WorkingDate,
                                _ => _.Qty,
                                //_ => _.TransId,
                                _ => _.TransDatetime,
                                //_ => _.VehicleId,
                                _ => _.VinNo,
                                _ => _.PartType,
                                _ => _.CarType,
                                _ => _.InteriorColor,
                                _ => _.ExtColor,
                                _ => _.IsActive,
                                _ => _.Route
                                );
                });
        }
    }
}
