using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.GPS.Dto;
using prod.Inventory.GPS.Exporting;
using prod.Storage;
using System.Collections.Generic;
namespace vovina.Inventory.GPS.Exporting
{
    public class InvGpsIssuingExcelExporter : NpoiExcelExporterBase, IInvGpsIssuingExcelExporter
    {
        public InvGpsIssuingExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<InvGpsIssuingDto> stockissuing)
        {
            return CreateExcelPackage(
                "InventoryGPSStockIssuing.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("StockIssuing");
                    AddHeader(
                                sheet,
                                ("PartNo"),
                                    ("PartName"),
                                    ("Uom"),
                                   /* ("Boxqty"),
                                    ("Box"),
                                    ("Qty"),*/
                                    ("LotNo"),
                                   // ("ProdDate"),
                                    ("ExpDate"),
                                    //("ReceivedDate"),
                                   // ("Supplier"),
                                    ("IssueDate"),
                                    ("CostCenter"),
                                    ("QtyIssue"),
                                    ("IsIssue"),
                                    ("Status"),
                                    ("IsGentani")
                                   );
                    AddObjects(
                         sheet, stockissuing,
                                _ => _.PartNo,
                                _ => _.PartName,
                                _ => _.Uom,
                               /* _ => _.Boxqty,
                                _ => _.Box,
                                _ => _.Qty,*/
                                _ => _.LotNo,
                               // _ => _.ProdDate,
                                _ => _.ExpDate,
                                _ => _.IssueDate,
                               // _ => _.ReceivedDate,
                               // _ => _.Supplier,
                                _ => _.CostCenter,
                                _ => _.QtyIssue,
                                _ => _.IsIssue,
                                _ => _.Status,
                                _ => _.IsGentani

                                );

                    for (var i = 0; i < 8; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });

        }
    }
}
