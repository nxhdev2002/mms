using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.SPP.Cost.Dto;
using prod.Storage;
using System.Collections.Generic;

namespace prod.Inventory.SPP.Cost.Exporting
{
    public class InvSppCostExcelExporter : NpoiExcelExporterBase, IInvSppCostExcelExporter
    {
        public InvSppCostExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<InvSppCostDto> InvSppCost)
        {
            return CreateExcelPackage(
                "InvSppCost.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("InvSppCost");
                    AddHeader(
                                sheet,
                                ("Part No"),
                                ("Invoice Qty"),
                                ("Receive Qty"),
                                ("Price"),
                                ("PriceVn"),
                                ("Amount"),
                                ("AmountVn"),
                                ("Stock"),
                                ("Month"),
                                ("Year")
                               );
                    AddObjects(
                         sheet, InvSppCost,
                                _ => _.PartNo,
                                _ => _.InvoiceNo,
                                _ => _.ReciveQty,
                                _ => _.Price,
                                _ => _.PriceVn,
                                _ => _.Amount,
                                _ => _.AmountVn,
                                _ => _.Stock,
                                _ => _.Month,
                                _ => _.Year
                                );
                });
        }
    }
}
