using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.GPS.Exporting;
using prod.Inventory.GPS.Dto;
using prod.Storage;
using prod.Inventory.GPS.Dto;
namespace prod.Inventory.GPS.Exporting
{
    public class InvGpsStockRundownExcelExporter : NpoiExcelExporterBase, IInvGpsStockRundownExcelExporter
    {
        public InvGpsStockRundownExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<InvGpsStockRundownDto> gpsstockrundown)
        {
            return CreateExcelPackage(
                "InventoryGPSGpsStockRundown.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("GpsStockRundown");
                    AddHeader(
                                sheet,
                                ("PartNo"),
                                ("PartName")
                                //("PartId"),
                                //("MaterialId"),
                                //("Qty"),
                                //("WorkingDate"),
                                //("TransactionId")

                               );
                    AddObjects(
                         sheet, gpsstockrundown,
                                _ => _.PartNo,
                                _ => _.PartName
                              //  _ => _.PartId,
                               // _ => _.MaterialId,
                              //  _ => _.Qty,
                              //  _ => _.WorkingDate,
                              //  _ => _.TransactionId

                                );

                    for (var i = 0; i < 8; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });

        }
    }
}
