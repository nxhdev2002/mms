using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.Gps.StockIssuingTransDetails.Dto;
using prod.Inventory.GPS;
using prod.Inventory.GPS.Exporting;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.Gps.StockIssuingTransDetails.Exporting
{
    public class InvGpsStockIssuingTransactionExcelExporter : NpoiExcelExporterBase, IInvGpsStockIssuingTransactionExcelExporter
    {
        public InvGpsStockIssuingTransactionExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }

        public FileDto ExportToFile(List<InvGpsStockIssuingTransactionDto> stockissuingtrans)
        {
            return CreateExcelPackage(
                "InventoryGpsStockIssuingTrans.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("GpsStockIssuingTransaction");
                    AddHeader(
                                sheet,
                                ("Part No"),
                                ("Puom"),
                                ("Qty"),
                                ("Cost Center"),
                                ("Working Date")
                               );
                    AddObjects(
                         sheet, stockissuingtrans,
                                
                                _ => _.PartNo,
                                _ => _.Puom,
                                _ => _.Qty,
                                _ => _.CostCenter,
                                _ => _.WorkingDate
                                );
                });



        }


        public FileDto ExportErrToFile(List<InvGpsStockIssuingTransactionDto> stockissuingtransErr)
        {
            return CreateExcelPackage(
                "InventoryGpsStockIssuingTrans.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("GpsStockIssuingTransaction");
                    AddHeader(
                                sheet,
                                ("Part No"),
                                ("Puom"),                     
                                ("Qty"),
                                ("Cost Center"),
                                ("Working Date"),
                                ("Error Description")
                               );
                    AddObjects(
                         sheet, stockissuingtransErr,
                                _ => _.PartNo,
                                 _ => _.Puom,
                                _ => _.Qty,
                                _ => _.CostCenter,
                                _ => _.WorkingDate,
                                _ => _.ErrorDescription
                                );
                });
        }
        }
}
