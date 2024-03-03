using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.LogA.Sps.Exporting;
using prod.LogA.Sps.Dto;
using prod.Storage;
using prod.LogA.Sps.Dto;
namespace prod.LogA.Sps.Exporting
{
    public class LgaSpsStockExcelExporter : NpoiExcelExporterBase, ILgaSpsStockExcelExporter
    {
        public LgaSpsStockExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<LgaSpsStockDto> stock)
        {
            return CreateExcelPackage(
                "LogASpsStock.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Stock");
                    AddHeader(
                                sheet,
                                ("PartNo"),
                                ("PartName"),
                                ("SupplierNo"),
                                ("BackNo"),
                                ("SpsRackAddress"),
                                ("PcRackAddress"),
                                ("RackCapBox"),
                                ("PcPicKingMember"),
                                ("EkbQty"),
                                ("StockQty"),
                                ("BoxQty"),
                                ("Process"),
                                ("IsActive")
                               );
                    AddObjects(
                         sheet, stock,
                                _ => _.PartNo,
                                _ => _.PartName,
                                _ => _.SupplierNo,
                                _ => _.BackNo,
                                _ => _.SpsRackAddress,
                                _ => _.PcRackAddress,
                                _ => _.RackCapBox,
                                _ => _.PcPicKingMember,
                                _ => _.EkbQty,
                                _ => _.StockQty,
                                _ => _.BoxQty,
                                _ => _.Process,
                                _ => _.IsActive
                                );
                });

        }
    }
}
