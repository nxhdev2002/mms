using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.GPS.Dto;
using prod.Inventory.GPS.Exporting;
using prod.Storage;

namespace vovina.Inventory.GPS.Exporting
{
    public class InvGpsDailyOrderExcelExporter : NpoiExcelExporterBase, IInvGpsDailyOrderExcelExporter
    {
        public InvGpsDailyOrderExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<InvGpsDailyOrderDto> invgpsdailyorder)
        {
            return CreateExcelPackage(
                "InventoryGPSInvGpsDailyOrder.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("InvGpsDailyOrder");
                    AddHeader(
                                sheet,
                                ("WorkingDate"),
                                ("Shift"),
                                ("SupplierName"),
                                ("SupplierCode"),
                                ("OrderNo"),
                                ("OrderDatetime"),
                                ("TripNo"),
                                ("TruckNo"),
                                ("EstArrivalDatetime"),                             
                                ("TruckUnloadingId"),
                                ("IsActive")
                               );
                    AddObjects(
                         sheet, invgpsdailyorder,
                                _ => _.WorkingDate,
                                _ => _.Shift,
                                _ => _.SupplierName,
                                _ => _.SupplierCode,
                                _ => _.OrderNo,
                                _ => _.OrderDatetime,
                                _ => _.TripNo,
                                _ => _.TruckNo,
                                _ => _.EstArrivalDatetime,                              
                                _ => _.TruckUnloadingId,
                                _ => _.IsActive
                                );

                   
                });

        }
    }
}
