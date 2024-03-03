using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.CPS;
using prod.Inventory.CPS.Dto;
using prod.Inventory.CPS.InvCpsRcvShipmentHeader.Exproting;
using prod.Storage;
using System.Collections.Generic;
namespace vovina.Inventory.CPS.Exporting
{
    public class InvCpsRcvShipmentHeadersExcelExporter : NpoiExcelExporterBase, IInvCpsRcvShipmentHeadersExcelExporter
    {
        public InvCpsRcvShipmentHeadersExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<InvCpsRcvShipmentHeadersDto> invcpsrcvshipmentheaders)
        {
            return CreateExcelPackage(
                "InventoryCPSRcvShipmentHeaders.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("RcvShipmentHeaders");
                    AddHeader(
                                sheet,
                                ("ReceiptNumber"),
                                    ("RecieveDate"),
                                    ("InventoryGroup"),
                                    ("VendorName")
                                   );
                    AddObjects(
                         sheet, invcpsrcvshipmentheaders,
                                _ => _.ReceiptNum,
                                _ => _.CreationTime,
                                _ => _.Productgroupname,
                                _ => _.SupplierName
               
                                );

                   
                });

        }

        public FileDto ExportToFileLine(List<InvCpsRcvShipmentLineDto> invcpsrcvshipmentlines)
        {
            return CreateExcelPackage(
                "InventoryCPSRcvShipmentLines.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("RcvShipmentLines");
                    AddHeader(
                                sheet,
                                    ("PoNumber"),
                                    ("PartNo"),
                                    ("PartName"),
                                    ("QuantityShipped"),
                                    ("QuantityReceived"),
                                    ("UnitOfMeasure")
                                   );
                    AddObjects(
                         sheet, invcpsrcvshipmentlines,
                                _ => _.PoNumber,
                                _ => _.PartNo,
                                _ => _.ItemDescription,
                                _ => _.QuantityShipped,
                                _ => _.QuantityReceived,
                                _ => _.UnitOfMeasure
                                );

                  
                });

        }
    }
}
