using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.LogA.Bar.Dto;
using prod.Storage;
namespace prod.LogA.Bar.Exporting
{
    public class LgaBarScanInfoExcelExporter : NpoiExcelExporterBase, ILgaBarScanInfoExcelExporter
    {
        public LgaBarScanInfoExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<LgaBarScanInfoDto> barscaninfo)
        {
            return CreateExcelPackage(
                "LogABarBarScanInfo.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("BarScanInfo");
                    AddHeader(
                                sheet,
                                ("UserId"),
                                ("UserName"),
                                ("ScanValue"),
                                ("ScanPartNo"),
                                ("ScanBackNo"),
                                ("ScanType"),
                                ("ScanDatetime"),
                                ("ProdLine"),
                                ("RefId"),
                                ("Status"),
                                ("IsActive")

                               );
                    AddObjects(
                         sheet, barscaninfo,
                                _ => _.UserId,
                                _ => _.UserName,
                                _ => _.ScanValue,
                                _ => _.ScanPartNo,
                                _ => _.ScanBackNo,
                                _ => _.ScanType,
                                _ => _.ScanDatetime,
                                _ => _.ProdLine,
                                _ => _.RefId,
                                _ => _.Status,
                                _ => _.IsActive

                                );
                });

        }
    }
}
