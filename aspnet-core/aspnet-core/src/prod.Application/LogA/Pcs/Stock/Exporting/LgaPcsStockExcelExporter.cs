using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.LogA.Pcs.Exporting;
using prod.LogA.Pcs.Dto;
using prod.Storage;
using prod.LogA.Pcs.Dto;
namespace prod.LogA.Pcs.Exporting
{
    public class LgaPcsStockExcelExporter : NpoiExcelExporterBase, ILgaPcsStockExcelExporter
    {
        public LgaPcsStockExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<LgaPcsStockDto> stock)
        {
            return CreateExcelPackage(
                "LogAPcsStock.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Stock");
                    AddHeader(
                                sheet,
                                ("PartNo"),
                                ("PartName"),
                                ("SupplierNo"),
                                ("BackNo"),
                                ("PcRackAddress"),
                                ("UsagePerHour"),
                                ("RackCapBox"),
                                ("OutType"),
                                ("StockQty"),
                                ("BoxQty"),
                                ("IsActive")
                               );
                    AddObjects(
                         sheet, stock,
                                _ => _.PartNo,
                                _ => _.PartName,
                                _ => _.SupplierNo,
                                _ => _.BackNo,
                                _ => _.PcRackAddress,
                                _ => _.UsagePerHour,
                                _ => _.RackCapBox,
                                _ => _.OutType,
                                _ => _.StockQty,
                                _ => _.BoxQty,
                                _ => _.IsActive
                                );
                });

        }
    }
}
